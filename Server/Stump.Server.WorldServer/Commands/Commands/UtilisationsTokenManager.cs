using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Commands
{
	internal class UtilisationTokenManager
	{
		private Character character;
		private string transactionType;
		private int id;
		private int v;
		private int tokenAvantAchat;

		public UtilisationTokenManager(Character character, string transactionType, int id, int v, int tokenAvantAchat)
		{
			this.character = character;
			this.transactionType = transactionType;
			this.id = id;
			this.v = v;
			this.tokenAvantAchat = tokenAvantAchat;
		}
	}
}