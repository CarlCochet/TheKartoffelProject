

// Generated on 02/19/2017 13:42:15
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("SpeakingItemText", "com.ankamagames.dofus.datacenter.livingObjects")]
    [Serializable]
    public class SpeakingItemText : IDataObject, IIndexedData
    {
        public const String MODULE = "SpeakingItemsText";
        public int textId;
        public double textProba;
        [I18NField]
        public uint textStringId;
        public int textLevel;
        public int textSound;
        public String textRestriction;
        int IIndexedData.Id
        {
            get { return (int)textId; }
        }
        [D2OIgnore]
        public int TextId
        {
            get { return this.textId; }
            set { this.textId = value; }
        }
        [D2OIgnore]
        public double TextProba
        {
            get { return this.textProba; }
            set { this.textProba = value; }
        }
        [D2OIgnore]
        public uint TextStringId
        {
            get { return this.textStringId; }
            set { this.textStringId = value; }
        }
        [D2OIgnore]
        public int TextLevel
        {
            get { return this.textLevel; }
            set { this.textLevel = value; }
        }
        [D2OIgnore]
        public int TextSound
        {
            get { return this.textSound; }
            set { this.textSound = value; }
        }
        [D2OIgnore]
        public String TextRestriction
        {
            get { return this.textRestriction; }
            set { this.textRestriction = value; }
        }
    }
}