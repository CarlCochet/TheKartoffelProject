using Microsoft.Win32;
using WorldEditor.Loaders.D2p;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WorldEditor.Helpers;

namespace WorldEditor.Editors.Files.D2P
{
    public class D2PEditorModelView : INotifyPropertyChanged
    {
        private readonly D2PEditor m_editor;
        private readonly D2pFile m_file;
        private ReadOnlyObservableCollection<D2PGridRow> m_readOnylRows;
        private ObservableCollection<D2PGridRow> m_rows = new ObservableCollection<D2PGridRow>();
        private DelegateCommand m_exploreFolderCommand;
        private DelegateCommand m_addFileCommand;
        private DelegateCommand m_removeFileCommand;
        private DelegateCommand m_extractCommand;
        private DelegateCommand m_extractAllCommand;
        private DelegateCommand m_saveCommand;
        private DelegateCommand m_saveAsCommand;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get            {                return "D2P Editor : " + File.FilePath + ((CurrentFolder == null) ? string.Empty : (" - " + CurrentFolder.FullName));            }
        }

        public D2pFile File
        {          
            get            {                return m_file;            }
        }

        public ReadOnlyObservableCollection<D2PGridRow> Rows
        {
            get            {                return m_readOnylRows;            }
        }

        private D2pDirectory _currentFolder;
        public D2pDirectory CurrentFolder
        {
            get			{                return _currentFolder;			}
            private set
			{
				if (_currentFolder == value)
				{
					return;
				}
                _currentFolder = value;
				OnPropertyChanged("Title");
				OnPropertyChanged("AddFileCommand");
				OnPropertyChanged("CurrentFolder");
			}
        }
        public DelegateCommand ExploreFolderCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = m_exploreFolderCommand) == null)
                {
                    arg_31_0 = (m_exploreFolderCommand = new DelegateCommand(new Action<object>(OnExploreFolder), (CanExploreFolder)));
                }
                return arg_31_0;
            }
        }
        public DelegateCommand AddFileCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = m_addFileCommand) == null)
                {
                    arg_31_0 = (m_addFileCommand = new DelegateCommand(new Action<object>(OnAddFile), (CanAddFile)));
                }
                return arg_31_0;
            }
        }
        public DelegateCommand RemoveFileCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = m_removeFileCommand) == null)
                {
                    arg_31_0 = (m_removeFileCommand = new DelegateCommand(new Action<object>(OnRemoveFile), (CanRemoveFile)));
                }
                return arg_31_0;
            }
        }
        public DelegateCommand ExtractCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = m_extractCommand) == null)
                {
                    arg_31_0 = (m_extractCommand = new DelegateCommand(new Action<object>(OnExtract), (CanExtract)));
                }
                return arg_31_0;
            }
        }
        public DelegateCommand ExtractAllCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = m_extractAllCommand) == null)
                {
                    arg_31_0 = (m_extractAllCommand = new DelegateCommand(new Action<object>(OnExtractAll), (CanExtractAll)));
                }
                return arg_31_0;
            }
        }
        public DelegateCommand SaveCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = m_saveCommand) == null)
                {
                    arg_31_0 = (m_saveCommand = new DelegateCommand(new Action<object>(OnSave), (D2PEditorModelView.CanSave)));
                }
                return arg_31_0;
            }
        }
        public DelegateCommand SaveAsCommand
        {
            get
            {
                DelegateCommand arg_31_0;
                if ((arg_31_0 = m_saveAsCommand) == null)
                {
                    arg_31_0 = (m_saveAsCommand = new DelegateCommand(new Action<object>(OnSaveAs), (CanSaveAs)));
                }
                return arg_31_0;
            }
        }
        public D2PEditorModelView(D2PEditor editor, D2pFile file)
        {
            m_editor = editor;
            m_file = file;
            m_readOnylRows = new ReadOnlyObservableCollection<D2PGridRow>(m_rows);
            Open(null);
        }
        private void Open(D2pDirectory directory)
        {
            m_rows.Clear();
            List<D2PGridRow> rows = new List<D2PGridRow>();
            if (directory == null)
            {
                rows.AddRange(
                    from rootDirectory in m_file.RootDirectories
                    select new D2PFolderRow(rootDirectory));
                rows.AddRange(
                    from entry in m_file.Entries
                    where entry.Directory == null
                    select new D2PFileRow(entry));
            }
            else
            {
                rows.Add(new D2PLastFolderRow(directory.Parent));
                rows.AddRange(
                    from subFolder in directory.Directories
                    select new D2PFolderRow(subFolder));
                rows.AddRange(
                    from entry in directory.Entries
                    select new D2PFileRow(entry));
            }
            CurrentFolder = directory;
            m_readOnylRows = new ReadOnlyObservableCollection<D2PGridRow>(m_rows = new ObservableCollection<D2PGridRow>(rows));
            OnPropertyChanged("Rows");
        }
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        private bool CanExploreFolder(object parameter)
        {
            return parameter is D2PFolderRow;
        }
        private void OnExploreFolder(object parameter)
        {
            if (parameter != null && CanExploreFolder(parameter))
            {
                D2PFolderRow folderRow = (D2PFolderRow)parameter;
                Open(folderRow.Folder);
            }
        }
        private bool CanAddFile(object parameter)
        {
            return true;
        }
        private void OnAddFile(object parameter)
        {
            if (CanAddFile(parameter))
            {
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "*|*",
                    CheckPathExists = true,
                    Title = "Select the files to add",
                    Multiselect = true
                };
                if (!(dialog.ShowDialog() != true))
                {
                    string[] fileNames = dialog.FileNames;
                    for (int i = 0; i < fileNames.Length; i++)
                    {
                        string filePath = fileNames[i];
                        AddFile(filePath);
                    }
                }
            }
        }
        public void AddFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                MessageService.ShowError(m_editor, string.Format("Cannot add file : File {0} not found", filePath));
            }
            else
            {
                string packedFileName = (CurrentFolder != null) ? Path.Combine(CurrentFolder.FullName, Path.GetFileName(filePath)) : Path.GetFileName(filePath);
                D2pEntry entry = File.AddFile(packedFileName, System.IO.File.ReadAllBytes(filePath));
                D2PFileRow row = new D2PFileRow(entry);
                m_rows.Add(row);
              //  m_editor.FilesGrid.ScrollIntoView(row);
            }
        }
        private bool CanRemoveFile(object parameter)
        {
            bool arg_42_0;
            if (parameter != null && parameter is IList)
            {
                arg_42_0 = ((parameter as IList).OfType<D2PGridRow>().Count((D2PGridRow x) => !(x is D2PLastFolderRow)) > 0);
            }
            else
            {
                arg_42_0 = false;
            }
            return arg_42_0;
        }
        private void OnRemoveFile(object parameter)
        {
            if (parameter != null && CanRemoveFile(parameter))
            {
                D2PGridRow[] rows = (parameter as IList).OfType<D2PGridRow>().ToArray<D2PGridRow>();
                D2PGridRow[] array = rows;
                for (int i = 0; i < array.Length; i++)
                {
                    D2PGridRow row = array[i];
                    if (row is D2PFileRow)
                    {
                        D2PFileRow fileRow = row as D2PFileRow;
                        if (!File.RemoveEntry(fileRow.Entry))
                        {
                            MessageService.ShowError(m_editor, "Cannot remove this file. Unknown reason");
                            break;
                        }
                        m_rows.Remove(fileRow);
                    }
                    else
                    {
                        if (row is D2PFolderRow)
                        {
                            D2PFolderRow folderRow = row as D2PFolderRow;
                            if (folderRow.Folder.Entries.Count((D2pEntry entry) => !File.RemoveEntry(entry)) > 0)
                            {
                                MessageService.ShowError(m_editor, "Some files cannot be removed. Unknow reason.");
                                break;
                            }
                            m_rows.Remove(folderRow);
                        }
                    }
                }
            }
        }
        private bool CanExtract(object parameter)
        {
            bool arg_42_0;
            if (parameter != null && parameter is IList)
            {
                arg_42_0 = ((parameter as IList).OfType<D2PGridRow>().Count((D2PGridRow x) => !(x is D2PLastFolderRow)) > 0);
            }
            else
            {
                arg_42_0 = false;
            }
            return arg_42_0;
        }
        private void OnExtract(object parameter)
        {
            if (parameter != null && CanExtract(parameter))
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog
                {
                    ShowNewFolderButton = true,
                    Description = "Select a folder where to extract these files ..."
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    D2PGridRow[] rows = (parameter as IList).OfType<D2PGridRow>().ToArray<D2PGridRow>();
                    D2PGridRow[] array = rows;
                    for (int i = 0; i < array.Length; i++)
                    {
                        D2PGridRow row = array[i];
                        if (row is D2PFileRow)
                        {
                            File.ExtractFile((row as D2PFileRow).Entry.FullFileName, dialog.SelectedPath, false);
                        }
                        else
                        {
                            if (row is D2PFolderRow)
                            {
                                File.ExtractDirectory((row as D2PFolderRow).Folder.FullName, dialog.SelectedPath);
                            }
                        }
                    }
                    MessageService.ShowMessage(m_editor, "Files extracted");
                }
            }
        }
        private bool CanExtractAll(object parameter)
        {
            return true;
        }
        private void OnExtractAll(object parameter)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                Description = "Select a folder where to extract these files ..."
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.ExtractAllFiles(dialog.SelectedPath, false, false);
            }
            MessageService.ShowMessage(m_editor, "All files extracted");
        }
        private static bool CanSave(object parameter)
        {
            return true;
        }
        private void OnSave(object parameter)
        {
            try
            {
                File.Save();
                MessageService.ShowMessage(m_editor, string.Format("{0} saved !", File.FilePath));
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
        private bool CanSaveAs(object parameter)
        {
            return true;
        }
        private void OnSaveAs(object parameter)
        {
            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog
            {
                FileName = File.FilePath,
                Title = "Save file as ...",
                DefaultExt = ".d2p",
                OverwritePrompt = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = dialog.FileName;
                    File.SaveAs(filePath, true);
                    OnPropertyChanged("Title");
               //     MessageService.ShowMessage(m_editor, string.Format("{0} saved !", File.FileName));
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
        }
        public void Dispose()
        {
            File.Dispose();
        }
    }
}
