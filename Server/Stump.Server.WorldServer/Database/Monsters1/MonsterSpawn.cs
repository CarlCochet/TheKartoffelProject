using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Server.WorldServer.Database.Monsters
{
    public class MonsterSpawnRelator
    {
        public static string FetchQuery = "SELECT * FROM monsters_spawns";
    }

    [TableName("monsters_spawns")]
    public class MonsterSpawn : IAutoGeneratedRecord
    {
        private Map m_map;
        private SubArea m_subArea;

        public int Id
        {
            get;
            set;
        }

        public int? MapId
        {
            get;
            set;
        }

        [Ignore]
        public Map Map
        {
            get
            {
                if (!MapId.HasValue)
                    return null;

                return m_map ?? (m_map = Game.World.Instance.GetMap(MapId.Value));
            }
            set
            {
                m_map = value;

                if (value == null)
                    MapId = null;
                else
                    MapId = value.Id;
            }
        }

        public int? SubAreaId
        {
            get;
            set;
        }

        [Ignore]
        public SubArea SubArea
        {
            get
            {
                if (!SubAreaId.HasValue)
                    return null;

                return m_subArea ?? (m_subArea = Game.World.Instance.GetSubArea(SubAreaId.Value));
            }
            set
            {
                m_subArea = value;

                if (value == null)
                    SubAreaId = null;
                else
                    SubAreaId = value.Id;
            }
        }

        public int MonsterId
        {
            get;
            set;
        }
        public MonsterTemplate Template => MonsterManager.Instance.GetTemplate(MonsterId);
        [DefaultSetting(1.0)]
        [NumericPrecision(16, 8)]
        public double Frequency
        {
            get
            {
                return Template == null ? 0 : Template.IsMiniBoss ? 0.006 : 1;
            }
        }

        [DefaultSetting(1)]
        public int MinGrade
        {
            get;
            set;
        }

        [DefaultSetting(5)]
        public int MaxGrade
        {
            get;
            set;
        }

        public bool IsDisabled
        {
            get;
            set;
        }
    }
}