using System;
using System.ComponentModel;
namespace WorldEditor.Editors.Files.D2P
{
    public abstract class D2PGridRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public abstract string Name
        {
            get;
            set;
        }
        public abstract string Type
        {
            get;
        }
        public abstract bool HasSize
        {
            get;
        }
        public abstract int Size
        {
            get;
        }
        public abstract string Container
        {
            get;
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
