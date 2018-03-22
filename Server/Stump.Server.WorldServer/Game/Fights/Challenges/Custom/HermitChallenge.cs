﻿using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.ANACHORÈTE)]
    [ChallengeIdentifier((int)ChallengeEnum.PUSILLANIME)]
    public class HermitChallenge : DefaultChallenge
    {
        readonly FightTeam m_team;

        public HermitChallenge(int id, IFight fight)
            : base(id, fight)
        {
            if (id == (int)ChallengeEnum.ANACHORÈTE)
            {
                BonusMin = 20;
                BonusMax = 30;
            }
            else
            {
                BonusMin = 30;
                BonusMax = 30;  
            }

            m_team = Fight.DefendersTeam is FightMonsterTeam ? Fight.DefendersTeam : Fight.ChallengersTeam;
            if (id == (int)ChallengeEnum.ANACHORÈTE)
                m_team = m_team.OpposedTeam; 
        }

        public override void Initialize()
        {
            base.Initialize();

            Fight.BeforeTurnStopped += OnBeforeTurnStopped;
        }

        public override bool IsEligible() => m_team.GetAllFighters().Count() > 1;

        void OnBeforeTurnStopped(IFight fight, FightActor fighter)
        {
            if (!(fighter is CharacterFighter) || fighter.IsDead())
                return;

            if (!fighter.Position.Point.GetAdjacentCells(x => m_team.GetOneFighter(y => y.IsAlive() && y.Cell == Fight.Map.GetCell(x)) != null).Any())
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED);
            Fight.BeforeTurnStopped -= OnBeforeTurnStopped;
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            OnBeforeTurnStopped(fight, fight.FighterPlaying);

            base.OnWinnersDetermined(fight, winners, losers, draw);

            Fight.BeforeTurnStopped -= OnBeforeTurnStopped;
        }
    }
}
