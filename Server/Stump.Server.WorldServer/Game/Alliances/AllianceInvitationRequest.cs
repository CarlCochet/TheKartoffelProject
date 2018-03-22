using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Handlers.Alliances;
using Stump.Server.WorldServer.Handlers.Guilds;

namespace Stump.Server.WorldServer.Game.Guilds
{
	public class AllianceInvitationRequest : RequestBox
	{
		public AllianceInvitationRequest(Character source, Character target) : base(source, target)
		{
		}
		protected override void OnOpen()
		{
			AllianceHandler.SendAllianceInvitationStateRecruterMessage(Source.Client, Target, GuildInvitationStateEnum.GUILD_INVITATION_SENT);
            AllianceHandler.SendAllianceInvitationStateRecrutedMessage(Target.Client, GuildInvitationStateEnum.GUILD_INVITATION_SENT);
            AllianceHandler.SendAllianceInvitedMessage(Target.Client, Source);
		}
		protected override void OnAccept()
		{
            if (Source.Guild?.Alliance != null && Target.Guild?.Alliance == null)
            {
                if (Source.Guild.Alliance.TryAddGuild(Target.Guild))
                {
                    AllianceHandler.SendAllianceInvitationStateRecruterMessage(Source.Client, Target, GuildInvitationStateEnum.GUILD_INVITATION_OK);
                    AllianceHandler.SendAllianceInvitationStateRecrutedMessage(Target.Client, GuildInvitationStateEnum.GUILD_INVITATION_OK);
                }
                else
                {
                    AllianceHandler.SendAllianceInvitationStateRecruterMessage(Source.Client, Target, GuildInvitationStateEnum.GUILD_INVITATION_CANCELED);
                    AllianceHandler.SendAllianceInvitationStateRecruterMessage(Target.Client, Target, GuildInvitationStateEnum.GUILD_INVITATION_CANCELED);
                }
            }			        
		}
		protected override void OnDeny()
		{
            AllianceHandler.SendAllianceInvitationStateRecruterMessage(Source.Client, Target, GuildInvitationStateEnum.GUILD_INVITATION_CANCELED);
            AllianceHandler.SendAllianceInvitationStateRecruterMessage(Target.Client, Target, GuildInvitationStateEnum.GUILD_INVITATION_CANCELED);
		}
		protected override void OnCancel()
		{
            AllianceHandler.SendAllianceInvitationStateRecruterMessage(Source.Client, Target, GuildInvitationStateEnum.GUILD_INVITATION_CANCELED);
            AllianceHandler.SendAllianceInvitationStateRecruterMessage(Target.Client, Target, GuildInvitationStateEnum.GUILD_INVITATION_CANCELED);
		}
	}
}
