using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Effects;

namespace Stump.Server.WorldServer.Game.Challenges.Checkers
{
    public class UntouchableChallenge : ChallengeChecker
    {
        // FIELDS
        private short m_cellId;

        // PROPERTIES
        public override ushort ChallengeId
        {
            get
            {
                return 17;
            }
        }

        // CONSTRUCTORS
        public UntouchableChallenge() : base() { }
        private UntouchableChallenge(ChallengeChecker pattern, Fight fight)
            : base(pattern, fight)
        { }

        // METHODS
        public override void BindEvents()
        {
            base.m_fight.FightEnded += this.Fight_FightEnded;
            foreach (var character in base.m_fight.RedTeam.GetAllFighters<CharacterFighter>())
            {
                //character.LifePointsChanged += this.Character_LifePointsChanged;
            }
        }

        public override void UnbindEvents()
        {
            base.m_fight.FightEnded -= this.Fight_FightEnded;
            foreach (var character in base.m_fight.RedTeam.GetAllFighters<CharacterFighter>())
            {
                //character.LifePointsChanged -= this.Character_LifePointsChanged;
            }
        }

        #region Events

        private void Fight_FightEnded(Fight obj)
        {
            base.ChallengeSuccessful();
        }

        private void Character_LifePointsChanged(FightActor actor, EffectSchoolEnum damage, int delta, int permanentDamages, FightActor from)
        {
            if (delta < 0) //check if this is not a heal/buff 
                base.ChallengeFailed();
        }

        #endregion

        public override bool IsCompatible(Fight fight)
        {
            return true;
        }
        public override ChallengeChecker BuildChallenge(Fight fight)
        {
            return new UntouchableChallenge(this, fight);
        }
    }
}
