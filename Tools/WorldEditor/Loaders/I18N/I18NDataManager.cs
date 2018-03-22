using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorldEditor.Config;
using WorldEditor.Helpers.Reflection;
using WorldEditor.Helpers.Extentions;
using WorldEditor.Loaders.Database;
using System.IO;
using System.Text.RegularExpressions;

namespace WorldEditor.Loaders.I18N
{
    public class I18NDataManager : Singleton<I18NDataManager>
    {
        private Dictionary<uint, LangText> _langs;
        private Dictionary<string, LangTextUi> _langsUi;
        private Dictionary<Languages, D2IFile> _files;

        public ReadOnlyDictionary<uint, LangText> Langs
        {
            get { return new ReadOnlyDictionary<uint, LangText>(_langs); }
        }

        public ReadOnlyDictionary<string, LangTextUi> LangsUi
        {
            get { return new ReadOnlyDictionary<string, LangTextUi>(_langsUi); }
        }

        public Languages DefaultLanguage { get; set; }

        public void Initialize()
        {
            DatabaseManager.Instance.Database.OneTimeCommandTimeout = 120;
            _langs = DatabaseManager.Instance.Database.Query<LangText>("SELECT * FROM langs").ToDictionary(entry => entry.Id);
            _langsUi = DatabaseManager.Instance.Database.Query<LangTextUi>("SELECT * FROM langs_ui").ToDictionary(entry => entry.Name);
            _files = new Dictionary<Languages, D2IFile>();
            foreach (string rawFile in Directory.EnumerateFiles(Settings.LoaderSettings.D2IDirectory, "*.d2i"))
            {
                var match = Regex.Match(Path.GetFileName(rawFile), "i18n_(\\w+)\\.d2i");
                _files.Add(LoaderSettings._stringToLang.GetOrDefault(match.Groups[1].Value), new D2IFile(rawFile));
            }
        }

        public string ReadText(uint id, Languages? lang)
        {
            LangText record;
            return (!_langs.TryGetValue(id, out record)) ? string.Format("not found {0}", id) : record.GetText(lang ?? DefaultLanguage);
        }

        public string ReadText(string id, Languages? lang)
        {
            LangTextUi record;
            return (!_langsUi.TryGetValue(id, out record)) ? string.Format("not found {0}", id) : record.GetText(lang ?? DefaultLanguage);
        }

        public string ReadText(string id)
        {
            return ReadText(id, null);
        }

        public string ReadText(uint id)
        {
            return ReadText(id, null);
        }

        public string ReadText(int id)
        {
            return ReadText((uint)id, null);
        }

        public LangTextUi GetText(string id)
        {
            return _langsUi.GetOrDefault(id);
        }

        public LangText GetText(uint id)
        {
            return _langs.GetOrDefault(id);
        }

        public LangText GetText(int id)
        {
            return _langs.GetOrDefault((uint)id);
        }

        public void SaveText(LangText text)
        {
            _langs[text.Id] = text;
            foreach(var file in _files)
            {
                file.Value.SetText(text.Id, text.GetText(file.Key));
            }
            DatabaseManager.Instance.Database.Update(text);
        }

        public void SaveText(LangTextUi text)
        {
            _langsUi[text.Name] = text;
            foreach (var file in _files)
            {
                file.Value.SetText(text.Name, text.GetText(file.Key));
            }
            DatabaseManager.Instance.Database.Update(text);
        }

        public void CreateText(LangText text)
        {
            _langs.Add(text.Id, text);
            foreach (var file in _files)
            {
                file.Value.CreateText(text.Id, text.GetText(file.Key));
            }
            DatabaseManager.Instance.Database.Insert(text);
        }

        public void CreateText(LangTextUi text)
        {
            _langsUi.Add(text.Name, text);
            foreach (var file in _files)
            {
                file.Value.CreateText(text.Name, text.GetText(file.Key));
            }
            DatabaseManager.Instance.Database.Insert(text);
        }

        public void DeleteText(LangText text)
        {
            _langs.Remove(text.Id);
            foreach (var file in _files)
            {
                file.Value.DeleteText(text.Id);
            }
            DatabaseManager.Instance.Database.Delete(text);
        }

        public void DeleteText(LangTextUi text)
        {
            _langsUi.Remove(text.Name);
            foreach (var file in _files)
            {
                file.Value.DeleteText(text.Name);
            }
            DatabaseManager.Instance.Database.Delete(text);
        }

        public uint FindFreeId()
        {
            uint id = DatabaseManager.Instance.Database.ExecuteScalar<uint>("SELECT MAX(Id) FROM langs") + 1;
            id = ((id < Settings.MinI18NId) ? Settings.MinI18NId : id);

            foreach (var file in _files)
            {
                var fileId = (uint)file.Value.FindFreeId();
                if (fileId > id)
                {
                    id = fileId;
                }
            }

            return id;
        }

        public void Save()
        {
            foreach(var file in _files.Values)
            {
                file.Save();
            }
        }
    }
}