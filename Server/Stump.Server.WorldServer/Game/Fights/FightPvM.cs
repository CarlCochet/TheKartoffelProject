﻿using Stump.Core.Mathematics;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights.Challenges;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Formulas;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightPvM : Fight<FightMonsterTeam, FightPlayerTeam>
    {
        private bool m_ageBonusDefined;

        public FightPvM(int id, Map fightMap, FightMonsterTeam defendersTeam, FightPlayerTeam challengersTeam)
            : base(id, fightMap, defendersTeam, challengersTeam)
        {
        }

        public override void StartPlacement()
        {
            base.StartPlacement();

            m_placementTimer = Map.Area.CallDelayed(FightConfiguration.PlacementPhaseTime, StartFighting);
        }

        public override void StartFighting()
        {
            m_placementTimer.Dispose();
            if (PlayerTeam.Leader.Character.IsPartyLeader())
                ActiveIdols = PlayerTeam.Leader.Character.Party.IdolInventory.ComputeIdols(this).ToList();
            else
                ActiveIdols = PlayerTeam.Leader.Character.IdolInventory.ComputeIdols(this).ToList();

            base.StartFighting();
        }

        protected override void OnFightStarted()
        {
            base.OnFightStarted();

            if (!Map.AllowFightChallenges)
                return;

            initChallenge();

            if (Map.IsDungeon() || IsPvMArenaFight)
                initChallenge();

            
        }
        void initChallenge()
        {
            var challenge = ChallengeManager.Instance.GetRandomChallenge(this);

            // no challenge found
            if (challenge == null)
                return;

            challenge.Initialize();
            AddChallenge(challenge);
        }
        protected override void OnFighterAdded(FightTeam team, FightActor actor)
        {
            base.OnFighterAdded(team, actor);

            if (!(team is FightMonsterTeam) || m_ageBonusDefined)
                return;
            MonsterFighter monsterFighter = team.Leader as MonsterFighter;
            if ( monsterFighter != null)
                AgeBonus = monsterFighter.Monster.Group.AgeBonus;

            m_ageBonusDefined = true;
        }

        public FightPlayerTeam PlayerTeam => Teams.FirstOrDefault(x => x.TeamType == TeamTypeEnum.TEAM_TYPE_PLAYER) as FightPlayerTeam;

        public FightMonsterTeam MonsterTeam => Teams.FirstOrDefault(x => x.TeamType == TeamTypeEnum.TEAM_TYPE_MONSTER) as FightMonsterTeam;

        public override FightTypeEnum FightType => FightTypeEnum.FIGHT_TYPE_PvM;

        public override bool IsPvP => false;

        public bool IsPvMArenaFight
        {
            get;
            set;
        }

        protected override List<IFightResult> GetResults()
        {
            var results = new List<IFightResult>();
            results.AddRange(GetFightersAndLeavers().Where(entry => entry.HasResult).Select(entry => entry.GetFightResult()));

            try
            {
                if (Map.IsDungeon())
                {
                    var taxCollectors = Map.TaxCollector;
                    if (taxCollectors != null)
                        results.Add(new TaxCollectorProspectingResult(taxCollectors, this));

                }
                else
                {
                    var taxCollectors = Map.SubArea.Maps.Select(x => x.TaxCollector).Where(x => x != null && x.CanGatherLoots()).FirstOrDefault();
                    if (taxCollectors != null)
                        results.Add(new TaxCollectorProspectingResult(taxCollectors, this));
                }

                foreach (var team in m_teams)
                {
                    IEnumerable<FightActor> droppers = team.OpposedTeam.GetAllFighters(entry => entry.IsDead() && entry.CanDrop()).ToList();
                    var looters = results.Where(x => x.CanLoot(team) && !(x is TaxCollectorProspectingResult)).OrderByDescending(entry => entry.Prospecting).
                        Concat(results.OfType<TaxCollectorProspectingResult>().Where(x => x.CanLoot(team)).OrderByDescending(x => x.Prospecting)); // tax collector loots at the end
                    var teamPP = team.GetAllFighters<CharacterFighter>().Sum(entry => entry.Stats[PlayerFields.Prospecting].Total);
                    var kamas = Winners == team ? droppers.Sum(entry => entry.GetDroppedKamas()) * team.GetAllFighters<CharacterFighter>().Count() : 0;
                    var limit = 750000000;
                    foreach (var looter in looters)
                    {
                        looter.Loot.Kamas = teamPP > 0 ? FightFormulas.AdjustDroppedKamas(looter, teamPP, kamas) : 0;
                        if (team == Winners)
                        {
                            foreach (var item in droppers.SelectMany(dropper => dropper.RollLoot(looter)))
                            {
                                looter.Loot.AddItem(item);
                            }

                            #region Orbes
                            int sum = 0;
                            foreach (var monster in this.DefendersTeam.GetAllFighters())
                            {
                                if (monster is MonsterFighter)
                                {
                                    if ((monster as MonsterFighter).Monster.Template.Id != 494)
                                    {
                                        sum += monster.Level;
                                    }
                                }
                            }
                            int maxLootNumber = (int)((sum / (double)this.DefendersTeam.GetAllFighters().Count()));
                            looter.Loot.AddItem(new DroppedItem(8374, ((uint)new Random().Next((int)(maxLootNumber / 4.0 < 1 ? 1 : maxLootNumber / 4.0), (int)(maxLootNumber < 1 ? 1 : maxLootNumber))) * (uint)Rates.OrbsRate));
                            #endregion

                            #region Points boutique
                            if (looter is FightPlayerResult && this.DefendersTeam.GetAllFighters().Where(x => x is MonsterFighter).ToList().Exists(x => (x as MonsterFighter).Monster.Template.IsBoss))
                            {
                                Character character = (looter as FightPlayerResult).Character;
                                BasePlayerItem BPsearcher = character.Inventory.TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS);
                                if (BPsearcher != null && BPsearcher.Template.Id == 10657)
                                {
                                    MonsterFighter boss = this.DefendersTeam.GetAllFighters().Where(x => x is MonsterFighter).ToList().FirstOrDefault(x => (x as MonsterFighter).Monster.Template.IsBoss) as MonsterFighter;
                                    if (boss != null)
                                    {
                                        uint bp = ((uint)new CryptoRandom().Next((int)1, (int)Math.Ceiling(boss.Level / 15 * 1.09)));
                                        looter.Loot.AddItem(new DroppedItem(12124, bp));
                                        character.Client.Account.Tokens += (int)bp;
                                    }

                                }
                            }
                            #endregion
                        }
                        if (looter is IExperienceResult)
                        {
                            var winXP = FightFormulas.CalculateWinExp(looter, team.GetAllFighters<CharacterFighter>(), droppers);
                            var total = team == Winners ? winXP : (int)Math.Round(winXP * 0.10);
                            total = total > limit ? limit : total;
                            (looter as IExperienceResult).AddEarnedExperience(total);
                        }
                    }
                }
            }
            catch {
                
            }
            

            return results;
        }

        protected override void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, true, true, IsStarted, IsStarted ? 0 : (int)GetPlacementTimeLeft().TotalMilliseconds / 100, FightType);
        }

        protected override bool CanCancelFight() => false;

        public override TimeSpan GetPlacementTimeLeft()
        {
            var timeleft = FightConfiguration.PlacementPhaseTime - (DateTime.Now - CreationTime).TotalMilliseconds;

            if (timeleft < 0)
                timeleft = 0;

            return TimeSpan.FromMilliseconds(timeleft);
        }

        protected override void OnDisposed()
        {
            if (m_placementTimer != null)
                m_placementTimer.Dispose();

            base.OnDisposed();
        }
    }
}