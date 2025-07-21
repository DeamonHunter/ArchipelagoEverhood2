using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Archipelago.MultiClient.Net.Models;
using ArchipelagoEverhood.Data;
using ArchipelagoEverhood.Patches;
using Fungus;
using TMPro;
using UnityEngine;

namespace ArchipelagoEverhood.Archipelago
{
    public class ArchipelagoItemHandler
    {
        private HashSet<ScoutedItemInfo> _blockedItems = new();
        private ConcurrentQueue<ItemUnlock> _itemsToAdd = new();
        private readonly int _currentSlot;
        private readonly int _currentTeam;

        private Queue<ItemUnlock> _queuedSays = new();

        private struct ItemUnlock
        {
            public long ItemId;
            public string DisplayName;
            public string? PlayerName;
            public int PlayerSlot;
            public int PlayerTeam;
            public bool Remote;

            public ItemUnlock(global::Archipelago.MultiClient.Net.Models.ItemInfo info, bool remote)
            {
                ItemId = info.ItemId;
                DisplayName = info.ItemDisplayName;
                PlayerName = remote ? info.Player.Alias : null;
                PlayerSlot = info.Player.Slot;
                PlayerTeam = info.Player.Team;
                Remote = remote;
            }

            public ItemUnlock(long itemId, string itemName, int playerSlot, int playerTeam)
            {
                ItemId = itemId;
                DisplayName = itemName;
                PlayerName = null;
                PlayerSlot = playerSlot;
                PlayerTeam = playerTeam;
                Remote = false;
            }
        }

        public ArchipelagoItemHandler(int currentSlot, int team)
        {
            _currentSlot = currentSlot;
            _currentTeam = team;

            if (!Globals.GameplayRoot)
            {
                Globals.Logging.Error("ItemHandler", "Gameplay root not loaded yet?");
                return;
            }
        }

        public void HandleScoutedItem(ScoutedItemInfo info)
        {
            if (!_blockedItems.Add(info))
            {
                Globals.Logging.Error("ItemHandler", $"Scouted Item has been handled twice? {info.LocationId}:{info.ItemId}");
                return;
            }

            Globals.Logging.Log("ItemHandler", $"Received scouted item: {info.ItemId}");
            if (info.Player.Slot == _currentSlot && info.Player.Team == _currentTeam)
                _itemsToAdd.Enqueue(new ItemUnlock(info, false));
        }

        public void HandleRemoteItem(global::Archipelago.MultiClient.Net.Models.ItemInfo info)
        {
            //Todo: Does this always work?
            if (_blockedItems.Any(x => x.ItemId == info.ItemId && x.LocationId == info.LocationId && x.Player.Slot == info.Player.Slot))
                return;

            Globals.Logging.Log("ItemHandler", $"Received remote item: {info.ItemId}");
            _itemsToAdd.Enqueue(new ItemUnlock(info, true));
        }

        public void HandleOfflineItem(string chestDataItemName)
        {
            if (!ItemData.ItemIdsByName.TryGetValue(chestDataItemName, out var id))
            {
                Globals.Logging.Error("Item Handler", $"Tried to unlock a standard item without an id: {id}");
                return;
            }

            Globals.Logging.Log("ItemHandler", $"Received offline item: {id}");
            _itemsToAdd.Enqueue(new ItemUnlock(id, chestDataItemName, _currentSlot, _currentTeam));
        }

        public void Update()
        {
            //Globals.Logging.Log("ItemHandler", $"Update: {_inBattle}, {SceneManagerRoot.sceneIsLoading}, {Globals.TopdownRoot!.Player.MovementState} {_itemsToAdd.Count}");
            //Don't reward items while loading.
            if (SceneManagerRoot.sceneIsLoading)
                return;

            //Don't reward items in battle unless we are on the victory screen.
            if (Globals.GameplayRoot!.IsRootAvailable() && !(Globals.BattleVictoryResult!.Executing && Globals.VictoryScreenCanvas!.gameObject.activeSelf))
                return;

            //if (Globals.CurrentTopdownLevel == 6)
            //    return;

            //Don't reward items if the player can't move (likely cutscene or dialogue)
            if (!Globals.TopdownRoot!.Player.MovementState)
            {
                if (!_itemsToAdd.TryPeek(out var nextItem))
                    return;

                //Make a special exception for xp items in this case as it doesn't block dialogue.
                if (!ItemData.XpsById.ContainsKey(nextItem.ItemId))
                    return;
            }

            while (_itemsToAdd.TryDequeue(out var item))
                UnlockItem(item);

            if (Globals.GameplayRoot.IsRootAvailable())
                return;

            if (SayDialog.ActiveSayDialog)
            {
                var canvas = SayDialog.ActiveSayDialog.GetComponent<Canvas>();
                if (SayDialog.ActiveSayDialog.gameObject.activeInHierarchy && canvas && canvas.enabled)
                    return;

                if (SayOnEnterPatch.OverridenDialogue)
                {
                    SayDialog.ActiveSayDialog.ClearContinue();
                    SayOnEnterPatch.OverridenDialogue = false;
                }
            }

            if (!_queuedSays.TryDequeue(out var sayItem))
                return;

            Globals.Logging.LogDebug("ItemHandler", $"Unqueued say: {sayItem.DisplayName}. Still have {_queuedSays.Count} left.");

            if (sayItem.PlayerSlot != _currentSlot && sayItem.PlayerTeam != _currentTeam)
                SayOnEnterPatch.ForceShowDialogue($"Received {sayItem.DisplayName} from {sayItem.PlayerName}", null);
            else
                SayOnEnterPatch.ForceShowDialogue($"You found your {sayItem.DisplayName}!", null);
        }

        public void ForceRewardItems()
        {
            while (_itemsToAdd.TryDequeue(out var item))
                UnlockItem(item);
        }

        private void UnlockItem(ItemUnlock itemUnlock)
        {
            Globals.Logging.LogDebug("ItemHandler", $"Looking to unlock: {itemUnlock.DisplayName}");
            if (!itemUnlock.Remote && (_currentSlot != itemUnlock.PlayerSlot || _currentTeam != itemUnlock.PlayerTeam))
            {
                Globals.Logging.Error("ItemHandler", $"Tried to Unlock someone else's item. {itemUnlock.DisplayName} : {itemUnlock.ItemId} : {itemUnlock.PlayerSlot}");
                return;
            }

            var data = Globals.ServicesRoot!.GameData.GeneralData;
            //Todo: Item Sprites?
            if (ItemData.ItemsById.TryGetValue(itemUnlock.ItemId, out var item))
            {
                switch (item.Item)
                {
                    case Item.RoomKey23:
                    case Item.RoomKeyGold:
                    case Item.RoomKeyGreen:
                    case Item.RoomKeyPinecone:
                    case Item.RoomKeyOmega:
                        //These items break if you get more than 1.
                        if (data.collectedItems.TryGetValue(item.Item.ToString(), out var count) && count > 0)
                            return;
                        break;
                }


                data.AddCollectedItem(item.Item.ToString(), 1);
                if (itemUnlock.Remote)
                    _queuedSays.Enqueue(itemUnlock);
            }
            else if (ItemData.WeaponsById.TryGetValue(itemUnlock.ItemId, out var weapon))
            {
                data.AddWeapon(weapon.Weapon);
                if (itemUnlock.Remote)
                    _queuedSays.Enqueue(itemUnlock);
            }
            else if (ItemData.ArtifactsById.TryGetValue(itemUnlock.ItemId, out var artifact))
            {
                data.AddArtifactItem(artifact.Artifact.ToString(), 1);
                if (itemUnlock.Remote)
                    _queuedSays.Enqueue(itemUnlock);
            }
            else if (ItemData.DoorKeysById.TryGetValue(itemUnlock.ItemId, out var doorKey))
            {
                Globals.EverhoodDoors.OnReceiveDoorKey(doorKey.DoorId);
                if (itemUnlock.Remote)
                    _queuedSays.Enqueue(itemUnlock);
            }
            else if (ItemData.ColorsById.TryGetValue(itemUnlock.ItemId, out var color))
            {
                Globals.EverhoodOverrides.ReceivedColor(color);
                if (itemUnlock.Remote)
                    _queuedSays.Enqueue(itemUnlock);
            }
            else if (ItemData.XpsById.TryGetValue(itemUnlock.ItemId, out var xp))
            {
                if (Globals.GameplayRoot!.IsRootAvailable() && Globals.BattleVictoryResult!.Executing && Globals.VictoryScreenCanvas!.gameObject.activeSelf)
                {
                    var xpLevelPlayer = (GeneralData.XpLevelInfo)(typeof(BattleVictoryResult).GetMethod("AddPlayerXp", BindingFlags.Instance | BindingFlags.NonPublic)
                        !.Invoke(Globals.BattleVictoryResult, new object[] { Globals.ServicesRoot!, Globals.ServicesRoot!.GameData.GeneralData.xpLevel_player, xp.Xp }));
                    var text = (TextMeshProUGUI)(typeof(BattleVictoryResult).GetField("playerXpLeftLabel", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(Globals.BattleVictoryResult));
                    text.text = (xpLevelPlayer.xpRequiredForNextLevel - xpLevelPlayer.lastXp).ToString();
                    Globals.ServicesRoot.GameData.GeneralData.playerJustLeveledUp = xpLevelPlayer.leveledUp;
                    FindAndActivateBattleVictoryXpAnimation(xpLevelPlayer);
                }
                else
                {
                    var xpLevel = data.xpLevel_player;
                    var playerLevelXpRequired = Globals.ServicesRoot!.InfinityProjectExperience.GetPlayerLevelXpRequired(xpLevel.level);
                    Globals.TopdownRoot!.PlayerXpGainUI.GainXP(xpLevel.AddXp(xp.Xp, playerLevelXpRequired));
                }
            }
            else if (ItemData.CosmeticsById.TryGetValue(itemUnlock.ItemId, out var cosmetic))
            {
                data.AddCosmectic(cosmetic.Cosmetic);
                if (itemUnlock.Remote)
                    _queuedSays.Enqueue(itemUnlock);
            }
        }

        public void RemoveItem(global::Archipelago.MultiClient.Net.Models.ItemInfo itemInfo)
        {
            Globals.Logging.LogDebug("ItemHandler", $"Looking to remove: {itemInfo.ItemDisplayName}");
            if (_currentSlot != itemInfo.Player.Slot || _currentTeam != itemInfo.Player.Team)
            {
                Globals.Logging.Error("ItemHandler", $"Tried to Unlock someone else's item. {itemInfo.ItemDisplayName} : {itemInfo.ItemId} : {itemInfo.Player.Slot}");
                return;
            }

            var data = Globals.ServicesRoot!.GameData.GeneralData;
            //Todo: Item Sprites?
            if (ItemData.ItemsById.TryGetValue(itemInfo.ItemId, out var item))
            {
                data.collectedItems ??= new Dictionary<string, int>();
                if (!data.collectedItems.ContainsKey(item.Item.ToString()))
                    return;
                data.collectedItems[item.Item.ToString()] -= 1;
            }
            else if (ItemData.WeaponsById.TryGetValue(itemInfo.ItemId, out var weapon))
            {
                data.RemoveWeapon(weapon.Weapon);
            }
            else if (ItemData.ArtifactsById.TryGetValue(itemInfo.ItemId, out var artifact))
            {
                data.UseArtifactItem(artifact.Artifact.ToString(), 1);
            }
            else if (ItemData.DoorKeysById.TryGetValue(itemInfo.ItemId, out var doorKey))
            {
                Globals.Logging.Error("ItemHandler", $"Tried to remove a doorkey {doorKey}? These shouldn't be erroring.");
            }
            else if (ItemData.XpsById.TryGetValue(itemInfo.ItemId, out var xp))
            {
                var xpLevel = data.xpLevel_player;
                var playerLevelXpRequired = Globals.ServicesRoot!.InfinityProjectExperience.GetPlayerLevelXpRequired(xpLevel.level);
                xpLevel.AddXp(-xp.Xp, playerLevelXpRequired);
            }
            else if (ItemData.CosmeticsById.TryGetValue(itemInfo.ItemId, out var cosmetic))
            {
                data.collectedCosmetics.Remove(cosmetic.Cosmetic);
            }
        }

        private void FindAndActivateBattleVictoryXpAnimation(GeneralData.XpLevelInfo xpLevelPlayer)
        {
            bool foundMethod = false;
            //The animation function is a local function of Victory. Which means getting it by name is hard and even more prone to failure
            foreach (var methodSet in typeof(BattleVictoryResult).GetNestedTypes(BindingFlags.NonPublic)
                         .Where(x => x.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
                         .Select(x => (x, x.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))))
            {
                foreach (var method in methodSet.Item2)
                {
                    if (!method.Name.Contains("<Victory>g__DoPlayerLevelSliderUpdate"))
                        continue;

                    var nestedClass = Activator.CreateInstance(methodSet.x);
                    foreach (var property in methodSet.x.GetFields(BindingFlags.Instance | BindingFlags.Public))
                    {
                        if (property.Name.Contains("this"))
                            property.SetValue(nestedClass, Globals.BattleVictoryResult);
                        else if (property.Name.Contains("servicesRoot"))
                            property.SetValue(nestedClass, Globals.ServicesRoot);
                        else if (property.Name.Contains("xpLevel_player"))
                            property.SetValue(nestedClass, xpLevelPlayer);
                        else if (property.Name.Contains("lastDefeatedBattleRoot"))
                            property.SetValue(nestedClass, null); //Todo: Hopefully this doesn't cause issues.
                        else
                            Globals.Logging.Error("ItemHandler", $"Unknown property in Level Up Animation {property.Name}");
                    }

                    var coRoutine = (IEnumerator)method!.Invoke(nestedClass, new object[] { xpLevelPlayer });
                    Globals.BattleVictoryResult!.StartCoroutine(coRoutine);
                    foundMethod = true;
                    break;
                }

                if (foundMethod)
                    break;
            }

            if (!foundMethod)
                throw new Exception("Failed to find the right method");
        }
    }
}