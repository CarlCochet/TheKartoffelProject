using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using WorldEditor.Helpers.IO;

namespace WorldEditor.Loaders.Data
{
	public class D2OWriter : IDisposable
	{
		private const int NullIdentifier = -1431655766;
		private readonly object m_writingSync = new object();
		private Dictionary<Type, int> m_allocatedClassId = new Dictionary<Type, int>();
		private Dictionary<int, D2OClassDefinition> m_classes;
		private Dictionary<int, int> m_indexTable;
		private bool m_needToBeSync;
		private Dictionary<int, object> m_objects = new Dictionary<int, object>();
		private BigEndianWriter m_writer;
		private bool m_writing;

        public string BakFilename { get; set; }

        public string Filename { get; set; }

        public Dictionary<int, object> Objects
        {
            get { return m_objects; }
        }

        public D2OWriter(string filename, bool readDefinitionsOnly = false)
        {
            Filename = filename;
            if (File.Exists(filename))
            {
                OpenWrite(readDefinitionsOnly);
            }
            else
            {
                CreateWrite(filename);
            }
        }

		public void Dispose()
		{
			if (m_writing)
			{
				EndWriting();
			}
		}

		public static void CreateEmptyFile(string path)
		{
			if (File.Exists(path))
			{
				throw new Exception("File already exists, delete before overwrite");
			}
			BinaryWriter writer = new BinaryWriter(File.OpenWrite(path));
			writer.Write("D2O");
			writer.Write((int)writer.BaseStream.Position + 4);
			writer.Write(0);
			writer.Write(0);
			writer.Flush();
			writer.Close();
		}

		private void CreateWrite(string filename)
		{
			m_indexTable = new Dictionary<int, int>();
			m_classes = new Dictionary<int, D2OClassDefinition>();
			m_objects = new Dictionary<int, object>();
			m_allocatedClassId = new Dictionary<Type, int>();
		}

		private void OpenWrite(bool readDefinitionsOnly = false)
		{
			ResetMembersByReading(readDefinitionsOnly);
		}

		private void ResetMembersByReading(bool readDefinitionsOnly = false)
		{
			D2OReader reader = new D2OReader(File.OpenRead(Filename));
			m_indexTable = (readDefinitionsOnly ? new Dictionary<int, int>() : reader.Indexes);
			m_classes = reader.Classes;
			m_allocatedClassId = m_classes.ToDictionary(entry => entry.Value.ClassType, entry => entry.Key);
			m_objects = (readDefinitionsOnly ? new Dictionary<int, object>() : reader.ReadObjects(false, false));
			reader.Close();
		}

		public void StartWriting(bool backupFile = true)
		{
			if (File.Exists(Filename))
			{
				if (backupFile)
				{
					BakFilename = Filename + ".bak";
					File.Copy(Filename, BakFilename, true);
				}
				File.Delete(Filename);
			}

			m_writer = new BigEndianWriter(File.Create(Filename));
			m_writing = true;
			lock (m_writingSync)
			{
				if (m_needToBeSync)
				{
					ResetMembersByReading(false);
				}
			}
		}

		public void EndWriting()
		{
			lock (m_writingSync)
			{
				m_writer.Seek(0, SeekOrigin.Begin);
				m_writing = false;
				m_needToBeSync = false;
				WriteHeader();
                foreach (var obj in m_objects)
                {
                    if (!m_indexTable.ContainsKey(obj.Key))
                    {
                        m_indexTable.Add(obj.Key, (int)m_writer.BaseStream.Position);
                    }
                    else
                    {
                        m_indexTable[obj.Key] = (int)m_writer.BaseStream.Position;
                    }
                    WriteObject(m_writer, obj.Value, obj.Value.GetType());
                }
                foreach(var index in m_indexTable.Keys.ToArray())
                {
                    if(!m_objects.ContainsKey(index))
                    {
                        m_indexTable.Remove(index);
                    }
                }
				WriteIndexTable();
				WriteClassesDefinition();
				m_writer.Dispose();
			}
		}

		private void WriteHeader()
		{
			m_writer.WriteUTFBytes("D2O");
			m_writer.WriteInt(0);
		}

		private void WriteIndexTable()
		{
			int offset = (int)m_writer.BaseStream.Position;
			m_writer.Seek(3, SeekOrigin.Begin);
			m_writer.WriteInt(offset);
			m_writer.Seek(offset, SeekOrigin.Begin);
			m_writer.WriteInt(m_indexTable.Count * 8);
			foreach (KeyValuePair<int, int> index in m_indexTable)
			{
				m_writer.WriteInt(index.Key);
				m_writer.WriteInt(index.Value);
			}
		}

		private void WriteClassesDefinition()
		{
			m_writer.WriteInt(m_classes.Count);
			foreach (D2OClassDefinition classDefinition in m_classes.Values)
			{
				classDefinition.Offset = (long)((int)m_writer.BaseStream.Position);
				m_writer.WriteInt(classDefinition.Id);
				m_writer.WriteUTF(classDefinition.Name);
				m_writer.WriteUTF(classDefinition.PackageName);
				m_writer.WriteInt(classDefinition.Fields.Count);
				foreach (D2OFieldDefinition field in classDefinition.Fields.Values)
				{
					field.Offset = (long)((int)m_writer.BaseStream.Position);
					m_writer.WriteUTF(field.Name);
					m_writer.WriteInt((int)field.TypeId);
					Tuple<D2OFieldType, Type>[] vectorTypes = field.VectorTypes;
					for (int i = 0; i < vectorTypes.Length; i++)
					{
						Tuple<D2OFieldType, Type> vectorType = vectorTypes[i];
						m_writer.WriteUTF(ConvertNETTypeToAS3(vectorType.Item2));
						m_writer.WriteInt((int)vectorType.Item1);
					}
				}
			}
		}

		public void Write(object obj, Type type, int index)
		{
			if (!m_writing)
			{
				StartWriting(true);
			}
			lock (m_writingSync)
			{
				m_needToBeSync = true;
				if (!IsClassDeclared(type))
				{
					DefineClassDefinition(type);
				}
				if (m_objects.ContainsKey(index))
				{
					m_objects[index] = obj;
				}
				else
				{
					m_objects.Add(index, obj);
				}
			}
		}

		public void Write(object obj, Type type)
		{
			Write(obj, type, (m_objects.Count > 0) ? (m_objects.Keys.Max() + 1) : 1);
		}

		public void Write(object obj, int index)
		{
			Write(obj, obj.GetType(), index);
		}

		public void Write(object obj)
		{
			Write(obj, obj.GetType());
		}

		public void Write<T>(T obj)
		{
			Write(obj, typeof(T));
		}

		public void Write<T>(T obj, int index)
		{
			Write(obj, typeof(T), index);
		}

		public void Delete(int index)
		{
			lock (m_writingSync)
			{
				if (m_objects.ContainsKey(index))
				{
					m_objects.Remove(index);
				}
			}
		}

		private bool IsClassDeclared(Type classType)
		{
			return m_allocatedClassId.ContainsKey(classType);
		}

		private int AllocateClassId(Type classType)
		{
			int id = (m_allocatedClassId.Count > 0) ? (m_allocatedClassId.Values.Max() + 1) : 1;
			AllocateClassId(classType, id);
			return id;
		}

		private void AllocateClassId(Type classType, int classId)
		{
			m_allocatedClassId.Add(classType, classId);
		}

		private void DefineClassDefinition(Type classType)
		{
			if (m_classes.Count(entry => entry.Value.ClassType == classType) > 0)
			{
				return;
			}
			if (!m_allocatedClassId.ContainsKey(classType))
			{
				AllocateClassId(classType);
			}
			object[] attributes = classType.GetCustomAttributes(typeof(D2OClassAttribute), false);
			if (attributes.Length != 1)
			{
				throw new Exception("The given class has no D2OClassAttribute attribute and cannot be wrote");
			}
			string package = ((D2OClassAttribute)attributes[0]).PackageName;
			string name = (!string.IsNullOrEmpty(((D2OClassAttribute)attributes[0]).Name)) ? ((D2OClassAttribute)attributes[0]).Name : classType.Name;
			List<D2OFieldDefinition> fields = new List<D2OFieldDefinition>();
            foreach (var field in classType.GetFields())
			{
				if (!field.GetCustomAttributes(typeof(D2OIgnore), false).Any() && !field.IsStatic && !field.IsPrivate && !field.IsInitOnly)
				{
					D2OFieldAttribute attr = (D2OFieldAttribute)field.GetCustomAttributes(typeof(D2OFieldAttribute), false).SingleOrDefault();
					D2OFieldType fieldType = GetIdByType(field);
					Tuple<D2OFieldType, Type>[] vectorTypes = GetVectorTypes(field.FieldType);
					string fieldName2 = (attr != null) ? attr.FieldName : field.Name;
					fields.Add(new D2OFieldDefinition(fieldName2, fieldType, field, -1L, vectorTypes));
				}
			}
			foreach(var property in classType.GetProperties())
			{
				if (!property.GetCustomAttributes(typeof(D2OIgnore), false).Any<object>() && property.CanWrite)
				{
					D2OFieldAttribute attr2 = (D2OFieldAttribute)property.GetCustomAttributes(typeof(D2OFieldAttribute), false).SingleOrDefault();
					D2OFieldType fieldType2 = GetIdByType(property);
					Tuple<D2OFieldType, Type>[] vectorTypes2 = GetVectorTypes(property.PropertyType);
					string fieldName = (attr2 != null) ? attr2.FieldName : property.Name;
					if (!fields.Any((D2OFieldDefinition x) => x.Name == fieldName))
					{
						fields.Add(new D2OFieldDefinition(fieldName, fieldType2, property, -1L, vectorTypes2));
					}
				}
			}
			m_classes.Add(m_allocatedClassId[classType], new D2OClassDefinition(m_allocatedClassId[classType], name, package, classType, fields, -1L));
			DefineAllocatedTypes();
		}

        private void DefineAllocatedTypes()
        {
            foreach (var allocatedClass in m_allocatedClassId.Where(entry => !m_classes.ContainsKey(entry.Value)).ToArray())
            {
                DefineClassDefinition(allocatedClass.Key);
            }
        }

		private D2OFieldType GetIdByType(FieldInfo field)
		{
			if (field.GetCustomAttribute<I18NFieldAttribute>() != null)
			{
				return D2OFieldType.I18N;
			}
			return GetIdByType(field.FieldType);
		}

		private D2OFieldType GetIdByType(PropertyInfo property)
		{
			if (property.GetCustomAttribute<I18NFieldAttribute>() != null)
			{
				return D2OFieldType.I18N;
			}
			return GetIdByType(property.PropertyType);
		}

		private D2OFieldType GetIdByType(Type fieldType)
		{
			if (fieldType == typeof(int))
			{
				return D2OFieldType.Int;
			}
			if (fieldType == typeof(bool))
			{
				return D2OFieldType.Bool;
			}
			if (fieldType == typeof(string))
			{
				return D2OFieldType.String;
			}
			if (fieldType == typeof(double) || fieldType == typeof(float))
			{
				return D2OFieldType.Double;
			}
			if (fieldType == typeof(uint))
			{
				return D2OFieldType.UInt;
			}
			if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
			{
				return D2OFieldType.List;
			}
            if (!m_allocatedClassId.ContainsKey(fieldType))
            {
                AllocateClassId(fieldType);
            }
			return (D2OFieldType)m_allocatedClassId[fieldType];
		}

		private Tuple<D2OFieldType, Type>[] GetVectorTypes(Type vectorType)
		{
			List<Tuple<D2OFieldType, Type>> ids = new List<Tuple<D2OFieldType, Type>>();
			if (vectorType.IsGenericType)
			{
                Type[] genericArguments = vectorType.GetGenericArguments();
				while (genericArguments.Length > 0)
				{
                    ids.Add(Tuple.Create<D2OFieldType, Type>(GetIdByType(genericArguments[0]), vectorType));
                    vectorType = genericArguments[0];
                    genericArguments = vectorType.GetGenericArguments();
				}
			}
			return ids.ToArray();
		}

		private string ConvertNETTypeToAS3(Type type)
		{
			string name;
			switch (name = type.Name)
			{
			case "Int32":
			case "Int16":
			case "UInt16":
				return "int";
			case "UInt32":
				return "uint";
			case "Int64":
			case "UInt64":
			case "Single":
			case "Double":
				return "Number";
			case "String":
				return "String";
			}
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
			{
				return "Vector.<" + ConvertNETTypeToAS3(type.GetGenericArguments()[0]) + ">";
			}
            D2OClassDefinition @class = m_classes.Values.FirstOrDefault(entry => entry.ClassType == type);
			if (@class == null)
			{
				throw new Exception(string.Format("Cannot found AS3 type associated to {0}", type));
			}
			return @class.PackageName + "::" + @class.Name;
		}

		private void WriteObject(BigEndianWriter writer, object obj, Type type)
		{
			if (!m_allocatedClassId.ContainsKey(obj.GetType()))
			{
				DefineClassDefinition(obj.GetType());
			}
			D2OClassDefinition @class = m_classes[m_allocatedClassId[type]];
			writer.WriteInt(@class.Id);
			foreach (var field in @class.Fields)
			{
				object fieldValue = field.Value.GetValue(obj);
				WriteField(writer, field.Value.TypeId, field.Value, fieldValue, 0);
			}
		}

        private void WriteField(BigEndianWriter writer, D2OFieldType fieldType, D2OFieldDefinition field, dynamic obj, int vectorDimension = 0)
        {
            if (fieldType == D2OFieldType.List)
            {
                WriteFieldVector(writer, field, obj, vectorDimension);
                return;
            }
            switch (fieldType)
            {
                case D2OFieldType.UInt:
                    if (obj.GetType() != typeof(uint))
                    {
                        D2OWriter.WriteFieldUInt(writer, (uint)obj);
                    }
                    else
                    {
                        D2OWriter.WriteFieldUInt(writer, obj);
                    }
                    break;
                case D2OFieldType.I18N:
                    if (obj.GetType() != typeof(int))
                    {
                        D2OWriter.WriteFieldI18n(writer, (int)obj);
                    }
                    else
                    {
                        D2OWriter.WriteFieldInt(writer, obj);
                    }
                    break;
                case D2OFieldType.Double:
                    D2OWriter.WriteFieldDouble(writer, obj);
                    break;
                case D2OFieldType.String:
                    D2OWriter.WriteFieldUTF(writer, obj);
                    break;
                case D2OFieldType.Bool:
                    D2OWriter.WriteFieldBool(writer, obj);
                    break;
                case D2OFieldType.Int:
                    if (obj.GetType() != typeof(int))
                    {
                        D2OWriter.WriteFieldInt(writer, (int)obj);
                    }
                    else
                    {
                        D2OWriter.WriteFieldInt(writer, obj);
                    }
                    break;
                default:
                    WriteFieldObject(writer, obj);
                    break;
            }
        }

        private void WriteFieldVector(BigEndianWriter writer, D2OFieldDefinition field, IList list, int vectorDimension)
		{
			if (list == null)
			{
				writer.WriteInt(0);
				return;
			}
			writer.WriteInt(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				WriteField(writer, field.VectorTypes[vectorDimension].Item1, field, list[i], vectorDimension + 1);
			}
		}

        private void WriteFieldObject(BigEndianWriter writer, object obj)
		{
			if (obj == null)
			{
				writer.WriteInt(-1431655766);
				return;
			}
			if (!m_allocatedClassId.ContainsKey(obj.GetType()))
			{
				DefineClassDefinition(obj.GetType());
			}
			WriteObject(writer, obj, obj.GetType());
		}

        private static void WriteFieldInt(BigEndianWriter writer, int value)
		{
			writer.WriteInt(value);
		}

        private static void WriteFieldUInt(BigEndianWriter writer, uint value)
		{
			writer.WriteUInt(value);
		}

        private static void WriteFieldBool(BigEndianWriter writer, bool value)
		{
			writer.WriteBoolean(value);
		}

        private static void WriteFieldUTF(BigEndianWriter writer, string value)
		{
			if (value == null)
			{
				value = string.Empty;
			}
			writer.WriteUTF(value);
		}

        private static void WriteFieldDouble(BigEndianWriter writer, double value)
		{
			writer.WriteDouble(value);
		}

        private static void WriteFieldI18n(BigEndianWriter writer, int value)
		{
			writer.WriteInt(value);
		}

        private static void WriteFieldI18n(BigEndianWriter writer, uint value)
        {
            writer.WriteUInt(value);
        }
	}
}
