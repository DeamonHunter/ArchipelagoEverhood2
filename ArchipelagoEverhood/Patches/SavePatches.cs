using System.IO;
using HarmonyLib;

namespace ArchipelagoEverhood.Patches
{
    [HarmonyPatch(typeof(GeneralData), nameof(GeneralData.GetFileName))]
    public static class GeneralDataFilenamePatch
    {
        public static bool Prefix(ref string result)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return false;

            result = Path.Combine("Archipelago", $"{Globals.EverhoodOverrides.Seed}_file1");
            return true;
        }
    }

    [HarmonyPatch(typeof(GeneralData), nameof(GeneralData.Load))]
    public static class GeneralDataLoadPatch
    {
        public static void Postfix()
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;

            Globals.SessionHandler.SaveFileLoaded();
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
}