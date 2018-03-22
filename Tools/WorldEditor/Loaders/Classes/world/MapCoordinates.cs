

// Generated on 02/19/2017 13:42:21
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("MapCoordinates", "com.ankamagames.dofus.datacenter.world")]
    [Serializable]
    public class MapCoordinates : IDataObject
    {
        public const String MODULE = "MapCoordinates";
        public uint compressedCoords;
        public List<int> mapIds;
        [D2OIgnore]
        public uint CompressedCoords
        {
            get { return this.compressedCoords; }
            set { this.compressedCoords = value; }
        }
        [D2OIgnore]
        public List<int> MapIds
        {
            get { return this.mapIds; }
            set { this.mapIds = value; }
        }
    }
}