

// Generated on 02/19/2017 13:42:21
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("Phoenix", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class Phoenix : IDataObject, IIndexedData
    {
        public const String MODULE = "Phoenixes";
        public uint mapId;
        int IIndexedData.Id
        {
            get { return (int)mapId; }
        }
        [D2OIgnore]
        public uint MapId
        {
            get { return this.mapId; }
            set { this.mapId = value; }
        }
    }
}