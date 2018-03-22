

// Generated on 02/19/2017 13:42:11
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("EffectInstanceCreature", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceCreature : EffectInstance
    {
        public uint monsterFamilyId;
        [D2OIgnore]
        public uint MonsterFamilyId
        {
            get { return this.monsterFamilyId; }
            set { this.monsterFamilyId = value; }
        }
    }
}