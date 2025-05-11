using Fungus;
using HarmonyLib;

namespace ArchipelagoEverhood.Patches
{
    [HarmonyPatch(typeof(MoveLean), "ExecuteTween")]
    public class MoveLeanPatch
    {
        private static bool Prefix(TransformData ____toTransform)
        {
            if (!Globals.SessionHandler.LoggedIn || !____toTransform.transformVal)
                return true;

            //Todo: Block other doors.
            if (____toTransform.transformVal.name == "HallOfCon_Door")
                return false;

            return true;
        }
    }

    [HarmonyPatch(typeof(SetActive), "OnEnter")]
    public class SetActivePatch
    {
        private static bool Prefix(GameObjectData ____targetGameObject)
        {
            if (!Globals.SessionHandler.LoggedIn || !____targetGameObject.gameObjectVal)
                return true;

            //Todo: Block other doors.
            switch (____targetGameObject.gameObjectVal.name)
            {
                case "LevelLoad-HallOfCon-Start":
                case "LevelLoad-HallOfCon-Mirror":
                case "LevelLoad-HallOfCon-PostDragon":
                    return false;
                default:
                    return true;
            }
        }
    }
}