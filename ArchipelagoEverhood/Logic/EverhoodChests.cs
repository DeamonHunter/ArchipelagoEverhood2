using System.Collections.Generic;
using Archipelago.MultiClient.Net.Helpers;
using ArchipelagoEverhood.Archipelago;
using ArchipelagoEverhood.Data;
using ArchipelagoEverhood.Util;
using Fungus;

namespace ArchipelagoEverhood.Logic
{
    public class EverhoodChests
    {
        private ILocationCheckHelper? _locations;
        private Dictionary<long, ChestData>? _activeChestData;

        public void CompleteChecksLoadedFromSave(ILocationCheckHelper locations)
        {
            _locations = locations;
            _activeChestData = new Dictionary<long, ChestData>();

            var locationsToCheck = new List<long>();
            foreach (var chest in ChestStorage.Chests)
            {
                var clone = chest.Clone();
                clone.InLogic = locations.AllLocations.Contains(clone.LocationId);
                //Globals.Logging.Log("InLogic", $"{clone.LocationId} : {clone.InLogic}");
                if (!_activeChestData.TryAdd(clone.LocationId, clone))
                {
                    Globals.Logging.Error($"Multiple chests have the id: {clone.LocationId}");
                    continue;
                }

                if (_locations.AllLocationsChecked.Contains(clone.LocationId))
                {
                    clone.Achieved = true;
                    SetVariable(clone);
                    continue;
                }

                clone.Achieved = CheckIfPasses(clone);
                if (!clone.Achieved)
                    continue;

                locationsToCheck.Add(clone.LocationId);
            }

            Globals.SessionHandler.LogicHandler!.CheckLocations(locationsToCheck);
        }

        public void ScoutForScene(string sceneName)
        {
            Globals.Logging.Log("EverhoodChests", $"Scout for Scene: {sceneName}");
            var locationsToScout = new List<long>();
            foreach (var chest in ChestStorage.Chests)
            {
                if (chest.SceneName != sceneName)
                    continue;
                locationsToScout.Add(chest.LocationId);
            }

            if (locationsToScout.Count > 0)
                Globals.SessionHandler.LogicHandler!.ScoutLocations(locationsToScout);
        }

        public ChestData? ChestOpened(Cosmetics cosmetic)
        {
            if (_activeChestData == null)
                return null;

            var cosmeticStr = cosmetic.ToString();

            foreach (var chestData in _activeChestData.Values)
            {
                if (chestData.Type != ChestType.Cosmetic)
                    continue;

                if (chestData.ItemName != cosmeticStr)
                    continue;

                if (chestData.Achieved)
                {
                    Globals.Logging.Log("Chests", "Chest Already Opened");
                    return chestData;
                }

                CheckLocation(chestData);
                return chestData;
            }

            Globals.Logging.Warning("Chests", $"Did not find any chest for the cosmetic {cosmeticStr}. Missing?");
            return null;
        }

        public ChestData? ChestOpened(Dictionary<Item, int> items)
        {
            if (_activeChestData == null)
                return null;

            foreach (var item in items.Keys)
            {
                var itemStr = item.ToString();

                foreach (var chestData in _activeChestData.Values)
                {
                    if (chestData.Type != ChestType.Item)
                        continue;

                    if (chestData.ItemName != itemStr)
                        continue;

                    if (chestData.Achieved)
                        continue;

                    if (!CheckIfPasses(chestData))
                        continue;

                    CheckLocation(chestData);
                    return chestData;
                }
            }

            Globals.Logging.Warning("Chests", $"Did not find any chest for the item {string.Join(", ", items.Keys)}. Missing?");
            return null;
        }

        public ChestData? ChestOpened(Artifact[] artifacts)
        {
            if (_activeChestData == null)
                return null;

            foreach (var artifact in artifacts)
            {
                var itemStr = artifact.ToString();

                foreach (var chestData in _activeChestData.Values)
                {
                    if (chestData.Type != ChestType.Item)
                        continue;

                    if (chestData.ItemName != itemStr)
                        continue;

                    if (chestData.Achieved)
                        continue;

                    if (!CheckIfPasses(chestData))
                        continue;

                    CheckLocation(chestData);
                    return chestData;
                }
            }

            Globals.Logging.Warning("Chests", $"Did not find any chest for the artifacts {string.Join(", ", artifacts)}. Missing?");
            return null;
        }

        public ChestData? ChestOpened(Weapon weapon)
        {
            if (_activeChestData == null)
                return null;

            var weaponStr = weapon.ToString();
            foreach (var chestData in _activeChestData.Values)
            {
                if (chestData.Type != ChestType.Item)
                    continue;

                if (chestData.ItemName != weaponStr)
                    continue;

                if (chestData.Achieved)
                    continue;

                if (!CheckIfPasses(chestData))
                    continue;

                CheckLocation(chestData);
                return chestData;
            }

            Globals.Logging.Warning("Chests", $"Did not find any chest for the weapon {weaponStr}. Missing?");
            return null;
        }

        public ChestData? ChestOpened(int xp)
        {
            if (_activeChestData == null)
                return null;

            var xpStr = $"{xp}xp";
            foreach (var chestData in _activeChestData.Values)
            {
                if (chestData.Type != ChestType.XP)
                    continue;

                if (chestData.ItemName != xpStr)
                    continue;

                if (chestData.Achieved)
                    continue;

                if (!CheckIfPasses(chestData))
                    continue;

                CheckLocation(chestData);
                return chestData;
            }

            Globals.Logging.Warning("Chests", $"Did not find any chest for the xp {xpStr}. Missing?");
            return null;
        }

        private bool CheckIfPasses(ChestData chestData)
        {
            if (chestData.VariableName == null)
                return false;

            if (!Globals.ServicesRoot!.GameData.GeneralData.boolVariables.TryGetValue(chestData.VariableName, out var boolValue))
            {
                Globals.Logging.Error($"{chestData.VariableName} not successfully unlocked: Variable doesn't exist yet.");
                return false;
            }

            if (boolValue)
            {
                Globals.Logging.Error($"{chestData.VariableName} successfully unlocked. {boolValue}/True");
                return true;
            }

            Globals.Logging.Error($"{chestData.VariableName} not successfully unlocked. {boolValue}/True");
            return false;
        }

        private void SetVariable(ChestData battleData)
        {
            /*
            if (battleData.IntegerCount.HasValue)
            {
                int value = battleData.IntegerCount.Value;
                if (Globals.ServicesRoot!.GameData.GeneralData.intVariables.TryGetValue(battleData.VariableName, out var intValue))
                    value = Math.Max(battleData.IntegerCount.Value, intValue);

                Globals.ServicesRoot!.GameData.GeneralData.intVariables[battleData.VariableName] = value;

                var intVar = FungusManager.Instance.GlobalVariables.GetVariable(battleData.VariableName) as IntegerVariable;
                if (intVar != null)
                    intVar.Value = value;
                return;
            }
            */

            Globals.ServicesRoot!.GameData.GeneralData.boolVariables[battleData.VariableName] = true;
            var boolVar = FungusManager.Instance.GlobalVariables.GetVariable(battleData.VariableName) as BooleanVariable;
            if (boolVar != null)
                boolVar.Value = true;
        }

        private void CheckLocation(ChestData chestData)
        {
            chestData.Achieved = true;
            if (chestData.InLogic)
                Globals.SessionHandler.LogicHandler!.CheckLocations(new List<long> { chestData.LocationId });
            else if (chestData.ItemName != null)
                Globals.SessionHandler.ItemHandler!.HandleOfflineItem(chestData.ItemName);
        }

        public string GetItemName(ChestData chestData)
        {
            if (chestData.InLogic)
                return Globals.SessionHandler.LogicHandler!.GetScoutedItemText(chestData.LocationId, true);

            string itemName;
            if (ItemData.ItemIdsByName.TryGetValue(chestData.ItemName, out var id) && ItemData.AllItemsByID.TryGetValue(id, out var itemInfo))
                return EverhoodHelpers.ConstructCollectedItemText(itemInfo.ItemName, itemInfo.ItemFlags, null, true);

            itemName = AssetHelpers.NicifyName(chestData.ItemName);
            return $"You found your <voffset=5><cspace=-10><sprite=250></voffset>{itemName[0]}</cspace>{(itemName.Length > 1 ? itemName[1..] : "")}";
        }
    }
}