

// Generated on 02/19/2017 13:42:17
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("RideFood", "com.ankamagames.dofus.datacenter.mounts")]
    [Serializable]
    public class RideFood : IDataObject
    {
        public String MODULE = "RideFood";
        public uint gid;
        public uint typeId;
        [D2OIgnore]
        public uint Gid
        {
            get { return this.gid; }
            set { this.gid = value; }
        }
        [D2OIgnore]
        public uint TypeId
        {
            get { return this.typeId; }
            set { this.typeId = value; }
        }
    }
}