using System;

namespace WorldEditor.Loaders.Data
{
    public class D2OClassAttribute : Attribute
    {
        public string Name { get; set; }

        public string PackageName { get; set; }

        public bool AutoBuild { get; set; }

        public D2OClassAttribute(string name, bool autoBuild = true)
        {
            Name = name;
            AutoBuild = autoBuild;
        }

        public D2OClassAttribute(string name, string packageName, bool autoBuild = true)
        {
            Name = name;
            PackageName = packageName;
            AutoBuild = autoBuild;
        }
    }
}
