using Stump.ORM;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using WorldEditor.Config;
using WorldEditor.Helpers.Reflection;

namespace WorldEditor.Loaders.Database
{
    public class DatabaseManager : Singleton<DatabaseManager>, INotifyPropertyChanged
    {
        private readonly DatabaseAccessor m_dbAccessor = new DatabaseAccessor();
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _connected;

        public Stump.ORM.Database Database
        {
            get { return m_dbAccessor.Database; }
        }

        public bool Connected
        {
            get { return _connected; }
            private set
            {
                if (_connected == value)
                {
                    return;
                }
                _connected = value;
                OnPropertyChanged("Connected");
            }
        }

        public void Initialize(Assembly worldAssembly)
        {
            m_dbAccessor.RegisterMappingAssembly(worldAssembly);
        }

        public void Connect()
        {
            m_dbAccessor.Configuration = Settings.DatabaseConfiguration;
            m_dbAccessor.OpenConnection();
            Connected = true;
        }

        public void Disconnect()
        {
            m_dbAccessor.CloseConnection();
            Connected = false;
        }

        public bool TryConnection(DatabaseConfiguration config)
        {
            var db = new Stump.ORM.Database(config.GetConnectionString(), config.ProviderName)
            {
                KeepConnectionAlive = true
            };
            try
            {
                db.OpenSharedConnection();
            }
            catch (Exception)
            {
                return false;
            }
            db.CloseSharedConnection();
            return true;
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
