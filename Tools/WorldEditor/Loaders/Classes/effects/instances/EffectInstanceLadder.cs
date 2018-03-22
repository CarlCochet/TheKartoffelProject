

// Generated on 02/19/2017 13:42:11
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("EffectInstanceLadder", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceLadder : EffectInstanceCreature
    {
        public uint monsterCount;
        [D2OIgnore]
        public uint MonsterCount
        {
            get { return this.monsterCount; }
            set { this.monsterCount = value; }
        }
    }
}