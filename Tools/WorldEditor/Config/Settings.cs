using Stump.ORM;
using System;
using System.IO;
using WorldEditor.Config.Configuration;
using WorldEditor.Config.Configuration.Config;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Config
{
    public class Settings
    {
        public const string ConfigPath = "editor_config.xml";
        private static XmlConfig m_config;

        [Variable(Priority = 10)]
        public static DatabaseConfiguration DatabaseConfiguration = new DatabaseConfiguration
        {
            Host = "localhost",
            DbName = "stump_data",
            User = "root",
            Password = "",
            ProviderName = "MySql.Data.MySqlClient"
        };

        private static bool m_isFirstLaunch = true;

        [Variable]
        public static LoaderSettings LoaderSettings = new LoaderSettings
        {
            BasePath = "Dofus2\\",
            D2IRelativeDirectory = "data\\i18n",
            D2ORelativeDirectory = "data\\common",
            ItemIconsRelativeFile = "content\\gfx\\items\\bitmap0.d2p",
            MapsRelativeFile = "content\\maps\\maps0.d2p",
            WorldGfxRelativeFile = "content\\gfx\\world\\gfx0.d2p",
            WorldEleRelativeFile = "content\\maps\\elements.ele",
            GenericMapDecryptionKey = "649ae451ca33ec53bbcbcc33becf15f4"
        };

        [Variable]
        public static Languages DefaultLanguage = Languages.French;

        [Variable]
        public static uint MinI18NId = 200000;

        [Variable]
        public static int MinDataId = 20000;

        [Variable]
        public static bool IsFirstLaunch
        {
            get { return m_isFirstLaunch; }
            set { m_isFirstLaunch = value; }
        }

        private static string FindDofusPath()
        {
            string programFiles = Environment.GetEnvironmentVariable("programfiles(x86)");
            if (string.IsNullOrEmpty(programFiles))
            {
                programFiles = Environment.GetEnvironmentVariable("programfiles");
            }

            if (string.IsNullOrEmpty(programFiles))
            {
                return null;
            }

            if (Directory.Exists(Path.Combine(programFiles, "Dofus2", "app")))
            {
                return Path.Combine(programFiles, "Dofus2", "app");
            }

            if (Directory.Exists(Path.Combine(programFiles, "Dofus 2", "app")))
            {
                return Path.Combine(programFiles, "Dofus 2", "app");
            }
            else
            {
                return null;
            }
        }

        public static void LoadSettings()
        {
            m_config = new XmlConfig(ConfigPath);
            m_config.AddAssembly(typeof(Settings).Assembly);
            if (!File.Exists(ConfigPath))
            {
                m_config.Create(false);
            }
            else
            {
                m_config.Load();
            }
        }

        public static void SaveSettings()
        {
            if (m_config == null)
            {
                throw new Exception("Cannot save settings, config file not loaded");
            }
            m_config.Save();
        }
    }
}