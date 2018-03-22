using Stump.Core.Reflection;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Handlers.Items;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Formulas;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Jobs;
using Stump.Server.WorldServer.Handlers.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stump.Server.WorldServer.Game.Dialogs.Interactives.Magus
{
    public class MagusDialog : IDialogExchange
    {
        #region Fields

        //Effects
        public static int[] ID_EFECTOS_ARMAS = { 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 108 };
        private static int RUNE_FIRM = 7508;
        private bool _interrupt;
        private int _count;
        private bool _allItems;

        #endregion Fields

        #region Constructors

        public MagusDialog(Character character, int skillId)
        {
            Character = character;
            var requieredJob =
                    JobManager.Instance.GetJobTemplateBySkillId((short)skillId);
            Job = character.Jobs.GetJob(requieredJob.Id);
            SkillId = (uint)skillId;
        }

        #endregion Constructors

        #region Properties

        public Character Character { get; set; }
        public Job Job { get; set; }

        public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_EXCHANGE;

        public uint SkillId { get; private set; }
        public BasePlayerItem ItemToMagus { get; set; }
        public BasePlayerItem RuneOrPotion { get; set; }
        public BasePlayerItem AdditionalRune { get; set; }
        public bool IsRunning { get; private set; }

        #endregion Properties

        #region Methods

        public static PlayerFields GetRuneStat(EffectsEnum effect)
        {
            PlayerFields result;
            var fields = DefaultItemEffect.m_addEffectsBinds;
            fields.TryGetValue(effect, out result);
            return result;
        }

        public static int calculChanceByElement(int lvlJob, int lvlObject,
                                            int lvlRune)
        {
            var K = 1;
            if (lvlRune == 20)
                K = 100;
            else if (lvlRune == 40)
                K = 175;
            else if (lvlRune == 80)
                K = 350;
            return lvlJob * 100 / (K + lvlObject);
        }

        public void Open()
        {
            ItemToMagus = null;
            RuneOrPotion = null;
            AdditionalRune = null;
            IsRunning = false;
            Character.SetDialog(this);
            InventoryHandler.SendExchangeStartOkCraftWithInformationMessage(Character.Client, SkillId);
        }

        public void Close()
        {
            if (ItemToMagus != null && Character.Inventory.HasItem(ItemToMagus))
            {
                BasePlayerItem item;
                if (Character.Inventory.IsStackable(ItemToMagus, out item) && item != null)
                {
                    Character.Inventory.StackItem(item, ItemToMagus.Stack);
                    Character.Inventory.RemoveItem(ItemToMagus);
                }
            }
            ItemToMagus = null;
            RuneOrPotion = null;
            AdditionalRune = null;
            IsRunning = false;
            Character.CloseDialog(this);
            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, true);
        }

        public void Magus(BasePlayerItem item, BasePlayerItem runeOrPotion, bool firm)
        {
            var lvlElementRune = 0;
            var statAgre = -1;
            var lvlQuaStatsRune = 0;
            var poid = 0;
            var effectToMagus = runeOrPotion.Effects.FirstOrDefault().EffectId;
            var statToMagus = GetRuneStat(effectToMagus); //all runes just have 1 effect
            var templateId = runeOrPotion.Template.Id;
            var objectLevel = (int)runeOrPotion.Template.Level;
            var value = runeOrPotion.Effects.OfType<EffectInteger>().FirstOrDefault().Value;
            var isOrb = false;
            switch (templateId)
            {
                case 1333:// pocion chispa
                    statAgre = 99;
                    lvlElementRune = objectLevel;
                    break;

                case 1335:// pocion llovisna
                    statAgre = 96;
                    lvlElementRune = objectLevel;
                    break;

                case 1337:// pocion de corriente de airee
                    statAgre = 98;
                    lvlElementRune = objectLevel;
                    break;

                case 1338:// pocion de sacudida
                    statAgre = 97;
                    lvlElementRune = objectLevel;
                    break;

                case 1340:// pocion derrumbamiento
                    statAgre = 97;
                    lvlElementRune = objectLevel;
                    break;

                case 1341:// pocion chaparron
                    statAgre = 96;
                    lvlElementRune = objectLevel;
                    break;

                case 1342:// pocion de rafaga
                    statAgre = 98;
                    lvlElementRune = objectLevel;
                    break;

                case 1343:// pocion de Flameacion
                    statAgre = 99;
                    lvlElementRune = objectLevel;
                    break;

                case 1345:// pocion Incendio
                    statAgre = 99;
                    lvlElementRune = objectLevel;
                    break;

                case 1346:// pocion Tsunami
                    statAgre = 96;
                    lvlElementRune = objectLevel;
                    break;

                case 1347:// pocion huracan
                    statAgre = 98;
                    lvlElementRune = objectLevel;
                    break;

                case 1348:// pocion de seismo
                    statAgre = 97;
                    lvlElementRune = objectLevel;
                    break;

                case 17275: //Orbe Magistral
                    isOrb = true;
                    break;

                case 17274: //Orbe Mayor
                    isOrb = true;
                    break;

                case 17273: //Orbe
                    isOrb = true;
                    break;

                case 17272: //Orbe Menor
                    isOrb = true;
                    break;

                default:
                    lvlQuaStatsRune = objectLevel;
                    poid = (int)(value * AdditionalFormulas.GetRunePower(templateId));
                    break;
            }
            //Add xp
            var objModelo = item;
            var objTemplate = item.Template;
            var chance = 0;
            var lvlJob = Job.Level;
            var currentWeightTotal = 1;
            var pwrPerte = 0;
            var chances = new List<int>();
            var objTemplateID = item.Template.Id;
            //here a problem, we need the min and max element but EffectInteger just have value
            var stats = objModelo.Effects;
            if (isOrb)
            {
                if (runeOrPotion.Template.Level > lvlJob || item.Template.Level > runeOrPotion.Template.Level)
                {
                    InventoryHandler.SendExchangeObjectRemovedMessage(Character.Client, false, runeOrPotion.Guid);
                    return;
                }
                else
                {
                    if (CheckEffects(item))
                    {
                        //delete rune
                        RemoveItem(RuneOrPotion, 1);
                        //final
                        var effects = Singleton<ItemManager>.Instance.GenerateItemEffects(item.Template, false);
                        item.SetEffects(effects);
                        var container = item.GetObjectItem();
                        using (var exchangeCraftResultMagicWithObjectDescMessage = new ExchangeCraftResultMagicWithObjectDescMessage(
                            (sbyte)CraftResultEnum.CRAFT_SUCCESS, new ObjectItemNotInContainer(container.objectGID,
                            container.effects, container.objectUID, 1), 1))
                        {
                            Character.Client.Send(exchangeCraftResultMagicWithObjectDescMessage);
                        }
                        using (var exchangeCraftInformationObjectMessage = new ExchangeCraftInformationObjectMessage((sbyte)CraftResultEnum.CRAFT_SUCCESS,
                            (ushort)item.Template.Id, (uint)Character.Id))
                        {
                            Character.Map.ForEach(x => x.Client.Send(exchangeCraftInformationObjectMessage));
                        }
                        Character.Inventory.RefreshItem(item);
                        Character.RefreshActor();
                    }
                    else
                    {
                        //final
                        var container = item.GetObjectItem();
                        using (var exchangeCraftResultMagicWithObjectDescMessage = new ExchangeCraftResultMagicWithObjectDescMessage(
                            (sbyte)CraftResultEnum.CRAFT_IMPOSSIBLE, new ObjectItemNotInContainer(container.objectGID,
                            container.effects, container.objectUID, 1), 1))
                        {
                            Character.Client.Send(exchangeCraftResultMagicWithObjectDescMessage);
                        }
                    }

                }
                return;
            }
            if (lvlElementRune > 0 && lvlQuaStatsRune == 0) //change element
            {
                chance = calculChanceByElement(lvlJob, (int)objTemplate.Level, lvlElementRune);
                if (chance > 100 - (lvlJob / 20.0))
                    chance = (int)(100 - (lvlJob / 20.0));
                if (chance < (lvlJob / 20.0))
                    chance = (int)(lvlJob / 20.0);
                chances.Add(chance);
                chances.Add(0);
                chances.Add(100 - chance);
            }
            else if (lvlQuaStatsRune > 0 && lvlElementRune == 0)
            {// si cambia de stats
                var currentWeightStats = 1;
                if (stats.Count > 0)
                {
                    currentWeightTotal = currentTotalWeigthBase(item.Effects, item); // Poids total de l'objet : PWRg | changed stats to item.Effects
                    currentWeightStats = currentWeithStats(item, effectToMagus); // Poids � ajouter : PWRcarac
                }
                var currentTotalBase = WeithTotalBase(item.Template); // Poids maximum de l'objet : PWRmax
                var currentMinBase = WeithTotalBaseMin(item.Template);
                if (currentTotalBase < 0)
                    currentTotalBase = 0;
                if (currentWeightStats < 0)
                    currentWeightStats = 0;
                if (currentWeightTotal < 0)
                    currentWeightTotal = 0;
                var coef = 1.0;
                var baseStats = viewBaseStatsItem(item, (int)effectToMagus);
                var currentStats = viewActualStatsItem(item, (int)effectToMagus);
                if (baseStats == 1 && currentStats == 1
                    || baseStats == 1 && currentStats == 0)
                    coef = 1.0;
                else if (baseStats == 2 && currentStats == 2)
                    coef = 0.50;
                else if (baseStats == 0 && currentStats == 0
                    || baseStats == 0 && currentStats == 1)
                    coef = 0.25;

                var x = 1.0;
                var canFM = true;
                var statMax = getStatBaseMaxs(objTemplate, (int)effectToMagus); //max stat to magus value from object
                var actuelJet = getActualJet(item, (int)effectToMagus); //The actual value to the stat to magus
                if (actuelJet >= statMax) //Changued to >=
                {
                    x = 0.8;
                    var overPerEffect = AdditionalFormulas.getOverPerEffet((int)effectToMagus);
                    //if (statMax == 0)
                    if (actuelJet >= (statMax + overPerEffect + 1))
                        canFM = false;
                    else if (actuelJet >= (statMax + overPerEffect))
                        canFM = false;
                }
                if (lvlJob < objTemplate.Level)
                    canFM = false;

                var diff = (int)Math.Abs((currentTotalBase * 1.3)
                    - currentWeightTotal); //The differency the max gap - the actual object gap
                if (canFM)
                {
                    if (objTemplate.Id == 2469 && (poid == 100 || poid == 90) && actuelJet == 0) // Si c'est un gelano et que l'on essaie de remettre le PA
                    {
                        chances.Add(49);
                        chances.Add(18);
                    }
                    else
                    {
                        //here we get if success, neutral etc
                        var baseMin = getStatBaseMins(objTemplate, (int)effectToMagus); //The min possible stat of actual item template
                        var currentStatItem = currentStat(item, (int)effectToMagus); //Actual value of the effect to magus in item
                        //Console.WriteLine($"MaxGap : {currentTotalBase} | MinGap : {currentMinBase} | Current Gap : {currentWeightTotal} | Current stat gap : {currentWeightStats} | Poid {poid} | Diff {diff} | Coef {coef} | StatMax {statMax} | BaseMin {baseMin} | CurrentStatItem { currentStatItem} | X : {x} | Value : {value}");
                        chances = chanceFM(currentTotalBase, currentMinBase, currentWeightTotal, currentWeightStats, poid, diff, coef, statMax, baseMin, currentStatItem, x, false, value);
                    }
                }
                else
                // Si l'objet est au dessus de l'over (impossible statistiquement ... mais evite un gelano 2 PA :p)
                {
                    chances.Add(0);
                    chances.Add(0);
                }
            }

            foreach (var chan in chances)
            {
                Character.SendServerMessage($"The actual chance is {chan}");
            }

            var aleatoryChance = new AsyncRandom().Next(1, 100);
            var SC = chances[0];
            var SN = chances[1];
            var successC = (aleatoryChance <= SC);
            var successN = (aleatoryChance <= (SC + SN));

            if (successC || successN)
            {
                var winXP = calculXpWinFm(objTemplate.Level, poid)
                        * 1; //rate job
                //Console.WriteLine($"Win xp {winXP}");
                if (winXP > 0)
                {
                    Character.Jobs.AddExperience(Character, Job.Template.Id, (uint)winXP);
                }
            }

            if (successC)
            {
                var coef = 0;
                if (lvlElementRune == 20)
                    coef = 50;
                else if (lvlElementRune == 40)
                    coef = 65;
                else if (lvlElementRune == 80)
                    coef = 85;
                if (firm)
                    item.Effects.Add(new EffectString(985, Character.Name, new EffectBase()));
                if (lvlElementRune > 0 && lvlQuaStatsRune == 0)
                {
                    foreach (var effect in item.Effects.OfType<EffectDice>().ToList())
                    {
                        if (effect.EffectId != EffectsEnum.Effect_DamageNeutral)
                            continue;
                        var min = effect.DiceNum;
                        var max = effect.DiceFace;
                        var nuevoMin = ((min * coef) / 100.0);
                        var nuevoMax = ((max * coef) / 100.0);
                        if (nuevoMin == 0)
                            nuevoMin = 1;
                        item.Effects.Remove(effect);
                        item.Effects.Add(new EffectDice((short)statAgre, effect.Value, (short)nuevoMin, (short)nuevoMax, new EffectBase()));
                    }
                }
                else if (lvlQuaStatsRune > 0 && lvlElementRune == 0)
                {
                    var negative = false;
                    var actualStat = viewActualStatsItem(item, (int)effectToMagus);
                    var effect = (int)effectToMagus;
                    var statAMaguear = effect;
                    if (actualStat == 2) // los stats existentes actual son negativos
                    {
                        if (effect == 0x7b)
                            statAMaguear = 0x98;
                        if (effect == 0x77)
                            statAMaguear = 0x9a;
                        if (effect == 0x7e)
                            statAMaguear = 0x9b;
                        if (effect == 0x76)
                            statAMaguear = 0x9d;
                        if (effect == 0x7c)
                            statAMaguear = 0x9c;
                        if (effect == 0x7d)
                            statAMaguear = 0x99;
                        negative = true;
                    }
                    if (actualStat == 1 || actualStat == 2)
                    {
                        if (item.Effects.Count == 0)
                        {
                            var effectToAdd = new EffectInteger((short)statAMaguear, value, new EffectBase());
                            item.Effects.Add(effectToAdd);
                        }
                        else
                        {
                            var finalStats = convertirStatsAStringFM(statAMaguear, item, value, negative);
                            item.SetEffect(finalStats);
                        }
                    }
                    else
                    {
                        if (item.Effects.Count == 0)
                        {
                            item.Effects.Add(new EffectInteger((short)statAMaguear, value, new EffectBase()));
                        }
                        else
                        {
                            var finalStats = convertirStatsAStringFM(statAMaguear, item, value, negative);
                            finalStats.Add(new EffectInteger((short)statAMaguear, value, new EffectBase()));
                            item.SetEffect(finalStats);
                        }
                    }
                }
                //delete firm rune
                if (firm)
                    RemoveItem(AdditionalRune, 1);
                //delete rune
                RemoveItem(RuneOrPotion, 1);
                //final
                var finalEffects = item.Effects.ToList();
                item.SetEffects(finalEffects);
                var container = item.GetObjectItem();
                using (var exchangeCraftResultMagicWithObjectDescMessage = new ExchangeCraftResultMagicWithObjectDescMessage(
                    (sbyte)CraftResultEnum.CRAFT_SUCCESS, new ObjectItemNotInContainer(container.objectGID,
                    container.effects, container.objectUID, 1), 1))
                {
                    Character.Client.Send(exchangeCraftResultMagicWithObjectDescMessage);
                }
                using (var exchangeCraftInformationObjectMessage = new ExchangeCraftInformationObjectMessage((sbyte)CraftResultEnum.CRAFT_SUCCESS,
                    (ushort)item.Template.Id, (uint)Character.Id))
                {
                    Character.Map.ForEach(x => x.Client.Send(exchangeCraftInformationObjectMessage));
                }
                Character.Inventory.RefreshItem(item);
                Character.RefreshActor();
            }
            else if (successN)
            {
                pwrPerte = 0;
                if (firm)
                    item.Effects.Add(new EffectString(985, Character.Name, new EffectBase()));
                var negative = false;
                var currentStats = viewActualStatsItem(item, (int)effectToMagus);
                var effect = (int)effectToMagus;
                var statAMaguear = effect;
                if (currentStats == 2) // los stats existentes actual son negativos
                {
                    if (effect == 0x7b)
                        statAMaguear = 0x98;
                    if (effect == 0x77)
                        statAMaguear = 0x9a;
                    if (effect == 0x7e)
                        statAMaguear = 0x9b;
                    if (effect == 0x76)
                        statAMaguear = 0x9d;
                    if (effect == 0x7c)
                        statAMaguear = 0x9c;
                    if (effect == 0x7d)
                        statAMaguear = 0x99;
                    negative = true;
                }
                if (currentStats == 1 || currentStats == 2)
                {
                    if (item.Effects.Count == 0)
                    {
                        var effectToAdd = new EffectInteger((short)statAMaguear, value, new EffectBase());
                        item.Effects.Add(effectToAdd);
                    }
                    else
                    {
                        var finalStats = new List<EffectBase>();
                        //HERE
                        if (item.Gap <= 0) // EC en premier s'il n'y a pas de puits
                        {
                            finalStats = parseStringStatsEC_FM(item, poid, statAMaguear);
                            item.SetEffect(finalStats);
                            pwrPerte = currentWeightTotal -
                                currentTotalWeigthBase(finalStats, item); //OMG this is sooo wrong
                        }
                        finalStats = convertirStatsAStringFM(statAMaguear, item, value, negative);
                        item.SetEffect(finalStats);
                    }
                }
                else
                {
                    if (item.Effects.Count == 0)
                    {
                        item.Effects.Add(new EffectInteger((short)statAMaguear, value, new EffectBase()));
                    }
                    else
                    {
                        var finalStats = new List<EffectBase>();
                        //HERE
                        if (item.Gap <= 0) // EC en premier s'il n'y a pas de puits
                        {
                            finalStats = parseStringStatsEC_FM(item, poid, statAMaguear);
                            item.SetEffect(finalStats);
                            pwrPerte = currentWeightTotal -
                                currentTotalWeigthBase(finalStats, item); //OMG this is sooo wrong
                        }
                        finalStats = convertirStatsAStringFM(statAMaguear, item, value, negative);
                        finalStats.Add(new EffectInteger((short)statAMaguear, value, new EffectBase()));
                        item.SetEffect(finalStats);
                    }
                }
                //delete firm rune
                if (firm)
                    RemoveItem(AdditionalRune, 1);
                //delete rune
                RemoveItem(RuneOrPotion, 1);
                //final
                var finalEffects = item.Effects.ToList();
                item.SetEffects(finalEffects);
                var container = item.GetObjectItem();
                using (var exchangeCraftResultMagicWithObjectDescMessage = new ExchangeCraftResultMagicWithObjectDescMessage(
                    (sbyte)CraftResultEnum.CRAFT_NEUTRAL, new ObjectItemNotInContainer(container.objectGID,
                    container.effects, container.objectUID, 1), 1))
                {
                    Character.Client.Send(exchangeCraftResultMagicWithObjectDescMessage);
                }
                using (var exchangeCraftInformationObjectMessage = new ExchangeCraftInformationObjectMessage((sbyte)CraftResultEnum.CRAFT_NEUTRAL,
                    (ushort)item.Template.Id, (uint)Character.Id))
                {
                    Character.Map.ForEach(x => x.Client.Send(exchangeCraftInformationObjectMessage));
                }
                Character.Inventory.RefreshItem(item);
                Character.RefreshActor();
            }
            else
            {
                pwrPerte = 0;
                //delete firm rune
                if (firm)
                    RemoveItem(AdditionalRune, 1);
                //delete rune
                RemoveItem(RuneOrPotion, 1);
                if (stats.Count > 0)
                {
                    var finalStats = new List<EffectBase>();
                    //HERE
                    finalStats = parseStringStatsEC_FM(item, poid, -1);
                    item.SetEffect(finalStats);
                    pwrPerte = currentWeightTotal -
                        currentTotalWeigthBase(finalStats, item); //OMG this is sooo wrong
                }
                //final
                var finalEffects = item.Effects.ToList();
                item.SetEffects(finalEffects);
                var container = item.GetObjectItem();
                using (var exchangeCraftResultMagicWithObjectDescMessage = new ExchangeCraftResultMagicWithObjectDescMessage(
                    (sbyte)CraftResultEnum.CRAFT_FAILED, new ObjectItemNotInContainer(container.objectGID,
                    container.effects, container.objectUID, 1), 1))
                {
                    Character.Client.Send(exchangeCraftResultMagicWithObjectDescMessage);
                }
                using (var exchangeCraftInformationObjectMessage = new ExchangeCraftInformationObjectMessage((sbyte)CraftResultEnum.CRAFT_FAILED,
                    (ushort)item.Template.Id, (uint)Character.Id))
                {
                    Character.Map.ForEach(x => x.Client.Send(exchangeCraftInformationObjectMessage));
                }
                Character.Inventory.RefreshItem(item);
                Character.RefreshActor();
            }
            //Console.WriteLine($"Gap : {item.Gap} | PwrPerte : {pwrPerte} | Poid : {poid}");
            item.Gap = (item.Gap + pwrPerte) - poid;
        }

        private static bool CheckEffects(BasePlayerItem item)
        {
            foreach (var effect in item.Template.Effects)
            {
                var final = item.Effects.FirstOrDefault(x => x.EffectId == effect.EffectId);
                if (final == null)
                    return false;
            }
            return true;
        }

        internal void Stop()
        {
            if (!_interrupt)
                _interrupt = true;
            _allItems = false;
            IsRunning = false;
        }

        internal void UpdateCount(int count)
        {
            if (RuneOrPotion != null && Character.Inventory.HasItem(RuneOrPotion) && count == -1 && !IsRunning)
                _allItems = true;
        }

        internal void Execute()
        {
            if (ItemToMagus != null && RuneOrPotion != null && !IsRunning)
            {
                _interrupt = false;
                Task.Factory.StartNew(
                    () =>
                    {
                        var j = _allItems ? _count : 1;
                        for (int i = 0; i < j; i++)
                        {
                            if (_interrupt)
                                break;
                            if (Character.Inventory.HasItem(ItemToMagus) && Character.Inventory.HasItem(RuneOrPotion) && !_interrupt)
                            {
                                var firm = false;
                                if (AdditionalRune != null && Character.Inventory.HasItem(AdditionalRune))
                                    firm = true;
                                IsRunning = true;
                                Magus(ItemToMagus, RuneOrPotion, firm);
                                Thread.Sleep(400);
                            }
                        }
                        IsRunning = false;
                        _allItems = false;
                    }
                , TaskCreationOptions.LongRunning);
            }
        }

        public void RemoveItem(BasePlayerItem item, uint quantity)
        {
            if (Character.Inventory.HasItem(item))
            {
                Character.Inventory.RemoveItem(item, quantity);
                if (item.Stack <= 0)
                    InventoryHandler.SendExchangeObjectRemovedMessage(Character.Client, false, item.Guid);
                else
                    InventoryHandler.SendExchangeObjectModifiedMessage(Character.Client, false, item);
            }
            else
                InventoryHandler.SendExchangeObjectRemovedMessage(Character.Client, false, item.Guid);
        }

        public void Exchange(ExchangeObjectMoveMessage message)
        {
            var item = Character.Inventory.TryGetItem((int)message.objectUID);
            if (item != null && !IsRunning)
            {
                //Console.WriteLine($"uid : {message.objectUID} | quantity {message.quantity}");yyi
                if (message.quantity > 0 && item.Stack >= message.quantity)
                {
                    var fItem = Character.Inventory.CutItem(item, (int)message.quantity);
                    InventoryHandler.SendExchangeObjectAddedMessage(Character.Client, false, fItem); //TODO Quantity

                    if (IsPotionMagus(item.Template.Id) || item.Template.TypeId == 78 || item.Template.TypeId == 189) //rune or potion
                    {
                        RuneOrPotion = fItem;
                        _count = message.quantity;
                    }
                    else if (item.Template.Id == RUNE_FIRM)
                        AdditionalRune = fItem;
                    else
                    {
                        //TODO Check it's correct table
                        ItemToMagus = fItem;
                    }
                }
                else if (message.quantity < 0 && item.Stack >= (uint)Math.Abs(message.quantity))
                {
                    if (IsPotionMagus(item.Template.Id) || item.Template.TypeId == 78 || item.Template.TypeId == 189) //rune or potion
                    {
                        if (RuneOrPotion != null)
                        {
                            RuneOrPotion = null;
                            _count = 0;
                        }
                        else
                            return;
                    }
                    else if (item.Template.Id == RUNE_FIRM)
                    {
                        if (AdditionalRune != null)
                            AdditionalRune = null;
                        else
                            return;
                    }
                    else
                    {
                        if (ItemToMagus != null && Character.Inventory.HasItem(ItemToMagus))
                        {
                            BasePlayerItem item2;
                            if (Character.Inventory.IsStackable(ItemToMagus, out item2) && item2 != null)
                            {
                                Character.Inventory.StackItem(item2, (int)ItemToMagus.Stack);
                                Character.Inventory.RemoveItem(ItemToMagus);
                            }
                        }
                        //TODO Check it's correct table
                        if (ItemToMagus != null)
                            ItemToMagus = null;
                        else
                            return;
                    }
                    InventoryHandler.SendExchangeObjectRemovedMessage(Character.Client, false, item.Guid);
                }
            }
        }

        private static List<EffectBase> parseStringStatsEC_FM(BasePlayerItem item, int poid, int carac)
        {
            var p = 0;
            var key = 0;
            var perte = 0.0;
            var itemStats = item.Effects.OfType<EffectInteger>();
            var keys = itemStats.Select(x => (int)x.EffectId).ToList();
            var stats = itemStats.ToDictionary(x => (int)x.EffectId, y => y.Value); //I don't think has same effectId :p
            var finalStats = new List<EffectBase>();
            if (keys.Count > 1)
            {
                foreach (var effect in keys)// On cherche un OverFM
                {
                    var value = stats[effect];
                    if (item.Template.IsOverFm(effect, value))
                    {
                        key = effect;
                        break;
                    }
                    p++;
                }
                if (key > 0) // On place l'OverFm en t�te de liste pour �tre niqu�
                {
                    keys.Remove(keys[p]);
                    keys.Insert(p, keys[0]);
                    keys.Remove(keys[0]);
                    keys.Insert(0, key);
                }
            }
            foreach (var i in keys)
            {
                var newstats = 0;
                var statID = i;
                var value = stats[i];
                if (perte > poid || statID == carac)
                {
                    newstats = value;
                }
                else if ((statID == 152) || (statID == 154) || (statID == 155)
                    || (statID == 157) || (statID == 116) || (statID == 153))
                {
                    var a = (value * poid / 100.0);
                    if (a < 1.0)
                        a = 1.0;
                    var chute = value + a;
                    newstats = (int)Math.Floor(chute);
                    var max = getStatBaseMaxs(item.Template, i);
                    if (newstats > max)
                        newstats = max;
                }
                else
                {
                    if ((statID == 127) || (statID == 101))
                        continue;
                    if (statID >= 91 && statID <= 100)
                        continue;

                    double chute;
                    if (item.Template.IsOverFm(statID, value)) // Gros kick dans la gueulle de l'over FM
                        chute = (value - value
                            * (poid - Math.Floor(perte)) * 2 / 100.0);
                    else
                        chute = (value - value
                                * (poid - Math.Floor(perte)) / 100.0);
                    if ((chute / value) < 0.75)
                        chute = (value) * 0.75; // On ne peut pas perdre plus de 25% d'une stat d'un coup

                    var chutePwr = (value - chute)
                        * AdditionalFormulas.getPwrPerEffet(statID);

                    perte += chutePwr;

                    newstats = (int)Math.Floor(chute);
                }
                if (newstats < 1)
                    continue;
                var final = itemStats.FirstOrDefault(x => (int)x.EffectId == statID);
                final.Value = (short)newstats;
                finalStats.Add(final);
            }
            return finalStats;
        }

        //TEST
        private static int calculXpWinFm(uint lvl, int numCase)
        {
            switch (numCase)
            {
                case 1:
                    if (lvl < 80)
                        return 1;
                    return 0;

                case 2:
                    if (lvl < 120)
                        return 5;
                    return 0;

                case 3:
                    if (lvl > 18 && lvl < 160)
                        return 12;
                    return 0;

                case 4:
                    if (lvl > 38)
                        return 25;
                    return 0;

                case 5:
                    if (lvl > 78)
                        return 50;
                    return 0;

                case 6:
                    if (lvl > 118)
                        return 125;
                    return 0;

                case 7:
                    if (lvl > 158)
                        return 250;
                    return 0;

                case 8:
                    if (lvl > 198)
                        return 500;
                    return 0;
            }
            return 0;
        }

        private static List<int> chanceFM(int WeightTotalMaxBase,
                                                        int WeightTotalBaseMin, int currentWeithTotal,
                                                        int currentWeightStats, int weight, int diff, double coef,
                                                        int maxStat, int minStat, int actualStat, double x,
                                                        bool bonusRune, int statsAdd)
        {
            var chances = new List<int>();
            var c = 1.0;
            var m1 = (double)(maxStat - (actualStat + statsAdd)); // max possible stat - (actual object value of stat + value rune); | If negative it's more than possible | 100 - (50 +1) = 49;
            var m2 = (double)(maxStat - minStat); // max possible stat - min possible stats (but if max it's equal to min what happend?) (100 - 40)=60;
            if ((1 - (m1 / m2)) > 1.0) // ((1 - (49/60)) > 1.0 = 0.184
                c = (1 - ((1 - (m1 / m2)) / 2)) / 2;
            else if ((1 - (m1 / m2)) > 0.8) //= 0.184
                c = 1 - ((1 - (m1 / m2)) / 2);
            if (c < 0)
                c = 0;
            // la variable c reste � 1 si le jet ne depasse pas 80% sinon il diminue tr�s fortement. Si le jet d�passe 100% alors il diminue encore plus.

            var moyenne = Math.Floor(WeightTotalMaxBase //promedio (gapMax object - ((gapMax object - gapMin object) / 2.0);
                - ((WeightTotalMaxBase - WeightTotalBaseMin) / 2.0));

            var mStat = (moyenne / currentWeithTotal); // Si l'item est un bon jet dans l'ensemble, diminue les chances sinon l'inverse. (promedio / gapactual) =

            if (mStat > 1.2)
                mStat = 1.2;

            var a = ((((((WeightTotalMaxBase + diff) * coef) * mStat) * 0.4) * x) * 1.0); //Rate fm
            var b = (Math.Sqrt(currentWeithTotal + currentWeightStats) + weight);
            if (b < 1.0)
                b = 1.0;

            var p1 = (int)Math.Floor(a / b); // Succes critique
            var p2 = 0; // Succes neutre
            var p3 = 0; // Echec critique
            if (bonusRune)
                p1 += 20;
            if (p1 < 1)
            {
                p1 = 1;
                p2 = 0;
                p3 = 99;
            }
            else if (p1 > 100)
            {
                p1 = 66;
                p2 = 34;
            }
            else if (p1 > 66)
                p1 = 66;

            if (p2 == 0 && p3 == 0)
            {
                p2 = (int)Math.Floor(a
                        / (Math.Sqrt(currentWeithTotal + currentWeightStats)));
                if (p2 > (100 - p1))
                    p2 = (100 - p1);
                if (p2 > 50)
                    p2 = 50;
            }
            chances.Add(p1);
            chances.Add(p2);
            chances.Add(p3);
            return chances;
        }

        private static int currentStat(BasePlayerItem item, int effectToMagus)
        {
            foreach (var effect in item.Effects.OfType<EffectInteger>())
            {
                var effectId = (int)effect.EffectId;
                if (effectId != effectToMagus)
                    continue;
                return effect.Value;
            }
            return 0;
        }

        private static int getStatBaseMins(ItemTemplate objTemplate, int effectToMagus)
        {
            foreach (var effect in objTemplate.Effects.OfType<EffectDice>())
            {
                var effectId = (int)effect.EffectId;
                if (effectId != effectToMagus)
                    continue;
                return effect.DiceNum;
            }
            return 0;
        }

        private static int getActualJet(BasePlayerItem item, int effectToMagus)
        {
            foreach (var effect in item.Effects.OfType<EffectInteger>())
            {
                var effectId = (int)effect.EffectId;
                if (effectId != effectToMagus) //Inutil effect
                    continue;
                return effect.Value;
            }
            return 0;
        }

        private static int WeithTotalBaseMin(ItemTemplate template)
        {
            var weight = 0;
            var alt = 0;
            foreach (var s in template.Effects.OfType<EffectDice>())
            {
                var statID = (int)s.EffectId;
                if (ID_EFECTOS_ARMAS.Contains(statID))
                    continue;
                var value = 1;
                var min = s.DiceNum;
                value = min;
                var statX = 1;
                statX = AdditionalFormulas.GetMulti(statID);
                weight = value * statX; // peso de la caracteristica
                alt += weight;
            }
            return alt;
        }

        private static List<EffectBase> stringStatsFCForja(BasePlayerItem item, double runa)
        {
            foreach (var entry in item.Effects.OfType<EffectInteger>())
            {
                var nuevosStats = 0;
                var statID = (int)entry.EffectId;
                var valor = entry.Value;
                if (statID == 152 || statID == 154 || statID == 155 || statID == 157 || statID == 116 || statID == 153)
                {
                    var a = ((valor * runa) / 100.0);
                    if (a < 1)
                        a = 1;
                    var chute = (valor + a);
                    nuevosStats = (int)Math.Floor(chute);
                    var basemax = getStatBaseMaxs(item.Template, statID);
                    if (nuevosStats > basemax)
                    {
                        nuevosStats = basemax;
                    }
                }
                else
                {
                    if (statID == 127 || statID == 101)
                        continue;
                    var chute = (valor - ((valor * runa) / 100.0));
                    nuevosStats = (int)Math.Floor(chute);
                }
                if (nuevosStats < 1)
                    continue;
                entry.Value = (short)nuevosStats;
            }
            return item.Effects;
        }

        private static int getStatBaseMaxs(ItemTemplate template, int statID)
        {
            foreach (var s in template.Effects.OfType<EffectDice>())
            {
                var effect = (int)s.EffectId;
                if (effect != statID)
                    continue;
                var max = s.DiceFace;
                if (max == 0)
                    max = s.DiceNum;
                return max;
            }
            return 0;
        }

        private static List<EffectBase> convertirStatsAStringFM(int statAMaguear, BasePlayerItem item, int agregar, bool negativo)
        {
            var effectsToRemove = new List<EffectBase>();
            foreach (var entry in item.Effects.OfType<EffectInteger>())
            {
                var statID = (int)entry.EffectId;
                if (statAMaguear == statID)
                {
                    var newstats = 0;
                    if (negativo)
                    {
                        newstats = entry.Value - agregar;
                        if (newstats < 1)
                        {
                            effectsToRemove.Add(entry);
                            continue;
                        }

                    }
                    else
                        newstats = entry.Value + agregar;
                    entry.Value = (short)newstats;
                }
            }
            item.Effects.RemoveAll(effectsToRemove.Contains);
            return item.Effects;
        }

        private static int viewActualStatsItem(BasePlayerItem item, int effectToMagus) // 0 = no tiene, 1 = tiene, 2 = negativo
        {
            foreach (var effect in item.Effects)
            {
                var effectId = (int)effect.EffectId;
                if (effectId != effectToMagus)//Effets inutiles
                {
                    if (effectId == 0x98 && effectToMagus == 0x7b)
                        return 2;
                    if (effectId == 0x9a && effectToMagus == 0x77)
                        return 2;
                    if (effectId == 0x9b && effectToMagus == 0x7e)
                        return 2;
                    if (effectId == 0x9d && effectToMagus == 0x76)
                        return 2;
                    if (effectId == 0x9c && effectToMagus == 0x7c)
                        return 2;
                    if (effectId == 0x99 && effectToMagus == 0x7d)
                        return 2;
                    continue;
                }
                else //The effect exist cool!
                    return 1;
            }
            return 0;
        }

        private static int WeithTotalBase(ItemTemplate template)
        {
            var weight = 0;
            var alt = 0;
            foreach (var s in template.Effects.OfType<EffectDice>())
            {
                var statID = (int)s.EffectId;
                if (ID_EFECTOS_ARMAS.Contains(statID))
                    continue;
                var value = 1;
                var min = s.DiceNum;
                var max = s.DiceFace;
                value = min;
                if (max != 0)
                    value = max;
                var statX = 1;
                statX = AdditionalFormulas.GetMulti(statID);
                weight = value * statX; // peso de la caracteristica
                alt += weight;
            }
            return alt;
        }

        private static int currentWeithStats(BasePlayerItem item, EffectsEnum statToMagus)
        {
            foreach (var effect in item.Effects.OfType<EffectInteger>())
            {
                if (effect.EffectId != statToMagus)
                    continue;
                var statID = (int)effect.EffectId;
                var statX = 1;
                var coef = 1;
                var BaseStats = viewBaseStatsItem(item, (int)statToMagus);
                if (BaseStats == 2)
                    coef = 3;
                else if (BaseStats == 0)
                    coef = 8;
                statX = AdditionalFormulas.GetMulti(statID);
                return effect.Value * statX * coef;
            }
            return 0;
        }

        private static int currentTotalWeigthBase(List<EffectBase> stats, BasePlayerItem item) //If is the PWRG we don't manage coef
        {
            var Weigth = 0;
            var Alto = 0;
            foreach (var s in stats)
            {
                var statID = (int)s.EffectId;
                if (ID_EFECTOS_ARMAS.Contains(statID))
                    continue;
                var qua = 1;
                if (s is EffectDice)
                {
                    var x = s as EffectDice;
                    var min = x.DiceNum;
                    var max = x.DiceFace;
                    qua = min;
                    if (max != 0)
                        qua = max;
                }
                else if (s is EffectInteger)
                {
                    qua = ((EffectInteger)s).Value;
                }
                var statX = 1;
                var coef = 1;
                var statsBase = viewBaseStatsItem(item, statID);
                if (statsBase == 2)
                    coef = 3;
                else if (statsBase == 0)
                    coef = 2;
                statX = AdditionalFormulas.GetMulti(statID);
                Weigth = qua * statX * coef; // peso de la caracteristica
                Alto += Weigth;
            }
            return Alto;
        }

        private static byte viewBaseStatsItem(BasePlayerItem item, int stat)
        {
            foreach (var effect in item.Effects)
            {
                var effectId = (int)effect.EffectId;
                if (effectId != stat) //the effect don't exist
                {
                    if (effectId == 0x98 && stat == 0x7b)
                        return 2;
                    if (effectId == 0x9a && stat == 0x77)
                        return 2;
                    if (effectId == 0x9b && stat == 0x7e)
                        return 2;
                    if (effectId == 0x9d && stat == 0x76)
                        return 2;
                    if (effectId == 0x9c && stat == 0x7c)
                        return 2;
                    if (effectId == 0x74 && stat == 0x75)
                        return 2;
                    if (effectId == 0x99 && stat == 0x7d)
                        return 2;
                    continue;
                }
                else //The effect exist yei!
                    return 1;
            }
            return 0;
        }

        private static bool IsPotionMagus(int templateId)
        {
            switch (templateId)
            {
                case 1333:// pocion chispa
                case 1335:// pocion llovisna
                case 1337:// pocion de corriente de airee
                case 1338:// pocion de sacudida
                case 1340:// pocion derrumbamiento
                case 1341:// pocion chaparron
                case 1342:// pocion de rafaga
                case 1343:// pocion de Flameacion
                case 1345:// pocion Incendio
                case 1346:// pocion Tsunami
                case 1347:// pocion huracan
                case 1348:// pocion de seismo
                    return true;
            }
            return false;
        }

        #endregion Methods
    }
}
