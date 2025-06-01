using System;
using Archipelago.MultiClient.Net.Enums;
using ArchipelagoEverhood.Archipelago;
using ArchipelagoEverhood.Util;
using Fungus;
using HarmonyLib;

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
                        SayOnEnterPatch.ForceShowDialogue($"You need 3 {EverhoodHelpers.ConstructItemText("Power Gems", ItemFlags.Advancement, false)} fight the dragon!", __instance);
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
}