﻿using System;
using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Game;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class MuteCommand : TargetCommand
    {
        public MuteCommand()
        {
            Aliases = new[] { "mute" };
            RequiredRole = RoleEnum.Moderator;
            AddTargetParameter();
            AddParameter("time", "time", "Mute for x minutes", 5);
        }

        public override void Execute(TriggerBase trigger)
        {
            foreach (var target in GetTargets(trigger))
            {
                var time = trigger.Get<int>("time");

                target.Mute(TimeSpan.FromMinutes(time), trigger.User as Character);
                trigger.Reply("{0} muted", target.Name);
                target.OpenPopup(string.Format("★Vous avez été privé de parole.", target.Name, 25));
                World.Instance.SendAnnounce("★(Modératio) Le joueur " + target.Name + " à été privé de parole", Color.Orange);

            }
        }
    }

    public class UnMuteCommand : TargetCommand
    {
        public UnMuteCommand()
        {
            Aliases = new[] { "unmute" };
            RequiredRole = RoleEnum.Moderator;
            AddTargetParameter();
        }

        public override void Execute(TriggerBase trigger)
        {
            foreach (var target in GetTargets(trigger))
            {
                target.UnMute();
                trigger.Reply("{0} unmuted", target.Name);
            }
        }
    }

    public class MuteMapCommand : CommandBase
    {
        public MuteMapCommand()
        {
            Aliases = new[] { "mutemap" };
            RequiredRole = RoleEnum.Moderator;
        }

        public override void Execute(TriggerBase trigger)
        {
            var map = ((GameTrigger) trigger).Character.Map;
            var mute = map.ToggleMute();

            var message = mute
                ? "★ (Map) La carte est maintenant réduite au silence !"
                : "★ (Map) La carte n'est plus réduite au silence !";

            BasicHandler.SendTextInformationMessage(map.Clients, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 0,
                string.Format("<font col" +
                              "or=\"#{0}\">{1}</font>", Color.Red.ToArgb().ToString("X"), message));
        }
    }
}