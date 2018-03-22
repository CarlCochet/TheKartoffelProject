using Stump.ORM;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using WorldEditor.Helpers;
using WorldEditor.Loaders.Database;

namespace WorldEditor.Config
{
    public class ConfigDialogModelView : INotifyPropertyChanged
    {
        private DelegateCommand m_testDBCommand;
        public event PropertyChangedEventHandler PropertyChanged;

        private List<DbFactoryInformation> _factories;
        private DatabaseConfiguration _dbConfig;
        private LoaderSettings _loaderSettings;

        public bool IsFirstLaunch
        {
            get { return Settings.IsFirstLaunch; }
            set
            {
                if (IsFirstLaunch == value)
                {
                    return;
                }
                Settings.IsFirstLaunch = value;
                OnPropertyChanged("IsFirstLaunch");
            }
        }

        public List<DbFactoryInformation> Factories
        {
            get { return _factories; }
            set
            {
                if (_factories == value)
                {
                    return;
                }
                _factories = value;
                OnPropertyChanged("Factories");
            }
        }

        public DatabaseConfiguration DBConfig
        {
            get { return _dbConfig; }
            set
            {
                if (_dbConfig == value)
                {
                    return;
                }
                _dbConfig = value;
                OnPropertyChanged("TestDBCommand");
                OnPropertyChanged("DBConfig");
            }
        }

        public LoaderSettings LoaderSettings
        {
            get { return _loaderSettings; }
            set
            {
                if (_loaderSettings == value)
                {
                    return;
                }
                _loaderSettings = value;
                OnPropertyChanged("LoaderSettings");
            }
        }

        public DelegateCommand TestDBCommand
        {
            get
            {
                if (m_testDBCommand == null)
                {
                    m_testDBCommand = new DelegateCommand(OnTestDB, CanTestDB);
                }
                return m_testDBCommand;
            }
        }

        public ConfigDialogModelView()
        {
            Factories = new List<DbFactoryInformation>();
            foreach (DataRow row in DbProviderFactories.GetFactoryClasses().Rows)
            {
                Factories.Add(new DbFactoryInformation
                {
                    Name = row["Name"].ToString(),
                    InvariantName = row["InvariantName"].ToString()
                });
            }
        }

        private bool CanTestDB(object parameter)
        {
            return true;
        }

        private void OnTestDB(object parameter)
        {
            MessageService.ShowMessage(null, DatabaseManager.Instance.TryConnection(DBConfig) ? "Connection works" : "Connection doesn't work");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}