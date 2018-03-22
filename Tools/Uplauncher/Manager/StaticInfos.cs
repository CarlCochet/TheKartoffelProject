using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uplauncher.Manager
{
    public class StaticInfos : DataManager<StaticInfos>
    {
        private Dictionary<string, object> m_infos = new Dictionary<string, object>();
        public const string Version = "0.4";
        public CancellationToken Cancel;

        public void UpdateInfo<T>(string key, T info) where T : class
        {
            if (m_infos.ContainsKey(key))
                m_infos[key] = info;
            else
                m_infos.Add(key, info);
        }

        public T GetInfo<T>(string key) where T : class
        {
            if (m_infos.ContainsKey(key))
                return m_infos[key] as T;
            return default(T);
        }

        public void UpdateValue<T>(string key, T info) where T : IConvertible
        {
            if (m_infos.ContainsKey(key))
                m_infos[key] = info;
            else
                m_infos.Add(key, info);
        }

        public T GetValue<T>(string key) where T : IConvertible
        {
            if (m_infos.ContainsKey(key))
                return (T)Convert.ChangeType(m_infos[key], typeof(T));
            return default(T);
        }

        public void Load()
        {
            try
            {
                if (File.Exists(".\\datas.sdw"))
                {
                    foreach (var item in File.ReadAllLines(".\\datas.sdw"))
                        if (item != "")
                            m_infos.Add(item.Split('=')[0], item.Split('=')[1]);
                    if(!m_infos.ContainsKey("version"))
                    {
                        m_infos.Clear();
                        m_infos.Add("version", "2.42");
                    }
                }
                else
                {
                    m_infos.Add("version", "2.42");
                }
            }
            catch (Exception)
            {
                m_infos.Add("version", "2.42");
                File.Delete(".\\datas.sdw");
            }
        }

        public void Save()
        {
            string[] infos = m_infos.Where(x => (x.Value as string).Contains("=")).Select(x => x.Key + "=" + x.Value).ToArray();
            File.WriteAllLines(".\\datas.sdw", infos);
        }
    }
}
