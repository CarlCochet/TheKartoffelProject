using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Core.Reflection;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
//using Stump.Server.WorldServer.Game.Effects.Spells.
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Ecaflip
{
    [SpellCastHandler(SpellIdEnum.PORTAL)]
    public class PortalSpawn : DefaultSpellCastHandler
    {
        public PortalSpawn(SpellCastInformations cast)
            : base(cast)
        {
        }

        public override void Execute()
        {
            var portalout = Fight.GetTriggers().FirstOrDefault(x => x is Portal && x.ContainsCell(TargetedCell)) as Portal;
            if (portalout != null && Fight.GetActivableGamePortalsCount(portalout.Caster.Team) < 2)
            {
            }
            else
            {
                //break;
                if (!m_initialized)
                    Initialize();
                EffectDice subap = new EffectDice((short)EffectsEnum.Effect_1081, 0, 0, 0, new EffectBase());
                SpellEffectHandler spellEffectHandler = Singleton<EffectManager>.Instance.GetSpellEffectHandler(subap, base.Caster, this, base.TargetedCell, false);
                spellEffectHandler.Apply();
            }
        }
    }
    [SpellCastHandler(SpellIdEnum.STUPOR_5386)]
    public class StuporSpawn : DefaultSpellCastHandler
    {
        public StuporSpawn(SpellCastInformations cast)
            : base(cast)
        {
        }

        public override void Execute()
        {
            var portalout = Fight.GetTriggers().FirstOrDefault(x => x is Portal && x.ContainsCell(TargetedCell)) as Portal;
            //break;
            if (!m_initialized)
                Initialize();
            EffectDice subap = new EffectDice((short)EffectsEnum.Effect_Trap, 0, 5359, 1, new EffectBase());
            SpellEffectHandler spellEffectHandler = Singleton<EffectManager>.Instance.GetSpellEffectHandler(subap, base.Caster, this, base.TargetedCell, false);
            spellEffectHandler.Dice.Value = 1;
            spellEffectHandler.Apply();
        }
    }
    [SpellCastHandler(SpellIdEnum.NEUTRAL_5381)]
    public class Neutral : DefaultSpellCastHandler
    {
        public Neutral(SpellCastInformations cast)
            : base(cast)
        {
        }

        public override void Execute()
        {
            var portalout = Fight.GetTriggers().FirstOrDefault(x => x is Portal && x.ContainsCell(TargetedCell)) as Portal;
            if (portalout != null)
            {
                if (portalout.Caster.Team == Caster.Team)
                {
                    portalout.IsNeutral = true;
                    portalout.Disable();
                }
            }
            Fight.RefreshClientsPortals();
        }
    }
}