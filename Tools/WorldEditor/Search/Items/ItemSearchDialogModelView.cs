using System;
using System.Collections.Generic;
using System.Linq;
using WorldEditor.Editors.Items;
using WorldEditor.Loaders.Data;
using WorldEditor.Loaders.Database;
using WorldEditor.Loaders.Objetcs;

namespace WorldEditor.Search.Items
{
    public class ItemSearchDialogModelView : SearchDialogModelView<ItemWrapper>
    {
        public ItemSearchDialogModelView()
            : base(typeof(ItemTemplate))
        {
        }

        protected override bool CanEditItem(object parameter)
        {
            return parameter is ItemWrapper;
        }

        protected override void OnEditItem(object parameter)
        {
            if (parameter != null && CanEditItem(parameter))
            {
                var editor = new ItemEditor(((ItemWrapper)parameter).WrappedItem);
                editor.Show();
            }
        }

        public override IEnumerable<ItemWrapper> FindMatches()
        {
            var whereStatement = GetSQLWhereStatement();
            var matches = new List<ItemTemplate>();
            if (string.IsNullOrEmpty(whereStatement))
            {
                matches = ObjectDataManager.Instance.Query<ItemTemplate>("SELECT * FROM items_templates").ToList();
                matches.AddRange(ObjectDataManager.Instance.Query<WeaponTemplate>("SELECT * FROM items_templates_weapons"));
            }
            else
            {
                matches = ObjectDataManager.Instance.Query<ItemTemplate>(string.Format("SELECT * FROM items_templates WHERE {0}", whereStatement)).ToList();
                matches.AddRange(ObjectDataManager.Instance.Query<WeaponTemplate>(string.Format("SELECT * FROM items_templates_weapons WHERE {0}", whereStatement)));
            }
            return matches.Select(entry => new ItemWrapper(entry));
        }
    }
}
