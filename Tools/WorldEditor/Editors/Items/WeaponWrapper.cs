using WorldEditor.Loaders.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorldEditor.Loaders.Data;
using WorldEditor.Loaders.I18N;
using WorldEditor.Loaders.Objetcs;

namespace WorldEditor.Editors.Items
{
    public class WeaponWrapper : ItemWrapper
    {
        public WeaponTemplate WrappedWeapon
        {
            get { return (WeaponTemplate)base.WrappedItem; }
        }

        public int ApCost
        {
            get { return WrappedWeapon.ApCost; }
            set
            {
                if (ApCost == value)
                {
                    return;
                }
                WrappedWeapon.ApCost = value;
                OnPropertyChanged("ApCost");
            }
        }

        public int MinRange
        {
            get { return WrappedWeapon.MinRange; }
            set
            {
                if (MinRange == value)
                {
                    return;
                }
                WrappedWeapon.MinRange = value;
                OnPropertyChanged("MinRange");
            }
        }

        public int Range
        {
            get { return WrappedWeapon.Range; }
            set
            {
                if (Range == value)
                {
                    return;
                }
                WrappedWeapon.Range = value;
                OnPropertyChanged("Range");
            }
        }

        public bool CastInLine
        {
            get { return WrappedWeapon.CastInLine; }
            set
            {
                if (CastInLine == value)
                {
                    return;
                }
                WrappedWeapon.CastInLine = value;
                OnPropertyChanged("CastInLine");
            }
        }

        public bool CastInDiagonal
        {
            get { return WrappedWeapon.CastInDiagonal; }
            set
            {
                if (CastInDiagonal == value)
                {
                    return;
                }
                WrappedWeapon.CastInDiagonal = value;
                OnPropertyChanged("CastInDiagonal");
            }
        }

        public bool CastTestLos
        {
            get { return WrappedWeapon.CastTestLos; }
            set
            {
                if (CastTestLos == value)
                {
                    return;
                }
                WrappedWeapon.CastTestLos = value;
                OnPropertyChanged("CastTestLos");
            }
        }

        public int CriticalHitProbability
        {
            get { return WrappedWeapon.CriticalHitProbability; }
            set
            {
                if (CriticalHitProbability == value)
                {
                    return;
                }
                WrappedWeapon.CriticalHitProbability = value;
                OnPropertyChanged("CriticalHitProbability");
            }
        }

        public int CriticalHitBonus
        {
            get { return WrappedWeapon.CriticalHitBonus; }
            set
            {
                if (CriticalHitBonus == value)
                {
                    return;
                }
                WrappedWeapon.CriticalHitBonus = value;
                OnPropertyChanged("CriticalHitBonus");
            }
        }

        public int CriticalFailureProbability
        {
            get { return WrappedWeapon.CriticalFailureProbability; }
            set
            {
                if (CriticalFailureProbability == value)
                {
                    return;
                }
                WrappedWeapon.CriticalFailureProbability = value;
                OnPropertyChanged("CriticalFailureProbability");
            }
        }

        public WeaponWrapper()
        {
            base.WrappedItem = new WeaponTemplate();
            m_name = new LangText();
            m_name.SetText(Languages.All, "New weapon");
            m_description = new LangText();
            m_description.SetText(Languages.All, "Item description");
            m_effects = new ObservableCollection<EffectWrapper>();
            WrappedItem.PossibleEffects = new List<EffectInstance>();
            WrappedItem.Criteria = "";
            WrappedItem.CriteriaTarget = "";
            WrappedItem.ItemSetId = -1;
            New = true;
        }

        public WeaponWrapper(WeaponTemplate wrappedWeapon)
            : base(wrappedWeapon)
        {
            base.WrappedItem = wrappedWeapon;
        }

        public override void Save()
        {
            if (New)
            {
                Id = Math.Max(ObjectDataManager.Instance.FindFreeId<ItemTemplate>(), ObjectDataManager.Instance.FindFreeId<WeaponTemplate>());
                NameId = I18NDataManager.Instance.FindFreeId();
                DescriptionId = NameId + 1;
            }
            WrappedItem.PossibleEffects = WrappedEffects.Select(entry => entry.WrappedEffect).ToList();
            if (New)
            {
                ObjectDataManager.Instance.Insert(WrappedWeapon);
            }
            else
            {
                ObjectDataManager.Instance.Update(WrappedWeapon);
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
            New = false;
        }
    }
}