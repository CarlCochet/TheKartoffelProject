using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace WorldEditor.Editors.Files.D2I
{
    public abstract class D2IGridRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public abstract string Text
        {
            get;
            set;
        }

        private RowState _state;
        public RowState State
        {
            get
			{
                return _state;
			}
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
        public void OnPropertyChanged(string property)
        {
            if (property == "Text" || property == "Id")
            {
                State = RowState.Dirty;
            }
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
        public abstract string GetKey();
    }
}
