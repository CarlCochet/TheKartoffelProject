using System.Windows;
using System.Windows.Controls;
//using WorldEditor.Loaders.I18N;

namespace WorldEditor.Editors.Files.D2I
{
    public partial class D2IEditor : Window
    {
        public D2IEditorModelView ModelView { get; private set; }

        public D2IEditor(Stump.DofusProtocol.D2oClasses.Tools.D2i.D2IFile file)
        {
            InitializeComponent();
            ModelView = new D2IEditorModelView(this, file);
            base.DataContext = ModelView;
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            ExpanderRow.Height = GridLength.Auto;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            ExpanderRow.Height = GridLength.Auto;
        }

        private void TextsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModelView.RemoveRowCommand.RaiseCanExecuteChanged();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ModelView.FindCommand.RaiseCanExecuteChanged();
        }
    }
}
