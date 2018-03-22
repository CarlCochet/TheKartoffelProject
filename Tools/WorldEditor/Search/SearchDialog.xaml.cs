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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorldEditor.Search
{
    public partial class SearchDialog : UserControl
    {
        public static readonly DependencyProperty ResultItemTemplateProperty = DependencyProperty.Register("ResultItemTemplate", typeof(DataTemplate), typeof(SearchDialog), new PropertyMetadata(null));
       
        public SearchDialog()
        {
            InitializeComponent();
        }

        public DataTemplate ResultItemTemplate
        {
            get { return (DataTemplate)base.GetValue(SearchDialog.ResultItemTemplateProperty); }
            set { base.SetValue(SearchDialog.ResultItemTemplateProperty, value); }
        }

        private void ResultListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
