using System;
using System.Collections;
using System.Reflection;
using Archipelago.MultiClient.Net.Enums;
using ArchipelagoEverhood.Util;
using Fungus;
using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ArchipelagoEverhood.Patches
{
    [HarmonyPatch(typeof(BaseLeanTweenCommand), "OnEnter")]
    public class BaseLeanTweenCommandPatch
    {
        private static bool Prefix(MoveLean __instance, GameObjectData ____targetObject)
        {
            try
            {
                if (!Globals.SessionHandler.LoggedIn || !____targetObject.gameObjectVal)
                    return true;

                switch (____targetObject.Value.name)
                {
                    case "HallOfCon_Door":
                        if (EverhoodHelpers.GetItemCount(nameof(Item.WeaponToken)) >= 3)
                            return true;

                        __instance.IsExecuting = true;
                        SayOnEnterPatch.ForceShowDialogue($"You need 3 {EverhoodHelpers.ConstructItemText("Power Gems", ItemFlags.Advancement, false)} in total to fight the dragon! You have {EverhoodHelpers.GetItemCount(nameof(Item.WeaponToken))}.", __instance);
                        return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Globals.Logging.Error("MoveLean", e);
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(SetActive), "OnEnter")]
    public class SetActivePatch
    {
        private static bool Prefix(SetActive __instance, GameObjectData ____targetGameObject)
        {
            try
            {
                if (!Globals.SessionHandler.LoggedIn || !____targetGameObject.gameObjectVal)
                    return true;

                //Todo: Block other doors.
                switch (____targetGameObject.gameObjectVal.name)
                {
                    case "LevelLoad-HallOfCon-Start":
                    case "LevelLoad-HallOfCon-Mirror":
                    case "LevelLoad-HallOfCon-PostDragon":
                    case "PortalEffect_HallOfCon":
                        if (EverhoodHelpers.GetItemCount(nameof(Item.WeaponToken)) >= 3)
                            return true;

                        __instance.Continue();
                        return false;
                    default:
                        return true;
                }
            }
            catch (Exception e)
            {
                Globals.Logging.Error("SetActive", e);
                return true;
            }
        }
    }

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
                        if (__instance.ParentBlock.BlockName != "Which key" && !MenuDialogPatch.ElevatorItem.HasValue)
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
    public class MenuDialogPatch
    {
        public static Item? ElevatorItem;

        private static bool Prefix(MenuDialog __instance, string text, bool interactable, bool hideOption, Block targetBlock, out bool __result)
        {
            try
            {
                if (!Globals.SessionHandler.LoggedIn || targetBlock?.BlockName != "AutoKey")
                {
                    Globals.Logging.Log("BlockName", targetBlock?.BlockName);
                    __result = false;
                    return true;
                }

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
                if (!Globals.SessionHandler.LoggedIn || !MenuDialogPatch.ElevatorItem.HasValue)
                    return true;
                
                Globals.Logging.Log("EvaluateCondition", $"Check: {___variable.Key}");
                
                if (___variable.Key == "GL_1FinishedHillbertQuest")
                {
                    if (MenuDialogPatch.ElevatorItem != Item.RoomKey23)
                        return false;
                    MenuDialogPatch.ElevatorItem = null;
                    __result = true;
                    return false;
                }
                
                if (___variable.Key == "GL_2FinishedHillbertQuest")
                {
                    if (MenuDialogPatch.ElevatorItem != Item.RoomKeyGold)
                        return false;
                    MenuDialogPatch.ElevatorItem = null;
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