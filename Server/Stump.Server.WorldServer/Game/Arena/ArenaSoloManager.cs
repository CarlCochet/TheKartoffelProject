using NLog;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Reflection;
using Stump.Core.Threading;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaSoloManager : Singleton<ArenaSoloManager>
    {
        // FIELDS
        protected readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly object m_sync = new object();
        private static Task m_queueRefresherTask;
        private static int m_queRefresherElapsedTime = 10000;

        [Variable]
        public static byte MinimumLevelArena = 50;
        [Variable]
        public static int[] ArenaMapsId = new int[]
        {
            94634497,
            94634499,
            94634519,
            94634505,
            81788928,
        };

        private readonly ConcurrentList<Character> m_queue;
        private readonly ConcurrentList<ArenaParty> m_partyQueue;

        private readonly AsyncRandom m_asyncRandom;

        // PROPERTIES
        public object SyncRoot
        {
            get
            {
                return this.m_sync;
            }
        }

        // CONSTRUCTORS
        public ArenaSoloManager()
        {
            this.m_queue = new ConcurrentList<Character>();
            this.m_partyQueue = new ConcurrentList<ArenaParty>();

            this.m_asyncRandom = new AsyncRandom();
        }

        // METHODS
        [Initialization(InitializationPass.Last)]
        private static void Initialize()
        {
            ArenaSoloManager.m_queueRefresherTask = Task.Factory.StartNewDelayed(m_queRefresherElapsedTime, new Action(Singleton<ArenaSoloManager>.Instance.TryCreateTeams));
        }

        public bool CanArena(Character character)
        {
            var result = true;

            if (character.Level < MinimumLevelArena)
            {
                // TODO : message
                result = false;
            }

            return result;
        }
        public bool CanArena(Character character, ArenaParty party)
        {
            var result = this.CanArena(character);
            return result;
        }

        public Map GetRandomMap()
        {
            var mapId = ArenaMapsId[this.m_asyncRandom.Next(ArenaMapsId.Length)];

            var map = Singleton<World>.Instance.GetMap(mapId);
            if (map == null)
            {
                throw new ConverterException(string.Format("'{0}' map not found", mapId));
            }

            return map;
        }

        public bool IsInQueue(Character character)
        {
            return this.m_queue.Contains(character);
        }
        public void Register(Character character)
        {
            if (this.CanArena(character))
            {
                this.m_queue.Add(character);
                this.GuessHiddenRank(character);

                this.BindEvents(character);
            }
        }

        public void Unregister(Character character)
        {
            if (this.IsInQueue(character))
            {
                this.m_queue.Remove(character);
            }

            this.UnBindEvents(character);
        }

        private void TryCreateTeams()
        {
            try
            {
                var time = DateTime.Now;
                List<Character> characters;

                lock (this.m_sync)
                {
                    characters = this.m_queue.Where(entry => entry.ArenaInvitation == null).ToList().OrderByDescending(entry => entry.Record.HiddenArenaRank).ToList();
                }

                #region handle P1

                Character fighter1 = characters[0];
                Character fighter2 = characters[1];
                characters.Remove(fighter1);
                characters.Remove(fighter2);
                var team1 = new Character[1];
                var team2 = new Character[1];
                team1[0] = fighter1;
                team2[0] = fighter2;
                var arenaInvitation = new ArenaInvitation(new ArenaPartyCreation(team1), new ArenaPartyCreation(team2), true); // TODO }

                #endregion

                this.logger.Debug(string.Format("TryCreateTeams() executed : {0} ms", (DateTime.Now - time).Milliseconds));
            }
            finally
            {
                ArenaSoloManager.m_queueRefresherTask = Task.Factory.StartNewDelayed(m_queRefresherElapsedTime, new Action(this.TryCreateTeams));
            }
        }

        public void GuessHiddenRank(Character character)
        {
            if (character.Record.HiddenArenaRank < 0 || character.Record.ArenaFightCount == 0)
            {
                character.Record.HiddenArenaRank = (short)(5 * character.Level);
            }
        }

        #region Bind events

        private void OnCharacterLoggedOut(Character character)
        {
            this.Unregister(character);
        }

        #endregion

        private void BindEvents(Character character)
        {
            character.LoggedOut += new Action<Character>(this.OnCharacterLoggedOut);
        }

        private void UnBindEvents(Character character)
        {
            character.LoggedOut -= new Action<Character>(this.OnCharacterLoggedOut);
        }
    }
}
