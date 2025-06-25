using System;
using ArchipelagoEverhood.Data;
using Fungus;
using HarmonyLib;

namespace ArchipelagoEverhood.Patches
{
    [HarmonyPatch(typeof(BooleanVariable), "Apply")]
    public static class BooleanVariableHookPatch
    {
        private static void Postfix(BooleanVariable __instance, SetOperator setOperator, bool value)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;

            try
            {
                if (setOperator != SetOperator.Assign)
                    return;
                var data = Globals.EverhoodChests.OnBoolVariableSet(__instance.Key, value);
                if (data == null)
                    return;

                //The very first artifact does this, and it saves *immediately* after which means we need to handle it *now*
                if (__instance.Key == "GL_MovementTutorialBattle")
                    Globals.SessionHandler.ItemHandler!.ForceRewardItems();

                Globals.Logging.Log("BooleanVariableHook", $"Got Variable: {__instance.Key}");

                var itemText = Globals.EverhoodChests.GetItemName(data);
                if (data.RewardConditions.HasFlag(RewardConditions.ForceShowDialogue))
                {
                    if (data.RewardConditions.HasFlag(RewardConditions.ForceShowDialogue) && data.Shown)
                        return;

                    data.Shown = true;
                    SayOnEnterPatch.ForceShowDialogue(itemText, null);
                }
                else
                {
                    SayOnEnterPatch.SetOverrideText(itemText);
                }
            }
            catch (Exception e)
            {
                Globals.Logging.Error("BooleanVariableHook", e);
            }
        }
    }
}