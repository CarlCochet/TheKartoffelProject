using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;
using System;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public class SpellBuff : Buff
    {
        public SpellBuff(int id, FightActor target, FightActor caster, SpellEffectHandler effect, Spell spell, Spell boostedSpell, short boost, bool critical, FightDispellableEnum dispelable)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            BoostedSpell = boostedSpell;
            Boost = boost;
        }

        public Spell BoostedSpell
        {
            get;
        }

        public short Boost
        {
            get;
        }

        public override void Apply()
        {
            base.Apply();
            Target.BuffSpell(BoostedSpell, Boost);
        }

        public override void Dispell()
        {
            base.Dispell();
            Target.UnBuffSpell(BoostedSpell, Boost);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            if (Delay == 0)
                return new FightTemporarySpellBoostEffect(Id, Target.Id, Duration, (sbyte)Dispellable, (short) Spell.Id, EffectFix?.ClientEffectId ?? Effect.Id, 0, Boost, (short) BoostedSpell.Id);

            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, Delay,
                (sbyte)Dispellable,
                (short)Spell.Id, EffectFix?.ClientEffectId ?? Effect.Id, 0,
                (values.Length > 0 ? Convert.ToInt32(values[0]) : 0),
                (values.Length > 1 ? Convert.ToInt32(values[1]) : 0),
                (values.Length > 2 ? Convert.ToInt32(values[2]) : 0),
                Delay);
        }
    }
}