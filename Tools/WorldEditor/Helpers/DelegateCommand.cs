using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WorldEditor.Helpers
{
    public class DelegateCommand : ICommand
    {
        private readonly Func<object, bool> m_canExecute;
        private readonly Action<object> m_execute;
        private List<WeakReference> m_weakHandlers;

        public DelegateCommand(Action execute, Func<bool> canExecute = null)
            : this(execute != null ? p => execute() : (Action<object>)null, canExecute != null ? p => canExecute() : (Func<object, bool>)null)
        {
        }

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            m_execute = execute;
            m_canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (m_weakHandlers == null)
                {
                    m_weakHandlers = new List<WeakReference>(new[] { new WeakReference(value) });
                }
                else
                {
                    m_weakHandlers.Add(new WeakReference(value));
                }

                CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (m_weakHandlers == null)
                {
                    return;
                }

                for (int i = m_weakHandlers.Count - 1; i >= 0; i--)
                {
                    WeakReference weakReference = m_weakHandlers[i];
                    var handler = weakReference.Target as EventHandler;
                    if (handler == null || handler == value)
                    {
                        m_weakHandlers.RemoveAt(i);
                    }
                }

                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return m_canExecute == null || m_canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("The command cannot be executed because the canExecute action returned false.");
            }

            m_execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            PurgeWeakHandlers();

            if (m_weakHandlers == null)
            {
                return;
            }

            WeakReference[] handlers = m_weakHandlers.ToArray();
            foreach (WeakReference reference in handlers)
            {
                var handler = reference.Target as EventHandler;
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        private void PurgeWeakHandlers()
        {
            if (m_weakHandlers == null)
            {
                return;
            }

            for (int i = m_weakHandlers.Count - 1; i >= 0; i--)
            {
                if (!m_weakHandlers[i].IsAlive)
                {
                    m_weakHandlers.RemoveAt(i);
                }
            }

            if (m_weakHandlers.Count == 0)
            {
                m_weakHandlers = null;
            }
        }
    }
}
