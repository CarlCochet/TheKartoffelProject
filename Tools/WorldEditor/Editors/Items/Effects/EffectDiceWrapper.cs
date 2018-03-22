using WorldEditor.Loaders.Classes;
using System;

namespace WorldEditor.Editors.Items.Effects
{
    public class EffectDiceWrapper : EffectValueWrapper
    {
        private readonly EffectInstanceDice m_effect;
        public uint DiceSide
        {
            get
            {
                return m_effect.diceSide;
            }
            set
            {
                if (value != m_effect.diceSide)
                {
                    m_effect.diceSide = value;
                    OnPropertyChanged("Description");
                }
                OnPropertyChanged("Parameters");
                OnPropertyChanged("Item");
                OnPropertyChanged("DiceSide");
            }
        }
        public uint DiceNum
        {
            get
            {
                return m_effect.diceNum;
            }
            set
            {
                if (value != m_effect.diceSide)
                {
                    m_effect.diceNum = value;
                    OnPropertyChanged("Description");
                }
                OnPropertyChanged("Parameters");
                OnPropertyChanged("Item");
                OnPropertyChanged("DiceNum");
            }
        }
        public override int ParametersCount
        {
            get
            {
                return 3;
            }
        }
        public override object[] Parameters
        {
            get
            {
                return new object[]
				{
					DiceNum,
					DiceSide,
					base.Value
				};
            }
        }
        public override object this[int index]
        {
            get
            {
                object result;
                if (index == 0)
                {
                    result = DiceNum;
                }
                else
                {
                    if (index == 1)
                    {
                        result = DiceSide;
                    }
                    else
                    {
                        if (index == 2)
                        {
                            result = base.Value;
                        }
                        else
                        {
                            result = null;
                        }
                    }
                }
                return result;
            }
            set
            {
                if (index == 0)
                {
                    DiceNum = (uint)((int)value);
                }
                if (index == 1)
                {
                    DiceSide = (uint)((int)value);
                }
                if (index == 2)
                {
                    base.Value = (int)value;
                }
            }
        }
        public EffectDiceWrapper(EffectInstanceDice effect)
            : base(effect)
        {
            m_effect = effect;
        }
    }
}
