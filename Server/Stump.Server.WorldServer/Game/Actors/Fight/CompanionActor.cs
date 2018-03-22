using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Spells;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Database.Companion;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public sealed class CompanionActor : NamedFighter
    {
        // FIELDS

        int m_criticalWeaponBonus;
        int m_damageTakenBeforeFight;
        int m_weaponUses;
        bool m_left;

        // CONSTRUCTORS
        public CompanionActor(Character character, FightTeam team, ActorLook look, List<Spell> spell, byte companionId,
            sbyte id) : base(team)
        {
            FighterId = id;
            Master = character;
            Look = look;
            Look.RemoveAuras();
            CompanionId = companionId;
            CompanionSpell = spell;
            Cell cell;
            CompanionStats = new StatsFields(this);
            CompanionStats.Initialize(Master);
            if (!Fight.FindRandomFreeCell(this, out cell, false)) return;
            Position = new ObjectPosition(character.Map, cell, character.Direction);
            InitializeCharacterFighter();
        }

        public CompanionRecord Record { get; set; }

        // PROPERTIES
        public Character Master { get; }
        public byte CompanionId { get; set; }

        public ReadyChecker PersonalReadyChecker { get; set; }
        public List<Spell> CompanionSpell { get; set; }

        public int FighterId { get; set; }

        public override int Id => FighterId;

        public override string Name => Master.Name;

        public override ActorLook Look { get; set; }

        public override ObjectPosition MapPosition => Position;

        public override byte Level => Master.Level;

        public StatsFields CompanionStats { get; set; }

        public override StatsFields Stats => CompanionStats;

        public bool IsDisconnected { get; private set; }

        public int? RemainingRounds { get; private set; }



        // METHODS

        public void LeaveFight(bool force = false)
        {
            if (HasLeft())
                return;

            m_left = !force;

            OnLeft();
        }
        private void InitializeCharacterFighter()
        {
            m_damageTakenBeforeFight = Stats.Health.DamageTaken;
            if (Fight.FightType == FightTypeEnum.FIGHT_TYPE_CHALLENGE)
            {
                Stats.Health.DamageTaken = 0;
            }
        }
        
        public CharacterCharacteristicsInformations GetCompanionStatsMessage()
        {
            return new CharacterCharacteristicsInformations((long)Master.Experience,
                (long)Master.LowerBoundExperience,
                (long)Master.UpperBoundExperience,
                0,
                0,
                0,
                0,
                0,
                 Master.GetActorAlignmentExtendInformations(),
                (int)Stats.Health.Total,
                (int)Stats.Health.TotalMax,
                (short)Master.Energy,
                (short)Master.EnergyMax,
                (short)Stats[PlayerFields.AP].Total,
                (short)Stats[PlayerFields.MP].Total,
                Stats[PlayerFields.Initiative],
                Stats[PlayerFields.Prospecting],
                Stats[PlayerFields.AP],
                Stats[PlayerFields.MP],
                Stats[PlayerFields.Strength],
                Stats[PlayerFields.Vitality],
                Stats[PlayerFields.Wisdom],
                Stats[PlayerFields.Chance],
                Stats[PlayerFields.Agility],
                Stats[PlayerFields.Intelligence],
                Stats[PlayerFields.Range],
                Stats[PlayerFields.SummonLimit],
                Stats[PlayerFields.DamageReflection],
                Stats[PlayerFields.CriticalHit],
                0,
                Stats[PlayerFields.CriticalMiss],
                Stats[PlayerFields.HealBonus],
                Stats[PlayerFields.DamageBonus],
                Stats[PlayerFields.WeaponDamageBonus],
                Stats[PlayerFields.DamageBonusPercent],
                Stats[PlayerFields.TrapBonus],
                Stats[PlayerFields.TrapBonusPercent],
                Stats[PlayerFields.GlyphBonusPercent],
                Stats[PlayerFields.RuneBonusPercent],
                Stats[PlayerFields.PermanentDamagePercent],
                Stats[PlayerFields.TackleBlock],
                Stats[PlayerFields.TackleEvade],
                Stats[PlayerFields.APAttack],
                Stats[PlayerFields.MPAttack],
                Stats[PlayerFields.PushDamageBonus],
                Stats[PlayerFields.CriticalDamageBonus],
                Stats[PlayerFields.NeutralDamageBonus],
                Stats[PlayerFields.EarthDamageBonus],
                Stats[PlayerFields.WaterDamageBonus],
                Stats[PlayerFields.AirDamageBonus],
                Stats[PlayerFields.FireDamageBonus],
                Stats[PlayerFields.DodgeAPProbability],
                Stats[PlayerFields.DodgeMPProbability],
                Stats[PlayerFields.NeutralResistPercent],
                Stats[PlayerFields.EarthResistPercent],
                Stats[PlayerFields.WaterResistPercent],
                Stats[PlayerFields.AirResistPercent],
                Stats[PlayerFields.FireResistPercent],
                Stats[PlayerFields.NeutralElementReduction],
                Stats[PlayerFields.EarthElementReduction],
                Stats[PlayerFields.WaterElementReduction],
                Stats[PlayerFields.AirElementReduction],
                Stats[PlayerFields.FireElementReduction],
                Stats[PlayerFields.PushDamageReduction],
                Stats[PlayerFields.CriticalDamageReduction],
                Stats[PlayerFields.PvpNeutralResistPercent],
                Stats[PlayerFields.PvpEarthResistPercent],
                Stats[PlayerFields.PvpWaterResistPercent],
                Stats[PlayerFields.PvpAirResistPercent],
                Stats[PlayerFields.PvpFireResistPercent],
                Stats[PlayerFields.PvpNeutralElementReduction],
                Stats[PlayerFields.PvpEarthElementReduction],
                Stats[PlayerFields.PvpWaterElementReduction],
                Stats[PlayerFields.PvpAirElementReduction],
                Stats[PlayerFields.PvpFireElementReduction],
                Stats[PlayerFields.MeleeDamageDonePercent],
                Stats[PlayerFields.MeleeDamageReceivedPercent],
                Stats[PlayerFields.RangedDamageDonePercent],
                Stats[PlayerFields.RangedDamageReceivedPercent],
                Stats[PlayerFields.WeaponDamageDonePercent],
                Stats[PlayerFields.WeaponDamageReceivedPercent],
                Stats[PlayerFields.SpellDamageDonePercent],
                Stats[PlayerFields.SpellDamageReceivedPercent],
                new List<CharacterSpellModification>().ToArray(), 0);
        }
        public List<SpellItem> GetSpellsItem()
        {
            return CompanionSpell.Select(spell => new SpellItem(spell.Id, (sbyte)spell.CurrentLevel)).ToList();
        }

        public ShortcutSpell[] GetShortcut()
        {
            var shortcutSpell = new ShortcutSpell[CompanionSpell.Count];
            for (var i = 0; i < CompanionSpell.Count; i++)
            {
                shortcutSpell[i] = new ShortcutSpell((sbyte)i, (short)CompanionSpell[i].Id);
            }

            return shortcutSpell;
        }
        public override ObjectPosition GetLeaderBladePosition()
        {
            return GetPositionBeforeMove();
        }
        
        public override bool CastSpell(SpellCastInformations spell)
        {

            bool result;
            if (!IsFighterTurn())
            {
                result = false;
            }
            else
            {
                if (spell.Spell.Id != 0)
                {
                    result = base.CastSpell(spell);
                }
                else
                {
                    return false;
                }
            }
            return result;
        }

        public override SpellCastResult CanCastSpell(SpellCastInformations cast)
        {
            var result = base.CanCastSpell(cast);

            if (result == SpellCastResult.OK || cast.IsConditionBypassed(result))
                return result;

            if (cast.Silent)
                return result;

            switch (result)
            {
                case SpellCastResult.NO_LOS:
                    // Impossible de lancer ce sort : un obstacle gène votre vue !
                    Master.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 174);
                    break;
                case SpellCastResult.HAS_NOT_SPELL:
                    // Impossible de lancer ce sort : vous ne le possédez pas !
                    Master.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 169);
                    break;
                case SpellCastResult.NOT_ENOUGH_AP:
                    // Impossible de lancer ce sort : Vous avez %1 PA disponible(s) et il vous en faut %2 pour ce sort !
                    Master.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 170, AP, cast.SpellLevel.ApCost);
                    break;
                case SpellCastResult.UNWALKABLE_CELL:
                    // Impossible de lancer ce sort : la cellule visée n'est pas disponible !
                    Master.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 172);
                    break;
                case SpellCastResult.CELL_NOT_FREE:
                    //Impossible de lancer ce sort : la cellule visée n'est pas valide !
                    Master.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 193);
                    break;
                default:
                    //Impossible de lancer ce sort actuellement.
                    Master.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 175);
                    break;
            }

            return result;
        }

        public override FightSpellCastCriticalEnum RollCriticalDice(SpellLevelTemplate spell)
            => Master.CriticalMode ? FightSpellCastCriticalEnum.CRITICAL_HIT : base.RollCriticalDice(spell);

        public override FightSpellCastCriticalEnum RollCriticalDice(WeaponTemplate weapon)
            => Master.CriticalMode ? FightSpellCastCriticalEnum.CRITICAL_HIT : base.RollCriticalDice(weapon);

        public override Damage CalculateDamageBonuses(Damage damage)
        {
            if (Master.GodMode)
                damage.Amount = short.MaxValue;

            if (m_isUsingWeapon)
                damage.Amount += m_criticalWeaponBonus;

            return base.CalculateDamageBonuses(damage);
        }

        public bool CanUseWeapon(Cell cell, WeaponTemplate weapon)
        {
            if (!IsFighterTurn())
                return false;

            if (HasState((int)SpellStatesEnum.AFFAIBLI_42))
                return false;

            var point = new MapPoint(cell);

            if ((weapon.CastInDiagonal && (point.EuclideanDistanceTo(Position.Point) > weapon.WeaponRange || point.EuclideanDistanceTo(Position.Point) < weapon.MinRange)) ||
                (!weapon.CastInDiagonal && point.ManhattanDistanceTo(Position.Point) > weapon.WeaponRange || point.ManhattanDistanceTo(Position.Point) < weapon.MinRange))
                return false;

            if (m_weaponUses >= weapon.MaxCastPerTurn)
                return false;

            return AP >= weapon.ApCost && Fight.CanBeSeen(cell, Position.Cell);
        }

        public override Spell GetSpell(int id) => CompanionSpell.FirstOrDefault(x => x.Template.Id == id);

        public override bool HasSpell(int id) => CompanionSpell.Exists(x => x.Template.Id == id);

        
        public override void ResetFightProperties()
        {
            base.ResetFightProperties();

            if (Fight.IsDeathTemporarily)
                Stats.Health.DamageTaken = m_damageTakenBeforeFight;
            else if (Stats.Health.Total <= 0)
                Stats.Health.DamageTaken = (Stats.Health.TotalMax - 1);
        }

        public override bool MustSkipTurn()
            => base.MustSkipTurn() || (IsDisconnected && Team.GetAllFighters<CharacterFighter>().Any(x => x.IsAlive() && !x.IsDisconnected));

        

        public override IFightResult GetFightResult(FightOutcomeEnum outcome) => new FightResult(this, outcome, Loot);

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
            => new FightTeamMemberCompanionInformations(Id, (sbyte)CompanionId, (sbyte)Level, Master.Id);

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightCompanionInformations(Id,
                                                      Look.GetEntityLook(),
                                                      GetEntityDispositionInformations(client),
                                                      (sbyte)Team.Id,
                                                      0,
                                                      IsAlive(),
                                                      GetGameFightMinimalStats(client),
                                                      new short[0],
                                                      (sbyte)CompanionId, (sbyte)Level, Master.Id);
        }

        public override GameFightFighterLightInformations GetGameFightFighterLightInformations(WorldClient client = null)
            => new GameFightFighterCompanionLightInformations(
                Master.Sex == SexTypeEnum.SEX_FEMALE,
                IsAlive(),
                Id,
                0,
                Level,
                (sbyte)Master.Breed.Id,
                (sbyte)CompanionId,
                Master.Id);

        public override string ToString()
        {
            return Master.ToString();
        }

        #region God state
        public override bool UseAP(short amount)
        {
            if (!Master.GodMode)
                return base.UseAP(amount);

            base.UseAP(amount);
            RegainAP(amount);

            return true;
        }

        public override bool UseMP(short amount)
        {
            return Master.GodMode || base.UseMP(amount);
        }

        public override bool LostAP(short amount, FightActor source)
        {
            if (!Master.GodMode)
                return base.LostAP(amount, source);

            base.LostAP(amount, source);
            RegainAP(amount);

            return true;
        }

        public override bool LostMP(short amount, FightActor source)
        {
            return Master.GodMode || base.LostMP(amount, source);
        }


        public override int InflictDamage(Damage damage)
        {
            if (!Master.GodMode)
                return base.InflictDamage(damage);

            damage.GenerateDamages();
            OnBeforeDamageInflicted(damage);

            damage.Source.TriggerBuffs(damage.Source, BuffTriggerType.BeforeAttack, damage);
            TriggerBuffs(damage.Source, BuffTriggerType.BeforeDamaged, damage);
            TriggerBuffs(damage.Source, BuffTriggerType.OnDamaged, damage);

            OnDamageReducted(damage.Source, damage.Amount);

            damage.Source.TriggerBuffs(damage.Source, BuffTriggerType.AfterAttack, damage);
            TriggerBuffs(damage.Source, BuffTriggerType.AfterDamaged, damage);

            OnDamageInflicted(damage);

            return 0;
        }
        public PartyCompanionMemberInformations GetPartyCompanionMemberInformations()
        {
            return new PartyCompanionMemberInformations((sbyte)FighterId, (sbyte)CompanionId, Look.GetEntityLook(),
                (short)Stats[PlayerFields.Initiative].Total, (int)LifePoints, (int)MaxLifePoints,
                (short)Stats[PlayerFields.Prospecting].Total, (sbyte)Master.RegenSpeed);
        }
        #endregion
    }
}