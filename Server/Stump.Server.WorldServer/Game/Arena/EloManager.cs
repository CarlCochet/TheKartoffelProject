using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Elo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Game.Arena
{
    public class EloManager : DataManager<EloManager>
    {
        private readonly Dictionary<short, EloRecord> m_elos;

        public EloManager()
        {
            this.m_elos = new Dictionary<short, EloRecord>();
        }

        [Initialization(InitializationPass.Third)]
        public override void Initialize()
        {
            base.Initialize();
            foreach (EloRecord current in base.Database.Query<EloRecord>(EloRelator.FetchQuery, new object[0]))
            {
                this.m_elos.Add(current.Difference, current);
            }
        }

        public double GetProbability(short difference)
        {
            var result = 0.5;

            KeyValuePair<short, EloRecord> record = this.m_elos.FirstOrDefault(entry => entry.Key > difference);
            if (!record.Equals(default(KeyValuePair<short, EloRecord>)))
            {
                result = record.Value.Probability;
            }

            return result;
        }
    }
}
