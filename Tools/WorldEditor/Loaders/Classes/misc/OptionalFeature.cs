

// Generated on 02/19/2017 13:42:16
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("OptionalFeature", "com.ankamagames.dofus.datacenter.misc")]
    [Serializable]
    public class OptionalFeature : IDataObject, IIndexedData
    {
        public const String MODULE = "OptionalFeatures";
        public int id;
        public String keyword;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public String Keyword
        {
            get { return this.keyword; }
            set { this.keyword = value; }
        }
    }
}