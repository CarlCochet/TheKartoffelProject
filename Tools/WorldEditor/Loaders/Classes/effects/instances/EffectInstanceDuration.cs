

// Generated on 02/19/2017 13:42:11
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("EffectInstanceDuration", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceDuration : EffectInstance
    {
        public uint days;
        public uint hours;
        public uint minutes;
        [D2OIgnore]
        public uint Days
        {
            get { return this.days; }
            set { this.days = value; }
        }
        [D2OIgnore]
        public uint Hours
        {
            get { return this.hours; }
            set { this.hours = value; }
        }
        [D2OIgnore]
        public uint Minutes
        {
            get { return this.minutes; }
            set { this.minutes = value; }
        }
    }
}