using Stump.Core.Reflection;
using Stump.Core.Threading;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Handlers.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaInvitation : Notification
    {
        public static short ArenaInvitationDuration = 60;

        private readonly object m_locker = new object();
        private List<int> m_acceptedIds;
        private bool m_disposed = false;
        private int declinednumber = 0;
        private bool kolisolo = false;
        
        public ArenaPartyCreation RedTeam
        {
            get;
            private set;
        }
        public ArenaPartyCreation BlueTeam
        {
            get;
            private set;
        }

        public ArenaInvitation(ArenaPartyCreation redTeam, ArenaPartyCreation blueTeam)
        {
            this.m_acceptedIds = new List<int>();

            this.RedTeam = redTeam;
            this.BlueTeam = blueTeam;

            this.ForEach(new System.Action<ArenaPartyCreation>((ArenaPartyCreation entry) => entry.SendFightProposition(this)));

            Task.Factory.StartNewDelayed(ArenaInvitationDuration * 1000, this.TimerElapsed);
        }
        public ArenaInvitation(ArenaPartyCreation redTeam, ArenaPartyCreation blueTeam, bool solo)
        {
            this.m_acceptedIds = new List<int>();
            kolisolo = solo;
            this.RedTeam = redTeam;
            this.BlueTeam = blueTeam;

            this.ForEach(new System.Action<ArenaPartyCreation>((ArenaPartyCreation entry) => entry.SendFightProposition(this)));

            Task.Factory.StartNewDelayed(ArenaInvitationDuration * 1000, this.TimerElapsed);
        }

        public void Accept(Character character)
        {
            lock (this.m_locker)
            {
                if (!this.m_acceptedIds.Contains(character.Id))
                {
                    this.m_acceptedIds.Add(character.Id);

                    if (kolisolo)
                    {
                        if (this.m_acceptedIds.Count == 2)
                        {
                            this.StartFight();

                            this.Dispose();
                        }
                        else if(declinednumber != 0)
                        {
                            this.Dispose();
                        }
                    }
                    else
                    {
                        this.ForEach(new System.Action<Character>((Character entry) => ContextHandler.SendGameRolePlayArenaFighterStatusMessage(entry.Client, 1, character, true)));
                        if (this.m_acceptedIds.Count == 6)
                        {
                            this.StartFight();

                            this.Dispose();
                        }
                    }
                }
            }
        }
        public void Deny(Character character)
        {
            lock (this.m_locker)
            {
                if (kolisolo)
                {

                    if (this.m_acceptedIds.Count != 2)
                    {
                        this.UnregisterCharacter(character);
                        character.Record.kolisolo = false;
                        character.SendServerMessage("Vous vous êtes <b>Désinscrit<b> du Kolizeum 1V1 suite au refus du combat.", System.Drawing.Color.DarkRed);
                        declinednumber++;
                        if (declinednumber == 2)
                        {
                            this.Dispose();
                        }
                    }
                }
                else
                {
                    if (this.m_acceptedIds.Count != 6)
                    {
                        this.UnregisterCharacter(character);

                        this.ForEach(new System.Action<Character>((Character entry) => ContextHandler.SendGameRolePlayArenaFighterStatusMessage(entry.Client, 1, character, false)));

                        this.Dispose();
                    }
                }
            }
        }
      

        public void ForEach(System.Action<Character> action)
        {
            lock (this.m_locker)
            {
                this.RedTeam.ForEach(action);
                this.BlueTeam.ForEach(action);
            }
        }

        private void ForEach(System.Action<ArenaPartyCreation> action)
        {
            lock (this.m_locker)
            {
                action(this.RedTeam);
                action(this.BlueTeam);
            }
        }

        private void TimerElapsed()
        {
            lock (this.m_locker)
            {
                this.ForEach(new System.Action<Character>((Character entry) =>
                {
                    if (!this.m_acceptedIds.Contains(entry.Id))
                    {
                        this.UnregisterCharacter(entry);
                    }
                }));

                this.Dispose();
            }
        }

        private void StartFight()
        {
            var canStart = true;
            this.ForEach(new Action<Character>((Character entry) =>
            {
                if (!entry.IsInWorld || entry.IsBusy() || entry.IsInFight())
                {
                    this.UnregisterCharacter(entry);

                    canStart = false;
                }
            }));

            if (canStart)
            {
                this.ForEach(new Action<Character>((Character entry) => this.UnregisterCharacter(entry)));
                var map = Singleton<ArenaManager>.Instance.GetRandomMap();
                if(kolisolo)
                {
                    map = Singleton<ArenaSoloManager>.Instance.GetRandomMap();
                }
                var fight = Singleton<FightManager>.Instance.CreateArenaFight(map, kolisolo);
                fight.AddRedTeam(this.RedTeam);
                fight.AddBlueTeam(this.BlueTeam);
            }
        }

        private void UnregisterCharacter(Character character)
        {
            if(kolisolo)
            {
                Singleton<ArenaSoloManager>.Instance.Unregister(character);
                character.Record.kolisolo = false;
            }
            else
            {
                var party = character.ArenaParty;
                if (party != null)
                {
                    //Singleton<ArenaManager>.Instance.Unregister(party);
                }
                else
                {
                    Singleton<ArenaManager>.Instance.Unregister(character);
                }
            }
        }

        public void Dispose()
        {
            if (!this.m_disposed)
            {
                this.ForEach(new System.Action<Character>((Character entry) =>
                {
                    entry.SetArenaInvitation(null);
                }));

                this.m_disposed = true;
            }
        }
    }
}
