using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.Characters
{
    public partial class CharacterHandler
    {
        public static void SendLifePointsRegenBeginMessage(IPacketReceiver client, sbyte regenRate)
        {
            client.Send(new LifePointsRegenBeginMessage(regenRate));
        }

        public static void SendUpdateLifePointsMessage(WorldClient client)
        {
            client.Send(new UpdateLifePointsMessage(
                client.Character.Stats.Health.Total,
                client.Character.Stats.Health.TotalMax));
        }

        public static void SendLifePointsRegenEndMessage(WorldClient client, int recoveredLife)
        {
            client.Send(new LifePointsRegenEndMessage(
                client.Character.Stats.Health.Total,
                client.Character.Stats.Health.TotalMax,
                recoveredLife));
        }

        public static void SendCharacterStatsListMessage(WorldClient client)
        {
            client.Send(new CharacterStatsListMessage(client.Character.GetCharacterCharacteristicsInformations()));
        }

        public static void SendCharacterLevelUpMessage(IPacketReceiver client, sbyte level)
        {
            client.Send(new CharacterLevelUpMessage(level));
        }

        public static void SendCharacterLevelUpInformationMessage(IPacketReceiver client, Character character, sbyte level)
        {
            client.Send(new CharacterLevelUpInformationMessage(level, character.Name, character.Id));
        }

        public static void SendGameRolePlayPlayerLifeStatusMessage(IPacketReceiver client, PlayerLifeStatusEnum status, int phoenixMapId)
        {
            client.Send(new GameRolePlayPlayerLifeStatusMessage((sbyte)status, phoenixMapId));
        }

        public static void SendCharacterExperienceGainMessage(IPacketReceiver client, long experienceCharacter,
           long experienceMount, long experienceGuild, long experienceIncarnation)
        {
            client.Send(new CharacterExperienceGainMessage(experienceCharacter, experienceMount, experienceGuild,
                experienceIncarnation));
        }

    }
}