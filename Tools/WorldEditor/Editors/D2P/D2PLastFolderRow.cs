using WorldEditor.Loaders.D2p;
using System;
namespace WorldEditor.Editors.Files.D2P
{
    public class D2PLastFolderRow : D2PFolderRow
    {
        public override string Name
        {
            get
            {
                return "..";
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
        public D2PLastFolderRow(D2pDirectory folder)
            : base(folder)
        {
        }
    }
}
