using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Challenges.Checkers {
    public class SaverChallenge : ChallengeChecker {
        private readonly Dictionary<int, List<int>> _castedSpells = new Dictionary<int, List<int>>();
        // CONSTRUCTORS
        public SaverChallenge() {}

        private SaverChallenge(ChallengeChecker pattern, Fight fight)
            : base(pattern, fight) {}

        public override ushort ChallengeId => 5;

        public override bool IsCompatible(Fight fight) {
            return true;
        }

        public override ChallengeChecker BuildChallenge(Fight fight) {
            return new SaverChallenge(this, fight);
        }

        //METHODS
        public override void BindEvents() {
            m_fight.FightEnded += Fight_FightEnded;
            foreach (var character in m_fight.RedTeam.GetAllFighters<CharacterFighter>()) {
                //character.SpellCasted += Character_SpellCasted;
            }
        }

        private void Character_SpellCasted(FightActor caster, Spell spell, Cell target,
            FightSpellCastCriticalEnum critical, bool silentcast) {
            if (!_castedSpells.ContainsKey(caster.Id)) {
                _castedSpells.Add(caster.Id, new List<int> {spell.Id});
            }
            else {
                var actorSpells = _castedSpells[caster.Id];
                if (actorSpells.Contains(spell.Id))
                    ChallengeFailed();
                else
                    actorSpells.Add(spell.Id);
            }
        }

        public override void UnbindEvents() {
            m_fight.FightEnded -= Fight_FightEnded;
            foreach (var character in m_fight.RedTeam.GetAllFighters<CharacterFighter>()) {
                //character.SpellCasted -= Character_SpellCasted;
            }
        }

        private void Fight_FightEnded(Fight obj) {
            ChallengeSuccessful();
        }
    }
}