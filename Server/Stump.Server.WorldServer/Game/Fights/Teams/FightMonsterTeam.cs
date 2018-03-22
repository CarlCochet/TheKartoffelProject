﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Fights.Teams
{
    public class FightMonsterTeam : FightTeamWithLeader<MonsterFighter>
    {
        private int v;
        private Cell[] cell;

        public FightMonsterTeam(TeamEnum id, Cell[] placementCells) : base(id, placementCells)
        {
        }

        public FightMonsterTeam(TeamEnum id, Cell[] placementCells, AlignmentSideEnum alignmentSide)
            : base(id, placementCells, alignmentSide)
        {
        }

        public override TeamTypeEnum TeamType
        {
            get { return TeamTypeEnum.TEAM_TYPE_MONSTER; }
        }

        public override FighterRefusedReasonEnum CanJoin(Character character)
        {
            return !character.IsGameMaster() ? FighterRefusedReasonEnum.WRONG_ALIGNMENT : base.CanJoin(character);
        }
    }
}