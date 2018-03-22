﻿using Stump.Server.WorldServer.Game.Dialogs;

namespace Stump.Server.WorldServer.Game.Exchanges
{
    public abstract class Exchanger : IDialoger
    {
        private readonly IExchange m_exchange;

        protected Exchanger(IExchange exchange)
        {
            m_exchange = exchange;
        }

        public abstract bool MoveItem(int id, int quantity);
        public abstract bool SetKamas(int amount);

        public IDialog Dialog
        {
            get { return m_exchange; }
        }
    }
}