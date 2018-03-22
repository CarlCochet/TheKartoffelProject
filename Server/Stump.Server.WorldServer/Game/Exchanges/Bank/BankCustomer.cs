﻿using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System.Collections.Generic;

namespace Stump.Server.WorldServer.Game.Exchanges.Bank
{
    public class BankCustomer : Exchanger
    {
        public BankCustomer(Character character, BankDialog dialog)
            : base(dialog)
        {
            Character = character;
        }

        public Character Character
        {
            get;
        }

        public override bool MoveItem(int id, int quantity)
        {
            if (quantity > 0)
            {
                var item = Character.Inventory.TryGetItem(id);

                return item != null && Character.Bank.StoreItem(item, quantity, true) != null;
            }

            if (quantity >= 0)
                return false;

            var deleteItem = Character.Bank.TryGetItem(id);

            return Character.Bank.TakeItemBack(deleteItem, -quantity, true) != null;
        }

        public override bool SetKamas(int amount)
        {            
            if (amount > 0)
                return Character.Bank.StoreKamas(amount);

            return amount < 0 && Character.Bank.TakeKamas(-amount);
        }

        public void MoveItems(bool store, IEnumerable<int> guids, bool all, bool existing)
        {
            if (store)
                Character.Bank.StoreItems(guids, all, existing);
            else
                Character.Bank.TakeItemsBack(guids, all, existing);
        }
    }
}