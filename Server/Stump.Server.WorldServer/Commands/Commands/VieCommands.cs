using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Interactives;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Maps;
//By Geraxi
namespace Plug.Commands
{
    class AddLifeCommands : InGameCommand
    {
        public AddLifeCommands()
        {
            base.Aliases = new[] { "zaap" };
            base.RequiredRole = RoleEnum.Moderator;
        }
        public override void Execute(GameTrigger trigger)
        {
            /*Character player = trigger.Character;
            player.StopRegen();
            player.Stats.Health.DamageTaken = 0;
            player.RefreshStats();
            player.SendServerMessage("Vous avez récupéré tout vos points de vie.");*/

            InteractiveSpawn sp = new InteractiveSpawn() { MapId = trigger.Character.Map.Id, CellId = trigger.Character.Cell.Id, TemplateId = 16 };
            InteractiveObject x = new InteractiveObject(trigger.Character.Map, sp);
            ZaapDialog ZP = new ZaapDialog(trigger.Character, x, new List<Map> { World.Instance.GetMap(152044545)/*Start*/, World.Instance.GetMap(163054592)/*PNJ*/, World.Instance.GetMap(154010374)/*PvM*/, World.Instance.GetMap(17045510)/*Shop*/, World.Instance.GetMap(17041413)/*Prestige*/, World.Instance.GetMap(88212759)/*PvP*/, World.Instance.GetMap(84674563)/*Zaap Astrub*/, World.Instance.GetMap(101451267)/*Quete*/, World.Instance.GetMap(81788928) /*Koli*/ }.AsEnumerable<Map>());

            ZP.Open();
            // Change la map 1587
        }
    }
}
