using WorldEditor.Loaders.D2p;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using WorldEditor.Editors.Files.D2P;

namespace WorldEditor.Editors.Files.D2P
{
    /// <summary>
    /// Logique d'interaction pour D2PEditor.xaml
    /// </summary>
    public partial class D2PEditor : Window
    {
        public D2PEditorModelView ModelView { get; private set; }

        public D2PEditor(D2pFile file)
        {
            InitializeComponent();
            ModelView = new D2PEditorModelView(this, file);
            base.DataContext = ModelView;
        }

        private void MouseDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            ModelView.ExploreFolderCommand.Execute(FilesGrid.SelectedItem);
        }

        private void FilesGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ModelView.RemoveFileCommand.RaiseCanExecuteChanged();
            ModelView.ExtractCommand.RaiseCanExecuteChanged();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ModelView.Dispose();
        }

        private void FilesGrid_PreviewDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
                if (filenames.Any(new Func<string, bool>(File.Exists)))
                {
                    e.Effects = DragDropEffects.Copy;
                }
            }
            e.Handled = true;
        }

        private void FilesGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
                string[] array = filenames;
                for (int i = 0; i < array.Length; i++)
                {
                    string filename = array[i];
                    if (File.Exists(filename))
                    {
                        ModelView.AddFile(filename);
                    }
                }
            }
            e.Handled = true;
        }
    }
}
