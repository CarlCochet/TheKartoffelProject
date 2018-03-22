using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorldEditor.Editors.Files.D2O;

namespace WorldEditor.Editors
{
    public class EditedObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private object _objects;
        public object Object
        {
            get { return _objects; }
            private set
			{
				if (object.Equals(_objects, value))
				{
					return;
				}
                _objects = value;
				OnPropertyChanged("Object");
			}
        }

        private ObjectState _state;
        public ObjectState State
        {
            get { return _state; }
            set
			{
				if (_state == value)
				{
					return;
				}
                _state = value;
				OnPropertyChanged("State");
			}
        }

        public EditedObject(object o, ObjectState state)
        {
            Object = o;
            State = state;
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
