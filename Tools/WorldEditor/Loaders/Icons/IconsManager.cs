using System;
using System.Collections.Generic;
using System.Linq;
using WorldEditor.Config;
using WorldEditor.Helpers.Reflection;
using WorldEditor.Loaders.D2p;

namespace WorldEditor.Loaders.Icons
{
    public class IconsManager : Singleton<IconsManager>
    {
        private D2pFile m_d2PFile;
        private Dictionary<int, Icon> m_icons;

        public Icon ErrorIcon { get; private set; }

        public Icon EmptyIcon { get; private set; }

        public IEnumerable<Icon> Icons
        {
            get { return m_icons.Values; }
        }

        public void Initialize()
        {
            m_d2PFile = new D2pFile(Settings.LoaderSettings.ItemIconsFile);
            m_icons = EnumerateIcons().ToDictionary(entry => entry.Id);
            ErrorIcon = new Icon(-1, m_d2PFile.ReadFile("error.png"));
            EmptyIcon = new Icon(0, m_d2PFile.ReadFile("empty.png"));
        }

        public Icon GetIcon(int id)
        {
            if (id == 0)
            {
                return EmptyIcon;
            }
            else if (!m_icons.ContainsKey(id))
            {
                return ErrorIcon;
            }
            else
            {
                var data = m_d2PFile.ReadFile(id + ".png");
                return new Icon(id, data);
            }
        }

        private IEnumerable<Icon> EnumerateIcons()
        {
            foreach (var current in m_d2PFile.Entries)
            {
                if (current.FullFileName.EndsWith(".png"))
                {
                    var data = m_d2PFile.ReadFile(current);
                    var text = current.FileName.Replace(".png", "");
                    int id;
                    if (int.TryParse(text, out id) && text != "empty" && text != "error")
                    {
                        yield return new Icon(id, data);
                    }
                }
            }
        }
    }
}
