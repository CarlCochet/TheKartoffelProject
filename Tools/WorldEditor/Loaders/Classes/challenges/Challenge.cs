

// Generated on 02/19/2017 13:42:10
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("Challenge", "com.ankamagames.dofus.datacenter.challenges")]
    [Serializable]
    public class Challenge : IDataObject, IIndexedData
    {
        public const String MODULE = "Challenge";
        public int id;
        [I18NField]
        public uint nameId;
        [I18NField]
        public uint descriptionId;
        public Boolean dareAvailable;
        public List<uint> incompatibleChallenges;
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
        public uint DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
        [D2OIgnore]
        public Boolean DareAvailable
        {
            get { return this.dareAvailable; }
            set { this.dareAvailable = value; }
        }
        [D2OIgnore]
        public List<uint> IncompatibleChallenges
        {
            get { return this.incompatibleChallenges; }
            set { this.incompatibleChallenges = value; }
        }
    }
}