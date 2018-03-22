using Database;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.UtilisationTokens
{
    class UtilisationTokenManager
    {
        private UtilisationTokenRecord record;
        public UtilisationTokenManager(Character character, string TransactionType = "", int ItemId = 0, int ItemStack = 0, int TokenAvantAchat = 0, int TokenApreAchat = 0, int Token = 0)
        {
            try
            {
                string tokenName = "Ogrina";
                if (character != null)
                {
                    if (Token > 0)
                    {
                        if (Token == 10275)
                            tokenName = "Chapa";
                        else if (Token == 12736)
                            tokenName = "Kolificha";
                    }
                    else if (TokenApreAchat == 0)
                        TokenApreAchat = character.Inventory.Tokens != null ? (int)character.Inventory.Tokens.Stack : 0;
                    record = new UtilisationTokenRecord();
                    record.OwnerName = character.Name;
                    record.OwnerId = character.Id;
                    record.Time = DateTime.Now;
                    record.TransactionType = TransactionType;
                    record.itemId = ItemId;
                    record.ItemStack = ItemStack;
                    record.TokenAvantAchat = TokenAvantAchat;
                    record.TokenApreAchat = TokenApreAchat;
                    record.TokenName = tokenName;
                    WorldServer.Instance.DBAccessor.Database.Insert(record);
                }
            }
            catch { }
        }
    }
}

