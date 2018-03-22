using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using WorldEditor.Loaders.I18N;
using WorldEditor.Helpers.Extentions;

namespace WorldEditor.Config
{
    [Serializable]
    public class LoaderSettings : INotifyPropertyChanged
    {
        public static readonly Dictionary<string, Languages> _stringToLang = new Dictionary<string, Languages>
        {
            { "fr", Languages.French },
            { "en", Languages.English },
            { "es", Languages.Spanish },
            { "de", Languages.German },
            { "it", Languages.Italian },
            { "ja", Languages.Japanish },
            { "nl", Languages.Dutsh },
            { "pt", Languages.Portugese },
            { "ru", Languages.Russish }
        };

        public event PropertyChangedEventHandler PropertyChanged;

        private string _basePath;
        private string _mapsRelativeFile;
        private string _d2oRelativeDirectory;
        private string _d2iRelativeDirectory;
        private string _itemIconsRelativeFile;
        private string _worldGfxRelativeFile;
        private string _worldEleRelativeFile;
        private string _genericMapDecryptionKey;
        //private Dictionary<Languages, string> _d2iFiles;

        public string BasePath
        {
            get { return _basePath; }
            set
            {
                if (string.Equals(_basePath, value, StringComparison.Ordinal))
                {
                    return;
                }
                _basePath = value;
                OnPropertyChanged("MapsFile");
                OnPropertyChanged("D2ODirectory");
                OnPropertyChanged("D2IDirectory");
                OnPropertyChanged("ItemIconsFile");
                OnPropertyChanged("WorldGfxFile");
                OnPropertyChanged("WorldEleFile");
                OnPropertyChanged("BasePath");
            }
        }

        public string MapsRelativeFile
        {
            get { return _mapsRelativeFile; }
            set
            {
                if (string.Equals(_mapsRelativeFile, value, StringComparison.Ordinal))
                {
                    return;
                }
                _mapsRelativeFile = value;
                OnPropertyChanged("MapsFile");
                OnPropertyChanged("MapsRelativeFile");
            }
        }

        public string D2ORelativeDirectory
        {
            get { return _d2oRelativeDirectory; }
            set
            {
                if (string.Equals(_d2oRelativeDirectory, value, StringComparison.Ordinal))
                {
                    return;
                }
                _d2oRelativeDirectory = value;
                OnPropertyChanged("D2ODirectory");
                OnPropertyChanged("D2ORelativeDirectory");
            }
        }

        public string D2IRelativeDirectory
        {
            get { return _d2iRelativeDirectory; }
            set
            {
                if (string.Equals(_d2iRelativeDirectory, value, StringComparison.Ordinal))
                {
                    return;
                }
                _d2iRelativeDirectory = value;
                OnPropertyChanged("D2IDirectory");
                OnPropertyChanged("D2IRelativeDirectory");
            }
        }

        public string ItemIconsRelativeFile
        {
            get { return _itemIconsRelativeFile; }
            set
            {
                if (string.Equals(_itemIconsRelativeFile, value, StringComparison.Ordinal))
                {
                    return;
                }
                _itemIconsRelativeFile = value;
                OnPropertyChanged("ItemIconsFile");
                OnPropertyChanged("ItemIconsRelativeFile");
            }
        }

        public string WorldGfxRelativeFile
        {
            get { return _worldGfxRelativeFile; }
            set
            {
                if (string.Equals(_worldGfxRelativeFile, value, StringComparison.Ordinal))
                {
                    return;
                }
                _worldGfxRelativeFile = value;
                OnPropertyChanged("WorldGfxFile");
                OnPropertyChanged("WorldGfxRelativeFile");
            }
        }

        public string WorldEleRelativeFile
        {
            get { return _worldEleRelativeFile; }
            set
            {
                if (string.Equals(_worldEleRelativeFile, value, StringComparison.Ordinal))
                {
                    return;
                }
                _worldEleRelativeFile = value;
                OnPropertyChanged("WorldEleFile");
                OnPropertyChanged("WorldEleRelativeFile");
            }
        }

        public string GenericMapDecryptionKey
        {
            get { return _genericMapDecryptionKey; }
            set
            {
                if (string.Equals(_genericMapDecryptionKey, value, StringComparison.Ordinal))
                {
                    return;
                }
                _genericMapDecryptionKey = value;
                OnPropertyChanged("GenericMapDecryptionKey");
            }
        }

        public string MapsFile
        {
            get { return Path.Combine(BasePath, MapsRelativeFile); }
        }

        public string D2ODirectory
        {
            get { return Path.Combine(BasePath, D2ORelativeDirectory); }
        }

        public string D2IDirectory
        {
            get { return Path.Combine(BasePath, D2IRelativeDirectory); }
        }

      /*  public string MainD2IFile
        {
            get { return _d2iFiles.GetOrDefault(Settings.DefaultLanguage); }
        }*/

        public string ItemIconsFile
        {
            get { return Path.Combine(BasePath, ItemIconsRelativeFile); }
        }

        public string WorldGfxFile
        {
            get { return Path.Combine(BasePath, WorldGfxRelativeFile); }
        }

        public string WorldEleFile
        {
            get { return Path.Combine(BasePath, WorldEleRelativeFile); }
        }

       /* public void Initialize()
        {
            _d2iFiles = new Dictionary<Languages, string>();
            foreach (string rawFile in Directory.EnumerateFiles(D2IDirectory, "*.d2i"))
            {
                var match = Regex.Match(Path.GetFileName(rawFile), "i18n_(\\w+)\\.d2i");
                _d2iFiles.Add(_stringToLang.GetOrDefault(match.Groups[1].Value), rawFile);
            }
        }
        */
        public LoaderSettings Clone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0L;
            return (LoaderSettings)formatter.Deserialize(stream);
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}