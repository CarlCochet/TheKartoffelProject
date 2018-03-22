using WorldEditor.Loaders.D2p;
using System;
namespace WorldEditor.Editors.Files.D2P
{
    public class D2PFolderRow : D2PGridRow
    {
        private readonly D2pDirectory m_folder;
        public override string Name
        {
            get
            {
                return Folder.Name;
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
                return "Folder";
            }
        }
        public override bool HasSize
        {
            get
            {
                return false;
            }
        }
        public override int Size
        {
            get
            {
                return 0;
            }
        }
        public override string Container
        {
            get
            {
                return string.Empty;
            }
        }
        public D2pDirectory Folder
        {
            get
            {
                return m_folder;
            }
        }
        public D2PFolderRow(D2pDirectory folder)
        {
            m_folder = folder;
        }
    }
}
