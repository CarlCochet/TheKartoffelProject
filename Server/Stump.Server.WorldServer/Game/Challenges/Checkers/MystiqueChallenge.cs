using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Game.Challenges.Checkers
{
    public class MystiqueChallenge : ChallengeChecker
    {
        // FIELDS
        private short m_cellId;

        // PROPERTIES
        public override ushort ChallengeId
        {
            get
            {
                return 11;
            }
        }

        // CONSTRUCTORS
        public MystiqueChallenge() : base() { }
        private MystiqueChallenge(ChallengeChecker pattern, Fight fight)
            : base(pattern, fight)
        { }

        // METHODS
        public override void BindEvents()
        {
            base.m_fight.FightEnded += this.Fight_FightEnded;
            foreach (var character in base.m_fight.RedTeam.GetAllFighters<CharacterFighter>())
            {
                character.WeaponUsed += Character_WeaponUsed;
            }
        }

        public override void UnbindEvents()
        {
            base.m_fight.FightEnded -= this.Fight_FightEnded;
            foreach (var character in base.m_fight.RedTeam.GetAllFighters<CharacterFighter>())
            {
                character.WeaponUsed -= this.Character_WeaponUsed;
            }
        }

        #region Events

        private void Fight_FightEnded(Fight obj)
        {
            base.ChallengeSuccessful();
        }

        private void Character_WeaponUsed(FightActor arg1, Database.Items.Templates.WeaponTemplate arg2, Database.World.Cell arg3, DofusProtocol.Enums.FightSpellCastCriticalEnum arg4, bool arg5)
        {
            base.ChallengeFailed();
        }

        #endregion

        public override bool IsCompatible(Fight fight)
        {
            return true;
        }
        public override ChallengeChecker BuildChallenge(Fight fight)
        {
            return new MystiqueChallenge(this, fight);
        }
    }
}
