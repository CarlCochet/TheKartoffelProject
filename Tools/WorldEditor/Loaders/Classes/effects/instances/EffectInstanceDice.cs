

// Generated on 02/19/2017 13:42:11
using System;
using System.Collections.Generic;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Loaders.Classes
{
    [D2OClass("EffectInstanceDice", "com.ankamagames.dofus.datacenter.effects.instances")]
    [Serializable]
    public class EffectInstanceDice : EffectInstanceInteger
    {
        public uint diceNum;
        public uint diceSide;
        [D2OIgnore]
        public uint DiceNum
        {
            get { return this.diceNum; }
            set { this.diceNum = value; }
        }
        [D2OIgnore]
        public uint DiceSide
        {
            get { return this.diceSide; }
            set { this.diceSide = value; }
        }
    }
}