
using Stump.Core.Reflection;
using Stump.Server.WorldServer.Database.Challenges;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Basic;
using System;
using System.Linq;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Game.Challenges
{
    public enum ChallengeStateEnum
    {
        CHALLENGE_STATE_PENDING,
        CHALLENGE_STATE_SUCESSFUL,
        CHALLENGE_STATE_FAILED
    }

    // http://dofus.jeuxonline.info/article/7786/challenges
    // http://astucededofus.blogspot.fr/2012/07/drivers-dofus-les-challenges.html
    public abstract class ChallengeChecker
    {
        // FIELDS
        protected readonly Fight m_fight;
        protected readonly ChallengeRecord m_challenge;

        private ChallengeChecker m_pattern;
        private ChallengeStateEnum m_state;
        private int m_bonus;
        private int m_nbGrade;

        // PROPERTIES
        public abstract ushort ChallengeId
        {
            get;
        }
        public ChallengeRecord Challenge
        {
            get
            {
                return this.m_pattern.m_challenge;
            }
        }
        public bool Hidden
        {
            get
            {
                return this.m_pattern.m_challenge.Hidden;
            }
        }
        public int MinBonus
        {
            get
            {
                return this.m_pattern.m_challenge.MinBonus;
            }
        }
        public int MaxBonus
        {
            get
            {
                return this.m_pattern.m_challenge.MaxBonus;
            }
        }
        public virtual int TargetId
        {
            get
            {
                return 0;
            }
        }
        public bool IsFailed
        {
            get
            {
                return this.m_state == ChallengeStateEnum.CHALLENGE_STATE_FAILED;
            }
        }
        public bool IsSuccessful
        {
            get
            {
                return this.m_state == ChallengeStateEnum.CHALLENGE_STATE_SUCESSFUL;
            }
        }

        // CONSTRUCTORS
        public ChallengeChecker()
        {
            this.m_pattern = this;

            this.m_challenge = Singleton<ChallengeManager>.Instance.GetChallenge(this.ChallengeId);
            if (this.m_challenge == null)
            {
                return;
                throw new Exception("Challenge checker is not related to a valid challenge.");
            }

            this.m_nbGrade = Math.Max(0, (this.MaxBonus - this.MinBonus) / 5);
        }
        protected ChallengeChecker(ChallengeChecker pattern, Fight fight)
        {
            this.m_pattern = pattern;

            this.m_fight = fight;
            this.m_state = ChallengeStateEnum.CHALLENGE_STATE_PENDING;

            this.CalculateChallengeBonus();

            this.BindEvents();
        }

        // METHODS
        public void Initialize()
        {
            if (!this.Hidden)
            {
                ContextHandler.SendChallengeInfoMessage(this.m_fight.Clients, this.ChallengeId, this.TargetId, (ushort)this.m_bonus);
            }
        }

        public abstract bool IsCompatible(Fight fight);
        public abstract ChallengeChecker BuildChallenge(Fight fight);

        public abstract void BindEvents();
        public abstract void UnbindEvents();

        protected void ChallengeFailed()
        {
            if (this.m_state == ChallengeStateEnum.CHALLENGE_STATE_PENDING)
            {
                this.m_state = ChallengeStateEnum.CHALLENGE_STATE_FAILED;

                this.UnbindEvents();

                if (!this.Hidden)
                {
                    ContextHandler.SendChallengeResultMessage(this.m_fight.Clients, this.ChallengeId, false);

                    if (this.m_fight.FighterPlaying is CharacterFighter)
                    {
                        BasicHandler.SendTextInformationMessage(this.m_fight.Clients, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 188, (this.m_fight.FighterPlaying as CharacterFighter).Character.Name, this.ChallengeId);
                    }
                }
            }
        }
        protected void ChallengeSuccessful()
        {
            if (this.m_state == ChallengeStateEnum.CHALLENGE_STATE_PENDING)
            {
                this.m_state = ChallengeStateEnum.CHALLENGE_STATE_SUCESSFUL;

                this.UnbindEvents();

                if (!this.Hidden)
                {
                    ContextHandler.SendChallengeResultMessage(this.m_fight.Clients, this.ChallengeId, true);
                }
            }
        }

        public int GetChallengeBonus()
        {
            if (this.m_state == ChallengeStateEnum.CHALLENGE_STATE_SUCESSFUL)
            {
                return this.m_bonus;
            }
            else
            {
                return 0;
            }
        }

        private void CalculateChallengeBonus()
        {
            if (this.MinBonus >= this.MaxBonus)
            {
                this.m_bonus = this.MinBonus;
            }
            else
            {
                var ratio = Math.Min(1, this.m_fight.RedTeam.GetAllFighters().Sum(entry => (double)entry.Level) /
                    this.m_fight.BlueTeam.GetAllFighters().Sum(entry => (double)entry.Level));

                var grade = 1.0 / this.m_pattern.m_nbGrade;

                this.m_bonus = Math.Min(this.MaxBonus, this.MinBonus + (5 * (int)((1 - ratio) / grade)));
            }
        }
    }
}
