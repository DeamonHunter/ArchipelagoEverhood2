using System;
using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net.Helpers;
using ArchipelagoEverhood.Archipelago;
using ArchipelagoEverhood.Data;
using ArchipelagoEverhood.Patches;

namespace ArchipelagoEverhood.Logic
{
    public class EverhoodBattles
    {
        private BattleData? CurrentBattle;
        private bool InVictoryBattle;
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
                clone.InLogic = locations.AllLocations.Contains(clone.LocationId);
                if (!_activeBattleData.TryAdd(clone.LocationId, clone))
                {
                    Globals.Logging.Error("EverhoodBattles", $"Multiple battles have the id: {clone.LocationId}");
                    continue;
                }

                if (_locations.AllLocationsChecked.Contains(clone.LocationId))
                {
                    Globals.Logging.Log("EverhoodBattles", $"Achieved: {clone.LocationId}");
                    clone.Achieved = true;
                    continue;
                }

                clone.Achieved = CheckIfPasses(clone);
                if (!clone.Achieved)
                    continue;

                locationsToCheck.Add(clone.LocationId);
            }

            if (locationsToCheck.Count > 0)
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
            try
            {
                CurrentBattle = null;
                InVictoryBattle = false;
                if (loadedGameplayRoot.ReplayBattle_State)
                    return;

                var battleSceneName = loadedGameplayRoot.gameObject.scene.name;
                if (BattleStorage.SkipBattles.Contains(battleSceneName))
                    return;

                switch (Globals.EverhoodOverrides.Settings!.Goal)
                {
                    default:
                    case EverhoodGoal.Dragon:
                        if (battleSceneName == "DragonPart2-Battle")
                        {
                            Globals.Logging.Warning("EverhoodBattles", $"Starting Victory fight!");
                            InVictoryBattle = true;
                            return;
                        }
                        break;
                    case EverhoodGoal.JudgeCreation:
                        if (battleSceneName == "JudgeCreation-Battle")
                        {
                            Globals.Logging.Warning("EverhoodBattles", $"Starting Victory fight!");
                            InVictoryBattle = true;
                            return;
                        }
                        break;
                    // new BattleData(_battleStartId + X, "ShadePostCredits-Battle", "GL_PostCredits_ShadeBattleDefeated", 0),
                    // CatGodHairball-Battle //777xp
                }

                var relevantData = _activeBattleData!.Where(x => x.Value.SceneName == battleSceneName).Select(x => x.Value).ToList();
                if (relevantData.Count <= 0)
                {
                    Globals.Logging.Error("EverhoodBattles", $"Failed to find battle: {battleSceneName}.");
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
                Globals.SessionHandler.LogicHandler!.ScoutLocations(new List<long> { startedFight.LocationId });
            }
            catch (Exception e)
            {
                Globals.Logging.Error("BattleStart", e);
            }
        }

        public BattleData? CompletedBattle()
        {
            if (InVictoryBattle)
            {
                Globals.SessionHandler.SendCompletion();
                SayOnEnterPatch.SetOverrideText("{rcolor}Congrats you've won!\nThanks for Testing!");
                return null;
            }

            if (CurrentBattle == null)
            {
                Globals.Logging.Warning("Battles", "No active battle loaded. Not unlocking anything.");
                return null;
            }

            Globals.Logging.Warning("Battles", $"Successful fight: {CurrentBattle.LocationId} {CurrentBattle.SceneName}, {CurrentBattle.VariableName}");
            if (CurrentBattle.Achieved)
                return CurrentBattle;

            CurrentBattle.Achieved = true;
            if (CurrentBattle.InLogic)
                Globals.SessionHandler.LogicHandler!.CheckLocations(new List<long> { CurrentBattle.LocationId });
            else
                Globals.SessionHandler.ItemHandler!.HandleOfflineItem(CurrentBattle.DefaultXp + "xp");
            return CurrentBattle;
        }

        private bool CheckIfPasses(BattleData battleData)
        {
            if (battleData.IntegerCount.HasValue)
            {
                if (!Globals.ServicesRoot!.GameData.GeneralData.intVariables.TryGetValue(battleData.VariableName, out var intValue))
                {
                    //Globals.Logging.LogDebug("EverhoodBattles", $"{battleData.VariableName} not successfully unlocked: Variable doesn't exist yet.");
                    return false;
                }

                if (intValue >= battleData.IntegerCount.Value)
                {
                    //Globals.Logging.LogDebug("EverhoodBattles",$"{battleData.VariableName} successfully unlocked. {intValue}/{battleData.IntegerCount}");
                    return true;
                }

                //Globals.Logging.LogDebug("EverhoodBattles", $"{battleData.VariableName} not successfully unlocked. {intValue}/{battleData.IntegerCount}");
                return false;
            }

            if (!Globals.ServicesRoot!.GameData.GeneralData.boolVariables.TryGetValue(battleData.VariableName, out var boolValue))
            {
                //Globals.Logging.LogDebug("EverhoodBattles", $"{battleData.VariableName} not successfully unlocked: Variable doesn't exist yet.");
                return false;
            }

            if (boolValue)
            {
                //Globals.Logging.LogDebug("EverhoodBattles", $"{battleData.VariableName} successfully unlocked. {boolValue}/True");
                return true;
            }

            //Globals.Logging.LogDebug("EverhoodBattles", $"{battleData.VariableName} not successfully unlocked. {boolValue}/True");
            return false;
        }
    }
}