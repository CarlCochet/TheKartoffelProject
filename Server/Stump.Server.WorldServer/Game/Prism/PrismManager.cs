using System.Collections.Generic;
using System.Linq;
using Stump.Core.Pool;
using Stump.Core.Reflection;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Prisms
{
	public class PrismManager : DataManager<PrismManager>, ISaveable
	{
		private readonly List<PrismNpc> m_activePrisms = new List<PrismNpc>();
		// FIELDS
		private UniqueIdProvider m_idProvider;
		private Dictionary<int, WorldMapPrismRecord> m_prismSpawns;

		public void Save()
		{
		}

		// PROPERTIES

		// CONSTRUCTORS

		// METHODS
		[Initialization(InitializationPass.Eighth)]
		public override void Initialize()
		{
			m_prismSpawns = Database.Query<WorldMapPrismRecord>(WorldMapPrismRelator.FetchQuery)
				.ToDictionary(entry => entry.Id);

			m_idProvider = m_prismSpawns.Any()
				? new UniqueIdProvider((from item in m_prismSpawns select item.Value.Id).Max())
				: UniqueIdProvider.Default;

			Singleton<World>.Instance.RegisterSaveableInstance(this);
			Singleton<World>.Instance.SpawnPrisms();
		}

		public WorldMapPrismRecord[] GetPrismSpawns()
		{
			return m_prismSpawns.Values.ToArray();
		}

		public WorldMapPrismRecord[] GetPrismSpawns(int allianceId)
		{
			return (
				from entry in m_prismSpawns.Values
				where entry.AllianceId == allianceId
				select entry).ToArray();
		}

		public bool TryAddPrism(Character character, bool lazySave = true)
		{
			bool result = false;
			if (character.SubArea.Record.Capturable && !character.SubArea.HasPrism)
			{
				var position = character.Position.Clone();
				var npc = new PrismNpc(m_idProvider.Pop(), position.Map.GetNextContextualId(), position,
					character.Guild.Alliance);
				if (lazySave)
				{
					ServerBase<WorldServer>.Instance.IOTaskPool.AddMessage(delegate { Database.Insert(npc.Record); });
				}
				else
				{
					Database.Insert(npc.Record);
				}
				m_prismSpawns.Add(npc.GlobalId, npc.Record);
				m_activePrisms.Add(npc);
				npc.Map.Enter(npc);
				character.Guild.Alliance.AddPrism(npc);

				result = true;
			}
			return result;
		}
	}
}