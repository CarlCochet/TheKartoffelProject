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
using WorldEditor.Loaders.Icons;

namespace WorldEditor.Editors.Items
{
    public partial class IconSelectionDialog : Window
    {
        public static readonly DependencyProperty SelectedIconProperty = DependencyProperty.Register("SelectedIcon", typeof(Icon), typeof(IconSelectionDialog), new PropertyMetadata(null));
        public static readonly DependencyProperty IconsSourceProperty = DependencyProperty.Register("IconsSource", typeof(IEnumerable<Icon>), typeof(IconSelectionDialog), new PropertyMetadata(null));

        public Icon SelectedIcon
        {
            get { return (Icon)base.GetValue(IconSelectionDialog.SelectedIconProperty); }
            set { base.SetValue(IconSelectionDialog.SelectedIconProperty, value); }
        }

        public IEnumerable<Icon> IconsSource
        {
            get { return (IEnumerable<Icon>)base.GetValue(IconSelectionDialog.IconsSourceProperty); }
            set { base.SetValue(IconSelectionDialog.IconsSourceProperty, value); }
        }

        public IconSelectionDialog()
        {
            InitializeComponent();
        }

        private void OnButtonOKClick(object sender, RoutedEventArgs e)
        {
            base.DialogResult = new bool?(true);
            base.Close();
        }

        private void OnButtonCancelClick(object sender, RoutedEventArgs e)
        {
            base.DialogResult = new bool?(false);
            base.Close();
        }
    }
}
