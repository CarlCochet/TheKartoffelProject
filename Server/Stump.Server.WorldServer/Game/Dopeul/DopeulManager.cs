using Stump.Server.BaseServer.Database;

using System.Collections.Generic;
using System.Linq;
using Database.Dopeul;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Server.WorldServer.Game.Dopeul
{
    class DopeulManager : DataManager<DopeulManager>
    {
        private List<DopeulRecord> _records = new List<DopeulRecord>();

        [Initialization(InitializationPass.Seventh)]
        public override void Initialize()
        {
            _records = Database.Fetch<DopeulRecord>("select * from characters_dopeul");
        }

        public List<DopeulRecord> GetCharacterRecords(int characterId)
        {
            return _records.Where(x => x.CharacterId == characterId).ToList();
        }

        public List<DopeulRecord> GetCharacterRecords(string ip)
        {
            return _records.Where(x => x.Ip == ip).ToList();
        }

        public void AddRecord(DopeulRecord record)
        {
            _records.Add(record);
        }

        public void DeleteRecord(DopeulRecord record)
        {
            _records.Remove(record);
        }
    }
}
