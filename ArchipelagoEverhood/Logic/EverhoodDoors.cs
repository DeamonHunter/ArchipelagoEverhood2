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
        public bool DoorRandoEnabled = false;

        private HashSet<long> _activeDoors = new();

        private int _marzianKeys;
        private const long marzian_key_id = 1;

        private static readonly Dictionary<long, string> _keysToDoors = new()
        {
            { 0, "NeonDistrictDoor" }, //Blue Route
            { marzian_key_id, "MarzianStoryDoor" }, //Green Route
            { 2, "EternalWarDoor" }, //Red Route
            { 3, "Smega_Door" }, //Post Dragon Content
            { 4, "TheLab" }, //Post Dragon Content
            { 5, "HomeTown_Door" }, //Post Dragon Content
            { 6, "Hall Of Con" }, //Dragon door. Should probably always be available.
        };

        public void OnEnterMainHub(Scene scene)
        {
            Globals.ServicesRoot!.GameData.GeneralData.boolVariables["Archipelago_ReachedMain"] = true;
            SpawnMarzianEra0Door(scene);
            ChangeDoorsMainHub(scene);
        }

        public void OnReceiveDoorKey(long keyId)
        {
            _activeDoors.Add(keyId);
            if (keyId == marzian_key_id)
                _marzianKeys++;

            var scene = SceneManager.GetSceneByName("CosmicHubInfinity");
            if (scene.isLoaded)
                ChangeDoorsMainHub(scene);
        }

        private void ChangeDoorsMainHub(Scene scene)
        {
            if (!DoorRandoEnabled)
                return;

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

                if (value.Value != "MarzianStoryDoor")
                    continue;

                for (var i = child.childCount - 1; i >= 0; i--)
                {
                    var transform = child.GetChild(i);
                    switch (transform.name)
                    {
                        case "LevelLoad-MarzianPart1":
                            transform.gameObject.SetActive(!EverhoodHelpers.HasFlag("GL_M1_GorillaDefeated") && _marzianKeys >= 1);
                            break;
                        case "LevelLoad-MarzianPart2":
                            transform.gameObject.SetActive(!EverhoodHelpers.HasFlag("GL_M2_Something") && _marzianKeys >= 2);
                            break;
                        case "LevelLoad-MarzianPart3":
                            transform.gameObject.SetActive(!EverhoodHelpers.HasFlag("GL_M3_Something") && _marzianKeys >= 3);
                            break;
                        case "LevelLoad-MarzianPart4":
                            transform.gameObject.SetActive(!EverhoodHelpers.HasFlag("GL_M4_Something") && _marzianKeys >= 4);
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
                                        transform.gameObject.SetActive(!EverhoodHelpers.HasFlag("GL_M1_GorillaDefeated") && _marzianKeys >= 1);
                                        break;
                                    case "Part2Numbers":
                                        transform.gameObject.SetActive(!EverhoodHelpers.HasFlag("GL_M2_GorillaDefeated") && _marzianKeys >= 2);
                                        break;
                                    case "Part3Numbers":
                                        transform.gameObject.SetActive(!EverhoodHelpers.HasFlag("GL_M3_Something") && _marzianKeys >= 3);
                                        break;
                                    case "Part4Numbers":
                                        transform.gameObject.SetActive(!EverhoodHelpers.HasFlag("GL_M4_Something") && _marzianKeys >= 4);
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
    }
}