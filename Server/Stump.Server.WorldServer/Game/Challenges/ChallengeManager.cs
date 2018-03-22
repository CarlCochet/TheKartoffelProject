using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Challenges;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Challenges {
    public class ChallengeManager : DataManager<ChallengeManager>, ISaveable {
        // FIELDS
        private Dictionary<int, ChallengeRecord> m_challenges;
        private List<ChallengeChecker> m_checkerPatterns;

        // PROPERTIES

        // CONSTRUCTORS
        private ChallengeManager() {}

        public void Save() {}

        // METHODS
        [Initialization(InitializationPass.Seventh)]
        public override void Initialize() {
            m_challenges = Database.Query<ChallengeRecord>(ChallengeRelator.FetchQuery)
                .ToDictionary(entry => entry.Id);

            m_checkerPatterns = (from type in typeof(ChallengeChecker).Assembly.GetTypes()
                where type.IsSubclassOf(typeof(ChallengeChecker))
                select (ChallengeChecker) Activator.CreateInstance(type)).ToList();

            Singleton<World>.Instance.RegisterSaveableInstance(this);
        }

        public ChallengeRecord GetChallenge(int id) {
            return m_challenges.ContainsKey(id) ? m_challenges[id] : null;
        }

        public void GenerateChallenges(Fight fight) {
            var checkers = m_checkerPatterns.Where(entry => entry.IsCompatible(fight))
                .ToList();

            for (var i = 0; i < fight.NumberChallenges && checkers.Count > 0; i++) {
                var current = checkers.RandomElementOrDefault();

                fight.AddChallenge(current.BuildChallenge(fight));

                checkers.Remove(current);
            }
        }
    }
}