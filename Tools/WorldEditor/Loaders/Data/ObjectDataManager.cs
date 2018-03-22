using NLog;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WorldEditor.Config;
using WorldEditor.Helpers.Reflection;
using WorldEditor.Loaders.Database;
using WorldEditor.Loaders.Objetcs;

namespace WorldEditor.Loaders.Data
{
    public class ObjectDataManager : Singleton<ObjectDataManager>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<Type, D2OTable> m_tables = new Dictionary<Type, D2OTable>();

        public IEnumerable<D2OTable> Tables
        {
            get { return m_tables.Values; }
        }

        public void Initialize()
        {
            foreach (var type in typeof(D2OTable).Assembly.GetTypes())
            {
                var attr = type.GetCustomAttribute<D2OClassAttribute>();
                var tableAttr = type.GetCustomAttribute<TableNameAttribute>();
                if (tableAttr != null && attr != null)
                {
                    var table = new D2OTable
                    {
                        Type = type,
                        ClassName = attr.Name,
                        TableName = tableAttr.TableName,
                    };
                    m_tables.Add(table.Type, table);
                }
            }
        }

        public T Get<T>(uint key) where T : class
        {
            return Get<T>((int)key);
        }

        public T Get<T>(int key) where T : class
        {
             AssertType<T>();
             return DatabaseManager.Instance.Database.Single<T>(key);
        }

        public T GetOrDefault<T>(uint key) where T : class
        {
            return GetOrDefault<T>((int)key);
        }

        public T GetOrDefault<T>(int key) where T : class
        {
            try
            {
                return Get<T>(key);
            }
            catch
            {
                return default(T);
            }
        }

        public void Insert<T>(T value) where T : class, IObject
        {
            AssertType<T>();
            DatabaseManager.Instance.Database.Insert(value);

            var table = m_tables[typeof(T)];
            var writer = new D2OWriter(Path.Combine(Settings.LoaderSettings.D2ODirectory, string.Format("{0}s.d2o", table.ClassName)));

            writer.StartWriting(false);

            writer.Write(value.GetD2oClass(null), value.Id);

            writer.EndWriting();
        }

        public void Update<T>(T value) where T : class, IObject
        {
            AssertType<T>();
            DatabaseManager.Instance.Database.Update(value);

            var table = m_tables[typeof(T)];
            var writer = new D2OWriter(Path.Combine(Settings.LoaderSettings.D2ODirectory, string.Format("{0}s.d2o", table.ClassName)));

            writer.StartWriting(false);

            var oldValue = writer.Objects.FirstOrDefault(entry => entry.Key == value.Id);

            writer.Delete(value.Id);
            writer.Write(value.GetD2oClass(oldValue), value.Id);

            writer.EndWriting();
        }

        public void Delete<T>(T value) where T : class, IObject
        {
            AssertType<T>();
            DatabaseManager.Instance.Database.Delete(value);

            var table = m_tables[typeof(T)];
            var writer = new D2OWriter(Path.Combine(Settings.LoaderSettings.D2ODirectory, string.Format("{0}s.d2o", table.ClassName)));

            writer.StartWriting(false);

            writer.Delete(value.Id);

            writer.EndWriting();
        }

        public int FindFreeId<T>()
        {
            AssertType<T>();
            var table = m_tables[typeof(T)];
            int maxId = DatabaseManager.Instance.Database.ExecuteScalar<int>("SELECT MAX(Id) FROM " + table.TableName) + 1;
            return (maxId < Settings.MinDataId) ? Settings.MinDataId : maxId;
        }

        public IEnumerable<T> EnumerateObjects<T>() where T : class
        {
            AssertType<T>();
            var table = m_tables[typeof(T)];
            return DatabaseManager.Instance.Database.Query<T>("SELECT * FROM " + table.TableName);
        }

        public IEnumerable<T> Query<T>(string sql, params object[] args) 
        {
            return DatabaseManager.Instance.Database.Query<T>(sql, args);
        }

        private void AssertType<T>()
        {
            if (!m_tables.ContainsKey(typeof(T)))
            {
                throw new ArgumentException("Cannot find table corresponding to type : " + typeof(T));
            }
        }
    }
}
