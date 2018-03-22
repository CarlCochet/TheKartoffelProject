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
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Helpers.Controls
{
    public partial class I18NTextBox : UserControl
    {
        private List<Languages> m_availableLanguages;

        public List<Languages> AvailableLanguages
        {
            get { return m_availableLanguages; }
        }

        public I18NTextBox()
        {
            InitializeComponent();
            m_availableLanguages = Enum.GetValues(typeof(Languages)).Cast<Languages>().ToList();
        }
    }
}