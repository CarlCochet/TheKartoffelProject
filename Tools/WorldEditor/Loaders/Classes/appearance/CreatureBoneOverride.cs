

// Generated on 02/19/2017 13:42:09
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("CreatureBoneOverride", "com.ankamagames.dofus.datacenter.appearance")]
    [Serializable]
    public class CreatureBoneOverride : IDataObject, IIndexedData
    {
        public const String MODULE = "CreatureBonesOverrides";
        public int boneId;
        public int creatureBoneId;
        int IIndexedData.Id
        {
            get { return (int)boneId; }
        }
        [D2OIgnore]
        public int BoneId
        {
            get { return this.boneId; }
            set { this.boneId = value; }
        }
        [D2OIgnore]
        public int CreatureBoneId
        {
            get { return this.creatureBoneId; }
            set { this.creatureBoneId = value; }
        }
    }
}