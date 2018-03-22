using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace WorldEditor.Editors.Files.D2O
{
    public class SerializePropertyOnlyResolver : DefaultContractResolver
    {
        public static readonly SerializePropertyOnlyResolver Instance = new SerializePropertyOnlyResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (member is FieldInfo)
            {
                property.Ignored = true;
            }
            return property;
        }
    }
}
