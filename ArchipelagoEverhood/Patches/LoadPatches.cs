using HarmonyLib;

namespace ArchipelagoEverhood.Patches
{
    [HarmonyPatch(typeof(SceneManagerRoot), nameof(SceneManagerRoot.LoadTopdownScene))]
    public static class LoadTopdownScenePatch
    {
        public static void Prefix(int buildIndexToLoad)
        {
            Globals.CurrentTopdownLevel = buildIndexToLoad;
        }
    }

    [HarmonyPatch(typeof(SceneManagerRoot), nameof(SceneManagerRoot.LoadFromMainMenu))]
    public static class LoadFromMainMenuPatch
    {
        public static void Prefix(int buildIndex)
        {
            Globals.CurrentTopdownLevel = buildIndex;
        }
    }
}