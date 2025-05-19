using System.Linq;
using Archipelago.MultiClient.Net.Enums;

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
    }
}