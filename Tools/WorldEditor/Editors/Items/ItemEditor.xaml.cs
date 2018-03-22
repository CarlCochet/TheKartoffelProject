using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WorldEditor.Loaders.Objetcs;

namespace WorldEditor.Editors.Items
{
    public partial class ItemEditor : Window
    {
        public ItemEditorModelView ModelView { get; set; }

        public ItemEditor(ItemTemplate item)
        {
            InitializeComponent();
            base.DataContext = (ModelView = new ItemEditorModelView(item));
        }

        public ItemEditor(ItemWrapper wrapper)
        {
            InitializeComponent();
            base.DataContext = (ModelView = new ItemEditorModelView(wrapper));
        }

        private void EffectsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModelView.RemoveEffectCommand.RaiseCanExecuteChanged();
            ModelView.EditEffectCommand.RaiseCanExecuteChanged();
        }

        private void ItemIdEdit_OnClick(object sender, RoutedEventArgs e)
        {
            ItemIdField.IsEnabled = !ItemIdField.IsEnabled;
        }
    }
}
