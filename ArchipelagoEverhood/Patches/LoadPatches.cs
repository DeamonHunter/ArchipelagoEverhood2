
using ArchipelagoEverhood.Util;
using HarmonyLib;
using UnityEngine;

namespace ArchipelagoEverhood.Patches
{
    [HarmonyPatch(typeof(SceneManagerRoot), nameof(SceneManagerRoot.LoadTopdownScene))]
    public static class LoadTopdownScenePatch
    {
        public static void Prefix(ref int buildIndexToLoad, ref Vector3 spawnPosition)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;

            //Home Town doesn't work quite correct.
            if (buildIndexToLoad == 38)
            {
                Globals.Logging.LogDebug("SceneManagerRoot", $"Setting Back Scene to: {buildIndexToLoad}");
                if (!EverhoodHelpers.HasFlag("GL_HomeTownVisit"))
                {
                    spawnPosition = new Vector3(-21.3f, -58.5f, 0);
                    buildIndexToLoad = 39;
                }
            }
            
            Globals.CurrentTopdownLevel = buildIndexToLoad;
            Globals.Logging.LogDebug("SceneManagerRoot", $"Setting Back Scene to: {buildIndexToLoad}");
        }
    }

    [HarmonyPatch(typeof(SceneManagerRoot), nameof(SceneManagerRoot.LoadFromMainMenu))]
    public static class LoadFromMainMenuPatch
    {
        public static void Prefix(ref int buildIndex)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;

            //Todo: Fancy load in. Flowchart editing is finicky and it doesn't play the ending animation properly if I jump to the end...
            if (buildIndex == 6)
            {
                buildIndex = 7;
                Globals.EverhoodOverrides.SetQuestionnaire();
            }

            Globals.CurrentTopdownLevel = buildIndex;
            Globals.Logging.LogDebug("SceneManagerRoot", $"Setting Back Scene to: {buildIndex}");
        }
    }

    [HarmonyPatch(typeof(SceneManagerRoot), nameof(SceneManagerRoot.LoadSceneSavedFromMainMenu))]
    public static class LoadSceneSavedFromMainMenuPatch
    {
        public static void Prefix()
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;

            Globals.CurrentTopdownLevel = -1;
            Globals.Logging.LogDebug("SceneManagerRoot", $"Setting Back Scene to: -1");
        }
    }
}