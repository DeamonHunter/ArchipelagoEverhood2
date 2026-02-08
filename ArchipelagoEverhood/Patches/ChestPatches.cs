using System;
using System.Collections.Generic;
using ArchipelagoEverhood.Data;
using Fungus;
using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArchipelagoEverhood.Patches
{
    [HarmonyPatch(typeof(GivePlayerItem), "OnEnter")]
    public static class GivePlayerItemPatch
    {
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

                if (!data.Achieved)
                {
                    __instance.Continue();
                    return false;
                }

                var itemText = Globals.EverhoodChests.GetItemName(data);
                if (___showDialogue || data.RewardConditions.HasFlag(RewardConditions.ForceShowDialogue))
                {
                    if (data.RewardConditions.HasFlag(RewardConditions.ForceShowDialogue) && data.Shown)
                    {
                        __instance.Continue();
                        return false;
                    }

                    data.Shown = true;
                    SayOnEnterPatch.ForceShowDialogue(itemText, __instance);
                }
                else
                {
                    SayOnEnterPatch.SetOverrideText(itemText);
                    __instance.Continue();
                }
            }
            catch (Exception e)
            {
                Globals.Logging.Error("GivePlayerItem", e);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(PickItem), "OnEnter")]
    public static class PickItemPatch
    {
        private static bool Prefix(PickItem __instance, Item ___item)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return true;

            try
            {
                Globals.Logging.Msg($"Unlocking Item: {___item}");
                var data = Globals.EverhoodChests.ChestOpened(new Dictionary<Item, int>() { { ___item, 1 } });
                if (data == null)
                {
                    //Banned item give aways
                    switch (___item)
                    {
                        case Item.WeaponToken:
                        case Item.DeathCoin:
                            Globals.Logging.Log("Pick Item", $"Preventing default pickup of {___item}.");
                            
                            __instance.Continue();
                            return false;
                            break;
                    }
                    
                    return true;
                }

                if (!data.Achieved)
                {
                    __instance.Continue();
                    return false;
                }

                var itemText = Globals.EverhoodChests.GetItemName(data);
                if (data.RewardConditions.HasFlag(RewardConditions.ForceShowDialogue))
                {
                    if (data.Shown)
                    {
                        __instance.Continue();
                        return false;
                    }

                    data.Shown = true;
                    SayOnEnterPatch.ForceShowDialogue(itemText, __instance);
                }
                else
                {
                    SayOnEnterPatch.SetOverrideText(itemText);
                    __instance.Continue();
                }
            }
            catch (Exception e)
            {
                Globals.Logging.Error("PickItem", e);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(GivePlayerArtifact), "OnEnter")]
    public static class GivePlayerArtifactPatch
    {
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

                if (!data.Achieved)
                {
                    __instance.Continue();
                    return false;
                }

                var itemText = Globals.EverhoodChests.GetItemName(data);
                if (___showDialogue || data.RewardConditions.HasFlag(RewardConditions.ForceShowDialogue))
                {
                    if (data.RewardConditions.HasFlag(RewardConditions.ForceShowDialogue) && data.Shown)
                    {
                        __instance.Continue();
                        return false;
                    }

                    data.Shown = true;
                    SayOnEnterPatch.ForceShowDialogue(itemText, __instance);
                }
                else
                {
                    SayOnEnterPatch.SetOverrideText(itemText);
                    __instance.Continue();
                }
            }
            catch (Exception e)
            {
                Globals.Logging.Error("GivePlayerArtifact", e);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(PickArtifact), "OnEnter")]
    public static class PickArtifactPatch
    {
        private static bool Prefix(PickArtifact __instance, Artifact ___artifact)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return true;

            try
            {
                Globals.Logging.Msg($"Unlocking Artifact: {___artifact}");
                var data = Globals.EverhoodChests.ChestOpened(new[] { ___artifact });
                if (data == null)
                    return true;

                if (!data.Achieved)
                {
                    __instance.Continue();
                    return false;
                }

                var itemText = Globals.EverhoodChests.GetItemName(data);
                if (data.RewardConditions.HasFlag(RewardConditions.ForceShowDialogue))
                {
                    if (data.Shown)
                    {
                        __instance.Continue();
                        return false;
                    }

                    data.Shown = true;
                    SayOnEnterPatch.ForceShowDialogue(itemText, __instance);
                }
                else
                {
                    SayOnEnterPatch.SetOverrideText(itemText);
                    __instance.Continue();
                }
            }
            catch (Exception e)
            {
                Globals.Logging.Error("PickArtifact", e);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(PickArtifactAndEquip), "OnEnter")]
    public static class PickArtifactAndEquipPatch
    {
        private static bool Prefix(PickArtifactAndEquip __instance, Artifact ___artifact)
        {
            if (!Globals.SessionHandler.LoggedIn || ___artifact == Artifact.None)
                return true;

            try
            {
                Globals.Logging.Msg($"Unlocking Artifact: {___artifact}");
                var data = Globals.EverhoodChests.ChestOpened(new[] { ___artifact });
                if (data == null)
                    return true;

                if (!data.Achieved)
                {
                    __instance.Continue();
                    return false;
                }

                var itemText = Globals.EverhoodChests.GetItemName(data);
                if (data.RewardConditions.HasFlag(RewardConditions.ForceShowDialogue))
                {
                    if (data.Shown)
                    {
                        __instance.Continue();
                        return false;
                    }

                    data.Shown = true;
                    SayOnEnterPatch.ForceShowDialogue(itemText, __instance);
                }
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

                if (!data.Achieved)
                {
                    __instance.Continue();
                    return false;
                }

                var itemText = Globals.EverhoodChests.GetItemName(data);
                if (data.RewardConditions.HasFlag(RewardConditions.ForceShowDialogue))
                {
                    if (data.Shown)
                    {
                        __instance.Continue();
                        return false;
                    }

                    data.Shown = true;
                    SayOnEnterPatch.ForceShowDialogue(itemText, __instance);
                }
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
        private static readonly HashSet<Cosmetics> _allowedNeonDistrictCosmetics = new()
        {
            Cosmetics.Hairstyle1_Anime,
            Cosmetics.Mage_Hat,
            Cosmetics.Hairstyle2_Wild,
            Cosmetics.Hairstyle3_Backslick,
            Cosmetics.Hairstyle4_Stylish,
            Cosmetics.Hairstyle5_Natural,
            Cosmetics.Hairstyle6_Afro,
        };

        private static bool Prefix(UnlockCosmetic __instance, Cosmetics ___cosmetic)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return true;

            try
            {
                var scene = SceneManager.GetSceneByName("IntroLevel");
                if (scene.isLoaded)
                {
                    Globals.Logging.Msg($"Used Mirror to try and unlock Cosmetic.");
                    __instance.Continue();
                    return false;
                }

                scene = SceneManager.GetSceneByName("Neon_NeonDistrict");
                if (scene.isLoaded)
                {
                    if (!_allowedNeonDistrictCosmetics.Contains(___cosmetic))
                    {
                        Globals.Logging.Log("UnlockCosmetic", $"Not unlocking '{___cosmetic}' as it is not a default cosmetic in the Neon District.");
                        return true;
                    }
                }

                Globals.Logging.Msg($"Unlocking Cosmetic: {___cosmetic}");
                var data = Globals.EverhoodChests.ChestOpened(___cosmetic);
                if (data == null)
                    return true;

                if (!data.Achieved || data.Shown)
                {
                    __instance.Continue();
                    return false;
                }

                var itemText = Globals.EverhoodChests.GetItemName(data);
                if (data.RewardConditions.HasFlag(RewardConditions.ForceShowDialogue))
                {
                    data.Shown = true;
                    SayOnEnterPatch.ForceShowDialogue(itemText, __instance);
                }
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

    [HarmonyPatch(typeof(SetPlayerCosmetic), "OnEnter")]
    public static class SetPlayerCosmeticPatch
    {
        private static bool Prefix(SetPlayerCosmetic __instance, Cosmetics ___cosmetic)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return true;

            if (Globals.ServicesRoot!.GameData.GeneralData.collectedCosmetics.Contains(___cosmetic))
                return true;

            Globals.Logging.LogDebug("SetPlayerCosmetic", "Ignoring Set: Don't have cosmetic.");
            __instance.Continue();
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

                if (!data.Achieved)
                {
                    __instance.Continue();
                    return false;
                }

                var itemText = Globals.EverhoodChests.GetItemName(data);
                if (data.RewardConditions.HasFlag(RewardConditions.ForceShowDialogue) || ___showDialogue)
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
                Globals.Logging.Error("GivePlayerXP", e);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(Say), "OnEnter")]
    public static class SayOnEnterPatch
    {
        private static string? _overrideTextValue;

        public static bool OverridenDialogue;

        private static HashSet<string> _bannedSays = new()
        {
            "Companion Dialogue box"
        };

        private static void Prefix(Say __instance, ref string ___overridingName,
            ref Characters ___characterByEnum, ref CharacterPortrait ___characterPortrait, ref TextType ___textType, ref bool ___showAlways,
            ref bool ___extendPrevious, ref bool ___waitForClick, ref Color ___overridingNameColor, ref Color ___overridingDialogueColor,
            ref bool ___disableInstantComplete, out bool __state)
        {
            __state = false;
            if (!Globals.SessionHandler.LoggedIn || _overrideTextValue == null)
                return;

            Globals.Logging.Log("Say", $"Triggered Override: {_overrideTextValue}");
            if (_bannedSays.Contains(__instance.gameObject.name))
            {
                Globals.Logging.Log("Say", "Skipped due to being inventory.");
                return;
            }

            __state = true;
            if (___characterByEnum != Characters.Description)
            {
                ___characterByEnum = Characters.Generic;
                ___characterPortrait = CharacterPortrait.None;
                ___overridingNameColor = Color.white;
                ___overridingDialogueColor = Color.white;
                ___disableInstantComplete = true;
            }

            ___textType = TextType.Normal;
            ___overridingName = "Archipelago";
            ___showAlways = true;
            ___extendPrevious = false;
            ___waitForClick = true;
            __instance.SetStoryText(_overrideTextValue);
            _overrideTextValue = null;
            SayGetStringIdPatch.Override = true;
            WriterProcessTokens.Override = true;
            OverridenDialogue = true;
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
            dialog.SetCharacter(null);
            dialog.SetCharacterImage(null);
            dialog.SetJustCharacterName("Archipelago");
            dialog.SetCharacterNameColor(Color.white);
            dialog.SetCharacterDialogueColor(Color.white);
            dialog.Writer.SetDisableInstanteComplete(true);
            SayDialog.ActiveSayDialog = dialog;
            WriterProcessTokens.Override = true;
            dialog.Say(text, true, true, true, false, false, null, () =>
            {
                try
                {
                    Globals.Logging.Warning("ForceShowDialogue", "Unlocking Movement");
                    dialog.Writer.SetDisableInstanteComplete(false);
                    if (unlockMovement)
                        topDown.Player.SetTopDownPlayerMovementState(true);

                    dialog.ClearContinue();
                    dialog.SetActive(false);
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

    [HarmonyPatch(typeof(Writer), "ProcessTokens")]
    public static class WriterProcessTokens
    {
        public static bool Override;
        private static bool _originalIgnore;
        private static float _originalSpeed;

        private static void Prefix(ref bool stopAudio, ref bool ___ignoreSettingsTextSpeed, ref float ___writingSpeed)
        {
            if (!Globals.SessionHandler.LoggedIn || !Override)
                return;

            stopAudio = true;
            _originalIgnore = ___ignoreSettingsTextSpeed;
            ___ignoreSettingsTextSpeed = true;
            _originalSpeed = ___writingSpeed;
            ___writingSpeed = 999999f;
        }

        private static void PostFix(ref bool stopAudio, ref bool ___ignoreSettingsTextSpeed, ref float ___writingSpeed)
        {
            if (!Globals.SessionHandler.LoggedIn || !Override)
                return;

            Override = false;
            ___ignoreSettingsTextSpeed = _originalIgnore;
            ___writingSpeed = _originalSpeed;
        }
    }
}