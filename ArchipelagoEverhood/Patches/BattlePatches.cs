using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fungus;
using HarmonyLib;
using TMPro;

//Disable these warnings due to
// ReSharper disable InconsistentNaming 

namespace ArchipelagoEverhood.Patches
{
    [HarmonyPatch(typeof(Main_GameplayRoot), "SetBattlesRoots")]
    public static class Main_GameplayRootSetBattlesRootsPatch
    {
        private static void Prefix(Main_GameplayRoot __instance, List<GameplayBattleRoot> battleRoots)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;

            try
            {
                Globals.Logging.Msg($"New Battle Loaded. Roots: {string.Join(", ", battleRoots.Select(x => x.gameObject.scene.name))}. " +
                                    $"Is Replay: {battleRoots.First().ReplayBattle_State}.");

                Globals.EverhoodBattles.StartedBattle(battleRoots.Last());
            }
            catch (Exception e)
            {
                Globals.Logging.Error("Main_GameplayRoot", e);
            }
        }
    }

    [HarmonyPatch(typeof(Main_GameplayRoot), "GameplayEnemyDefeated")]
    public static class Main_GameplayRootGameplayEnemyDefeatedPatch
    {
        private static void Prefix(Main_GameplayRoot __instance, GameplayBattleRoot ____activeBattleRoot, List<GameplayBattleRoot> ____battlesRoot, ref int? __state)
        {
            __state = null;
            if (!Globals.SessionHandler.LoggedIn)
            {
                Globals.Logging.Msg($"GameplayEnemyDefeated: {____activeBattleRoot.GameplayEnemy.gameObject.scene.name}. " +
                                    $"Is Replay: {____activeBattleRoot.ReplayBattle_State}. Count Left: {____battlesRoot.Count}. " +
                                    $"Xp: {Globals.ServicesRoot.InfinityProjectExperience.GetXpRewardCount(____activeBattleRoot.GameplayEnemy.gameObject)}.");
                return;
            }

            try
            {
                if (____battlesRoot.Count > 1 || ____activeBattleRoot.ReplayBattle_State)
                    return;

                __state = Globals.EverhoodBattles.CompletedBattle();
            }
            catch (Exception e)
            {
                Globals.Logging.Error("Main_GameplayRoot", e);
            }
        }

        private static void Postfix(BattleVictoryResult ___battleVictoryResult, int? __state)
        {
            try
            {

                if (!Globals.SessionHandler.LoggedIn || __state == null)
                    return;

                var textFields = typeof(BattleVictoryResult).GetField("enemyDefeatedLabels", BindingFlags.Instance | BindingFlags.NonPublic);
                var texts = (TextMeshProUGUI[])textFields!.GetValue(___battleVictoryResult);

                texts[0].gameObject.SetActive(true);
                texts[0].text = Globals.SessionHandler.LogicHandler!.GetScoutedItemText(__state.Value);
                texts[1].gameObject.SetActive(false);
                texts[2].gameObject.SetActive(false);
            }
            catch (Exception e)
            {
                Globals.Logging.Error("Main_GameplayRoot", e);
            }
        }
    }

    //Todo: I think this is modded only
    [HarmonyPatch(typeof(LoadGameplayBattle), "OnEnter")]
    public static class LoadGameplayBattlePatch
    {
        private static void Prefix(SceneManagerRoot.BattleToLoadData[] ___battleScenesToLoad, bool ___isReplayBattle)
        {
            try
            {
                Globals.Logging.Msg($"Loading Battle: {___battleScenesToLoad[0].battleScene.BuildIndex}. Replay: {___isReplayBattle}");
            }
            catch (Exception e)
            {
                Globals.Logging.Error("LoadGameplayBattle", e);
            }
        }
    }
}