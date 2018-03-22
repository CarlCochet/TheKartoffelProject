using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Replies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items;
using Game.UtilisationTokens;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;

namespace Database.Npcs.Replies
{
    [Discriminator("Loteria", typeof(NpcReply), new System.Type[]{typeof(NpcReplyRecord)})]
    public class LoterieReply : NpcReply
    {
        public LoterieReply(NpcReplyRecord record) : base(record){}

        
        public override bool Execute(Npc npc, Character character)
        {

            if (character.Inventory.Tokens != null && character.Inventory.Tokens.Stack >= 0){
                List<int> m_Loterie = new List<int>() { 0, 50, 100, 25, 200, 0, 50, 50, 100, 25, 10, 10, 10, 1, 5, 50, 50, 0, 0, 80, 40, 60, 20, 10, 1, 1, 1, 30, 150 };
                int Loterie = m_Loterie[new Random().Next(0, 5)];
                int tokenAvantAchat = (int)character.Inventory.Tokens.Stack;
                character.Inventory.Tokens.Record.Stack += (uint)Loterie;
                character.Inventory.UnStackItem(character.Inventory.Tokens, 0);
                int tokenApreAchat = (int)(character.Inventory.Tokens != null ? character.Inventory.Tokens.Stack : 0);
                character.SendServerMessage(string.Format("Acabas de ganar <b>{0} Ogrinas</b> en la Lotería, Te encontrarás siempre un npc como éste, al final de cada mazmorra :). ", Loterie));
                WorldServer.Instance.IOTaskPool.AddMessage(new Action(character.SaveNow));
                UtilisationTokenManager token = new UtilisationTokenManager(character, "LOTERIE", 0, 0, tokenAvantAchat);
            }
            else if (DateTime.Now.DayOfWeek != DayOfWeek.Monday)
            {
                character.SendServerMessage("Tienes que esperar al viernes en la noche 20H.");

            }
            else if (DateTime.Now.TimeOfDay.Hours < 1)
            {
                character.SendServerMessage("Tienes que esperar <b>1h</b>.", System.Drawing.Color.Green);
            }
            else
                character.SendServerMessage("Tu no tienes Ogrinas (1) para usar la lotería.", System.Drawing.Color.Green);
            return true;
        }

        
    }
}
