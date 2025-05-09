﻿using System;
using System.Reflection;
using Fungus;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace ArchipelagoEverhood.Patches
{
    [HarmonyPatch(typeof(GivePlayerItem), "OnEnter")]
    public static class GivePlayerItemPatch
    {
        private static int message = 0;
        
        private static bool Prefix(string ___id)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return false;
            
            try
            {
                var itemInfo = Globals.ServicesRoot!.InfinityProjectExperience.GetItemRewardInfo(___id);
                Globals.Logging.Msg($"Unlocking Item: {itemInfo.id}");
                var data = Globals.EverhoodChests.ChestOpened(itemInfo.);
                if (data == null)
                    return true;

                var itemText = Globals.SessionHandler.LogicHandler!.GetScoutedItemText(data.LocationId);
                if (data.ForceSayDialogue)
                    SayOnEnterPatch.ForceShowDialogue(itemText);
                else
                    SayOnEnterPatch.OverrideTextValue = itemText;
            }
            catch (Exception e)
            {
                Globals.Logging.Error("UnlockCosmetic", e);
            }

            return true;
        }
    }
    
    [HarmonyPatch(typeof(UnlockCosmetic), "OnEnter")]
    public static class UnlockCosmeticPatch
    {
        private static int message = 0;
        
        private static bool Prefix(Cosmetics ___cosmetic)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return false;
            
            try
            {
                Globals.Logging.Msg($"Unlocking Cosmetic: {___cosmetic}");
                var data = Globals.EverhoodChests.ChestOpened(___cosmetic);
                if (data == null)
                    return true;

                var itemText = Globals.SessionHandler.LogicHandler!.GetScoutedItemText(data.LocationId);
                if (data.ForceSayDialogue)
                    SayOnEnterPatch.ForceShowDialogue(itemText);
                else
                    SayOnEnterPatch.OverrideTextValue = itemText;
            }
            catch (Exception e)
            {
                Globals.Logging.Error("UnlockCosmetic", e);
            }
            
            return true;
        }
    }

    [HarmonyPatch(typeof(GivePlayerXP), "OnEnter")]
    public static class GivePlayerXPPatch
    {
        private static bool Prefix(GivePlayerXP __instance, string ___id)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return false;
            
            try
            {
                if (!Globals.EverhoodOverrides.OriginalXpLevels.TryGetValue(___id, out var xpRewardCount))
                {
                    Globals.Logging.Msg($"Couldn't find xp for {___id}");
                    return true;
                }
                
                Globals.Logging.Msg($"Unlocking Xp: {xpRewardCount}");
                var data = Globals.EverhoodChests.ChestOpened(xpRewardCount);
                if (data == null)
                    return true;

                var itemText = Globals.SessionHandler.LogicHandler!.GetScoutedItemText(data.LocationId);
                if (data.ForceSayDialogue)
                    SayOnEnterPatch.ForceShowDialogue(itemText);
                else
                    SayOnEnterPatch.OverrideTextValue = itemText;
            }
            catch (Exception e)
            {
                Globals.Logging.Error("UnlockCosmetic", e);
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Say), "OnEnter")]
    public static class SayOnEnterPatch
    {
        public static string? OverrideTextValue;
        
        private static void Prefix(Say __instance, ref string ___overridingName)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;
            
            if (OverrideTextValue == null)
                return;

            ___overridingName = "Archipelago";
            __instance.SetStoryText(OverrideTextValue);
            OverrideTextValue = null;
            SayGetStringIdPatch.Override = true;
        }

        public static void ForceShowDialogue(string text)
        {
            var topDown = GameObject.FindFirstObjectByType<Main_TopdownRoot>(FindObjectsInactive.Include);
            var dialog = topDown.GetSayDialogue(DialogueBoxType.Topdown_NoPortrait);
            dialog.SetActive(true);
            dialog.Writer.SetDisableInstanteComplete(true);
            dialog.NameText.text = "Archipelago";
            SayDialog.ActiveSayDialog = dialog;
            dialog.Say(text, true, true, true, true, false, null, () =>
            { 
                Globals.Logging.Warning("Cosmetic Patch", "Unlocking Movement");
                topDown.Player.SetTopDownPlayerMovementState(true);
            });
                
            //The flowchart of many of these cosmetics automatically unlock movement
            MelonEvents.OnUpdate.Subscribe(( )=>
            {
                Globals.Logging.Warning("Cosmetic Patch", "Locking Movement");
                topDown.Player.SetTopDownPlayerMovementState(false);
            }, 0, true);
        }
    }
    [HarmonyPatch(typeof(Say), "GetStringId")]
    public static class SayGetStringIdPatch
    {
        public static bool Override;
        
        private static bool Prefix(Say __instance, ref string __result)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return false;
            
            if (!Override)
                return false;

            Override = false;
            __result = "-9999";
            return true;
        }
    }
}