using WorldEditor.Loaders.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WorldEditor.Editors.Items.Effects;
using WorldEditor.Helpers;
using WorldEditor.Loaders.Data;
using WorldEditor.Loaders.Icons;
using WorldEditor.Loaders.Objetcs;

namespace WorldEditor.Editors.Items
{
    public class ItemEditorModelView : INotifyPropertyChanged
    {
        private List<EffectTemplate> m_effects;
        public event Action<ItemEditorModelView, ItemWrapper> ItemSaved;
        public event PropertyChangedEventHandler PropertyChanged;
        private ItemWrapper _item;
        private List<ItemTypeRecord> _types;
        private List<ItemSetTemplate> _itemSets;

        public ItemWrapper Item
        {
            get { return _item; }
            set
            {
                if (_item == value)
                {
                    return;
                }
                _item = value;
                OnPropertyChanged("IsWeapon");
                OnPropertyChanged("ResetItemSetCommand");
                OnPropertyChanged("ChangeIconCommand");
                OnPropertyChanged("EditEffectCommand");
                OnPropertyChanged("RemoveEffectCommand");
                OnPropertyChanged("AddEffectCommand");
                OnPropertyChanged("SaveCommand");
                OnPropertyChanged("Item");
            }
        }

        public List<ItemTypeRecord> Types
        {
            get { return _types; }
            set
            {
                if (_types == value)
                {
                    return;
                }
                _types = value;
                OnPropertyChanged("Types");
            }
        }

        public List<ItemSetTemplate> ItemSets
        {
            get { return _itemSets; }
            set
            {
                if (_itemSets == value)
                {
                    return;
                }
                _itemSets = value;
                OnPropertyChanged("ItemSets");
            }
        }

        public bool IsWeapon
        {
            get { return Item is WeaponWrapper; }
        }

        public DelegateCommand ResetItemSetCommand { get; private set; }

        public DelegateCommand ChangeIconCommand { get; set; }

        public DelegateCommand EditEffectCommand { get; set; }

        public DelegateCommand RemoveEffectCommand { get; set; }

        public DelegateCommand AddEffectCommand { get; set; }

        public DelegateCommand SaveCommand { get; set; }

        public ItemEditorModelView(ItemTemplate item)
            : this((item is WeaponTemplate) ? new WeaponWrapper((WeaponTemplate)item) : new ItemWrapper(item))
        {
        }

        public ItemEditorModelView(ItemWrapper wrapper)
        {
            ResetItemSetCommand = new DelegateCommand(OnResetItemSet, CanResetItemSet);
            ChangeIconCommand = new DelegateCommand(OnChangeIcon, CanChangeIcon);
            EditEffectCommand = new DelegateCommand(OnEditEffect, CanEditEffect);
            RemoveEffectCommand = new DelegateCommand(OnRemoveEffect, CanRemoveEffect);
            AddEffectCommand = new DelegateCommand(OnAddEffect, CanAddEffect);
            SaveCommand = new DelegateCommand(OnSave, CanSave);

            Item = wrapper;

            Types = ObjectDataManager.Instance.EnumerateObjects<ItemTypeRecord>()
                .Where(entry => IsWeapon ? ((int)entry.ZoneShape != 0) : ((int)entry.ZoneShape == 0))
                .ToList(); //si arme rawzone valide, si item rawzone invalide

            ItemSets = ObjectDataManager.Instance.EnumerateObjects<ItemSetTemplate>().ToList();
        }

        protected virtual void OnItemSaved(ItemWrapper arg2)
        {
            var handler = ItemSaved;
            if (handler != null)
            {
                handler(this, arg2);
            }
        }

        private static bool CanResetItemSet(object parameter)
        {
            return true;
        }

        private void OnResetItemSet(object parameter)
        {
            Item.ItemSetId = int.MaxValue;
        }

        private static bool CanChangeIcon(object parameter)
        {
            return true;
        }

        private void OnChangeIcon(object parameter)
        {
            IconSelectionDialog dialog = new IconSelectionDialog
            {
                IconsSource = IconsManager.Instance.Icons,
                SelectedIcon = IconsManager.Instance.GetIcon(Item.IconId)
            };
            if (dialog.ShowDialog() == true)
            {
                Item.IconId = dialog.SelectedIcon.Id;
            }
        }

        private static bool CanEditEffect(object parameter)
        {
            return parameter is EffectWrapper;
        }

        private void OnEditEffect(object parameter)
        {
            if (parameter != null && ItemEditorModelView.CanEditEffect(parameter))
            {
                var effect = (EffectWrapper)parameter;
                var effectEditorDialog = new EffectEditorDialog();
                effectEditorDialog.EffectToEdit = (EffectWrapper)effect.Clone();

                if (m_effects == null)
                {
                    m_effects = ObjectDataManager.Instance.EnumerateObjects<EffectTemplate>().ToList();
                }

                effectEditorDialog.EffectsSource = m_effects;

                if (effectEditorDialog.ShowDialog() == true)
                {
                    int index = Item.WrappedEffects.IndexOf(effect);
                    Item.WrappedEffects.Remove(effect);
                    Item.WrappedEffects.Insert(index, effectEditorDialog.EffectToEdit);
                }
            }
        }

        private static bool CanRemoveEffect(object parameter)
        {
            return parameter is EffectWrapper || parameter is ICollection;
        }

        private void OnRemoveEffect(object parameter)
        {
            if (parameter != null && ItemEditorModelView.CanRemoveEffect(parameter))
            {
                var wrapper = parameter as EffectWrapper;
                if (wrapper != null)
                {
                    Item.WrappedEffects.Remove(wrapper);
                }
                else
                {
                    ICollection collection = parameter as ICollection;
                    if (collection != null)
                    {
                        Array effects = new object[collection.Count];
                        collection.CopyTo(effects, 0);
                        foreach (EffectWrapper effect in effects)
                        {
                            Item.WrappedEffects.Remove(effect);
                        }
                    }
                }
            }
        }

        private static bool CanAddEffect(object parameter)
        {
            return true;
        }

        private void OnAddEffect(object parameter)
        {
            var effectEditorDialog = new EffectEditorDialog();

            if (m_effects == null)
            {
                m_effects = ObjectDataManager.Instance.EnumerateObjects<EffectTemplate>().ToList();
            }

            effectEditorDialog.EffectsSource = m_effects;

            EffectDiceWrapper effect = new EffectDiceWrapper(new EffectInstanceDice
            {
                EffectId = (uint)m_effects.First().Id
            });

            effectEditorDialog.EffectToEdit = effect;
            if (effectEditorDialog.ShowDialog() == true)
            {
                Item.WrappedEffects.Add(effectEditorDialog.EffectToEdit);
            }
        }

        private static bool CanSave(object parameter)
        {
            return true;
        }

        private void OnSave(object parameter)
        {
            try
            {
                Item.Save();
            }
            catch (Exception ex)
            {
                MessageService.ShowError(null, "Cannot save properly : " + ex);
                return;
            }
            OnItemSaved(Item);
            MessageService.ShowMessage(null, "File saved successfully");
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}