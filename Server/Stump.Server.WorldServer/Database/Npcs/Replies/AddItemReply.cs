using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using System;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [Discriminator("AddItem", typeof(NpcReply), new Type[] { typeof(NpcReplyRecord) })]
    public class AddItemReply : NpcReply
    {
        private ItemTemplate m_itemTemplate;

        public int ItemId
        {
            get
            {
                return this.Record.GetParameter<int>(0U, false);
            }
            set
            {
                this.Record.SetParameter<int>(0U, value);
            }
        }

        public ItemTemplate Item
        {
            get
            {
                ItemTemplate itemTemplate;
                if ((itemTemplate = this.m_itemTemplate) == null)
                    itemTemplate = this.m_itemTemplate = Singleton<ItemManager>.Instance.TryGetTemplate(this.ItemId);
                return itemTemplate;
            }
            set
            {
                this.m_itemTemplate = value;
                this.ItemId = value.Id;
            }
        }

        public int Amount
        {
            get
            {
                return this.Record.GetParameter<int>(1U, false);
            }
            set
            {
                this.Record.SetParameter<int>(1U, value);
            }
        }

        public AddItemReply(NpcReplyRecord record)
            : base(record)
        {
        }

        public override bool Execute(Npc npc, Character character)
        {
            bool flag;
            if (!base.Execute(npc, character))
            {
                flag = false;
            }
            else
            {
                BasePlayerItem playerItem = Singleton<ItemManager>.Instance.CreatePlayerItem(character, Item, Amount, false);
                if (playerItem == null)
                {
                    flag = false;
                }
                else
                {
                    ((ItemsCollection<BasePlayerItem>)character.Inventory).AddItem(playerItem);
                    character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, (short)21, (object)this.Amount, (object)playerItem.Template.Id);
                    flag = true;
                }
            }
            return flag;
        }
    }
}