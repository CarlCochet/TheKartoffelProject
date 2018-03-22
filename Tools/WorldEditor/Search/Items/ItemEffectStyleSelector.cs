using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace WorldEditor.Search.Items
{
    public class ItemEffectStyleSelector : StyleSelector
    {
        public Brush LineBrush { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            Style st = new Style
            {
                TargetType = typeof(ListBoxItem)
            };
            Setter backGroundSetter = new Setter
            {
                Property = Control.BackgroundProperty
            };
            ListBox listBox = ItemsControl.ItemsControlFromItemContainer(container) as ListBox;
            int index = listBox.ItemContainerGenerator.IndexFromContainer(container);
            backGroundSetter.Value = ((index % 2 == 0) ? LineBrush : Brushes.Transparent);
            st.Setters.Add(backGroundSetter);
            return st;
        }
    }
}
