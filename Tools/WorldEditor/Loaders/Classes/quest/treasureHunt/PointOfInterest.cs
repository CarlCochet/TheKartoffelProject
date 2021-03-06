

// Generated on 02/19/2017 13:42:19
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("PointOfInterest", "com.ankamagames.dofus.datacenter.quest.treasureHunt")]
    [Serializable]
    public class PointOfInterest : IDataObject, IIndexedData
    {
        public const String MODULE = "PointOfInterest";
        public uint id;
        [I18NField]
        public uint nameId;
        public uint categoryId;
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
        public uint CategoryId
        {
            get { return this.categoryId; }
            set { this.categoryId = value; }
        }
    }
}