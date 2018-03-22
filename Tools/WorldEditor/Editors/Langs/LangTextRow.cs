using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Editors.Langs
{
    public sealed class LangTextRow : LangGridRow
    {
        private readonly LangText m_record;

        public uint Id
        {
            get { return this.m_record.Id; }
            set
            {
                if (this.Id == value)
                {
                    return;
                }
                this.m_record.Id = value;
                this.OnPropertyChanged("Id");
            }
        }

        public override string French
        {
            get { return this.m_record.French; }
            set
            {
                if (string.Equals(this.French, value, StringComparison.Ordinal))
                {
                    return;
                }
                this.m_record.French = value;
                this.OnPropertyChanged("French");
            }
        }

        public override string English
        {
            get { return this.m_record.English; }
            set
            {
                if (string.Equals(this.English, value, StringComparison.Ordinal))
                {
                    return;
                }
                this.m_record.English = value;
                this.OnPropertyChanged("English");
            }
        }

        public override string German
        {
            get { return this.m_record.German; }
            set
            {
                if (string.Equals(this.German, value, StringComparison.Ordinal))
                {
                    return;
                }
                this.m_record.German = value;
                this.OnPropertyChanged("German");
            }
        }

        public override string Spanish
        {
            get { return this.m_record.Spanish; }
            set
            {
                if (string.Equals(this.Spanish, value, StringComparison.Ordinal))
                {
                    return;
                }
                this.m_record.Spanish = value;
                this.OnPropertyChanged("Spanish");
            }
        }

        public override string Italian
        {
            get { return this.m_record.Italian; }
            set
            {
                if (string.Equals(this.Italian, value, StringComparison.Ordinal))
                {
                    return;
                }
                this.m_record.Italian = value;
                this.OnPropertyChanged("Italian");
            }
        }

        public override string Japanish
        {
            get { return this.m_record.Japanish; }
            set
            {
                if (string.Equals(this.Japanish, value, StringComparison.Ordinal))
                {
                    return;
                }
                this.m_record.Japanish = value;
                this.OnPropertyChanged("Japanish");
            }
        }

        public override string Dutsh
        {
            get { return this.m_record.Dutsh; }
            set
            {
                if (string.Equals(this.Dutsh, value, StringComparison.Ordinal))
                {
                    return;
                }
                this.m_record.Dutsh = value;
                this.OnPropertyChanged("Dutsh");
            }
        }

        public override string Portugese
        {
            get { return this.m_record.Portugese; }
            set
            {
                if (string.Equals(this.Portugese, value, StringComparison.Ordinal))
                {
                    return;
                }
                this.m_record.Portugese = value;
                this.OnPropertyChanged("Portugese");
            }
        }

        public override string Russish
        {
            get { return this.m_record.Russish; }
            set
            {
                if (string.Equals(this.Russish, value, StringComparison.Ordinal))
                {
                    return;
                }
                this.m_record.Russish = value;
                this.OnPropertyChanged("Russish");
            }
        }

        public LangTextRow()
        {
        }

        public LangTextRow(LangText record)
        {
            this.m_record = record;
        }

        public override string GetKey()
        {
            return this.Id.ToString();
        }

        public override void Save()
        {
            switch (base.State)
            {
                case RowState.Dirty:
                    I18NDataManager.Instance.SaveText(this.m_record);
                    break;
                case RowState.Added:
                    I18NDataManager.Instance.CreateText(this.m_record);
                    break;
                case RowState.Removed:
                    I18NDataManager.Instance.DeleteText(this.m_record);
                    break;
            }
            base.State = RowState.None;
        }
    }
}
