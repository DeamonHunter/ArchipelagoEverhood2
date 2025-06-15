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
            new ChestData(_itemStartId + 6, "Neon_NeonJungle", "WeaponToken", "GL_NJ_WeaponCrystalPickedUp", ChestType.Item),

            new ChestData(_itemStartId + 7, "Neon_NeonDistrict", "Hairstyle1_Anime", "NG_Hair1Spike", ChestType.Cosmetic), //Given on startup
            new ChestData(_itemStartId + 8, "Neon_NeonDistrict", "Mage_Hat", null, ChestType.Cosmetic, true), //This does not seem to have a variable. Likely because these don't use it.
            new ChestData(_itemStartId + 9, "Neon_NeonDistrict", "Hairstyle2_Wild", "NG_Hair2Wild", ChestType.Cosmetic, true),
            new ChestData(_itemStartId + 10, "Neon_NeonDistrict", "Hairstyle3_Backslick", "NG_Hair3Back", ChestType.Cosmetic, true),
            new ChestData(_itemStartId + 11, "Neon_NeonDistrict", "Hairstyle4_Stylish", "NG_Hair4Stylish", ChestType.Cosmetic, true),
            new ChestData(_itemStartId + 12, "Neon_NeonDistrict", "Hairstyle5_Natural", "NG_Hair5Natural", ChestType.Cosmetic, true),
            new ChestData(_itemStartId + 13, "Neon_NeonDistrict", "Hairstyle6_Afro", "NG_Hair6Afro", ChestType.Cosmetic, true),

            new ChestData(_itemStartId + 14, "EternalWar_Battlefield", "100xp", "GL_1_EWb_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 15, "EternalWar_Battlefield", "Hotdog", "GL_2_EWb_ChestOpen", ChestType.Cosmetic),
            new ChestData(_itemStartId + 16, "EternalWar_Battlefield", "50xp", "GL_2_EWb_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 17, "EternalWar_Battlefield", "75xp", "GL_3_EWb_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 18, "EternalWar_Battlefield", "50xp", "GL_4_EWb_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 19, "EternalWar_Battlefield", "50xp", "GL_5_EWb_ChestOpen", ChestType.XP),

            new ChestData(_itemStartId + 20, "EternalWar_Dungeon", "50xp", "GL_1A_EWd_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 21, "EternalWar_Dungeon", "WeaponToken", "GL_EWd_TrapsActive", ChestType.Item, expectedValue: false),

            new ChestData(_itemStartId + 22, "Marzian_Part1Hero_Mines", "50xp", "GL_1A_M1m_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 23, "Marzian_Part1Hero_Mines", "WeaponToken", "GL_M1m_ElectricSwitch_Removed", ChestType.Item),
            new ChestData(_itemStartId + 24, "Marzian_Part1Hero_MinesHallway", "CrimsonBandana", "GL_2A_M1h_ChestOpen",
                ChestType.Item), //Todo: Block the other door so chest is not missable? Or set things up so re-entry is possible?
            new ChestData(_itemStartId + 25, "Marzian_Part1Hero_MinesHallway", "Red_Bandana", "GL_2A_M1h_ChestOpen", ChestType.Cosmetic),

            //Todo: Hub Key
            new ChestData(_itemStartId + 26, "Neon_HotelEntrance", "RoomKey23", "GL_1RoomKeyInventory", ChestType.Item),
            new ChestData(_itemStartId + 27, "Neon_HotelEntrance", "100xp", "GL_1A_HHe_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 28, "Neon_HotelEntrance", "CatEarsBald", "GL_1A_HHe_ChestOpen", ChestType.Cosmetic),
            new ChestData(_itemStartId + 29, "Neon_HotelEntrance", "CatEarsHair", "GL_1A_HHe_ChestOpen", ChestType.Cosmetic),
            new ChestData(_itemStartId + 30, "GL_2RoomKeyInventory", "RoomKeyGold", "GL_2RoomKeyInventory", ChestType.Item), //Requires beating 2 starting areas
            new ChestData(_itemStartId + 31, "Neon_HotelEntrance", "50xp", "GL_2A_HHe_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 32, "Neon_HotelEntrance", "Hairstyle7_OingoBoingo", "GL_2A_HHe_ChestOpen", ChestType.Cosmetic),

            new ChestData(_itemStartId + 33, "Neon_Hillbert_Room1", "50xp", "GL_1A_HH1_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 34, "Neon_Hillbert_Room1", "50xp", "GL_2A_HH1_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 35, "Neon_Hillbert_Room1", "50xp", "GL_3A_HH1_ChestOpen", ChestType.XP),

            new ChestData(_itemStartId + 36, "Neon_Hillbert_Room2Bobo", "50xp", "GL_1A_HH2_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 37, "Neon_HotelEntrance", "RoomKeyGreen", "GL_3RoomKeyInventory", ChestType.Item), //Based on map progression, [Dot] Possibly Green 2/Year 1000 still need to test this.
            
            //Add to the other HotelEntrance/Key items on the list
            new ChestData(_itemStartId + 39, "IntroLevel", "Clock", "GL_MovementTutorialBattle", ChestType.Item),
            new ChestData(_itemStartId + 41, "Neon_HotelEntrance", "WeaponToken", "GL_3A_HHe_ChestOpen", ChestType.Item), //Power Gem, Complete Green Floor
            new ChestData(_itemStartId + 42, "Neon_HotelEntrance", "RoomKeyPinecone", "GL_4RoomKeyInventory", ChestType.Item), // Requires Green 3/Year 2000 completed (RoomKeyPinecone)
            new ChestData(_itemStartId + 43, "Neon_HotelEntrance", "WeaponToken", "GL_4A_HHe_ChestOpen", ChestType.Item), //Power Gem. Requires Green 3/Year 2000 completed (RoomKeyPinecone)
            new ChestData(_itemStartId + 44, "Neon_HotelEntrance", "ReindeerSkull", "GL_4A_HHe_ChestOpen", ChestType.Cosmetic), // Reindeer Skull. Requires Green 3/Year 2000 completed (RoomKeyPinecone)
            new ChestData(_itemStartId + 45, "Neon_HotelEntrance", "RoomKeyOmega", "GL_5RoomKeyInventory", ChestType.Item), // Requires beating Vanguard3D in Irvine's Pocket Dimension
            new ChestData(_itemStartId + 46, "Neon_HotelEntrance", "WeaponToken", "GL_5A_HHe_ChestOpen", ChestType.Item), // After Omega Room
            new ChestData(_itemStartId + 47, "Neon_HotelEntrance", "JesterHat", "NG_Cosmetic_JesterHat", ChestType.Cosmetic), // After Omega Room
            //Floor Pinecone - The Simple Life
            new ChestData(_itemStartId + 48, "Neon_Hillbert_Room4", "50xp", "GL_1A_HH4_ChestOpen", ChestType.XP),
            //Dragon gives soul weapon but I didn't see the command for it, will need to recheck. 
            //Year 3000 (accessible after Dragon)
            new ChestData(_itemStartId + 49, "Marzian_Part3Hero_Wasteland", "100xp", "GL_1A_M4_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 50, "Marzian_Part3Hero_City", "50xp", "GL_1C_M4_ChestOpen", ChestType.XP),
            //Year 4000
            new ChestData(_itemStartId + 51, "Marzian_Part4Hero", "WeaponToken", "GL_1C_M5_ChestOpen", ChestType.Item),
            //Mushroom Forest (accessible after Dragon)
            new ChestData(_itemStartId + 52, "MushroomBureau_SunPath", "SunInsignia", "GL_MB_Sun", ChestType.Item),
            new ChestData(_itemStartId + 53, "MushroomBureau_MoonPath", "WeaponToken", "GL_1A_MBm_ChestOpen", ChestType.Item),
            new ChestData(_itemStartId + 54, "MushroomBureau_MoonPath", "MoonInsignia", "GL_MB_Moon", ChestType.Item),
            new ChestData(_itemStartId + 55, "MushroomBureau_MoonPath", "DeathCoin", "GL_MB_DeathCoinPickedUp", ChestType.Item),
            //Lucy Room 266 888
            new ChestData(_itemStartId + 56, "LucyRoom", "Duality", "GL_HH_LucyEncounterOutro", ChestType.Item),
            //Sam's Room 111 568
            new ChestData(_itemStartId + 57, "SamRoom", "CrystalKey", "GL_SR_CrystalKeyPickedUp", ChestType.Item),
            //Year 5000 (requires Crystal Key)
            //new ChestData(_itemStartId + 58, "Marzian_Part5Hero", "SoulWeapon", "GL_M6_WeaponPickedUp", ChestType.Item),
            //Lab (gives code 888 688 for Colosseum)
            new ChestData(_itemStartId + 59, "Lab", "35xp", "GL_1A_LAB_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 60, "Lab", "Clover", "GL_3A_LAB_ChestOpen", ChestType.Item),
            new ChestData(_itemStartId + 61, "Lab", "WeaponToken", "GL_2A_LAB_ChestOpen", ChestType.Item),
            new ChestData(_itemStartId + 110, "Lab", "Katana", "Add Katana", ChestType.Item), // I don't know what to do about the katana, Add Katana was the only thing I saw.
            //Omega Room
            new ChestData(_itemStartId + 62, "Neon_Hillbert_Room5", "WeaponToken", "GL_1A_HH5_ChestOpen", ChestType.Item),
            //Liminal Rooms, death coin door
            new ChestData(_itemStartId + 63, "DeathCoinDoor_LiminalRooms", "WeaponToken", "GL_1A_LH_ChestOpen", ChestType.Item),
            new ChestData(_itemStartId + 64, "DeathCoinDoor_LiminalRooms", "WeaponToken", "GL_2A_LH_ChestOpen", ChestType.Item),
            //Smega Station
            new ChestData(_itemStartId + 65, "Smega_Start", "50xp", "GL_1A_SMmb_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 66, "Smega_Start", "50xp", "GL_3A_SSmb_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 67, "Smega_Start", "WeaponToken", "GL_4A_SSmb_ChestOpen", ChestType.Item),
            new ChestData(_itemStartId + 68, "Smega_Audio", "50xp", "GL_2A_SSmb_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 69, "Smega_Audio", "GasMask", "GL_SS_GasMaskGiven", ChestType.Cosmetic), // Speaking to Doctor Dump after fixing issues in RAM
            new ChestData(_itemStartId + 70, "Smega_Audio", "WeaponToken", "GL_SS_GasMaskGiven", ChestType.Item), // Speaking to Doctor Dump after fixing issues in RAM
            new ChestData(_itemStartId + 71, "Smega_RAM", "50xp", "GL_SSRAM1_ChestOpen", ChestType.XP),
            //new ChestData(_itemStartId + X, "Smega_RAM", "", "GL_SSRAM2_ChestOpen", ChestType.), // Not a real chest, turns into a rocket and leaves
            new ChestData(_itemStartId + 72, "Smega_RAM", "100xp", "GL_SSRAM3_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 73, "Smega_RAM", "50xp", "GL_SSRAM4_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 74, "Smega_RAM", "WeaponToken", "GL_SSRAM5_ChestOpen", ChestType.Item),
            //new ChestData(_itemStartId + X, "Smega_RAM", "50xp", "GL_SSRAM6_ChestOpen", ChestType.XP), // Couldn't find, unsure if exists
            new ChestData(_itemStartId + 75, "Smega_RAM", "50xp", "GL_SSRAM7_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 76, "Smega_RAM", "WeaponToken", "GL_SSRAM8_ChestOpen", ChestType.Item),
            new ChestData(_itemStartId + 77, "Smega_RAM", "50xp", "GL_SSRAM9_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 78, "Smega_RAM", "WeaponToken", "GL_SSRAM10_ChestOpen", ChestType.Item),
            new ChestData(_itemStartId + 79, "Smega_RAM", "50xp", "GL_SSRAM11_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 80, "Smega_RAM", "50xp", "GL_SSRAM12_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 81, "Smega_RAM", "WeaponToken", "GL_SSRAM13_ChestOpen", ChestType.Item),
            new ChestData(_itemStartId + 82, "Smega_RAM", "50xp", "GL_SSRAM14_ChestOpen", ChestType.XP),
            //[Dot] I didn't find GL_1A_SSp_ChestOpen
            new ChestData(_itemStartId + 83, "Smega_Processor", "WeaponToken", "GL_2A_SSp_ChestOpen", ChestType.Item),
            new ChestData(_itemStartId + 84, "Smega_Processor", "WeaponToken", "GL_3A_SSp_ChestOpen", ChestType.Item),
            new ChestData(_itemStartId + 85, "Smega_Processor", "50xp", "GL_4A_SSp_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 86, "Smega_Processor", "50xp", "GL_5A_SSp_ChestOpen", ChestType.XP),
            new ChestData(_itemStartId + 87, "Smega_Processor", "50xp", "GL_6A_SSp_ChestOpen", ChestType.XP),
            //Death Mountain
            new ChestData(_itemStartId + 88, "DeathMountain", "50xp", "Chest_1A_BI", ChestType.XP),
            new ChestData(_itemStartId + 89, "DeathMountain", "WeaponToken", "Chest_2A_BI", ChestType.Item),
            new ChestData(_itemStartId + 90, "DeathMountain", "WeaponToken", "Chest_3A_BI", ChestType.Item),
            new ChestData(_itemStartId + 91, "DeathMountain", "50xp", "Chest_4A_BI", ChestType.XP),
            new ChestData(_itemStartId + 92, "DeathMountain", "50xp", "Chest_5A_BI", ChestType.XP),
            new ChestData(_itemStartId + 93, "DeathMountain", "50xp", "Chest_6A_BI", ChestType.XP),
            new ChestData(_itemStartId + 94, "DeathMountain", "DeathCoin", "Chest_7A_BI", ChestType.Item), // Secret Village, for the Scholar Puzzle
            new ChestData(_itemStartId + 95, "DeathMountain", "50xp", "GL_BI_VictoryTrigger", ChestType.XP), // actually a "[Command] Pick 1 DeathCoin" but this may be better?
            //Everhood 1
            new ChestData(_itemStartId + 96, "Everhood1", "V.I.P.Ticket", "GL_DE_GreenMageBriefingOutside", ChestType.Item), // [Command] Pick 1 VIPTicket
            new ChestData(_itemStartId + 97, "Everhood1", "LongPlank", "GL_DE_RastaInteracted", ChestType.Item), // [Command] Pick 1 LongPlank
            new ChestData(_itemStartId + 98, "Everhood1", "YellowMask", "GL_DE_BlueDead", ChestType.Item), // Not sure what to do for YellowMask. [Command] YellowMask = False, and GL_DE_BlueDead == True
            new ChestData(_itemStartId + 99, "Everhood1", "DeathCoin", "GL_DE_DeathCoin", ChestType.Item),
            //new ChestData(_itemStartId + 100, "Everhood1", "SoulWeapon", "GL_DE_DeathCoin", ChestType.Item),
            //Pandemonium
            //new ChestData(_itemStartId + 100, "Pandemonium", "PandemoniumKey", "GL_Pa_SproutPKey", ChestType.Item), // from speaking to Sprout
            //The Colosseum, without using a code is accessed at Pandemonium
            new ChestData(_itemStartId + 101, "Colosseum", "WeaponToken", "GL_GauntletQuest1_Finished", ChestType.Item), // [Command] recieveitem, [Command] Pick 1 WeaponToken
            new ChestData(_itemStartId + 102, "Colosseum", "Knight_Helmet", "GL_GauntletQuest1_Finished", ChestType.Cosmetic), // [Command] recieveitem, [Command] Knight_Helmet
            new ChestData(_itemStartId + 103, "Colosseum", "DeathCoin", "GL_GauntletQuest2_Finished", ChestType.Item),
            new ChestData(_itemStartId + 104, "Colosseum", "WeaponToken", "GL_GauntletQuest3_Finished", ChestType.Item), // [Command] recieveitem [Command] Pick 2 WeaponToken
            new ChestData(_itemStartId + 105, "Colosseum", "WeaponToken", "GL_GauntletQuest4_Finished", ChestType.Item), // or GL_DoubleDsDragonArenaBattle, [Command] Pick 3 WeaponToken
            //new ChestData(_itemStartId + X, "Colosseum", "WeaponToken", "GL_GauntletQuest5_Finished", ChestType.Item), // Post ShadePostCredits, may need GL_GauntletQuest5_Finished. [Command] Pick 2 WeaponToken
            //new ChestData(_itemStartId + X, "Colosseum", "WeaponToken", "GL_GauntletQuest6_Finished", ChestType.Item), // No fight, [Command] Pick 2 WeaponToken
            // Pandemonium boatman journey, requires 1 Death Coin
            new ChestData(_itemStartId + 108, "Pandemonium", "WeaponToken", "GL_1A_SEnd1_ChestOpen", ChestType.Item),
            //Torment Room 222 883
            new ChestData(_itemStartId + 109, "TormentRoom", "WeaponToken", "GL_1A_SEnd6_ChestOpen", ChestType.Item),
            //new ChestData(_itemStartId + 110, "Raven Hub", "SoulWeapon", "GL_DE_DeathCoin", ChestType.Item),
        };
    }
}
