using Stump.Core.Attributes;
//using Stump.Core.I18N;
using Stump.Core.Reflection;
using Stump.Core.Xml.Config;
using Stump.DofusProtocol.D2oClasses;
//using Stump.DofusProtocol.D2oClasses.Tools.D2i;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.Items.Templates;
//using Stump.Server.WorldServer.Database.I18n;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Stump.Core.Extensions;
using System.Text.RegularExpressions;
using System.Text;

using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses.Tools.D2p;
using Stump.Server.WorldServer.Database.World.Maps;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Database.Breeds;
using WorldEditor.Loaders.I18N;

namespace DBSynchroniser
{
    internal class Program
    {
        public const string ConfigFile = "sync_config.xml";
        private static XmlConfig m_config;
        private static int m_cursorLeft;
        private static int m_cursorTop;

        [Variable]
        public static readonly DatabaseConfiguration WorldDatabaseConfiguration = new DatabaseConfiguration
        {
            DbName = "stump_world",
            Host = "localhost",
            User = "root",
            Password = "",
            ProviderName = "MySql.Data.MySqlClient"
        };

        [Variable(true)]
        public static string SpecificLanguage = "fr,en";

        [Variable(true)]
        public static string DofusCustomPath = "";

        [Variable(true)]
        public static string DecryptionKey = "649ae451ca33ec53bbcbcc33becf15f4";

        private static readonly Dictionary<string, Languages> _stringToLang = new Dictionary<string, Languages>
        {
            { "fr", Languages.French },
            { "en", Languages.English },
            { "es", Languages.Spanish },
            { "de", Languages.German },
            { "it", Languages.Italian },
            { "ja", Languages.Japanish },
            { "nl", Languages.Dutsh },
            { "pt", Languages.Portugese },
            { "ru", Languages.Russish }
        };

        private static readonly Tuple<string, Action>[] _menus = new Tuple<string, Action>[]
        {
            Tuple.Create("Set Dofus Path (empty = default)", new Action(SetDofusPath)), 
            Tuple.Create("Set languages (empty = all)", new Action(SetLanguages)),
            Tuple.Create("Create langs   d2i->stump_world", new Action(CreateLangs)),
            Tuple.Create("Create objects d2o->stump_world", new Action(CreateObjects)),
            //Tuple.Create("Create maps    d2p->stump_world", new Action(CreateMaps))
        };

        private static void Main()
        {
            Console.WriteLine("Load {0}...", ConfigFile);
            m_config = new XmlConfig(ConfigFile);
            m_config.AddAssembly(Assembly.GetExecutingAssembly());
            if (!File.Exists(ConfigFile))
            {
                m_config.Create(false);
            }
            else
            {
                m_config.Load();
            }

            while (true)
            {
                ShowMenus();
                try
                {
                    string input = Console.ReadLine();
                    int number;
                    while (!int.TryParse(input, out number) || number < 1 || number > _menus.Length)
                    {
                        Console.WriteLine("Give a valid number between 1 and {0}", _menus.Length);
                        input = Console.ReadLine();
                    }
                    Console.Clear();
                    _menus[number - 1].Item2.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex);
                }
                Console.WriteLine("Press Enter to return to the menu");
                Console.ReadLine();
                Console.Clear();
            }
        }

        private static void ShowMenus()
        {
            Console.WriteLine("Dofus path = {0}", FindDofusPath());
            Console.WriteLine("Languages = {0}", SpecificLanguage);
            Console.WriteLine();
            Console.WriteLine("What to do ?");
            int number = 1;
            foreach(var menu in _menus)
            {
                Console.WriteLine(number + ". " + menu.Item1);
                number++;
            }
        }

        private static string FindDofusPath()
        {
            if (!string.IsNullOrEmpty(DofusCustomPath))
            {
                return DofusCustomPath;
            }
            string programFiles = Environment.GetEnvironmentVariable("programfiles(x86)");
            if (string.IsNullOrEmpty(programFiles))
            {
                programFiles = Environment.GetEnvironmentVariable("programfiles");
            }
            if (string.IsNullOrEmpty(programFiles))
            {
                return Path.Combine(AskDofusPath(), "app");
            }
            if (Directory.Exists(Path.Combine(programFiles, "Dofus2", "app")))
            {
                return Path.Combine(programFiles, "Dofus2", "app");
            }
            if (Directory.Exists(Path.Combine(programFiles, "Dofus 2", "app")))
            {
                return Path.Combine(programFiles, "Dofus 2", "app");
            }
            string dofusDataPath = Path.Combine(AskDofusPath(), "app");
            if (!Directory.Exists(dofusDataPath))
            {
                throw new Exception("Dofus data path not found");
            }
            return dofusDataPath;
        }

        private static string AskDofusPath()
        {
            Console.WriteLine("Dofus path not found. Enter Dofus 2 root folder (%programFiles%/Dofus2):");
            return Path.GetFullPath(Console.ReadLine());
        }

        private static void Exit(string reason = "")
        {
            if (!string.IsNullOrEmpty(reason))
            {
                Console.WriteLine(reason);
            }
            Console.WriteLine("Press enter to exit");
            Console.Read();
            Environment.Exit(-1);
        }

        private static void SetDofusPath()
        {
            Console.WriteLine("Enter dofus path (empty = default path):");
            string path = Console.ReadLine();
            if (!Directory.Exists(path))
            {
                Console.WriteLine("Path '{0}' doesn't exists", path);
                return;
            }
            DofusCustomPath = path;
            m_config.Save();
            Console.WriteLine("Path changed (saved)");
        }

        private static void SetLanguages()
        {
            Console.WriteLine("Enter languages (default = fr,en) :");
            string langs = Console.ReadLine();
            SpecificLanguage = langs;
            m_config.Save();
            Console.WriteLine("Languages changed (saved)");
        }

        private static void CreateObjects()
        {
            Console.WriteLine("Enter the tables to build (separated by comma, empty = all)");
            var tables = Console.ReadLine().Split(new char[] { ',' });
            var d2oFiles = Directory.EnumerateFiles(Path.Combine(FindDofusPath(), "data", "common"), "*.d2o");
            var ignoredTables = new string[] { /*"monsters_grades", "monsters_drops",*/ "items_templates_weapons" };

            var database = GetWorldDatabase();
            if (database == null)
            {
                return;
            }

            //database.CreateShema();

            var worldTables = EnumerateTables(typeof(Rates).Assembly).ToList();

            var monsterGradeTable = worldTables.FirstOrDefault(entry => entry.ClassName == "MonsterGrade");
            var monsterDropTable = worldTables.FirstOrDefault(entry => entry.ClassName == "MonsterDrop");
            if (monsterGradeTable == null || monsterDropTable == null)
            {
                Console.WriteLine("a associated to class monster not found !");
            }

            worldTables = worldTables.Where(entry => !ignoredTables.Contains(entry.TableName)).ToList();

            foreach (var table in worldTables.Where(entry => tables.Any(subEntry => entry.TableName.Contains(subEntry))))
            {
                Console.WriteLine("Build table '{0}' ...", table.TableName);

                var file = d2oFiles.FirstOrDefault(entry => IsRightName(table.ClassName, entry));
                if (string.IsNullOrEmpty(file))
                {
                    Console.WriteLine("Cannot find file for {0} ", table.TableName);
                    continue;
                }

                var d2oReader = new D2OReader(file);

                database.Database.Execute("DELETE FROM " + table.TableName);
                database.Database.Execute("ALTER TABLE " + table.TableName + " AUTO_INCREMENT=1");

                if(table.Type == typeof(ItemTemplate))
                {
                    database.Database.Execute("DELETE FROM items_templates_weapons");
                    database.Database.Execute("ALTER TABLE items_templates_weapons AUTO_INCREMENT=1");
                }

              //  using (var transaction = database.Database.GetTransaction())
               // {
                    InitializeCounter();

                    int i = 0;
                    int spell = 1;
                    foreach (var entry in d2oReader.GetObjectsClasses())
                    {
                        IAssignedByD2O record;
                        var obj = d2oReader.ReadObject(entry.Key, false);
                        if (obj is Stump.DofusProtocol.D2oClasses.Breed)
                        {
                            var breed = obj as Stump.DofusProtocol.D2oClasses.Breed;
                            var breedSpell = new BreedSpell();
                            int l = 0;
                            breedSpell.Id = spell;
                            breedSpell.BreedId = breed.Id;
                            breedSpell.ObtainLevel = 1;
                            breedSpell.Spell = 0;
                            spell++;
                            database.Database.Insert(breedSpell);
                            foreach (var spells in breed.BreedSpellsId)
                            {
                                breedSpell.Id = spell;
                                breedSpell.BreedId = breed.Id;
                                var levels = new int[] {
                                                    1,
                                                    1,
                                                    1,
                                                    3,
                                                    6,
                                                    9,
                                                    13,
                                                    17,
                                                    21,
                                                    26,
                                                    31,
                                                    36,
                                                    42,
                                                    48,
                                                    54,
                                                    60,
                                                    70,
                                                    80,
                                                    90,
                                                    100,
                                                    200
                                            };
                                breedSpell.ObtainLevel = levels[l];
                                breedSpell.Spell = (int)spells;
                                l++;
                                spell++;
                                database.Database.Insert(breedSpell);
                            }
                        }
                        var monster = obj as Monster;
                        if (monster != null)
                        {
                            foreach (var monsterGrade in monster.grades)
                            {
                                record = (IAssignedByD2O)monsterGradeTable.Constructor.DynamicInvoke();
                                record.AssignFields(monsterGrade);
                                database.Database.Insert(record);
                            }

                            foreach (var monsterDrop in monster.drops)
                            {
                                record = (IAssignedByD2O)monsterDropTable.Constructor.DynamicInvoke();
                                record.AssignFields(monsterDrop);
                                database.Database.Insert(record);
                            }

                            foreach (var area in monster.Subareas)
                            {
                                var recordd = new MonsterSpawn() { MapId = null, MaxGrade = 5, MinGrade = 1, MonsterId = monster.id, SubAreaId = (int)area };
                                database.Database.Insert(recordd);
                            }
                        }

                        var weapon = obj as Weapon;
                        if (weapon != null)
                        {
                            record = new WeaponTemplate();
                            record.AssignFields(obj);
                            database.Database.Insert(record);

                            UpdateCounter(i, d2oReader.IndexCount);
                            i++;
                            continue;
                        }

                        if(obj is Stump.DofusProtocol.D2oClasses.ItemSet)
                        {

                        }

                        record = (IAssignedByD2O)table.Constructor.DynamicInvoke();
                        record.AssignFields(obj);
                        database.Database.Insert(record);

                        UpdateCounter(i, d2oReader.IndexCount);
                        i++;
                    }

                    //transaction.Complete();
                    EndCounter();
                //}

                d2oReader.Close();
            }

            database.CloseConnection();
        }

        private static void CreateLangs()
        {
            var database = GetWorldDatabase();
            if (database == null)
            {
                return;
            }

            var d2iFiles = new Dictionary<string, D2IFile>();
            string d2iFolder = Path.Combine(FindDofusPath(), "data", "i18n");
            foreach (string rawFile in Directory.EnumerateFiles(d2iFolder, "*.d2i"))
            {
                var match = Regex.Match(Path.GetFileName(rawFile), "i18n_(\\w+)\\.d2i");
                var i18NFile = new D2IFile(rawFile);
                d2iFiles.Add(match.Groups[1].Value, i18NFile);
            }

            var records = new Dictionary<int, LangText>();
            var uiRecords = new Dictionary<string, LangTextUi>();

            foreach (var d2iFile in  d2iFiles.Where(entry => SpecificLanguage.Contains(entry.Key)))
            {
                Languages lang = _stringToLang[d2iFile.Key];
                Console.WriteLine("Import {0}...", Path.GetFileName(d2iFile.Value.FilePath));
                var texts = d2iFile.Value.GetAllText();
                foreach (var ellement in texts)
                {
                    //LangText record;
                    //if (!records.ContainsKey(ellement.Key))
                    //{
                    //    record = new LangText
                    //    {
                    //        Id = (uint)ellement.Key
                    //    };
                    //    records.Add(ellement.Key, record);
                    //}
                    //else
                    //{
                    //    record = records[ellement.Key];
                    //}
                    records.Add(ellement.Key, new LangText()
                    {
                        Id = (uint)ellement.Key,
                        French = ellement.Value
                    });
                    //record.French = ellement.Value;
                }

                foreach (var uiEllement in d2iFile.Value.GetAllUiText())
                {
                    //LangTextUi uiRecord;
                    //if (!uiRecords.ContainsKey(uiEllement.Key))
                    //{
                    //    uiRecord = new LangTextUi
                    //    {
                    //        Name = uiEllement.Key
                    //    };
                    //    uiRecords.Add(uiEllement.Key, uiRecord);
                    //}
                    //else
                    //{
                    //    uiRecord = uiRecords[uiEllement.Key];
                    //}
                    //uiRecord.French = uiEllement.Value;
                    uiRecords.Add(uiEllement.Key, new LangTextUi()
                    {
                        Name = uiEllement.Key,
                        French = uiEllement.Value
                    });
                }
            }

            //database.Database.Execute("DELETE FROM langs");
            //database.Database.Execute("ALTER TABLE langs AUTO_INCREMENT=1");
            database.Database.Execute("DELETE FROM langs_ui");
            database.Database.Execute("ALTER TABLE langs_ui AUTO_INCREMENT=1");

            int i = 0;
            int count = records.Count + uiRecords.Count;
            Console.WriteLine("Save texts ...");

            InitializeCounter();

            foreach (var record in records.Values)
            {
                try
                {
                    database.Database.Execute($"INSERT INTO langs (Id, French) VALUES ({record.Id}, \"{record.French.Replace('"', ' ').Replace('@', ' ')}\");");
                    //database.Database.Insert(record);
                    i++;
                    UpdateCounter(i, count);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            foreach (var record in uiRecords.Values)
            {
                database.Database.Insert(record);
                i++;
                UpdateCounter(i, count);
            }

            EndCounter();
            database.CloseConnection();
        }

        //private static void CreateMaps()
        //{
        //    var d2pFile = Path.Combine(FindDofusPath(), "content", "maps", "maps0.d2p");

        //    if(!File.Exists(d2pFile))
        //    {
        //        Console.WriteLine("cannot find the map file");
        //        return;
        //    }

        //    var database = GetWorldDatabase();
        //    if (database == null)
        //    {
        //        return;
        //    }

        //    var d2pReader = new D2pFile(d2pFile);
        //    var files = d2pReader.ReadAllFiles().Values.ToList();

        //    int i = 0;
        //    InitializeCounter();
        //    foreach (var file in files)
        //    {
        //        i++;
        //        using(var stream = new MemoryStream(file))
        //        {
        //            var mapFile = new DlmReader(stream, "649ae451ca33ec53bbcbcc33becf15f4").ReadMap();
        //            var cells = mapFile.Cells.Select(entry => new Cell() { Id = (short)entry.Id, Floor = entry.Floor, Data = entry.Data, Speed = entry.Speed, MapChangeData = entry.MapChangeData, MoveZone = entry.MoveZone }).ToList();
        //            var ellements = mapFile.Layers.SelectMany(entry => entry.Cells.SelectMany(entry2 => entry2.Elements)).OfType<DlmGraphicalElement>().Where(entry => entry.Identifier != 0).Select(entry3 => new MapElement(entry3.Identifier,(ushort) entry3.Cell.Id)).ToList();

        //            //var oldRecord = database.Database.FirstOrDefault<MapRecord>(MapRecordRelator.FetchByIdSimple, mapFile.Id);
        //            //if (oldRecord != null)
        //            //{//old map to update
        //            //    oldRecord.ClientBottomNeighbourId = mapFile.BottomNeighbourId;
        //            //    oldRecord.ClientTopNeighbourId = mapFile.TopNeighbourId;
        //            //    oldRecord.ClientLeftNeighbourId = mapFile.LeftNeighbourId;
        //            //    oldRecord.ClientRightNeighbourId = mapFile.RightNeighbourId;

        //            //    oldRecord.SubAreaId = mapFile.SubAreaId;
        //            //    oldRecord.Elements = ellements.ToArray();
        //            //    oldRecord.Cells = cells.ToArray();

        //            //    database.Database.Update(oldRecord);
        //            //}
        //            //else
        //            //{//new map
        //                database.Database.Insert(new MapRecord()
        //                {
        //                    Id = mapFile.Id,

        //                    BottomNeighbourId = mapFile.BottomNeighbourId,
        //                    TopNeighbourId = mapFile.TopNeighbourId,
        //                    LeftNeighbourId = mapFile.LeftNeighbourId,
        //                    RightNeighbourId = mapFile.RightNeighbourId,

        //                    ClientBottomNeighbourId = mapFile.BottomNeighbourId,
        //                    ClientTopNeighbourId = mapFile.TopNeighbourId,
        //                    ClientLeftNeighbourId = mapFile.LeftNeighbourId,
        //                    ClientRightNeighbourId = mapFile.RightNeighbourId,

        //                    SubAreaId = mapFile.SubAreaId,
        //                    Elements = ellements.ToArray(),
        //                    Cells = cells.ToArray()
        //                });
        //            //}
        //            UpdateCounter(i, files.Count);
        //        }
        //    }
        //    EndCounter();

        //    d2pReader.Dispose();
        //    database.CloseConnection();
        //}

        private static bool IsRightName(string fisrtName, string secondName)
        {
            return Regex.Match(secondName, string.Format(".+\\\\{0}s?.d2o", fisrtName)).Success;
        }

        private static DatabaseAccessor GetWorldDatabase()
        {
            var worldDatabase = new DatabaseAccessor(WorldDatabaseConfiguration);
            worldDatabase.RegisterMappingAssembly(typeof(Rates).Assembly);

            Console.WriteLine("Connecting to {0}@{1}", WorldDatabaseConfiguration.DbName, WorldDatabaseConfiguration.Host);
            worldDatabase.Initialize();

            try
            {
                worldDatabase.OpenConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not connect : " + e);
                return null;
            }
            Console.WriteLine("Connected!");
            return worldDatabase;
        }

        private static IEnumerable<D2OTable> EnumerateTables(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                var d2oClassAttribute = type.GetCustomAttribute<D2OClassAttribute>();
                if (d2oClassAttribute != null)
                {
                    var tableNameAttribute = type.GetCustomAttribute<TableNameAttribute>();
                    if (tableNameAttribute != null)
                    {
                        D2OTable d2OTable = new D2OTable
                        {
                            Type = type,
                            ClassName = d2oClassAttribute.Name,
                            TableName = tableNameAttribute.TableName,
                            Constructor = type.GetConstructor(Type.EmptyTypes).CreateDelegate()
                        };
                        yield return d2OTable;
                    }
                }
            }
        }

        private static void InitializeCounter()
        {
            m_cursorLeft = Console.CursorLeft;
            m_cursorTop = Console.CursorTop;
        }

        private static void UpdateCounter(int i, int count)
        {
            Console.SetCursorPosition(m_cursorLeft, m_cursorTop);
            Console.Write("{0}/{1} ({2}%)", i, count, (int)((i / (double)count) * 100));
        }

        private static void EndCounter()
        {
            Console.SetCursorPosition(m_cursorLeft, m_cursorTop);
            Console.Write(new string(' ', Console.BufferWidth - m_cursorLeft));
            Console.SetCursorPosition(m_cursorLeft, m_cursorTop);
        }
    }
}
