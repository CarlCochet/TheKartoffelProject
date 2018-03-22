using System.Drawing;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class AnnounceCommand : CommandBase
    {
        [Variable(true)]
        public static string AnnounceColor = ColorTranslator.ToHtml(Color.Orange);

        public AnnounceCommand()
        {
            Aliases = new[] { "announce", "a" };
            Description = "Display an announce to all players";
            RequiredRole = RoleEnum.Moderator;
            AddParameter<string>("message", "msg", "The announce");
        }

        public override void Execute(TriggerBase trigger)
        {
            var color = ColorTranslator.FromHtml(AnnounceColor);

            var msg = trigger.Get<string>("msg");
            var formatMsg = trigger is GameTrigger
                                ? string.Format("★{0} : (Staff Geraxi) {1}", ((GameTrigger)trigger).Character.Name, msg)
                                : string.Format("★{0} : (Staff Geraxi)", msg);


            World.Instance.SendAnnounce(formatMsg, color);
        }
    }
}