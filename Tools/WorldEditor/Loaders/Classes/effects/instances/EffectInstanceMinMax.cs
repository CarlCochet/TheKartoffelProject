

// Generated on 02/19/2017 13:42:11
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("EffectInstanceMinMax", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceMinMax : EffectInstance
    {
        public uint min;
        public uint max;
        [D2OIgnore]
        public uint Min
        {
            get { return this.min; }
            set { this.min = value; }
        }
        [D2OIgnore]
        public uint Max
        {
            get { return this.max; }
            set { this.max = value; }
        }
    }
}