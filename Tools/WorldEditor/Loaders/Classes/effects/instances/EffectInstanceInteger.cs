

// Generated on 02/19/2017 13:42:11
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("EffectInstanceInteger", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceInteger : EffectInstance
    {
        public int value;
        [D2OIgnore]
        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}