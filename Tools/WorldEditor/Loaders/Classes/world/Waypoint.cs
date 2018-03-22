

// Generated on 02/19/2017 13:42:22
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("Waypoint", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class Waypoint : IDataObject, IIndexedData
    {
        public const String MODULE = "Waypoints";
        public uint id;
        public uint mapId;
        public uint subAreaId;
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
        public uint MapId
        {
            get { return this.mapId; }
            set { this.mapId = value; }
        }
        [D2OIgnore]
        public uint SubAreaId
        {
            get { return this.subAreaId; }
            set { this.subAreaId = value; }
        }
    }
}