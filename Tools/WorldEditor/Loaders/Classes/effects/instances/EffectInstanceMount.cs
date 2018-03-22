

// Generated on 02/19/2017 13:42:11
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("EffectInstanceMount", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceMount : EffectInstance
    {
        public double date;
        public uint modelId;
        public uint mountId;
        [D2OIgnore]
        public double Date
        {
            get { return this.date; }
            set { this.date = value; }
        }
        [D2OIgnore]
        public uint ModelId
        {
            get { return this.modelId; }
            set { this.modelId = value; }
        }
        [D2OIgnore]
        public uint MountId
        {
            get { return this.mountId; }
            set { this.mountId = value; }
        }
    }
}