//using Accord.Statistics.Distributions.Univariate;
using Accord.Statistics.Distributions.Univariate;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Maps.Cells;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
//By Geraxi
namespace Stump.Server.WorldServer.Commands.Commands.Teleport
{
   
    public class Random : InGameCommand
    {
        #region Random
        
        #endregion
        public Random()
        {
            Aliases = new[] { "rand" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Randomize a number with binomial law";
        }

        public override void Execute(GameTrigger trigger)
        {
            // Creates a distribution with n = 16 and success probability 0.12
            var bin = new BinomialDistribution(trials: 10, probability: 0.02);

            // Probability mass functions
            double pdf = bin.ProbabilityMassFunction(k: 1); // 0.28218979948821621

            double chf = bin.CumulativeHazardFunction(x: 0); // 0.86750056770472328
            double result = pdf + chf;
            // String representation
            string str = bin.ToString(CultureInfo.InvariantCulture); // "Binomial(x; n = 16, p = 0.12)"

            trigger.Character.SendServerMessage($" failure rate double : {chf}");
            trigger.Character.SendServerMessage($" success rate : {pdf}");
            trigger.Character.SendServerMessage($" result : {1 - result}");
        }


    }
	
    class Exo_command : InGameCommand
   {
        private const int SHOP_POINT = 100;
        public EffectsEnum[] effectAP = { EffectsEnum.Effect_AddAP_111 };
        public EffectsEnum[] effectMP = { EffectsEnum.Effect_AddMP_128, EffectsEnum.Effect_AddMP };
        public EffectsEnum[] effectPO = { EffectsEnum.Effect_AddRange, EffectsEnum.Effect_AddRange_136 };
        public EffectsEnum[] effectInvoc = { EffectsEnum.Effect_AddSummonLimit };
        public Exo_command()
        {
            Aliases = new string[] { "exostaff" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Exo PA, PM, PO ou Invoc pour 100 PB => Utilisation : .exo [pa|pm|po|invoc] [cape|coiffe|amulette|anneau|botte|ceinture]";
            AddParameter<string>("type", "type", "type", null, true);
            AddParameter<string>("choice", "effet", "effet", null, true);
        }
        public override void Execute(GameTrigger trigger)
        {
            string str1 = (string)trigger.Get<string>("choice");
            string str2 = (string)trigger.Get<string>("type");
            BasePlayerItem jetons = Enumerable.FirstOrDefault<BasePlayerItem>(trigger.Character.Inventory.GetItems((CharacterInventoryPositionEnum)63), (x => x.Template.Id == Inventory.TokenTemplateId));
            bool flag = false;
            if (jetons == null)
                trigger.ReplyError("Vous ne posséder pas de points boutique.");
            else if (jetons.Stack < SHOP_POINT)
            {
                trigger.ReplyError($"Vous n'avez pas <b>{SHOP_POINT}</b> points boutique.");
            }
            else
            {
                EffectsEnum effectsEnum = EffectsEnum.Effect_AddAP_111;
                EffectsEnum effecttype;
                if (str2.ToLower() == "pa")
                    effecttype = EffectsEnum.Effect_AddAP_111;
                else if (str2.ToLower() == "pm")
                    effecttype = EffectsEnum.Effect_AddMP_128;
                else if (str2.ToLower() == "po")
                {
                    effecttype = EffectsEnum.Effect_AddRange;
                }
                else if (str2.ToLower() == "invoc")
                {
                    effecttype = EffectsEnum.Effect_AddSummonLimit;
                }
                else
                {
                    trigger.Character.SendServerMessage("Impossible de trouver l'element à fm : " + (object)effectsEnum);
                    goto label_20;
                }
                switch (str1)
                {
                    case "cape":
                        flag = this.ApplyMagicalCraft(trigger.Character, effecttype, str2.ToUpper(), CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE);
                        break;
                    case "coiffe":
                        flag = this.ApplyMagicalCraft(trigger.Character, effecttype, str2.ToUpper(), CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT);
                        break;
                    case "amulette":
                        flag = this.ApplyMagicalCraft(trigger.Character, effecttype, str2.ToUpper(), CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET);
                        break;
                    case "anneau":
                        flag = this.ApplyMagicalCraft(trigger.Character, effecttype, str2.ToUpper(), CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT);
                        break;
                    case "botte":
                        flag = this.ApplyMagicalCraft(trigger.Character, effecttype, str2.ToUpper(), CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS);
                        break;
                    case "ceinture":
                        flag = this.ApplyMagicalCraft(trigger.Character, effecttype, str2.ToUpper(), CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT);
                        break;
                    default:
                        trigger.Character.SendServerMessage("type incorrect");
                        break;
                }
            }
            label_20:
            if (!flag)
                return;
            trigger.Character.SendServerMessage("L'équipement souhaité à été forgemagé  + <b>1" + str2.ToUpper() + "</b> Veuillez le ré-équiper afin de ne pas perdre l'équipement forgemagé");
            trigger.Character.SendServerMessage($"<b>{SHOP_POINT}</b> Ppoints boutique vous ont été retiré.");
            trigger.Character.SaveLater();
        }

        private bool ApplyMagicalCraft(Character character, EffectsEnum effecttype, string EffectName, CharacterInventoryPositionEnum position)
        {
            BasePlayerItem[] items = character.Inventory.GetItems(position);
            Enumerable.ToArray<BasePlayerItem>(Enumerable.Where<BasePlayerItem>(character.Inventory.GetItems((CharacterInventoryPositionEnum)63), (entry => entry.Template.Id == Inventory.TokenTemplateId)));
            bool flag = false;
            EffectInteger effectInteger1 = new EffectInteger(effecttype, (short)1);

            foreach (BasePlayerItem basePlayerItem in items)
            {

                if (basePlayerItem.Position == position && basePlayerItem.IsEquiped())
                {
                    bool result = true;
                    switch (effecttype)
                    {
                        case EffectsEnum.Effect_AddAP_111:
                            foreach (EffectsEnum effect in effectAP)
                            {
                                if (basePlayerItem.Effects.Exists(x => x.EffectId == effect))
                                {
                                    result = false;
                                    break;
                                }
                            }
                            break;
                        case EffectsEnum.Effect_AddMP_128:
                            foreach (EffectsEnum effect in effectMP)
                            {
                                if (basePlayerItem.Effects.Exists(x => x.EffectId == effect))
                                {
                                    result = false;
                                    break;
                                }
                            }
                            break;
                        case EffectsEnum.Effect_AddRange:
                            foreach (EffectsEnum effect in effectPO)
                            {
                                if (basePlayerItem.Effects.Exists(x => x.EffectId == effect))
                                {
                                    result = false;
                                    break;
                                }
                            }
                            break;
                        case EffectsEnum.Effect_AddSummonLimit:
                            foreach (EffectsEnum effect in effectInvoc)
                            {
                                if (basePlayerItem.Effects.Exists(x => x.EffectId == effect))
                                {
                                    result = false;
                                    break;
                                }
                            }
                            break;
                    }
                    if (!result)
                    {
                        character.OpenPopup("L'équipement possède déjà cette effet !");

                        break;
                    }
                    character.Inventory.UnStackItem(character.Inventory.GetItems(CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED).First(x => x.Template.Id == Inventory.TokenTemplateId), SHOP_POINT);
                    EffectInteger effectInteger2 = new EffectInteger(effecttype, (short)1);
                    basePlayerItem.Effects.Add((EffectBase)effectInteger2);
                    character.Client.Send(new ExchangeCraftInformationObjectMessage((sbyte)2, (short)basePlayerItem.Template.Id, (long)character.Id));
                    character.Inventory.RefreshItem(basePlayerItem);
                    Singleton<EffectManager>.Instance.GetItemEffectHandler((EffectBase)effectInteger2, character, null, true).Apply();
                    character.RefreshStats();
                    character.SaveLater();
                    flag = true;
                    BasePlayerItem tokens = character.Inventory.GetItems(CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED).FirstOrDefault(x => x.Template.Id == 12124);
                    if (tokens != null)
                    {
                        if (IPCAccessor.Instance.IsConnected)
                        {
                            character.Client.Account.Tokens = (int)tokens.Stack;
                            IPCAccessor.Instance.Send(new UpdateTokensMessage(character.Client.Account.Tokens, character.Client.Account.Id));
                        }
                    }
                    else
                    {
                        if (IPCAccessor.Instance.IsConnected)
                        {
                            character.Client.Account.Tokens = 0;
                            IPCAccessor.Instance.Send(new UpdateTokensMessage(character.Client.Account.Tokens, character.Client.Account.Id));
                        }

                    }
                }
            }
            return flag;
        }
        public class fmcac : InGameCommand
        {
            private const int SHOP_POINTS = 50;

            public fmcac()
            {

                Aliases = new[] { "fmcac", };
                RequiredRole = RoleEnum.Player;
                Description = "FM un Cac équipé exemple : .fm eau|terre|feu|air à 50 Points boutique";
                AddParameter<string>("choice", "strenght", "FMterre", null, true, null);

            }



            public override void Execute(GameTrigger trigger)
            {

                var trigger_ = trigger.Get<string>("choice");
                if (trigger_ == "terre")
                    ApplyMagicalCraft(trigger.Character, EffectsEnum.Effect_DamageEarth, "Terre");
                if (trigger_ == "feu")
                    ApplyMagicalCraft(trigger.Character, EffectsEnum.Effect_DamageFire, "Feu");
                if (trigger_ == "air")
                    ApplyMagicalCraft(trigger.Character, EffectsEnum.Effect_DamageAir, "Air");
                if (trigger_ == "eau")
                    ApplyMagicalCraft(trigger.Character, EffectsEnum.Effect_DamageWater, "Eau");


            }
            private void ApplyMagicalCraft(Character character, EffectsEnum effecttype, string EffectName)
            {
                var items_ = character.Inventory.GetItems(Stump.DofusProtocol.Enums.CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
                if (items_.Count() > 0 && items_.Any(x => x.Template.Id == 12124) && items_.First(x => x.Template.Id == 12124).Stack >= SHOP_POINTS)
                {
                    var items = character.Inventory.GetEquipedItems();
                    foreach (var item in items)
                    {
                        if (item.Position == Stump.DofusProtocol.Enums.CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON && item.IsEquiped())
                        {
                            if (item.Effects.Exists(x => x.EffectId == EffectsEnum.Effect_DamageNeutral))
                            {

                                var effect = item.Effects.Find(x => x.EffectId == EffectsEnum.Effect_DamageNeutral);
                                item.Effects.Remove(effect);
                                var effect_ = (EffectDice)effect;
                                item.Effects.Add(new EffectDice((short)effecttype, 0, effect_.DiceNum, effect_.DiceFace, new EffectBase()));
                                character.Client.Send(new ExchangeCraftInformationObjectMessage(1, (short)item.Template.Id, character.Id));
                                character.SendServerMessage("L'item a été FM <b> " + EffectName + " <b>");
                                character.Inventory.UnStackItem(character.Inventory.GetItems(CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED).First(x => x.Template.Id == 12124), SHOP_POINTS);
                                character.Inventory.RefreshItem(item);
                                character.Inventory.RefreshItemInstance(item);
                                character.RefreshActor();
                                character.SaveLater();
                                BasePlayerItem tokens = character.Inventory.GetItems(CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED).FirstOrDefault(x => x.Template.Id == 12124);
                                if (tokens != null)
                                {
                                    if (IPCAccessor.Instance.IsConnected)
                                    {
                                        character.Client.Account.Tokens = (int)tokens.Stack;
                                        IPCAccessor.Instance.Send(new UpdateTokensMessage(character.Client.Account.Tokens, character.Client.Account.Id));
                                    }
                                }
                                else
                                {
                                    if (IPCAccessor.Instance.IsConnected)
                                    {
                                        character.Client.Account.Tokens = 0;
                                        IPCAccessor.Instance.Send(new UpdateTokensMessage(character.Client.Account.Tokens, character.Client.Account.Id));
                                    }

                                }
                            }
                            else
                            {

                                //character.Client.Send(new ExchangeStartOkMulticraftCrafterMessage(1, 2));
                                character.SendServerMessage("L'item possede deja un FM", System.Drawing.Color.Red);
                            }
                        }
                        else
                        {
                            character.SendServerMessage("Vous ne possedez aucune arme équipée");
                        }

                    }
                }
                else
                {
                    character.SendServerMessage("Vous ne possedez pas assez de points boutiques et cet fm coûte" + SHOP_POINTS + ".");
                }
            }
        }
        public class LevelUpCommand : TargetCommand
        {
            public LevelUpCommand()
            {
                Aliases = new[] { "addlevel" };
                RequiredRole = RoleEnum.GameMaster;
                Description = "Gives some levels to the target";
                AddParameter("amount", "amount", "Amount of levels to add", (short)1);
            }

            public override void Execute(TriggerBase trigger)
            {
                foreach (var target in GetTargets(trigger))
                {
                    byte delta;

                    var amount = trigger.Get<short>("amount");
                    if (amount > 0 && amount <= byte.MaxValue)
                    {
                        delta = (byte)(amount);
                        target.LevelUp(delta);
                        trigger.Reply("Vous vous êtes ajouté " + trigger.Bold("{0}") + " niveaux !", delta, target.Name);

                    }
                    else if (amount < 0 && -amount <= byte.MaxValue)
                    {
                        trigger.ReplyError("Impossible de perdre des levels.");

                    }

                }
            }

        }
    }
	
}
