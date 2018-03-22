

// Generated on 02/19/2017 13:42:19
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("PointOfInterestCategory", "com.ankamagames.dofus.datacenter.quest.treasureHunt")]
    [Serializable]
    public class PointOfInterestCategory : IDataObject, IIndexedData
    {
        public const String MODULE = "PointOfInterestCategory";
        public uint id;
        [I18NField]
        public uint actionLabelId;
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
        public uint ActionLabelId
        {
            get { return this.actionLabelId; }
            set { this.actionLabelId = value; }
        }
    }
}