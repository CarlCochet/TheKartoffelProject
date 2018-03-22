using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Game.Challenges.Checkers
{
    public class NomadChallenge : ChallengeChecker
    {
        // FIELDS

        // PROPERTIES
        public override ushort ChallengeId => 8;

        // CONSTRUCTORS
        public NomadChallenge() : base() { }
        private NomadChallenge(ChallengeChecker pattern, Fight fight)
            : base(pattern, fight)
        { }

        // METHODS
        public override void BindEvents()
        {
            m_fight.FightEnded += Fight_FightEnded;
            foreach (var character in m_fight.RedTeam.GetAllFighters<CharacterFighter>())
            {
                character.TurnPassed += Character_TurnPassed;
            }
        }

        public override void UnbindEvents()
        {
            m_fight.FightEnded -= Fight_FightEnded;
            foreach (var character in m_fight.RedTeam.GetAllFighters<CharacterFighter>())
            {
                character.TurnPassed -= Character_TurnPassed;
            }
        }

        #region Events

        private void Fight_FightEnded(Fight obj)
        {
            ChallengeSuccessful();
        }

        private void Character_TurnPassed(FightActor obj)
        {
            if (obj.Stats.MP.Total > 0)
                ChallengeFailed();
        }

        #endregion

        public override bool IsCompatible(Fight fight)
        {
            return true;
        }
        public override ChallengeChecker BuildChallenge(Fight fight)
        {
            return new NomadChallenge(this, fight);
        }
    }
}
