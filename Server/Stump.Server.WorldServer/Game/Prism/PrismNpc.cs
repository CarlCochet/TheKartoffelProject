using System;
using System.Collections.Generic;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Alliances;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Game.Dialogs.Prisms;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Prisms
{
	public class PrismNpc : RolePlayActor, IAutoMovedEntity, IContextDependant, IInteractNpc
	{
		private readonly int m_contextId;
		// FIELDS
		private readonly List<IDialog> m_openedDialogs = new List<IDialog>();
		private ActorLook m_look;

		// CONSTRUCTORS
		public PrismNpc(int globalId, int contextId, ObjectPosition position, Alliance alliance)
		{
			m_contextId = contextId;
			Position = position;
			Alliance = alliance;
			Record = new WorldMapPrismRecord
			{
				Id = globalId,
				Map = Position.Map,
				Cell = Position.Cell.Id,
				AllianceId = alliance.Id,
				Date = DateTime.Now
			};
			Position.Map.SubArea.HasPrism = true;
			IsDirty = true;
		}

		public sealed override ObjectPosition Position
		{
			get { return base.Position; }
			protected set { base.Position = value; }
		}

		public PrismNpc(WorldMapPrismRecord record, int contextId)
		{
			Record = record;
			m_contextId = contextId;
			if (!record.MapId.HasValue)
			{
				throw new Exception("Prism's map not found");
			}

			Position = new ObjectPosition(record.Map, record.Map.Cells[Record.Cell], DirectionsEnum.DIRECTION_EAST);
			Alliance = Singleton<AllianceManager>.Instance.TryGetAlliance(Record.AllianceId);
		}

		public int GlobalId
		{
			get { return Record.Id; }
			protected set { Record.Id = value; }
		}

		public WorldMapPrismRecord Record { get; }

		public Alliance Alliance { get; }

		public override ActorLook Look => m_look ?? RefreshLook();

		public bool IsDirty { get; private set; }
		public DateTime NextMoveDate { get; set; }
		public DateTime LastMoveDate { get; private set; }

		// PROPERTIES
		public override int Id => m_contextId;

		public void InteractWith(NpcActionTypeEnum actionType, Character dialoguer)
		{
			if (CanInteractWith(actionType, dialoguer))
			{
				var infoDialog = new PrismInfoDialog(dialoguer, this);
				infoDialog.Open();
			}
		}

		public bool CanInteractWith(NpcActionTypeEnum action, Character dialoguer)
		{
			return CanBeSee(dialoguer) && action == NpcActionTypeEnum.ACTION_TALK;
		}

		public override bool StartMove(Path movementPath)
		{
			bool result;
			if (!CanMove() || movementPath.IsEmpty())
			{
				result = false;
			}
			else
			{
				Position = movementPath.EndPathPosition;
				var keys = movementPath.GetServerPathKeys();
				Map.ForEach(
					delegate (Character entry) { ContextHandler.SendGameMapMovementMessage(entry.Client, keys, this); });
				StopMove();
				LastMoveDate = DateTime.Now;
				result = true;
			}
			return result;
		}

		// METHODS
		public ActorLook RefreshLook()
		{
			m_look = new ActorLook
			{
				BonesID = 2211
			};
			if (this.Alliance.Emblem.Template != null)
			{
				this.m_look.AddSkin((short)this.Alliance.Emblem.Template.IconId);
				this.m_look.AddSkin((short)this.Alliance.Emblem.Template.SkinId);
			}
			this.m_look.AddColor(1, this.Alliance.Emblem.BackgroundColor);
			this.m_look.AddColor(2, this.Alliance.Emblem.SymbolColor);
			this.m_look.AddColor(3, this.Alliance.Emblem.BackgroundColor);
			this.m_look.AddColor(4, this.Alliance.Emblem.SymbolColor);
			this.m_look.AddColor(5, this.Alliance.Emblem.BackgroundColor);
			this.m_look.AddColor(6, this.Alliance.Emblem.SymbolColor);
			this.m_look.AddColor(7, this.Alliance.Emblem.BackgroundColor);
			this.m_look.AddColor(8, this.Alliance.Emblem.SymbolColor);

			return m_look;
		}

		public override GameContextActorInformations GetGameContextActorInformations(Character character)
		{
			return new GameRolePlayPrismInformations(Id, Look.GetEntityLook(), GetEntityDispositionInformations(),
				GetAlliancePrismInformation());
		}

		public PrismInformation GetAlliancePrismInformation()
		{
			return new AlliancePrismInformation(
				(sbyte)PrismListenEnum.PRISM_LISTEN_MINE,
				(sbyte)PrismStateEnum.PRISM_STATE_NORMAL,
				Record.Date.GetUnixTimeStamp(),
				Record.Date.GetUnixTimeStamp(),
				0,
				Alliance.GetAllianceInformations());
		}

		public PrismInformation GetAllianceInsiderPrismInformation()
		{
			return new AllianceInsiderPrismInformation(
				(sbyte)PrismListenEnum.PRISM_LISTEN_MINE,
				(sbyte)PrismStateEnum.PRISM_STATE_NORMAL,
				Record.Date.GetUnixTimeStamp(),
				Record.Date.GetUnixTimeStamp(),
				0,
				0,
				0,
				0,
				"Artorias",
				new ObjectItem[0]); //TODO Revisar....
		}

		public void OnDialogOpened(IDialog dialog)
		{
			m_openedDialogs.Add(dialog);
		}

		public void OnDialogClosed(IDialog dialog)
		{
			m_openedDialogs.Remove(dialog);
		}
	}
}