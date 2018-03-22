

// Generated on 02/19/2017 13:42:21
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("MapReference", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class MapReference : IDataObject, IIndexedData
    {
        public const String MODULE = "MapReferences";
        public int id;
        public uint mapId;
        public int cellId;
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
        public uint MapId
        {
            get { return this.mapId; }
            set { this.mapId = value; }
        }
        [D2OIgnore]
        public int CellId
        {
            get { return this.cellId; }
            set { this.cellId = value; }
        }
    }
}