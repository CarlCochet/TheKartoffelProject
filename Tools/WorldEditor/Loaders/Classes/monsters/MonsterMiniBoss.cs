

// Generated on 02/19/2017 13:42:17
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("MonsterMiniBoss", "com.ankamagames.dofus.datacenter.monsters")]
    [Serializable]
    public class MonsterMiniBoss : IDataObject, IIndexedData
    {
        public const String MODULE = "MonsterMiniBoss";
        public int id;
        public int monsterReplacingId;
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
        public int MonsterReplacingId
        {
            get { return this.monsterReplacingId; }
            set { this.monsterReplacingId = value; }
        }
    }
}