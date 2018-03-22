using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WorldEditor.Helpers
{
    public class DataResource : Freezable
    {
        public static readonly DependencyProperty BindingTargetProperty = DependencyProperty.Register("BindingTarget", typeof(object), typeof(DataResource), new UIPropertyMetadata(null));

        public object BindingTarget
        {
            get { return GetValue(DataResource.BindingTargetProperty); }
            set { SetValue(DataResource.BindingTargetProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return (Freezable)Activator.CreateInstance(GetType());
        }

        protected sealed override void CloneCore(Freezable sourceFreezable)
        {
            base.CloneCore(sourceFreezable);
        }
    }
}
