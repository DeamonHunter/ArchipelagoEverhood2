using System;
using Fungus;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace ArchipelagoEverhood.Patches
{
    [HarmonyPatch(typeof(GivePlayerItem), "OnEnter")]
    public static class GivePlayerItemPatch
    {
        private static int message = 0;

        private static bool Prefix(GivePlayerItem __instance, string ___id, bool ___showDialogue)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return true;

            try
            {
                var itemInfo = Globals.ServicesRoot!.InfinityProjectExperience.GetItemRewardInfo(___id);
                Globals.Logging.Msg($"Unlocking Item: {itemInfo.id}");
                var data = Globals.EverhoodChests.ChestOpened(itemInfo.item);
                if (data == null)
                    return true;

                var itemText = Globals.EverhoodChests.GetItemName(data);
                if (___showDialogue || data.ForceSayDialogue)
                    SayOnEnterPatch.ForceShowDialogue(itemText, __instance);
                else
                {
                    SayOnEnterPatch.SetOverrideText(itemText);
                    __instance.Continue();
                }
            }
            catch (Exception e)
            {
                Globals.Logging.Error("UnlockCosmetic", e);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(GivePlayerArtifact), "OnEnter")]
    public static class GivePlayerArtifactPatch
    {
        private static int message = 0;

        private static bool Prefix(GivePlayerArtifact __instance, string ___id, bool ___showDialogue)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return true;

            try
            {
                var artifactInfo = Globals.ServicesRoot!.InfinityProjectExperience.GetArtifactRewardInfo(___id);
                Globals.Logging.Msg($"Unlocking Item: {artifactInfo.id}");
                var data = Globals.EverhoodChests.ChestOpened(artifactInfo.artifacts);
                if (data == null)
                    return true;

                var itemText = Globals.EverhoodChests.GetItemName(data);
                if (___showDialogue || data.ForceSayDialogue)
                    SayOnEnterPatch.ForceShowDialogue(itemText, __instance);
                else
                {
                    SayOnEnterPatch.SetOverrideText(itemText);
                    __instance.Continue();
                }
            }
            catch (Exception e)
            {
                Globals.Logging.Error("UnlockCosmetic", e);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(AddWeapon), "OnEnter")]
    public static class AddWeaponPatch
    {
        private static int message = 0;

        private static bool Prefix(AddWeapon __instance, Weapon ___playerWeapon)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return true;

            try
            {
                Globals.Logging.Msg($"Unlocking Item: {___playerWeapon}");
                var data = Globals.EverhoodChests.ChestOpened(___playerWeapon);
                if (data == null)
                    return true;

                var itemText = Globals.EverhoodChests.GetItemName(data);
                if (data.ForceSayDialogue)
                    SayOnEnterPatch.ForceShowDialogue(itemText, __instance);
                else
                {
                    SayOnEnterPatch.SetOverrideText(itemText);
                    __instance.Continue();
                }
            }
            catch (Exception e)
            {
                Globals.Logging.Error("UnlockCosmetic", e);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(UnlockCosmetic), "OnEnter")]
    public static class UnlockCosmeticPatch
    {
        private static int message = 0;

        private static bool Prefix(UnlockCosmetic __instance, Cosmetics ___cosmetic)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return true;

            try
            {
                Globals.Logging.Msg($"Unlocking Cosmetic: {___cosmetic}");
                var data = Globals.EverhoodChests.ChestOpened(___cosmetic);
                if (data == null)
                    return true;

                var itemText = Globals.EverhoodChests.GetItemName(data);
                if (data.ForceSayDialogue)
                    SayOnEnterPatch.ForceShowDialogue(itemText, __instance);
                else
                {
                    SayOnEnterPatch.SetOverrideText(itemText);
                    __instance.Continue();
                }
            }
            catch (Exception e)
            {
                Globals.Logging.Error("UnlockCosmetic", e);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(GivePlayerXP), "OnEnter")]
    public static class GivePlayerXPPatch
    {
        private static bool Prefix(GivePlayerXP __instance, string ___id, bool ___showDialogue)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return true;

            try
            {
                if (!Globals.EverhoodOverrides.OriginalXpLevels.TryGetValue(___id.ToLower(), out var xpRewardCount))
                {
                    Globals.Logging.Msg($"Couldn't find xp for {___id}");
                    return true;
                }

                Globals.Logging.Msg($"Unlocking Xp: {xpRewardCount}");
                var data = Globals.EverhoodChests.ChestOpened(xpRewardCount);
                if (data == null)
                    return true;

                var itemText = Globals.EverhoodChests.GetItemName(data);
                if (data.ForceSayDialogue || ___showDialogue)
                {
                    Globals.Logging.Log("XP Patch", $"Force Show Dialogue: {itemText}");
                    SayOnEnterPatch.ForceShowDialogue(itemText, __instance);
                }
                else
                {
                    Globals.Logging.Log("XP Patch", $"Setting override text: {itemText}");
                    SayOnEnterPatch.SetOverrideText(itemText);
                }
            }
            catch (Exception e)
            {
                Globals.Logging.Error("UnlockCosmetic", e);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(Say), "OnEnter")]
    public static class SayOnEnterPatch
    {
        private static string? _overrideTextValue;

        private static void Prefix(Say __instance, ref string ___overridingName)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return;

            Globals.Logging.Log("Say", "Triggered");

            if (_overrideTextValue == null)
                return;

            Globals.Logging.Log("Say", $"Had Override Text: {_overrideTextValue}");

            ___overridingName = "Archipelago";
            __instance.SetStoryText(_overrideTextValue);
            _overrideTextValue = null;
            SayGetStringIdPatch.Override = true;
        }

        public static void SetOverrideText(string text)
        {
            if (string.IsNullOrEmpty(_overrideTextValue))
                _overrideTextValue = text;
            else
                _overrideTextValue += "\n" + text;
        }

        public static void ForceShowDialogue(string text, Command? callingCommand, bool unlockMovement = true)
        {
            var topDown = GameObject.FindFirstObjectByType<Main_TopdownRoot>(FindObjectsInactive.Include);
            var dialog = topDown.GetSayDialogue(DialogueBoxType.Topdown_NoPortrait);
            dialog.SetActive(true);
            dialog.Writer.SetDisableInstanteComplete(true);
            dialog.NameText.text = "Archipelago";
            SayDialog.ActiveSayDialog = dialog;

            dialog.Say(text, true, true, true, false, false, null, () =>
            {
                try
                {
                    Globals.Logging.Warning("ForceShowDialogue", "Unlocking Movement");
                    if (unlockMovement)
                        topDown.Player.SetTopDownPlayerMovementState(true);

                    if (!callingCommand)
                        return;

                    callingCommand!.Continue();
                }
                catch (Exception e)
                {
                    Globals.Logging.Error("ForceShowDialogue (Inner)", e);
                    throw;
                }
            });

            //The flowchart of many of these cosmetics automatically unlock movement
            MelonEvents.OnUpdate.Subscribe(() =>
            {
                Globals.Logging.Warning("ForceShowDialogue", "Locking Movement");
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
                return true;

            if (!Override)
                return true;

            Override = false;
            __result = "-9999";
            return false;
        }
    }

    [HarmonyPatch(typeof(RemoveWeapon), "OnEnter")]
    public static class RemoveWeaponPatch
    {
        private static bool Prefix(Weapon ___playerWeapon)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return true;

            Globals.Logging.LogDebug("RemoveWeapon", $"Tried to remove the weapon: {___playerWeapon}");
            return false;
        }
    }

    [HarmonyPatch(typeof(UseItem), "OnEnter")]
    public static class UseItemPatch
    {
        private static bool Prefix(Item ___item)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return true;

            Globals.Logging.LogDebug("RemoveWeapon", $"Tried to use the item: {___item}");
            return false;
        }
    }
}