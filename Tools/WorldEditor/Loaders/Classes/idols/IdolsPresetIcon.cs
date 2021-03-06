

// Generated on 02/19/2017 13:42:12
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("IdolsPresetIcon", "com.ankamagames.dofus.datacenter.idols")]
    [Serializable]
    public class IdolsPresetIcon : IDataObject, IIndexedData
    {
        public const String MODULE = "IdolsPresetIcons";
        public int id;
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
        public int Order
        {
            get { return this.order; }
            set { this.order = value; }
        }
    }
}