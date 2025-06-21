using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArchipelagoEverhood.Data;
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
        public int PowerGemsRequired { get; private set; }

        public Dictionary<string, int> OriginalXpLevels = new();

        private SoulColor _soulColor;

        public void ArchipelagoConnected(string seed, SoulColor soulColor, int powerGemAmount)
        {
            if (Overriding)
            {
                Debug.LogError("We are somehow overriding twice? Did we log in again?");
                return;
            }

            Overriding = true;
            Seed = seed;
            _soulColor = soulColor;
            PowerGemsRequired = powerGemAmount;
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

            if (Globals.ServicesRoot != null)
            {
                var infProjExperience = typeof(InfinityProjectExperience);
                var xpRewardInfo = infProjExperience.GetField("xpRewardInfo", BindingFlags.Instance | BindingFlags.NonPublic);
                OriginalXpLevels = (Dictionary<string, int>)xpRewardInfo.GetValue(Globals.ServicesRoot.InfinityProjectExperience);
                xpRewardInfo.SetValue(Globals.ServicesRoot.InfinityProjectExperience, OriginalXpLevels);
            }

            if (Globals.ExitToHubButton)
                GameObject.Destroy(Globals.ExitToHubButton.gameObject);
        }

        public void OnSceneChange(string sceneName, Scene scene)
        {
            if (Globals.CurrentTopdownLevel == -1)
            {
                Globals.CurrentTopdownLevel = scene.buildIndex;
                Globals.Logging.LogDebug("Overrides", $"Setting Scene to {Globals.CurrentTopdownLevel}");
            }

            Globals.ExitToHubButton.SetActive(EverhoodHelpers.HasFlag("Archipelago_ReachedMain") && sceneName != "CosmicHubInfinity");
            switch (sceneName)
            {
                case "CosmicHubInfinity":
                    Globals.EverhoodDoors.OnEnterMainHub(scene);
                    break;
                case "Marzian_Part1Hero_MinesHallway":
                    OnEnterMarzianHallway(scene);
                    break;
                case "Neon_NeonDistrict":
                    OnEnterNeonDistrict(scene);
                    break;
            }
        }

        public bool HasSoulColor() => _soulColor != SoulColor.None;

        public void SetQuestionnaire()
        {
            //Set the player position to the correct point.
            Globals.TopdownRoot!.Player.transform.position = new Vector3(2.85f, -2.5576f, 0);
            Globals.ServicesRoot!.GameData.GeneralData.boolVariables["GL_StartedGame"] = true;
            Globals.ServicesRoot!.GameData.GeneralData.boolVariables["GL_TutorialDeath"] = true; //???
            Globals.ServicesRoot!.GameData.GeneralData.intVariables["GL_PlayerAge"] = 15; //Not exactly used but set anyway to ensure nothing breaks.

            Globals.ServicesRoot!.GameData.GeneralData.EquipWeapon(Weapon.Unarmed);
            switch (_soulColor)
            {
                case SoulColor.None:
                case SoulColor.Blue:
                    Globals.ServicesRoot!.GameData.GeneralData.intVariables["GL_Blue"] = 5;
                    Globals.ServicesRoot!.GameData.GeneralData.playerColor = PlayerColor.Blue;
                    break;
                case SoulColor.Green:
                    Globals.ServicesRoot!.GameData.GeneralData.intVariables["GL_Green"] = 5;
                    Globals.ServicesRoot!.GameData.GeneralData.playerColor = PlayerColor.Green;
                    break;
                case SoulColor.Red:
                    Globals.ServicesRoot!.GameData.GeneralData.intVariables["GL_Red"] = 5;
                    Globals.ServicesRoot!.GameData.GeneralData.playerColor = PlayerColor.Red;
                    break;
            }
        }

        private void OnEnterMarzianHallway(Scene scene)
        {
            if (!EverhoodHelpers.HasFlag("GL_M1_GorillaDefeated"))
                return;

            if (!EverhoodHelpers.TryGetGameObjectWithName("GAMEPLAY", scene.GetRootGameObjects(), out var gameplay))
                throw new Exception("Failed to edit Marzian Hallway: Could not find 'GAMEPLAY'.");

            if (!EverhoodHelpers.TryGetChildWithName("West-Gate", gameplay, out var gate))
                throw new Exception("Failed to edit Marzian Hallway: Could not find 'West-Gate'.");

            gate.gameObject.SetActive(false);
        }

        private void OnEnterNeonDistrict(Scene scene)
        {
            if (EverhoodHelpers.HasFlag("GL_ND_PaidStoneguard"))
            {
                if (!EverhoodHelpers.TryGetGameObjectWithName("Club10000", scene.GetRootGameObjects(), out var club10000))
                    throw new Exception("Failed to edit Neon District: Could not find 'Club10000'.");
                if (!EverhoodHelpers.TryGetChildWithName("GAMEPLAY", club10000, out var clubGameplay))
                    throw new Exception("Failed to edit Neon District: Could not find 'GAMEPLAY' under 'Club10000'.");
                if (EverhoodHelpers.TryGetChildWithName("StoneGuardNeon[NPC]", clubGameplay, out var stoneGuard))
                    stoneGuard.transform.position = new Vector3(55.367f, -12.449f, 0);
            }
            
            if (!EverhoodHelpers.TryGetGameObjectWithName("GAMEPLAY", scene.GetRootGameObjects(), out var gameplay))
                throw new Exception("Failed to edit Neon District: Could not find 'GAMEPLAY'.");
            

            if (EverhoodHelpers.TryGetChildWithName("FirstTime", gameplay, out var firstTime))
                firstTime.gameObject.SetActive(false);

            if (!EverhoodHelpers.TryGetChildWithName("InifinteTown", gameplay, out var infiniteTown))
                return;

            if (EverhoodHelpers.TryGetChildWithName("X1", infiniteTown, out var x1Minus))
                x1Minus.gameObject.SetActive(false);
        }

#region Main Menu

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
            Globals.Logging.Log("ReturnToHub", $"Loading Main Menu from: {Globals.CurrentTopdownLevel}.");
            Globals.SceneManagerRoot!.LoadTopdownScene("FadeInBasic", "FadeOutBasic", Globals.CurrentTopdownLevel, 10, new Vector3(2.938f, -3.7142f, 0), Direction.South, true);
        }

#endregion
    }
}