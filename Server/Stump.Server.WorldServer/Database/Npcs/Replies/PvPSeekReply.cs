﻿using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;
using System.Linq;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [Discriminator("PvPSeek", typeof(NpcReply), typeof(NpcReplyRecord))]
    public class PvPSeekReply : NpcReply
    {
        private TimedStack<Pair<int, int>> m_pvpSeekHistory = new TimedStack<Pair<int, int>>(60 * 10);

        public PvPSeekReply(NpcReplyRecord record)
            : base(record)
        {
        }

        public override bool Execute(Npc npc, Character character)
        {
            if (!base.Execute(npc, character))
                return false;

            //Check if player already get a contract in the last 10 mins
            m_pvpSeekHistory.Clean();
            if (m_pvpSeekHistory.Any(x => x.First.First == character.Id))
            {
                character.SendServerMessage("Vous avez eu une mission pour moins de <b> 10 minutes </ b>, s'il vous plaît réessayer plus tard.");
                return false;
            }

            if (character.AlignmentSide == AlignmentSideEnum.ALIGNMENT_NEUTRAL)
            {
                character.SendServerMessage("Vous devez avoir un <b> alignement </> pour commencer une mission.");
                return false;
            }

            if (!character.PvPEnabled)
            {
                character.SendServerMessage("Vous devez activer vos <b> Ailes </ b> pour pouvoir commencer une mission");
                return false;
            }

            if (character.Level < 20)
            {
                character.SendServerMessage("Vous devez être au moins <l> lvl 20 </ b> Pour pouvoir effectuer une mission");
                return false;
            }

            var target = Game.World.Instance.GetCharacters(x => character.CanAgress(x, true) == FighterRefusedReasonEnum.FIGHTER_ACCEPTED).RandomElementOrDefault();

            if (target == null)
            {
                character.SendServerMessage("Nous n'avons pas encore été en mesure de localiser un objectif à votre hauteur. Réessayez dans quelques minutes..");
                return false;
            }

            foreach (var contract in character.Inventory.GetItems(x => x.Template.Id == (int)ItemIdEnum.ORDRE_DEXECUTION_10085))
                character.Inventory.RemoveItem(contract);

            var item = ItemManager.Instance.CreatePlayerItem(character, (int)ItemIdEnum.ORDRE_DEXECUTION_10085, 25);

            var seekEffect = item.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_Seek);

            if (seekEffect != null)
                item.Effects.Remove(seekEffect);

            item.Effects.Add(new EffectString(EffectsEnum.Effect_Seek, target.Name));
            item.Effects.Add(new EffectInteger(EffectsEnum.Effect_Alignment, (short)target.AlignmentSide));
            item.Effects.Add(new EffectInteger(EffectsEnum.Effect_Grade, target.AlignmentGrade));
            item.Effects.Add(new EffectInteger(EffectsEnum.Effect_Level, target.Level));
            item.Effects.Add(new EffectInteger(EffectsEnum.Effect_NonExchangeable_981, 0));

            character.Inventory.AddItem(item);

            m_pvpSeekHistory.Push(new Pair<int, int>(character.Id, target.Id));

            character.SendServerMessage($"Maintenant, votre mission est: <b> {target.Name} </ b>, 25 parchemins de recherche vous ont été assignés pour localiser votre adversaire.");

            return true;
        }
    }
}