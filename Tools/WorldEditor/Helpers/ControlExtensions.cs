using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;

namespace WorldEditor.Helpers
{
    public class ControlExtensions
    {
        public static readonly System.Windows.DependencyProperty TriggersProperty = System.Windows.DependencyProperty.RegisterAttached("Triggers", typeof(StyleTriggerCollection), typeof(ControlExtensions), new System.Windows.UIPropertyMetadata(null, new System.Windows.PropertyChangedCallback(ControlExtensions.OnTriggersChanged)));
    
        public static StyleTriggerCollection GetTriggers(System.Windows.DependencyObject obj)
        {
            return (StyleTriggerCollection)obj.GetValue(ControlExtensions.TriggersProperty);
        }
        
        public static void SetTriggers(System.Windows.DependencyObject obj, StyleTriggerCollection value)
        {
            obj.SetValue(ControlExtensions.TriggersProperty, value);
        }
        
        private static void OnTriggersChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            StyleTriggerCollection triggers = (StyleTriggerCollection)e.NewValue;
            if (triggers != null)
            {
                TriggerCollection existingTriggers = Interaction.GetTriggers(d);
                foreach (TriggerBase trigger in triggers)
                {
                    existingTriggers.Add((TriggerBase)trigger.Clone());
                }
            }
        }
    }
}
