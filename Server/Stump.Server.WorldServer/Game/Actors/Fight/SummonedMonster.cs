using System.Globalization;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using System.Collections.Generic;
using System.Collections;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Actors.Interfaces;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public class SummonedMonster : SummonedFighter, ICreature
    {
        readonly StatsFields m_stats;

        public SummonedMonster(int id, FightTeam team, FightActor summoner, MonsterGrade template, Cell cell)
            : base(id, team, template.Spells.ToArray(), summoner, cell, template.MonsterId, template)
        {
            Monster = template;
            Look = Monster.Template.EntityLook.Clone();
            m_stats = new StatsFields(this);
            m_stats.Initialize(template);

            if (Monster.Template.Race.SuperRaceId == 28) //Invocations
                AdjustStats();
        }

        void AdjustStats()
        {
            var stats = new List<int>
            {
                Summoner.Stats.Intelligence.Total,
                Summoner.Stats.Agility.Total,
                Summoner.Stats.Chance.Total,
                Summoner.Stats.Strength.Total

            };
            stats.Sort();
            stats.Reverse();
            var boost = (short)(((1 * Summoner.Level) / 2) + 2) * (Summoner.Level / 100) + (stats[0] / 10);
            var maxmultiplier = (short)((1 + (Summoner.Level / 500d)));
            maxmultiplier = maxmultiplier > 2 ? (short)2 : maxmultiplier;
            m_stats.Health.Base *= maxmultiplier;
           
            switch (this.Monster.Template.Id)
            {
                //case 117: //hinchable
                //    m_stats.Health.Base *= (short)((2 + (Summoner.Level / 100d)));
                //    m_stats.Intelligence.Base += boost;
                //    break;
                //DOPEULS E INVOS ROTAS
                case 3136: // ZOBAL
                case 2609: // OSAMODAS
                case 963: // UGINAK
                case 960: // ZURCARAK
                case 955: // FECA
                case 958: // SRAM
                case 962: // YOPUKA
                case 964: // SADIDA
                case 957: // ANUTROF
                case 2608: // SACOGRITO
                case 969: // PANDAWA
                case 959: // XELOR 
                case 961: // ANIRIPSA
                case 262: // CHAFER
                case 4312: // HIPERMAGO
                case 3303: // STEAMER
                case 3131: // TYMADOR
                case 2630: // ARAKNA
                case 3990: // SELOTROP
                case 4802: // UGINAK
                    m_stats.Health.Base *= (3);
                    m_stats.Intelligence.Base *= (m_stats.Intelligence.Base * 3);
                    m_stats.Chance.Base *= (m_stats.Chance.Base * 3);
                    m_stats.Strength.Base *= (m_stats.Strength.Base * 3);
                    m_stats.Agility.Base *= (m_stats.Agility.Base * 3);
                    break;

                case 4564: // JALATO BLANCO
                case 4563: // JALATO NEGRO
                    m_stats.Intelligence.Base *= (m_stats.Intelligence.Base * 1) + boost;
                    m_stats.Chance.Base *= (m_stats.Chance.Base * 1) + boost;
                    m_stats.Strength.Base *= (m_stats.Strength.Base * 1) + boost;
                    m_stats.Agility.Base *= (m_stats.Agility.Base * 1) + boost;
                    break;
                case 117: // HINCHABLE
                case 114: // LOCA
                    m_stats.Health.Base *= (2);
                    m_stats.Wisdom.Base *= (short)((2 + (Summoner.Level / 170d)));
                    m_stats.Intelligence.Base *= (m_stats.Intelligence.Base * 13);
                    m_stats.Chance.Base *= (m_stats.Chance.Base * 2);
                    m_stats.Strength.Base *= (m_stats.Strength.Base * 2);
                    m_stats.Agility.Base *= (m_stats.Agility.Base * 16);
                    break;
                case 115: // BLOQUEADORA
                    m_stats.Health.Base *= (3);
                    break;
                case 42: // SUPERPODEROSA
                    m_stats.Health.Base *= (2);
                    m_stats.Wisdom.Base *= (short)((1 + (Summoner.Level / 140d)));
                    m_stats.Agility.Base *= (m_stats.Agility.Base * 20);
                    break;
                case 516: // PANDAWASTA
                case 3960: // ESFERA
                    m_stats.Health.Base *= (2);
                    m_stats.Intelligence.Base *= (m_stats.Intelligence.Base * 10) + boost;
                    m_stats.Chance.Base *= (m_stats.Chance.Base * 10) + boost;
                    m_stats.Strength.Base *= (m_stats.Strength.Base * 10) + boost;
                    m_stats.Agility.Base *= (m_stats.Agility.Base * 10) + boost;
                    break;
                case 39: // CUNEJO
                    m_stats.Health.Base *= (2);
                    break;
                case 285: // COFRE
                    m_stats.Health.Base *= (3);
                    m_stats.Intelligence.Base *= (m_stats.Intelligence.Base * 5);
                    m_stats.Chance.Base *= (m_stats.Chance.Base * 5);
                    m_stats.Strength.Base *= (m_stats.Strength.Base * 5);
                    m_stats.Agility.Base *= (m_stats.Agility.Base * 5);
                    break;
                case 237: // MOCHILA
                    m_stats.Health.Base *= (4);
                    m_stats.Intelligence.Base *= (m_stats.Intelligence.Base * 10);
                    m_stats.Chance.Base *= (m_stats.Chance.Base * 10);
                    m_stats.Strength.Base *= (m_stats.Strength.Base * 10);
                    m_stats.Agility.Base *= (m_stats.Agility.Base * 10);
                    break;
                case 4776: // INVO UGINAK
                case 45: // INVO ZURKA
                    m_stats.Health.Base *= (2);
                    m_stats.Intelligence.Base *= (m_stats.Intelligence.Base * 16);
                    m_stats.Chance.Base *= (m_stats.Chance.Base * 16);
                    m_stats.Strength.Base *= (m_stats.Strength.Base * 16);
                    m_stats.Agility.Base *= (m_stats.Agility.Base * 16);
                    break;
                case 3958:
                    m_stats.Intelligence.Base *= (m_stats.Intelligence.Base * 20);
                    m_stats.Chance.Base *= (m_stats.Chance.Base * 20);
                    m_stats.Strength.Base *= (m_stats.Strength.Base * 20);
                    m_stats.Agility.Base *= (m_stats.Agility.Base * 21);
                    break;


                default:

                    {
                        // +1% bonus per level
                        m_stats.Wisdom.Base *= (short)((3 + (Summoner.Level / 180d)));
                        m_stats.Intelligence.Base *= (m_stats.Intelligence.Base * 8) + boost;
                        m_stats.Chance.Base *= (m_stats.Chance.Base * 8) + boost;
                        m_stats.Strength.Base *= (m_stats.Strength.Base * 8) + boost;
                        m_stats.Agility.Base *= (m_stats.Agility.Base * 8) + boost;
                        //m_stats.Intelligence.Base *= (short)((200 + (Summoner.Level / 1d)));    
                        //m_stats.Chance.Base *= (short)((100 + (Summoner.Level / 1d)));
                        //m_stats.Strength.Base *= (short)((170 + (Summoner.Level / 1d)));
                        //m_stats.Agility.Base *= (short)((80 + (Summoner.Level / 1d)));
                       
                        break;
                    }

            };
            List<StatsData> stat = new List<StatsData> {
                m_stats.Health,
                m_stats.Intelligence,
                m_stats.Chance,
                m_stats.Strength,
                m_stats.Agility,
                m_stats.Wisdom};
            stat.AddRange(stat);
            //if ((Summoner as CharacterFighter).Character.UserGroup.Role > RoleEnum.Player)
            //{
                //(Summoner as CharacterFighter).Character.SendServerMessage($"{this.Monster.Template.Id}/{this.Monster.Template.Name}");
                //foreach (var s in stat)
                //    (Summoner as CharacterFighter).Character.SendServerMessage($"{s.Name}=>{s.Base}");
                //(Summoner as CharacterFighter).Character.SendServerMessage($"Boost of => {boost}");
            //}
        }

        public override int CalculateArmorValue(int reduction)
        {
            return (int)(reduction * (100 + 5 * Summoner.Level) / 100d);
        }

        public override bool CanPlay() => base.CanPlay() && Monster.Template.CanPlay;

        public override bool CanMove() => base.CanMove() && MonsterGrade.MovementPoints > 0;

        public override bool CanTackle(FightActor fighter) => base.CanTackle(fighter) && Monster.Template.CanTackle;

        public MonsterGrade Monster
        {
            get;
        }

        public override ObjectPosition MapPosition
        {
            get { return Position; }
        }

        public override byte Level
        {
            get { return (byte)Monster.Level; }
        }

        public MonsterGrade MonsterGrade
        {
            get { return Monster; }
        }

        public override StatsFields Stats
        {
            get { return m_stats; }
        }

        public override string GetMapRunningFighterName()
        {
            return Monster.Id.ToString(CultureInfo.InvariantCulture);
        }

        public override string Name
        {
            get { return Monster.Template.Name; }
        }

        public override bool CanBePushed()
        {
            return base.CanBePushed() && Monster.Template.CanBePushed;
        }

        public override bool CanSwitchPos()
        {
            return base.CanSwitchPos() && Monster.Template.CanSwitchPos;
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberMonsterInformations(Id, Monster.Template.Id, (sbyte)Monster.GradeId);
        }

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightMonsterInformations(
                Id,
                Look.GetEntityLook(),
                GetEntityDispositionInformations(client),
                (sbyte)Team.Id,
                0,
                IsAlive(),
                GetGameFightMinimalStats(client),
                new short[0],
                (short)Monster.Template.Id,
                (sbyte)Monster.GradeId);
        }

        public override GameFightFighterLightInformations GetGameFightFighterLightInformations(WorldClient client = null)
        {
            return new GameFightFighterMonsterLightInformations(
                true,
                IsAlive(),
                Id,
                0,
                Level,
                (sbyte)BreedEnum.MONSTER,
                (short)Monster.Template.Id);
        }

        public override GameFightMinimalStats GetGameFightMinimalStats(WorldClient client = null)
        {
            return new GameFightMinimalStats(
                Stats.Health.Total,
                Stats.Health.TotalMax,
                Stats.Health.TotalMaxWithoutPermanentDamages,
                Stats[PlayerFields.PermanentDamagePercent].Total,
                Stats.Shield.TotalSafe,
                (short)Stats.AP.Total,
                (short)Stats.AP.TotalMax,
                (short)Stats.MP.Total,
                (short)Stats.MP.TotalMax,
                Summoner.Id,
                true,
                (short)Stats[PlayerFields.NeutralResistPercent].Total,
                (short)Stats[PlayerFields.EarthResistPercent].Total,
                (short)Stats[PlayerFields.WaterResistPercent].Total,
                (short)Stats[PlayerFields.AirResistPercent].Total,
                (short)Stats[PlayerFields.FireResistPercent].Total,
                (short)Stats[PlayerFields.NeutralElementReduction].Total,
                (short)Stats[PlayerFields.EarthElementReduction].Total,
                (short)Stats[PlayerFields.WaterElementReduction].Total,
                (short)Stats[PlayerFields.AirElementReduction].Total,
                (short)Stats[PlayerFields.FireElementReduction].Total,
                (short)Stats[PlayerFields.CriticalDamageReduction].Total,
                (short)Stats[PlayerFields.PushDamageReduction].Total,
                (short)Stats[PlayerFields.PvpNeutralResistPercent].Total,
                (short)Stats[PlayerFields.PvpEarthResistPercent].Total,
                (short)Stats[PlayerFields.PvpWaterResistPercent].Total,
                (short)Stats[PlayerFields.PvpAirResistPercent].Total,
                (short)Stats[PlayerFields.PvpFireResistPercent].Total,
                (short)Stats[PlayerFields.PvpNeutralElementReduction].Total,
                (short)Stats[PlayerFields.PvpEarthElementReduction].Total,
                (short)Stats[PlayerFields.PvpWaterElementReduction].Total,
                (short)Stats[PlayerFields.PvpAirElementReduction].Total,
                (short)Stats[PlayerFields.PvpFireElementReduction].Total,
                (short)Stats[PlayerFields.DodgeAPProbability].Total,
                (short)Stats[PlayerFields.DodgeMPProbability].Total,
                (short)Stats[PlayerFields.TackleBlock].Total,
                (short)Stats[PlayerFields.TackleEvade].Total,
                0,
                (sbyte)(client == null ? VisibleState : GetVisibleStateFor(client.Character)), // invisibility state
                (short)Stats[PlayerFields.MeleeDamageReceivedPercent].Total,
                    (short)Stats[PlayerFields.RangedDamageReceivedPercent].Total,
                    (short)Stats[PlayerFields.WeaponDamageReceivedPercent].Total,
                    (short)Stats[PlayerFields.SpellDamageReceivedPercent].Total
                );
        }
    }
}