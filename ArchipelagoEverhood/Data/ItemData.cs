using System.Collections.Generic;
using Archipelago.MultiClient.Net.Enums;

namespace ArchipelagoEverhood.Data
{
    public static class ItemData
    {
        public static Dictionary<long, EverhoodItemInfo> ItemsById = new()
        {
            { 100, new EverhoodItemInfo { Item = Item.WeaponToken, ItemFlags = ItemFlags.Advancement, ItemName = "Power Gem" } },
            { 101, new EverhoodItemInfo { Item = Item.DeathCoin, ItemFlags = ItemFlags.Advancement, ItemName = "Soul Coin" } },
            { 106, new EverhoodItemInfo { Item = Item.RoomKey23, ItemFlags = ItemFlags.Advancement, ItemName = "Floor 23 Key" } },
            { 107, new EverhoodItemInfo { Item = Item.RoomKeyGold, ItemFlags = ItemFlags.Advancement, ItemName = "Floor Gold Key" } },
            { 108, new EverhoodItemInfo { Item = Item.RoomKeyGreen, ItemFlags = ItemFlags.Advancement, ItemName = "Floor Green Key" } },
            { 112, new EverhoodItemInfo { Item = Item.MoonInsignia, ItemFlags = ItemFlags.Advancement, ItemName = "Moon Insignia" } },
            { 113, new EverhoodItemInfo { Item = Item.SunInsignia, ItemFlags = ItemFlags.Advancement, ItemName = "Sun Insignia" } },
            { 114, new EverhoodItemInfo { Item = Item.TomatoSeed, ItemFlags = ItemFlags.None, ItemName = "Tomato Seed" } },
            { 115, new EverhoodItemInfo { Item = Item.Druffle, ItemFlags = ItemFlags.None, ItemName = "Druffle" } },

            { 0, new EverhoodItemInfo { Item = Item.RoomKeyPinecone, ItemFlags = ItemFlags.Advancement, ItemName = "Floor Pinecone Key" } },
            { 1, new EverhoodItemInfo { Item = Item.RoomKeyOmega, ItemFlags = ItemFlags.Advancement, ItemName = "Floor Omega Key" } },
            { 2, new EverhoodItemInfo { Item = Item.RavenKey, ItemFlags = ItemFlags.NeverExclude, ItemName = "Raven Hub Key" } },
            { 3, new EverhoodItemInfo { Item = Item.FrogKey, ItemFlags = ItemFlags.NeverExclude, ItemName = "Frog Hub Key" } },
            { 4, new EverhoodItemInfo { Item = Item.CrystalKey, ItemFlags = ItemFlags.Advancement, ItemName = "Crystal Key" } },
            { 5, new EverhoodItemInfo { Item = Item.PandemoniumKey, ItemFlags = ItemFlags.NeverExclude, ItemName = "Pandemonium Key" } },


            /*
            None = 0,
            MushroomSpore = 2,
            Hook = 3,
            SmegaByte = 4,
            OnionSeeds = 5,
            Schmuckles = 6,
            MacDaller = 8,
            Soulflower_Red = 9,
            Soulflower_Green = 10, // 0x0000000A
            Soulflower_Blue = 11, // 0x0000000B
            Catnip = 12, // 0x0000000C
            WeaponToken = 13, // 0x0000000D
            Riley = 1004, // 0x000003EC
            Telephone = 1009, // 0x000003F1
            YellowMask = 1010, // 0x000003F2
            LongPlank = 1011, // 0x000003F3
            RavenStar = 1012, // 0x000003F4
            DevGnomeTool = 1015, // 0x000003F7
            Cosmetics_MushroomHatQUEST = 1017, // 0x000003F9
            OraclesTounge = 1019, // 0x000003FB
            VIPTicket = 1020, // 0x000003FC
            EntityFlask = 1023, // 0x000003FF
            */
        };

        public static Dictionary<long, EverhoodItemInfo> GemPacksById = new()
        {
            { 110, new EverhoodItemInfo { GemPack = 3, ItemFlags = ItemFlags.Advancement, ItemName = "Power Gems x3" } },
            {
                111, new EverhoodItemInfo { GemPack = 25, ItemFlags = ItemFlags.NeverExclude, ItemName = "Power Gems x25" }
            }, //Todo: This invalidates any checks. Might need to deal with this another way.
        };

        public static Dictionary<long, EverhoodItemInfo> WeaponsById = new()
        {
            { 102, new EverhoodItemInfo { Weapon = Weapon.Axe, ItemFlags = ItemFlags.Advancement, ItemName = "Red Soul Axe" } },
            { 103, new EverhoodItemInfo { Weapon = Weapon.Spear, ItemFlags = ItemFlags.Advancement, ItemName = "Green Soul Spear" } },
            { 104, new EverhoodItemInfo { Weapon = Weapon.Bow, ItemFlags = ItemFlags.Advancement, ItemName = "Blue Soul Knives" } },
            { 105, new EverhoodItemInfo { Weapon = Weapon.Katana, ItemFlags = ItemFlags.NeverExclude, ItemName = "Katana" } },
        };

        public static Dictionary<long, EverhoodItemInfo> ArtifactsById = new()
        {
            { 109, new EverhoodItemInfo { Artifact = Artifact.CrimsonBandana, ItemFlags = ItemFlags.NeverExclude, ItemName = "Crimson Bandana" } },
        };

        public static Dictionary<long, EverhoodItemInfo> XpsById = new()
        {
            { 200, new EverhoodItemInfo { Xp = 0, ItemFlags = ItemFlags.None, ItemName = "0xp" } },
            { 201, new EverhoodItemInfo { Xp = 5, ItemFlags = ItemFlags.None, ItemName = "5xp" } },
            { 202, new EverhoodItemInfo { Xp = 15, ItemFlags = ItemFlags.None, ItemName = "15xp" } },
            { 203, new EverhoodItemInfo { Xp = 20, ItemFlags = ItemFlags.None, ItemName = "20xp" } },
            { 204, new EverhoodItemInfo { Xp = 25, ItemFlags = ItemFlags.None, ItemName = "25xp" } },
            { 205, new EverhoodItemInfo { Xp = 30, ItemFlags = ItemFlags.None, ItemName = "30xp" } },
            { 206, new EverhoodItemInfo { Xp = 35, ItemFlags = ItemFlags.None, ItemName = "35xp" } },
            { 207, new EverhoodItemInfo { Xp = 36, ItemFlags = ItemFlags.None, ItemName = "36xp" } },
            { 208, new EverhoodItemInfo { Xp = 40, ItemFlags = ItemFlags.None, ItemName = "40xp" } },
            { 209, new EverhoodItemInfo { Xp = 50, ItemFlags = ItemFlags.None, ItemName = "50xp" } },
            { 210, new EverhoodItemInfo { Xp = 60, ItemFlags = ItemFlags.None, ItemName = "60xp" } },
            { 211, new EverhoodItemInfo { Xp = 64, ItemFlags = ItemFlags.None, ItemName = "64xp" } },
            { 212, new EverhoodItemInfo { Xp = 75, ItemFlags = ItemFlags.None, ItemName = "75xp" } },
            { 213, new EverhoodItemInfo { Xp = 76, ItemFlags = ItemFlags.None, ItemName = "76xp" } },
            { 214, new EverhoodItemInfo { Xp = 80, ItemFlags = ItemFlags.None, ItemName = "80xp" } },
            { 215, new EverhoodItemInfo { Xp = 100, ItemFlags = ItemFlags.None, ItemName = "100xp" } },
            { 216, new EverhoodItemInfo { Xp = 150, ItemFlags = ItemFlags.None, ItemName = "150xp" } },
            { 217, new EverhoodItemInfo { Xp = 400, ItemFlags = ItemFlags.None, ItemName = "400xp" } },
        };

        public static Dictionary<long, EverhoodItemInfo> CosmeticsById = new()
        {
            { 400, new EverhoodItemInfo { Cosmetic = Cosmetics.Hairstyle1_Anime, ItemFlags = ItemFlags.None, ItemName = "Anime Hairstyle Cosmetic" } },
            { 401, new EverhoodItemInfo { Cosmetic = Cosmetics.Hairstyle2_Wild, ItemFlags = ItemFlags.None, ItemName = "Wild Hairstyle Cosmetic" } },
            { 402, new EverhoodItemInfo { Cosmetic = Cosmetics.Hairstyle3_Backslick, ItemFlags = ItemFlags.None, ItemName = "Backslick Hairstyle Cosmetic" } },
            { 403, new EverhoodItemInfo { Cosmetic = Cosmetics.Hairstyle4_Stylish, ItemFlags = ItemFlags.None, ItemName = "Stylish Hairstyle Cosmetic" } },
            { 404, new EverhoodItemInfo { Cosmetic = Cosmetics.Hairstyle5_Natural, ItemFlags = ItemFlags.None, ItemName = "Natural Hairstyle Cosmetic" } },
            { 405, new EverhoodItemInfo { Cosmetic = Cosmetics.CatEarsHair, ItemFlags = ItemFlags.None, ItemName = "Cat Ears Cosmetic" } },
            { 406, new EverhoodItemInfo { Cosmetic = Cosmetics.CatEarsBald, ItemFlags = ItemFlags.None, ItemName = "Cat Ears Bald Cosmetic" } },
            { 407, new EverhoodItemInfo { Cosmetic = Cosmetics.Reindeer_Skull, ItemFlags = ItemFlags.None, ItemName = "Reindeer Skull Cosmetic" } },
            { 408, new EverhoodItemInfo { Cosmetic = Cosmetics.Hotdog, ItemFlags = ItemFlags.None, ItemName = "Hotdog Cosmetic" } },
            { 409, new EverhoodItemInfo { Cosmetic = Cosmetics.Red_Bandana, ItemFlags = ItemFlags.None, ItemName = "Red Bandana Cosmetic" } },
            { 410, new EverhoodItemInfo { Cosmetic = Cosmetics.Hairstyle7_OingoBoingo, ItemFlags = ItemFlags.None, ItemName = "Oingo Boingo Cosmetic" } },
        };

        public static Dictionary<string, long> ItemIdsByName;
        public static Dictionary<long, EverhoodItemInfo> AllItemsByID;

        static ItemData()
        {
            ItemIdsByName = new Dictionary<string, long>();
            AllItemsByID = new Dictionary<long, EverhoodItemInfo>();
            foreach (var item in ItemsById)
            {
                ItemIdsByName.Add(item.Value.Item.ToString(), item.Key);
                AllItemsByID.Add(item.Key, item.Value);
            }

            foreach (var gemPack in GemPacksById)
            {
                ItemIdsByName.Add(gemPack.Value.ItemName, gemPack.Key);
                AllItemsByID.Add(gemPack.Key, gemPack.Value);
            }

            foreach (var weapon in WeaponsById)
            {
                ItemIdsByName.Add(weapon.Value.Weapon.ToString(), weapon.Key);
                AllItemsByID.Add(weapon.Key, weapon.Value);
            }

            foreach (var artifact in ArtifactsById)
            {
                ItemIdsByName.Add(artifact.Value.Artifact.ToString(), artifact.Key);
                AllItemsByID.Add(artifact.Key, artifact.Value);
            }

            foreach (var xp in XpsById)
            {
                ItemIdsByName.Add(xp.Value.Xp + "xp", xp.Key);
                AllItemsByID.Add(xp.Key, xp.Value);
            }

            foreach (var cosmetic in CosmeticsById)
            {
                ItemIdsByName.Add(cosmetic.Value.Cosmetic.ToString(), cosmetic.Key);
                AllItemsByID.Add(cosmetic.Key, cosmetic.Value);
            }
        }

        public class EverhoodItemInfo
        {
            public Artifact Artifact;
            public Cosmetics Cosmetic;
            public Item Item;
            public int GemPack;
            public int Xp;
            public Weapon Weapon;

            public string ItemName;
            public ItemFlags ItemFlags;
        }
    }
}