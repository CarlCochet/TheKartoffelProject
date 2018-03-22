using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using WorldEditor.Helpers.IO;
using WorldEditor.Loaders.Classes;
using WorldEditor.Helpers.Reflection;

namespace WorldEditor.Loaders.Data
{
    public class D2OReader
    {
        private const int NullIdentifier = -1431655766;
        public static List<Assembly> ClassesContainers = new List<Assembly> { typeof(Breed).Assembly };
        private static readonly Dictionary<Type, Func<object[], object>> objectCreators = new Dictionary<Type, Func<object[], object>>();
        private readonly string m_filePath;
        private int m_classcount;
        private Dictionary<int, D2OClassDefinition> m_classes;
        private int m_headeroffset;
        private Dictionary<int, int> m_indextable;
        private int m_indextablelen;
        private BigEndianReader m_reader;
        private int m_contentOffset;

        public string FilePath
        {
            get { return m_filePath; }
        }

        public string FileName
        {
            get { return Path.GetFileNameWithoutExtension(FilePath); }
        }

        public int IndexCount
        {
            get { return m_indextable.Count; }
        }

        public int IndexTableOffset
        {
            get { return m_headeroffset; }
        }

        public Dictionary<int, D2OClassDefinition> Classes
        {
            get { return m_classes; }
        }

        public Dictionary<int, int> Indexes
        {
            get { return m_indextable; }
        }

        public D2OReader(string filePath)
            : this(new BigEndianReader(File.OpenRead(filePath)))
        {
            m_filePath = filePath;
        }

        public D2OReader(Stream stream)
        {
            m_indextable = new Dictionary<int, int>();
            m_reader = new BigEndianReader(stream);
            Initialize();
        }

        public D2OReader(BigEndianReader reader)
        {
            m_indextable = new Dictionary<int, int>();
            m_reader = reader;
            Initialize();
        }

        private void Initialize()
        {
            lock (m_reader)
            {
                ReadHeader();
                ReadIndexTable(false);
                ReadClassesTable();
            }
        }

        private void ReadHeader()
        {
            string header = m_reader.ReadUTFBytes(3);
            if (header != "D2O")
            {
                m_reader.Seek(0, SeekOrigin.Begin);
                try
                {
                    header = m_reader.ReadUTF();
                }
                catch
                {
                    throw new Exception("Header doesn't equal the string 'D2O' OR 'AKSF' : Corrupted file");
                }
                if (!(header == "AKSF"))
                {
                    throw new Exception("Header doesn't equal the string 'D2O' OR 'AKSF' : Corrupted file");
                }
                m_reader.ReadShort();
                int len = m_reader.ReadInt();
                m_reader.Seek(len, SeekOrigin.Current);
                m_contentOffset = (int)m_reader.Position;
                header = m_reader.ReadUTFBytes(3);
                if (header != "D2O")
                {
                    throw new Exception("Header doesn't equal the string 'D2O' : Corrupted file (signed file)");
                }
            }
        }

        private void ReadIndexTable(bool isD2OS = false)
        {
            m_headeroffset = m_reader.ReadInt();
            m_reader.Seek(m_contentOffset + m_headeroffset, SeekOrigin.Begin);
            m_indextablelen = m_reader.ReadInt();
            m_indextable = new Dictionary<int, int>(m_indextablelen / 8);
            for (int i = 0; i < m_indextablelen; i += 8)
            {
                m_indextable.Add(m_reader.ReadInt(), m_reader.ReadInt());
            }
        }

        private void ReadClassesTable()
        {
            Dictionary<D2OFieldDefinition, List<Tuple<D2OFieldType, string>>> tempVectorTypes = new Dictionary<D2OFieldDefinition, List<Tuple<D2OFieldType, string>>>();
            m_classcount = m_reader.ReadInt();
            m_classes = new Dictionary<int, D2OClassDefinition>(m_classcount);
            for (int i = 0; i < m_classcount; i++)
            {
                int classId = m_reader.ReadInt();
                string classMembername = m_reader.ReadUTF();
                string classPackagename = m_reader.ReadUTF();
                Type classType = D2OReader.FindType(classMembername);
                int fieldscount = m_reader.ReadInt();
                List<D2OFieldDefinition> fields = new List<D2OFieldDefinition>(fieldscount);
                for (int j = 0; j < fieldscount; j++)
                {
                    string fieldname = m_reader.ReadUTF();
                    D2OFieldType fieldtype = (D2OFieldType)m_reader.ReadInt();
                    FieldInfo field = classType.GetField(fieldname);
                    if (field == null)
                    {
                        //continue;
                        throw new Exception(string.Format("Missing field '{0}' ({1}) in class '{2}'", fieldname, fieldtype, classType.Name));
                    }
                    D2OFieldDefinition fieldDefinition = new D2OFieldDefinition(fieldname, fieldtype, field, m_reader.Position, new Tuple<D2OFieldType, Type>[0]);
                    new List<Tuple<D2OFieldType, object>>();
                    if (fieldtype == D2OFieldType.List)
                    {
                        D2OFieldType id;
                        do
                        {
                            string name = m_reader.ReadUTF();
                            id = (D2OFieldType)m_reader.ReadInt();
                            if (!tempVectorTypes.ContainsKey(fieldDefinition))
                            {
                                tempVectorTypes.Add(fieldDefinition, new List<Tuple<D2OFieldType, string>>());
                            }
                            tempVectorTypes[fieldDefinition].Add(Tuple.Create<D2OFieldType, string>(id, name));
                        }
                        while (id == D2OFieldType.List);
                    }
                    fields.Add(fieldDefinition);
                }
                m_classes.Add(classId, new D2OClassDefinition(classId, classMembername, classPackagename, classType, fields, m_reader.Position));
            }
            foreach (KeyValuePair<D2OFieldDefinition, List<Tuple<D2OFieldType, string>>> keyPair in tempVectorTypes)
            {
                keyPair.Key.VectorTypes = (
                    from tuple in keyPair.Value
                    select Tuple.Create<D2OFieldType, Type>(tuple.Item1, FindNETType(tuple.Item2))).ToArray<Tuple<D2OFieldType, Type>>();
            }
        }

        private Type FindNETType(string typeName)
        {
            string typeName2;
            if ((typeName2 = typeName) != null)
            {
                if (typeName2 == "int")
                {
                    return typeof(int);
                }
                if (typeName2 == "uint")
                {
                    return typeof(uint);
                }
                if (typeName2 == "Number")
                {
                    return typeof(double);
                }
                if (typeName2 == "String")
                {
                    return typeof(string);
                }
            }
            if (typeName.StartsWith("Vector.<"))
            {
                return typeof(List<>).MakeGenericType(new Type[]
				{
					FindNETType(typeName.Remove(typeName.Length - 1, 1).Remove(0, "Vector.<".Length))
				});
            }
            D2OClassDefinition @class = m_classes.Values.FirstOrDefault((D2OClassDefinition x) => x.PackageName + "::" + x.Name == typeName);
            if (@class == null)
            {
                throw new Exception(string.Format("Cannot found .NET type associated to {0}", typeName));
            }
            return @class.ClassType;
        }

        private static Type FindType(string className)
        {
            IEnumerable<Type> correspondantTypes =
                from asm in D2OReader.ClassesContainers
                let types = asm.GetTypes()
                from type in types
                where type.Name.Equals(className, StringComparison.InvariantCultureIgnoreCase) && type.HasInterface(typeof(IDataObject))
                select type;
            return correspondantTypes.Single<Type>();
        }

        private bool IsTypeDefined(Type type)
        {
            return m_classes.Count(entry => entry.Value.ClassType == type) > 0;
        }

        public Dictionary<int, T> ReadObjects<T>(bool allownulled = false) where T : class
        {
            if (!IsTypeDefined(typeof(T)))
            {
                throw new Exception("The file doesn't contain this class");
            }
            Dictionary<int, T> result = new Dictionary<int, T>(m_indextable.Count);
            using (BigEndianReader reader = CloneReader())
            {
                foreach (KeyValuePair<int, int> index in m_indextable)
                {
                    reader.Seek(index.Value, SeekOrigin.Begin);
                    int classid = reader.ReadInt();
                    if (!(m_classes[classid].ClassType == typeof(T)))
                    {
                        if (!m_classes[classid].ClassType.IsSubclassOf(typeof(T)))
                        {
                            continue;
                        }
                    }
                    try
                    {
                        result.Add(index.Key, BuildObject(m_classes[classid], reader) as T);
                    }
                    catch
                    {
                        if (!allownulled)
                        {
                            throw;
                        }
                        result.Add(index.Key, default(T));
                    }
                }
            }
            return result;
        }

        public Dictionary<int, object> ReadObjects(bool allownulled = false, bool cloneReader = false)
        {
            Dictionary<int, object> result = new Dictionary<int, object>(m_indextable.Count);
            BigEndianReader reader = cloneReader ? CloneReader() : m_reader;
            foreach (KeyValuePair<int, int> index in m_indextable)
            {
                reader.Seek(index.Value + m_contentOffset, SeekOrigin.Begin);
                try
                {
                    result.Add(index.Key, ReadObject(index.Key, reader));
                }
                catch
                {
                    if (!allownulled)
                    {
                        throw;
                    }
                    result.Add(index.Key, null);
                }
            }
            if (cloneReader)
            {
                reader.Dispose();
            }
            return result;
        }

        public IEnumerable<object> EnumerateObjects(bool cloneReader = false)
        {
            BigEndianReader dataReader = cloneReader ? CloneReader() : m_reader;
            foreach (KeyValuePair<int, int> current in m_indextable)
            {
                BigEndianReader arg_9C_0 = dataReader;
                KeyValuePair<int, int> keyValuePair = current;
                arg_9C_0.Seek(keyValuePair.Value + m_contentOffset, SeekOrigin.Begin);
                KeyValuePair<int, int> keyValuePair2 = current;
                yield return ReadObject(keyValuePair2.Key, dataReader);
            }
            if (cloneReader)
            {
                dataReader.Dispose();
            }
            yield break;
        }

        public object ReadObject(int index, bool cloneReader = false)
        {
            object result;
            if (cloneReader)
            {
                using (BigEndianReader reader = CloneReader())
                {
                    result = ReadObject(index, reader);
                    return result;
                }
            }
            lock (m_reader)
            {
                result = ReadObject(index, m_reader);
            }
            return result;
        }

        private object ReadObject(int index, BigEndianReader reader)
        {
            int offset = m_indextable[index];
            reader.Seek(offset + m_contentOffset, SeekOrigin.Begin);
            int classid = reader.ReadInt();
            return BuildObject(m_classes[classid], reader);
        }

        private object BuildObject(D2OClassDefinition classDefinition, BigEndianReader reader)
        {
            if (!D2OReader.objectCreators.ContainsKey(classDefinition.ClassType))
            {
                Func<object[], object> creator = D2OReader.CreateObjectBuilder(classDefinition.ClassType, (
                    from entry in classDefinition.Fields
                    select entry.Value.FieldInfo).ToArray<FieldInfo>());
                D2OReader.objectCreators.Add(classDefinition.ClassType, creator);
            }
            List<object> values = new List<object>();
            foreach (D2OFieldDefinition field in classDefinition.Fields.Values)
            {
                object fieldValue = ReadField(reader, field, field.TypeId, 0);
                if (fieldValue != null && !field.FieldType.IsInstanceOfType(fieldValue))
                {
                    if (fieldValue is IConvertible && field.FieldType.GetInterface("IConvertible") != null)
                    {
                        try
                        {
                            if (fieldValue is int && (int)fieldValue < 0 && field.FieldType == typeof(uint))
                            {
                                values.Add((uint)((int)fieldValue));
                            }
                            else
                            {
                                values.Add(Convert.ChangeType(fieldValue, field.FieldType));
                            }
                            continue;
                        }
                        catch
                        {
                            throw new Exception(string.Format("Field '{0}.{1}' with value {2} is not of type '{3}'", new object[]
							{
								classDefinition.Name,
								field.Name,
								fieldValue,
								fieldValue.GetType()
							}));
                        }
                    }
                    throw new Exception(string.Format("Field '{0}.{1}' with value {2} is not of type '{3}'", new object[]
					{
						classDefinition.Name,
						field.Name,
						fieldValue,
						fieldValue.GetType()
					}));
                }
                values.Add(fieldValue);
            }
            return D2OReader.objectCreators[classDefinition.ClassType](values.ToArray());
        }

        public T ReadObject<T>(int index, bool cloneReader = false) where T : class
        {
            if (cloneReader)
            {
                using (BigEndianReader reader = CloneReader())
                {
                    return ReadObject<T>(index, reader);
                }
            }
            return ReadObject<T>(index, m_reader);
        }

        private T ReadObject<T>(int index, BigEndianReader reader) where T : class
        {
            if (!IsTypeDefined(typeof(T)))
            {
                throw new Exception("The file doesn't contain this class");
            }
            int offset = 0;
            if (!m_indextable.TryGetValue(index, out offset))
            {
                throw new Exception(string.Format("Can't find Index {0} in {1}", index, FileName));
            }
            reader.Seek(offset + m_contentOffset, SeekOrigin.Begin);
            int classid = reader.ReadInt();
            if (m_classes[classid].ClassType != typeof(T) && !m_classes[classid].ClassType.IsSubclassOf(typeof(T)))
            {
                throw new Exception(string.Format("Wrong type, try to read object with {1} instead of {0}", typeof(T).Name, m_classes[classid].ClassType.Name));
            }
            return BuildObject(m_classes[classid], reader) as T;
        }

        public int FindFreeId()
        {
            return m_indextable.Keys.Max() + 1;
        }

        public Dictionary<int, D2OClassDefinition> GetObjectsClasses()
        {
            return m_indextable.ToDictionary((KeyValuePair<int, int> index) => index.Key, (KeyValuePair<int, int> index) => GetObjectClass(index.Key));
        }

        public D2OClassDefinition GetObjectClass(int index)
        {
            D2OClassDefinition result;
            lock (m_reader)
            {
                int offset = m_indextable[index];
                m_reader.Seek(offset + m_contentOffset, SeekOrigin.Begin);
                int classid = m_reader.ReadInt();
                result = m_classes[classid];
            }
            return result;
        }

        private object ReadField(BigEndianReader reader, D2OFieldDefinition field, D2OFieldType typeId, int vectorDimension = 0)
        {
            if (typeId == D2OFieldType.List)
            {
                return ReadFieldVector(reader, field, vectorDimension);
            }
            switch (typeId)
            {
                case D2OFieldType.UInt:
                    return D2OReader.ReadFieldUInt(reader);
                case D2OFieldType.I18N:
                    return D2OReader.ReadFieldI18n(reader);
                case D2OFieldType.Double:
                    return D2OReader.ReadFieldDouble(reader);
                case D2OFieldType.String:
                    return D2OReader.ReadFieldUTF(reader);
                case D2OFieldType.Bool:
                    return D2OReader.ReadFieldBool(reader);
                case D2OFieldType.Int:
                    return D2OReader.ReadFieldInt(reader);
                default:
                    return ReadFieldObject(reader);
            }
        }

        private object ReadFieldVector(BigEndianReader reader, D2OFieldDefinition field, int vectorDimension)
        {
            int count = reader.ReadInt();
            Type vectorType = field.FieldType;
            for (int i = 0; i < vectorDimension; i++)
            {
                vectorType = vectorType.GetGenericArguments()[0];
            }
            if (!D2OReader.objectCreators.ContainsKey(vectorType))
            {
                Func<object[], object> creator = D2OReader.CreateObjectBuilder(vectorType, new FieldInfo[0]);
                D2OReader.objectCreators.Add(vectorType, creator);
            }
            IList result = D2OReader.objectCreators[vectorType](new object[0]) as IList;
            for (int j = 0; j < count; j++)
            {
                vectorDimension++;
                result.Add(ReadField(reader, field, field.VectorTypes[vectorDimension - 1].Item1, vectorDimension));
                vectorDimension--;
            }
            return result;
        }

        private object ReadFieldObject(BigEndianReader reader)
        {
            int classid = reader.ReadInt();
            if (classid == -1431655766)
            {
                return null;
            }
            if (Classes.Keys.Contains(classid))
            {
                return BuildObject(Classes[classid], reader);
            }
            return null;
        }

        private static int ReadFieldInt(BigEndianReader reader)
        {
            return reader.ReadInt();
        }

        private static uint ReadFieldUInt(BigEndianReader reader)
        {
            return reader.ReadUInt();
        }

        private static bool ReadFieldBool(BigEndianReader reader)
        {
            return reader.ReadByte() != 0;
        }

        private static string ReadFieldUTF(BigEndianReader reader)
        {
            return reader.ReadUTF();
        }

        private static double ReadFieldDouble(BigEndianReader reader)
        {
            return reader.ReadDouble();
        }

        private static int ReadFieldI18n(BigEndianReader reader)
        {
            return reader.ReadInt();
        }

        internal BigEndianReader CloneReader()
        {
            lock (m_reader)
            {
                if (m_reader.Position > 0L)
                {
                    m_reader.Seek(0, SeekOrigin.Begin);
                }
                else
                {
                    Stream streamClone = new MemoryStream();
                    ((BigEndianReader)m_reader).BaseStream.CopyTo(streamClone);
                    return new BigEndianReader(streamClone);
                }
            }
            return null;
        }

        public void Close()
        {
            lock (m_reader)
            {
                m_reader.Dispose();
            }
        }

        private static Func<object[], object> CreateObjectBuilder(Type classType, params FieldInfo[] fields)
        {
            DynamicMethod method = new DynamicMethod(Guid.NewGuid().ToString("N"), typeof(object), new Type[]
			{
				typeof(object[])
			}.ToArray<Type>());
            ILGenerator ilGenerator = method.GetILGenerator();
            ilGenerator.DeclareLocal(classType);
            ilGenerator.DeclareLocal(classType);
            ilGenerator.Emit(OpCodes.Newobj, classType.GetConstructor(Type.EmptyTypes));
            ilGenerator.Emit(OpCodes.Stloc_0);
            for (int i = 0; i < fields.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldloc_0);
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldc_I4, i);
                ilGenerator.Emit(OpCodes.Ldelem_Ref);
                if (fields[i].FieldType.IsClass)
                {
                    ilGenerator.Emit(OpCodes.Castclass, fields[i].FieldType);
                }
                else
                {
                    ilGenerator.Emit(OpCodes.Unbox_Any, fields[i].FieldType);
                }
                ilGenerator.Emit(OpCodes.Stfld, fields[i]);
            }
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Stloc_1);
            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Ret);
            return (Func<object[], object>)method.CreateDelegate(Expression.GetFuncType(new Type[]
			{
				typeof(object[]),
				typeof(object)
			}.ToArray<Type>()));
        }
    }
}