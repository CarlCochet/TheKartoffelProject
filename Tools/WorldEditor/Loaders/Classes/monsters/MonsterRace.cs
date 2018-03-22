

// Generated on 02/19/2017 13:42:17
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("MonsterRace", "com.ankamagames.dofus.datacenter.monsters")]
    [Serializable]
    public class MonsterRace : IDataObject, IIndexedData
    {
        public const String MODULE = "MonsterRaces";
        public int id;
        public int superRaceId;
        [I18NField]
        public uint nameId;
        public List<uint> monsters;
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
        public int SuperRaceId
        {
            get { return this.superRaceId; }
            set { this.superRaceId = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public List<uint> Monsters
        {
            get { return this.monsters; }
            set { this.monsters = value; }
        }
    }
}