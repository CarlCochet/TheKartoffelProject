using System;
using System.Collections.Generic;
using Stump.Core.Attributes;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using NLog;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Database.World;
using Stump.Core.Pool;
using Stump.Core.Timers;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Core.Reflection;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using System.Linq;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Core.Extensions;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Database.Items.Templates;
using System.Collections.ObjectModel;
using System.Drawing;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.WorldServer.Game.Challenges
{
    public abstract class Fight : WorldObjectsContext, ICharacterContainer
    {
        public WorldClientCollection Clients => throw new NotImplementedException();

        public CharacterFighter FighterPlaying { get; internal set; }
        public FightTeam RedTeam { get; }

        public FightTeam BlueTeam { get; }
        public virtual ushort NumberChallenges => 0;

        public event Action<Fight, FightActor> TurnStarted;
        public event Action<Fight> FightStarted;
        public event Action<Fight> FightEnded;
        public event Action<Fight, FightActor> SummonAdded;

        private readonly List<FightActor> _mFighters = new List<FightActor>();

        public IEnumerable<FightActor> GetAllFighters()
        {
            return _mFighters;
        }

        protected readonly List<ChallengeChecker> MChallenges = new List<ChallengeChecker>();
        public void AddChallenge(ChallengeChecker challenge)
        {
            MChallenges.Add(challenge);
        }

        public void ForEach(Action<Character> action)
        {
            
        }

        public IEnumerable<Character> GetAllCharacters()
        {
            throw new NotImplementedException();
        }
    }
}