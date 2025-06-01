using Fungus;
using HarmonyLib;

namespace ArchipelagoEverhood.Patches
{
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
        private static bool Prefix(UseItem __instance, Item ___item)
        {
            if (!Globals.SessionHandler.LoggedIn)
                return true;

            switch (___item)
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
                    Globals.Logging.LogDebug("RemoveWeapon", $"Tried to use the item: {___item}");
                    __instance.Continue();
                    return false;

                default:
                    return true;
            }
        }
    }
}