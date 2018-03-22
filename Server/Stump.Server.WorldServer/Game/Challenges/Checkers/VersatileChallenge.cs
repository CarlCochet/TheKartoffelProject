using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Challenges.Checkers {
    public class VersatileChallenge : ChallengeChecker {
        private readonly List<int> _castedSpells = new List<int>();
        // CONSTRUCTORS
        public VersatileChallenge() {}

        private VersatileChallenge(ChallengeChecker pattern, Fight fight)
            : base(pattern, fight) {}

        public override ushort ChallengeId => 6;

        public override bool IsCompatible(Fight fight) {
            return true;
        }

        public override ChallengeChecker BuildChallenge(Fight fight) {
            return new VersatileChallenge(this, fight);
        }

        //METHODS
        public override void BindEvents() {
            m_fight.FightEnded += Fight_FightEnded;
            foreach (var character in m_fight.RedTeam.GetAllFighters<CharacterFighter>()) {
                //character.SpellCasted += Character_SpellCasted;
                character.TurnPassed += Character_TurnPassed;
                character.TurnStarted += Character_TurnPassed;
            }
        }

        private void Character_TurnPassed(FightActor obj) {
            _castedSpells.Clear();
        }

        private void Character_SpellCasted(FightActor caster, Spell spell, Cell target,
            FightSpellCastCriticalEnum critical, bool silentcast) {
            if (_castedSpells.Contains(spell.Id))
                ChallengeFailed();
            else
                _castedSpells.Add(spell.Id);
        }

        public override void UnbindEvents() {
            m_fight.FightEnded -= Fight_FightEnded;
            foreach (var character in m_fight.RedTeam.GetAllFighters<CharacterFighter>()) {
                //character.SpellCasted -= Character_SpellCasted;
                character.TurnPassed -= Character_TurnPassed;
                character.TurnStarted -= Character_TurnPassed;
            }
        }

        private void Fight_FightEnded(Fight obj) {
            ChallengeSuccessful();
        }
    }
}