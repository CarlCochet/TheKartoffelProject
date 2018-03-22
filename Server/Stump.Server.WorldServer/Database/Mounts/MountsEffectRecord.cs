using System.Collections.Generic;
using Stump.DofusProtocol.Types;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace Stump.Server.WorldServer.Database.Mounts
{
	class MountsEffectRelator
	{
		public static string FetchQuery = "SELECT * FROM mounts_effect";
	}
	[TableName("mounts_effect")]
	public class MountsEffectRelation
	{
		public int Id { get; set; }
		public int MountId { get; set; }
		public string FormatedEffects { get; set; }

		public List<ObjectEffectInteger> Effects = new List<ObjectEffectInteger>();
	}
}