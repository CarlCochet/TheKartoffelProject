using System;
using System.Reflection;
namespace WorldEditor.Loaders.Data
{
    public class D2OFieldDefinition
    {
        public string Name { get; set; }

        public D2OFieldType TypeId { get; set; }

        public Tuple<D2OFieldType, Type>[] VectorTypes { get; set; }

        internal long Offset { get; set; }

        public Type FieldType
        {
            get
            {
                if (PropertyInfo == null)
                {
                    return FieldInfo.FieldType;
                }
                return PropertyInfo.PropertyType;
            }
        }

        internal PropertyInfo PropertyInfo { get; set; }

        internal FieldInfo FieldInfo { get; set; }

        public D2OFieldDefinition(string name, D2OFieldType typeId, FieldInfo fieldInfo, long offset, params Tuple<D2OFieldType, Type>[] vectorsTypes)
        {
            Name = name;
            TypeId = typeId;
            FieldInfo = fieldInfo;
            Offset = offset;
            VectorTypes = vectorsTypes;
        }

        public D2OFieldDefinition(string name, D2OFieldType typeId, PropertyInfo propertyInfo, long offset, params Tuple<D2OFieldType, Type>[] vectorsTypes)
        {
            Name = name;
            TypeId = typeId;
            PropertyInfo = propertyInfo;
            Offset = offset;
            VectorTypes = vectorsTypes;
        }

        public object GetValue(object instance)
        {
            if (this.PropertyInfo != null)
            {
                return PropertyInfo.GetValue(instance, null);
            }
            if (this.FieldInfo != null)
            {
                return FieldInfo.GetValue(instance);
            }
            throw new NullReferenceException();
        }

        public void SetValue(object instance, object value)
        {
            if (PropertyInfo != null)
            {
                PropertyInfo.SetValue(instance, value, null);
                return;
            }
            if (FieldInfo != null)
            {
                FieldInfo.SetValue(instance, value);
            }
        }
    }
}
