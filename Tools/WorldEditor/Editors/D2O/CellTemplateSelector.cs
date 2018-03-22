using System;
using System.Windows;
using System.Windows.Controls;
namespace WorldEditor.Editors.Files.D2O
{
    public class CellTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Template { get; set; }

        public Type ExpectedType { get; set; }

        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return DefaultTemplate;
            }
            else
            {
                Type type = item.GetType();
                if (type != ExpectedType && !type.IsSubclassOf(ExpectedType))
                {
                    return DefaultTemplate;
                }
                else
                {
                    return Template;
                }
            }
        }
    }
}
