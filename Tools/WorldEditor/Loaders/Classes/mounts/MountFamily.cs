

// Generated on 02/19/2017 13:42:17
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("MountFamily", "com.ankamagames.dofus.datacenter.mounts")]
    [Serializable]
    public class MountFamily : IDataObject, IIndexedData
    {
        private String MODULE = "MountFamily";
        public uint id;
        [I18NField]
        public uint nameId;
        public String headUri;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public String HeadUri
        {
            get { return this.headUri; }
            set { this.headUri = value; }
        }
    }
}