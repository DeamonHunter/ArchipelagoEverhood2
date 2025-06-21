using System;
using System.Collections;
using System.Reflection;
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

        private static bool Prefix(MenuDialog __instance, string text, bool interactable, bool hideOption, Block targetBlock, ref Button[] ___cachedButtons, out bool __result)
        {
            try
            {
                if (!Globals.SessionHandler.LoggedIn || targetBlock?.BlockName != "AutoKey")
                {
                    Globals.Logging.Log("BlockName", targetBlock?.BlockName);
                    __result = false;
                    return true;
                }

                if (__instance.gameObject.scene.name == "Neon_HotelEntrance" && ___cachedButtons.Length < 7)
                {
                    Array.Resize(ref ___cachedButtons, 7);
                    var copyFrom = ___cachedButtons[3].gameObject;
                    ___cachedButtons[4] = GameObject.Instantiate(copyFrom, copyFrom.transform.position, copyFrom.transform.rotation, copyFrom.transform.parent).GetComponent<Button>();
                    ___cachedButtons[5] = GameObject.Instantiate(copyFrom, copyFrom.transform.position, copyFrom.transform.rotation, copyFrom.transform.parent).GetComponent<Button>();
                    ___cachedButtons[6] = GameObject.Instantiate(copyFrom, copyFrom.transform.position, copyFrom.transform.rotation, copyFrom.transform.parent).GetComponent<Button>();
                }
               
                            
                var last = __instance.CachedButtons[^1];

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

    [HarmonyPatch(typeof(VariableCondition), "EvaluateCondition")]
    public class IfPatch
    {
        private static bool Prefix(VariableCondition __instance, Variable ___variable, out bool __result)
        {
            __result = false;
            try
            {
                if (!Globals.SessionHandler.LoggedIn || !MenuDialogAddOptionPatch.ElevatorItem.HasValue)
                    return true;

                Globals.Logging.Log("EvaluateCondition", $"Check: {___variable.Key}");

                if (___variable.Key == "GL_1FinishedHillbertQuest")
                {
                    if (MenuDialogAddOptionPatch.ElevatorItem != Item.RoomKey23)
                        return false;
                    MenuDialogAddOptionPatch.ElevatorItem = null;
                    __result = true;
                    return false;
                }

                if (___variable.Key == "GL_2FinishedHillbertQuest")
                {
                    if (MenuDialogAddOptionPatch.ElevatorItem != Item.RoomKeyGold)
                        return false;
                    MenuDialogAddOptionPatch.ElevatorItem = null;
                    __result = true;
                    return false;
                }
                
                if (___variable.Key == "GL_3FinishedHillbertQuest")
                {
                    if (MenuDialogAddOptionPatch.ElevatorItem != Item.RoomKeyGreen)
                        return false;
                    MenuDialogAddOptionPatch.ElevatorItem = null;
                    __result = true;
                    return false;
                }
                if (___variable.Key == "GL_4FinishedHillbertQuest")
                {
                    if (MenuDialogAddOptionPatch.ElevatorItem != Item.RoomKeyPinecone)
                        return false;
                    MenuDialogAddOptionPatch.ElevatorItem = null;
                    __result = true;
                    return false;
                }
                if (___variable.Key == "GL_5FinishedHillbertQuest")
                {
                    if (MenuDialogAddOptionPatch.ElevatorItem != Item.RoomKeyOmega)
                        return false;
                    MenuDialogAddOptionPatch.ElevatorItem = null;
                    __result = true;
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Globals.Logging.Error("VariableCondition", e);
                return true;
            }
        }
    }
}