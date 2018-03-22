using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Guilds;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [Discriminator("GuildCreation", typeof(NpcReply), typeof(NpcReplyRecord))]
    class CreateGuildReply : NpcReply
    {
        public CreateGuildReply(NpcReplyRecord record) : base(record)
        {

        }
        public override bool CanExecute(Npc npc, Character character)
        {
            if(character.Guild == null)
            {
                return true;
            }
            switch (character.Account.Lang)
            {
                case "fr":
                    character.SendServerMessage("Vous êtes actuellement dans une guilde. Veuillez la quitter pour en créer une.", System.Drawing.Color.Red);
                    break;
                case "es":
                    character.SendServerMessage("Vous êtes actuellement dans une guilde. Veuillez la quitter pour en créer une.", System.Drawing.Color.Red);
                    break;
                case "en":
                    character.SendServerMessage("Vous êtes actuellement dans une guilde. Veuillez la quitter pour en créer une.", System.Drawing.Color.Red);
                    break;
            }
            return false;
        }
        public override bool Execute(Npc npc, Character character)
        {
            try
            {
                GuildCreationPanel panel = new GuildCreationPanel(character);
                panel.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}