﻿using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace Stump.Server.WorldServer.Database.World.Maps
{

    public class MapScrollActionRecordRelator
    {
        public static string FetchQuery = "SELECT * FROM world_maps_scroll_actions";
    }
    [D2OClass("MapScrollActions", "com.ankamagames.dofus.datacenter.world"), TableName("world_maps_scroll_actions")]
    public class MapScrollActionRecord : IAutoGeneratedRecord, IAssignedByD2O
    {
        public int Id { get; set; }
        public bool RightExists { get; set; }
        public bool BottomExists { get; set; }
        public bool LeftExists { get; set; }
        public bool TopExists { get; set; }
        public int RightMapId { get; set; }
        public int BottomMapId { get; set; }
        public int LeftMapId { get; set; }
        public int TopMapId { get; set; }
        public void AssignFields(object d2oObject)
        {
            var mapScroll = (MapScrollAction) d2oObject;
            Id = mapScroll.id;
            RightExists = mapScroll.rightExists;
            BottomExists = mapScroll.bottomExists;
            LeftExists = mapScroll.leftExists;
            TopExists = mapScroll.topExists;
            RightMapId = mapScroll.rightMapId;
            BottomMapId = mapScroll.bottomMapId;
            LeftMapId = mapScroll.leftMapId;
            TopMapId = mapScroll.topMapId;
        }
    }
}