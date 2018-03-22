using Stump.ORM;
using System.Windows;

namespace WorldEditor.Config
{
    public partial class ConfigDialog : Window
    {
        public ConfigDialogModelView ModelView { get; set; }

        public ConfigDialog()
        {
            InitializeComponent();
            ModelView = new ConfigDialogModelView();
            DataContext = ModelView;

            ModelView.DBConfig = new DatabaseConfiguration(
                Settings.DatabaseConfiguration.Host,
                Settings.DatabaseConfiguration.Port,
                Settings.DatabaseConfiguration.User,
                Settings.DatabaseConfiguration.Password,
                Settings.DatabaseConfiguration.DbName,
                Settings.DatabaseConfiguration.ProviderName);

            ModelView.LoaderSettings = Settings.LoaderSettings.Clone();
        }

        private void OnOKClicked(object sender, RoutedEventArgs e)
        {
            if (ModelView.IsFirstLaunch)
            {
                ModelView.IsFirstLaunch = false;
            }
            Settings.DatabaseConfiguration = ModelView.DBConfig;
            Settings.LoaderSettings = ModelView.LoaderSettings;
            Settings.SaveSettings();
            DialogResult = new bool?(true);
            Close();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = new bool?(false);
            Close();
        }
    }
}