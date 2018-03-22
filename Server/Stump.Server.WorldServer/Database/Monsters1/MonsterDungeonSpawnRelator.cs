namespace Stump.Server.WorldServer.Database.Monsters
{
	public class MonsterDungeonSpawnRelator
	{
		public static string FetchQuery = "SELECT * FROM monsters_spawns_dungeons INNER JOIN monsters_spawns_dungeons_groups ON monsters_spawns_dungeons_groups.DungeonSpawnId = monsters_spawns_dungeons.Id";

		private MonsterDungeonSpawn m_current;

		public MonsterDungeonSpawn Map(MonsterDungeonSpawn spawn, MonsterDungeonSpawnEntity dummy)
		{
			MonsterDungeonSpawn result;
			if (spawn == null)
			{
				result = this.m_current;
			}
			else
			{
				if (this.m_current != null && this.m_current.Id == spawn.Id)
				{
                    this.m_current.GroupMonsters.Add(dummy);
					result = null;
				}
				else
				{
					MonsterDungeonSpawn current = m_current;
					this.m_current = spawn;
                    this.m_current.GroupMonsters.Add(dummy);
					result = current;
				}
			}
			return result;
		}
	}
}
