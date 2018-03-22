using Stump.DofusProtocol.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Summons
{
    [BrainIdentifier((int)MonsterIdEnum.LAPINO_39)]
    public class LapinoBrain : Brain
    {
        public LapinoBrain(AIFighter fighter) : base(fighter)
        {
            fighter.GetAlive += OnGetAlive;
        }

        private void OnGetAlive(FightActor obj)
        {
            Fighter.CastAutoSpell(new Spell((int)SpellIdEnum.STIMULATING_WORD_6656, obj.Level), (Fighter as SummonedMonster).Summoner.Cell);
        }
    }
}
