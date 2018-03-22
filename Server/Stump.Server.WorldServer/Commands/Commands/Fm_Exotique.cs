using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System.Drawing;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.BaseServer;
using Stump.Server.WorldServer;
using System;
using Stump.Core.Reflection;
using Stump.Server.WorldServer.Game;
using Game.UtilisationTokens;
//By Geraxi
namespace Commands
{
	class Fm_Exotique : InGameCommand
	{
		public Fm_Exotique()
		{
			Aliases = new[] { "exo" };
			RequiredRole = RoleEnum.Player;
			Description = "Cette commande permet de Fm un item contre 20 jetons, les types d'éxo possible sont : PA / PM / PO / CC / INVOC / VITA, Les équipements modifiables sont : COIFFE / CAPE / AMULETTE / CEINTURE / ANNEAU_DROIT / ANNEAU_GAUCHE / BOTTES";
			AddParameter<string>("Item");
			AddParameter<string>("Type_Exo");
		}
		public override void Execute(GameTrigger trigger)
		{
			if (!trigger.Character.IsFighting())
			{
				var _item_A_Exo = trigger.Get<string>("Item");
				var _type_De_Exo = trigger.Get<string>("Type_Exo");
				if (trigger.Character.Inventory.Tokens != null && trigger.Character.Inventory.Tokens.Stack >= 20 || trigger.Character.Level <= 40)
				{
					switch (_item_A_Exo.ToUpper())
					{
						case "COIFFE":
							#region Type_Exo
							switch (_type_De_Exo.ToUpper())
							{
								case "PA":
									FmItem(trigger.Character, EffectsEnum.Effect_AddAP_111, "PA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT);
									break;
								case "PM":
									FmItem(trigger.Character, EffectsEnum.Effect_AddMP_128, "PM", CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT);
									break;
								case "PO":
									FmItem(trigger.Character, EffectsEnum.Effect_AddRange, "PO", CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT);
									break;
								case "CC":
									FmItem(trigger.Character, EffectsEnum.Effect_AddCriticalHit, "CC", CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT);
									break;
								case "INVOC":
									FmItem(trigger.Character, EffectsEnum.Effect_AddSummonLimit, "INVOC", CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT);
									break;
								case "VITA":
									FmItem(trigger.Character, EffectsEnum.Effect_AddVitality, "VITA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT);
									break;
								default:
									trigger.ReplyError("Les types d'éxo possible sont :<b> PA / PM / PO / CC / INVOC / VITA <b>");
									break;
							}
							#endregion
							break;

						case "CAPE":
							#region Type_Exo
							switch (_type_De_Exo.ToUpper())
							{
								case "PA":
									FmItem(trigger.Character, EffectsEnum.Effect_AddAP_111, "PA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE);
									break;
								case "PM":
									FmItem(trigger.Character, EffectsEnum.Effect_AddMP_128, "PM", CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE);
									break;
								case "PO":
									FmItem(trigger.Character, EffectsEnum.Effect_AddRange, "PO", CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE);
									break;
								case "CC":
									FmItem(trigger.Character, EffectsEnum.Effect_AddCriticalHit, "CC", CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE);
									break;
								case "INVOC":
									FmItem(trigger.Character, EffectsEnum.Effect_AddSummonLimit, "INVOC", CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE);
									break;
								case "VITA":
									FmItem(trigger.Character, EffectsEnum.Effect_AddVitality, "VITA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT);
									break;
								default:
									trigger.ReplyError("Les types d'éxo possible sont :<b> PA / PM / PO / CC / INVOC / VITA <b>");
									break;
							}
							#endregion
							break;

						case "AMULETTE":
							#region Type_Exo
							switch (_type_De_Exo.ToUpper())
							{
								case "PA":
									FmItem(trigger.Character, EffectsEnum.Effect_AddAP_111, "PA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET);
									break;
								case "PM":
									FmItem(trigger.Character, EffectsEnum.Effect_AddMP_128, "PM", CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET);
									break;
								case "PO":
									FmItem(trigger.Character, EffectsEnum.Effect_AddRange, "PO", CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET);
									break;
								case "CC":
									FmItem(trigger.Character, EffectsEnum.Effect_AddCriticalHit, "CC", CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET);
									break;
								case "INVOC":
									FmItem(trigger.Character, EffectsEnum.Effect_AddSummonLimit, "INVOC", CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET);
									break;
								case "VITA":
									FmItem(trigger.Character, EffectsEnum.Effect_AddVitality, "VITA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT);
									break;
								default:
									trigger.ReplyError("Les types d'éxo possible sont :<b> PA / PM / PO / CC / INVOC / VITA <b>");
									break;
							}
							#endregion
							break;

						case "ANNEAU_GAUCHE":
							#region Type_Exo
							switch (_type_De_Exo.ToUpper())
							{
								case "PA":
									FmItem(trigger.Character, EffectsEnum.Effect_AddAP_111, "PA", CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT);
									break;
								case "PM":
									FmItem(trigger.Character, EffectsEnum.Effect_AddMP_128, "PM", CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT);
									break;
								case "PO":
									FmItem(trigger.Character, EffectsEnum.Effect_AddRange, "PO", CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT);
									break;
								case "CC":
									FmItem(trigger.Character, EffectsEnum.Effect_AddCriticalHit, "CC", CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT);
									break;
								case "INVOC":
									FmItem(trigger.Character, EffectsEnum.Effect_AddSummonLimit, "INVOC", CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT);
									break;
								case "VITA":
									FmItem(trigger.Character, EffectsEnum.Effect_AddVitality, "VITA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT);
									break;
								default:
									trigger.ReplyError("Les types d'éxo possible sont :<b> PA / PM / PO / CC / INVOC / VITA <b>");
									break;
							}
							#endregion
							break;

						case "ANNEAU_DROIT":
							#region Type_Exo
							switch (_type_De_Exo.ToUpper())
							{
								case "PA":
									FmItem(trigger.Character, EffectsEnum.Effect_AddAP_111, "PA", CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT);
									break;
								case "PM":
									FmItem(trigger.Character, EffectsEnum.Effect_AddMP_128, "PM", CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT);
									break;
								case "PO":
									FmItem(trigger.Character, EffectsEnum.Effect_AddRange, "PO", CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT);
									break;
								case "CC":
									FmItem(trigger.Character, EffectsEnum.Effect_AddCriticalHit, "CC", CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT);
									break;
								case "INVOC":
									FmItem(trigger.Character, EffectsEnum.Effect_AddSummonLimit, "INVOC", CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT);
									break;
								case "VITA":
									FmItem(trigger.Character, EffectsEnum.Effect_AddVitality, "VITA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT);
									break;
								default:
									trigger.ReplyError("Les types d'éxo possible sont :<b> PA / PM / PO / CC / INVOC / VITA <b>");
									break;
							}
							#endregion
							break;

						case "CEINTURE":
							#region Type_Exo
							switch (_type_De_Exo.ToUpper())
							{
								case "PA":
									FmItem(trigger.Character, EffectsEnum.Effect_AddAP_111, "PA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT);
									break;
								case "PM":
									FmItem(trigger.Character, EffectsEnum.Effect_AddMP_128, "PM", CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT);
									break;
								case "PO":
									FmItem(trigger.Character, EffectsEnum.Effect_AddRange, "PO", CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT);
									break;
								case "CC":
									FmItem(trigger.Character, EffectsEnum.Effect_AddCriticalHit, "CC", CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT);
									break;
								case "INVOC":
									FmItem(trigger.Character, EffectsEnum.Effect_AddSummonLimit, "INVOC", CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT);
									break;
								case "VITA":
									FmItem(trigger.Character, EffectsEnum.Effect_AddVitality, "VITA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT);
									break;
								default:
									trigger.ReplyError("Les types d'éxo possible sont :<b> PA / PM / PO / CC / INVOC / VITA <b>");
									break;
							}
							#endregion
							break;

						case "BOTTES":
							#region Type_Exo
							switch (_type_De_Exo.ToUpper())
							{
								case "PA":
									FmItem(trigger.Character, EffectsEnum.Effect_AddAP_111, "PA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS);
									break;
								case "PM":
									FmItem(trigger.Character, EffectsEnum.Effect_AddMP_128, "PM", CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS);
									break;
								case "PO":
									FmItem(trigger.Character, EffectsEnum.Effect_AddRange, "PO", CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS);
									break;
								case "CC":
									FmItem(trigger.Character, EffectsEnum.Effect_AddCriticalHit, "CC", CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS);
									break;
								case "INVOC":
									FmItem(trigger.Character, EffectsEnum.Effect_AddSummonLimit, "INVOC", CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS);
									break;
								case "VITA":
									FmItem(trigger.Character, EffectsEnum.Effect_AddVitality, "VITA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT);
									break;
								default:
									trigger.ReplyError("Les types d'éxo possible sont :<b> PA / PM / PO / CC / INVOC / VITA <b>");
									break;
							}
							#endregion
							break;

						default:
							trigger.Character.SendServerMessage("La saisie est incorrecte les items qui sont modifiable : COIFFE / CAPE / AMULETTE / CEINTURE / ANNEAU_DROIT / ANNEAU_GAUCHE / BOTTES.", Color.Red);
							break;
					}
				}
				else
				{
					trigger.ReplyError("Vous n'avez pas assez de jetons.", Color.Red);
				}
			}
			else
			{
				trigger.ReplyError("Pour lancer cette commande, il faut être en dehors d'un combat. ");
			}
		}
		private void FmItem(Character character, EffectsEnum EffectExo, string _type_Exo, CharacterInventoryPositionEnum Position)
		{
			int tokenAvantAchat = (int)character.Inventory.Tokens.Stack;
			string transactionType = string.Empty;
			var _verification = character.Inventory.GetEquipedItems();
			foreach (var item in _verification)
			{
				if (item.Position == Position)
				{
					switch (EffectExo)
					{
						case EffectsEnum.Effect_AddAP_111:
						case EffectsEnum.Effect_AddMP_128:
							#region PA / PM
							if (!item.Effects.Exists(x => x.EffectId == EffectsEnum.Effect_AddAP_111) && !item.Effects.Exists(y => y.EffectId == EffectsEnum.Effect_AddMP_128))
							{
								character.Inventory.UnStackItem(character.Inventory.Tokens, 150);
								item.Effects.Add(new EffectInteger(EffectExo, 1));
								character.Inventory.MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
								character.SendServerMessage(string.Format("L'item a était éxo + {0} avec succès.", _type_Exo));
								character.SendServerMessage("Veuillez vous déconnecter pour avoir l'éxo dans vos caractéristique.", Color.Red);
								transactionType = "FM " + _type_Exo;
							}
							else
							{
								character.SendServerMessage("L'item possède déjà un éxo + PA ou PM. ", Color.Red);
							}
							#endregion
							break;
						case EffectsEnum.Effect_AddRange:
							#region PO
							if (!item.Effects.Exists(x => x.EffectId == EffectsEnum.Effect_AddRange))
							{
								character.Inventory.UnStackItem(character.Inventory.Tokens, 150);
								item.Effects.Add(new EffectInteger(EffectExo, 1));
								character.Inventory.MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
								character.SendServerMessage(string.Format("L'item a était éxo + {0} avec succès.", _type_Exo));
								character.SendServerMessage("Veuillez vous déconnecter pour avoir l'éxo dans vos caractéristique.", Color.Red);
								transactionType = "FM " + _type_Exo;
							}
							else
							{
								character.SendServerMessage("L'item possède déjà un éxo + PA/PM ou PO. ", Color.Red);
							}
							#endregion
							break;
						case EffectsEnum.Effect_AddCriticalHit:
							#region CC
							if (!item.Effects.Exists(x => x.EffectId == EffectsEnum.Effect_AddCriticalHit))
							{
								character.Inventory.UnStackItem(character.Inventory.Tokens, 150);
								item.Effects.Add(new EffectInteger(EffectExo, 1));
								character.Inventory.MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
								character.SendServerMessage(string.Format("L'item a était éxo + {0} avec succès.", _type_Exo));
								character.SendServerMessage("Veuillez vous déconnecter pour avoir l'éxo dans vos caractéristique.", Color.Red);
								transactionType = "FM " + _type_Exo;
							}
							else
							{
								character.SendServerMessage("L'item possède déjà un éxo + CC. ", Color.Red);
							}
							#endregion
							break;
						case EffectsEnum.Effect_AddSummonLimit:
							#region INVOC

							if (!item.Effects.Exists(x => x.EffectId == EffectsEnum.Effect_AddSummonLimit))
							{
								character.Inventory.UnStackItem(character.Inventory.Tokens, 150);
								item.Effects.Add(new EffectInteger(EffectExo, 1));
								character.Inventory.MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
								character.SendServerMessage(string.Format("L'item a était éxo + {0} avec succès.", _type_Exo));
								character.SendServerMessage("Veuillez vous déconnecter pour avoir l'éxo dans vos caractéristique.", Color.Red);
								transactionType = "FM " + _type_Exo;
							}
							else
							{
								character.SendServerMessage("L'item possède déjà un éxo + INVOC. ", Color.Red);
							}
							#endregion
							break;

						case EffectsEnum.Effect_AddVitality:
							#region VITA

							int EffectAddVitalityCount = 0;
							foreach (var EffectAddVitalityExist in item.Effects)
							{
								if (EffectAddVitalityExist.EffectId == EffectsEnum.Effect_AddVitality)
									EffectAddVitalityCount++;
							}
							if (EffectAddVitalityCount < 2)
							{
								character.Inventory.UnStackItem(character.Inventory.Tokens, 100);
								item.Effects.Add(new EffectInteger(EffectExo, 100));
								character.Inventory.MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
								character.SendServerMessage("L'item a était modifié + 100 vitalité avec succès.");
								character.SendServerMessage("Veuillez vous déconnecter pour avoir le bonus dans vos caractéristique.", Color.Red);
								WorldServer.Instance.IOTaskPool.AddMessage(new Action(character.SaveNow));
								UtilisationTokenManager token = new UtilisationTokenManager(character, "FM VITA", item.Template.Id, 0, tokenAvantAchat);
							}
							else
								character.SendServerMessage("Votre item possède déjà 2 bonus vitalité.", Color.Red);
							#endregion
							break;
					}

					{

					}
					ServerBase<WorldServer>.Instance.IOTaskPool.AddMessage(new Action(character.SaveNow));
					if (transactionType != string.Empty)
					{
						UtilisationTokenManager token = new UtilisationTokenManager(character, transactionType, item.Template.Id, 0, tokenAvantAchat);
					}
					break;
				}
			}
		}
	}
}