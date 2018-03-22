using WorldEditor.Loaders.D2p;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using WorldEditor.Editors.Files.D2I;
using WorldEditor.Editors.Files.D2O;
using WorldEditor.Editors.Files.D2P;
//using WorldEditor.Loaders.I18N;

namespace WorldEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new StartModelView();
        }

        private void MdiContainer_PreviewDragEnter(object sender, DragEventArgs e)
        {
            bool isCorrect = false;
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
                if ((
                    from filename in filenames
                    let ext = Path.GetExtension(filename)
                    where (ext == ".d2p" || ext == ".d2i" || ext == ".d2o" || ext == ".d2os" || ext == ".meta" || ext == ".dlm") && File.Exists(filename)
                    select filename).Any<string>())
                {
                    isCorrect = true;
                }
            }
            e.Effects = (isCorrect ? DragDropEffects.All : DragDropEffects.None);
            e.Handled = true;
        }

        private void MdiContainer_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
                foreach (Thread newWindowThread in
                    from filename in filenames
                    let position = base.PointToScreen(Mouse.GetPosition(this))
                    select new Thread(new ThreadStart(delegate
                    {
                        MainWindow.ThreadStartingPoint(filename, position);
                    })))
                {
                    newWindowThread.SetApartmentState(ApartmentState.STA);
                    newWindowThread.IsBackground = true;
                    newWindowThread.Start();
                }
            }
        }

        private static void ThreadStartingPoint(string filename, Point mousePosition)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            if (File.Exists(filename))
            {
                string extension = Path.GetExtension(filename);
                if (extension != null)
                {
                    Window window = null;
                    switch (extension)
                    {
                        case ".d2p":
                            window = new D2PEditor(new D2pFile(filename));
                            break;
                        case ".d2i":
                            window = new D2IEditor(new Stump.DofusProtocol.D2oClasses.Tools.D2i.D2IFile(filename));
                            break;
                        case ".d2o":
                        case ".d2os":
                            window = new D2OEditor(filename);
                            break;
                    }
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.Show();
                    Dispatcher.Run();
                }
            }
        }
    }
}