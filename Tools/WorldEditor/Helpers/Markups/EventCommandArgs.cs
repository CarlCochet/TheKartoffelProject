using System;
using System.Windows;

namespace WorldEditor.Helpers.Markups
{
    public sealed class EventCommandArgs<TEventArgs> where TEventArgs : RoutedEventArgs
    {
        public TEventArgs EventArgs { get; private set; }

        public object Sender { get; private set; }

        public EventCommandArgs(object sender, TEventArgs args)
        {
            Sender = sender;
            EventArgs = args;
        }
    }
}