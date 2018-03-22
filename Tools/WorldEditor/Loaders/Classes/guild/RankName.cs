

// Generated on 02/19/2017 13:42:12
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("RankName", "com.ankamagames.dofus.datacenter.guild")]
    [Serializable]
    public class RankName : IDataObject, IIndexedData
    {
        public const String MODULE = "RankNames";
        public int id;
        [I18NField]
        public uint nameId;
        public int order;
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
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public int Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
    }
}