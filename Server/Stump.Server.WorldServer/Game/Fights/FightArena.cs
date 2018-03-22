using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Arena;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightArena : Fight<FightPlayerTeam, FightPlayerTeam>
    {
        private readonly System.Collections.Generic.Dictionary<Character, Map> m_playersMaps = new System.Collections.Generic.Dictionary<Character, Map>();

        private short m_redEloTeam;
        private short m_blueEloTeam;
        private bool solokoli;

        public override FightTypeEnum FightType
        {
            get
            {
                return FightTypeEnum.FIGHT_TYPE_PVP_ARENA;
            }
        }

        public override bool IsPvP => throw new NotImplementedException();

        protected override bool CanCancelFight()
        {
            return false;
        }

        public FightArena(int id, Map fightMap, FightArenaTeam blueTeam, FightArenaTeam redTeam, bool solo) : base(id, fightMap, blueTeam, redTeam)
        {
            solokoli = solo;
        }

        public override void StartPlacement()
        {
            base.StartPlacement();
        }

        public void AddRedTeam(ArenaPartyCreation redTeam)
        {
            this.m_redEloTeam = (short)redTeam.GetCharacters().Sum(entry => entry.Record.HiddenArenaRank);

            this.AddTeam(redTeam.GetCharacters(), this.ChallengersTeam);
        }
        public void AddBlueTeam(ArenaPartyCreation blueTeam)
        {
            this.m_blueEloTeam = (short)blueTeam.GetCharacters().Sum(entry => entry.Record.HiddenArenaRank);

            this.AddTeam(blueTeam.GetCharacters(), this.DefendersTeam);
        }

        private void AddTeam(IEnumerable<Character> characters, FightTeam team)
        {
            foreach (var entry in characters)
            {
                Character character = entry;
                this.m_playersMaps.Add(character, character.Map);
                Character character1 = character;
                character.Area.ExecuteInContext(() =>
                {
                    // TODO : enlever bouclier
                    character1.Teleport(this.Map, character1.Cell);
                    this.Map.Area.ExecuteInContext(() =>
                    {
                        team.AddFighter(character.CreateFighter(team));
                        if (solokoli)
                        {
                            if (this.GetAllCharacters().Count() == 2)
                            {
                                this.StartPlacement();
                                this.m_placementTimer = base.Map.Area.CallDelayed(30000, new Action(this.StartFighting));
                            }
                        }
                        else
                        {
                            if (this.GetAllCharacters().Count() == 6)
                            {
                                this.StartPlacement();
                                this.m_placementTimer = base.Map.Area.CallDelayed(30000, new Action(this.StartFighting));
                            }
                        }
                    });
                });
            }
        }

        protected  IEnumerable<IFightResult> GenerateResults()
        {
            IFightResult[] array = (
                from entry in base.GetFightersAndLeavers()
                where !(entry is SummonedFighter)
                select entry into fighter
                select fighter.GetFightResult()).ToArray<IFightResult>();
            IFightResult[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                IFightResult fightResult = array2[i];
                FightPlayerResult fightPlayerResult = fightResult as FightPlayerResult;
                if (fightPlayerResult != null)
                {
                    this.UpdateRank(fightPlayerResult.Fighter);

                    if (fightPlayerResult.Fighter.HasWin() && !this.Leavers.Contains(fightPlayerResult.Fighter))
                    {
                        fightPlayerResult.Character.Record.ArenaVictoryCount++;
                        if (solokoli)
                        {
                            fightPlayerResult.SetEarnedHonor(this.CalculateEarnedHonor(fightPlayerResult.Fighter), this.CalculateEarnedDishonor(fightPlayerResult.Fighter));
                        }

                        fightPlayerResult.AddEarnedExperience((int)((12500 * (fightPlayerResult.Character.Level / 200.0)) * fightPlayerResult.Character.Level));
                        fightPlayerResult.Loot.Kamas = 20 * fightPlayerResult.Character.Level;

                        if (fightPlayerResult.Character.Record.ArenaRank / 100 > 0)
                        {
                            fightPlayerResult.Loot.AddItem(12736, (uint)(fightPlayerResult.Character.Record.ArenaRank / 100));
                        }
                    }
                }
            }
            return array;
        }
        public short CalculateEarnedHonor(CharacterFighter character)
        {
            short result;
            if (base.Draw)
            {
                result = 0;
            }
            else
            {
                if (character.OpposedTeam.AlignmentSide == AlignmentSideEnum.ALIGNMENT_NEUTRAL)
                {
                    result = 0;
                }
                else
                {
                    try
                    {
                        if (base.Losers == character.Team)// Perdant 
                        {
                            short Honeur = 0;
                            switch ((character as CharacterFighter).Character.AlignmentGrade)
                            {
                                case 1:
                                    Honeur = -75;
                                    break;
                                case 2:
                                    Honeur = -100;
                                    break;
                                case 3:
                                    Honeur = -150;
                                    break;
                                case 4:
                                    Honeur = -200;
                                    break;
                                case 5:
                                    Honeur = -250;
                                    break;
                                case 6:
                                    Honeur = -300;
                                    break;
                                case 7:
                                    Honeur = -350;
                                    break;
                                case 8:
                                    Honeur = -400;
                                    break;
                                case 9:
                                    Honeur = -500;
                                    break;
                                case 10:
                                    Honeur = -750;
                                    break;
                            }
                            result = Honeur;
                        }
                        else
                            result = (short)new Random().Next(70, 100);

                    }
                    catch
                    {
                        if (base.Losers == character.Team)// Perdant 
                            result = -100;
                        else
                            result = 100;
                    }
                }
            }
            return result;
        }
        public short CalculateEarnedDishonor(CharacterFighter character)
        {
            short result;
            if (base.Draw)
            {
                result = 0;
            }
            else
            {
                if (character.OpposedTeam.AlignmentSide != AlignmentSideEnum.ALIGNMENT_NEUTRAL)
                {
                    result = 0;
                }
                else
                {
                    result = 1;
                }
            }
            return result;
        }
        protected void SendGameFightJoinMessage(FightSpectator spectator)
        {
            ContextHandler.SendGameFightJoinMessage(spectator.Character.Client, false, false, true, base.IsStarted, this.GetPlacementTimeLeft(), this.FightType);
        }

        protected override void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, this.CanCancelFight(), !base.IsStarted, false, base.IsStarted, this.GetPlacementTimeLeft(), this.FightType);
        }

        public int GetPlacementTimeLeft()
        {
            double num = (double)30000 - (System.DateTime.Now - base.CreationTime).TotalMilliseconds;
            if (num < 0.0)
            {
                num = 0.0;
            }
            return (int)num;
        }

        public void UpdateRank(CharacterFighter fighter)
        {
            short ownRankTeam;
            short opponentRankTeam;

            if (fighter.Team == this.ChallengersTeam)
            {
                ownRankTeam = this.m_redEloTeam;
                opponentRankTeam = this.m_blueEloTeam;
            }
            else
            {
                opponentRankTeam = this.m_redEloTeam;
                ownRankTeam = this.m_blueEloTeam;
            }

            fighter.Character.Record.HiddenArenaRank += (short)(this.GetRankCoefficient(fighter) * (((fighter.HasWin() && !this.Leavers.Contains(fighter)) ? 1 : 0) -
                Singleton<EloManager>.Instance.GetProbability((short)(ownRankTeam - opponentRankTeam))));

            fighter.Character.Record.ArenaFightCount++;
            fighter.Character.Record.ArenaRank += (short)((fighter.Character.Record.HiddenArenaRank - fighter.Character.Record.ArenaRank) / 2);
            fighter.Character.CheckArenaRank();
        }

        private byte GetRankCoefficient(CharacterFighter fighter)
        {
            byte result = 10;
            if (fighter.Character.Record.ArenaFightCount <= 30)
            {
                result *= 4;
            }
            else if (fighter.Character.Record.BestArenaRank < 1200)
            {
                result *= 2;
            }

            return result;
        }

        protected override void OnWinnersDetermined(FightTeam winners, FightTeam losers, bool draw)
        {
            using (System.Collections.Generic.IEnumerator<CharacterFighter> enumerator = winners.GetAllFighters<CharacterFighter>().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.Character.NextMap = this.m_playersMaps[enumerator.Current.Character];
                }
            }

            base.OnWinnersDetermined(winners, losers, draw);
        }
    }
}
