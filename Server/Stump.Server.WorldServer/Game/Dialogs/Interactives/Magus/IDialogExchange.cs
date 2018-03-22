using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Game.Dialogs.Interactives.Magus
{
    public interface IDialogExchange: IDialog
    {

        /// <summary>
        /// Manage Exchange Message
        /// </summary>
        /// <param name="message"></param>
        void Exchange(ExchangeObjectMoveMessage message);
    }
}