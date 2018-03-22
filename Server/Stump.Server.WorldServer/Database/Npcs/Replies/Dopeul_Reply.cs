
using Database.Dopeul;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Replies;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dopeul;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Handlers.Context;
using System;
using System.Drawing;
using System.Linq;

namespace Database.Npcs.Replies
{
    [Discriminator("Dopeul", typeof(NpcReply), typeof(NpcReplyRecord))]
    internal class DopeulReplies : NpcReply
    {
        public DopeulReplies(NpcReplyRecord record) : base(record)
        {
        }

        public int MonsterId
        {
            get {
                var result = 0;
                try
                {
                    result = Record.GetParameter<int>(0u);
                }
                catch { }
                return result;
            }
            set { Record.SetParameter(0u, value); }
        }

        public override bool Execute(Npc npc, Character character)
        {
            DopeulRecord EditDopple = null;
            var compareTime = DateTime.Now;
            character.DopeulCollection.Load(character.Id);
            foreach (var dopeul in character.DopeulCollection.Dopeul.Where(dopeul => dopeul.DopeulId == MonsterId))
            {
                EditDopple = dopeul;
                compareTime = dopeul.Time;
                break;
            }
            if (compareTime <= DateTime.Now)
            {
                var dopplesIp = new DopeulCollection();
                dopplesIp.Load(character.Client.IP);
                foreach (var dopple in dopplesIp.Dopeul.Where(dopeul => dopeul.DopeulId == MonsterId))
                {
                    EditDopple = dopple;
                    compareTime = dopple.Time;
                    break;
                }
                if (!(compareTime <= DateTime.Now))
                {
                    switch (character.Account.Lang)
                    {
                        case "es":
                            character.SendServerMessage(
                        $"Espera, no puedes hacer esto!, El Dopeul esta cansado, la lucha se llevo con alguno de tus otros personajes, Debes esperar <b>{compareTime.Subtract(DateTime.Now).Hours} horas, {compareTime.Subtract(DateTime.Now).Minutes} minutos</b>",
                        Color.Red);
                            break;
                        case "fr":
                            character.SendServerMessage(
                        $"Espera, no puedes hacer esto!, El Dopeul esta cansado, la lucha se llevo con alguno de tus otros personajes, Debes esperar <b>{compareTime.Subtract(DateTime.Now).Hours} horas, {compareTime.Subtract(DateTime.Now).Minutes} minutos</b>",
                        Color.Red);
                            break;
                        default:
                            character.SendServerMessage(
                        $"Espera que no puedas hacer esto, alguna ya peleaste con un dopeul en alguna de tus otras cuentas !, Tienes que esperar <b>{compareTime.Subtract(DateTime.Now).Hours} horas, {compareTime.Subtract(DateTime.Now).Minutes} minutos</b>",
                        Color.Red);
                            break;
                    }
                    character.LeaveDialog();
                    return false;
                }
                var monsterGradeId = 1;
                while (monsterGradeId < 12)
                {
                    if (character.Level > monsterGradeId * 20 + 10)
                        monsterGradeId++;
                    else
                        break;
                }
                var grade = Singleton<MonsterManager>.Instance.GetMonsterGrade(MonsterId, monsterGradeId);
                var position = new ObjectPosition(character.Map, character.Cell, (DirectionsEnum)5);
                var monster = new Monster(grade, new MonsterGroup(0, position));

                var fight = Singleton<FightManager>.Instance.CreatePvDFight(character.Map);
                fight.ChallengersTeam.AddFighter(character.CreateFighter(fight.ChallengersTeam));
                fight.DefendersTeam.AddFighter(new MonsterFighter(fight.DefendersTeam, monster));
                fight.StartPlacement();

                ContextHandler.HandleGameFightJoinRequestMessage(character.Client,
                    new GameFightJoinRequestMessage(character.Fighter.Id, fight.Id));
                if (EditDopple != null)
                {
                    EditDopple.Ip = character.Client.IP;
                    EditDopple.IsUpdated = true;
                    EditDopple.Time = DateTime.Now.AddHours(2);
                }
                else
                {
                    Singleton<ItemManager>.Instance.CreateDopeul(character, MonsterId);
                }
                character.SaveLater();
                return true;
            }
            switch (character.Account.Lang)
            {
                case "es":
                    character.SendServerMessage(
                $"Espera, no puedes hacer esto!, El Dopeul esta cansado, Debes esperar <b>{compareTime.Subtract(DateTime.Now).Hours} horas, {compareTime.Subtract(DateTime.Now).Minutes} minutos</b>",
                Color.Red);
                    break;
                case "fr":
                    character.SendServerMessage(
                $"Espera, no puedes hacer esto!, El Dopeul esta cansado, la lucha se llevo con alguno de tus otros personajes, Debes esperar <b>{compareTime.Subtract(DateTime.Now).Hours} horas, {compareTime.Subtract(DateTime.Now).Minutes} minutos</b>",
                Color.Red);
                    break;
                default:
                    character.SendServerMessage(
                $"Espera que no puedas hacer esto, alguna ya peleaste con un dopeul en alguna de tus otras cuentas !, Tienes que esperar <b>{compareTime.Subtract(DateTime.Now).Hours} horas, {compareTime.Subtract(DateTime.Now).Minutes} minutos</b>",
                Color.Red);
                    break;
            }
            character.LeaveDialog();
            return false;
        }
    }
}
