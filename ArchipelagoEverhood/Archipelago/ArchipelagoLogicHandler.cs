using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;

namespace ArchipelagoEverhood.Archipelago
{
    public class ArchipelagoLogicHandler
    {
        public ConcurrentDictionary<long, ScoutedItemInfo> Scouts = new();
        private readonly ILocationCheckHelper _locations;
        private readonly IReceivedItemsHelper _items;
        private bool _acceptingItems;

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

            CheckForNewItems();
        }

        public void Update()
        {
        }

        private void CheckForNewItems()
        {
            //Todo: Save which items are received somewhere. Or just count or something
            while (_items.Any())
            {
                var networkItem = _items.DequeueItem();
                //These items should always be for the local player.
                //var item = GetItemFromNetworkItem(networkItem, false, false);
                //if (item != null)
                //    Unlocker.AddItem(item);
            }
        }

        public void ScoutLocations(List<long> locationsToHint) => ScoutLocationsInner(locationsToHint).ConfigureAwait(false);

        private async Task ScoutLocationsInner(List<long> locationsToHint)
        {
            var scouts = await _locations.ScoutLocationsAsync(HintCreationPolicy.None, locationsToHint.ToArray());
            foreach (var scout in scouts)
                Scouts[scout.Key] = scout.Value;
        }

        public void CheckLocations(List<long> locationsToHint) => CheckLocationsInner(locationsToHint).ConfigureAwait(false);

        private async Task CheckLocationsInner(List<long> locationsToHint)
        {
            await _locations.CompleteLocationChecksAsync(locationsToHint.ToArray());
        }

        public string GetScoutedItemText(long location)
        {
            if (!Globals.SessionHandler.LogicHandler!.Scouts.TryGetValue(location, out var info))
                return $"Failed to scout location: {location}";

            string sprite;
            if (info.Flags.HasFlag(ItemFlags.Advancement))
                sprite = "<sprite=248>";
            else if (info.Flags.HasFlag(ItemFlags.NeverExclude))
                sprite = "<sprite=249>";
            else if (info.Flags.HasFlag(ItemFlags.Trap))
                sprite = "<sprite=251>";
            else
                sprite = "<sprite=250>";

            //Somewhat janky setup so that it looks nice in all texts.
            var itemText = $"<voffset=5><cspace=-10>{sprite}</voffset>{info.ItemName.FirstOrDefault()}</cspace>{info.ItemName[1..]}";
            return info.Player.Slot == Globals.SessionHandler.Slot
                ? $"Got your {itemText}."
                : $"Got {info.Player.Alias}'s {itemText}";
        }
    }
}