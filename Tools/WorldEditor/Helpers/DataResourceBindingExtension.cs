using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace WorldEditor.Helpers
{
    public class DataResourceBindingExtension : MarkupExtension
    {
        private DataResource mDataResouce;
        private object mTargetObject;
        private object mTargetProperty;

        public DataResourceBindingExtension()
            : base()
        {

        }

        public DataResource DataResource
        {
            get
            {
                return mDataResouce;
            }
            set
            {
                if (!object.Equals(mDataResouce, value))
                {
                    if (mDataResouce != null)
                    {
                        mDataResouce.Changed -= new EventHandler(DataResource_Changed);
                    }
                    mDataResouce = value;
                    if (mDataResouce != null)
                    {
                        mDataResouce.Changed += new EventHandler(DataResource_Changed);
                    }
                }
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            mTargetObject = target.TargetObject;
            mTargetProperty = target.TargetProperty;
            Debug.Assert(mTargetProperty != null || DesignerProperties.GetIsInDesignMode(new DependencyObject()));

            if (DataResource.BindingTarget == null && mTargetProperty != null)
            {
                PropertyInfo propInfo = mTargetProperty as PropertyInfo;
                if (propInfo != null)
                {
                    try
                    {
                        return Activator.CreateInstance(propInfo.PropertyType);
                    }
                    catch (MissingMethodException)
                    {
                    }
                }
                DependencyProperty depProp = mTargetProperty as DependencyProperty;
                if (depProp != null)
                {
                    DependencyObject depObj = (DependencyObject)mTargetObject;
                    return depObj.GetValue(depProp);
                }
            }
            return DataResource.BindingTarget;
        }

        private void DataResource_Changed(object sender, EventArgs e)
        {
            DataResource dataResource = (DataResource)sender;
            DependencyProperty depProp = mTargetProperty as DependencyProperty;
            if (depProp != null)
            {
                DependencyObject depObj = (DependencyObject)mTargetObject;
                object value = Convert(dataResource.BindingTarget, depProp.PropertyType);
                depObj.SetValue(depProp, value);
            }
            else
            {
                PropertyInfo propInfo = mTargetProperty as PropertyInfo;
                if (propInfo != null)
                {
                    object value = Convert(dataResource.BindingTarget, propInfo.PropertyType);
                    propInfo.SetValue(mTargetObject, value, new object[0]);
                }
            }
        }

        private object Convert(object obj, Type toType)
        {
            try
            {
                return System.Convert.ChangeType(obj, toType);
            }
            catch (InvalidCastException)
            {
                return obj;
            }
        }
    }
}
