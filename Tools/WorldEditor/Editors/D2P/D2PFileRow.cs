using WorldEditor.Loaders.D2p;
using System;
using System.IO;
namespace WorldEditor.Editors.Files.D2P
{
    public class D2PFileRow : D2PGridRow
    {
        private readonly D2pEntry m_entry;
        private string m_type;
        public D2pEntry Entry
        {
            get
            {
                return m_entry;
            }
        }
        public override string Name
        {
            get
            {
                return m_entry.FileName;
            }
            set
            {
                if (string.Equals(Name, value, StringComparison.Ordinal))
                {
                    return;
                }
                OnPropertyChanged("Name");
            }
        }
        public override string Type
        {
            get
            {
                string arg_3A_0;
                if ((arg_3A_0 = m_type) == null)
                {
                    arg_3A_0 = (m_type = Path.GetExtension(m_entry.FileName).Remove(0, 1).ToUpper() + " File");
                }
                return arg_3A_0;
            }
        }
        public override bool HasSize
        {
            get
            {
                return true;
            }
        }
        public override int Size
        {
            get
            {
                return m_entry.Size;
            }
        }
        public override string Container
        {
            get
            {
                return m_entry.Container.FilePath;
            }
        }
        public D2PFileRow(D2pEntry entry)
        {
            m_entry = entry;
        }
    }
}
