using WorldEditor.Loaders.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WorldEditor.Loaders.Data;
using WorldEditor.Loaders.I18N;
using WorldEditor.Loaders.Objetcs;

namespace WorldEditor.Editors.Items
{
    public class ItemWrapper : INotifyPropertyChanged
    {
        protected LangText m_name;
        protected LangText m_description;
        private ItemTypeRecord m_type;
        protected ObservableCollection<EffectWrapper> m_effects;
        private Languages m_currentLanguage = Languages.All;
        public event PropertyChangedEventHandler PropertyChanged;

        private ItemTemplate _wrappedItem;
        public ItemTemplate WrappedItem
        {
            get { return _wrappedItem; }
            protected set
            {
                if (_wrappedItem == value)
                {
                    return;
                }
                _wrappedItem = value;
                OnPropertyChanged("Id");
                OnPropertyChanged("NameId");
                OnPropertyChanged("Name");
                OnPropertyChanged("TypeId");
                OnPropertyChanged("Type");
                OnPropertyChanged("DescriptionId");
                OnPropertyChanged("Description");
                OnPropertyChanged("IconId");
                OnPropertyChanged("Level");
                OnPropertyChanged("RealWeight");
                OnPropertyChanged("Cursed");
                OnPropertyChanged("UseAnimationId");
                OnPropertyChanged("Usable");
                OnPropertyChanged("Targetable");
                OnPropertyChanged("Price");
                OnPropertyChanged("TwoHanded");
                OnPropertyChanged("Etheral");
                OnPropertyChanged("ItemSetId");
                OnPropertyChanged("Criteria");
                OnPropertyChanged("CriteriaTarget");
                OnPropertyChanged("HideEffects");
                OnPropertyChanged("Enhanceable");
                OnPropertyChanged("NonUsableOnAnother");
                OnPropertyChanged("AppearanceId");
                OnPropertyChanged("SecretRecipe");
                OnPropertyChanged("BonusIsSecret");
                OnPropertyChanged("PossibleEffects");
                OnPropertyChanged("Weight");
                OnPropertyChanged("WrappedItem");
            }
        }

        private bool _new;
        public bool New
        {
            get { return _new; }
            protected set
            {
                if (_new == value)
                {
                    return;
                }
                _new = value;
                OnPropertyChanged("New");
            }
        }

        public Languages CurrentLanguage
        {
            get { return m_currentLanguage; }
            set
            {
                if (m_currentLanguage == value)
                {
                    return;
                }
                m_currentLanguage = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("Description");
                OnPropertyChanged("CurrentLanguage");
            }
        }

        public string Name
        {
            get
            {
                if (m_name == null)
                {
                    m_name = I18NDataManager.Instance.GetText(NameId);
                }
                return (m_name != null) ? m_name.GetText(CurrentLanguage, I18NDataManager.Instance.DefaultLanguage) : "NO_NAME";
            }
            set
            {
                if (string.Equals(Name, value, StringComparison.Ordinal))
                {
                    return;
                }
                if (m_name == null)
                {
                    m_name = new LangText();
                }
                m_name.SetText(CurrentLanguage, value);
                OnPropertyChanged("Name");
            }
        }

        public int Id
        {
            get { return WrappedItem.Id; }
            set
            {
                if (Id == value)
                {
                    return;
                }
                WrappedItem.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public uint NameId
        {
            get { return WrappedItem.NameId; }
            set
            {
                if (NameId == value)
                {
                    return;
                }
                WrappedItem.NameId = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("NameId");
            }
        }

        public int TypeId
        {
            get { return (int)WrappedItem.TypeId; }
            set
            {
                if (TypeId == value)
                {
                    return;
                }
                WrappedItem.TypeId = (uint)value;
                m_type = ObjectDataManager.Instance.Get<ItemTypeRecord>(TypeId);
                OnPropertyChanged("Type");
                OnPropertyChanged("TypeId");
            }
        }

        public uint DescriptionId
        {
            get { return WrappedItem.DescriptionId; }
            set
            {
                if (DescriptionId == value)
                {
                    return;
                }
                WrappedItem.DescriptionId = value;
                OnPropertyChanged("Description");
                OnPropertyChanged("DescriptionId");
            }
        }

        public string Description
        {
            get
            {
                if (m_description == null)
                {
                    m_description = I18NDataManager.Instance.GetText(DescriptionId);
                }
                return (m_description != null) ? m_description.GetText(CurrentLanguage, I18NDataManager.Instance.DefaultLanguage) : "NO_DESCRIPTION";
            }
            set
            {
                if (string.Equals(Description, value, StringComparison.Ordinal))
                {
                    return;
                }
                if (m_description == null)
                {
                    m_description = new LangText();
                }
                m_description.SetText(CurrentLanguage, value);
                OnPropertyChanged("Description");
            }
        }

        public int IconId
        {
            get { return WrappedItem.IconId; }
            set
            {
                if (IconId == value)
                {
                    return;
                }
                WrappedItem.IconId = value;
                OnPropertyChanged("IconId");
            }
        }

        public uint Level
        {
            get { return WrappedItem.Level; }
            set
            {
                if (Level == value)
                {
                    return;
                }
                WrappedItem.Level = value;
                OnPropertyChanged("Level");
            }
        }

        public uint RealWeight
        {
            get { return WrappedItem.RealWeight; }
            set
            {
                if (RealWeight == value)
                {
                    return;
                }
                WrappedItem.RealWeight = value;
                WrappedItem.Weight = value;
                OnPropertyChanged("RealWeight");
            }
        }

        public bool Cursed
        {
            get { return WrappedItem.Cursed; }
            set
            {
                if (Cursed == value)
                {
                    return;
                }
                WrappedItem.Cursed = value;
                OnPropertyChanged("Cursed");
            }
        }

        public int UseAnimationId
        {
            get { return WrappedItem.UseAnimationId; }
            set
            {
                if (UseAnimationId == value)
                {
                    return;
                }
                WrappedItem.UseAnimationId = value;
                OnPropertyChanged("UseAnimationId");
            }
        }

        public bool Usable
        {
            get
            {
                return WrappedItem.Usable;
            }
            set
            {
                if (Usable == value)
                {
                    return;
                }
                WrappedItem.Usable = value;
                OnPropertyChanged("Usable");
            }
        }

        public bool Targetable
        {
            get
            {
                return WrappedItem.Targetable;
            }
            set
            {
                if (Targetable == value)
                {
                    return;
                }
                WrappedItem.Targetable = value;
                OnPropertyChanged("Targetable");
            }
        }

        public double Price
        {
            get
            {
                return WrappedItem.Price;
            }
            set
            {
                if (Price == value)
                {
                    return;
                }
                WrappedItem.Price = (float)value;
                OnPropertyChanged("Price");
            }
        }

        public bool TwoHanded
        {
            get { return WrappedItem.TwoHanded; }
            set
            {
                if (TwoHanded == value)
                {
                    return;
                }
                WrappedItem.TwoHanded = value;
                OnPropertyChanged("TwoHanded");
            }
        }

        public bool Etheral
        {
            get { return WrappedItem.Etheral; }
            set
            {
                if (Etheral == value)
                {
                    return;
                }
                WrappedItem.Etheral = value;
                OnPropertyChanged("Etheral");
            }
        }

        public int ItemSetId
        {
            get { return WrappedItem.ItemSetId; }
            set
            {
                if (ItemSetId == value)
                {
                    return;
                }
                WrappedItem.ItemSetId = value;
                OnPropertyChanged("ItemSetId");
            }
        }

        public string Criteria
        {
            get { return WrappedItem.Criteria; }
            set
            {
                if (string.Equals(Criteria, value, StringComparison.Ordinal))
                {
                    return;
                }
                WrappedItem.Criteria = value;
                OnPropertyChanged("Criteria");
            }
        }

        public string CriteriaTarget
        {
            get { return WrappedItem.CriteriaTarget; }
            set
            {
                if (string.Equals(CriteriaTarget, value, StringComparison.Ordinal))
                {
                    return;
                }
                WrappedItem.CriteriaTarget = value;
                OnPropertyChanged("CriteriaTarget");
            }
        }

        public bool HideEffects
        {
            get { return WrappedItem.HideEffects; }
            set
            {
                if (HideEffects == value)
                {
                    return;
                }
                WrappedItem.HideEffects = value;
                OnPropertyChanged("HideEffects");
            }
        }

        public uint AppearanceId
        {
            get { return WrappedItem.AppearanceId; }
            set
            {
                if (AppearanceId == value)
                {
                    return;
                }
                WrappedItem.AppearanceId = value;
                OnPropertyChanged("AppearanceId");
            }
        }

        public bool BonusIsSecret
        {
            get { return WrappedItem.BonusIsSecret; }
            set
            {
                if (BonusIsSecret == value)
                {
                    return;
                }
                WrappedItem.BonusIsSecret = value;
                OnPropertyChanged("BonusIsSecret");
            }
        }

        public List<EffectInstance> PossibleEffects
        {
            get { return WrappedItem.PossibleEffects; }
            set
            {
                if (PossibleEffects == value)
                {
                    return;
                }
                WrappedItem.PossibleEffects = value;
                OnPropertyChanged("PossibleEffects");
            }
        }

        public ObservableCollection<EffectWrapper> WrappedEffects
        {
            get { return m_effects; }
        }

        public ItemTypeRecord Type
        {
            get
            {
                if (m_type == null)
                {
                    m_type = ObjectDataManager.Instance.Get<ItemTypeRecord>(TypeId);
                }
                return m_type;
            }
            set
            {
                if (m_type == value)
                {
                    return;
                }
                m_type = value;
                OnPropertyChanged("Type");
                WrappedItem.TypeId = (uint)value.Id;
                OnPropertyChanged("TypeId");
            }
        }

        public uint Weight
        {
            get { return WrappedItem.Weight; }
            set
            {
                if (Weight == value)
                {
                    return;
                }
                WrappedItem.Weight = value;
                WrappedItem.RealWeight = value;
                OnPropertyChanged("Weight");
            }
        }

        public ItemWrapper()
        {
            WrappedItem = new ItemTemplate();
            m_name = new LangText();
            m_name.SetText(Languages.All, "New item");
            m_description = new LangText();
            m_description.SetText(Languages.All, "Item description");
            m_effects = new ObservableCollection<EffectWrapper>();
            WrappedItem.PossibleEffects = new List<EffectInstance>();
            WrappedItem.Criteria = "";
            WrappedItem.CriteriaTarget = "";
            WrappedItem.ItemSetId = -1;
            New = true;
        }

        public ItemWrapper(ItemTemplate wrappedItem)
        {
            WrappedItem = wrappedItem;
            m_effects = new ObservableCollection<EffectWrapper>(PossibleEffects.Select(new Func<EffectInstance, EffectWrapper>(EffectWrapper.Create)));
        }
        public virtual void Save()
        {
            if (New)
            {
                if (Id == 0)
                {
                    Id = Math.Max(ObjectDataManager.Instance.FindFreeId<ItemTemplate>(), ObjectDataManager.Instance.FindFreeId<WeaponTemplate>());
                }
                NameId = I18NDataManager.Instance.FindFreeId();
                DescriptionId = NameId + 1;
            }

            WrappedItem.PossibleEffects = WrappedEffects.Select(entry => entry.WrappedEffect).ToList();

            if (New)
            {
                ObjectDataManager.Instance.Insert<ItemTemplate>(WrappedItem);
            }
            else
            {
                ObjectDataManager.Instance.Update<ItemTemplate>(WrappedItem);
            }
            if (New)
            {
                m_name.Id = NameId;
                m_description.Id = DescriptionId;
                I18NDataManager.Instance.CreateText(m_name);
                I18NDataManager.Instance.CreateText(m_description);
            }
            else
            {
                I18NDataManager.Instance.SaveText(m_name);
                I18NDataManager.Instance.SaveText(m_description);
            }
            I18NDataManager.Instance.Save();
            New = false;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}