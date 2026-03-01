using System;
using System.Collections;
using System.Reflection;
using ArchipelagoEverhood.Util;
using Fungus;
using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ArchipelagoEverhood.Patches
{
    [HarmonyPatch(typeof(Condition), "OnEnter")]
    public class ConditionPatch
    {
        private static bool Prefix(Condition __instance)
        {
            try
            {
                if (!Globals.SessionHandler.LoggedIn || __instance is not ElseIfHaveItem haveItem)
                    return true;

                var item = typeof(ElseIfHaveItem).GetField("item", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(haveItem);
                switch (item)
                {
                    case Item.RoomKey23:
                    case Item.RoomKeyGold:
                    case Item.RoomKeyGreen:
                    case Item.RoomKeyPinecone:
                    case Item.RoomKeyOmega:
                    case Item.CrystalKey:
                    case Item.RavenKey:
                    case Item.FrogKey:
                    case Item.PandemoniumKey:
                    {
                        if (__instance.ParentBlock.BlockName != "Which key" && !MenuDialogAddOptionPatch.ElevatorItem.HasValue)
                        {
                            Globals.Logging.LogDebug("Condition", $"Skipping Due to '{__instance.ParentBlock.BlockName}'.");
                            return true;
                        }

                        Globals.Logging.LogDebug("Condition", $"Tried to use the item: {item}");
                        typeof(ElseIfHaveItem).GetMethod("EvaluateAndContinue", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(haveItem, null);
                        return false;
                    }

                    default:
                        return true;
                }
            }
            catch (Exception e)
            {
                Globals.Logging.Error("ElseIfHaveItem", e);
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(MenuDialog), "AddOption", typeof(string), typeof(bool), typeof(bool), typeof(Block))]
    public class MenuDialogAddOptionPatch
    {
        public static Item? ElevatorItem;

        private static bool Prefix(MenuDialog __instance, string text, bool interactable, bool hideOption, Block targetBlock, int ___nextOptionIndex, ref Button[] ___cachedButtons, out bool __result)
        {
            try
            {
                if (!Globals.SessionHandler.LoggedIn || __instance.gameObject.scene.name != "Neon_HotelEntrance")
                {
                    __result = false;
                    return true;
                }

                var group = __instance.GetComponentInChildren<HorizontalLayoutGroup>();
                if (targetBlock?.BlockName != "AutoKey")
                {
                    switch (targetBlock?.BlockName)
                    {
                        case "CosmicHub":
                        case "Cancel":
                            break;
                        default:
                            group.spacing = 35.24f;
                            break;
                    }

                    __result = false;
                    return true;
                }

                Globals.Logging.Log("BK", targetBlock?.BlockName + " : " + ___nextOptionIndex);
                if (___cachedButtons.Length < 7)
                {
                    Array.Resize(ref ___cachedButtons, 7);
                    var copyFrom = ___cachedButtons[3].gameObject;
                    ___cachedButtons[4] = GameObject.Instantiate(copyFrom, copyFrom.transform.position, copyFrom.transform.rotation, copyFrom.transform.parent).GetComponent<Button>();
                    ___cachedButtons[5] = GameObject.Instantiate(copyFrom, copyFrom.transform.position, copyFrom.transform.rotation, copyFrom.transform.parent).GetComponent<Button>();
                    ___cachedButtons[6] = GameObject.Instantiate(copyFrom, copyFrom.transform.position, copyFrom.transform.rotation, copyFrom.transform.parent).GetComponent<Button>();
                }

                if (___nextOptionIndex >= 2)
                {
                    foreach (Transform child in group.transform)
                    {
                        var rect = child.GetComponent<RectTransform>();
                        rect.sizeDelta = new Vector2(80, rect.sizeDelta.y);
                    }

                    group.spacing = 5;
                }
                else
                    group.spacing = 35.24f;


                __result = __instance.AddOption(text, interactable, hideOption, Do);
                return false;

                void Do()
                {
                    try
                    {
                        EventSystem.current.SetSelectedGameObject(new GameObject(null));
                        __instance.StopAllCoroutines();
                        __instance.Clear();
                        __instance.HideSayDialog();
                        __instance.gameObject.SetActive(false);

                        if (text.Contains("Key 23"))
                            ElevatorItem = Item.RoomKey23;
                        else if (text.Contains("Gold"))
                            ElevatorItem = Item.RoomKeyGold;
                        else if (text.Contains("Green"))
                            ElevatorItem = Item.RoomKeyGreen;
                        else if (text.Contains("Pinecone"))
                            ElevatorItem = Item.RoomKeyPinecone;
                        else if (text.Contains("Omega"))
                            ElevatorItem = Item.RoomKeyOmega;

                        Globals.Logging.Log("MenuDialog", $"Chosen {text} : {ElevatorItem}");
                        Flowchart flowchart = targetBlock.GetFlowchart();
                        Flowchart.choosing = false;

                        IEnumerator CallBlock(Block block)
                        {
                            yield return new WaitForEndOfFrame();
                            block.StartExecution();
                        }

                        flowchart.StartCoroutine(CallBlock(targetBlock));
                    }
                    catch (Exception e)
                    {
                        Globals.Logging.Error("MenuDialog", e);
                    }
                }
            }
            catch (Exception e)
            {
                Globals.Logging.Error("MenuDialog", e);
                __result = false;
                return true;
            }
        }
    }
    
    [HarmonyPatch(typeof(Call), "OnEnter")]
    public static class CallOnEnterPatch
    {
        private static bool Prefix(Call __instance, Block ___targetBlock)
        {
            if (!Globals.SessionHandler.LoggedIn || Globals.CurrentTopdownLevel != 15)
                return true;

            try
            {
                switch (___targetBlock.BlockName)
                {
                    case "Quest1Next":
                        if (!EverhoodHelpers.HasFlag("GL_2KeyShown"))
                            return true;
                        break;
                    case "Quest2Next":
                        if (!EverhoodHelpers.HasFlag("GL_3KeyShown"))
                            return true;
                        break;
                    case "Quest3Next":
                        if (!EverhoodHelpers.HasFlag("GL_4KeyShown"))
                            return true;
                        break;
                    case "Quest4Next":
                        if (!EverhoodHelpers.HasFlag("GL_5KeyShown"))
                            return true;
                        break;
                    
                    default:
                        return true;
                }
                
                Globals.TopdownRoot!.Player.SetTopDownPlayerMovementState(true);
                __instance.Continue();
                return false;
            }
            catch (Exception e)
            {
                Globals.Logging.Error("Call", e);
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(VariableCondition), "EvaluateCondition")]
    public class IfPatch
    {
        private static bool Prefix(VariableCondition __instance, Variable ___variable, BooleanData ___booleanData, out bool __result)
        {
            __result = false;
            try
            {
                if (!Globals.SessionHandler.LoggedIn)
                    return true;

                switch (Globals.CurrentTopdownLevel)
                {
                    case 10:
                        if (HillbertQuestFlags(___variable, out __result))
                            return false;
                        break;

                    case 15:
                        if (MenuDialogAddOptionPatch.ElevatorItem.HasValue && Elevator(___variable, out __result))
                            return false;
                        break;
                    
                    case 68:
                        Globals.Logging.LogDebug("If Override", "Doing If Override");
                        //If this is on, we may be checking the final fight which would soft lock.
                        if (EverhoodHelpers.HasFlag("GL_MB_GateOpen"))
                            return true;
                        
                        if (___variable.Key == "GL_MB_Sun")
                        {
                            Globals.Logging.LogDebug("If Override", "Sun");
                            
                            __result = Globals.ServicesRoot!.GameData.GeneralData.collectedItems.ContainsKey("SunInsignia");
                            return false;
                        }
                        
                        if (___variable.Key == "GL_MB_Moon")
                        {
                            Globals.Logging.LogDebug("If Override", "Moon");
                            __result = Globals.ServicesRoot!.GameData.GeneralData.collectedItems.ContainsKey("MoonInsignia");
                            return false;
                        }
                        
                        break;
                }


                return true;
            }
            catch (Exception e)
            {
                Globals.Logging.Error("VariableCondition", e);
                return true;
            }
        }

        private static bool Elevator(Variable variable, out bool result)
        {
            Globals.Logging.LogDebug("EvaluateCondition", $"Check: {variable.Key} : {MenuDialogAddOptionPatch.ElevatorItem}");
            result = false;
            switch (variable.Key)
            {
                case "GL_1FinishedHillbertQuest":
                {
                    if (MenuDialogAddOptionPatch.ElevatorItem != Item.RoomKey23)
                    {
                        result = false;
                        return true;
                    }

                    MenuDialogAddOptionPatch.ElevatorItem = null;
                    result = true;
                    return true;
                }
                case "GL_2FinishedHillbertQuest":
                {
                    if (MenuDialogAddOptionPatch.ElevatorItem != Item.RoomKeyGold)
                    {
                        result = false;
                        return true;
                    }

                    MenuDialogAddOptionPatch.ElevatorItem = null;
                    result = true;
                    return true;
                }
                case "GL_3FinishedHillbertQuest":
                {
                    if (MenuDialogAddOptionPatch.ElevatorItem != Item.RoomKeyGreen)
                    {
                        result = false;
                        return true;
                    }

                    MenuDialogAddOptionPatch.ElevatorItem = null;
                    result = true;
                    return true;
                }
                case "GL_4FinishedHillbertQuest":
                {
                    if (MenuDialogAddOptionPatch.ElevatorItem != Item.RoomKeyPinecone)
                    {
                        result = false;
                        return true;
                    }

                    MenuDialogAddOptionPatch.ElevatorItem = null;
                    result = true;
                    return true;
                }
                case "GL_5FinishedHillbertQuest":
                {
                    if (MenuDialogAddOptionPatch.ElevatorItem != Item.RoomKeyOmega)
                    {
                        result = false;
                        return true;
                    }

                    MenuDialogAddOptionPatch.ElevatorItem = null;
                    result = true;
                    return true;
                }
                default:
                    return false;
            }
        }

        private static bool HillbertQuestFlags(Variable variable, out bool result)
        {
            Globals.Logging.LogDebug("EvaluateCondition", $"Check: {variable.Key} : {MenuDialogAddOptionPatch.ElevatorItem}");
            result = false;
            switch (variable.Key)
            {
                case "GL_1FinishedHillbertQuest":
                    result = EverhoodHelpers.HasFlag("GL_1RoomKeyInventory");
                    return true;
                case "GL_2FinishedHillbertQuest":
                    result = EverhoodHelpers.HasFlag("GL_2RoomKeyInventory");
                    return true;
                case "GL_3FinishedHillbertQuest":
                    result = EverhoodHelpers.HasFlag("GL_3RoomKeyInventory");
                    return true;
                case "GL_4FinishedHillbertQuest":
                    result = EverhoodHelpers.HasFlag("GL_4RoomKeyInventory");
                    return true;
                case "GL_5FinishedHillbertQuest":
                    result = EverhoodHelpers.HasFlag("GL_5RoomKeyInventory");
                    return true;
                default:
                    return false;
            }
        }
    }
}