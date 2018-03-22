using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WorldEditor.Config;
using WorldEditor.Editors.Items;
using WorldEditor.Editors.Langs;
using WorldEditor.Helpers;
using WorldEditor.Search.Items;

namespace WorldEditor
{
    public class StartModelView
    {
        public DelegateCommand CreateItemCommand { get; set; }
        public DelegateCommand CreateWeaponCommand { get; set; }
        public DelegateCommand SearchItemCommand { get; set; }
        public DelegateCommand EditLangsCommand { get; private set; }
        public DelegateCommand OpenConfigCommand { get; private set; }

        public StartModelView()
        {
            OpenConfigCommand =  new DelegateCommand(OnOpenConfig, CanOpenConfig);
            EditLangsCommand = new DelegateCommand(OnEditLangs, CanEditLangs);
            SearchItemCommand = new DelegateCommand(OnSearchItem, CanSearchItem);
            CreateWeaponCommand = new DelegateCommand(OnCreateWeapon, CanCreateWeapon);
            CreateItemCommand = new DelegateCommand(OnCreateItem, CanCreateItem);
        }
        
        private static bool CanCreateItem(object parameter)
        {
            return true;
        }

        private static void OnCreateItem(object parameter)
        {
            ItemEditor editor = new ItemEditor(new ItemWrapper());
            editor.Show();
        }

        private bool CanCreateWeapon(object parameter)
        {
            return true;
        }

        private void OnCreateWeapon(object parameter)
        {
            ItemEditor editor = new ItemEditor(new WeaponWrapper());
            editor.Show();
        }

        private static bool CanSearchItem(object parameter)
        {
            return true;
        }

        private static void OnSearchItem(object parameter)
        {
            ItemSearchDialog window = new ItemSearchDialog();
            window.Show();
        }

        private bool CanEditLangs(object parameter)
        {
            return true;
        }

        private void OnEditLangs(object parameter)
        {
            LangEditor editor = new LangEditor();
            editor.Show();
        }

        private bool CanOpenConfig(object parameter)
        {
            return true;
        }

        private static void OnOpenConfig(object parameter)
        {
            if (new ConfigDialog().ShowDialog() == true)
            {
                if (MessageService.ShowYesNoQuestion(null, "To take the config changes in account you have to restart the application. Do you want to restart ?"))
                {
                    Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                }
            }
        }
    }
}
