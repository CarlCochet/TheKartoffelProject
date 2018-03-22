using WorldEditor.Loaders.Classes;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using WorldEditor.Editors.Items.Effects;
using WorldEditor.Loaders.I18N;
using WorldEditor.Loaders.Objetcs;
using WorldEditor.Loaders.Database;
using WorldEditor.Helpers.Pattern;
using WorldEditor.Loaders.Data;

namespace WorldEditor.Editors.Items
{
    [Serializable]
    public abstract class EffectWrapper : ICloneable, INotifyPropertyChanged
    {
        private string m_description;
        private EffectTemplate m_template;
        public event PropertyChangedEventHandler PropertyChanged;
        private EffectInstance _wrappedEffect;

        public EffectInstance WrappedEffect
        {
            get { return _wrappedEffect; }
            private set
            {
                if (_wrappedEffect == value)
                {
                    return;
                }
                _wrappedEffect = value;
                OnPropertyChanged("EffectId");
                OnPropertyChanged("Template");
                OnPropertyChanged("Description");
                OnPropertyChanged("Operator");
                OnPropertyChanged("Priority");
                OnPropertyChanged("TargetId");
                OnPropertyChanged("Duration");
                OnPropertyChanged("Delay");
                OnPropertyChanged("Random");
                OnPropertyChanged("Group");
                OnPropertyChanged("Modificator");
                OnPropertyChanged("Trigger");
                OnPropertyChanged("Hidden");
                OnPropertyChanged("ZoneSize");
                OnPropertyChanged("ZoneShape");
                OnPropertyChanged("ZoneMinSize");
                OnPropertyChanged("RawZone");
                OnPropertyChanged("WrappedEffect");
            }
        }

        public EffectTemplate Template
        {
            get
            {
                if (m_template == null)
                {
                    m_template = ObjectDataManager.Instance.Query<EffectTemplate>("SELECT * FROM effects_templates WHERE Id=@0", EffectId).SingleOrDefault();
                }
                return m_template;
            }
        }

        public int EffectId
        {
            get { return (int)WrappedEffect.effectId; }
            set
            {
                if (EffectId == value)
                {
                    return;
                }
                WrappedEffect.effectId = (uint)value;
                m_template = ObjectDataManager.Instance.Query<EffectTemplate>("SELECT * FROM effects_templates WHERE Id=@0", EffectId).SingleOrDefault();
                OnPropertyChanged("Description");
                OnPropertyChanged("Template");
                OnPropertyChanged("Operator");
                OnPropertyChanged("Priority");
                OnPropertyChanged("EffectId");
            }
        }

        public int TargetId
        {
            get { return WrappedEffect.targetId; }
            set
            {
                if (TargetId == value)
                {
                    return;
                }
                WrappedEffect.targetId = value;
                OnPropertyChanged("TargetId");
            }
        }

        public int Duration
        {
            get { return WrappedEffect.duration; }
            set
            {
                if (Duration == value)
                {
                    return;
                }
                WrappedEffect.duration = value;
                OnPropertyChanged("Duration");
            }
        }

        public int Delay
        {
            get { return WrappedEffect.delay; }
            set
            {
                if (Delay == value)
                {
                    return;
                }
                WrappedEffect.delay = value;
                OnPropertyChanged("Delay");
            }
        }

        public int Random
        {
            get { return WrappedEffect.random; }
            set
            {
                if (Random == value)
                {
                    return;
                }
                WrappedEffect.random = value;
                OnPropertyChanged("Random");
            }
        }

        public int Group
        {
            get { return WrappedEffect.group; }
            set
            {
                if (Group == value)
                {
                    return;
                }
                WrappedEffect.group = value;
                OnPropertyChanged("Group");
            }
        }

        public int Modificator
        {
            get { return WrappedEffect.modificator; }
            set
            {
                if (Modificator == value)
                {
                    return;
                }
                WrappedEffect.modificator = value;
                OnPropertyChanged("Modificator");
            }
        }

        public bool Trigger
        {
            get { return WrappedEffect.trigger; }
            set
            {
                if (Trigger == value)
                {
                    return;
                }
                WrappedEffect.trigger = value;
                OnPropertyChanged("Trigger");
            }
        }

        public bool Hidden
        {
            get { return WrappedEffect.visibleInTooltip; }
            set
            {
                if (Hidden == value)
                {
                    return;
                }
                WrappedEffect.visibleInTooltip = value;
                OnPropertyChanged("Hidden");
            }
        }

        public uint ZoneSize
        {
            get { return (uint)WrappedEffect.zoneSize; }
            set
            {
                if (ZoneSize == value)
                {
                    return;
                }
                WrappedEffect.zoneSize = value;
                OnPropertyChanged("ZoneSize");
            }
        }

        public uint ZoneShape
        {
            get { return WrappedEffect.zoneShape; }
            set
            {
                if (ZoneShape == value)
                {
                    return;
                }
                WrappedEffect.zoneShape = value;
                OnPropertyChanged("ZoneShape");
            }
        }

        public uint ZoneMinSize
        {
            get { return (uint)WrappedEffect.zoneMinSize; }
            set
            {
                if (ZoneMinSize == value)
                {
                    return;
                }
                WrappedEffect.zoneMinSize = value;
                OnPropertyChanged("ZoneMinSize");
            }
        }

        public string RawZone
        {
            get { return WrappedEffect.rawZone; }
            set
            {
                if (string.Equals(RawZone, value, StringComparison.Ordinal))
                {
                    return;
                }
                WrappedEffect.rawZone = value;
                OnPropertyChanged("RawZone");
            }
        }

        public string Description
        {
            get
            {
                if (m_description != null)
                {
                    return m_description;
                }
                else
                {
                    string pattern = I18NDataManager.Instance.ReadText(Template.DescriptionId);
                    StringPatternDecoder decoder = new StringPatternDecoder(pattern, GetDescriptionValues());
                    int? num;
                    int? index = num = decoder.CheckValidity(false);
                    if (num.HasValue)
                    {
                        return string.Format("Error in pattern '{0}' at index {1}", pattern, index);
                    }
                    else
                    {
                        return (m_description = decoder.Decode());
                    }
                }
            }
        }

        public string Operator
        {
            get { return Template.Operator; }
        }

        public uint Priority
        {
            get { return Template.Priority; }
        }

        public abstract int ParametersCount { get; }

        public abstract object this[int index] { get; set; }

        public abstract object[] Parameters { get; }

        protected EffectWrapper(EffectInstance wrappedEffect)
        {
            WrappedEffect = wrappedEffect;
        }

        private object[] GetDescriptionValues()
        {
            return Parameters.Select(entry => (entry is uint && (uint)entry == 0) ? null : entry).ToArray();
        }

        public object Clone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, WrappedEffect);
            stream.Position = 0L;
            return EffectWrapper.Create((EffectInstance)formatter.Deserialize(stream));
        }

        public static EffectWrapper Create(EffectInstance effect)
        {
            EffectInstanceDice dice = effect as EffectInstanceDice;
            EffectWrapper result;
            if (dice != null)
            {
                result = new EffectDiceWrapper(dice);
            }
            else
            {
                EffectInstanceInteger integer = effect as EffectInstanceInteger;
                if (integer == null)
                {
                    throw new NotImplementedException();
                }
                result = new EffectValueWrapper(integer);
            }
            return result;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == "Description")
            {
                m_description = null;
            }
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
    {
        [UsedImplicitly]
        public string ParameterName { get; private set; }

        public NotifyPropertyChangedInvocatorAttribute()
        {
        }

        public NotifyPropertyChangedInvocatorAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public sealed class UsedImplicitlyAttribute : Attribute
    {
        [UsedImplicitly]
        public ImplicitUseKindFlags UseKindFlags { get; private set; }

        [UsedImplicitly]
        public ImplicitUseTargetFlags TargetFlags { get; private set; }

        [UsedImplicitly]
        public UsedImplicitlyAttribute()
            : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
        {
        }

        [UsedImplicitly]
        public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
        {
            UseKindFlags = useKindFlags;
            TargetFlags = targetFlags;
        }

        [UsedImplicitly]
        public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
            : this(useKindFlags, ImplicitUseTargetFlags.Default)
        {
        }

        [UsedImplicitly]
        public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
            : this(ImplicitUseKindFlags.Default, targetFlags)
        {
        }
    }

    [Flags]
    public enum ImplicitUseTargetFlags
    {
        Default = 1,
        Itself = 1,
        Members = 2,
        WithMembers = 3
    }

    [Flags]
    public enum ImplicitUseKindFlags
    {
        Default = 7,
        Access = 1,
        Assign = 2,
        InstantiatedWithFixedConstructorSignature = 4,
        InstantiatedNoFixedConstructorSignature = 8
    }
}