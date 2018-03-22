using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Pool;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Alliances;
using Stump.Server.WorldServer.Database.Symbols;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Alliances
{
    public class AllianceManager : DataManager<AllianceManager>, ISaveable
    {
        // FIELDS
        public const int ALLIOGEMME_ID = 14290;
        private readonly Stack<Alliance> m_alliancesToDelete = new Stack<Alliance>();
        private readonly object m_lock = new object();
        private Dictionary<int, Alliance> m_alliances;
        private Dictionary<int, EmblemRecord> m_emblems;

        private UniqueIdProvider m_idProvider;

        public void Save()
        {
            lock (m_lock)
            {
                Alliance temp;
                using (var enumerator = (
                    from alliance in m_alliances.Values
                    where alliance.IsDirty
                    select alliance).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        temp = enumerator.Current;

                        temp.Save(Database);
                    }
                }

                while (m_alliancesToDelete.Count > 0)
                {
                    temp = m_alliancesToDelete.Pop();

                    Database.Delete(temp.Record);
                }
            }
        }

        // PROPERTIES

        // CONSTRUCTORS

        // METHODS
        [Initialization(InitializationPass.Seventh)]
        public override void Initialize()
        {
            m_emblems = Database.Query<EmblemRecord>(EmblemRelator.FetchQuery).ToDictionary(x => x.Id);
            m_alliances = (from temp in Database.Query<AllianceRecord>(AllianceRelator.FetchQuery).ToList()
                select new Alliance(temp)).ToDictionary(entry => entry.Id);

            if (!m_alliances.Any())
            {
                m_idProvider = new UniqueIdProvider(1);
            }
            else
            {
                m_idProvider = new UniqueIdProvider((
                    from temp in m_alliances
                    select temp.Value.Id).Max());
            }

            Singleton<World>.Instance.RegisterSaveableInstance(this);
        }

        public SocialGroupCreationResultEnum CreateAlliance(Character character, string allianceName, string allianceTag,
            GuildEmblem emblem)
        {
            SocialGroupCreationResultEnum result;
            if (DoesNameExist(allianceName))
            {
                result = SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_ERROR_NAME_ALREADY_EXISTS;
            }
            else
            {
                if (DoesTagExist(allianceTag))
                {
                    result = SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_ERROR_TAG_ALREADY_EXISTS;
                }
                else
                {
                    if (DoesEmblemExist(emblem))
                    {
                        result = SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_ERROR_EMBLEM_ALREADY_EXISTS;
                    }
                    else
                    {
                        var basePlayerItem =
                            character.Inventory.TryGetItem(Singleton<ItemManager>.Instance.TryGetTemplate(ALLIOGEMME_ID));
                        if (basePlayerItem == null)
                        {
                            result = SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_ERROR_REQUIREMENT_UNMET;
                        }
                        else
                        {
                            character.Inventory.RemoveItem(basePlayerItem, 1);
                            character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, 1,
                                basePlayerItem.Template.Id);

                            var alliance = CreateAlliance(allianceName, allianceTag);
                            if (alliance == null)
                            {
                                result = SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_ERROR_CANCEL;
                            }
                            else
                            {
                                alliance.Emblem.ChangeEmblem(emblem);
                                if (!alliance.TryAddGuild(character.Guild))
                                {
                                    DeleteAlliance(alliance);
                                    result = SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_ERROR_CANCEL;
                                }
                                else
                                {
                                    character.Guild.Alliance = alliance;
                                    character.RefreshActor();
                                    result = SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_OK;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public List<Alliance> GetAlliances()
        {
            return m_alliances.Values.ToList();
        }

        public Alliance CreateAlliance(string name, string tag)
        {
            Alliance result;

            lock (m_lock)
            {
                var alliance = new Alliance(m_idProvider.Pop(), name, tag);
                m_alliances.Add(alliance.Id, alliance);

                result = alliance;
                Save();
            }

            return result;
        }

        public Alliance TryGetAlliance(int id)
        {
            Alliance result;
            lock (m_lock)
            {
                Alliance alliance;

                result = m_alliances.TryGetValue(id, out alliance) ? alliance : null;
            }
            return result;
        }

        public Alliance TryGetAlliance(string name)
        {
            Alliance result;
            lock (m_lock)
            {
                result =
                    m_alliances.FirstOrDefault(
                        entry => string.Equals(entry.Value.Name, name, StringComparison.CurrentCultureIgnoreCase)).Value;
            }
            return result;
        }

        public EmblemRecord TryGetEmblem(int id)
        {
            EmblemRecord record;

            return m_emblems.TryGetValue(id, out record) ? record : null;
        }

        public bool DeleteAlliance(Alliance alliance)
        {
            bool result;
            lock (m_lock)
            {
                // TODO : clean alliance
                m_alliances.Remove(alliance.Id);
                m_alliancesToDelete.Push(alliance);
                result = true;
            }
            return result;
        }

        public bool DoesNameExist(string name)
        {
            return
                m_alliances.Any(pair => string.Equals(pair.Value.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool DoesTagExist(string tag)
        {
            return m_alliances.Any(pair => string.Equals(pair.Value.Tag, tag, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool DoesEmblemExist(GuildEmblem emblem)
        {
            return m_alliances.Any(entry => entry.Value.Emblem.DoesEmblemMatch(emblem));
        }

        public IEnumerable<AllianceFactSheetInformations> GetAlliancesFactSheetInformations()
        {
            lock (m_lock)
            {
                return m_alliances.Values
                    .Select(entry => entry.GetAllianceFactSheetInformations());
            }
        }

        public IEnumerable<AllianceVersatileInformations> GetAlliancesVersatileInformations()
        {
            lock (m_lock)
            {
                return m_alliances.Values
                    .Select(entry => entry.GetAllianceVersatileInformations());
            }
        }
    }
}