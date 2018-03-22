

// Generated on 02/19/2017 13:42:09
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("SkinMapping", "com.ankamagames.dofus.datacenter.appearance")]
    [Serializable]
    public class SkinMapping : IDataObject, IIndexedData
    {
        public const String MODULE = "SkinMappings";
        public int id;
        public int lowDefId;
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
        public int LowDefId
        {
            get { return this.lowDefId; }
            set { this.lowDefId = value; }
        }
    }
}