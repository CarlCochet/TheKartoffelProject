using Game.Dialogs.Alliances;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Alliances;
using Stump.Server.WorldServer.Game.Guilds;
using System.Linq;

namespace Stump.Server.WorldServer.Handlers.Alliances
{
	public class AllianceHandler : WorldHandlerContainer
	{
		private AllianceHandler() { }

		[WorldHandler(AllianceCreationValidMessage.Id)]
		public static void HandleAllianceCreationValidMessage(WorldClient client, AllianceCreationValidMessage message)
		{
			var allianceCreationPanel = client.Character.Dialog as AllianceCreationPanel;
			allianceCreationPanel?.CreateAlliance(message.allianceName, message.allianceTag, message.allianceEmblem);
		}
		[WorldHandler(AllianceInsiderInfoRequestMessage.Id)]
		public static void HandleAllianceInsiderInfoRequestMessage(WorldClient client, AllianceInsiderInfoRequestMessage message)
		{
			if (client.Character.Guild?.Alliance != null)
			{
				SendAllianceInsiderInfoMessage(client, client.Character.Guild.Alliance);
			}
		}

		[WorldHandler(GuildFactsRequestMessage.Id)]
		public static void HandleGuildFactsRequestMessage(WorldClient client, GuildFactsRequestMessage message)
		{
			var guild = Singleton<GuildManager>.Instance.TryGetGuild((int)message.guildId);
			if (guild != null)
			{
				SendGuildFactsMessage(client, guild);
			}
		}
		[WorldHandler(AllianceFactsRequestMessage.Id)]
		public static void HandleAllianceFactsRequestMessage(WorldClient client, AllianceFactsRequestMessage message)
		{
			var alliance = Singleton<AllianceManager>.Instance.TryGetAlliance((int)message.allianceId);
			if (alliance != null)
			{
				SendAllianceFactsMessage(client, alliance);
			}
		}
		[WorldHandler(SetEnableAVARequestMessage.Id)]
		public static void HandleSetEnableAVARequestMessage(WorldClient client, SetEnableAVARequestMessage message)
		{
		}

		[WorldHandler(AllianceInvitationMessage.Id)]
		public static void HandleGuildInvitationMessage(WorldClient client, AllianceInvitationMessage message)
		{
			if (client.Character.Guild != null)
			{
				if (!client.Character.Guild.Boss.IsBoss != true)
				{
					client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 207);
				}
				else
				{
					var character = Singleton<World>.Instance.GetCharacter((int)message.targetId);
					if (character == null)
					{
						client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 208);
					}
					else
					{
						if (character.Guild.Alliance != null)
						{
							client.Character.SendServerMessage("Ce joueur appartient déjà à une Alliance.");
						}
						else
						{
							if (character.IsBusy())
							{
								client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR,
									209);
							}
							else
							{
								//if (!client.Character.Guild.CanAddMember())
								//{
								//    client.Character.SendInformationMessage(
								//        TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 55, Guild.MaxMembersNumber);
								//}
								//else
								//{
								var allianceInvitationRequest = new AllianceInvitationRequest(client.Character, character);
								allianceInvitationRequest.Open();
								//}
							}
						}
					}
				}
			}
		}

		public static void SendAllianceCreationStartedMessage(IPacketReceiver client)
		{
			client.Send(new AllianceCreationStartedMessage());
		}
		public static void SendAllianceCreationResultMessage(IPacketReceiver client, SocialGroupCreationResultEnum result)
		{
			client.Send(new AllianceCreationResultMessage((sbyte)result));
		}
		public static void SendAllianceInsiderInfoMessage(IPacketReceiver client, Alliance alliance)
		{
			client.Send(new AllianceInsiderInfoMessage(
				alliance.GetAllianceFactSheetInformations(),
				alliance.GetGuildsInformations(),
				alliance.GetPrismsInformations()));
		}
		public static void SendGuildFactsMessage(IPacketReceiver client, Guild guild)
		{
			client.Send(new GuildFactsMessage(
				new GuildFactSheetInformations((uint)guild.Id, guild.Name, guild.Level, guild.Emblem.GetNetworkGuildEmblem(), (uint)guild.Boss.Id, (ushort)guild.Members.Count),
				guild.Record.CreationDate.GetUnixTimeStamp(),
				(ushort)guild.TaxCollectors.Count,
				false,
				from x in guild.Members
				where x.Character != null //TODO A null exception
				select new CharacterMinimalInformations((uint)x.Id, x.Character.Level, x.Character.Name)));
		}
		public static void SendAllianceJoinedMessage(IPacketReceiver client, Alliance alliance)
		{
			client.Send(new AllianceJoinedMessage(
				new AllianceInformations((uint)alliance.Id, alliance.Tag, alliance.Name, alliance.Emblem.GetNetworkGuildEmblem()),
				false));
		}
		public static void SendAllianceMembershipMessage(IPacketReceiver client, Alliance alliance)
		{
			client.Send(new AllianceMembershipMessage(alliance.GetAllianceInformations(), false));
		}
		public static void SendAllianceFactsMessage(IPacketReceiver client, Alliance alliance)
		{
			client.Send(new AllianceFactsMessage(
				alliance.GetAllianceFactSheetInformations(),
				alliance.GetGuildsInAllianceInformations(),
				alliance.Prisms.Select(x => (ushort)x.SubArea.Id),
				(uint)alliance.Boss.Boss.Id,
				alliance.Boss.Boss.Name));
		}

		[WorldHandler(AllianceInvitationAnswerMessage.Id)]
		public static void HandleAllianceInvitationAnswerMessage(WorldClient client, AllianceInvitationAnswerMessage message)
		{
			var allianceInvitationRequest = client.Character.RequestBox as AllianceInvitationRequest;
			if (allianceInvitationRequest != null)
			{
				if (client.Character == allianceInvitationRequest.Source && !message.accept)
				{
					allianceInvitationRequest.Cancel();
				}
				else
				{
					if (client.Character == allianceInvitationRequest.Target)
					{
						if (message.accept)
						{
							allianceInvitationRequest.Accept();
						}
						else
						{
							allianceInvitationRequest.Deny();
						}
					}
				}
			}
		}

		public static void SendAllianceInvitedMessage(IPacketReceiver client, Character recruter)
		{
			client.Send(new AllianceInvitedMessage((uint)recruter.Id, recruter.Name,
				recruter.Guild.Alliance.GetBasicAllianceNamedInformations()));
		}

		public static void SendAllianceInvitationStateRecrutedMessage(IPacketReceiver client,
			sbyte state)
		{
			client.Send(new AllianceInvitationStateRecrutedMessage(state));
		}

		public static void SendAllianceInvitationStateRecruterMessage(IPacketReceiver client, Character recruted,
			sbyte state)
		{
			client.Send(new AllianceInvitationStateRecruterMessage(recruted.Name, state));
		}

		public static void SendAllianceLeftMessage(IPacketReceiver client)
		{
			client.Send(new AllianceLeftMessage());
		}
	}
}
