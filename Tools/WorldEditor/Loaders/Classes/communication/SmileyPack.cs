

// Generated on 02/19/2017 13:42:10
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("SmileyPack", "com.ankamagames.dofus.datacenter.communication")]
    [Serializable]
    public class SmileyPack : IDataObject, IIndexedData
    {
        public const String MODULE = "SmileyPacks";
        public uint id;
        [I18NField]
        public uint nameId;
        public uint order;
        public List<uint> smileys;
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
        public uint Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
        [D2OIgnore]
        public List<uint> Smileys
        {
            get { return this.smileys; }
            set { this.smileys = value; }
        }
    }
}