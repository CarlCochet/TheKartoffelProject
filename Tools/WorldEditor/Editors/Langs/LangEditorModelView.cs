using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldEditor.Helpers;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Editors.Langs
{
    public class LangEditorModelView : INotifyPropertyChanged
    {
        private readonly LangEditor m_editor;
        private uint m_highestId;
        private Stack<LangGridRow> m_deletedRows = new Stack<LangGridRow>();
        private ReadOnlyObservableCollection<LangGridRow> m_readOnylRows;
        private ObservableCollection<LangGridRow> m_rows = new ObservableCollection<LangGridRow>();
        private DelegateCommand m_findCommand;
        private string m_searchText;
        private DelegateCommand m_findNextCommand;
        private DelegateCommand m_addRowCommand;
        private DelegateCommand m_addUIRowCommand;
        private DelegateCommand m_removeRowCommand;
        private DelegateCommand m_saveCommand;
        public event PropertyChangedEventHandler PropertyChanged;
        private string _searchType;
        private int _lastFoundIndex;

        public ReadOnlyObservableCollection<LangGridRow> Rows
        {
            get { return this.m_readOnylRows; }
        }

        public string Title
        {
            get { return "Lang Editor"; }
        }

        public DelegateCommand FindCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = this.m_findCommand) == null)
                {
                    arg_31_0 = (this.m_findCommand = new DelegateCommand(this.OnFind, this.CanFind));
                }
                return arg_31_0;
            }
        }

        public string SearchText
        {
            get { return this.m_searchText; }
            set
            {
                if (string.Equals(this.m_searchText, value, StringComparison.Ordinal))
                {
                    return;
                }
                this.m_searchText = value;
                this.OnPropertyChanged("FindCommand");
                this.OnPropertyChanged("FindNextCommand");
                this.OnPropertyChanged("SearchText");
            }
        }

        public string SearchType
        {
            get { return "French"; }
            set
            {
                if (string.Equals(_searchType, value, StringComparison.Ordinal))
                {
                    return;
                }
                _searchType = value;
                this.OnPropertyChanged("FindCommand");
                this.OnPropertyChanged("FindNextCommand");
                this.OnPropertyChanged("SearchType");
            }
        }

        public int LastFoundIndex
        {
            get { return this._lastFoundIndex; }
            set
            {
                if (this._lastFoundIndex == value)
                {
                    return;
                }
                this._lastFoundIndex = value;
                this.OnPropertyChanged("FindCommand");
                this.OnPropertyChanged("FindNextCommand");
                this.OnPropertyChanged("LastFoundIndex");
            }
        }

        public DelegateCommand FindNextCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = this.m_findNextCommand) == null)
                {
                    arg_31_0 = (this.m_findNextCommand = new DelegateCommand(this.OnFindNext, this.CanFindNext));
                }
                return arg_31_0;
            }
        }

        public DelegateCommand AddRowCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = this.m_addRowCommand) == null)
                {
                    arg_31_0 = (this.m_addRowCommand = new DelegateCommand(this.OnAddRow, this.CanAddRow));
                }
                return arg_31_0;
            }
        }

        public DelegateCommand AddUIRowCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = this.m_addUIRowCommand) == null)
                {
                    arg_31_0 = (this.m_addUIRowCommand = new DelegateCommand(this.OnAddUIRow, this.CanAddUIRow));
                }
                return arg_31_0;
            }
        }

        public DelegateCommand RemoveRowCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = this.m_removeRowCommand) == null)
                {
                    arg_31_0 = (this.m_removeRowCommand = new DelegateCommand(this.OnRemoveRow, this.CanRemoveRow));
                }
                return arg_31_0;
            }
        }

        public DelegateCommand SaveCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = this.m_saveCommand) == null)
                {
                    arg_31_0 = (this.m_saveCommand = new DelegateCommand(this.OnSave, LangEditorModelView.CanSave));
                }
                return arg_31_0;
            }
        }

        public LangEditorModelView(LangEditor editor)
        {
            this.m_editor = editor;
            this.m_readOnylRows = new ReadOnlyObservableCollection<LangGridRow>(this.m_rows);
            this.LastFoundIndex = -1;
            this.Open();
        }

        private void Open()
        {
            ReadOnlyDictionary<string, LangTextUi> uiTexts = I18NDataManager.Instance.LangsUi;
            foreach (KeyValuePair<string, LangTextUi> uiText in uiTexts)
            {
                this.m_rows.Add(new LangTextUiRow(uiText.Value.Copy()));
            }
            ReadOnlyDictionary<uint, LangText> texts = I18NDataManager.Instance.Langs;
            foreach (KeyValuePair<uint, LangText> text in texts)
            {
                this.m_rows.Add(new LangTextRow(text.Value.Copy()));
            }
            this.m_highestId = texts.Keys.Max<uint>();
        }

        private LangGridRow FindNext()
        {
            int startIndex = (this.LastFoundIndex == -1 || this.LastFoundIndex + 1 >= this.Rows.Count) ? 0 : (this.LastFoundIndex + 1);
            LangGridRow row = null;
            int index = -1;
            if (this.SearchType == "Key")
            {
                bool isNumber = this.SearchText.All(new Func<char, bool>(char.IsDigit));
                if (isNumber)
                {
                    int id = int.Parse(this.SearchText);
                    for (int i = startIndex; i < this.Rows.Count; i++)
                    {
                        if (this.Rows[i] is LangTextRow && (ulong)(this.Rows[i] as LangTextRow).Id == (ulong)((long)id))
                        {
                            row = this.Rows[i];
                            index = i;
                            break;
                        }
                    }
                }
                if (row == null)
                {
                    for (int i = startIndex; i < this.Rows.Count; i++)
                    {
                        if (this.Rows[i].GetKey().IndexOf(this.SearchText, StringComparison.InvariantCultureIgnoreCase) != -1)
                        {
                            row = this.Rows[i];
                            index = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                Languages lang = (Languages)Enum.Parse(typeof(Languages), this.SearchType);
                for (int i = startIndex; i < this.Rows.Count; i++)
                {
                    if (this.Rows[i].DoesContainText(this.SearchText, lang))
                    {
                        row = this.Rows[i];
                        index = i;
                        break;
                    }
                }
            }
            LangGridRow result;
            if (row == null)
            {
                this.LastFoundIndex = -1;
                result = null;
            }
            else
            {
                this.LastFoundIndex = index;
                result = row;
            }
            return result;
        }

        private bool CanFind(object parameter)
        {
            return !string.IsNullOrEmpty(this.m_searchText);
        }

        private void OnFind(object parameter)
        {
            if (this.CanFind(parameter))
            {
                this.LastFoundIndex = 0;
                LangGridRow row = this.FindNext();
                this.FindNextCommand.RaiseCanExecuteChanged();
                if (row != null)
                {
                    this.m_editor.TextsGrid.SelectedItem = row;
                    this.m_editor.TextsGrid.ScrollIntoView(row);
                    this.m_editor.TextsGrid.Focus();
                }
                else
                {
                    MessageService.ShowMessage(this.m_editor, "Not found");
                }
            }
        }

        private bool CanFindNext(object parameter)
        {
            return this.LastFoundIndex != -1;
        }

        private void OnFindNext(object parameter)
        {
            LangGridRow row = this.FindNext();
            if (row == null)
            {
                row = this.FindNext();
            }
            if (row != null)
            {
                this.m_editor.TextsGrid.SelectedItem = row;
                this.m_editor.TextsGrid.ScrollIntoView(row);
                this.m_editor.TextsGrid.Focus();
            }
            else
            {
                MessageService.ShowMessage(this.m_editor, "Not found");
            }
        }

        private bool CanAddRow(object parameter)
        {
            return true;
        }

        private void OnAddRow(object parameter)
        {
            LangTextRow row = new LangTextRow(new LangText
            {
                Id = this.m_highestId += 1u
            });
            row.State = RowState.Added;
            this.m_rows.Add(row);
            this.m_editor.TextsGrid.SelectedItem = row;
            this.m_editor.TextsGrid.ScrollIntoView(row);
            this.m_editor.TextsGrid.Focus();
        }

        private bool CanAddUIRow(object parameter)
        {
            return true;
        }

        private void OnAddUIRow(object parameter)
        {
            LangTextUiRow row = new LangTextUiRow(new LangTextUi());
            row.State = RowState.Added;
            this.m_rows.Add(row);
            this.m_editor.TextsGrid.SelectedItem = row;
            this.m_editor.TextsGrid.ScrollIntoView(row);
            this.m_editor.TextsGrid.Focus();
        }

        private bool CanRemoveRow(object parameter)
        {
            return parameter is IList && (parameter as IList).Count > 0;
        }

        private void OnRemoveRow(object parameter)
        {
            if (parameter != null && this.CanRemoveRow(parameter))
            {
                LangGridRow[] rows = (parameter as IList).OfType<LangGridRow>().ToArray<LangGridRow>();
                LangGridRow[] array = rows;
                for (int i = 0; i < array.Length; i++)
                {
                    LangGridRow row = array[i];
                    if (this.m_rows.Remove(row))
                    {
                        this.m_deletedRows.Push(row);
                    }
                    row.State = RowState.Removed;
                }
            }
        }

        private static bool CanSave(object parameter)
        {
            return true;
        }

        private void OnSave(object parameter)
        {
            foreach (LangGridRow row in this.m_rows)
            {
                row.Save();
            }
            foreach (LangGridRow row in this.m_deletedRows)
            {
                row.Save();
            }
            I18NDataManager.Instance.Save();
            MessageService.ShowMessage(this.m_editor, "Saved ! ");
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}