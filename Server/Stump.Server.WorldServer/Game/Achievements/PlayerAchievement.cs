using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Achievements;
using Stump.Server.WorldServer.Game.Achievements.Criterions;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Achievements;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.DofusProtocol.Enums.Custom;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Game.Achievements
{
    public class PlayerAchievement
    {
        // FIELDS
        public delegate void AchievementCompleted(Character character, AchievementTemplate achievement);

        private readonly object m_lock = new object();
        private AchievementPointsCriterion m_achievementPointsCriterion;
        private ChallengesCriterion m_challengesCriterion;

        private List<AchievementTemplate> m_finishedAchievements;
        private Dictionary<string, AbstractCriterion> m_finishedCriterions;

        private LevelCriterion m_levelCriterion;
        private Dictionary<AbstractCriterion, int> m_runningCriterions;

        // CONSTRUCTORS
        public PlayerAchievement(Character character)
        {
            Owner = character;

            InitializeEvents();
        }

        // PROPERTIES
        public Character Owner { get; }

        public IReadOnlyList<AchievementTemplate> FinishedAchievements => m_finishedAchievements.AsReadOnly();

        public IReadOnlyList<PlayerAchievementReward> RewardAchievements => Owner.Record.AchievementRewards;

        // METHODS
        public void LoadAchievements()
        {
            m_finishedAchievements = new List<AchievementTemplate>();
            m_finishedCriterions = new Dictionary<string, AbstractCriterion>();
            m_runningCriterions = new Dictionary<AbstractCriterion, int>();

            foreach (var achievementId in Owner.Record.FinishedAchievements)
            {
                var achievement = Singleton<AchievementManager>.Instance.TryGetAchievement(achievementId);
                if (achievement != null)
                {
                    m_finishedAchievements.Add(achievement);
                }
            }
            foreach (var finishedAchievementObjective in Owner.Record.FinishedAchievementObjectives)
            {
                var achievementObjective =
                    Singleton<AchievementManager>.Instance.TryGetAchievementObjective(finishedAchievementObjective);
                if (achievementObjective != null)
                {
                    m_finishedCriterions.Add(achievementObjective.Criterion, achievementObjective.AbstractCriterion);
                }
            }
            foreach (var runningAchievementObjective in Owner.Record.RunningAchievementObjectives)
            {
                var achievementObjective =
                    Singleton<AchievementManager>.Instance.TryGetAchievementObjective(
                        (uint)runningAchievementObjective.Key);
                if (achievementObjective != null)
                {
                    m_runningCriterions.Add(achievementObjective.AbstractCriterion, runningAchievementObjective.Value);
                }
            }

            foreach (var rewardableAchievement in Owner.Record.AchievementRewards)
            {
                rewardableAchievement.Initialize(Owner);
            }

            ManageCriterions();
        }

        private void InitializeEvents()
        {
            Owner.ChangeSubArea += OnChangeSubArea;
            Owner.FightEnded += OnFightEnded;
            Owner.LevelChanged += OnLevelChanged;
        }

        public AchievementTemplate TryGetFinishedAchievement(short id)
        {
            return m_finishedAchievements.FirstOrDefault(entry => entry.Id == id);
        }

        public IEnumerable<Achievement> TryGetFinishedAchievements(AchievementCategoryRecord category)
        {
            var result = from template in category.Achievements
                         where m_finishedAchievements.Contains(template)
                         select template.GetAchievement(this);

            return result;
        }

        public void CompleteAchievement(AchievementTemplate achievement)
        {
            lock (m_lock)
            {
                var reward = Owner.Record.AchievementRewards.FirstOrDefault(entry => entry == Owner.Level);
                if (reward == null)
                {
                    reward = new PlayerAchievementReward(Owner, achievement);

                    Owner.Record.AchievementRewards.Add(reward);
                }
                else
                {
                    reward.AddRewardableAchievement(achievement);
                }

                Owner.Record.FinishedAchievements.Add((ushort)achievement.Id);
                Owner.Record.AchievementPoints += (int)achievement.Points;

                m_finishedAchievements.Add(achievement);
            }

            AchievementHandler.SendAchievementFinishedMessage(Owner.Client, (ushort)achievement.Id, Owner.Level);

            OnAchievementCompleted(achievement);
        }

        public List<AchievementRewardable> GetRewardableAchievements()
        {
            var achievements = new List<AchievementRewardable>();
            foreach (var item in RewardAchievements)
            {
                achievements.AddRange(item.GetRewardableAchievements());
            }

            return achievements;
        }

        public int GetRunningCriterion(AbstractCriterion criterion)
        {
            return m_runningCriterions.ContainsKey(criterion) ? m_runningCriterions[criterion] : 0;
        }

        public bool RewardAchievement(AchievementTemplate achievement, out int experience, out int guildExperience)
        {
            bool result;
            PlayerAchievementReward reward = null;
            experience = 0;
            guildExperience = 0;

            if (achievement != null)
            {
                lock (m_lock)
                {
                    foreach (var item in Owner.Record.AchievementRewards)
                    {
                        if (item.Contains(achievement))
                        {
                            reward = item;
                            break;
                        }
                    }
                }

                result = reward != null && RewardAchievement(achievement, reward, out experience, out guildExperience);
            }
            else
            {
                result = false;
            }

            return result;
        }

        public bool RewardAchievement(AchievementTemplate achievement, PlayerAchievementReward owner, out int experience,
            out int guildExperience)
        {
            experience = 0;
            guildExperience = 0;
            if (!owner.Remove(achievement))
            {
                return false;
            }

            experience = achievement.GetExperienceReward(Owner.Client);
            if (experience > 0)
            {
                if (Owner.GuildMember != null && Owner.GuildMember.GivenPercent > 0)
                {
                    var guildXP = (int)(experience * (Owner.GuildMember.GivenPercent * 0.01));
                    var adjustedGuildExperience = (int)Owner.Guild.AdjustGivenExperience(Owner, guildXP);
                    adjustedGuildExperience = Math.Min(Guild.MaxGuildXP, adjustedGuildExperience);

                    experience = (int)(experience * (100.0 - Owner.GuildMember.GivenPercent) * 0.01);
                    if (adjustedGuildExperience > 0)
                    {
                        guildExperience = adjustedGuildExperience;
                    }
                }
            }
            if (experience < 0)
            {
                experience = 0;
            }

            var kamas = achievement.GetKamasReward(Owner.Client);
            if (kamas > 0)
            {
                Owner.Inventory.AddKamas(kamas);
            }

            foreach (var item in achievement.Rewards)
            {
                for (var i = 0; i < item.ItemsReward.Length; i++)
                {
                    var id = item.ItemsReward[i];
                    var quantity = item.ItemsQuantityReward[i];

                    var itemTemplate = Singleton<ItemManager>.Instance.TryGetTemplate((int)id);
                    if (itemTemplate != null)
                    {
                        Owner.Inventory.AddItem(itemTemplate, (int)quantity);
                    }
                }

                foreach (var emoteId in item.EmotesReward)
                {
                    Owner.Record.EmotesCSV = Owner.Record.EmotesCSV + "," + emoteId; //TODO Test
                    Owner.Client.Send(new EmoteAddMessage((sbyte)emoteId));
                }

                foreach (var spellId in item.SpellsReward.Where(spellId => !Owner.Spells.HasSpell((int)spellId)))
                {
                    Owner.Spells.LearnSpell((int)spellId);
                }

                foreach (var titleId in item.TitlesReward.Where(titleId => !Owner.HasTitle((ushort)titleId)))
                {
                    Owner.AddTitle((ushort)titleId);
                }

                foreach (var ornamentId in item.OrnamentsReward.Where(ornamentId => !Owner.HasOrnament((ushort)ornamentId)))
                {
                    Owner.AddOrnament((ushort)ornamentId);
                }
            }
            // TODO : items

            if (!owner.Any())
            {
                Owner.Record.AchievementRewards.Remove(owner);
            }

            return true;
        }

        public void RewardAllAchievements(Action<AchievementTemplate, bool> action)
        {
            var totalExperience = 0;
            var totalGuildExperience = 0;
            lock (m_lock)
            {
                while (RewardAchievements.Count > 0)
                {
                    var achievementReward = RewardAchievements[0];
                    while (achievementReward.RewardAchievements.Count > 0)
                    {
                        var achievement = achievementReward.RewardAchievements[0];

                        int guildExperience;
                        int experience;
                        action(achievement,
                            RewardAchievement(achievement, achievementReward, out experience, out guildExperience));
                        totalExperience += experience;
                        totalGuildExperience += guildExperience;
                    }
                }
            }

            if (totalExperience > 0)
            {
                Owner.AddExperience(totalExperience);
            }
            else
            {
                totalExperience = 0;
            }

            if (Owner.GuildMember != null && totalGuildExperience > 0)
            {
                Owner.GuildMember.AddXP(totalGuildExperience);
            }
            else
            {
                totalGuildExperience = 0;
            }

            CharacterHandler.SendCharacterExperienceGainMessage(Owner.Client, (long)totalExperience, 0L,
                (long)totalGuildExperience, 0L);
        }

        #region Handlers

        private void OnFightEnded(Character character, CharacterFighter fighter)
        {
            if (fighter.Fight.FightType == FightTypeEnum.FIGHT_TYPE_PvM)
            {
                if (fighter.HasWin())
                {
                    if (fighter.Fight.Challenges.Any(entry => entry.Status == ChallengeStatusEnum.SUCCESS))
                    {
                        if (fighter.Fight.ChallengersTeam.GetAllFighters().Sum(entry => entry.Level) <
                            fighter.Fight.GetAllFighters().Sum(entry => entry.Level))
                        {
                            // TODO : 1 challenge
                        }
                    }
                    if (fighter.Fight.Map.IsDungeonSpawn)
                    {
                        // TODO : check si c'est un boss
                        foreach (var criterion in fighter.Fight.DefendersTeam.GetAllFighters<MonsterFighter>().Select(item => Singleton<AchievementManager>.Instance.TryGetCriterionByBoss(item.Monster.Template)).Where(criterion => criterion != null))
                        {
                            if (m_runningCriterions.ContainsKey(criterion))
                                m_runningCriterions[criterion]++;
                            else
                                m_runningCriterions.Add(criterion, 1);

                            if (ContainsCriterion(criterion.Criterion)) continue;
                            if (criterion.Eval(Owner))
                                AddCriterion(criterion);
                        }
                    }

                    foreach (var criterion in fighter.Fight.DefendersTeam.GetAllFighters<MonsterFighter>().Select(item => Singleton<AchievementManager>.Instance.TryGetCriterionByMonster(item.Monster.Template)).Where(criterion => criterion != null))
                    {
                        if (m_runningCriterions.ContainsKey(criterion))
                            m_runningCriterions[criterion]++;
                        else
                            m_runningCriterions.Add(criterion, 1);

                        if (ContainsCriterion(criterion.Criterion)) continue;
                        if (criterion.Eval(Owner))
                            AddCriterion(criterion);
                    }
                }
            }
        }

        private void OnChangeSubArea(RolePlayActor actor, SubArea subArea)
        {
            var achievement = subArea.Record.Achievement;
            lock (m_lock)
            {
               if (achievement != null)
               {
                   if (!m_finishedAchievements.Contains(achievement))
                   {
                       CompleteAchievement(achievement);
                   }
               }
            }
            //if (actor is Character && (actor as Character).LastMap != null && (actor as Character).LastMap.SubArea != subArea)
            //{
            //   if(World.Instance.GetCharacters(x => x.SubArea == actor.LastMap.SubArea).Where(x=> x.Guild.Alliance == (actor as Character).Guild.Alliance).Count() == 0)
            //   {
            //       subArea.AlliancesInZone.Remove((actor as Character).Guild.Alliance);
            //   }
            //}
            //if (actor is Character && !subArea.AlliancesInZone.Contains((actor as Character).Guild.Alliance))
            //{
            //   subArea.AlliancesInZone.Add((actor as Character).Guild.Alliance);
            //}
        }

        private void OnLevelChanged(Character character, byte currentLevel, int difference)
        {
            if (difference > 0)
            {
                ManageIncrementalCriterions(ref m_levelCriterion);
            }
        }

        private void OnAchievementCompleted(AchievementTemplate achievement)
        {
            var achievementCriterion = Singleton<AchievementManager>.Instance.TryGetAchievementCriterion(achievement);
            if (achievementCriterion != null)
            {
                AddCriterion(achievementCriterion);
            }
        }

        private void ManageCriterions()
        {
            m_levelCriterion = Singleton<AchievementManager>.Instance.MinLevelCriterion;
            m_achievementPointsCriterion = Singleton<AchievementManager>.Instance.MinAchievementPointsCriterion;

            ManageIncrementalCriterions(ref m_levelCriterion);
            ManageIncrementalCriterions(ref m_challengesCriterion);
            ManageIncrementalCriterions(ref m_achievementPointsCriterion);
        }

        private void ManageIncrementalCriterions<T>(ref T criterion)
            where T : AbstractCriterion
        {
            while (criterion != null && criterion.Eval(Owner))
            {
                if (!ContainsCriterion(criterion.Criterion))
                {
                    AddCriterion(criterion);
                }

                criterion = (T)criterion.Next;
            }
        }

        #endregion

        #region Criterions manager

        public void AddCriterion(AbstractCriterion criterion)
        {
            if (!m_finishedCriterions.ContainsKey(criterion.Criterion)) // esto es igual a decir no exite
            {
                m_finishedCriterions.Add(criterion.Criterion, criterion);

                foreach (var item in criterion.UsefullFor.Where(item => item.Objectives.All(entry => m_finishedCriterions.ContainsKey(entry.Criterion))))
                {
                    CompleteAchievement(item);
                }

                Owner.Record.FinishedAchievementObjectives.Add(criterion.DefaultObjective.Id);
            }
        }

        public bool ContainsCriterion(string criterion)
        {
            return m_finishedCriterions.ContainsKey(criterion);
        }

        #endregion
    }
}