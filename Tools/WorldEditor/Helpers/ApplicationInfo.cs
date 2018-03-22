using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorldEditor.Helpers
{
    public static class ApplicationInfo
    {
        private static string m_productName;
        private static bool m_productNameCached;
        private static string m_version;
        private static bool m_versionCached;
        private static string m_company;
        private static bool m_companyCached;
        private static string m_copyright;
        private static bool m_copyrightCached;
        private static string m_applicationPath;
        private static bool m_applicationPathCached;

        public static string ProductName
        {
            get
            {
                if (!ApplicationInfo.m_productNameCached)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        AssemblyProductAttribute attribute = (AssemblyProductAttribute)Attribute.GetCustomAttribute(entryAssembly, typeof(AssemblyProductAttribute));
                        ApplicationInfo.m_productName = ((attribute != null) ? attribute.Product : "");
                    }
                    else
                    {
                        ApplicationInfo.m_productName = "";
                    }
                    ApplicationInfo.m_productNameCached = true;
                }
                return ApplicationInfo.m_productName;
            }
        }
        public static string Version
        {
            get
            {
                if (!ApplicationInfo.m_versionCached)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    ApplicationInfo.m_version = ((entryAssembly != null) ? entryAssembly.GetName().Version.ToString() : "");
                    ApplicationInfo.m_versionCached = true;
                }
                return ApplicationInfo.m_version;
            }
        }
        public static string Company
        {
            get
            {
                if (!ApplicationInfo.m_companyCached)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        AssemblyCompanyAttribute attribute = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(entryAssembly, typeof(AssemblyCompanyAttribute));
                        ApplicationInfo.m_company = ((attribute != null) ? attribute.Company : "");
                    }
                    else
                    {
                        ApplicationInfo.m_company = "";
                    }
                    ApplicationInfo.m_companyCached = true;
                }
                return ApplicationInfo.m_company;
            }
        }
        public static string Copyright
        {
            get
            {
                if (!ApplicationInfo.m_copyrightCached)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        AssemblyCopyrightAttribute attribute = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(entryAssembly, typeof(AssemblyCopyrightAttribute));
                        ApplicationInfo.m_copyright = ((attribute != null) ? attribute.Copyright : "");
                    }
                    else
                    {
                        ApplicationInfo.m_copyright = "";
                    }
                    ApplicationInfo.m_copyrightCached = true;
                }
                return ApplicationInfo.m_copyright;
            }
        }

        public static string ApplicationPath
        {
            get
            {
                if (!ApplicationInfo.m_applicationPathCached)
                {
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    ApplicationInfo.m_applicationPath = ((entryAssembly != null) ? Path.GetDirectoryName(entryAssembly.Location) : "");
                    ApplicationInfo.m_applicationPathCached = true;
                }
                return ApplicationInfo.m_applicationPath;
            }
        }
    }
}
