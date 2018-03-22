using WorldEditor.Loaders.Classes;
using System;
namespace WorldEditor.Editors.Items.Effects
{
    public class EffectValueWrapper : EffectWrapper
    {
        private readonly EffectInstanceInteger m_wrappedEffect;
        public int Value
        {
            get
            {
                return m_wrappedEffect.Value;
            }
            set
            {
                if (value != m_wrappedEffect.value)
                {
                    m_wrappedEffect.value = value;
                    OnPropertyChanged("Description");
                }
                OnPropertyChanged("Item");
                OnPropertyChanged("Parameters");
                OnPropertyChanged("Value");
            }
        }
        public override int ParametersCount
        {
            get
            {
                return 1;
            }
        }
        public override object this[int index]
        {
            get
            {
                object result;
                if (index == 0)
                {
                    result = Value;
                }
                else
                {
                    result = null;
                }
                return result;
            }
            set
            {
                if (index == 0)
                {
                    Value = (int)value;
                }
            }
        }
        public override object[] Parameters
        {
            get
            {
                return new object[]
				{
					Value
				};
            }
        }
        public EffectValueWrapper(EffectInstanceInteger wrappedEffect)
            : base(wrappedEffect)
        {
            m_wrappedEffect = wrappedEffect;
        }
    }
}
