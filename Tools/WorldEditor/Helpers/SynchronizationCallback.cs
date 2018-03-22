using System;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
namespace WorldEditor.Helpers

{
    public sealed class SynchronizationCallback
    {
        private Delegate Callback
        {
            get;
            set;
        }
        private int CallbackParameters
        {
            get;
            set;
        }
        private SynchronizationContext Context
        {
            get;
            set;
        }
        private SynchronizationCallback(Delegate callback)
            : this(callback, SynchronizationContext.Current ?? new SynchronizationContext())
        {
        }
        public SynchronizationCallback(Action callback)
            : this(callback, SynchronizationContext.Current ?? new SynchronizationContext())
        {
        }
        public SynchronizationCallback(Action<object> callback)
            : this(callback, SynchronizationContext.Current ?? new SynchronizationContext())
        {
        }
        public SynchronizationCallback(Action<SynchronizationContext, object> callback)
            : this(callback, SynchronizationContext.Current ?? new SynchronizationContext())
        {
        }
        private SynchronizationCallback(Delegate callback, SynchronizationContext context)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            ParameterInfo[] parameters = callback.Method.GetParameters();
            if (parameters.Length != 0 && (parameters[0].ParameterType != typeof(object) || parameters.Length > 1))
            {
                throw new ArgumentException("The delegate must have exactly one parameter of type object.", "callback");
            }
            Callback = callback;
            CallbackParameters = parameters.Length;
            Context = context;
        }
        public SynchronizationCallback(Action<SynchronizationContext, object> callback, SynchronizationContext context)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            Callback = callback;
            CallbackParameters = 2;
            Context = context;
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = (SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy))]
        public void Invoke()
        {
            Invoke(null);
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = (SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy))]
        public void Invoke(object state)
        {
            SynchronizationContext current = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(Context);
            try
            {
                switch (CallbackParameters)
                {
                    case 0:
                        Callback.DynamicInvoke(null);
                        break;
                    case 1:
                        Callback.DynamicInvoke(new object[]
					{
						state
					});
                        break;
                    case 2:
                        Callback.DynamicInvoke(new object[]
					{
						Context,
						state
					});
                        break;
                    default:
                        throw new InvalidProgramException("Delegate has an invalid number of callback parameters.");
                }
            }
            finally
            {
                if (SynchronizationContext.Current == Context)
                {
                    SynchronizationContext.SetSynchronizationContext(current);
                }
            }
        }
        public static implicit operator SynchronizationCallback(WaitCallback callback)
        {
            return new SynchronizationCallback(callback);
        }
        public static implicit operator SynchronizationCallback(TimerCallback callback)
        {
            return new SynchronizationCallback(callback);
        }
        public static implicit operator SynchronizationCallback(SendOrPostCallback callback)
        {
            return new SynchronizationCallback(callback);
        }
        public static implicit operator SynchronizationCallback(ThreadStart callback)
        {
            return new SynchronizationCallback(callback);
        }
        public static implicit operator SynchronizationCallback(ParameterizedThreadStart callback)
        {
            return new SynchronizationCallback(callback);
        }
        public static implicit operator SynchronizationCallback(Action callback)
        {
            return new SynchronizationCallback(callback);
        }
        public static implicit operator SynchronizationCallback(Action<object> callback)
        {
            return new SynchronizationCallback(callback);
        }
        public static implicit operator WaitCallback(SynchronizationCallback callback)
        {
            return new WaitCallback(callback.Invoke);
        }
        public static implicit operator TimerCallback(SynchronizationCallback callback)
        {
            return new TimerCallback(callback.Invoke);
        }
        public static implicit operator SendOrPostCallback(SynchronizationCallback callback)
        {
            return new SendOrPostCallback(callback.Invoke);
        }
        public static implicit operator ThreadStart(SynchronizationCallback callback)
        {
            return new ThreadStart(callback.Invoke);
        }
        public static implicit operator ParameterizedThreadStart(SynchronizationCallback callback)
        {
            return new ParameterizedThreadStart(callback.Invoke);
        }
        public static implicit operator Action(SynchronizationCallback callback)
        {
            return new Action(callback.Invoke);
        }
        public static implicit operator Action<object>(SynchronizationCallback callback)
        {
            return new Action<object>(callback.Invoke);
        }
        public WaitCallback ToWaitCallback()
        {
            return new WaitCallback(Invoke);
        }
        public TimerCallback ToTimerCallback()
        {
            return new TimerCallback(Invoke);
        }
        public SendOrPostCallback ToSendOrPostCallback()
        {
            return new SendOrPostCallback(Invoke);
        }
        public ThreadStart ToThreadStart()
        {
            return new ThreadStart(Invoke);
        }
        public ParameterizedThreadStart ToParameterizedThreadStart()
        {
            return new ParameterizedThreadStart(Invoke);
        }
        public Action ToAction()
        {
            return new Action(Invoke);
        }
        public Action<object> ToObjectAction()
        {
            return new Action<object>(Invoke);
        }
    }
}
