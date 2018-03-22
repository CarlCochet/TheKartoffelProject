using Stump.Server.WorldServer.Commands.Commands.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.Server.WorldServer.Commands.Trigger;

namespace Commands
{
	class HonorCommand : InGameCommand
	{
		public HonorCommand()
		{
			Aliases = new string[] { "Honor" };
			Description = "Cette commande permet de add les points honeur";
			RequiredRole = Stump.DofusProtocol.Enums.RoleEnum.Administrator;
			AddParameter<ushort>("Points");
		}
		public override void Execute(GameTrigger trigger)
		{
			var saisie = trigger.Get<ushort>("Points");
			trigger.Character.AddHonor(saisie);
			trigger.Character.SendServerMessage(string.Format("Vous avez add {0} points d'honor.", saisie));
		}
	}
}