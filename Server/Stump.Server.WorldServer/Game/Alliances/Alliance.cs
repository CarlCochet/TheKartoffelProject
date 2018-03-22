using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NLog;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Alliances;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Game.Prisms;
using Stump.Server.WorldServer.Handlers.Alliances;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Game.Alliances
{
    public class Alliance
    {
        // FIELDS
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<int, Guild> m_guilds = new Dictionary<int, Guild>();
        private readonly object m_lock = new object();

        private bool m_isDirty;

        // CONSTRUCTORS
        public Alliance(int id, string name, string tag)
        {
            Record = new AllianceRecord();
            Id = id;
            Name = name;
            Tag = tag;
            Record.CreationDate = DateTime.Now;
            Emblem = new AllianceEmblem(Record)
            {
                BackgroundColor = Color.White,
                BackgroundShape = 1,
                SymbolColor = Color.Black,
                SymbolShape = 1
            };
            Prisms = new List<PrismNpc>();
            Record.IsNew = true;
            IsDirty = true;
        }

        public Alliance(AllianceRecord record)
        {
            Record = record;
            Emblem = new AllianceEmblem(Record);
            Prisms = new List<PrismNpc>();
            foreach (var item in Singleton<GuildManager>.Instance.GetGuildsByAlliance(Record))
            {
                item.SetAlliance(this);

                m_guilds.Add(item.Id, item);

                foreach (var guild in item.Clients)
                    if (!Clients.Contains(guild))
                        Clients.Add(guild);

                if (Record.Owner == item.Id)
                    SetBoss(item);
            }

            if (Boss == null)
            {
                if (m_guilds.Count == 0) {}
                else
                    SetBoss(m_guilds.First().Value);
            }
        }

        // PROPERTIES
        public int Id
        {
            get { return Record.Id; }
            private set { Record.Id = value; }
        }

        public string Name
        {
            get { return Record.Name; }
            private set
            {
                Record.Name = value;
                IsDirty = true;
            }
        }

        public string Tag
        {
            get { return Record.Tag; }
            private set
            {
                Record.Tag = value;
                IsDirty = true;
            }
        }

        public AllianceRecord Record { get; }

        public Guild Boss { get; private set; }

        public AllianceEmblem Emblem { get; protected set; }

        public List<PrismNpc> Prisms { get; set; }

        public WorldClientCollection Clients { get; } = new WorldClientCollection();

        public bool IsDirty
        {
            get { return m_isDirty || Emblem.IsDirty; }
            set
            {
                m_isDirty = value;
                if (!value)
                {
                    Emblem.IsDirty = false;
                }
            }
        }

        public ushort Members
        {
            get { return (ushort) m_guilds.Sum(entry => entry.Value.Members.Count); }
        }

        // METHODS
        public void AddPrism(PrismNpc prism)
        {
            Prisms.Add(prism);
        }

        /// <summary>
        /// Add new guild to the alliance
        /// </summary>
        /// <param name="guild"></param>
        /// <returns></returns>
        public bool TryAddGuild(Guild guild)
        {
            bool result;
            lock (m_lock) {
                if (guild.Alliance != null) // TODO : more conditions
                    result = false;
                else {
                    m_guilds.Add(guild.Id, guild);
                    guild.SetAlliance(this);
                    foreach (var client in guild.Clients) {
                        if (!Clients.Contains(client))
                            Clients.Add(client);
                    }
                    if (m_guilds.Count == 1)
                        SetBoss(guild);
                    OnGuildAdded(guild);

                    result = true;
                }
            }
            return result;
        }

        public Guild GetGuildById(uint id)
        {
            return m_guilds.TryGetValue((int) id, out var guild) ? guild : null;
        }

        public AllianceInformations GetAllianceInformations()
        {
            return new AllianceInformations((uint) Id, Tag, Name, Emblem.GetNetworkGuildEmblem());
        }

        public BasicNamedAllianceInformations GetBasicNamedAllianceInformations() {
            return new BasicNamedAllianceInformations((uint) Id, Tag, Name);
        }

        public AllianceFactSheetInformations GetAllianceFactSheetInformations()
        {
            return new AllianceFactSheetInformations((uint) Id, Tag, Name, Emblem.GetNetworkGuildEmblem(),
                Record.CreationDate.GetUnixTimeStamp());
        }

        public AllianceVersatileInformations GetAllianceVersatileInformations()
        {
            return new AllianceVersatileInformations((uint) Id, (ushort) m_guilds.Count, Members, 0);
        }

        public IEnumerable<GuildInsiderFactSheetInformations> GetGuildsInformations()
        {
            return from guild in m_guilds
                select new GuildInsiderFactSheetInformations(
                    (uint) guild.Value.Id,
                    guild.Value.Name,
                    guild.Value.Level,
                    guild.Value.Emblem.GetNetworkGuildEmblem(),
                    (uint) guild.Value.Boss.Id,
                    (ushort) guild.Value.Members.Count,
                    guild.Value.Boss.Name,
                    (ushort) guild.Value.Members.Count(entry => entry.IsConnected),
                    (sbyte) guild.Value.TaxCollectors.Count,
                    guild.Value.CreationDate.GetUnixTimeStamp());
        }

        public IEnumerable<PrismSubareaEmptyInfo> GetPrismsInformations() {
            if (Prisms != null)
            {
                var list =
                    Prisms.Select(
                        prismRecord =>
                            new PrismGeolocalizedInformation((ushort) prismRecord.SubArea.Id,
                                (uint)prismRecord.Alliance.Id, //TODO Send by original
                                (short) prismRecord.Map.Position.X, (short) prismRecord.Map.Position.Y,
                                prismRecord.Map.Id,
                                prismRecord.GetAllianceInsiderPrismInformation()))
                        .ToList();
                return list;
            }
            return new PrismSubareaEmptyInfo[0];
        }

        public IEnumerable<GuildInAllianceInformations> GetGuildsInAllianceInformations()
        {
            return from guild in m_guilds
                select new GuildInAllianceInformations(
                    (uint) guild.Value.Id,
                    guild.Value.Name,
                    guild.Value.Level,
                    guild.Value.Emblem.GetNetworkGuildEmblem(),
                    (byte) guild.Value.Members.Count);
        }

        protected virtual void OnGuildAdded(Guild guild)
        {
            foreach (var member in guild.Members)
                if (member.IsConnected)
                    AllianceHandler.SendAllianceJoinedMessage(member.Character.Client, this);
        }

        public void SetBoss(Guild guild)
        {
            Boss = guild;

            if (Record.Owner != Boss.Id)
            {
                Record.Owner = Boss.Id;
                IsDirty = true;
            }
        }

        public void Save(ORM.Database database)
        {
            if (Record.IsNew)
                database.Insert(Record);
            else
                database.Update(Record);
            IsDirty = false;
            Record.IsNew = false;
        }

        public void SendPrismsInfoValidMessage() {
            Clients.Send(
                new PrismsInfoValidMessage(
                    Prisms.Where(x => x.IsFighting).Select(x => x.Fighter.GetPrismFightersInformation())));
        }

        public void SendInformationMessage(TextInformationTypeEnum msgType, short msgId, params object[] parameters)
        {
            BasicHandler.SendTextInformationMessage(Clients, msgType, msgId, parameters);
        }
    }
}