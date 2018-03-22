

// Generated on 02/19/2017 13:42:11
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("EffectInstanceString", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceString : EffectInstance
    {
        public String text;
        [D2OIgnore]
        public String Text
        {
            get { return this.text; }
            set { this.text = value; }
        }
    }
}