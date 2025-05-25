using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArchipelagoEverhood.Util;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ArchipelagoEverhood.Archipelago
{
    public class EverhoodOverrides
    {
        public bool Overriding;
        public string Seed { get; private set; }

        public Dictionary<string, int> OriginalXpLevels = new();

        public void ArchipelagoConnected(string seed)
        {
            if (Overriding)
            {
                Debug.LogError("We are somehow overriding twice? Did we log in again?");
                return;
            }

            Overriding = true;
            Seed = seed;
            Globals.ServicesRoot = GameObject.FindObjectsByType<ServicesRoot>(FindObjectsInactive.Include, FindObjectsSortMode.None).First();

            var infProjExperience = typeof(InfinityProjectExperience);
            var xpRewardInfo = infProjExperience.GetField("xpRewardInfo", BindingFlags.Instance | BindingFlags.NonPublic);
            OriginalXpLevels = (Dictionary<string, int>)xpRewardInfo.GetValue(Globals.ServicesRoot.InfinityProjectExperience);

            var overrideDict = new Dictionary<string, int>();
            foreach (var pair in OriginalXpLevels)
                overrideDict[pair.Key] = 0;

            xpRewardInfo.SetValue(Globals.ServicesRoot.InfinityProjectExperience, overrideDict);

            AddExitToHubButton();
        }

        public void ArchipelagoDisconnected()
        {
            if (Overriding)
            {
                Debug.LogError("We are somehow overriding twice? Did we log in again?");
                return;
            }

            var infProjExperience = typeof(InfinityProjectExperience);
            var xpRewardInfo = infProjExperience.GetField("xpRewardInfo", BindingFlags.Instance | BindingFlags.NonPublic);
            OriginalXpLevels = (Dictionary<string, int>)xpRewardInfo.GetValue(Globals.ServicesRoot.InfinityProjectExperience);
            xpRewardInfo.SetValue(Globals.ServicesRoot.InfinityProjectExperience, OriginalXpLevels);
            GameObject.Destroy(Globals.ExitToHubButton.gameObject);
        }

        public void OnSceneChange(string sceneName, Scene scene)
        {
            Globals.ExitToHubButton.SetActive(EverhoodHelpers.HasFlag("Archipelago_ReachedMain") && sceneName != "CosmicHubInfinity");
            switch (sceneName)
            {
                case "CosmicHubInfinity":
                    OnEnterMainHub(scene);
                    break;
                case "Marzian_Part1Hero_MinesHallway":
                    OnEnterMarzianHallway(scene);
                    break;
            }
        }

        private void OnEnterMainHub(Scene scene)
        {
            Globals.Logging.Error("EverhoodOverride", "Entered Hub");
            Globals.ServicesRoot!.GameData.GeneralData.boolVariables["Archipelago_ReachedMain"] = true;
            if (!EverhoodHelpers.HasFlag("GL_M1_GorillaDefeated"))
                return;

            if (!EverhoodHelpers.TryGetGameObjectWithName("GAMEPLAY", scene.GetRootGameObjects(), out var gameplay))
                throw new Exception("Failed to edit Main Hub: Could not find 'GAMEPLAY'.");

            if (!EverhoodHelpers.TryGetChildWithName("MarzianStoryDoor", gameplay, out var door))
                throw new Exception("Failed to edit Main Hub: Could not find 'MarzianStoryDoor'.");

            var doorTransform = door.transform;
            doorTransform.position = new Vector3(4.7f, doorTransform.position.y, doorTransform.position.z);
            var copy = GameObject.Instantiate(door.gameObject, doorTransform.position - new Vector3(0.5f, 0, 0f), doorTransform.rotation, doorTransform.parent);

            for (var i = copy.transform.childCount - 1; i >= 0; i--)
            {
                var transform = copy.transform.GetChild(i);
                switch (transform.name)
                {
                    case "LevelLoad-MarzianPart1":
                        transform.gameObject.SetActive(true);
                        break;
                    case "LevelLoad-MarzianPart2":
                    case "LevelLoad-MarzianPart3":
                    case "LevelLoad-MarzianPart4":
                        GameObject.Destroy(transform.gameObject);
                        break;
                    case "StoneHengeDoorBottom":
                        if (!EverhoodHelpers.TryGetChildWithName("NumberPad", transform, out var numberPad))
                            throw new Exception("Failed to edit Main Hub: Could not find 'NumberPad'.");

                        for (var j = numberPad.childCount - 1; j >= 0; j--)
                        {
                            var numberTransform = numberPad.GetChild(j);
                            switch (numberTransform.name)
                            {
                                case "Part1Numbers":
                                    numberTransform.gameObject.SetActive(true);
                                    break;
                                case "Part2Numbers":
                                case "Part3Numbers":
                                case "Part4Numbers":
                                case "Part5Numbers":
                                case "Part6Numbers":
                                    GameObject.Destroy(numberTransform.gameObject);
                                    break;
                            }
                        }

                        break;
                }
            }
        }

        private void OnEnterMarzianHallway(Scene scene)
        {
            Globals.Logging.Error("EverhoodOverride", "Entered Hallway");
            if (!EverhoodHelpers.HasFlag("GL_M1_GorillaDefeated"))
                return;

            if (!EverhoodHelpers.TryGetGameObjectWithName("GAMEPLAY", scene.GetRootGameObjects(), out var gameplay))
                throw new Exception("Failed to edit Marzian Hallway: Could not find 'GAMEPLAY'.");

            if (!EverhoodHelpers.TryGetChildWithName("West-Gate", gameplay, out var gate))
                throw new Exception("Failed to edit Marzian Hallway: Could not find 'West-Gate'.");

            gate.gameObject.SetActive(false);
        }

        private void AddExitToHubButton()
        {
            var scene = SceneManager.GetSceneByName("ServicesRoot");
            if (!scene.isLoaded)
                throw new Exception("ServicesRoot not loaded?");

            if (!EverhoodHelpers.TryGetGameObjectWithName("Root", scene.GetRootGameObjects(), out var root))
                throw new Exception("Failed to edit Esc Menu: Could not find 'Root'.");

            if (!EverhoodHelpers.TryGetChildWithName("Inventory Global", root, out var invGlobal))
                throw new Exception("Failed to edit Esc Menu: Could not find 'Inventory Global'.");

            if (!EverhoodHelpers.TryGetChildWithName("Inventory - Canvas", invGlobal, out var invCanvas))
                throw new Exception("Failed to edit Esc Menu: Could not find 'Inventory - Canvas'.");

            if (!EverhoodHelpers.TryGetChildWithName("Rect", invCanvas, out var rect))
                throw new Exception("Failed to edit Esc Menu: Could not find 'Rect'.");

            if (!EverhoodHelpers.TryGetChildWithName("Panel", rect, out var panel))
                throw new Exception("Failed to edit Esc Menu: Could not find 'Panel'.");

            if (!EverhoodHelpers.TryGetChildWithName("Menu", panel, out var menu))
                throw new Exception("Failed to edit Esc Menu: Could not find 'Menu'.");

            if (!EverhoodHelpers.TryGetChildWithName("Vertical Group", menu, out var verticalGroup))
                throw new Exception("Failed to edit Esc Menu: Could not find 'Vertical Group'.");

            Globals.ExitToHubButton = GameObject.Instantiate(verticalGroup.GetChild(0).gameObject, verticalGroup);
            Globals.ExitToHubButton.GetComponent<TextMeshProUGUI>().text = "Return To Cosmic Hub";

            var button = Globals.ExitToHubButton.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(ReturnToHub);

            Globals.ExitToHubButton.SetActive(false);
        }

        private void ReturnToHub()
        {
            Globals.SceneManagerRoot!.LoadTopdownScene("FadeInBasic", "FadeOutBasic", Globals.CurrentTopdownLevel, 10, new Vector3(2.938f, -3.7142f, 0), Direction.South, true);
        }
    }
}