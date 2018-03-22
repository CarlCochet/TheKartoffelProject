using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WorldEditor.Helpers
{
    public static class MessageService
    {
        private static MessageBoxResult MessageBoxResult
        {
            get
            {
                return MessageBoxResult.None;
            }
        }
        private static MessageBoxOptions MessageBoxOptions
        {
            get
            {
                return CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RtlReading : MessageBoxOptions.None;
            }
        }
        public static void ShowMessage(object owner, string message)
        {
            Window ownerWindow = owner as Window;
            if (ownerWindow != null)
            {
                MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.None, MessageService.MessageBoxResult, MessageService.MessageBoxOptions);
            }
            else
            {
                MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.None, MessageService.MessageBoxResult, MessageService.MessageBoxOptions);
            }
        }
        public static void ShowWarning(object owner, string message)
        {
            Window ownerWindow = owner as Window;
            if (ownerWindow != null)
            {
                MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageService.MessageBoxResult, MessageService.MessageBoxOptions);
            }
            else
            {
                MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageService.MessageBoxResult, MessageService.MessageBoxOptions);
            }
        }
        public static void ShowError(object owner, string message)
        {
            Window ownerWindow = owner as Window;
            if (ownerWindow != null)
            {
                MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Hand, MessageService.MessageBoxResult, MessageService.MessageBoxOptions);
            }
            else
            {
                MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Hand, MessageService.MessageBoxResult, MessageService.MessageBoxOptions);
            }
        }
        public static bool? ShowQuestion(object owner, string message)
        {
            Window ownerWindow = owner as Window;
            MessageBoxResult result;
            if (ownerWindow != null)
            {
                result = MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel, MessageService.MessageBoxOptions);
            }
            else
            {
                result = MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel, MessageService.MessageBoxOptions);
            }
            bool? result2;
            switch (result)
            {
                case MessageBoxResult.Yes:
                    result2 = new bool?(true);
                    break;
                case MessageBoxResult.No:
                    result2 = new bool?(false);
                    break;
                default:
                    result2 = null;
                    break;
            }
            return result2;
        }
        public static bool ShowYesNoQuestion(object owner, string message)
        {
            Window ownerWindow = owner as Window;
            MessageBoxResult result;
            if (ownerWindow != null)
            {
                result = MessageBox.Show(ownerWindow, message, ApplicationInfo.ProductName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageService.MessageBoxOptions);
            }
            else
            {
                result = MessageBox.Show(message, ApplicationInfo.ProductName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageService.MessageBoxOptions);
            }
            return result == MessageBoxResult.Yes;
        }
    }
}
