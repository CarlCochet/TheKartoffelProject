

// Generated on 02/19/2017 13:42:21
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("SpellType", "com.ankamagames.dofus.datacenter.spells")]
    [Serializable]
    public class SpellType : IDataObject, IIndexedData
    {
        public const String MODULE = "SpellTypes";
        public int id;
        [I18NField]
        public uint longNameId;
        [I18NField]
        public uint shortNameId;
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
        public uint LongNameId
        {
            get { return this.longNameId; }
            set { this.longNameId = value; }
        }
        [D2OIgnore]
        public uint ShortNameId
        {
            get { return this.shortNameId; }
            set { this.shortNameId = value; }
        }
    }
}