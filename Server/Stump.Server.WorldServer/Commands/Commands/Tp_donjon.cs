using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Commands
{
	class Tp_Donjon : InGameCommand
	{
		private readonly List<Tuple<string, int, short, string>> DestinitationName = new List<Tuple<string, int, short, string>>
	{

		new Tuple<string, int, short , string>("rm", 55050240, 284,"Donjon Royalmouth"),
		new Tuple<string, int, short , string>("mr", 56098816, 477,"Donjon Mansot Royal"),
		new Tuple<string, int, short , string>("ben", 56360960, 228,"Donjon Ben le Ripate"),
		new Tuple<string, int, short , string>("sakai", 57934593, 346,"Donjon Sakai"),
		new Tuple<string, int, short , string>("korri", 62915584, 171,"Donjon Korriandre"),
		new Tuple<string, int, short , string>("glour", 62131720, 304,"Donjon gloursonne"),
		new Tuple<string, int, short , string>("kolosso", 61998084, 409,"Donjon kolosso"),
		new Tuple<string, int, short , string>("obsi", 57149697, 313,"Donjon de l'Obsidiantre"),
		new Tuple<string, int, short , string>("givrefoux", 59511808, 316,"Donjon Givrefoux"),
		new Tuple<string, int, short , string>("comte", 112198145, 383,"Donjon comte"),
		new Tuple<string, int, short , string>("missiz", 112198145, 383,"Donjon missiz"),
		new Tuple<string, int, short , string>("klime", 110362624, 248,"Donjon klime"),
		new Tuple<string, int, short , string>("nileza", 109576705, 355,"Donjon nileza"),
		new Tuple<string, int, short , string>("sylargh", 110100480, 355,"Donjon sylargh"),
		new Tuple<string, int, short , string>("dc", 72353792, 372,"Donjon dc"),
		new Tuple<string, int, short , string>("krala", 26738688, 326,"Donjon krala"),
		new Tuple<string, int, short , string>("kankreblath", 146675712, 300,"Donjon kankreblath"),
		new Tuple<string, int, short , string>("arche", 22282240, 410,"Donjon arche"),
		new Tuple<string, int, short , string>("kimbo", 21495808, 246,"Donjon kimbo"),
		new Tuple<string, int, short , string>("koulosse", 107216896, 414,"Donjon koulosse"),
		new Tuple<string, int, short , string>("chene", 149423104, 356,"Donjon chene"),
		new Tuple<string, int, short , string>("kardorim", 152829952, 289,"Donjon kardorim"),
		new Tuple<string, int, short , string>("domaine", 149684224, 372,"Donjon domaine"),
		new Tuple<string, int, short , string>("blop", 166986752, 255,"Donjon clos des blops"),
		new Tuple<string, int, short , string>("bouftou", 121373185, 344,"Donjon bouftou"),
		new Tuple<string, int, short , string>("champs", 105381888, 245,"Donjon champs"),
		new Tuple<string, int, short , string>("croca", 27787264, 123,"Donjon croca"),
		new Tuple<string, int, short , string>("forgerons", 87295489, 356,"Donjon forgerons"),
		new Tuple<string, int, short , string>("ratsbonta", 27000832, 400,"Donjon rats bonta"),
		new Tuple<string, int, short , string>("ratsbrakmar", 40108544, 341,"Donjon rats brakmar"),
		new Tuple<string, int, short , string>("ratsamakna", 102760961, 358,"Donjon rats amakna"),
		new Tuple<string, int, short , string>("scara", 94110720, 341,"Donjon scara"),
		new Tuple<string, int, short , string>("squelette", 87033344, 370,"Donjon squelette"),
		new Tuple<string, int, short , string>("bworker", 104333825, 356,"Donjon bworker"),
		new Tuple<string, int, short , string>("rasboul", 22808576, 257,"Donjon rasboul"),
		new Tuple<string, int, short , string>("hesque", 161295, 315,"Donjon hesque"),
		new Tuple<string, int, short , string>("kwakwa", 64749568, 399,"Donjon kwakwa"),
		new Tuple<string, int, short , string>("skeunk", 107481088, 174,"Donjon skeunk"),
		new Tuple<string, int, short , string>("baleine", 140771328, 256,"Donjon baleine"),
		new Tuple<string, int, short , string>("rdv", 137101312, 287,"Donjon reinevoleur"),
		new Tuple<string, int, short , string>("ekarlate", 136578048, 356,"Donjon ekarlate"),
		new Tuple<string, int, short , string>("vortex", 143393281, 298,"Donjon vortex"),
		new Tuple<string, int, short , string>("horologium", 143917569, 328,"Donjon horologium"),
		new Tuple<string, int, short , string>("fraktale", 143138823, 329,"Donjon fraktale"),
		new Tuple<string, int, short , string>("chouque", 157024256, 205,"Donjon chouque"),
		new Tuple<string, int, short , string>("kanniboul", 157548544, 288,"Donjon kanniboul"),
		new Tuple<string, int, short , string>("ombre", 123207680, 451,"Donjon ombre"),
		new Tuple<string, int, short , string>("aquadome", 119276033, 311,"Donjon aquadome"),
		new Tuple<string, int, short , string>("nidas", 129500160, 314,"Donjon nidas"),
		new Tuple<string, int, short , string>("phossile", 130548736, 297,"Donjon phossile"),
		new Tuple<string, int, short , string>("toxoliath", 136841216, 356,"Donjon toxoliath"),
		new Tuple<string, int, short , string>("ensable", 105644032, 326,"Donjon ensable"),
		new Tuple<string, int, short , string>("coco", 166988802, 262,"Donjon coco"),
		new Tuple<string, int, short , string>("reinette", 166987778, 262,"Donjon reinette"),
		new Tuple<string, int, short , string>("indigo", 166985730, 262,"Donjon indigo"),
		new Tuple<string, int, short , string>("griotte", 166986754, 262,"Donjon griotte"),
		new Tuple<string, int, short , string>("tofu", 96338946, 344,"Donjon tofu"),
		new Tuple<string, int, short , string>("tofuroyal", 96338948, 327,"Donjon tofuroyal"),
		new Tuple<string, int, short , string>("larve", 96994817, 358,"Donjon larve"),
		new Tuple<string, int, short , string>("bwork", 104595971, 342,"Donjon bwork"),
		new Tuple<string, int, short , string>("bulbes", 17564931, 150,"Donjon bulbes"),
		new Tuple<string, int, short , string>("moon",  157286400, 274,"Donjon moon"),
		new Tuple<string, int, short , string>("mallefisk", 130286592, 371,"Donjon mallefisk"),
		new Tuple<string, int, short , string>("donjonnowel", 66585088, 355,"Donjon donjonnowel"),
		new Tuple<string, int, short , string>("cavernenowel", 66846720, 357,"Donjon cavernenowel"),
		new Tuple<string, int, short , string>("papanowel", 66322432, 384,"Donjon papanowel"),
		new Tuple<string, int, short , string>("fanto", 163578368, 356,"Donjon fanto"),
		new Tuple<string, int, short , string>("gelax", 98566657, 356,"Donjon gelax"),
		new Tuple<string, int, short , string>("chaloeil", 160564224, 288,"Donjon chaloeil"),
		new Tuple<string, int, short , string>("pounicheur", 161743872, 299,"Donjon pounicheur"),
		new Tuple<string, int, short , string>("ush", 162004992, 383,"Donjon ush"),
		};

		public Tp_Donjon()
		{
			Aliases = new[]
			{
				"donjon"
			};
			RequiredRole = RoleEnum.Player;
			Description = "Cette commande (.Donjon) permet de voir les donjons qui sont disponibles et si vous effectuez cette commande .dj suivie du nom du donjon,vous allez être téléporté au donjon en question.";
			AddParameter<string>("Destination");
		}

		public override void Execute(GameTrigger trigger)
		{
			string saisie = trigger.Get<string>("Destination");
			if (saisie != null)
			{
				if (!trigger.Character.IsFighting())
				{
					Tuple<string, int, short, string> destination = DestinitationName.FirstOrDefault(x => x.Item1 == saisie.ToUpper());
					if (destination != null)
					{
						trigger.Character.Teleport(new ObjectPosition(World.Instance.GetMap(destination.Item2), destination.Item3));
						trigger.Character.SendServerMessage(string.Format("Vous avez été téléporté au {0} ", destination.Item4));
					}
				}
				else
					trigger.ReplyError("Pour lancer cette commande, il faut être en dehors d'un combat. ");
			}
			else
			{
				trigger.Character.SendServerMessage("les donjons disponibles sont :");
				foreach (Tuple<string, int, short, string> dj in DestinitationName)
				{
					trigger.Character.SendServerMessage(string.Format("{0} faites .dj {1}", dj.Item4, dj.Item1.ToLower()));
				}
			}
		}
	}
}
