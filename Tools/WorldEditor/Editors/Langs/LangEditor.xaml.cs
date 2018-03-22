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

namespace WorldEditor.Editors.Langs
{
    public partial class LangEditor : Window
    {
        public LangEditorModelView ModelView { get; private set; }

        public LangEditor()
        {
            this.InitializeComponent();
			this.ModelView = new LangEditorModelView(this);
            base.DataContext = this.ModelView;
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            this.ExpanderRow.Height = GridLength.Auto;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            this.ExpanderRow.Height = GridLength.Auto;
        }

        private void TextsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ModelView.RemoveRowCommand.RaiseCanExecuteChanged();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.ModelView.FindCommand.RaiseCanExecuteChanged();
        }
    }
}
