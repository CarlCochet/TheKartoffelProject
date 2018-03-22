using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
//By Geraxi
namespace Stump.Server.WorldServer.Commands.Commands
{
	public class sorts : InGameCommand
	{
		public sorts()
		{
			this.Aliases = new string[1]
			{
	"modospells"
			};
			this.RequiredRole = RoleEnum.GameMaster;
			this.Description = "Ajoute des sorts de modération et des sorts utiles";
		}

		public override void Execute(GameTrigger trigger)
		{
			trigger.Character.Spells.LearnSpell(411);
			trigger.Character.Spells.LearnSpell(390);
			trigger.Character.Spells.LearnSpell(391);
			trigger.Character.Spells.LearnSpell(392);
			trigger.Character.Spells.LearnSpell(394);
			trigger.Character.Spells.LearnSpell(396);
			trigger.Character.Spells.LearnSpell(397);
			trigger.Character.Spells.LearnSpell(415);
			trigger.Character.Spells.LearnSpell(728);
			trigger.Character.Spells.LearnSpell(350);
			trigger.Character.Spells.LearnSpell(364);
			trigger.Character.Spells.LearnSpell(214);
			trigger.Reply("Spells Learned !");
		}
	}
}