using Stump.Core.Threading;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Challenges.Checkers {
    public class ReprieveChallenge : ChallengeChecker {
        // FIELDS
        private readonly FightActor m_target;

        // CONSTRUCTORS
        public ReprieveChallenge() {}

        private ReprieveChallenge(ChallengeChecker pattern, Fight fight)
            : base(pattern, fight) {
            var random = new AsyncRandom();

            m_target = m_fight.BlueTeam.Fighters[random.Next(fight.BlueTeam.Fighters.Count)];
        }

        // PROPERTIES
        public override ushort ChallengeId => 4;

        public override int TargetId => m_target.Id;

        // METHODS
        public override void BindEvents() {
            m_fight.BlueTeam.FighterAdded += BlueTeam_FighterAdded;
            m_fight.BlueTeam.FighterRemoved += BlueTeam_FighterRemoved;
            foreach (var item in m_fight.BlueTeam.Fighters) {
                item.Dead += Fighter_Dead;
            }
        }

        public override void UnbindEvents() {
            m_fight.BlueTeam.FighterAdded -= BlueTeam_FighterAdded;
            m_fight.BlueTeam.FighterRemoved -= BlueTeam_FighterRemoved;
            foreach (var item in m_fight.BlueTeam.Fighters) {
                item.Dead -= Fighter_Dead;
            }
        }

        public override bool IsCompatible(Fight fight) {
            return fight.BlueTeam.Fighters.Count > 1;
        }

        public override ChallengeChecker BuildChallenge(Fight fight) {
            return new ReprieveChallenge(this, fight);
        }

        #region Handlers

        private void BlueTeam_FighterRemoved(FightTeam arg1, FightActor arg2) {
            arg2.Dead -= Fighter_Dead;
        }

        private void BlueTeam_FighterAdded(FightTeam arg1, FightActor arg2) {
            arg2.Dead += Fighter_Dead;
        }

        private void Fighter_Dead(FightActor arg1, FightActor arg2) {
            if (arg1 == m_target && m_fight.BlueTeam.AreAllDead())
                ChallengeSuccessful();
            else
                ChallengeFailed();
        }

        #endregion
    }
}