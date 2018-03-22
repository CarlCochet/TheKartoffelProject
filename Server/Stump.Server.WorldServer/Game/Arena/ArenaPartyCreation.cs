using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System.Collections.Generic;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaPartyCreation
    {
        private readonly object m_memberLocker = new object();

        private ArenaParty m_party;
        private List<Character> m_characters;
        private Character m_character;

        public ArenaPartyCreation(ArenaParty party)
        {
            this.m_characters = new List<Character>();
            this.m_party = party;
        }
        public ArenaPartyCreation(ArenaParty party, params Character[] characters)
        {
            this.m_characters = new List<Character>();
            this.m_characters.AddRange(characters);
            this.m_party = party;
        }
        public ArenaPartyCreation(params Character[] characters)
        {
            this.m_characters = new List<Character>();
            this.m_characters.AddRange(characters);
            this.m_party = null;
        }
        public ArenaPartyCreation(Character character)
        {
            this.m_character = character;
            this.m_party = null;
        }

        public void ForEach(System.Action<Character> action)
        {
            lock (this.m_memberLocker)
            {
                foreach (Character current in this.m_characters)
                {
                    action(current);
                }

                if (this.m_party != null)
                {
                    this.m_party.ForEach(action);
                }
            }
        }

        internal void SendFightProposition(ArenaInvitation parent)
        {
            this.ForEach(entry =>
            {
                entry.SetArenaInvitation(parent);

                ContextHandler.SendGameRolePlayArenaFightPropositionMessage(entry.Client, 1, this.GetAlliesId(entry), ArenaInvitation.ArenaInvitationDuration);
            });
        }

        private IEnumerable<int> GetAlliesId(Character character)
        {
            var result = new List<int>();

            this.ForEach(entry => result.Add(entry.Id));

            return result;
        }

        public IEnumerable<Character> GetCharacters()
        {
            var characters = new List<Character>();

            lock (this.m_memberLocker)
            {
                characters.AddRange(this.m_characters);
                if (this.m_party != null)
                {
                    characters.AddRange(this.m_party.Members);
                }
            }

            return characters;
        }
    }
}
