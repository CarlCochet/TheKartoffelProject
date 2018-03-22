

// Generated on 02/19/2017 13:42:11
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("EffectInstanceDate", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceDate : EffectInstance
    {
        public uint year;
        public uint month;
        public uint day;
        public uint hour;
        public uint minute;
        [D2OIgnore]
        public uint Year
        {
            get { return this.year; }
            set { this.year = value; }
        }
        [D2OIgnore]
        public uint Month
        {
            get { return this.month; }
            set { this.month = value; }
        }
        [D2OIgnore]
        public uint Day
        {
            get { return this.day; }
            set { this.day = value; }
        }
        [D2OIgnore]
        public uint Hour
        {
            get { return this.hour; }
            set { this.hour = value; }
        }
        [D2OIgnore]
        public uint Minute
        {
            get { return this.minute; }
            set { this.minute = value; }
        }
    }
}