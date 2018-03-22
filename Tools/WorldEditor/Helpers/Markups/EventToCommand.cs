using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
namespace WorldEditor.Helpers.Markups
{
    public class EventToCommand : MarkupExtension
    {
        private Type m_eventArgsType;

        public string BindingCommandPath { get; set; }

        public ICommand Command { get; set; }

        public EventToCommand()
        {
        }

        public EventToCommand(string bindingCommandPath)
        {
            BindingCommandPath = bindingCommandPath;
        }

        public override object ProvideValue(IServiceProvider sp)
        {
            IProvideValueTarget pvt = sp.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            object result;
            if (pvt != null)
            {
                EventInfo evt = pvt.TargetProperty as EventInfo;
                MethodInfo doAction = base.GetType().GetMethod("DoAction", BindingFlags.Instance | BindingFlags.NonPublic);
                Type dlgType = null;
                if (evt != null)
                {
                    dlgType = evt.EventHandlerType;
                }
                MethodInfo mi = pvt.TargetProperty as MethodInfo;
                if (mi != null)
                {
                    dlgType = mi.GetParameters()[1].ParameterType;
                }
                if (dlgType != null)
                {
                    m_eventArgsType = dlgType.GetMethod("Invoke").GetParameters()[1].ParameterType;
                    result = Delegate.CreateDelegate(dlgType, this, doAction);
                    return result;
                }
            }
            result = null;
            return result;
        }

        private void DoAction(object sender, RoutedEventArgs e)
        {
            object dc = (sender as FrameworkElement).DataContext;
            if (BindingCommandPath != null)
            {
                Command = (ICommand)EventToCommand.ParsePropertyPath(dc, BindingCommandPath);
            }
            Type eventArgsType = typeof(EventCommandArgs<>).MakeGenericType(new Type[]
			{
				m_eventArgsType
			});
            object cmdParams = Activator.CreateInstance(eventArgsType, new object[]
			{
				sender,
				e
			});
            if (Command != null && Command.CanExecute(cmdParams))
            {
                Command.Execute(cmdParams);
            }
        }

        private static object ParsePropertyPath(object target, string path)
        {
            string[] props = path.Split(new char[]
			{
				'.'
			});
            return props.Aggregate(target, (object current, string prop) => current.GetType().GetProperty(prop).GetValue(current));
        }
    }
}
