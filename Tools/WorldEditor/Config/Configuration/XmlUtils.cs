using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace WorldEditor.Config.Configuration
{
    public static class XmlUtils
    {
        private static readonly Dictionary<Type, XmlSerializer> Serializers = new Dictionary<Type, XmlSerializer>();

        public static void Serialize<T>(string fileName, T item)
        {
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                if (!XmlUtils.Serializers.ContainsKey(typeof(T)))
                {
                    XmlUtils.Serializers.Add(typeof(T), new XmlSerializer(typeof(T)));
                }
                XmlUtils.Serializers[typeof(T)].Serialize(streamWriter, item);
            }
        }

        public static void Serialize<T>(Stream stream, T item)
        {
            using (StreamWriter streamWriter = new StreamWriter(stream))
            {
                if (!XmlUtils.Serializers.ContainsKey(typeof(T)))
                {
                    XmlUtils.Serializers.Add(typeof(T), new XmlSerializer(typeof(T)));
                }
                XmlUtils.Serializers[typeof(T)].Serialize(streamWriter, item);
            }
        }

        public static void Serialize(string fileName, object item, Type valueType)
        {
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                if (!XmlUtils.Serializers.ContainsKey(valueType))
                {
                    XmlUtils.Serializers.Add(valueType, new XmlSerializer(valueType));
                }
                XmlUtils.Serializers[valueType].Serialize(streamWriter, item);
            }
        }

        public static void Serialize(Stream stream, object item, Type valueType)
        {
            using (StreamWriter streamWriter = new StreamWriter(stream))
            {
                if (!XmlUtils.Serializers.ContainsKey(valueType))
                {
                    XmlUtils.Serializers.Add(valueType, new XmlSerializer(valueType));
                }
                XmlUtils.Serializers[valueType].Serialize(streamWriter, item);
            }
        }

        public static T Deserialize<T>(string fileName)
        {
            T result;
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                if (!XmlUtils.Serializers.ContainsKey(typeof(T)))
                {
                    XmlUtils.Serializers.Add(typeof(T), new XmlSerializer(typeof(T)));
                }
                result = (T)((object)XmlUtils.Serializers[typeof(T)].Deserialize(streamReader));
            }
            return result;
        }

        public static T Deserialize<T>(Stream stream)
        {
            T result;
            using (StreamReader streamReader = new StreamReader(stream))
            {
                if (!XmlUtils.Serializers.ContainsKey(typeof(T)))
                {
                    XmlUtils.Serializers.Add(typeof(T), new XmlSerializer(typeof(T)));
                }
                result = (T)((object)XmlUtils.Serializers[typeof(T)].Deserialize(streamReader));
            }
            return result;
        }

        public static T Deserialize<T>(StringReader reader)
        {
            if (!XmlUtils.Serializers.ContainsKey(typeof(T)))
            {
                XmlUtils.Serializers.Add(typeof(T), new XmlSerializer(typeof(T)));
            }
            return (T)((object)XmlUtils.Serializers[typeof(T)].Deserialize(reader));
        }

        public static object Deserialize(string fileName, Type valueType)
        {
            object result;
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                if (!XmlUtils.Serializers.ContainsKey(valueType))
                {
                    XmlUtils.Serializers.Add(valueType, new XmlSerializer(valueType));
                }
                result = XmlUtils.Serializers[valueType].Deserialize(streamReader);
            }
            return result;
        }

        public static object Deserialize(Stream stream, Type valueType)
        {
            object result;
            using (StreamReader streamReader = new StreamReader(stream))
            {
                if (!XmlUtils.Serializers.ContainsKey(valueType))
                {
                    XmlUtils.Serializers.Add(valueType, new XmlSerializer(valueType));
                }
                result = XmlUtils.Serializers[valueType].Deserialize(streamReader);
            }
            return result;
        }
    }
}