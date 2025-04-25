using System;
using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net.Helpers;
using ArchipelagoEverhood.Data;
using Fungus;

namespace ArchipelagoEverhood.Logic
{
    public class EverhoodBattles
    {
        private BattleData? CurrentBattle;
        private ILocationCheckHelper? _locations;
        private Dictionary<long, BattleData>? _activeBattleData;

        public void CompleteChecksLoadedFromSave(ILocationCheckHelper locations)
        {
            _locations = locations;
            _activeBattleData = new Dictionary<long, BattleData>();

            var locationsToCheck = new List<long>();
            foreach (var battle in BattleStorage.Battles)
            {
                var clone = battle.Clone();
                clone.InLogic = true; //Todo: Slot Data
                if (!_activeBattleData.TryAdd(battle.LocationId, battle.Clone()))
                {
                    Globals.Logging.Error($"Multiple battles have the id: {battle.LocationId}");
                    continue;
                }

                if (_locations.AllLocationsChecked.Contains(battle.LocationId))
                {
                    battle.Achieved = true;
                    continue;
                }

                battle.Achieved = CheckIfPasses(battle);
                if (!battle.Achieved)
                    continue;

                locationsToCheck.Add(battle.LocationId);
            }

            Globals.SessionHandler.LogicHandler!.CheckLocations(locationsToCheck);
        }

        public bool LocationChecked(long id)
        {
            if (!_activeBattleData!.TryGetValue(id, out var data))
                return false;

            data.Achieved = true;
            return true;
        }

        public void StartedBattle(GameplayBattleRoot loadedGameplayRoot)
        {
            CurrentBattle = null;
            if (loadedGameplayRoot.ReplayBattle_State)
                return;

            var battleSceneName = loadedGameplayRoot.gameObject.scene.name;
            if (BattleStorage.SkipBattles.Contains(battleSceneName))
                return;

            var relevantData = _activeBattleData!.Where(x => x.Value.SceneName == battleSceneName).Select(x => x.Value).ToList();
            if (relevantData.Count <= 0)
            {
                Globals.Logging.Error($"Failed to find battle: {battleSceneName}.");
                return;
            }

            var startedFight = relevantData.Where(battleData => !battleData.Achieved).Where(CheckIfPasses).FirstOrDefault();
            if (startedFight == null)
            {
                Globals.Logging.Warning("Battles", $"Did not find any battle for the scene {battleSceneName} that wasn't already unlocked. Potential refight that was missed?");
                return;
            }

            Globals.Logging.Warning("Battles", $"Found fight: {startedFight.LocationId} {battleSceneName}, {startedFight.VariableName}");
            CurrentBattle = startedFight;
            Globals.SessionHandler.LogicHandler!.ScoutLocations(new List<long>(startedFight.LocationId));
        }

        public int? CompletedBattle()
        {
            if (CurrentBattle == null)
            {
                Globals.Logging.Warning("Battles", "No active battle loaded. Not unlocking anything.");
                return null;
            }

            var id = CurrentBattle.LocationId;
            Globals.Logging.Warning("Battles", $"Successful fight: {id} {CurrentBattle.SceneName}, {CurrentBattle.VariableName}");
            CurrentBattle.Achieved = true;
            Globals.SessionHandler.LogicHandler!.CheckLocations(new List<long> { CurrentBattle.LocationId });
            CurrentBattle = null;
            return id;
        }

        private bool CheckIfPasses(BattleData battleData)
        {
            if (battleData.IntegerCount.HasValue)
            {
                if (!Globals.ServicesRoot!.GameData.GeneralData.intVariables.TryGetValue(battleData.VariableName, out var intValue))
                {
                    Globals.Logging.Error($"{battleData.VariableName} not successfully unlocked: Variable doesn't exist yet.");
                    return false;
                }

                if (intValue >= battleData.IntegerCount.Value)
                    return true;
                Globals.Logging.Error($"{battleData.VariableName} not successfully unlocked. {intValue}/{battleData.IntegerCount}");
                return false;
            }

            if (!Globals.ServicesRoot!.GameData.GeneralData.boolVariables.TryGetValue(battleData.VariableName, out var boolValue))
            {
                Globals.Logging.Error($"{battleData.VariableName} not successfully unlocked: Variable doesn't exist yet.");
                return false;
            }

            if (boolValue)
                return true;

            Globals.Logging.Error($"{battleData.VariableName} not successfully unlocked. {boolValue}/True");
            return false;
        }
    }
}