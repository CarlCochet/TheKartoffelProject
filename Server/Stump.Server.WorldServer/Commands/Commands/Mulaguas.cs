//using System;
//using Stump.Core.Extensions;
//using Stump.DofusProtocol.Enums;
//using Stump.Server.BaseServer.Commands;
//using Stump.Server.WorldServer.Commands.Commands.Patterns;
//using Stump.Server.WorldServer.Commands.Trigger;
//using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;
//using Stump.Core.Reflection;
//using System.Linq;
//using Stump.Server.WorldServer.Database.Mounts;
//using Stump.Server.WorldServer.Game.Items;
//using Stump.Server.WorldServer.Game.Effects.Instances;

//namespace Stump.Server.WorldServer.Commands.Commands
//{

//    //Character.cs => LoadMounts() in first lines add:
//    //
//    //toeliminate
//    //            if (Record.EquippedMount.HasValue)
//    //            {
//    //                EquippedMount = null;
//    //                return;
//    //            }
//    //endtoelim

//    //info
//    public class MulaguasCommand : SubCommandContainer//cambia el nombre de la clase a MountComm dandole click derecho y Cambiar nombre
//    {
//        public MulaguasCommand()
//        {
//            Aliases = new[] { "ml" };
//            RequiredRole = RoleEnum.Player;
//            Description = "Comando de Mulaguas, para usar es <b>.ml equip NOMBREDEMULAGUA</b>, para saber que mulaguas tienes es <b>.ml list</b>, también te sirve para saber que nombre poner";

//        }

//    }
//    public class MulaguasInfoCommand : InGameSubCommand
//    {
//        public MulaguasInfoCommand()
//        {
//            Aliases = new[] { "list" };
//            base.RequiredRole = RoleEnum.Player;
//            Description = "lista de Mulaguas";
//            ParentCommandType = typeof(MulaguasCommand);

//        }

//        public override void Execute(GameTrigger trigger)
//        {
//            var Character = trigger.Character;
//            #region lista
//            var tem = MountManager.Instance.GetTemplates().ToList();
//            var tem2 = Character.Inventory.GetItems().Where(x => x.Template.Type.ItemType == ItemTypeEnum.CERTIFICAT_DE_MULDO).ToList();
//            trigger.Reply("Lista de Mulaguas");
//            foreach (var drago in tem)
//            {
//                if (!tem2.Any(x => x.Template.Id == drago.MountId))
//                    continue;

//                string effects = "";
//                foreach (EffectInteger effect in drago.Effects)
//                {
//                    switch (effect.EffectId)
//                    {
//                        case (EffectsEnum)110:
//                            effects += "Vit";
//                            break;
//                        case EffectsEnum.Effect_AddDamageBonus_121:
//                            effects += "Dmg Bonus";
//                            break;
//                        case EffectsEnum.Effect_AddCriticalDamageBonus:
//                            effects += "CriticosDmg";
//                            break;
//                        case EffectsEnum.Effect_AddDodgeAPProbability:
//                            effects += "EsquivaAp";
//                            break;
//                        case EffectsEnum.Effect_AddLock:
//                            effects += "Adlock";
//                            break;
//                        case EffectsEnum.Effect_AddDodgeMPProbability:
//                            effects += "DodgePMPp";
//                            break;
//                        case EffectsEnum.Effect_AddCriticalHit:
//                            effects += "Critical";
//                            break;
//                        case EffectsEnum.Effect_AddVitality:
//                            effects += "Addvida";
//                            break;
//                        case EffectsEnum.Effect_AddAgility:
//                            effects += "Agi";
//                            break;
//                        case EffectsEnum.Effect_AddIntelligence:
//                            effects += "Inte";
//                            break;
//                        case EffectsEnum.Effect_AddChance:
//                            effects += "Cha";
//                            break;
//                        case EffectsEnum.Effect_AddStrength:
//                            effects += "Fo";
//                            break;
//                        case EffectsEnum.Effect_AddInitiative:
//                            effects += "Ini";
//                            break;
//                        case EffectsEnum.Effect_IncreaseDamage_138:
//                            effects += "Potencia";
//                            break;
//                        case EffectsEnum.Effect_AddProspecting:
//                            effects += "Pp";
//                            break;
//                        case EffectsEnum.Effect_AddWisdom:
//                            effects += "Sabiduria";
//                            break;
//                        case EffectsEnum.Effect_AddSummonLimit:
//                            effects += "Invo";
//                            break;
//                        case EffectsEnum.Effect_AddMP:
//                            effects += "PM";
//                            break;
//                        case EffectsEnum.Effect_AddRange:
//                            effects += "Al";
//                            break;
//                        case EffectsEnum.Effect_AddEarthResistPercent:
//                            effects += "%Fo";
//                            break;
//                        case EffectsEnum.Effect_AddWaterResistPercent:
//                            effects += "%Cha";
//                            break;
//                        case EffectsEnum.Effect_AddFireResistPercent:
//                            effects += "%Inte";
//                            break;
//                        case EffectsEnum.Effect_AddAirResistPercent:
//                            effects += "%agi";
//                            break;
//                        case EffectsEnum.Effect_AddNeutralResistPercent:
//                            effects += "%Neu";
//                            break;
//                        case EffectsEnum.Effect_AddDamageReflection:
//                            effects += "Refl";
//                            break;
//                        case EffectsEnum.Effect_AddDodge:
//                            effects += "Dodge";
//                            break; 
//                        default:
//                            effects += effect.EffectId.ToString() + ":" + effect.Value;
//                            trigger.ReplyError("Not Handled Name for " + effect.EffectId);
//                            break;
//                    }
//                    effects += ":" + effect.Value;
//                    if (drago.Effects.Count > 1)
//                        effects += "/";
//                }
//                Character.SendServerMessage(string.Format("<b>" + drago.NameMount + "</b>") + "  =>" + effects);
//            }
//            #endregion

//        }



//    }

//    public class MulaguaEquipCommand : InGameSubCommand
//    {
//        public MulaguaEquipCommand()
//        {
//            Aliases = new[] { "equip" };
//            RequiredRole = RoleEnum.Player;
//            Description = "Equip a mount";
//            ParentCommandType = typeof(MulaguasCommand);
//            base.AddParameter<string>("ddname", "Dd", "Nombre de Dragopavo", null, false);
//            //Aca ese primer parametro lo cambias que sea mas largo que el segundo

//        }


//        public override void Execute(GameTrigger trigger)
//        {
//            #region  Equipar

//            var tem = MountManager.Instance.GetTemplates().ToList();
//            var dd = trigger.Get<string>("ddname");
//            var Character = trigger.Character;


//            if (tem.Any(x => x.NameMount == dd))
//            {
//                var certificado = ItemManager.Instance.TryGetTemplate(tem.First(x => x.NameMount == dd).MountId);
//                if (Character.Record.Dragos.Contains((short)tem.First(x => x.NameMount == dd).MountId))
//                {

//                }
//                else if (Character.Inventory.HasItem(certificado))
//                {
//                    var certifi = ItemManager.Instance.TryGetTemplate(tem.First(x => x.NameMount == dd).MountId);
//                    var certifi2 = Character.Inventory.First(x => x.Template.Id == certifi.Id);
//                    //certifi2.Effects.Add(new Game.Effects.Instances.EffectInteger(EffectsEnum.Effect_NonExchangeable_981, 1));
//                    //Character.Inventory.RefreshItem(certifi2);
//                    //trigger.Reply("Se ha ligado el certificado a tu personaje.");
//                    Character.Inventory.RemoveItem(certifi2, 1);
//                    Character.Record.Dragos.Add((short)certifi2.Template.Id);
//                    trigger.Reply($"Se ha añadido el dragopavo {dd} a tu lista.");
//                    Character.RefreshActor();
//                }
//                else
//                {
//                    trigger.ReplyError($"No posees el certificado para <b>'{dd}'</b>.");
//                    return;
//                }


//                if (Character.HasEquippedMount())
//                {
//                    if (Character.IsRiding)
//                        Character.ToggleRiding();
//                    Character.EquippedMount = null;
//                }
//                var template = tem.First(x => x.NameMount == dd);
//                template.PodsBase = 1000;
//                var list = tem.First(x => x.NameMount == dd).Effects;
//                MountRecord rec = new MountRecord
//                {
//                    Behaviors = new System.Collections.Generic.List<int>(),
//                    Energy = 1000,
//                    GivenExperience = 1,
//                    Id = Character.Id,
//                    IsDirty = false,
//                    IsInStable = false,
//                    Love = 50,
//                    Maturity = 50,
//                    ReproductionCount = 0,
//                    TemplateId = template.Id,
//                    Name = "Pavo de " + Character.Name
//                };
//                var t = new Mount(rec);
//                t.Level = 100;
//                foreach (var ef in list)
//                    switch (ef.EffectId)
//                    {
//                        case EffectsEnum.Effect_AddHealth:
//                            ef.EffectId = EffectsEnum.Effect_AddVitality;
//                            break;
//                    }
//                t.ApplyEffects(list);
//                t.Owner = Character;
//                Character.EquipMount(t);
//                Character.ToggleRiding();
//                trigger.Reply($"Equipado <b>{dd}</b>: {GetEffects(template)}");
//                Character.Record.Drago = t.Template.MountId;
//            }
//            else
//                trigger.ReplyError("Ningun pavo por nombre " + "'" + dd + "'");


//            #endregion
//        }
//        public string GetEffects(MountTemplate drago)
//        {
//            string effects = "";
//            foreach (var effect in drago.Effects)
//            {
//                switch (effect.EffectId)
//                {
//                    case (EffectsEnum)110:
//                        effects += "Vit";
//                        break;
//                    case EffectsEnum.Effect_AddAgility:
//                        effects += "Agi";
//                        break;
//                    case EffectsEnum.Effect_AddIntelligence:
//                        effects += "Inte";
//                        break;
//                    case EffectsEnum.Effect_AddChance:
//                        effects += "Cha";
//                        break;
//                    case EffectsEnum.Effect_AddStrength:
//                        effects += "Fo";
//                        break;
//                    case EffectsEnum.Effect_AddInitiative:
//                        effects += "Ini";
//                        break;
//                    case EffectsEnum.Effect_IncreaseDamage_138:
//                        effects += "Potencia";
//                        break;
//                    case EffectsEnum.Effect_AddProspecting:
//                        effects += "Pp";
//                        break;
//                    case EffectsEnum.Effect_AddWisdom:
//                        effects += "Sabiduria";
//                        break;
//                    case EffectsEnum.Effect_AddSummonLimit:
//                        effects += "Invo";
//                        break;
//                    case EffectsEnum.Effect_AddMP:
//                        effects += "PM";
//                        break;
//                    case EffectsEnum.Effect_AddRange:
//                        effects += "Al";
//                        break;
//                    case EffectsEnum.Effect_AddEarthResistPercent:
//                        effects += "%Fo";
//                        break;
//                    case EffectsEnum.Effect_AddWaterResistPercent:
//                        effects += "%Cha";
//                        break;
//                    case EffectsEnum.Effect_AddFireResistPercent:
//                        effects += "%Inte";
//                        break;
//                    case EffectsEnum.Effect_AddAirResistPercent:
//                        effects += "%agi";
//                        break;
//                    case EffectsEnum.Effect_AddNeutralResistPercent:
//                        effects += "%Neu";
//                        break;
//                    case EffectsEnum.Effect_AddDamageReflection:
//                        effects += "Refl";
//                        break;
//                    default:
//                        effects += effect.EffectId.ToString() + ":" + effect.Value;
//                        break;
//                }
//                effects += ":" + effect.Value;
//                if (drago.Effects.Count > 1)
//                    effects += "/";

//            }
//            return effects;
//        }
//    }
//    public class MulaguaRideCommand : InGameSubCommand
//    {
//        public MulaguaRideCommand()
//        {
//            Aliases = new[] { "on", "off" };
//            RequiredRole = RoleEnum.Player;
//            Description = "Sube/Baja de la montura";
//            ParentCommandType = typeof(MulaguasCommand);
//        }

//        public override void Execute(GameTrigger trigger)
//        {
//            if (trigger.Character.HasEquippedMount())
//                trigger.Character.ToggleRiding();
//            else
//                trigger.ReplyError("No tienes una montura equipada");

//        }
//    }
//    public class MyMulaguaCommand : InGameSubCommand
//    {
//        public MyMulaguaCommand()
//        {
//            Aliases = new[] { "my" };
//            RequiredRole = RoleEnum.Player;
//            Description = "Lista de tus monturas";
//            ParentCommandType = typeof(MulaguasCommand);
//        }

//        public override void Execute(GameTrigger trigger)
//        {
//            if (trigger.Character.Record.Dragos.Count == 0)
//                trigger.Reply("No tienes monturas");
//            else
//            {
//                var tem = MountManager.Instance.GetTemplates().ToList();
//                foreach (var drag in trigger.Character.Record.Dragos)
//                {
//                    var t = tem.First(x => x.MountId == drag);
//                    trigger.Reply(t.NameMount + ":" + GetEffects(t));
//                }
//            }

//        }
//        public string GetEffects(MountTemplate drago)
//        {
//            string effects = "";
//            foreach (var effect in drago.Effects)
//            {
//                switch (effect.EffectId)
//                {
//                    case (EffectsEnum)110:
//                        effects += "Vit";
//                        break;
//                    case EffectsEnum.Effect_AddAgility:
//                        effects += "Agi";
//                        break;
//                    case EffectsEnum.Effect_AddIntelligence:
//                        effects += "Inte";
//                        break;
//                    case EffectsEnum.Effect_AddChance:
//                        effects += "Cha";
//                        break;
//                    case EffectsEnum.Effect_AddStrength:
//                        effects += "Fo";
//                        break;
//                    case EffectsEnum.Effect_AddInitiative:
//                        effects += "Ini";
//                        break;
//                    case EffectsEnum.Effect_IncreaseDamage_138:
//                        effects += "Potencia";
//                        break;
//                    case EffectsEnum.Effect_AddProspecting:
//                        effects += "Pp";
//                        break;
//                    case EffectsEnum.Effect_AddWisdom:
//                        effects += "Sabiduria";
//                        break;
//                    case EffectsEnum.Effect_AddSummonLimit:
//                        effects += "Invo";
//                        break;
//                    case EffectsEnum.Effect_AddMP:
//                        effects += "PM";
//                        break;
//                    case EffectsEnum.Effect_AddRange:
//                        effects += "Al";
//                        break;
//                    case EffectsEnum.Effect_AddEarthResistPercent:
//                        effects += "%Fo";
//                        break;
//                    case EffectsEnum.Effect_AddWaterResistPercent:
//                        effects += "%Cha";
//                        break;
//                    case EffectsEnum.Effect_AddFireResistPercent:
//                        effects += "%Inte";
//                        break;
//                    case EffectsEnum.Effect_AddAirResistPercent:
//                        effects += "%agi";
//                        break;
//                    case EffectsEnum.Effect_AddNeutralResistPercent:
//                        effects += "%Neu";
//                        break;
//                    case EffectsEnum.Effect_AddDamageReflection:
//                        effects += "Refl";
//                        break;
//                    default:
//                        effects += effect.EffectId.ToString() + ":" + effect.Value;
//                        break;
//                }
//                effects += ":" + effect.Value;
//                if (drago.Effects.Count > 1)
//                    effects += "/";

//            }
//            return effects;
//        }
//    }
//}
