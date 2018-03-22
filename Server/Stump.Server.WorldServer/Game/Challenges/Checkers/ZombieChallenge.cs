using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Challenges.Checkers
{
    public class ZombieChallenge : ChallengeChecker
    {
        // FIELDS
        private ushort m_movementPoints;

        // PROPERTIES
        public override ushort ChallengeId
        {
            get
            {
                return 1;
            }
        }

        // CONSTRUCTORS
        public ZombieChallenge() : base() { }
        private ZombieChallenge(ChallengeChecker pattern, Fight fight)
            : base(pattern, fight)
        { }

        // METHODS
        public override void BindEvents()
        {
            base.m_fight.FightEnded += this.Fight_FightEnded;
            foreach (var character in base.m_fight.RedTeam.GetAllFighters<CharacterFighter>())
            {
                character.MpUsed += this.Character_MpUsed;
                character.TurnStarted += this.Character_TurnStarted;
                character.TurnPassed += this.Character_TurnPassed;
            }
        }

        public override void UnbindEvents()
        {
            base.m_fight.FightEnded -= this.Fight_FightEnded;
            foreach (var character in base.m_fight.RedTeam.GetAllFighters<CharacterFighter>())
            {
                character.MpUsed -= this.Character_MpUsed;
                character.TurnStarted -= this.Character_TurnStarted;
                character.TurnPassed -= this.Character_TurnPassed;
            }
        }

        #region Events

        private void Fight_FightEnded(Fight obj)
        {
            this.ChallengeSuccessful();
        }

        private void Character_MpUsed(FightActor arg1, short arg2)
        {
            this.m_movementPoints += (ushort)arg2;
            if (this.m_movementPoints > 1)
            {
                base.ChallengeFailed();
            }
        }

        private void Character_TurnStarted(FightActor obj)
        {
            this.m_movementPoints = 0;
        }

        private void Character_TurnPassed(FightActor obj)
        {
            if (this.m_movementPoints != 1)
            {
                base.ChallengeFailed();
            }
        }

        #endregion

        public override bool IsCompatible(Fight fight)
        {
            return true;
        }
        public override ChallengeChecker BuildChallenge(Fight fight)
        {
            return new ZombieChallenge(this, fight);
        }
    }
}
