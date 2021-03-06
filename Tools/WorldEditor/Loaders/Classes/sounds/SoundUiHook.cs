

// Generated on 02/19/2017 13:42:20
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("SoundUiHook", "com.ankamagames.dofus.datacenter.sounds")]
    [Serializable]
    public class SoundUiHook : IDataObject, IIndexedData
    {
        public String MODULE = "SoundUiHook";
        public uint id;
        public String name;
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
        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
    }
}