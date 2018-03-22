using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Editors.Langs
{
    public abstract class LangGridRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private RowState _state;

        public abstract string French { get; set; }

        public abstract string English { get; set; }

        public abstract string German { get; set; }

        public abstract string Spanish { get; set; }

        public abstract string Italian { get; set; }

        public abstract string Japanish { get; set; }

        public abstract string Dutsh { get; set; }

        public abstract string Portugese { get; set; }

        public abstract string Russish { get; set; }

        public RowState State
        {
            get { return this._state; }
            set
            {
                if (this._state == value)
                {
                    return;
                }
                _state = value;
                this.OnPropertyChanged("State");
            }
        }

        public bool DoesContainText(string text)
        {
            return (!string.IsNullOrEmpty(this.French) && this.French.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (!string.IsNullOrEmpty(this.English) && this.English.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (!string.IsNullOrEmpty(this.Portugese) && this.Portugese.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (!string.IsNullOrEmpty(this.Russish) && this.Russish.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (!string.IsNullOrEmpty(this.German) && this.German.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (!string.IsNullOrEmpty(this.Japanish) && this.Japanish.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (!string.IsNullOrEmpty(this.Dutsh) && (this.Dutsh.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 | !string.IsNullOrEmpty(this.Spanish)) && this.Spanish.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (!string.IsNullOrEmpty(this.Italian) && this.Italian.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1);
        }

        public bool DoesContainText(string text, Languages lang)
        {
            if (lang == Languages.All)
            {
                return this.DoesContainText(text);
            }
            else
            {
                return ((lang == Languages.French && !string.IsNullOrEmpty(this.French) && this.French.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (lang == Languages.English && !string.IsNullOrEmpty(this.English) && this.English.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (lang == Languages.Portugese && !string.IsNullOrEmpty(this.Portugese) && this.Portugese.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (lang == Languages.Russish && !string.IsNullOrEmpty(this.Russish) && this.Russish.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (lang == Languages.German && !string.IsNullOrEmpty(this.German) && this.German.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (lang == Languages.Japanish && !string.IsNullOrEmpty(this.Japanish) && this.Japanish.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (lang == Languages.Dutsh && !string.IsNullOrEmpty(this.Dutsh) && this.Dutsh.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (lang == Languages.Spanish && !string.IsNullOrEmpty(this.Spanish) && this.Spanish.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) || (lang == Languages.Italian && !string.IsNullOrEmpty(this.Italian) && this.Italian.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1));
            }
        }

        public void OnPropertyChanged(string property)
        {
            if (this.State == RowState.None)
            {
                this.State = RowState.Dirty;
            }
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        public abstract string GetKey();

        public abstract void Save();
    }
}