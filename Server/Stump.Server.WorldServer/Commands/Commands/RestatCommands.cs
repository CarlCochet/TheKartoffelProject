using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System.Drawing;
//By Geraxi

public class RestatCommands : TargetCommand
{
	public RestatCommands()
	{
		base.Aliases = new string[]
		{
				"Restat"
		};
		RequiredRole = RoleEnum.Player;
		Description = "Permet de vous restat.";
	}
	public override void Execute(TriggerBase trigger)
	{
		Character[] targets = GetTargets(trigger);
		for (int i = 0; i < targets.Length; i++)
		{
			Character character = targets[i];
			character.ResetStats(true);
			character.SendServerMessage("Votre personnage" + "<b> " + character.Name + " </b>" + "a été restat</b>.");
		}
	}
}