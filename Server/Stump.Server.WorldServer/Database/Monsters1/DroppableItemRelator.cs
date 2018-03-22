namespace Stump.Server.WorldServer.Database.Monsters
{
	public class DroppableItemRelator
	{
		public static string FetchQuery = "SELECT * FROM monsters_drops";
		public static string FetchByOwner = "SELECT * FROM monsters_drops WHERE MonsterOwnerId = {0}";
	}
}
