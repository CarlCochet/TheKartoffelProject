using Stump.Core.Threading;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Challenges.Checkers
{
    public class UnwillingVolunteerChallenge : ChallengeChecker
    {
        // FIELDS
        private FightActor m_target;

        // PROPERTIES
        public override ushort ChallengeId
        {
            get
            {
                return 3;
            }
        }
        public override int TargetId
        {
            get
            {
                return this.m_target.Id;
            }
        }

        // CONSTRUCTORS
        public UnwillingVolunteerChallenge() : base() { }
        private UnwillingVolunteerChallenge(ChallengeChecker pattern, Fight fight)
            : base(pattern, fight)
        {
            var random = new AsyncRandom();

            this.m_target = base.m_fight.BlueTeam.Fighters[random.Next(fight.BlueTeam.Fighters.Count)];
        }

        // METHODS
        public override void BindEvents()
        {
            base.m_fight.BlueTeam.FighterAdded += this.BlueTeam_FighterAdded;
            base.m_fight.BlueTeam.FighterRemoved += this.BlueTeam_FighterRemoved;
            foreach (var item in base.m_fight.BlueTeam.Fighters)
            {
                item.Dead += this.Fighter_Dead;
            }
        }

        public override void UnbindEvents()
        {
            base.m_fight.BlueTeam.FighterAdded -= this.BlueTeam_FighterAdded;
            base.m_fight.BlueTeam.FighterRemoved -= this.BlueTeam_FighterRemoved;
            foreach (var item in base.m_fight.BlueTeam.Fighters)
            {
                item.Dead -= this.Fighter_Dead;
            }
        }

        #region Handlers

        private void BlueTeam_FighterRemoved(Fights.Teams.FightTeam arg1, FightActor arg2)
        {
            arg2.Dead -= this.Fighter_Dead;
        }

        private void BlueTeam_FighterAdded(Fights.Teams.FightTeam arg1, FightActor arg2)
        {
            arg2.Dead += this.Fighter_Dead;
        }

        private void Fighter_Dead(FightActor arg1, FightActor arg2)
        {
            if (arg1 == this.m_target)
            {
                this.ChallengeSuccessful();
            }
            else
            {
                this.ChallengeFailed();
            }
        }

        #endregion

        public override bool IsCompatible(Fight fight)
        {
            return fight.BlueTeam.Fighters.Count > 1;
        }
        public override ChallengeChecker BuildChallenge(Fight fight)
        {
            return new UnwillingVolunteerChallenge(this, fight);
        }
    }
}
