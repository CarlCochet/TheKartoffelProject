using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System;
using System.Threading;
//By Geraxi
namespace Stump.Server.WorldServer.Commands.Commands
{
	public class TimeREboot : TargetCommand
	{
		public TimeREboot()
		{
			base.Aliases = new string[]
			{
				"reboot"
			};
			base.Description = "reboot le serveur";
			base.RequiredRole = RoleEnum.Moderator;
			base.AddParameter<int>("time", "time", "Noire duration (in minutes)", 1, false, null);

		}
		public override void Execute(TriggerBase trigger)
		{
			string value = trigger.Args.PeekNextWord();
			if (string.IsNullOrEmpty(value))
			{
				if (trigger is GameTrigger)
				{
					int time = trigger.Get<int>("time");

					Singleton<World>.Instance.SendAnnounce("Reboot automatique du serveur dans 5 minute.", System.Drawing.Color.Orchid);
					Thread Restart =
						new Thread(
						  unused => SaveMinuteBefore(time * 60000)
						);
					Restart.Start();
				}
			}
			else
			{

			}
		}
		public void SaveMinuteBefore(int time)
		{
			Thread.Sleep(time - 40000);
			System.Predicate<Character> predicate = (Character x) => true;
			System.Collections.Generic.IEnumerable<Character> characters = Singleton<World>.Instance.GetCharacters(predicate);
			Singleton<World>.Instance.SendAnnounce("Reboot automatique du serveur dans 1 minutes.", System.Drawing.Color.Orchid);
			Thread.Sleep(50000);
			foreach (Character cr in characters)
			{
				cr.SaveLater();
				cr.SendServerMessage("Votre personnage à était sauvegardé.");
			}
			ServerBase<WorldServer>.Instance.IOTaskPool.AddMessage(new Action(Singleton<World>.Instance.Save));
			Singleton<World>.Instance.SendAnnounce("Reboot !!!", System.Drawing.Color.Orchid);
			Singleton<World>.Instance.SendAnnounce("Reboot !!!", System.Drawing.Color.Orchid);
			Singleton<World>.Instance.SendAnnounce("Reboot !!!", System.Drawing.Color.Orchid);
			Thread.Sleep(5000);
			Environment.Exit(1);
		}
	}
}