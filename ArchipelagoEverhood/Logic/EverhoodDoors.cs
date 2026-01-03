using System;
using System.Collections.Generic;
using System.Linq;
using ArchipelagoEverhood.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArchipelagoEverhood.Logic
{
    public class EverhoodDoors
    {
        private HashSet<long> _activeDoors = new();

        private int _marzianKeys;
        private const long marzian_key_id = 1;
        private int _frameCountdown;
        private Hub _loadingHub;
        
        private FieldInfo _sceneToLoad = typeof(SwitchTopDownScene).GetField("sceneToLoad", BindingFlags.Instance | BindingFlags.NonPublic)!;
        private FieldInfo _sceneSpawnPosition  = typeof(SwitchTopDownScene).GetField("spawnPosition", BindingFlags.Instance | BindingFlags.NonPublic)!;
        
        private FieldInfo _triggerSceneToLoad = typeof(TopDownSwitchSceneZoneTrigger).GetField("sceneToLoad", BindingFlags.Instance | BindingFlags.NonPublic)!;
        private FieldInfo _triggerSpawnPosition = typeof(TopDownSwitchSceneZoneTrigger).GetField("spawnPosition", BindingFlags.Instance | BindingFlags.NonPublic)!;
        private FieldInfo _trigger3D = typeof(TopDownSwitchSceneZoneTrigger).GetField("thirdVectorSpawnPos", BindingFlags.Instance | BindingFlags.NonPublic)!;
        
        private FieldInfo _sceneValue = typeof(SceneField).GetField("buildIndex", BindingFlags.Instance | BindingFlags.NonPublic)!;
        
        private static readonly Dictionary<long, string> _keysToDoors = new()
        {
            { 0, "NeonDistrictDoor" }, //Blue Route
            { marzian_key_id, "MarzianStoryDoor" }, //Green Route
            { 2, "EternalWarDoor" }, //Red Route
            { 3, "Smega_Door" }, //Post Dragon Content
            { 4, "TheLab" }, //Post Dragon Content
            { 5, "HomeTown_Door" }, //Post Dragon Content
            { 6, "Hall Of Con" }, //Dragon door. Should probably always be available.
            
            { 7, "MushroomDoor" },
            { 8, "3DDoor" },
            { 9, "SmellyDoor" },
            { 10, "BirdIslandDoor" },
            { 11, "Everhood1Door" },
        };
        
        private enum Hub
        {
            CosmicHub,
            TimeHub
        }

        public void OnEnterMainHub(Scene scene)
        {
            Globals.ServicesRoot!.GameData.GeneralData.boolVariables["Archipelago_ReachedMain"] = true;
            SpawnMarzianEra0Door(scene);
            _frameCountdown = 6; //This is kinda gross, but the home town door change is very late.
            _loadingHub = Hub.CosmicHub;
        }

        public void OnEnterTimeHub(Scene scene)
        {
            Globals.ServicesRoot!.GameData.GeneralData.boolVariables["Archipelago_ReachedTime"] = true;
            _frameCountdown = 6; //This is kinda gross, but the home town door change is very late.
            _loadingHub = Hub.TimeHub;
        }

        public void Update()
        {
            if (_frameCountdown <= 0)
                return;

            Globals.Logging.LogDebug("EverhoodDoors", $"Frame Countdown: {_frameCountdown}");
            _frameCountdown--;
            if (_frameCountdown > 0)
                return;

            switch (_loadingHub)
            {
                case Hub.CosmicHub:
                {
                    var scene = SceneManager.GetSceneByName("CosmicHubInfinity");
                    if (!scene.isLoaded)
                        throw new Exception("Failed to load the hub even though we are in it???");
                    ChangeDoorsMainHub(scene);
                    break;
                }
                case Hub.TimeHub:
                {
                    var scene = SceneManager.GetSceneByName("TimeHubInfinity");
                    if (!scene.isLoaded)
                        throw new Exception("Failed to load the hub even though we are in it???");
                    ChangeDoorsTimeHub(scene);
                    break;
                }
            }
        }

        public void ResetKeys()
        {
            _activeDoors.Clear();
            _marzianKeys = 0;
            var scene = SceneManager.GetSceneByName("CosmicHubInfinity");
            if (scene.isLoaded)
                ChangeDoorsMainHub(scene);
            scene = SceneManager.GetSceneByName("TimeHubInfinity");
            if (scene.isLoaded)
                ChangeDoorsTimeHub(scene);
        }

        public void OnReceiveDoorKey(long keyId)
        {
            _activeDoors.Add(keyId);
            if (keyId == marzian_key_id)
            {
                _marzianKeys++;
                Globals.Logging.Log("EverhoodDoors", $"Got Key: {keyId}. Count: {_marzianKeys}");
            }
            else
                Globals.Logging.Log("EverhoodDoors", $"Got Key: {keyId}");

            var scene = SceneManager.GetSceneByName("CosmicHubInfinity");
            if (scene.isLoaded)
                ChangeDoorsMainHub(scene);
        }

#region CosmicHub
        
        private void ChangeDoorsMainHub(Scene scene)
        {
            Globals.Logging.LogDebug("EverhoodDoors", "Attempting Change to Cosmic hub doors.");
            if (Globals.EverhoodOverrides.Settings == null || !Globals.EverhoodOverrides.Settings.DoorKeys)
                return;

            Globals.Logging.Log("EverhoodDoors", "Changing Cosmic hub doors.");

            if (!EverhoodHelpers.TryGetGameObjectWithName("GAMEPLAY", scene.GetRootGameObjects(), out var gameplay))
            {
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'GAMEPLAY'.");
                return;
            }

            foreach (Transform child in gameplay.transform)
            {
                var value = _keysToDoors.FirstOrDefault(x => x.Value == child.name);
                if (value.Value is null or "Hall Of Con")
                    continue;

                child.gameObject.SetActive(_activeDoors.Contains(value.Key));
                switch (value.Value)
                {
                    default:
                        child.gameObject.SetActive(_activeDoors.Contains(value.Key));
                        break;

                    case "Smega_Door":
                    {
                        if (_activeDoors.Contains(value.Key))
                        {
                            child.transform.position = new Vector3(1.8f, -2.45f, 0f);
                            child.gameObject.SetActive(true);
                        }
                        else
                            child.gameObject.SetActive(false);

                        break;
                    }
                    case "TheLab":
                    {
                        if (_activeDoors.Contains(value.Key))
                        {
                            child.transform.position = new Vector3(4.019f, -2.303f, 0);
                            child.gameObject.SetActive(true);
                        }
                        else
                            child.gameObject.SetActive(false);

                        break;
                    }
                    case "HomeTown_Door":
                    {
                        if (_activeDoors.Contains(value.Key))
                        {
                            child.transform.position = new Vector3(1.199f, -3.153f, 0f);
                            child.gameObject.SetActive(true);
                        }
                        else
                            child.gameObject.SetActive(false);

                        break;
                    }
                    case "MarzianStoryDoor":
                    {
                        var anyActive = false;
                        for (var i = 0; i < child.childCount; i++)
                        {
                            var transform = child.GetChild(i);
                            switch (transform.name)
                            {
                                case "LevelLoad-MarzianPart1":
                                {
                                    var active = !EverhoodHelpers.HasFlag("GL_M1_GorillaDefeated") && _marzianKeys >= 1;
                                    transform.gameObject.SetActive(active);
                                    anyActive |= active;
                                    Globals.Logging.Log("Doors", $"1:{active}");
                                    break;
                                }
                                case "LevelLoad-MarzianPart2":
                                {
                                    var active = EverhoodHelpers.HasFlag("GL_M1_GorillaDefeated") &&
                                                 !EverhoodHelpers.HasFlag("GL_PortalBattleFinished") && _marzianKeys >= 2;
                                    transform.gameObject.SetActive(active);
                                    anyActive |= active;
                                    Globals.Logging.Log("Doors", $"2:{active}");
                                    break;
                                }
                                case "LevelLoad-MarzianPart3":
                                {
                                    var active = EverhoodHelpers.HasFlag("GL_PortalBattleFinished") &&
                                                 !EverhoodHelpers.HasFlag("GL_M3_Apocalypse") && _marzianKeys >= 3;
                                    transform.gameObject.SetActive(active);
                                    anyActive |= active;
                                    Globals.Logging.Log("Doors", $"3:{active}");
                                    break;
                                }
                                case "LevelLoad-MarzianPart4":
                                {
                                    var active = EverhoodHelpers.HasFlag("GL_M3_Apocalypse") &&
                                                 !EverhoodHelpers.HasFlag("GL_M4_GorillaDefeated") && _marzianKeys >= 4;
                                    transform.gameObject.SetActive(active);
                                    anyActive |= active;
                                    Globals.Logging.Log("Doors", $"4:{active}");
                                    break;
                                }
                                case "Portal_Marzian":
                                {
                                    transform.gameObject.SetActive(anyActive);
                                    break;
                                }
                                case "StoneHengeDoorBottom":
                                {
                                    if (!EverhoodHelpers.TryGetChildWithName("NumberPad", transform, out var numberPad))
                                        throw new Exception("Failed to edit Main Hub: Could not find 'NumberPad'.");

                                    for (var j = numberPad.childCount - 1; j >= 0; j--)
                                    {
                                        var numberTransform = numberPad.GetChild(j);
                                        switch (numberTransform.name)
                                        {
                                            case "Part1Numbers":
                                                numberTransform.gameObject.SetActive(!EverhoodHelpers.HasFlag("GL_M1_GorillaDefeated") && _marzianKeys >= 1);
                                                break;
                                            case "Part2Numbers":
                                                numberTransform.gameObject.SetActive(EverhoodHelpers.HasFlag("GL_M1_GorillaDefeated") &&
                                                                                     !EverhoodHelpers.HasFlag("GL_PortalBattleFinished") && _marzianKeys >= 2);
                                                break;
                                            case "Part3Numbers":
                                                numberTransform.gameObject.SetActive(EverhoodHelpers.HasFlag("GL_PortalBattleFinished") &&
                                                                                     !EverhoodHelpers.HasFlag("GL_M3_Apocalypse") && _marzianKeys >= 3);
                                                break;
                                            case "Part4Numbers":
                                                numberTransform.gameObject.SetActive(EverhoodHelpers.HasFlag("GL_M3_Apocalypse") &&
                                                                                     !EverhoodHelpers.HasFlag("GL_M4_Something") && _marzianKeys >= 4);
                                                break;
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

                        break;
                    }
                }
            }
        }

        private void SpawnMarzianEra0Door(Scene scene)
        {
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

#endregion

#region TimeHub


        private void ChangeDoorsTimeHub(Scene scene)
        {
            if (Globals.EverhoodOverrides.Settings == null || !Globals.EverhoodOverrides.Settings.DoorKeys)
                return;

            _activeDoors.Add(10);
            _activeDoors.Add(11);

            if (!EverhoodHelpers.TryGetGameObjectWithName("GAMEPLAY", scene.GetRootGameObjects(), out var gameplay))
            {
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'GAMEPLAY'.");
                return;
            }

            AdjustTimeHubTriggers(scene);

            GameObject? mushroomDoor = null;
            GameObject? noseDoor = null;

            foreach (Transform child in gameplay.transform)
            {
                var value = _keysToDoors.FirstOrDefault(x => x.Value == child.name);
                if (value.Value is null)
                    continue;

                var active = _activeDoors.Contains(value.Key);
                switch (value.Value)
                {
                    default:
                        child.gameObject.SetActive(active);
                        break;
                    
                    case "MushroomDoor":
                        mushroomDoor = child.gameObject;
                        child.gameObject.SetActive(active);
                        break;
                    
                    case "SmellyDoor":
                        noseDoor = child.gameObject;
                        child.gameObject.SetActive(active);
                        break;

                    case "3DDoor":
                    {
                        child.gameObject.SetActive(true);
                        child.position = new Vector3(1.825f, -2.63f, 0.28f);
                        
                        if (EverhoodHelpers.TryGetChildWithName("Locked3D", child, out var locked))
                            locked.gameObject.SetActive(!active);
                        
                        if (EverhoodHelpers.TryGetChildWithName("RightPillar", child, out var rightPillar))
                            if (EverhoodHelpers.TryGetChildWithName("3DPortal", rightPillar, out var portal))
                                portal.gameObject.SetActive(active);
                        
                        
                        /*
                        if (_activeDoors.Contains(value.Key))
                        {
                            child.transform.position = new Vector3(1.8f, -2.45f, 0f);
                            child.gameObject.SetActive(true);
                        }
                        else
                            child.gameObject.SetActive(false);
                        */

                        
                        break;
                    }
                }
            }
                
            if (mushroomDoor != null && noseDoor != null)
                CreateNewDoors(mushroomDoor, noseDoor);
            else
                Globals.Logging.Error("EverhoodDoors", "Couldn't find mushroom or nose doors.");
        }

        private void CreateNewDoors(GameObject mushroomDoor, GameObject noseDoor)
        {
            if (_activeDoors.Contains(10))
            {
                var portalEffect = GameObject.Instantiate(noseDoor, noseDoor.transform.parent);
                portalEffect.transform.position = new Vector3(4.55f, -2.33f, 0f);
                portalEffect.gameObject.SetActive(true);
                if (EverhoodHelpers.TryGetChildWithName("Animator", portalEffect, out var animator))
                {
                    if (EverhoodHelpers.TryGetChildWithName("Nose", animator, out var nose))
                        nose.gameObject.SetActive(false);
                    if (EverhoodHelpers.TryGetChildWithName("PortalEffect_LostHillbert-Toilet", animator, out var vfx))
                    {
                        vfx.gameObject.SetActive(true);
                        if (EverhoodHelpers.TryGetChildWithName("PortalSpin (1)", vfx, out var portalSpin))
                        {
                            var system = portalSpin.GetComponent<ParticleSystem>().main;
                            system.startColor = new ParticleSystem.MinMaxGradient(new Color(0.3471698f, 0.6049057f, 0.1603774f, 1f));
                        }
                    }
                }
                if (EverhoodHelpers.TryGetChildWithName("Colliders_MushroomDoor", portalEffect, out var colliders))
                {
                    if (EverhoodHelpers.TryGetChildWithName("Interact", colliders, out var interact))
                        interact.gameObject.SetActive(false);
                    if (EverhoodHelpers.TryGetChildWithName("LoadScene - LostRoom", colliders, out var lostRoom))
                    {
                        lostRoom.gameObject.SetActive(true);
                        var switchTrigger = lostRoom.GetComponent<TopDownSwitchSceneZoneTrigger>();
                        var sceneInstance = _triggerSceneToLoad.GetValue(switchTrigger);
                        _sceneValue.SetValue(sceneInstance, 84);
                        _triggerSpawnPosition.SetValue(switchTrigger, new Vector2(1.808f, -5.9955f));
                    }
                }
            }
            
            if (_activeDoors.Contains(11))
            {
                var portalEffect = GameObject.Instantiate(mushroomDoor, mushroomDoor.transform.parent);
                portalEffect.transform.position = new Vector3(4.9f, -3.9f, 0f);
                portalEffect.transform.localScale = new Vector3(-1, 1, 1);
                portalEffect.gameObject.SetActive(true);
                
                if (EverhoodHelpers.TryGetChildWithName("DoorFrame", portalEffect, out var frame))
                {
                    frame.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 1f, 1);
                    if (EverhoodHelpers.TryGetChildWithName("MushroomDoor_Door", frame, out var interact))
                        interact.gameObject.SetActive(false);
                    if (EverhoodHelpers.TryGetChildWithName("LevelLoad-Mushroom-Start", frame, out var lostRoom))
                    {
                        lostRoom.gameObject.SetActive(true);
                        var switchTrigger = lostRoom.GetComponent<TopDownSwitchSceneZoneTrigger>();
                        var sceneInstance = _triggerSceneToLoad.GetValue(switchTrigger);
                        _sceneValue.SetValue(sceneInstance, 84);
                        _triggerSpawnPosition.SetValue(switchTrigger, new Vector2(1.808f, -5.9955f));
                    }
                    if (EverhoodHelpers.TryGetChildWithName("PortalEffect_MushroomForest", frame, out var vfx))
                    {
                        vfx.gameObject.SetActive(true);
                        if (EverhoodHelpers.TryGetChildWithName("PortalSpin (1)", vfx, out var portalSpin))
                        {
                            var system = portalSpin.GetComponent<ParticleSystem>().main;
                            system.startColor = new ParticleSystem.MinMaxGradient(new Color(1f, 0f, 0.674f, 1f));
                        }
                        if (EverhoodHelpers.TryGetChildWithName("PortalSFX", vfx, out var portalSFX))
                            portalSFX.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void AdjustTimeHubTriggers(Scene scene)
        {
            if (!EverhoodHelpers.TryGetGameObjectWithName("TRIGGERBOX", scene.GetRootGameObjects(), out var triggers))
            {
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'TRIGGERBOX'.");
                return;
            }
            
            if (EverhoodHelpers.TryGetChildWithName("Intro-RavenLockingGate", triggers, out var ravenLock))
                ravenLock.gameObject.SetActive(false);
            else
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'Intro-RavenLockingGate'.");
            
            if (EverhoodHelpers.TryGetChildWithName("LevelLoad-3DDimension", triggers, out var threeDLoad))
                threeDLoad.gameObject.SetActive(_activeDoors.Contains(8));
            else
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'LevelLoad-3DDimension'.");
            
            if (EverhoodHelpers.TryGetChildWithName("GotCoinsTrigger", triggers, out var gotcoins))
                gotcoins.gameObject.SetActive(false);
            else
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'GotCoinsTrigger'.");
        }

        public void OnEnter3DDimension(Scene scene)
        {
            if (Globals.EverhoodOverrides.Settings == null || !Globals.EverhoodOverrides.Settings.DoorKeys)
                return;
            
            if (!EverhoodHelpers.TryGetGameObjectWithName("TRIGGERBOX", scene.GetRootGameObjects(), out var triggers))
            {
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'TRIGGERBOX'.");
                return;
            }
            if (!EverhoodHelpers.TryGetGameObjectWithName("GAMEPLAY", scene.GetRootGameObjects(), out var gameplay))
            {
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'GAMEPLAY'.");
                return;
            }
            if (!EverhoodHelpers.TryGetGameObjectWithName("WORLD", scene.GetRootGameObjects(), out var world))
            {
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'WORLD'.");
                return;
            }
            if (!EverhoodHelpers.TryGetGameObjectWithName("FLOWCHARTS", scene.GetRootGameObjects(), out var flowcharts))
            {
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'FLOWCHARTS'.");
                return;
            }
            
            if (EverhoodHelpers.TryGetChildWithName("Levelload-TheLabTriggerBox", triggers, out var labTrigger))
                labTrigger.gameObject.SetActive(false);
            else
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'Levelload-TheLabTriggerBox'.");
            
            if (EverhoodHelpers.TryGetChildWithName("Levelload-SmegaTriggerBox", triggers, out var smegaTrigger))
                smegaTrigger.gameObject.SetActive(false);
            else
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'Levelload-SmegaTriggerBox'.");
            
            if (EverhoodHelpers.TryGetChildWithName("EvrensHead", gameplay, out var labPortal))
                labPortal.gameObject.SetActive(false);
            else
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'EvrensHead'.");
            
            if (EverhoodHelpers.TryGetChildWithName("SMEGA-Portal", world, out var smegaPortal))
                smegaPortal.gameObject.SetActive(false);
            else
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'SMEGA-Portal'.");
            
            foreach (var switchScene in flowcharts.GetComponentsInChildren<SwitchTopDownScene>())
            {
                var sceneInstance = (SceneField)_sceneToLoad.GetValue(switchScene);
                if (sceneInstance.BuildIndex != 84)
                    continue;
                
                _sceneValue.SetValue(sceneInstance, 64);
                _sceneSpawnPosition.SetValue(switchScene, new Vector2(3.6951f, -2.8166f));
            }
        }
        public void OnEnterBirdIsland(Scene scene)
        {
            //if (Globals.EverhoodOverrides.Settings == null || !Globals.EverhoodOverrides.Settings.DoorKeys)
            //    return;
            
            if (!EverhoodHelpers.TryGetGameObjectWithName("TRIGGERBOX", scene.GetRootGameObjects(), out var triggers))
            {
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'TRIGGERBOX'.");
                return;
            }

            if (EverhoodHelpers.TryGetChildWithName("LevelLoad-3DDimension", triggers, out var threeDPortal))
            {
                var trigger = threeDPortal.GetComponent<TopDownSwitchSceneZoneTrigger>();
                var sceneInstance = _triggerSceneToLoad.GetValue(trigger);
                _sceneValue.SetValue(sceneInstance, 64);
                _triggerSpawnPosition.SetValue(trigger, new Vector2(3.6951f, -2.8166f));
                _trigger3D.SetValue(trigger, false);
            }
            else
                Globals.Logging.Error("EverhoodDoors", "Failed to find 'LevelLoad-3DDimension'.");
        }

#endregion
    }
}