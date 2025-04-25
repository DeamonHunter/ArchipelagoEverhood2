using System.Collections.Generic;

namespace ArchipelagoEverhood.Data
{
    public static class ChestStorage
    {
        public static readonly List<ChestData> Chests = new()
        {
            
            new ChestData(100, "Neon_NeonJungle", "50xp", "GL_1A_NJ_ChestOpen", ChestType.XP), 
            new ChestData(101, "Neon_NeonJungle", "75xp", "GL_2A_NJ_ChestOpen", ChestType.XP), 
            new ChestData(102, "Neon_NeonJungle", "50xp", "GL_3A_NJ_ChestOpen", ChestType.XP),
            new ChestData(103, "Neon_NeonJungle", "75xp", "GL_4A_NJ_ChestOpen", ChestType.XP),
            new ChestData(104, "Neon_NeonJungle", "100xp", "GL_5A_NJ_ChestOpen", ChestType.XP),
            new ChestData(105, "Neon_NeonJungle", "0xp", "GL_6A_NJ_ChestOpen", ChestType.XP),
            new ChestData(106, "Neon_NeonJungle", "Power Gem", "GL_NJ_WeaponCrystalPickedUp", ChestType.Item),
            
            new ChestData(107, "Neon_HotelEntrance", "23 Key", "GL_1RoomKeyInventory", ChestType.Item),
            
            new ChestData(108, "Neon_NeonDistrict", "Hairstyle1_Anime", "NG_Hair1Spike", ChestType.Cosmetic),
            new ChestData(109, "Neon_NeonDistrict", "Mage_Hat", null, ChestType.Cosmetic), //This does not seem to have a variable. Likely because these don't use it.
            new ChestData(110, "Neon_NeonDistrict", "Hairstyle2_Wild", "NG_Hair2Wild", ChestType.Cosmetic),
            new ChestData(111, "Neon_NeonDistrict", "Hairstyle3_Backslick", "NG_Hair3Back", ChestType.Cosmetic),
            new ChestData(112, "Neon_NeonDistrict", "Hairstyle4_Stylish", "NG_Hair4Stylish", ChestType.Cosmetic),
            new ChestData(113, "Neon_NeonDistrict", "Hairstyle5_Natural", "NG_Hair5Natural", ChestType.Cosmetic),
            new ChestData(114, "Neon_NeonDistrict", "Hairstyle6_Afro", "NG_Hair6Afro", ChestType.Cosmetic),
            
        };
        /*
            //Blue Route - Neon Jungle Room 2
            "GL_3A_NJ_ChestOpen", //50xp
            "GL_4A_NJ_ChestOpen", //75xp
            //Blue Route - Neon Jungle Room 3
            "GL_5A_NJ_ChestOpen", //100xp
            "GL_6A_NJ_ChestOpen", //0xp with Gnome Laugh
            "GL_NJ_WeaponCrystalPickedUp", //Crystal 
            //Blue Route - Hillbert Hotel
            "GL_1RoomKeyInventory", //Gained from entering hotel (Room 23 Key)
            "GL_1A_HHe_ChestOpen", //100xp + Cat Ears + Cat Ears Bald. Gained from completing Human Party
            "GL_2RoomKeyInventory", //Gained from completing Human Party? (Gold Key)
            "GL_2A_HHe_ChestOpen", //100xp + Oingo Boingo Gained from completing VIP Area
            "GL_3RoomKeyInventory", //Based on map progression, Possibly Green 2? (Green Key)
            "GL_3A_HHe_ChestOpen", //Power Gem? Complete Green Floor
            "GL_4RoomKeyInventory", //Based on map progression, Possibly Green 3? (Pinecone Key)
            "GL_4A_HHe_ChestOpen", //Power Gem + Reindeer Skull. Based on map progression, Possibly Green 3? (Pinecone Key)
            //Green Route - Year 0 Starting Area
            "GL_1A_M1m_ChestOpen", //50xp
            "GL_M1m_ElectricSwitch_Removed", //Crystal
            //Green Route - Year 0 Hallway
            "GL_2A_M1h_ChestOpen", //Bandana //Todo: Block the other door so chest is not missable? Or set things up so re-entry is possible?
            //Red Route - Desert 1
            "GL_3_EWb_ChestOpen", //75xp
            "GL_4_EWb_ChestOpen", //50xp
            "GL_1_EWb_ChestOpen", //100xp
            "GL_2_EWb_ChestOpen", //Hotdog Cosmetic + 50xp
            //Red Route - Castle
            "GL_1A_EWd_ChestOpen", //50xp
            "GL_EWd_TrapsActive", //Crystal
            "GL_5_EWb_ChestOpen", //50xp
            //Floor 23 - Human Party
            "GL_3A_HH1_ChestOpen", //50xp
            "GL_2A_HH1_ChestOpen", //50xp
            "GL_1A_HH1_ChestOpen", //50xp
            //Floor Gold - The VIP Area
            "GL_1A_HH2_ChestOpen", //50xp
            //Floor Pinecone - The Simple Life
            "GL_1A_HH4_ChestOpen", //50xp
        */
    }
}