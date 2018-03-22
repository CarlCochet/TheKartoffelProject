using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;

namespace Stump.Server.WorldServer.Commands.Commands.Admin
{
    public class GiveLifeCommand : TargetCommand
    {
        public GiveLifeCommand()
        {
            Aliases = new[] {"givelife"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Permet de regénérer vos points de vie en combat";

        }

        public override void Execute(TriggerBase trigger)
        {
            var gameTrigger = trigger as GameTrigger;
            if (gameTrigger != null)
            {
                var character = gameTrigger.Character;
                if (character.IsInFight())
                {
                    character.Fighter.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_TRIGGERED);
                    character.Fighter.Heal(character.Stats.Health.TotalMax, character.Fighter);
                    character.Fighter.Fight.EndAllSequences();
                }
                else
                    trigger.ReplyBold("[INFO] Tu as besoin d'être en combat pour utiliser cette commande.");
            }
            else
                trigger.Reply("Impossible d'ajouter de la vitalité");
        }
    }
}