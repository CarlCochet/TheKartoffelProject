using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
//By Geraxi
namespace Stump.Server.WorldServer.Commands.Commands
{
	public class PA : InGameCommand
	{
		public PA()
		{
			this.Aliases = new string[1]
			{
	"pa"
			};
			this.RequiredRole = RoleEnum.GameMaster;
			this.Description = "Ajoute les Points d'action";
			this.AddParameter<int>("pa", "pa", "Pa to add", 0, false, (ConverterHandler<int>)null);
		}

		public override void Execute(GameTrigger trigger)
		{
			int ap = trigger.Get<int>("pa");
			this.AddEffects(trigger, ap);
			trigger.Reply("Added !");
		}

		private void AddEffects(GameTrigger trigger, int ap)
		{
			Character character = trigger.Character;
			character.Stats.AP.Given = ap;
			character.RefreshStats();
		}
	}
}