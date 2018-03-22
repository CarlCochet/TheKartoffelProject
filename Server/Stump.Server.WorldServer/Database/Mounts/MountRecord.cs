﻿using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.ORM;
using Stump.ORM.Relator;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;

namespace Stump.Server.WorldServer.Database.Mounts
{
	public class MountRecordRelator
	{
		public static string FetchQuery = "SELECT * FROM mounts";
		/// <summary>
		/// Use string.Format
		/// </summary>
		public static string FindById = "SELECT * FROM mounts WHERE Id={0}";

		public static string FindByOwner = "SELECT * FROM mounts WHERE OwnerId = {0}";

		public static string FindByOwnerStabled = "SELECT * FROM mounts WHERE OwnerId = {0} AND IsInStable = 1 AND PaddockId IS NOT NULL";

		public static string FindByOwnerPublicPaddocked = "SELECT * FROM mounts " +
			"INNER JOIN world_maps_paddock ON world_maps_paddock.Id = mounts.PaddockId WHERE mounts.OwnerId = {0} AND mounts.PaddockId IS NOT NULL AND mounts.IsInStable = 0 AND world_maps_paddock.GuildId IS NULL";

		public static string FetchByPaddockId = "SELECT * FROM mounts WHERE PaddockId={0} AND IsInStable=0";

		public static string DeleteStoredSince = "DELETE FROM mounts WHERE StoredSince IS NOT NULL AND StoredSince < \"{0}\"";
	}

	[TableName("mounts")]
	public class MountRecord : AutoAssignedRecord<MountRecord>, IJoined, IAutoGeneratedRecord
	{
		private string m_behaviorsCSV = string.Empty;
		private List<int> m_behaviors;
		private MountTemplate m_template;


		[Ignore]
		public bool IsDirty
		{
			get;
			set;
		}

		public String Name
		{
			get;
			set;
		}

		public Boolean Sex
		{
			get;
			set;
		}

		public int TemplateId
		{
			get;
			set;
		}

		[Ignore]
		public MountTemplate Template => m_template ?? (m_template = MountManager.Instance.GetTemplate(TemplateId));

		public long Experience
		{
			get;
			set;
		}

		public sbyte GivenExperience
		{
			get;
			set;
		}

		public int Stamina
		{
			get;
			set;
		}

		public int Maturity
		{
			get;
			set;
		}

		public int Energy
		{
			get;
			set;
		}

		public int Serenity
		{
			get;
			set;
		}

		public int Love
		{
			get;
			set;
		}

		public int ReproductionCount
		{
			get;
			set;
		}

		[Ignore]
		public List<int> Behaviors
		{
			get
			{
				return m_behaviors ??
					   (m_behaviors = BehaviorsCSV.FromCSV<int>(",").ToList());
			}
			set
			{
				m_behaviors = value;
				BehaviorsCSV = value.ToCSV(",");
			}
		}

		public string BehaviorsCSV
		{
			get { return m_behaviorsCSV; }
			set
			{
				m_behaviorsCSV = value;
				m_behaviors = BehaviorsCSV.FromCSV<int>(",").ToList();
			}
		}

		[Index]
		public int? OwnerId
		{
			get;
			set;
		}

		[NullString]
		public string OwnerName
		{
			get;
			set;
		}

		[Index]
		public int? PaddockId
		{
			get;
			set;
		}

		int IJoined.JoinedId => PaddockId ?? -1;

		public bool IsInStable
		{
			get;
			set;
		}

		public DateTime? StoredSince
		{
			get;
			set;
		}

		public override void BeforeSave(bool insert)
		{
			BehaviorsCSV = Behaviors.ToCSV(",");
			base.BeforeSave(insert);
		}
	}
}
