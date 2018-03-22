using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Game.PvP
{
    /// <summary>
    /// Contains all data about pvp in any character register on Redis
    /// </summary>
    public class PvPManager : Singleton<PvPManager>
    {
        public object _lock = new object();
        [Initialization(InitializationPass.Last)]
        public void Initialize()
        {
            PvPLogs = RedisInstance.Instance.PvPLogs.GetAll();
        }

        public IList<PvPRegister> PvPLogs { get; set; }

        public PvPRegister GetCharacterPvPLogs(Character character)
        {
            lock (_lock)
            {
                var log = TryGetRegister(character.Id);
                if (log == null)
                {
                    log = RedisInstance.Instance.StorePvPLog(character);
                    PvPLogs.Add(log);
                }
                return log;
            }
        }

        public PvPRegister TryGetRegister(long characterId)
        {
            return PvPLogs.FirstOrDefault(x => x.Id == characterId);
        }
    }
}
