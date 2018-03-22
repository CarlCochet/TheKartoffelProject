namespace Stump.Server.WorldServer.Database.Alliances
{
    public class AllianceRelator
    {
        public static string FetchQuery = "SELECT * FROM alliances";
        public static string FetchById = "SELECT * FROM alliances WHERE Id={0}";
    }
}
