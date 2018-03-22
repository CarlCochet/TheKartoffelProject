using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Alliances;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Handlers.Alliances;
using Stump.Server.WorldServer.Handlers.Dialogs;

namespace Game.Dialogs.Alliances
{
    public class AllianceCreationPanel : IDialog
    {
        // FIELDS

        // PROPERTIES
        public Character Character { get; private set; }
        public DialogTypeEnum DialogType { get { return DialogTypeEnum.DIALOG_ALLIANCE_CREATE; } }

        // CONSTRUCTORS
        public AllianceCreationPanel(Character character)
        {
            this.Character = character;
        }

        // METHODS
        public void Open()
        {
            if (this.Character.Guild == null || 
                this.Character.Guild.Alliance != null ||
                this.Character.Guild.Boss.Character != this.Character)
            {
                AllianceHandler.SendAllianceCreationResultMessage(this.Character.Client, SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_ERROR_REQUIREMENT_UNMET);
            }
            else
            {
                this.Character.SetDialog(this);

                AllianceHandler.SendAllianceCreationStartedMessage(this.Character.Client);
            }
        }

        public void CreateAlliance(string allianceName, string allianceTag, Stump.DofusProtocol.Types.GuildEmblem emblem)
        {
            var result = Singleton<AllianceManager>.Instance.CreateAlliance(this.Character, allianceName, allianceTag, emblem);

            AllianceHandler.SendAllianceCreationResultMessage(this.Character.Client, result);

            if (result == SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_OK)
            {
                this.Close();
            }
        }

        public void Close()
        {
            this.Character.CloseDialog(this);

            DialogHandler.SendLeaveDialogMessage(this.Character.Client, this.DialogType);
        }
    }
}