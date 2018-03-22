using Stump.Server.BaseServer.Database;
using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.BaseServer.IPC.Messages;

namespace Stump.Server.WorldServer.Database.Npcs.Replies.AlignReplies
{
    [Discriminator("Mercenary", typeof(NpcReply), new Type[] { typeof(NpcReplyRecord) })]
    public class MercenaryReply : NpcReply
    {
        public MercenaryReply(NpcReplyRecord record) : base(record)
        {
        }

        public override bool Execute(Npc npc, Character character)
        {
            var item = character.Inventory.TryGetItem(ItemManager.Instance.TryGetTemplate(12124));
            if (item != null)
            {
                if (item.Stack >= 100)
                {
                    character.ChangeAlignementSide(AlignmentSideEnum.ALIGNMENT_MERCENARY);
                    character.Inventory.UnStackItem(item, 100);
                    character.Client.Account.Tokens = (int)item.Stack;
                    if(IPCAccessor.Instance.IsConnected)
                    {
                        IPCAccessor.Instance.Send(new UpdateTokensMessage(character.Client.Account.Tokens, character.Client.Account.Id));
                    }
                }
                else
                {
                    character.SendServerMessage("No tienes los suficientes puntos boutique! ");
                }
            }
            else
            {
                character.SendServerMessage("No tienes los suficientes puntos boutique!");
            }
            return true;
        }
    }
}
