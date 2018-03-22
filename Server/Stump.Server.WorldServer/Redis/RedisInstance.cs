using System;
using System.Collections.Generic;
using NLog;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System.Runtime.Serialization;
using System.Linq;
using Stump.ORM;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps;
using ServiceStack.Redis.Generic;
using ServiceStack.Redis;
using Stump.Server.WorldServer.Game.PvP;

namespace Stump.Server.WorldServer.Redis {
    public class RedisInstance : Singleton<RedisInstance> {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public IRedisTypedClient<PvPRegister> PvPLogs { get; set; }

        public IRedisTypedClient<AdminActions> ItemsLog { get; set; }

        public IRedisTypedClient<Request> Requests { get; set; }

        public IRedisTypedClient<Error> Errors { get; set; }

        public IRedisTypedClient<MonsterSummonRegister> MonsterSummon { get; set; }

        public static RedisClient Redis { get; set; }
        public IRedisTypedClient<MonsterSummonAvA> MonsterAvA { get; private set; }

        [Initialization(InitializationPass.Second)]
        public void Initialize() {
            InitRedisInstace();
        }

        private void InitRedisInstace() {
            try {
                using (Redis = new RedisClient()) {
                    Errors = Redis.As<Error>();
                    Requests = Redis.As<Request>();
                    ItemsLog = Redis.As<AdminActions>();
                    PvPLogs = Redis.As<PvPRegister>();
                    MonsterSummon = Redis.As<MonsterSummonRegister>();
                    MonsterAvA = Redis.As<MonsterSummonAvA>();
                }
            }
            catch (Exception ex) {
                logger.Error(ex.Message);
            }
        }

        #region Delete

        public void DeleteMonsterSummon(MonsterSummonRegister register) {
            try {
                using (var trans = MonsterSummon.CreateTransaction()) {
                    trans.QueueCommand(x => x.Delete(register));
                    trans.Commit();
                }
            }
            catch (Exception ex) {
                logger.Error($"Can't delete monster summon : {ex.Message}");
            }
        }


        public void DeleteMonsterSummonAvA(MonsterSummonAvA register)
        {
            try
            {
                using (var trans = MonsterAvA.CreateTransaction())
                {
                    trans.QueueCommand(x => x.Delete(register));
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Can't delete monster summon : {ex.Message}");
            }
        }


        #endregion

        #region Set

        public void SetPvPRegister(int id, PvPRegister register) {
            using (var trans = PvPLogs.CreateTransaction())
            {
                trans.QueueCommand(x => x.SetValue("urn:pvpregister:" + id, register));
                trans.Commit();
            }
        }

        #endregion

        #region Store

        public MonsterSummonAvA StoreMonsterSummonAvA(int mapId, int quantity, int allianceId)
        {
            try
            {
                var summon = new MonsterSummonAvA
                {
                    Id = MonsterAvA.GetNextSequence(),
                    MapId = mapId,
                    Quantity = quantity,
                    AllianceId = allianceId
                };
                using (var trans = MonsterAvA.CreateTransaction())
                {
                    trans.QueueCommand(x => x.Store(summon));
                    trans.Commit();
                }
                return summon;
            }
            catch (Exception ex)
            {
                logger.Error($"Can't store monster summon : {ex.Message}");
                return null;
            }
        }

        public MonsterSummonRegister StoreMonsterSummon(uint mapId, ushort itemTemplateId, int ownerId) {
            try {
                var summon = new MonsterSummonRegister {
                    Id = MonsterSummon.GetNextSequence(),
                    MapId = mapId,
                    TemplateId = itemTemplateId,
                    OwnerId = ownerId
                };
                using (var trans = MonsterSummon.CreateTransaction())
                {
                    trans.QueueCommand(x => x.Store(summon));
                    trans.Commit();
                }
                return summon;
            }
            catch (Exception ex) {
                logger.Error($"Can't store monster summon : {ex.Message}");
                return null;
            }
        }

        public PvPRegister StorePvPLog(Character character) {
            try {
                var pvp = new PvPRegister {
                    Id = character.Id,
                    IPs = character.Client.IP,
                    CharacterPvPsIds = new List<int>(),
                    LastCharacterPvPId = 0,
                    LastPvP = DateTime.Now
                };
                using (var trans = PvPLogs.CreateTransaction())
                {
                    trans.QueueCommand(x => x.Store(pvp));
                    trans.Commit();
                }
                return pvp;
            }
            catch (Exception ex)
            {
                logger.Error($"Can't store pvp log : {ex.Message}");
                return null;
            }
        }

        public void StoreError(string Reason, string json) {
            try {
                var error = new Error {
                    Id = Errors.GetNextSequence(),
                    Reason = Reason,
                    Json = json
                };
                using (var trans = Errors.CreateTransaction())
                {
                    trans.QueueCommand(x => x.Store(error));
                    trans.Commit();
                }
            }
            catch (Exception ex) {
                logger.Error($"Can't store error : {ex.Message}");
            }
        }

        public void StoreRequest(string type, int itemId, int quantity, int characterId, string ip, string json) {
            try {
                var request = new Request {
                    Id = Requests.GetNextSequence(),
                    Type = type,
                    ItemId = itemId,
                    Quantity = quantity,
                    CharacterId = characterId,
                    Date = DateTime.Now,
                    Ip = ip,
                    Json = json
                };
                using (var trans = Requests.CreateTransaction())
                {
                    trans.QueueCommand(x => x.Store(request));
                    trans.Commit();
                }
            }
            catch (Exception ex) {
                logger.Error($"Can't store request : {ex.Message}");
            }
        }

        public void StoreLogsItem(string name, uint itemId, string sendTo, string type) {
            try {
                var item = new AdminActions {
                    Id = ItemsLog.GetNextSequence(),
                    Name = name,
                    ItemId = itemId,
                    SendTo = sendTo,
                    IsSet = type
                };
                using (var trans = ItemsLog.CreateTransaction())
                {
                    trans.QueueCommand(x => x.Store(item));
                    trans.Commit();
                }
            }
            catch (Exception ex) {
                logger.Error($"Can't store ItemsLog : {ex.Message}");
            }
        }

        #endregion
    }

    public class AdminActions {
        public long Id { get; set; }
        public string Name { get; set; }
        public uint ItemId { get; set; }
        public string SendTo { get; set; }
        public string IsSet { get; set; }
    }

    public class Error {
        public long Id { get; set; }
        public string Reason { get; set; }
        public string Json { get; set; }
    }

    public class Request {
        public long Id { get; set; }
        public string Type { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int CharacterId { get; set; }
        public DateTime Date { get; set; }
        public string Ip { get; set; }
        public string Json { get; set; }
    }

    //[IgnoreDataMember]

    public class PvPRegister {
        private List<PvPRegister> _characters;
        public long Id { get; set; }
        public string IPs { get; set; } //Here we save all ip's used by this Id, it's like check contains....
        public uint LastCharacterPvPId { get; set; }
        public List<int> CharacterPvPsIds { get; set; }

        [IgnoreDataMember]
        public List<PvPRegister> CharacterPvPs {
            get {
                if (_characters != null) return _characters;
                _characters = new List<PvPRegister>();
                foreach (var characterPvPsId in CharacterPvPsIds) {
                    var c = PvPManager.Instance.TryGetRegister(characterPvPsId);
                    if (c != null)
                        _characters.Add(c);
                }
                return _characters;
            }
            set {
                _characters = value;
                var c = CharacterPvPsIds.Where(x => !_characters.Select(s => s.Id).Contains(x));
                CharacterPvPsIds.AddRange(c);
            }
        }

        public DateTime LastPvP { get; set; }
        public uint WinDaily { get; set; }
        public uint LoseDaily { get; set; }

        [IgnoreDataMember]
        public uint DailyCountFight => WinDaily - LoseDaily;
        /// <summary>
        /// Fight Count in total with another people
        /// </summary>
        public uint FightCount { get; set; }
    }

    public class MonsterSummonRegister {
        private Map m_map;
        private ItemTemplate m_template;
        public long Id { get; set; }
        public uint MapId { get; set; }
        public ushort TemplateId { get; set; }
        public int OwnerId { get; set; }
        [IgnoreDataMember]
        public Map Map
        {
            get
            {
                Map arg_3F_0;
                if ((arg_3F_0 = m_map) == null)
                    arg_3F_0 = m_map = Singleton<Game.World>.Instance.GetMap((int) MapId);
                var result = arg_3F_0;
                return result;
            }
            set {
                m_map = value;
                MapId = (uint) value.Id;
            }
        }
        [IgnoreDataMember]
        public ItemTemplate Template
        {
            get
            {
                ItemTemplate arg_23_0;
                if ((arg_23_0 = m_template) == null)
                    arg_23_0 = m_template = Singleton<ItemManager>.Instance.TryGetTemplate(TemplateId);
                return arg_23_0;
            }
            set
            {
                m_template = value;
                TemplateId = (ushort) value.Id;
            }
        }
    }

    public class MonsterSummonAvA
    {
        public long Id { get; set; }
        public int MapId { get; set; }
        public int Quantity { get; set; }
        public int AllianceId { get; set; }
    }
}