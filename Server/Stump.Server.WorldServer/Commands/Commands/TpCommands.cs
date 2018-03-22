using Accord.Statistics.Distributions.Univariate;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Maps.Cells;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
//By Geraxi tp pour bdd
namespace Stump.Server.WorldServer.Commands.Commands.Teleport
{
	public class TpCommands : InGameCommand
	{
		public static void tpPlayer(Character player, int mapId, short cellId, DirectionsEnum playerDirection)
		{
			player.Teleport(new ObjectPosition(Singleton<World>.Instance.GetMap(mapId), cellId, playerDirection));
		}

		public TpCommands()
		{
			Aliases = new[] { "tp" };
			RequiredRole = RoleEnum.Player;
			Description = "Les commandes .tp disponibles sont : start - pvm - sakai - sarakech - wabbits- vulkania - otomai - frigost - pvp - boutique - enclos.";
			AddParameter<string>("destination", "dest", "destination de la téléportation", null, false, null);
		}

		public override void Execute(GameTrigger trigger)
		{
			string destination = trigger.Get<string>("destination");
			var player = trigger.Character;
			switch (destination)
			{
				case "start":
					tpPlayer(player, 84674563, 328, DirectionsEnum.DIRECTION_SOUTH_WEST);
					break;
				case "boutique":
					tpPlayer(player, 76287501, 498, DirectionsEnum.DIRECTION_NORTH_WEST);
					break;
				case "pvm":
					tpPlayer(player, 66062340, 245, DirectionsEnum.DIRECTION_SOUTH_WEST);
					break;
				case "sakai":
					tpPlayer(player, 68422148, 343, DirectionsEnum.DIRECTION_NORTH_EAST);
					break;
				case "sarakech":
					tpPlayer(player, 68422145, 353, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "wabbits":
					tpPlayer(player, 68420613, 382, DirectionsEnum.DIRECTION_NORTH_EAST);
					break;
				case "vulkania":
					tpPlayer(player, 68420098, 355, DirectionsEnum.DIRECTION_NORTH_EAST);
					break;
				case "otomai":
					tpPlayer(player, 95420420, 231, DirectionsEnum.DIRECTION_SOUTH_WEST);
					break;
				case "frigost":
					tpPlayer(player, 167380484, 328, DirectionsEnum.DIRECTION_NORTH_EAST);
					break;
				//case "boutique":
				//	tpPlayer(player, 175112196, 441, DirectionsEnum.DIRECTION_NORTH_WEST);
				//	break;
				//case "shop":
				//	tpPlayer(player, 175112194, 239, DirectionsEnum.DIRECTION_SOUTH_EAST);
				//	break;
				//case "prestige":
				//tpPlayer(player, 17041413, 424, DirectionsEnum.DIRECTION_SOUTH_EAST);
				//break;
				case "pvp":
					tpPlayer(player, 88212759, 301, DirectionsEnum.DIRECTION_SOUTH_EAST);
					player.TogglePvPMode(true);
					break;
				case "enclos":
					tpPlayer(player, 149816, 136, DirectionsEnum.DIRECTION_SOUTH_EAST);
					player.TogglePvPMode(true);
					break;

				//player.ToggleAway();
				default:
					player.SendServerMessage("Veuillez saisir après le .tp soit start - pvm - sakai - sarakech - wabbits- vulkania - otomai - frigost - boutique, enclos ,", System.Drawing.Color.OrangeRed);
					break;
			}
		}
	}
}