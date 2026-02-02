using System;
using System.IO;
using Fungus;
using HarmonyLib;
using MelonLoader;

namespace ArchipelagoEverhood.Patches
{
    [HarmonyPatch(typeof(GeneralData), nameof(GeneralData.GetFileName))]
    public static class GeneralDataFilenamePatch
    {
        public static bool Prefix(ref string __result)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return true;

            if (!Directory.Exists(Path.Combine(GameData.STORAGE_DIRECTORY, "Archipelago")))
                Directory.CreateDirectory(Path.Combine(GameData.STORAGE_DIRECTORY, "Archipelago"));

            __result = Path.Combine("Archipelago", $"{Globals.EverhoodOverrides.Settings!.Seed}_file1");
            return false;
        }
    }

    [HarmonyPatch(typeof(GameData), nameof(GameData.Load))]
    public static class GameDataLoadPatch
    {
        public static void Postfix()
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;

            Globals.SessionHandler.SaveFileLoaded();
            Globals.TopdownRoot!.Player.SetTopDownPlayerMovementState(true);
        }
    }
    
    [HarmonyPatch(typeof(GameData), "New")]
    public static class GameDataNewPatch
    {
        public static void Postfix()
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;

            Globals.SessionHandler.SaveFileLoaded();
            Globals.TopdownRoot!.Player.SetTopDownPlayerMovementState(true);
        }
    }

    [HarmonyPatch(typeof(SceneManagerRoot), nameof(SceneManagerRoot.LoadMainMenuFromBattleScene))]
    public static class SceneManagerBattleLoadPatch
    {
        public static void Prefix()
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;

            Globals.SessionHandler.QuitToMenu();
        }
    }

    [HarmonyPatch(typeof(SceneManagerRoot), nameof(SceneManagerRoot.LoadMainMenuFromTopdownScene))]
    public static class SceneManagerTopdownLoadPatch
    {
        public static void Prefix()
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;

            Globals.SessionHandler.QuitToMenu();
        }
    }

    [HarmonyPatch(typeof(SceneManagerRoot), nameof(SceneManagerRoot.LoadTopdownSceneFromBattleScene), new Type[0])]
    public static class SceneManagerLoadTopdownSceneFromBattleScenePatch
    {
        public static void Postfix()
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;

            if (Globals.ServicesRoot == null)
            {
                Globals.Logging.Warning("Saving", "Tried to save but didn't have services root cached.");
                return;
            }
            
            Globals.Logging.Warning("Saving", "Saving game after fight. Standard Func, Does this break things?");
            Globals.SaveRequested = true;
        }
    }
    
    [HarmonyPatch(typeof(SceneManagerRoot), nameof(SceneManagerRoot.TD_LoadTopdownSceneFromBattleScene))]
    public static class SceneManagerTD_LoadTopdownSceneFromBattleScenePatch
    {
        public static void Postfix()
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;

            if (Globals.ServicesRoot == null)
            {
                Globals.Logging.Warning("Saving", "Tried to save but didn't have services root cached.");
                return;
            }
            
            Globals.Logging.Warning("Saving", "Saving game after fight. TD Func, Does this break things?");
            Globals.SaveRequested = true;
        }
    }

    [HarmonyPatch(typeof(SceneManagerRoot), nameof(SceneManagerRoot.SLB_LoadTopdownSceneFromBattleScene))]
    public static class SceneManagerSLB_LoadTopdownSceneFromBattleScenePatch
    {
        public static void Postfix()
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;

            if (Globals.ServicesRoot == null)
            {
                Globals.Logging.Warning("Saving", "Tried to save but didn't have services root cached.");
                return;
            }
            
            Globals.Logging.Warning("Saving", "Saving game after fight. Sld Func, Does this break things?");
            Globals.SaveRequested = true;
        }
    }
}