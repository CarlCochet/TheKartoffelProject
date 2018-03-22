using System.Linq;
using WorldEditor.Loaders.Classes;
using WorldEditor.Loaders.Data;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using System;
using System.Collections.Generic;

namespace WorldEditor.Loaders.Objetcs
{
    [D2OClass("Item", "com.ankamagames.dofus.datacenter.items", true), TableName("items_templates_weapons")]
    public class WeaponTemplate : ItemTemplate, IObject
    {
        public int ApCost { get; set; }

        public int MinRange { get; set; }

        public int Range { get; set; }

        public int WeaponRange { get; set; }

        public uint MaxCastPerTurn { get; set; }

        public bool CastInLine { get; set; }

        public bool CastInDiagonal { get; set; }

        public bool CastTestLos { get; set; }

        public int CriticalHitProbability { get; set; }

        public int CriticalHitBonus { get; set; }

        public int CriticalFailureProbability { get; set; }

        public new object GetD2oClass(object baseObj)
        {
            var result = new Weapon();

            var weapon = baseObj as Weapon;

            //item
            result.id = this.Id;
            result.nameId = this.NameId;
            result.typeId = this.TypeId;
            result.descriptionId = this.DescriptionId;
            result.iconId = this.IconId;
            result.level = this.Level;
            result.realWeight = this.RealWeight;
            result.cursed = this.Cursed;
            result.useAnimationId = this.UseAnimationId;
            result.usable = this.Usable;
            result.targetable = this.Targetable;
            if (weapon != null)
            {
                result.exchangeable = weapon.exchangeable;
            }
            else
            {
                result.exchangeable = true;
            }
            result.price = (float)this.Price;
            result.twoHanded = this.TwoHanded;
            result.etheral = this.Etheral;
            result.itemSetId = this.ItemSetId;
            result.criteria = this.Criteria;
            result.criteriaTarget = this.CriteriaTarget;
            result.hideEffects = this.HideEffects;
            if (weapon != null)
            {
                result.enhanceable = weapon.enhanceable;
            }
            else
            {
                result.enhanceable = true;
            }

            if (weapon != null)
            {
                result.nonUsableOnAnother = weapon.nonUsableOnAnother;
            }
            else
            {
                result.nonUsableOnAnother = true;
            }

            //result.appearanceId = this.AppearanceId;

            if (weapon != null)
            {
                result.secretRecipe = weapon.secretRecipe;
            }
            else
            {
                result.secretRecipe = true;
            }

            if (weapon != null)
            {
                result.dropMonsterIds = weapon.dropMonsterIds;
            }
            else
            {
                result.dropMonsterIds = new List<uint>();
            }

            if (weapon != null)
            {
                result.recipeSlots = weapon.recipeSlots;
            }
            else
            {
                result.recipeSlots = 0;
            }
            result.recipeIds = this.RecipeIds.ToList();
            result.bonusIsSecret = this.BonusIsSecret;
            result.possibleEffects = this.PossibleEffects;
            result.favoriteSubAreas = this.FavoriteSubAreas.ToList();
            result.favoriteSubAreasBonus = this.FavoriteSubAreasBonus;
            result.weight = this.Weight;
            //weapon
            result.apCost = this.ApCost;
            result.minRange = this.MinRange;
            result.range = this.Range;
            result.maxCastPerTurn = this.MaxCastPerTurn;
            result.castInLine = this.CastInLine;
            result.castInDiagonal = this.CastInDiagonal;
            result.castTestLos = this.CastTestLos;
            result.criticalHitProbability = this.CriticalHitProbability;
            result.criticalHitBonus = this.CriticalHitBonus;
            result.criticalFailureProbability = this.CriticalFailureProbability;

            return result;
        }
    }

    public class WeaponTemplateRelator
    {
        public static string FetchQuery = "SELECT * FROM items_templates_weapons";
        public static string FetchById = "SELECT * FROM items_templates_weapons WHERE Id = @0";
        public static string FetchByTypeId = "SELECT * FROM items_templates_weapons WHERE TypeId = @0";
    }
}