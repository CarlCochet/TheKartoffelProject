using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Items.Player.Custom.LivingObjects;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler
    {
        public const int MimicryId = 14485;
        [WorldHandler(ObjectSetPositionMessage.Id)]
        public static void HandleObjectSetPositionMessage(WorldClient client, ObjectSetPositionMessage message)
        {
            if (!Enum.IsDefined(typeof(CharacterInventoryPositionEnum), (int) message.position))
                return;

            var item = client.Character.Inventory.TryGetItem(message.objectUID);

            if (item == null)
                return;

            client.Character.Inventory.MoveItem(item, (CharacterInventoryPositionEnum) message.position);
        }

        [WorldHandler(ObjectDeleteMessage.Id)]
        public static void HandleObjectDeleteMessage(WorldClient client, ObjectDeleteMessage message)
        {
            var item = client.Character.Inventory.TryGetItem(message.objectUID);

            if (item == null)
                return;

            client.Character.Inventory.RemoveItem(item, message.quantity);
        }

        [WorldHandler(ObjectUseMessage.Id)]
        public static void HandleObjectUseMessage(WorldClient client, ObjectUseMessage message)
        {
            var basePlayerItem = client.Character.Inventory.TryGetItem((int)message.objectUID);
            if (basePlayerItem != null)
                client.Character.Inventory.UseItem(basePlayerItem);
            if (basePlayerItem != null)
                switch (basePlayerItem.Template.Id)
                {
                    // ---------------- Bolsitas ------------------//
                    case 16819:
                        var itemTemplate6 = Singleton<ItemManager>.Instance.TryGetTemplate(2331); //Berenjena
                        client.Character.Inventory.AddItem(itemTemplate6, 10);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16820:
                        var itemTemplate13 = Singleton<ItemManager>.Instance.TryGetTemplate(1984); //Cenizas Eternas
                        client.Character.Inventory.AddItem(itemTemplate13, 10);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16821:
                        var itemTemplate14 = Singleton<ItemManager>.Instance.TryGetTemplate(1734); //Cerezas
                        client.Character.Inventory.AddItem(itemTemplate14, 10);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16822:
                        var itemTemplate = Singleton<ItemManager>.Instance.TryGetTemplate(1736); //Limones
                        client.Character.Inventory.AddItem(itemTemplate, 10);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16825:
                        var itemTemplate4 = Singleton<ItemManager>.Instance.TryGetTemplate(1977); //Especias
                        client.Character.Inventory.AddItem(itemTemplate4, 10);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16826:
                        var itemTemplate7 = Singleton<ItemManager>.Instance.TryGetTemplate(1974); //Enchalada (Lechuga e-e)
                        client.Character.Inventory.AddItem(itemTemplate7, 10);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16827:
                        var itemTemplate9 = Singleton<ItemManager>.Instance.TryGetTemplate(1983); //Grasa Gelatinosa
                        client.Character.Inventory.AddItem(itemTemplate9, 10);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16828:
                        var itemTemplate11 = Singleton<ItemManager>.Instance.TryGetTemplate(6671); //Alubias
                        client.Character.Inventory.AddItem(itemTemplate11, 10);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16830:
                        var itemTemplate2 = Singleton<ItemManager>.Instance.TryGetTemplate(1978); //Pimienta
                        client.Character.Inventory.AddItem(itemTemplate2, 10);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16831:
                        var itemTemplate3 = Singleton<ItemManager>.Instance.TryGetTemplate(1730); //Sal
                        client.Character.Inventory.AddItem(itemTemplate3, 10);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16832:
                        var itemTemplate5 = Singleton<ItemManager>.Instance.TryGetTemplate(1975); //Cebolla
                        client.Character.Inventory.AddItem(itemTemplate5, 10);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16833:
                        var itemTemplate8 = Singleton<ItemManager>.Instance.TryGetTemplate(519); //Polvos Magicos
                        client.Character.Inventory.AddItem(itemTemplate8, 10);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16834:
                        var itemTemplate10 = Singleton<ItemManager>.Instance.TryGetTemplate(1986); //Polvo Temporal
                        client.Character.Inventory.AddItem(itemTemplate10, 10);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16835:
                        var itemTemplate12 = Singleton<ItemManager>.Instance.TryGetTemplate(1985); //Resina
                        client.Character.Inventory.AddItem(itemTemplate12, 10);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    // ----------------- Tonel ---------------- //
                    case 16823:
                        var itemTemplate18 = Singleton<ItemManager>.Instance.TryGetTemplate(1731); //Zumo Sabroso
                        client.Character.Inventory.AddItem(itemTemplate18, 15);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16824:
                        var itemTemplate15 = Singleton<ItemManager>.Instance.TryGetTemplate(311); //Agua
                        client.Character.Inventory.AddItem(itemTemplate15, 15);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16829:
                        var itemTemplate17 = Singleton<ItemManager>.Instance.TryGetTemplate(1973); //Aceite para freir
                        client.Character.Inventory.AddItem(itemTemplate17, 15);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 16836:
                        var itemTemplate16 = Singleton<ItemManager>.Instance.TryGetTemplate(2012); //Sangre de Scorbuto
                        client.Character.Inventory.AddItem(itemTemplate16, 15);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;

                    // ----------------- Pociones ---------------- //
                    case 6965:
                        tpPlayer(client.Character, 5506048, 359, DirectionsEnum.DIRECTION_EAST);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 6964:
                        tpPlayer(client.Character, 13631488, 370, DirectionsEnum.DIRECTION_EAST);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 996: //Multigly
                        tpPlayer(client.Character, 98566657, 43, DirectionsEnum.DIRECTION_EAST);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case 548:
                        var objectPosition = client.Character.GetSpawnPoint();
                        var NextMap = objectPosition.Map;
                        var Cell = objectPosition.Cell;
                        client.Character.Teleport(NextMap, Cell);
                        client.Character.Inventory.RemoveItem(basePlayerItem, 1);
                        break;
                    case MimicryId:
                        client.Character.Inventory.Save();
                        client.Send(new ClientUIOpenedByObjectMessage(3, (int)basePlayerItem.Guid));
                        break;
                }

            // ----------------- Item's suprise ---------------- //

            if (itemsToGift.Contains(basePlayerItem.Template.Id))
            {
                var random = new Random().Next(0, 8);
                var ItemAdd = Singleton<ItemManager>.Instance.TryGetTemplate(itemsToGift[random]); //Suprise set Browk for tokens
                client.Character.Inventory.AddItem(ItemAdd, 1);
                client.Character.Inventory.RemoveItem(basePlayerItem, 1);
            }
        }

        static readonly int[] itemsToGift = { 6887, 6811, 6807, 6805, 6804, 6799, 6813, 6812 };

        

        [WorldHandler(ObjectUseMultipleMessage.Id)]
        public static void HandleObjectUseMultipleMessage(WorldClient client, ObjectUseMultipleMessage message)
        {
            var item = client.Character.Inventory.TryGetItem(message.objectUID);

            if (item == null)
                return;
            client.Character.Inventory.UseItem(item, message.quantity);

        }

        [WorldHandler(ObjectUseOnCellMessage.Id)]
        public static void HandleObjectUseOnCellMessage(WorldClient client, ObjectUseOnCellMessage message)
        {
            var item = client.Character.Inventory.TryGetItem(message.objectUID);

            if (item == null)
                return;

            var cell = client.Character.Map.GetCell(message.cells);

            if (cell == null)
                return;

            client.Character.Inventory.UseItem(item, cell);
        }

        [WorldHandler(ObjectUseOnCharacterMessage.Id)]
        public static void HandleObjectUseOnCharacterMessage(WorldClient client, ObjectUseOnCharacterMessage message)
        {
            var item = client.Character.Inventory.TryGetItem(message.objectUID);

            if (item == null)
                return;

            if (!item.Template.Targetable)
                return;

            var character = client.Character.Map.GetActor<Character>((int) message.characterId);

            if (character == null)
                return;

            client.Character.Inventory.UseItem(item, character);
        }

        [WorldHandler(ObjectFeedMessage.Id)]
        public static void HandleObjectFeedMessage(WorldClient client, ObjectFeedMessage message)
        {
            if (client.Character.IsInFight())
                return;

            var item = client.Character.Inventory.TryGetItem(message.objectUID);
            var food = client.Character.Inventory.TryGetItem(message.foodUID);

            if (item == null || food == null)
                return;

            if (food.Stack < message.foodQuantity)
                message.foodQuantity = (short) food.Stack;

            var i = 0;
            while (i < message.foodQuantity && item.Feed(food))
                i++;

            client.Character.Inventory.RemoveItem(food, i);
        }

        [WorldHandler(LivingObjectChangeSkinRequestMessage.Id)]
        public static void HandleLivingObjectChangeSkinRequestMessage(WorldClient client, LivingObjectChangeSkinRequestMessage message)
        {
            if (client.Character.IsInFight())
                return;

            var item = client.Character.Inventory.TryGetItem(message.livingUID);

            if (!(item is CommonLivingObject))
                return;

            ((CommonLivingObject) item).SelectedLevel = (short)message.skinId;
        }

        [WorldHandler(LivingObjectDissociateMessage.Id)]
        public static void HandleLivingObjectDissociateMessage(WorldClient client, LivingObjectDissociateMessage message)
        {
            if (client.Character.IsInFight())
                return;

            var item = client.Character.Inventory.TryGetItem(message.livingUID);

            (item as BoundLivingObjectItem)?.Dissociate();
        }

        [WorldHandler(ObjectDropMessage.Id)]
        public static void HandleObjectDropMessage(WorldClient client, ObjectDropMessage message)
        {
            if (client.Character.IsInFight() || client.Character.IsInExchange())
                return;

            client.Character.DropItem(message.objectUID, message.quantity);
        }

        [WorldHandler(MimicryObjectFeedAndAssociateRequestMessage.Id)]
        public static void HandleMimicryObjectFeedAndAssociateRequestMessage(WorldClient client, MimicryObjectFeedAndAssociateRequestMessage message)
        {
            if (client.Character.IsInFight())
                return;

            var character = client.Character;

            var host = character.Inventory.TryGetItem(message.hostUID);
            var food = character.Inventory.TryGetItem(message.foodUID);
            var mimisymbic = character.Inventory.TryGetItem(ItemIdEnum.MIMIBIOTE_14485);

            if (host == null || food == null)
            {
                SendMimicryObjectErrorMessage(client, host == null ? MimicryErrorEnum.NO_VALID_HOST : MimicryErrorEnum.NO_VALID_FOOD);
                return;
            }

            if (mimisymbic == null)
            {
                SendMimicryObjectErrorMessage(client, MimicryErrorEnum.NO_VALID_MIMICRY);
                return;
            }

            if (host.Effects.Any(x => x.EffectId == EffectsEnum.Effect_LivingObjectId || x.EffectId == EffectsEnum.Effect_Appearance || x.EffectId == EffectsEnum.Effect_Apparence_Wrapper)
                || !host.Template.Type.Mimickable)
            {
                SendMimicryObjectErrorMessage(client, MimicryErrorEnum.NO_VALID_HOST);
                return;
            }

            if (food.Effects.Any(x => x.EffectId == EffectsEnum.Effect_LivingObjectId || x.EffectId == EffectsEnum.Effect_Appearance || x.EffectId == EffectsEnum.Effect_Apparence_Wrapper)
                || !food.Template.Type.Mimickable)
            {
                SendMimicryObjectErrorMessage(client, MimicryErrorEnum.NO_VALID_FOOD);
                return;
            }

            if (food.Template.Id == host.Template.Id)
            {
                SendMimicryObjectErrorMessage(client, MimicryErrorEnum.SAME_SKIN);
                return;
            }

            if (food.Template.Level > host.Template.Level)
            {
                SendMimicryObjectErrorMessage(client, MimicryErrorEnum.FOOD_LEVEL);
                return;
            }

            if (food.Template.TypeId != host.Template.TypeId)
            {
                SendMimicryObjectErrorMessage(client, MimicryErrorEnum.FOOD_TYPE);
                return;
            }

            var modifiedItem = ItemManager.Instance.CreatePlayerItem(character, host);
            modifiedItem.Effects.Add(new EffectInteger(EffectsEnum.Effect_Appearance, (short)food.Template.Id));
            modifiedItem.Stack = 1;

            if (message.preview)
            {
                SendMimicryObjectPreviewMessage(client, modifiedItem);
            }
            else
            {
                character.Inventory.UnStackItem(food, 1);
                character.Inventory.UnStackItem(mimisymbic, 1);
                character.Inventory.UnStackItem(host, 1);
                character.Inventory.AddItem(modifiedItem);

                SendMimicryObjectAssociatedMessage(client, modifiedItem);
            }
        }

        [WorldHandler(MimicryObjectEraseRequestMessage.Id)]
        public static void HandleMimicryObjectEraseRequestMessage(WorldClient client, MimicryObjectEraseRequestMessage message)
        {
            if (client.Character.IsInFight())
                return;

            var host = client.Character.Inventory.TryGetItem(message.hostUID);

            if (host == null)
                return;

            host.Effects.RemoveAll(x => x.EffectId == EffectsEnum.Effect_Appearance);
            host.Invalidate();

            client.Character.Inventory.RefreshItem(host);
            client.Character.UpdateLook();

            SendInventoryWeightMessage(client);
        }

        [WorldHandler(WrapperObjectDissociateRequestMessage.Id)]
        public static void HandleWrapperObjectDissociateRequestMessage(WorldClient client, WrapperObjectDissociateRequestMessage message)
        {
            if (client.Character.IsInFight() || client.Character.IsInExchange())
                return;

            var host = client.Character.Inventory.TryGetItem(message.hostUID);

            if (host == null)
                return;

            var apparenceWrapper = host.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_Apparence_Wrapper) as EffectInteger;

            if (apparenceWrapper == null)
                return;

            var wrapperItemTemplate = ItemManager.Instance.TryGetTemplate(apparenceWrapper.Value);

            host.Effects.RemoveAll(x => x.EffectId == EffectsEnum.Effect_Apparence_Wrapper);

            host.Invalidate();
            client.Character.Inventory.RefreshItem(host);
            host.OnObjectModified();

            var wrapperItem = ItemManager.Instance.CreatePlayerItem(client.Character, wrapperItemTemplate, 1);

            client.Character.Inventory.AddItem(wrapperItem);
            client.Character.UpdateLook();

            SendInventoryWeightMessage(client);
        }

        public static void SendWrapperObjectAssociatedMessage(IPacketReceiver client, BasePlayerItem host)
        {
            client.Send(new WrapperObjectAssociatedMessage(host.Guid));
        }

        public static void SendMimicryObjectAssociatedMessage(IPacketReceiver client, BasePlayerItem host)
        {
            client.Send(new MimicryObjectAssociatedMessage(host.Guid));
        }

        public static void SendMimicryObjectPreviewMessage(IPacketReceiver client, BasePlayerItem host)
        {
            client.Send(new MimicryObjectPreviewMessage(host.GetObjectItem()));
        }

        public static void SendMimicryObjectErrorMessage(IPacketReceiver client, MimicryErrorEnum error)
        {
            client.Send(new MimicryObjectErrorMessage((sbyte)ObjectErrorEnum.SYMBIOTIC_OBJECT_ERROR, (sbyte)error, true));
        }

        public static void SendGameRolePlayPlayerLifeStatusMessage(IPacketReceiver client)
        {
            client.Send(new GameRolePlayPlayerLifeStatusMessage());
        }

        public static void SendInventoryContentMessage(WorldClient client)
        {
            client.Send(new InventoryContentMessage(client.Character.Inventory.Select(entry => entry.GetObjectItem()),
                client.Character.Inventory.Kamas));
        }

        public static void SendInventoryContentAndPresetMessage(WorldClient client)
        {
            client.Send(new InventoryContentAndPresetMessage(client.Character.Inventory.Select(entry => entry.GetObjectItem()),
                client.Character.Inventory.Kamas, client.Character.Inventory.Presets.Select(entry => entry.GetNetworkPreset()), new IdolsPreset[0]));
        }

        public static void SendInventoryWeightMessage(WorldClient client)
        {
            client.Send(new InventoryWeightMessage(client.Character.Inventory.Weight,
                                                   (int) client.Character.Inventory.WeightTotal));
        }

        public static void SendExchangeKamaModifiedMessage(IPacketReceiver client, bool remote, int kamasAmount)
        {
            client.Send(new ExchangeKamaModifiedMessage(remote, kamasAmount));
        }

        public static void SendObjectAddedMessage(IPacketReceiver client, IItem addedItem)
        {
            client.Send(new ObjectAddedMessage(addedItem.GetObjectItem()));
        }

        public static void SendObjectsAddedMessage(IPacketReceiver client, IEnumerable<ObjectItem> addeditems)
        {
            client.Send(new ObjectsAddedMessage(addeditems));
        }

        public static void SendObjectsQuantityMessage(IPacketReceiver client, IEnumerable<ObjectItemQuantity> itemQuantity)
        {
            client.Send(new ObjectsQuantityMessage(itemQuantity));
        }

        public static void SendObjectDeletedMessage(IPacketReceiver client, int guid)
        {
            client.Send(new ObjectDeletedMessage(guid));
        }

        public static void SendObjectsDeletedMessage(IPacketReceiver client, IEnumerable<int> guids)
        {
            client.Send(new ObjectsDeletedMessage(guids.Select(entry => entry).ToList()));
        }

        public static void SendObjectModifiedMessage(IPacketReceiver client, IItem item)
        {
            client.Send(new ObjectModifiedMessage(item.GetObjectItem()));
        }

        public static void SendObjectMovementMessage(IPacketReceiver client, BasePlayerItem movedItem)
        {
            client.Send(new ObjectMovementMessage(movedItem.Guid, (sbyte) movedItem.Position));
        }

        public static void SendObjectQuantityMessage(IPacketReceiver client, BasePlayerItem item)
        {
            client.Send(new ObjectQuantityMessage(item.Guid, (int) item.Stack));
        }

        public static void SendObjectErrorMessage(IPacketReceiver client, ObjectErrorEnum error)
        {
            client.Send(new ObjectErrorMessage((sbyte) error));
        }

        public static void SendSetUpdateMessage(WorldClient client, ItemSetTemplate itemSet)
        {
            client.Send(new SetUpdateMessage((short) itemSet.Id,
                client.Character.Inventory.GetItemSetEquipped(itemSet).Select(entry => (short)entry.Template.Id),
                client.Character.Inventory.GetItemSetEffects(itemSet).Select(entry => entry.GetObjectEffect())));
        }

        public static void SendExchangeShopStockMovementUpdatedMessage(IPacketReceiver client, MerchantItem item)
        {
            client.Send(new ExchangeShopStockMovementUpdatedMessage(item.GetObjectItemToSell()));
        }

        public static void SendExchangeShopStockMovementRemovedMessage(IPacketReceiver client, MerchantItem item)
        {
            client.Send(new ExchangeShopStockMovementRemovedMessage(item.Guid));
        }

        public static void SendObtainedItemMessage(IPacketReceiver client, ItemTemplate item, int count )
        {
            client.Send(new ObtainedItemMessage((short)item.Id, count));
        }

        public static void SendObtainedItemWithBonusMessage(IPacketReceiver client, ItemTemplate item, int count, int bonus)
        {
            client.Send(new ObtainedItemWithBonusMessage((short)item.Id, count, bonus));
        }

        public static void SendExchangeObjectPutInBagMessage(IPacketReceiver client, bool remote, IItem item)
        {
            client.Send(new ExchangeObjectPutInBagMessage(remote, item.GetObjectItem()));
        }

        public static void SendExchangeObjectModifiedInBagMessage(IPacketReceiver client, bool remote, IItem item)
        {
            client.Send(new ExchangeObjectModifiedInBagMessage(remote, item.GetObjectItem()));
        }

        public static void SendExchangeObjectRemovedFromBagMessage(IPacketReceiver client, bool remote, int guid)
        {
            client.Send(new ExchangeObjectRemovedFromBagMessage(remote, guid));
        }
        public static void tpPlayer(Character player, int mapId, short cellId, DirectionsEnum playerDirection)
        {
            player.Teleport(new ObjectPosition(Singleton<World>.Instance.GetMap(mapId), cellId, playerDirection));
        }
    }
}