using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System;
using System.Drawing;

namespace Stump.Server.WorldServer.Commands.Commands
{
	public class Blamecommand : TargetCommand
	{
		public Blamecommand()
		{
			base.Aliases = new string[]
			{
				"avertissement"
			};
			base.RequiredRole = RoleEnum.Moderator;
			base.AddTargetParameter(false, "Defined target");
		}
		public override void Execute(TriggerBase trigger)
		{
			Character[] targets = base.GetTargets(trigger);
			for (int i = 0; i < targets.Length; i++)
			{
				Character character = targets[i];
                trigger.Reply("Vous avez blâmé le joueur {0} merci d'en référer à l'Administration", new object[]

                {
                    character.Name,
                });
                World.Instance.SendAnnounce("★(Modération) Le joueur " + character.Name + " à reçu un avertissement.", Color.Orange);
                character.OpenPopup(string.Format("★Vous avez reçu un avertissement pour mauvais comportement en jeu. Cette avertissement pourrait devenir à l'avenir une sanction.", character.Name, 20));
                character.SendServerMessage("Votre personnage " +  "<b>"  + character.Name  + "</b>"  +  " à été <b>averti</b> par la modération à l'avenir veuillez faire attention ! ", Color.Orange);
			}
		}
	}
}
