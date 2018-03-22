using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace WorldEditor.Config.Configuration.Docs
{
    [XmlRoot("doc")]
    public class DotNetDocumentation
    {
        public class AssemblyInfo
        {
            [XmlElement("name")]
            public string Name
            {
                get;
                set;
            }
        }

        private static readonly Dictionary<char, MemberType> TypeMap;
        [XmlElement("assembly")]
        public DotNetDocumentation.AssemblyInfo Assembly
        {
            get;
            set;
        }
        [XmlArray("members"), XmlArrayItem("member")]
        public DocEntry[] Members
        {
            get;
            set;
        }

        public static DotNetDocumentation Load(string filename)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DotNetDocumentation));
            DotNetDocumentation result;
            using (XmlReader xmlReader = XmlReader.Create(filename, new XmlReaderSettings()))
            {
                result = (DotNetDocumentation)xmlSerializer.Deserialize(xmlReader);
            }
            return result;
        }

        static DotNetDocumentation()
        {
            DotNetDocumentation.TypeMap = new Dictionary<char, MemberType>();
            DotNetDocumentation.TypeMap.Add('T', MemberType.Type);
            DotNetDocumentation.TypeMap.Add('F', MemberType.Field);
            DotNetDocumentation.TypeMap.Add('P', MemberType.Property);
            DotNetDocumentation.TypeMap.Add('M', MemberType.Method);
            DotNetDocumentation.TypeMap.Add('E', MemberType.Event);
        }

        public static MemberType GetMemberType(char shortcut)
        {
            MemberType result;
            if (!DotNetDocumentation.TypeMap.TryGetValue(shortcut, out result))
            {
                throw new Exception("Undefined Type-shortcut: " + shortcut);
            }
            return result;
        }
    }
}