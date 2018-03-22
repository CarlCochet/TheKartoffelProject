

// Generated on 02/19/2017 13:42:15
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("Job", "com.ankamagames.dofus.datacenter.jobs")]
    [Serializable]
    public class Job : IDataObject, IIndexedData
    {
        public const String MODULE = "Jobs";
        public int id;
        [I18NField]
        public uint nameId;
        public int iconId;
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
        public int IconId
        {
            get { return this.iconId; }
            set { this.iconId = value; }
        }
    }
}