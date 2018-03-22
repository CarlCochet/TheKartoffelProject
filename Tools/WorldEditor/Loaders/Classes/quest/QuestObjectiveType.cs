

// Generated on 02/19/2017 13:42:18
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("QuestObjectiveType", "com.ankamagames.dofus.datacenter.quest")]
    [Serializable]
    public class QuestObjectiveType : IDataObject, IIndexedData
    {
        public const String MODULE = "QuestObjectiveTypes";
        public uint id;
        [I18NField]
        public uint nameId;
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
    }
}