using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WorldEditor.Helpers;
//using WorldEditor.Loaders.I18N;

namespace WorldEditor.Editors.Files.D2I
{
    public class D2IEditorModelView : INotifyPropertyChanged
    {
        private readonly D2IEditor m_editor;
        private readonly Stump.DofusProtocol.D2oClasses.Tools.D2i.D2IFile m_file;
        private int m_highestId;
        private Stack<D2IGridRow> m_deletedRows = new Stack<D2IGridRow>();
        private ReadOnlyObservableCollection<D2IGridRow> m_readOnylRows;
        private ObservableCollection<D2IGridRow> m_rows = new ObservableCollection<D2IGridRow>();
        private string m_searchText;
        public event PropertyChangedEventHandler PropertyChanged;
        private string _searchType;
        private int _lastFoundIndex;

        public ReadOnlyObservableCollection<D2IGridRow> Rows
        {
            get { return m_readOnylRows; }
        }

        public string Title
        {
            get { return "D2I Editor : " + Path.GetFileName(m_file.FilePath); }
        }

        public string SearchText
        {
            get { return m_searchText; }
            set
            {
                if (string.Equals(m_searchText, value, StringComparison.Ordinal))
                {
                    return;
                }
                m_searchText = value;
                OnPropertyChanged("FindCommand");
                OnPropertyChanged("FindNextCommand");
                OnPropertyChanged("SaveCommand");
                OnPropertyChanged("SaveAsCommand");
                OnPropertyChanged("SearchText");
            }
        }

        public string SearchType
        {
            get { return _searchType; }
            set
            {
                if (string.Equals(_searchType, value, StringComparison.Ordinal))
                {
                    return;
                }
                _searchType = value;
                OnPropertyChanged("FindCommand");
                OnPropertyChanged("FindNextCommand");
                OnPropertyChanged("SaveCommand");
                OnPropertyChanged("SaveAsCommand");
                OnPropertyChanged("SearchType");
            }
        }

        public int LastFoundIndex
        {
            get { return _lastFoundIndex; }
            set
            {
                if (_lastFoundIndex == value)
                {
                    return;
                }
                _lastFoundIndex = value;
                OnPropertyChanged("FindCommand");
                OnPropertyChanged("FindNextCommand");
                OnPropertyChanged("SaveCommand");
                OnPropertyChanged("SaveAsCommand");
                OnPropertyChanged("LastFoundIndex");
            }
        }

        public DelegateCommand FindCommand { get; private set; }

        public DelegateCommand FindNextCommand { get; private set; }

        public DelegateCommand AddRowCommand { get; private set; }

        public DelegateCommand AddUIRowCommand { get; private set; }

        public DelegateCommand RemoveRowCommand { get; private set; }

        public DelegateCommand ConvertToTxtCommand { get; private set; }

        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand SaveAsCommand{ get; private set; }

        public D2IEditorModelView(D2IEditor editor, Stump.DofusProtocol.D2oClasses.Tools.D2i.D2IFile file)
        {
            m_editor = editor;
            m_file = file;
            FindCommand = new DelegateCommand(OnFind, CanFind);
            FindNextCommand = new DelegateCommand(OnFindNext, CanFindNext);
            AddRowCommand = new DelegateCommand(OnAddRow, CanAddRow);
            AddUIRowCommand = new DelegateCommand(OnAddUIRow, CanAddUIRow);
            RemoveRowCommand = new DelegateCommand(OnRemoveRow, CanRemoveRow);
            ConvertToTxtCommand = new DelegateCommand(OnConvertToTxt, CanConvertToTxt);
            SaveCommand = new DelegateCommand(OnSave, CanSave);
            SaveAsCommand = new DelegateCommand(OnSaveAs, CanSaveAs);
            m_readOnylRows = new ReadOnlyObservableCollection<D2IGridRow>(m_rows);
            LastFoundIndex = -1;
            Open();
        }

        private void Open()
        {
            foreach (var uiText in m_file.GetAllUiText())
            {
                m_rows.Add(new D2ITextUiRow(uiText.Key, uiText.Value));
            }
            foreach (var text in m_file.GetAllText())
            {
                m_rows.Add(new D2ITextRow(text.Key, text.Value));
            }
            m_highestId = m_file.GetAllText().Keys.Max();
        }

        private D2IGridRow FindNext()
        {
            int startIndex = (LastFoundIndex == -1 || LastFoundIndex + 1 >= Rows.Count) ? 0 : (LastFoundIndex + 1);
            D2IGridRow row = null;
            int index = -1;
            string searchType = SearchType;
            if (!string.IsNullOrEmpty(searchType))
            {
                if (searchType == "Text")
                {
                    for (int i = startIndex; i < Rows.Count; i++)
                    {
                        if (Rows[i].Text.IndexOf(SearchText, StringComparison.InvariantCultureIgnoreCase) != -1)
                        {
                            row = Rows[i];
                            index = i;
                            break;
                        }
                    }
                }
                else if (searchType == "Key")
                {
                    if (SearchText.All(char.IsDigit))
                    {
                        int id = int.Parse(SearchText);
                        for (int i = startIndex; i < Rows.Count; i++)
                        {
                            if (Rows[i] is D2ITextRow && (Rows[i] as D2ITextRow).Id == id)
                            {
                                row = Rows[i];
                                index = i;
                                break;
                            }
                        }
                    }
                }
                if (row == null)
                {
                    for (int i = startIndex; i < Rows.Count; i++)
                    {
                        if (Rows[i].GetKey().IndexOf(SearchText, StringComparison.InvariantCultureIgnoreCase) != -1)
                        {
                            row = Rows[i];
                            index = i;
                            break;
                        }
                    }
                }
            }
            if (row == null)
            {
                LastFoundIndex = -1;
                return null;
            }
            else
            {
                LastFoundIndex = index;
                return row;
            }
        }

        private bool CanFind(object parameter)
        {
            return !string.IsNullOrEmpty(m_searchText);
        }

        private void OnFind(object parameter)
        {
            if (CanFind(parameter))
            {
                LastFoundIndex = 0;
                var row = FindNext();
                FindNextCommand.RaiseCanExecuteChanged();
                if (row != null)
                {
                    m_editor.TextsGrid.SelectedItem = row;
                    m_editor.TextsGrid.ScrollIntoView(row);
                    m_editor.TextsGrid.Focus();
                }
                else
                {
                    MessageService.ShowMessage(m_editor, "Not found");
                }
            }
        }

        private bool CanFindNext(object parameter)
        {
            return LastFoundIndex != -1;
        }

        private void OnFindNext(object parameter)
        {
            var row = FindNext();
            if (row == null)
            {
                row = FindNext();
            }
            if (row != null)
            {
                m_editor.TextsGrid.SelectedItem = row;
                m_editor.TextsGrid.ScrollIntoView(row);
                m_editor.TextsGrid.Focus();
            }
            else
            {
                MessageService.ShowMessage(m_editor, "Not found");
            }
        }

        private bool CanAddRow(object parameter)
        {
            return true;
        }

        private void OnAddRow(object parameter)
        {
            D2ITextRow row = new D2ITextRow(++m_highestId, string.Empty);
            row.State = RowState.Added;
            m_rows.Add(row);
            m_editor.TextsGrid.SelectedItem = row;
            m_editor.TextsGrid.ScrollIntoView(row);
            m_editor.TextsGrid.Focus();
        }

        private bool CanAddUIRow(object parameter)
        {
            return true;
        }

        private void OnAddUIRow(object parameter)
        {
            D2ITextUiRow row = new D2ITextUiRow(string.Empty, string.Empty);
            row.State = RowState.Added;
            m_rows.Add(row);
            m_editor.TextsGrid.SelectedItem = row;
            m_editor.TextsGrid.ScrollIntoView(row);
            m_editor.TextsGrid.Focus();
        }

        private bool CanRemoveRow(object parameter)
        {
            return parameter is IList && (parameter as IList).Count > 0;
        }

        private void OnRemoveRow(object parameter)
        {
            if (parameter != null && CanRemoveRow(parameter))
            {
                foreach(var row in (parameter as IList).OfType<D2IGridRow>())
                {
                    if (m_rows.Remove(row))
                    {
                        m_deletedRows.Push(row);
                    }
                    row.State = RowState.Removed;
                }
            }
        }

        private bool CanConvertToTxt(object parameter)
        {
            return true;
        }

        private void OnConvertToTxt(object parameter)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                FileName = Path.GetFileNameWithoutExtension(m_file.FilePath) + ".txt",
                InitialDirectory = Path.GetDirectoryName(m_file.FilePath),
                Title = "Convert to ...",
                DefaultExt = ".txt",
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                OverwritePrompt = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                int uiPadding = Rows.OfType<D2ITextUiRow>().Max((D2ITextUiRow x) => x.GetKey().Length + 1);
                int textPadding = Rows.OfType<D2ITextRow>().Max((D2ITextRow x) => x.GetKey().Length + 1);
                using (FileStream stream = File.Open(filePath, FileMode.Append, FileAccess.Write))
                {
                    StreamWriter writer = new StreamWriter(stream);
                    foreach (D2IGridRow row in Rows)
                    {
                        int padding = (row is D2ITextRow) ? textPadding : uiPadding;
                        writer.WriteLine("{0}{1}", row.GetKey().PadRight(padding), row.Text.Replace("\n", "\n" + new string(' ', padding)));
                    }
                }
                MessageService.ShowMessage(m_editor, string.Format("File converted to {0}", Path.GetFileName(filePath)));
            }
        }

        private static bool CanSave(object parameter)
        {
            return true;
        }

        private void OnSave(object parameter)
        {
            PerformSave(m_file.FilePath);
        }

        private bool CanSaveAs(object parameter)
        {
            return true;
        }

        private void OnSaveAs(object parameter)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                FileName = m_file.FilePath,
                Title = "Save file as ...",
                DefaultExt = ".d2i",
                OverwritePrompt = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                PerformSave(filePath);
            }
        }

        private void PerformSave(string filePath)
        {
            try
            {
                if (UpdateFile())
                {
                    m_file.Save(filePath);
                    MessageService.ShowMessage(m_editor, "File saved successfully");
                }
            }
            catch (IOException ex)
            {
                MessageService.ShowError(m_editor, "Cannot perform save : " + ex.Message);
            }
            catch (Exception ex2)
            {
                MessageService.ShowError(m_editor, "Cannot perform save : " + ex2);
            }
        }

        private bool UpdateFile()
        {
            foreach (var row in Rows.Where(entry => entry.State == RowState.Dirty || entry.State == RowState.Added))
            {
                if (Rows.Count(entry => entry.GetKey() == row.GetKey()) > 1)
                {
                    if (!MessageService.ShowYesNoQuestion(m_editor, string.Format("WARNING ! Found duplicated keys '{0}'. The file may save but a row will be deleted", row.GetKey()) + "Continue saving anyway ?"))
                    {
                        SearchType = "Key";
                        SearchText = row.GetKey();
                        OnFindNext(null);
                        return false;
                    }
                }
                if (row is D2ITextRow)
                {
                    m_file.SetText((row as D2ITextRow).Id, row.Text);
                }
                if (row is D2ITextUiRow)
                {
                    m_file.SetText((row as D2ITextUiRow).Id, row.Text);
                }
                row.State = RowState.None;
            }
            while (m_deletedRows.Count > 0)
            {
                var row = m_deletedRows.Pop();
                if (row is D2ITextRow)
                {
                    m_file.DeleteText((row as D2ITextRow).Id);
                }
                if (row is D2ITextUiRow)
                {
                    m_file.DeleteText((row as D2ITextUiRow).Id);
                }
            }
            return true;
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