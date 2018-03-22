﻿using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using FightLoot = Stump.Server.WorldServer.Game.Fights.Results.FightLoot;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class ArenaFightResult : FightResult<CharacterFighter>
    {
        public ArenaFightResult(CharacterFighter fighter, FightOutcomeEnum outcome, FightLoot loot, int rank, bool showLoot = true)
            : base(fighter, outcome, loot)
        {
            Rank = rank;
            ShowLoot = showLoot;
        }

        public override bool CanLoot(FightTeam team) => Outcome == FightOutcomeEnum.RESULT_VICTORY && !Fighter.HasLeft() && ShowLoot;

        public int Rank
        {
            get;
        }

        public bool ShowLoot
        {
            get;
        }

        public override FightResultListEntry GetFightResultListEntry()
        {
            var amount = 0;
            var kamas = 0;
            var Experience = 0;

            if (CanLoot(Fighter.Team))
            {
                Experience = Fighter.Character.ComputeWonExperience();
                amount = Fighter.Character.ComputeWonArenaTokens(Rank);
                kamas = Fighter.Character.ComputeWonArenaKamas();
            }

            var items = amount > 0 ? new[] { (short)ItemIdEnum.KOLIZETON_12736, (short)amount } : new short[0];

            var loot = new DofusProtocol.Types.FightLoot(items, kamas, Experience);

            return new FightResultPlayerListEntry((short)Outcome, 0, loot, Id, Alive, (sbyte)Level,
                new FightResultAdditionalData[0]);
        }

        public override void Apply()
        {
            Fighter.Character.UpdateArenaProperties(Rank, Outcome == FightOutcomeEnum.RESULT_VICTORY);
        }
    }
}