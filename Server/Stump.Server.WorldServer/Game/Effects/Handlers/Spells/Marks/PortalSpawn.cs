using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Fights.Teams;
using System.Drawing;
using NLog;
using Stump.Server.WorldServer.Core.Network;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Spells.Casts;
using System.Linq;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells
{
    [EffectHandler(EffectsEnum.Effect_1081)]
    public class PortalSpawn : SpellEffectHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public PortalSpawn(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical) : base(effect, caster, castHandler, targetedCell, critical)
        {
            SpellcastId = effect.Value;
        }
        public int SpellcastId;
        protected override bool InternalApply()
        {
            var spell = new Spell(5426, 1);
            if (spell.Template == null || !spell.ByLevel.ContainsKey(1))
            {
                Logger.Error(
                    "Cannot find glyph spell id = {0}, level = {1}. Casted Spell = {2}", Dice.DiceNum, Dice.DiceFace,
                    Spell.Id);
                return false;
            }
            if (!Caster.Fight.CanPortal(Caster.Team))
            {
                Caster.Fight.RemoveFirstPortal(Caster.Team);
            }
            var portal = new Portal((short)Fight.PopNextTriggerId(), Caster, Spell, TargetedCell, Dice, EffectZone.ShapeType, 0, 0, GetColorByTeam(Caster.Team), spell);
            Fight.AddTriger(portal);
            {
                Fight.RefreshClientsPortals();
            }
            return true;
        }

        private static Color GetColorByTeam(FightTeam team)
        {
            return team == team.Fight.ChallengersTeam ? Color.Red : Color.Blue;
        }
    }
}
