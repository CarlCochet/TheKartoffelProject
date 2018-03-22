using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace WorldEditor.Config.Configuration.Docs
{
    public class DocEntry
    {
        private string m_fullName;
        private string m_name;
        private MemberType m_type;
        public string Name
        {
            get
            {
                return this.m_name;
            }
        }
        public MemberType MemberType
        {
            get
            {
                return this.m_type;
            }
        }
        [XmlAttribute("name")]
        public string FullName
        {
            get
            {
                return this.m_fullName;
            }
            set
            {
                this.m_fullName = value;
                this.m_type = DotNetDocumentation.GetMemberType(this.m_fullName[0]);
                int num = this.m_fullName.IndexOf('(');
                if (num < 0)
                {
                    num = this.m_fullName.Length;
                }
                this.m_name = this.m_fullName.Substring(2, num - 2);
            }
        }
        [XmlElement("summary")]
        public object[] SummaryObjects
        {
            get;
            set;
        }
        public string Summary
        {
            get
            {
                return string.Join(" ",
                    from entry in this.SummaryObjects.Cast<XmlNode[]>().First<XmlNode[]>()
                    select entry.Value).Trim();
            }
        }
    }
}