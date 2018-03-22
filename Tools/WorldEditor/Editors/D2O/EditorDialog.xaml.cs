using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WorldEditor.Editors.Files.D2O
{
    /// <summary>
    /// Logique d'interaction pour EditorDialog.xaml
    /// </summary>
    public partial class EditorDialog : Window, INotifyPropertyChanged
    {
        private FrameworkElement m_editor;
        public EditorDialog()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public EditorDialog(FrameworkElement editor)
        {
            m_editor = editor;
            InitializeComponent();
            Container.Content = editor;
            base.DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ((IPersistableChanged)m_editor).PersistChanges();
            base.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }
    }
}
