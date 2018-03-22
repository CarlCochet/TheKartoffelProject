using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Game.Fights.Teams
{
    public class FightArenaTeam : FightTeam
    {
        public override TeamTypeEnum TeamType
        {
            get
            {
                return TeamTypeEnum.TEAM_TYPE_PLAYER;
            }
        }

        public FightArenaTeam(sbyte id, Cell[] placementCells) : base((TeamEnum)id, placementCells)
        {
        }
        public FightArenaTeam(sbyte id, Cell[] placementCells, AlignmentSideEnum alignmentSide) : base((TeamEnum)id, placementCells, alignmentSide)
        {
        }

        public override FighterRefusedReasonEnum CanJoin(Character character)
        {
            return FighterRefusedReasonEnum.TEAM_FULL;
        }
    }
}
