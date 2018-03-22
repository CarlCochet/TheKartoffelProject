﻿using NLog;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Achievements;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Game.Achievements.Criterions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Achievements
{
    public class AchievementManager : DataManager<AchievementManager>
    {
        // FIELDS
        protected readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private Dictionary<uint, AchievementRewardRecord> m_achievemenstRewards;

        private Dictionary<AchievementTemplate, AchievementCriterion> m_achievementCriterions;
        private Dictionary<uint, AchievementCategoryRecord> m_achievementsCategories;
        private Dictionary<uint, AchievementObjectiveRecord> m_achievementsObjectives;

        private Dictionary<uint, AchievementTemplate> m_achievementsTemplates;
        private Dictionary<string, AbstractCriterion> m_criterions;
        private Dictionary<Type, AbstractCriterion> m_incrementableCriterions;
        private Dictionary<MonsterTemplate, KillBossCriterion> m_monsterBossCriterions;
        private Dictionary<MonsterTemplate, KillMonsterWithChallengeCriterion> m_monsterCriterions;

        // CONSTRUCTORS
        private AchievementManager() { }

        // PROPERTIES
        public LevelCriterion MinLevelCriterion => m_incrementableCriterions[typeof(LevelCriterion)] as LevelCriterion;

        public AchievementPointsCriterion MinAchievementPointsCriterion
            => m_incrementableCriterions[typeof(AchievementPointsCriterion)] as AchievementPointsCriterion;

        // METHODS
        [Initialization(InitializationPass.Sixth)]
        public override void Initialize()
        {
            base.Initialize();

            m_criterions = new Dictionary<string, AbstractCriterion>();
            m_monsterCriterions = new Dictionary<MonsterTemplate, KillMonsterWithChallengeCriterion>();
            m_monsterBossCriterions = new Dictionary<MonsterTemplate, KillBossCriterion>();
            m_achievementCriterions = new Dictionary<AchievementTemplate, AchievementCriterion>();
            m_incrementableCriterions = new Dictionary<Type, AbstractCriterion>();

            m_achievementsTemplates =
                Database.Query<AchievementTemplate>(AchievementTemplateRelator.FetchQuery)
                    .ToDictionary(entry => entry.Id);

            m_achievemenstRewards =
                Database.Query<AchievementRewardRecord>(AchievementRewardRelator.FetchQuery)
                    .ToDictionary(entry => entry.Id);
            m_achievementsCategories =
                Database.Query<AchievementCategoryRecord>(AchievementCategoryRelator.FetchQuery)
                    .ToDictionary(entry => entry.Id);
            m_achievementsObjectives =
                Database.Query<AchievementObjectiveRecord>(AchievementObjectiveRelator.FetchQuery)
                    .ToDictionary(entry => entry.Id);

            foreach (var pair in m_achievementsTemplates)
            {
                pair.Value.Initialize();
            }
            foreach (var pair in m_achievementsObjectives)
            {
                pair.Value.Initialize();
            }
        }

        public IEnumerable<ushort> GetAchievementsIds()
        {
            return m_achievementsTemplates.Keys.Select(entry => (ushort)entry);
        }

        public IEnumerable<ushort> GetAchievementsIdsByCategory(uint category)
        {
            return m_achievementsTemplates
                .Where(entry => entry.Value.CategoryId == category)
                .Select(entry => (ushort)entry.Key);
        }

        public void AddCriterion(AbstractCriterion criterion)
        {
            if (criterion == null) { }

            m_criterions.Add(criterion.Criterion, criterion);
            if (criterion.IsIncrementable)
            {
                AddIncrementableCriterion(criterion);
            }
        }

        public bool AddAchievementCriterion(AchievementCriterion criterion)
        {
            bool result;
            if (m_achievementCriterions.ContainsKey(criterion.Achievement))
            {
                result = false;
            }
            else
            {
                m_achievementCriterions.Add(criterion.Achievement, criterion);
                result = true;
            }
            return result;
        }

        public bool AddKillMonsterWithChallengeCriterion(KillMonsterWithChallengeCriterion criterion)
        {
            bool result;
            if (m_monsterCriterions.ContainsKey(criterion.Monster))
            {
                result = false;
            }
            else
            {
                m_monsterCriterions.Add(criterion.Monster, criterion);
                result = true;
            }
            return result;
        }

        public bool AddKillBossCriterion(KillBossCriterion criterion)
        {
            bool result;
            if (m_monsterBossCriterions.ContainsKey(criterion.Monster))
                result = false;
            else
            {
                m_monsterBossCriterions.Add(criterion.Monster, criterion);
                result = true;
            }
            return result;
        }

        private bool AddIncrementableCriterion(AbstractCriterion criterion)
        {
            var criterionType = criterion.GetType();
            if (!m_incrementableCriterions.ContainsKey(criterionType))
            {
                m_incrementableCriterions.Add(criterionType, criterion);
            }
            else
            {
                var min = m_incrementableCriterions[criterionType];
                if (min < criterion)
                {
                    var temp = min;
                    var next = min.Next;
                    while (next != null && next < criterion)
                    {
                        temp = next;
                        next = temp.Next;
                    }

                    if (next == null)
                    {
                        temp.Next = criterion;
                    }
                    else
                    {
                        criterion.Next = next;
                        temp.Next = criterion;
                    }
                }
                else
                {
                    criterion.Next = min;
                    m_incrementableCriterions[criterionType] = criterion;
                }
            }

            return true;
        }

        public AchievementCriterion TryGetAchievementCriterion(AchievementTemplate achievement)
        {
            return m_achievementCriterions.ContainsKey(achievement) ? m_achievementCriterions[achievement] : null;
        }

        public bool TryGetAbstractCriterion(string criterion, out AbstractCriterion result)
        {
            result = null;
            if (m_criterions.ContainsKey(criterion))
            {
                result = m_criterions[criterion];
                return true;
            }

            return false;
        }

        public AchievementTemplate TryGetAchievement(uint id)
        {
            return m_achievementsTemplates.ContainsKey(id) ? m_achievementsTemplates[id] : null;
        }

        public AchievementCategoryRecord TryGetAchievementCategory(uint id)
        {
            return m_achievementsCategories.ContainsKey(id) ? m_achievementsCategories[id] : null;
        }

        public AchievementObjectiveRecord TryGetAchievementObjective(uint id)
        {
            return m_achievementsObjectives.ContainsKey(id) ? m_achievementsObjectives[id] : null;
        }

        public AchievementRewardRecord TryGetAchievementReward(uint id)
        {
            return m_achievemenstRewards.ContainsKey(id) ? m_achievemenstRewards[id] : null;
        }

        public KillMonsterWithChallengeCriterion TryGetCriterionByMonster(MonsterTemplate template)
        {
            return m_monsterCriterions.ContainsKey(template) ? m_monsterCriterions[template] : null;
        }

        public KillBossCriterion TryGetCriterionByBoss(MonsterTemplate template)
        {
            return m_monsterBossCriterions.ContainsKey(template) ? m_monsterBossCriterions[template] : null;
        }
    }
}
