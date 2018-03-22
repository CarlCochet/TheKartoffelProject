using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Basic;
using System.Drawing;
namespace Stump.Server.WorldServer.Commands.Commands
{
    public class TalkMap : CommandBase
    {
        [Variable(true)]
        public static string TalkMapColor = ColorTranslator.ToHtml(Color.Orange);
        public TalkMap()
        {
            base.Aliases = new string[]
            {
                "map"
            };
            base.RequiredRole = RoleEnum.Moderator;
            base.AddParameter<string>("message", "msg", "The announce", null, false, null);
        }
        public override void Execute(TriggerBase trigger)
        {
            Color color = ColorTranslator.FromHtml(TalkMap.TalkMapColor);
            string text = trigger.Get<string>("msg");
            Map map = ((GameTrigger)trigger).Character.Map;
            string arg = (trigger is GameTrigger) ? string.Format("★{0} : (Map) {1}", ((GameTrigger)trigger).Character.Name, text) : string.Format("(<b>Information</b>) {0}", text);
            BasicHandler.SendTextInformationMessage(map.Clients, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 0, new string[]
            {
                string.Format("<font color=\"#{0}\">{1}</font>", Color.Orange.ToArgb().ToString("X"), arg, text, color)
            });
        }
    }
}
