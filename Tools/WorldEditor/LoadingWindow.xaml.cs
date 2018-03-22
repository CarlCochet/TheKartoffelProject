using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WorldEditor.Config;
using WorldEditor.Loaders.Data;
using WorldEditor.Loaders.Database;
using WorldEditor.Loaders.I18N;
using WorldEditor.Loaders.Icons;

namespace WorldEditor
{
    public partial class LoadingWindow : Window
    {
        private ManualResetEvent m_reset;
        public static readonly DependencyProperty StatusTextProperty = DependencyProperty.Register("StatusText", typeof(string), typeof(LoadingWindow), new PropertyMetadata(null));

        public LoadingWindow()
        {
            InitializeComponent();
        }

        public string StatusText
        {
            get { return (string)base.GetValue(LoadingWindow.StatusTextProperty); }
            set { base.SetValue(LoadingWindow.StatusTextProperty, value); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(Initialize);
        }

        private void Initialize()
        {
            SetStatus("Load {0}...", Settings.ConfigPath);
            Settings.LoadSettings();
            if (Settings.IsFirstLaunch)
            {
                ShowConfigDialog();
            }

            OpenDB();
            I18NDataManager.Instance.DefaultLanguage = Languages.French;

            InitializeLoader(I18NDataManager.Instance.Initialize, "Loading d2i files ...");
            InitializeLoader(IconsManager.Instance.Initialize, "Loading item icons ...");
            InitializeLoader(ObjectDataManager.Instance.Initialize, "Loading tables informations ...");

            EndInitialization();
        }

        private void EndInitialization()
        {
            Dispatcher.Invoke(Close);
        }

        private void OpenDB()
        {
            SetStatus("Open Database ...");
            try
            {
                DatabaseManager.Instance.Connect();
                DatabaseManager.Instance.Database.CommandTimeout = 120;
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(string.Format("Unable to reach database on {0} : {1}\r\nDo you want to modify the config ?", Settings.DatabaseConfiguration.Host, ex.Message), "Error", MessageBoxButton.YesNo, MessageBoxImage.Hand) == MessageBoxResult.Yes)
                {
                    ShowConfigDialog();
                    OpenDB();
                }
                else
                {
                    Dispatcher.Invoke(Application.Current.Shutdown);
                }
            }
        }

        private void InitializeLoader(Action initializer, string message)
        {
            SetStatus(message);
            try
            {
                initializer();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(string.Format("Cannot perform initialization : {0}\r\nDo you want to modify the config ?", ex.Message), "Error", MessageBoxButton.YesNo, MessageBoxImage.Hand) == MessageBoxResult.Yes)
                {
                    ShowConfigDialog();
                    InitializeLoader(initializer, message);
                }
                else
                {
                    Dispatcher.Invoke(Application.Current.Shutdown);
                }
            }
        }

        private void ShowConfigDialog()
        {
            m_reset = new ManualResetEvent(false);
            Dispatcher.InvokeAsync(delegate
            {
                Hide();
                ConfigDialog dialog = new ConfigDialog();
                if (!object.Equals(dialog.ShowDialog(), true))
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    Show();
                    m_reset.Set();
                }
            });
            m_reset.WaitOne();
        }

        private void SetStatus(string text, params object[] args)
        {
            Dispatcher.InvokeAsync(() => StatusText = string.Format(text, args));
        }
    }
}
