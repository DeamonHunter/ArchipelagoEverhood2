using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using ArchipelagoEverhood.Data;
using ArchipelagoEverhood.Util;

namespace ArchipelagoEverhood.Archipelago
{
    public class ArchipelagoLogicHandler
    {
        public ConcurrentDictionary<long, ScoutedItemInfo> Scouts = new();
        private readonly ILocationCheckHelper _locations;
        private readonly IReceivedItemsHelper _items;
        private bool _acceptingItems;
        private int _itemIndex;

        public ArchipelagoLogicHandler(ArchipelagoSession session)
        {
            _locations = session.Locations;
            _items = session.Items;
        }

        public void SetAcceptingItems(bool acceptingItems)
        {
            _acceptingItems = acceptingItems;
            if (!acceptingItems)
                return;

            //TODO: CHECK FOR ITEM ACTUALLY EXISTS. ALSO HANDLE DESYNC ERROR
            var originalCount = _itemIndex;
            _itemIndex = _items.AllItemsReceived.Count;
            for (var i = 0; i < _itemIndex; i++)
            {
                var item = i < originalCount ? _items.AllItemsReceived[i] : _items.DequeueItem();
                if (item == null)
                {
                    Globals.Logging.Error("LogicHandler", $"Failed to find item at index: {i}");
                    continue;
                }

                if (ItemData.DoorKeysById.TryGetValue(item.ItemId, out var doorKey))
                {
                    Globals.EverhoodDoors.OnReceiveDoorKey(doorKey.DoorId);
                    MarkItemAdded(item.ItemId, i);
                    continue;
                }

                if (Globals.ServicesRoot!.GameData.GeneralData.intVariables.TryGetValue($"Archipelago_{i}", out var itemId))
                {
                    if (itemId == item.ItemId)
                        continue;
                    Globals.Logging.Error("LogicHandler", $"ITEM DESYNC AT INDEX {i}. Was: {itemId} Is: {item.ItemId} CURRENTLY CANNOT HANDLE!!!!!");
                    Globals.SessionHandler.ItemHandler!.RemoveItem(item);
                    Globals.SessionHandler.ItemHandler!.HandleRemoteItem(item);
                    MarkItemAdded(item.ItemId, i);
                }
                else
                {
                    Globals.SessionHandler.ItemHandler!.HandleRemoteItem(item);
                    MarkItemAdded(item.ItemId, i);
                }
            }
        }

        public void Update()
        {
            if (!_acceptingItems)
                return;

            CheckForNewItems();
        }

        private void CheckForNewItems()
        {
            //Todo: Save which items are received somewhere. Or just count or something
            while (_items.Any())
            {
                var networkItem = _items.DequeueItem();
                Globals.SessionHandler.ItemHandler!.HandleRemoteItem(networkItem);
                MarkItemAdded(networkItem.ItemId, _itemIndex);
                _itemIndex++;
            }
        }

        private void MarkItemAdded(long itemId, int itemIndex)
        {
            Globals.ServicesRoot!.GameData.GeneralData.intVariables[$"Archipelago_{itemIndex}"] = (int)itemId;
        }

        public void ScoutLocations(List<long> locationsToHint)
        {
            Globals.Logging.Log("ScoutLocations", $"Calling for location scouts {string.Join(", ", locationsToHint)}");
            ScoutLocationsInner(locationsToHint).ConfigureAwait(false);
        }

        private async Task ScoutLocationsInner(List<long> locationsToHint)
        {
            var scouts = await _locations.ScoutLocationsAsync(HintCreationPolicy.None, locationsToHint.ToArray());
            foreach (var scout in scouts)
                Scouts[scout.Key] = scout.Value;
        }

        public void CheckLocations(List<long> locationsToHint)
        {
            Globals.Logging.Log("CheckLocations", $"Calling for location checks {string.Join(", ", locationsToHint)}");
            CheckLocationsInner(locationsToHint).ConfigureAwait(false);
        }

        private async Task CheckLocationsInner(List<long> locationsToHint)
        {
            foreach (var location in locationsToHint)
            {
                if (!Scouts.TryGetValue(location, out var info))
                    continue;

                Globals.SessionHandler.ItemHandler!.HandleScoutedItem(info);
            }

            await _locations.CompleteLocationChecksAsync(locationsToHint.ToArray());
        }

        public string GetScoutedItemText(long location, bool inVictory)
        {
            if (!Scouts.TryGetValue(location, out var info))
                return $"Failed to scout location: {location}";

            return EverhoodHelpers.ConstructCollectedItemText(info.ItemName, info.Flags, info.Player.Slot != Globals.SessionHandler.Slot ? info.Player.Alias : null, inVictory);
        }

        public bool IsScouted(long location) => Scouts.ContainsKey(location);
    }
}