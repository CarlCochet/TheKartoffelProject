

// Generated on 02/19/2017 13:42:12
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("Incarnation", "com.ankamagames.dofus.datacenter.items")]
    [Serializable]
    public class Incarnation : IDataObject, IIndexedData
    {
        public const String MODULE = "Incarnation";
        public uint id;
        public String lookMale;
        public String lookFemale;
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
        public String LookMale
        {
            get { return this.lookMale; }
            set { this.lookMale = value; }
        }
        [D2OIgnore]
        public String LookFemale
        {
            get { return this.lookFemale; }
            set { this.lookFemale = value; }
        }
    }
}