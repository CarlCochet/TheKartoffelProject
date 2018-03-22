using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Game.Challenges.Checkers
{
    public class ExuberantChallenge : ChallengeChecker
    {
        // FIELDS
        private short m_cellId;

        // PROPERTIES
        public override ushort ChallengeId
        {
            get
            {
                return 41;
            }
        }

        // CONSTRUCTORS
        public ExuberantChallenge() : base() { }
        private ExuberantChallenge(ChallengeChecker pattern, Fight fight)
            : base(pattern, fight)
        { }

        // METHODS
        public override void BindEvents()
        {
            base.m_fight.FightEnded += this.Fight_FightEnded;
            foreach (var character in base.m_fight.RedTeam.GetAllFighters<CharacterFighter>())
            {
                character.TurnPassed += this.Character_TurnPassed;
            }
        }

        public override void UnbindEvents()
        {
            base.m_fight.FightEnded -= this.Fight_FightEnded;
            foreach (var character in base.m_fight.RedTeam.GetAllFighters<CharacterFighter>())
            {
                character.TurnPassed -= this.Character_TurnPassed;
            }
        }

        #region Events

        private void Fight_FightEnded(Fight obj)
        {
            base.ChallengeSuccessful();
        }

        private void Character_TurnPassed(FightActor obj)
        {
            if (obj.AP > 0)
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
            return new ExuberantChallenge(this, fight);
        }
    }
}
