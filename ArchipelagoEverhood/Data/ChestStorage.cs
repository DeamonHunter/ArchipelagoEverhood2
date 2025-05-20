using System.Collections.Generic;

namespace ArchipelagoEverhood.Data
{
    public static class ChestStorage
    {
        private const int _itemStartId = 100;

        public static readonly List<ChestData> Chests = new()
        {
            new ChestData(_itemStartId, "Neon_NeonJungle", "50xp", "GL_1A_NJ_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 1, "Neon_NeonJungle", "75xp", "GL_2A_NJ_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 2, "Neon_NeonJungle", "50xp", "GL_3A_NJ_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 3, "Neon_NeonJungle", "75xp", "GL_4A_NJ_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 4, "Neon_NeonJungle", "100xp", "GL_5A_NJ_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 5, "Neon_NeonJungle", "0xp", "GL_6A_NJ_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 6, "Neon_NeonJungle", "Power Gem", "GL_NJ_WeaponCrystalPickedUp", ChestType.Item),

            new ChestData(_itemStartId + 7, "Neon_NeonDistrict", "Hairstyle1_Anime", "NG_Hair1Spike", ChestType.Cosmetic),
            new ChestData(_itemStartId + 8, "Neon_NeonDistrict", "Mage_Hat", null, ChestType.Cosmetic), //This does not seem to have a variable. Likely because these don't use it.
            new ChestData(_itemStartId + 9, "Neon_NeonDistrict", "Hairstyle2_Wild", "NG_Hair2Wild", ChestType.Cosmetic),
            new ChestData(_itemStartId + 10, "Neon_NeonDistrict", "Hairstyle3_Backslick", "NG_Hair3Back", ChestType.Cosmetic),
            new ChestData(_itemStartId + 11, "Neon_NeonDistrict", "Hairstyle4_Stylish", "NG_Hair4Stylish", ChestType.Cosmetic),
            new ChestData(_itemStartId + 12, "Neon_NeonDistrict", "Hairstyle5_Natural", "NG_Hair5Natural", ChestType.Cosmetic),
            new ChestData(_itemStartId + 13, "Neon_NeonDistrict", "Hairstyle6_Afro", "NG_Hair6Afro", ChestType.Cosmetic),

            new ChestData(_itemStartId + 14, "EternalWar_Battlefield", "100xp", "GL_1_EWb_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 15, "EternalWar_Battlefield", "Hotdog", "GL_2_EWb_ChestOpen", ChestType.Cosmetic),
            new ChestData(_itemStartId + 16, "EternalWar_Battlefield", "50xp", "GL_2_EWb_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 17, "EternalWar_Battlefield", "75xp", "GL_3_EWb_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 18, "EternalWar_Battlefield", "50xp", "GL_4_EWb_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 19, "EternalWar_Battlefield", "50xp", "GL_5_EWb_ChestOpen", ChestType.XP),

            new ChestData(_itemStartId + 20, "EternalWar_Dungeon", "50xp", "GL_1A_EWd_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 21, "EternalWar_Dungeon", "Power Gem", "GL_EWd_TrapsActive", ChestType.Item), //Todo: Sets false

            new ChestData(_itemStartId + 22, "Marzian_Part1Hero_Mines", "50xp", "GL_1A_M1m_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 23, "Marzian_Part1Hero_Mines", "Power Gem", "GL_M1m_ElectricSwitch_Removed", ChestType.Item),
            new ChestData(_itemStartId + 24, "Marzian_Part1Hero_MinesHallway", "CrimsonBandana", "GL_2A_M1h_ChestOpen",
                ChestType.Item), //Todo: Block the other door so chest is not missable? Or set things up so re-entry is possible?
            new ChestData(_itemStartId + 25, "Marzian_Part1Hero_MinesHallway", "Red_Bandana", "GL_2A_M1h_ChestOpen", ChestType.Cosmetic),

            //Todo: Hub Key
            new ChestData(_itemStartId + 26, "Neon_HotelEntrance", "23 Key", "GL_1RoomKeyInventory", ChestType.Item),
            new ChestData(_itemStartId + 27, "Neon_HotelEntrance", "100xp", "GL_1A_HHe_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 28, "Neon_HotelEntrance", "CatEarsBald", "GL_1A_HHe_ChestOpen", ChestType.Cosmetic),
            new ChestData(_itemStartId + 29, "Neon_HotelEntrance", "CatEarsHair", "GL_1A_HHe_ChestOpen", ChestType.Cosmetic),
            new ChestData(_itemStartId + 30, "GL_2RoomKeyInventory", "Gold Key", "GL_2RoomKeyInventory", ChestType.Item), //Requires beating 2 starting areas
            new ChestData(_itemStartId + 31, "Neon_HotelEntrance", "50xp", "GL_2A_HHe_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 32, "Neon_HotelEntrance", "Hairstyle7_OingoBoingo", "GL_2A_HHe_ChestOpen", ChestType.Cosmetic),

            new ChestData(_itemStartId + 33, "Neon_Hillbert_Room1", "50xp", "GL_1A_HH1_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 34, "Neon_Hillbert_Room1", "50xp", "GL_2A_HH1_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 35, "Neon_Hillbert_Room1", "50xp", "GL_3A_HH1_ChestOpen", ChestType.XP),

            new ChestData(_itemStartId + 36, "Neon_Hillbert_Room2Bobo", "50xp", "GL_1A_HH2_ChestOpen", ChestType.XP),
        };
        /*
            new ChestData(_itemStartId + X, "GL_3RoomKeyInventory", "Green Key", "GL_3RoomKeyInventory", ChestType.Item),  //Based on map progression, [Dot] Possibly Green 2/Year 1000 still need to test this.
            new ChestData(_itemStartId + X, "Neon_HotelEntrance", "Power Gem", "GL_3A_HHe_ChestOpen", ChestType.Item), //Power Gem, Complete Green Floor
            new ChestData(_itemStartId + X, "GL_4RoomKeyInventory", "Pinecone Key", "GL_4RoomKeyInventory", ChestType.Item), // Requires Green 3/Year 2000 completed (Pinecone Key)
            new ChestData(_itemStartId + X, "Neon_HotelEntrance", "Power Gem", "GL_4A_HHe_ChestOpen", ChestType.Item), //Power Gem. Requires Green 3/Year 2000 completed (Pinecone Key)
            new ChestData(_itemStartId + X, "Neon_HotelEntrance", "ReindeerSkull", "GL_4A_HHe_ChestOpen", ChestType.Cosmetic), // Reindeer Skull. Requires Green 3/Year 2000 completed (Pinecone Key)
            //Floor Pinecone - The Simple Life
            new ChestData(_itemStartId + X, "Neon_Hillbert_Room4", "50xp", "GL_1A_HH4_ChestOpen", ChestType.XP), //50xp
        */
    }
}
