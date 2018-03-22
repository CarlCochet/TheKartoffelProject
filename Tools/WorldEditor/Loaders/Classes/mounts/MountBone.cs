

// Generated on 02/19/2017 13:42:17
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("MountBone", "com.ankamagames.dofus.datacenter.mounts")]
    [Serializable]
    public class MountBone : IDataObject, IIndexedData
    {
        private String MODULE = "MountBones";
        public uint id;
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
    }
}