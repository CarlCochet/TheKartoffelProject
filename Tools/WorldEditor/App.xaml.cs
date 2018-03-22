using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using WorldEditor.Helpers;

namespace WorldEditor
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (!Debugger.IsAttached)
            {
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            }
            MainWindow mainWindow = new MainWindow();
            base.MainWindow = mainWindow;
            mainWindow.Hide();
            LoadingWindow loadingWindow = new LoadingWindow();
            loadingWindow.ShowDialog();
            mainWindow.Show();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageService.ShowError(null, "Unhandled Exception : " + e.ExceptionObject);
            Clipboard.SetText(e.ExceptionObject.ToString());
        }
    }
}
