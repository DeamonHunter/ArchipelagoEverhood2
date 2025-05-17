using System.Collections.Generic;

namespace ArchipelagoEverhood.Data
{
    public static class ItemData
    {
        public static Dictionary<long, Item> ItemsById = new()
        {
            { 100, Item.WeaponToken }, //Power Gems
            { 101, Item.DeathCoin }, //Soul Coin
            { 106, Item.RoomKey23 },
            { 107, Item.RoomKeyGold },
            { 108, Item.RoomKeyGreen },
            { 112, Item.MoonInsignia },
            { 113, Item.SunInsignia },
            { 114, Item.TomatoSeed },
            { 115, Item.Druffle },


            { 0, Item.RoomKeyPinecone },
            { 1, Item.RoomKeyOmega },
            { 2, Item.RavenKey },
            { 3, Item.FrogKey },
            { 4, Item.CrystalKey },
            { 5, Item.PandemoniumKey },

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

        public static Dictionary<long, int> GemPacks = new()
        {
            { 110, 3 },
            { 111, 25 }
        };

        public static Dictionary<long, Weapon> WeaponsById = new()
        {
            { 102, Weapon.Axe },
            { 103, Weapon.Spear },
            { 104, Weapon.Bow },
            { 105, Weapon.Katana },
        };

        public static Dictionary<long, Artifact> ArtifactsById = new()
        {
            { 109, Artifact.CrimsonBandana }
        };

        public static Dictionary<long, int> XpsById = new()
        {
            { 200, 0 },
            { 201, 5 },
            { 202, 15 },
            { 203, 20 },
            { 204, 25 },
            { 205, 30 },
            { 206, 35 },
            { 207, 36 },
            { 208, 40 },
            { 209, 50 },
            { 210, 60 },
            { 211, 64 },
            { 212, 75 },
            { 213, 76 },
            { 214, 80 },
            { 215, 100 },
            { 216, 150 },
            { 217, 400 },
            //{218, 0},
            //{219, 0},
        };

        public static Dictionary<long, Cosmetics> CosmeticsById = new()
        {
            { 400, Cosmetics.Hairstyle1_Anime },
            { 401, Cosmetics.Hairstyle2_Wild },
            { 402, Cosmetics.Hairstyle3_Backslick },
            { 403, Cosmetics.Hairstyle4_Stylish },
            { 404, Cosmetics.Hairstyle5_Natural },
            { 405, Cosmetics.CatEarsHair },
            { 406, Cosmetics.CatEarsBald },
            { 407, Cosmetics.Reindeer_Skull },
            { 408, Cosmetics.Hotdog },
            { 409, Cosmetics.Red_Bandana },
            { 410, Cosmetics.Hairstyle7_OingoBoingo },
        };

        public static Dictionary<string, long> ItemIdsByName;

        static ItemData()
        {
            ItemIdsByName = new Dictionary<string, long>();
            foreach (var item in ItemsById)
                ItemIdsByName.Add(item.Value.ToString(), item.Key);
            foreach (var gemPack in GemPacks)
                ItemIdsByName.Add(gemPack.Value.ToString(), gemPack.Key);
            foreach (var weapon in WeaponsById)
                ItemIdsByName.Add(weapon.Value.ToString(), weapon.Key);
            foreach (var artifact in ArtifactsById)
                ItemIdsByName.Add(artifact.Value.ToString(), artifact.Key);
            foreach (var xp in XpsById)
                ItemIdsByName.Add(xp.Value + "xp", xp.Key);
            foreach (var cosmetic in CosmeticsById)
                ItemIdsByName.Add(cosmetic.Value.ToString(), cosmetic.Key);
        }


    }
}