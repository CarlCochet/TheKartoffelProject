using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Challenges.Checkers
{
    public class StatueChallenge : ChallengeChecker
    {
        // FIELDS
        private short m_cellId;

        // PROPERTIES
        public override ushort ChallengeId
        {
            get
            {
                return 2;
            }
        }

        // CONSTRUCTORS
        public StatueChallenge() : base() { }
        private StatueChallenge(ChallengeChecker pattern, Fight fight)
            : base(pattern, fight)
        { }

        // METHODS
        public override void BindEvents()
        {
            base.m_fight.FightEnded += this.Fight_FightEnded;
            foreach (var character in base.m_fight.RedTeam.GetAllFighters<CharacterFighter>())
            {
                character.TurnStarted += this.Character_TurnStarted;
                character.TurnPassed += this.Character_TurnPassed;
            }
        }

        public override void UnbindEvents()
        {
            base.m_fight.FightEnded -= this.Fight_FightEnded;
            foreach (var character in base.m_fight.RedTeam.GetAllFighters<CharacterFighter>())
            {
                character.TurnStarted -= this.Character_TurnStarted;
                character.TurnPassed -= this.Character_TurnPassed;
            }
        }

        #region Events

        private void Fight_FightEnded(Fight obj)
        {
            base.ChallengeSuccessful();
        }

        private void Character_TurnStarted(FightActor obj)
        {
            this.m_cellId = obj.Cell.Id;
        }

        private void Character_TurnPassed(FightActor obj)
        {
            if (this.m_cellId != obj.Cell.Id)
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
            return new StatueChallenge(this, fight);
        }
    }
}
