using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace WorldEditor.Helpers.IO
{
    [ProtoContract]
    public class ProtobufArray<T>
    {
        [ProtoMember(1)]
        public T[] MyArray;

        public ProtobufArray()
        { }

        public ProtobufArray(T[] array)
        {
            MyArray = array;
        }
    }

    public static class FormatterExtensions
    {
        public static byte[] ToBinary(this object obj)
        {
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Serializer.Serialize(memoryStream, obj);
                result = memoryStream.ToArray();
            }
            return result;
        }

        public static T ToObject<T>(this byte[] bytes)
        {
            T result;
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                result = Serializer.Deserialize<T>(memoryStream);
            }
            return result;
        }

        public static string ToCSV(this IEnumerable enumerable, string separator)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num = 0;
            foreach (object current in enumerable)
            {
                stringBuilder.Append(current);
                stringBuilder.Append(separator);
                num++;
            }
            if (num > 0)
            {
                stringBuilder.Remove(stringBuilder.Length - separator.Length, separator.Length);
            }
            return stringBuilder.ToString();
        }

        public static string ToCSV<TRsult, TEnum>(this IEnumerable<TEnum> enumerable, string separator, Func<TEnum, TRsult> formatter)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num = 0;
            foreach (var current in enumerable)
            {
                stringBuilder.Append(formatter(current));
                stringBuilder.Append(separator);
                num++;
            }
            if (num > 0)
            {
                stringBuilder.Remove(stringBuilder.Length - separator.Length, separator.Length);
            }
            return stringBuilder.ToString();
        }

        public static T[] FromCSV<T>(this string csvValue, string separator) where T : IConvertible
        {
            List<T> list = new List<T>();
            int num = 0;
            int num2 = csvValue.IndexOf(separator, StringComparison.Ordinal);
            while (num2 >= 0 && num2 < csvValue.Length)
            {
                list.Add((T)((object)Convert.ChangeType(csvValue.Substring(num, num2 - num), typeof(T))));
                num = num2 + separator.Length;
                num2 = csvValue.IndexOf(separator, num, StringComparison.Ordinal);
            }
            if (!string.IsNullOrEmpty(csvValue))
            {
                list.Add((T)((object)Convert.ChangeType(csvValue.Substring(num, csvValue.Length - num), typeof(T))));
            }
            return list.ToArray();
        }

        public static TResult[] FromCSV<TResult, TValue>(this string csvValue, string separator, Func<TValue, TResult> converter) where TValue : IConvertible
        {
            TValue[] tempValues = csvValue.FromCSV<TValue>(separator);
            List<TResult> result = new List<TResult>(tempValues.Length);
            foreach(var tempValue in tempValues)
            {
                result.Add(converter(tempValue));
            }
            return result.ToArray();
        }
    }
}