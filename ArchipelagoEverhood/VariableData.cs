using System.Collections.Generic;

namespace ArchipelagoEverhood
{
    public class VariableData
    {
        /// <summary>
        /// Cleans up the log once we stop needing to track things.
        /// </summary>
        private const bool _alwaysFail = true;
 
        private static HashSet<string> _chests = new HashSet<string>()
        {
            //Blue Route - Neon Jungle Room 1
            "GL_1A_NJ_ChestOpen", //50xp
            "GL_2A_NJ_ChestOpen", //75xp
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
        };

        private static HashSet<string> _itemVariabless = new HashSet<string>()
        {
            "NG_Cosmetic_RedBandana", //Gotten after Bandana Item (GL_2A_M1h_ChestOpen)
        };

        private static HashSet<string> _areaVariables = new HashSet<string>()
        {
            "GL_MarzianPhased" //Green Door 1, 
        };

        private static HashSet<string> _progressionVariables = new HashSet<string>()
        {
            "GL_MarzianPart_Blocked", //Possibly controls green door?
            "GL_MarzianPart1Raven", //Possibly controls what happens after green?
        };
        
        /// <summary>
        /// A set of mostly useless values for checking against for newer variables.
        /// </summary>
        private static HashSet<string> _globalValues = new HashSet<string>()
        {
            // Tutorial
            "GL_MovementTutorialBattle", //Set upon starting the first shade battle.
            "GL_Intro_ShadePickupMedium", //Set upon starting the medium version of the shade fight.
            "GL_Intro2_IgnoreRaven", //A variable for the shade fight to ignore you?
            "GL_Intro2_RavenTriggered", //A variable for raven getting triggered? Probably cutscene.
            "GL_RestartedTutorial", //Set upon starting Raven Battle?
            "GL_BigTestAttempt", //Set upon starting Raven Battle?
            
            //Main Hub
            "GL_DEMO", //Set at the end of the route area. Always 0, likely due to demo
            "GL_LucyPresent",
            "GL_FirstAdvice",
            "GL_RavensStars",
            
            // Green Area
            "GL_M1_PlayerIntro", //Set on entering green area 1.
            "GL_MarzianPart1_Balanced", //???
            "GL_M1m_StartRileyRescue", //Set on starting cutscene with Riley
            "GL_M1m_EndRileyRescue", //End Riley Sequence.
            "GL_M1m_PickedUpRiley",
            "GL_M1m_ElectricSwitch_Removed",
            "GL_1A_M1m_SharkJailorDeadComment",
            "GL_M1m_FirstRescue",
            "GL_1A_M1m_Marzian",
            "GL_5A_M1m_Marzian",
            "GL_1A_M1m_HyenaDeadComment",
            "GL_6A_M1m_Marzian",
            "GL_3A_M1m_Marzian",
            "GL_2A_M1m_Marzian",
            "GL_4A_M1m_Marzian",
            "GL_B1_M1ChasersComment",
            "GL_B1_M1_EncounterDeadComment",
            "GL_M1h_ElectricSwitch",
            "GL_B2_M1_EncounterDeadComment",
        };


        public static bool KnownVariable(string name) => !_alwaysFail && (_globalValues.Contains(name) || _itemVariabless.Contains(name) || _chests.Contains(name));
    }
}