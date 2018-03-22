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
    public partial class EffectEditorDialog : Window
    {
        public static readonly DependencyProperty EffectToEditProperty = DependencyProperty.Register("EffectToEdit", typeof(EffectWrapper), typeof(EffectEditorDialog), new PropertyMetadata(null));
        public static readonly DependencyProperty EffectsSourceProperty = DependencyProperty.Register("EffectsSource", typeof(IEnumerable<EffectTemplate>), typeof(EffectEditorDialog), new PropertyMetadata(null));

        public EffectWrapper EffectToEdit
        {
            get
            {
                return (EffectWrapper)base.GetValue(EffectEditorDialog.EffectToEditProperty);
            }
            set
            {
                base.SetValue(EffectEditorDialog.EffectToEditProperty, value);
            }
        }
        public IEnumerable<EffectTemplate> EffectsSource
        {
            get
            {
                return (IEnumerable<EffectTemplate>)base.GetValue(EffectEditorDialog.EffectsSourceProperty);
            }
            set
            {
                base.SetValue(EffectEditorDialog.EffectsSourceProperty, value);
            }
        }
        public EffectEditorDialog()
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
