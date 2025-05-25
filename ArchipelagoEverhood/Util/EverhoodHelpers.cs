using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Archipelago.MultiClient.Net.Enums;
using UnityEngine;

namespace ArchipelagoEverhood.Util
{
    public static class EverhoodHelpers
    {
        public static string ConstructCollectedItemText(string itemName, ItemFlags flags, string? otherPlayer, bool withTags)
        {
            var itemText = ConstructItemText(itemName, flags, withTags);
            return otherPlayer == null
                ? $"You found your {itemText}!"
                : $"You found {otherPlayer}'s {itemText}";
        }

        public static string ConstructItemText(string itemName, ItemFlags flags, bool withTags)
        {
            //Somewhat janky setup so that it looks nice in all texts.
            string sprite;
            if (flags.HasFlag(ItemFlags.Advancement))
                sprite = "<sprite=248>";
            else if (flags.HasFlag(ItemFlags.NeverExclude))
                sprite = "<sprite=249>";
            else if (flags.HasFlag(ItemFlags.Trap))
                sprite = "<sprite=251>";
            else
                sprite = "<sprite=250>";

            return withTags
                ? $"<voffset=5><cspace=-10>{sprite}</voffset>{itemName.FirstOrDefault()}</cspace>{itemName[1..]}"
                : $"{sprite}{itemName}";
        }

        public static int GetItemCount(string itemName)
        {
            var data = Globals.ServicesRoot!.GameData.GeneralData;
            return data.GetCollectItemCount(itemName) + data.GetUsedItemCount(itemName);
        }

        public static bool TryGetGameObjectWithName(string name, IEnumerable<GameObject> gameObjects, [NotNullWhen(returnValue: true)] out GameObject? found)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.name != name)
                    continue;

                found = gameObject;
                return true;
            }

            found = null;
            return false;
        }

        public static bool TryGetChildWithName(string name, GameObject gameObject, [NotNullWhen(returnValue: true)] out Transform? found)
            => TryGetChildWithName(name, gameObject.transform, out found);

        public static bool TryGetChildWithName(string name, Transform transform, [NotNullWhen(returnValue: true)] out Transform? found)
        {
            foreach (Transform child in transform)
            {
                if (child.name != name)
                    continue;

                found = child;
                return true;
            }

            found = null;
            return false;
        }

        public static bool HasFlag(string flag)
        {
            if (!Globals.ServicesRoot!.GameData.GeneralData.boolVariables.TryGetValue(flag, out var boolValue))
            {
                Globals.Logging.Error($"{flag} not successfully unlocked: Variable doesn't exist yet.");
                return false;
            }

            if (boolValue)
            {
                Globals.Logging.Error($"{flag} successfully unlocked. {boolValue}/True");
                return true;
            }

            Globals.Logging.Error($"{flag} not successfully unlocked. {boolValue}/True");
            return false;
        }

        public static bool HasFlag(string flag, int count)
        {
            if (!Globals.ServicesRoot!.GameData.GeneralData.intVariables.TryGetValue(flag, out var intValue))
            {
                Globals.Logging.Error($"{flag} not successfully unlocked: Variable doesn't exist yet.");
                return false;
            }

            if (intValue >= count)
            {
                Globals.Logging.Error($"{flag} successfully unlocked. {intValue}/{count}");
                return true;
            }

            Globals.Logging.Error($"{flag} not successfully unlocked. {intValue}/{count}");
            return false;
        }
    }
}