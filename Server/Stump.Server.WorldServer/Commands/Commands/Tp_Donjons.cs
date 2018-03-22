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

namespace Stump.Server.WorldServer.Commands.Commands.Teleport
{
	public class TpDonjons : InGameCommand
	{
		public static void TpPlayer(Character player, int mapId, short cellId, DirectionsEnum playerDirection)
		{
			player.Teleport(new ObjectPosition(Singleton<World>.Instance.GetMap(mapId), cellId, playerDirection));
		}

		public TpDonjons()
		{
			Aliases = new[] { "dj" };
			RequiredRole = RoleEnum.Player;
			Description = "Taper .donjon pour voir la liste des Donjons  ! pour vous y teleporté taper .dj nom du donjon";
			AddParameter<string>("destination", "dest", "destination de la téléportation", null, false, null);
		}

		public override void Execute(GameTrigger trigger)
		{
			string destination = trigger.Get<string>("destination");
			var player = trigger.Character;
			switch (destination)
			{
				case "rm":
					TpPlayer(player, 55050240, 470, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "mr":
					TpPlayer(player, 56098816, 288, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case
				 "sakai":
					TpPlayer(player, 57934593, 313, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "ben":
					TpPlayer(player, 56360960, 343, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "obsi":
				    TpPlayer(player, 57149697, 344, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "korri":
				    TpPlayer(player, 62915584, 344, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
				case "glour":
				    TpPlayer(player, 62131720, 369, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "kolosso":
					TpPlayer(player, 61998084, 369, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "givrefoux":
					TpPlayer(player, 59511808, 316, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
			    case "comte":
					TpPlayer(player, 112198145, 383, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "missiz":
				TpPlayer(player, 109838849, 260, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "klime":
					TpPlayer(player, 110362624, 248, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "nileza":
					TpPlayer(player, 109576705, 355, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "sylargh":
					TpPlayer(player, 110100480, 355, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "blop":
					TpPlayer(player, 166986752, 205, DirectionsEnum.DIRECTION_SOUTH_EAST);
				    break;
				case
				 "dc":
				    TpPlayer(player, 72353792, 372, DirectionsEnum.DIRECTION_SOUTH_EAST);
				    break;
				case
				 "krala":
					TpPlayer(player, 26738688, 326, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "kankreblath":
					TpPlayer(player, 146675712, 300, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "arche":
					TpPlayer(player, 22282240, 410, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "kimbo":
					TpPlayer(player, 21495808, 246, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "koulosse":
					TpPlayer(player, 107216896, 414, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "chene":
				    TpPlayer(player, 149423104, 356, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "kardorim":
					TpPlayer(player, 152829952, 289, DirectionsEnum.DIRECTION_SOUTH_EAST);
				    break;
				case "domaine":
				    TpPlayer(player, 149684224, 372, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "bouftou":
				    TpPlayer(player, 121373185, 344, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "champs":
					TpPlayer(player, 105381888, 245, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "croca":
					TpPlayer(player, 27787264, 123, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "forgerons":
					TpPlayer(player, 87295489, 356, DirectionsEnum.DIRECTION_SOUTH_EAST);
				    break;
				case
				 "ratsbonta":
				    TpPlayer(player, 27000832, 400, DirectionsEnum.DIRECTION_SOUTH_EAST);
				    break;
				case
				 "ratsbrakmar":
				    TpPlayer(player, 40108544, 341, DirectionsEnum.DIRECTION_SOUTH_EAST);
				    break;
				case
				"ratsamakna":
				    TpPlayer(player, 102760961, 358, DirectionsEnum.DIRECTION_SOUTH_EAST);
				     break;
				case
					"scara":
				    TpPlayer(player, 94110720, 341, DirectionsEnum.DIRECTION_SOUTH_EAST);
				    break;
				case
				"squelette":
				    TpPlayer(player, 87033344, 370, DirectionsEnum.DIRECTION_SOUTH_EAST);
				    break;
					case
				 "bworker":
				TpPlayer(player, 104333825, 356, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				 "rasboul":
				TpPlayer(player, 22808576, 257, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				"hesque":
				TpPlayer(player, 161295, 315, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				 "kwakwa":
				TpPlayer(player, 64749568, 399, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				 "skeunk":
				TpPlayer(player, 107481088, 174, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				 "baleine":
				TpPlayer(player, 140771328, 256, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
				case "rdv":
					TpPlayer(player, 137101312, 287, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
					case
				"ekarlate":
				TpPlayer(player, 136578048, 356, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				"vortex":
				TpPlayer(player, 143393281, 298, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				"horologium":
				TpPlayer(player, 143917569, 328, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				"fraktale":
				TpPlayer(player, 143138823, 329, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				 "chouque":
				TpPlayer(player, 157024256, 205, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
				case 
				"kanniboul":
				TpPlayer(player, 157548544, 288, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
				case "ombre":
				TpPlayer(player, 123207680, 451, DirectionsEnum.DIRECTION_SOUTH_EAST);
			 break;
					case
				 "aquadome":
				TpPlayer(player, 119276033, 311, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
				case "nidas":
					TpPlayer(player, 129500160, 314, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
					case "phossile":
					TpPlayer(player, 130548736, 297, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "toxoliath":
					TpPlayer(player, 136841216, 356, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "ensable":
					TpPlayer(player, 105644032, 326, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "coco":
					TpPlayer(player, 166988802, 262, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				 "reinette":
					TpPlayer(player, 166987778, 262, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
					case "indigo":
				TpPlayer(player, 166985730, 262, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				 "griotte":
				TpPlayer(player, 166986754, 262, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
				case "tofu":
					TpPlayer(player, 96338946, 344, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "tofuroyal":
				TpPlayer(player, 96338948, 327, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				 "larve":
				TpPlayer(player, 96994817, 358, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
				case "bwork":
				TpPlayer(player, 104595971, 342, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
				case "bulbes":
				TpPlayer(player, 17564931, 150, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				 "moon":
				TpPlayer(player, 157286400, 274, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
				case "mallefisk":
				TpPlayer(player, 130286592, 371, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
				case "donjonnowel":
				TpPlayer(player, 66585088, 355, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				"cavernenowel":
				TpPlayer(player, 66846720, 357, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
				case "papanowel":
				TpPlayer(player, 66322432, 384, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
				case "fanto":
					TpPlayer(player, 163578368, 356, DirectionsEnum.DIRECTION_SOUTH_EAST);
					break;
					case
				 "gelax":
				TpPlayer(player, 98566657, 356, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				 "chaloeil":
				TpPlayer(player, 160564224, 288, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				 "pounicheur":
			TpPlayer(player, 161743872, 299, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;
					case
				 "ush":
				TpPlayer(player, 162004992, 383, DirectionsEnum.DIRECTION_SOUTH_EAST);
				break;

				//player.ToggleAway();
				default:
					player.SendServerMessage("Veuillez saisir après le dj soit .dj nidas pour vous teleporté au donjons nidas , pour voir la liste des donjons taper .donjon ,", System.Drawing.Color.OrangeRed);
					break;
			}
		}
	}
}