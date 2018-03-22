using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System.Drawing;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer;
using System;
using Game.UtilisationTokens;
//By Geraxi
namespace Commands
{
	class Fm_Vita : InGameCommand
	{
		public Fm_Vita()
		{
			Aliases = new[] { "vita" };
			RequiredRole = RoleEnum.Player;
			Description = "Cette commande permet d'augmenter la vitalité de 50 une seule fois, excepté si l'item ne possède pas de bonus vitalité de base vous pourrez le fm 2 fois en échange de 50 jetons par Fm, les équipements modifiables sont : COIFFE / CAPE / AMULETTE / CEINTURE / ANNEAU_DROIT / ANNEAU_GAUCHE / BOTTES";
			AddParameter<string>("Item");
		}
		public override void Execute(GameTrigger trigger)
		{
			if (!trigger.Character.IsFighting())
			{
				var _item_A_Exo = trigger.Get<string>("Item");
				if (trigger.Character.Inventory.Tokens != null && trigger.Character.Inventory.Tokens.Stack >= 20 || trigger.Character.Level <= 40)
				{
					switch (_item_A_Exo.ToUpper())
					{
						case "COIFFE":
							FmItem(trigger.Character, EffectsEnum.Effect_AddVitality, "VITA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT);
							break;
						case "CAPE":
							FmItem(trigger.Character, EffectsEnum.Effect_AddVitality, "VITA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE);
							break;
						case "AMULETTE":
							FmItem(trigger.Character, EffectsEnum.Effect_AddVitality, "VITA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET);
							break;
						case "ANNEAU_GAUCHE":
							FmItem(trigger.Character, EffectsEnum.Effect_AddVitality, "VITA", CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT);
							break;
						case "ANNEAU_DROIT":
							FmItem(trigger.Character, EffectsEnum.Effect_AddVitality, "VITA", CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT);
							break;
						case "CEINTURE":
							FmItem(trigger.Character, EffectsEnum.Effect_AddVitality, "VITA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT);
							break;
						case "BOTTES":
							FmItem(trigger.Character, EffectsEnum.Effect_AddVitality, "VITA", CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS);
							break;
						default:
							trigger.Character.SendServerMessage("La saisie est incorrecte, les items modifiables sont : COIFFE / CAPE / AMULETTE / CEINTURE/ ANNEAU_DROIT / ANNEAU_GAUCHE / BOTTES.", Color.Red);
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
			foreach (var item in character.Inventory.GetEquipedItems())
			{
				if (item.Position == Position)
				{
					int EffectAddVitalityCount = 0;
					foreach (var EffectAddVitalityExist in item.Effects)
					{
						if (EffectAddVitalityExist.EffectId == EffectsEnum.Effect_AddVitality)
							EffectAddVitalityCount++;
					}
					if (EffectAddVitalityCount < 2)
					{
						character.Inventory.UnStackItem(character.Inventory.Tokens, 50);
						item.Effects.Add(new EffectInteger(EffectExo, 50));
						character.Inventory.MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
						character.SendServerMessage("L'item a était modifié + 50 vitalité avec succès.");
						character.SendServerMessage("Veuillez vous déconnecter pour avoir le bonus dans vos caractéristique.", Color.Red);
						WorldServer.Instance.IOTaskPool.AddMessage(new Action(character.SaveNow));
						UtilisationTokenManager token = new UtilisationTokenManager(character, "FM VITA", item.Template.Id, 0, tokenAvantAchat);
					}
					else
						character.SendServerMessage("Votre item possède déjà 2 bonus vitalité.", Color.Red);
					break;
				}
			}
		}
	}
}