

// Generated on 02/19/2017 13:42:09
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("CreatureBoneType", "com.ankamagames.dofus.datacenter.appearance")]
    [Serializable]
    public class CreatureBoneType : IDataObject, IIndexedData
    {
        public const String MODULE = "CreatureBonesTypes";
        public int id;
        public int creatureBoneId;
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
        public int CreatureBoneId
        {
            get { return this.creatureBoneId; }
            set { this.creatureBoneId = value; }
        }
    }
}