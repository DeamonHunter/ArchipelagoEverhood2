using System.Collections.Generic;

namespace ArchipelagoEverhood.Data
{
    public static class BattleStorage
    {
        private const int _battleStartId = 500;

        public static readonly List<BattleData> Battles = new()
        {
            //Intro
            new BattleData(_battleStartId + 0, "Tutorial2-Battle", "GL_Intro_RavenIntro", BattleType.MajorBattle, 0), //Todo: This one is touchy? "GL_RavenBattle_Dead" is set after battle.

            //Blue Route - Pre City
            new BattleData(_battleStartId + 1, "SpringHead-Battle", "GL_1A_ND_SpringHeadDead", BattleType.Trash, 20), //20xp + Druffle
            //Blue Route - Neon Jungle Room 1
            new BattleData(_battleStartId + 2, "SpringHead-Battle", "GL_1A_NJ_SpringHeadDead", BattleType.Trash, 20), //20xp + Druffle
            new BattleData(_battleStartId + 3, "SpringHead-Battle", "GL_2A_NJ_SpringHeadDead", BattleType.Trash, 20), //20xp + Druffle
            new BattleData(_battleStartId + 4, "SpringHead-Battle", "GL_3A_NJ_SpringHeadDead", BattleType.Trash, 40), //Double Battle.
            new BattleData(_battleStartId + 5, "DarkPiranha-Battle", "GL_1A_NJ_DarkPiranhaDead", BattleType.Trash, 30), //30xp
            new BattleData(_battleStartId + 6, "DarkPiranha-Battle", "GL_2A_NJ_DarkPiranhaDead", BattleType.Trash, 30), //30xp
            //Blue Route - Neon Jungle Room 2
            new BattleData(_battleStartId + 7, "Homonculus-Battle", "GL_1A_NJ_HomonculusEncounter ", BattleType.MajorBattle, 35), //35xp
            new BattleData(_battleStartId + 8, "SpringHead-Battle", "GL_1B_NJ_SpringHeadDead", BattleType.Trash, 20), //20xp + Druffle
            new BattleData(_battleStartId + 9, "SpringHead-Battle", "GL_2B_NJ_SpringHeadDead", BattleType.Trash, 20), //20xp + Druffle
            new BattleData(_battleStartId + 10, "SpringHead-Battle", "GL_3B_NJ_SpringHeadDead", BattleType.Trash, 20), //20xp + Druffle
            new BattleData(_battleStartId + 11, "NeonString-Battle", "GL_1B_NJ_NeonStringDead", BattleType.Trash, 50), //50xp
            new BattleData(_battleStartId + 12, "DarkPiranha-Battle", "GL_1B_NJ_DarkPiranhaDead", BattleType.Trash, 30), //30xp
            //Blue Route - Neon Jungle Room 3
            new BattleData(_battleStartId + 13, "HomonculusCowboy-Battle", "GL_2A_NJ_HomonculusEncounter", BattleType.MajorBattle, 36), //36xp
            new BattleData(_battleStartId + 14, "SpringHead-Battle", "GL_1C_NJ_SpringHeadDead", BattleType.Trash, 40), //Double Battle. 40xp + Druffle
            new BattleData(_battleStartId + 15, "DarkPiranha-Battle", "GL_1C_NJ_DarkPiranhaDead", BattleType.Trash, 60), //Double Battle. 60xp
            new BattleData(_battleStartId + 16, "NeonString-Battle", "GL_1C_NJ_NeonStringDead", BattleType.Trash, 50), //50xp
            //Blue Route - Misc
            new BattleData(_battleStartId + 17, "TODO: DOESN'T END NORMALLY", "GL_NTJ_TunnelBattle", BattleType.MajorBattle, 0),
            //Green Route - Year 0 Starting Area
            new BattleData(_battleStartId + 18, "Hyena-Battle", "GL_0A_M1m_HyenaDead", BattleType.Trash, 25), //25xp (Screech) //Todo: Trash battle? This one is required.
            new BattleData(_battleStartId + 19, "Hyena-Battle", "GL_1A_M1m_HyenaDead", BattleType.Trash, 25), //25xp (Warcry)
            new BattleData(_battleStartId + 20, "SharkJailor-Battle", "GL_1A_M1m_SharkJailorDead", BattleType.Trash, 25), //25xp (Bloodnose)
            new BattleData(_battleStartId + 21, "SharkJailor-Battle", "GL_M1m_StartRileyRescue", BattleType.Trash, 50), //Double Fight 50xp (Howler & Razor) //Todo: Trash battle? This one is required.
            new BattleData(_battleStartId + 22, "LaTigre-Battle", "GL_M1m_ElTigreStart", BattleType.MajorBattle, 100), //100xp (Feugo)
            //Green Route - Year 0 Hallway
            new BattleData(_battleStartId + 23, "AntMachine-Battle", "GL_B1_M1Chasers", BattleType.MajorBattle, 50), //50xp 
            new BattleData(_battleStartId + 24, "ChameleonBad-Battle", "GL_B1_M1_EncounterDead", BattleType.MajorBattle, 3), //3xp (Chase)
            //Todo: Remove Trigger on Marzian_Part1Hero_MinesHallway/GAMEPLAY/EastGate/EastDoor-Interact/TopDownFlowchartTrigger Or Allow re-entry into this zone.
            new BattleData(_battleStartId + 25, "SharkJailor-Battle", "GL_B2_M1_EncounterDead", BattleType.Trash, 75), //Triple Battle. 75xp (Howler & Razor & Maggot) //Todo: Trash battle? This one is required.
            new BattleData(_battleStartId + 26, "Gorilla-Battle", "GL_M1_GorillaDefeated", BattleType.MajorBattle, 75), //75xp (Howler & Razor & Maggot)
            //Green Route - Year 1000 
            new BattleData(_battleStartId + 27, "Portal-Battle", "GL_PortalBattleFinished", BattleType.MajorBattle, 400), //400xp (Portal)
            //Green Route - Year 2000 
            new BattleData(_battleStartId + 28, "StoneguardGrunt-Battle", "GL_StonegruntBattle1ADead", BattleType.MajorBattle, 150), //150xp (Blue Stonegrunt) //Todo: I think this can appear in 2nd hub?
            //Red Route - Desert
            new BattleData(_battleStartId + 29, "Tomato-Battle", "GL_1A_EWb_Tomato1Dead", BattleType.Trash, 15), //15xp (Red Onion)
            new BattleData(_battleStartId + 30, "Leek-Battle", "GL_1A_EWb_LeekWest_Dead", BattleType.Trash, 76), //76xp (Leek)
            new BattleData(_battleStartId + 31, "Brocoli-Battle", "GL_1A_EWb_BrocoliEast_Dead", BattleType.Trash, 76), //76xp (Bro-ccoli)
            new BattleData(_battleStartId + 32, "Paprika-Battle", "GL_1A_EWb_PaprikaEast_Dead", BattleType.Trash, 100), //100xp (Bell Pepper)
            new BattleData(_battleStartId + 33, "TomatoRush-Battle", "GL_1A_EWb_TomatoRampageDead", BattleType.Trash, 25), //25xp + Tomato Seed (Tomato Rush Lower Left)
            new BattleData(_battleStartId + 34, "TomatoRush-Battle", "GL_2A_EWb_TomatoRampageDead", BattleType.Trash, 25), //25xp + Tomato Seed (Tomato Rush Lower Middle)
            new BattleData(_battleStartId + 35, "TomatoRush-Battle", "GL_3A_EWb_TomatoRampageDead", BattleType.Trash, 25), //25xp + Tomato Seed (Tomato Rush Lower Right)
            new BattleData(_battleStartId + 36, "TomatoRush-Battle", "GL_4A_EWb_TomatoRampageDead", BattleType.Trash, 25), //25xp + Tomato Seed (Tomato Rush Upper Left)
            new BattleData(_battleStartId + 37, "TomatoRush-Battle", "GL_5A_EWb_TomatoRampageDead", BattleType.Trash, 25), //25xp + Tomato Seed (Tomato Rush Upper Middle)
            new BattleData(_battleStartId + 38, "TomatoRush-Battle", "GL_6A_EWb_TomatoRampageDead", BattleType.Trash, 25), //25xp + Tomato Seed (Tomato Rush Upper Right)
            new BattleData(_battleStartId + 39, "Melon-Battle", "GL_01_EWb_MelonEncounterStart", BattleType.MajorBattle, 100), //100xp (Melon) //Todo: Does this actually give the xp?
            new BattleData(_battleStartId + 40, "Chili-Battle", "GL_1A_EWb_Chili_Dead", BattleType.Trash, 15), //15xp (Chili)
            //Red Route - Castle
            new BattleData(_battleStartId + 41, "Capsicum-Battle", "GL_EWcd_FirstBattleStart", BattleType.MajorBattle, 15), //15xp (Capsicum)
            new BattleData(_battleStartId + 42, "Carrot-Battle", "GL_EWcd_SecondBattleStart", BattleType.MajorBattle, 15), //15xp (Carrot Mage)
            new BattleData(_battleStartId + 43, "JuiceMaster-Battle", "GL_EW_TomatoKingBattleStart", BattleType.MajorBattle, 15), //15xp (Juice Master#4671)
            //Floor 23 - Human Party
            new BattleData(_battleStartId + 44, "TODO: DOESN'T END NORMALLY", "GL_HH1_AngryWizardDead", BattleType.HillbertHotel, 0), //??xp (Yellow God?)
            new BattleData(_battleStartId + 45, "Processor_INT-Battle", "GL_1A_SSApr_DataDead", BattleType.HillbertHotel, 64), //64xp (INT Bug) //Todo: Is this really trash
            new BattleData(_battleStartId + 46, "Rasputin-Battle", "GL_RasputinDead", BattleType.HillbertHotel, 100), //100xp (Rasputin)
            //Floor Gold - The VIP Area
            new BattleData(_battleStartId + 47, "Bobo_Toxicated-Battle", "GL_HH2_BoboDefeated", BattleType.HillbertHotel, 80), //80xp (Drunk Bobo)
            //Floor Pinecone - The Simple Life
            new BattleData(_battleStartId + 48, "Squirrel-Battle", "GL_1A_HH4_SquirrelDead", BattleType.HillbertHotel, 80), //80xp (A lot of Squirrels)
            //Colloseum
            new BattleData(_battleStartId + 49, "JeanDArc-Battle", "GauntletQuest1count", BattleType.Colloseum, 0, 1) //"GL_GauntletQuest_Refresh"
        };

        public static readonly HashSet<string> SkipBattles = new()
        {
            "Tutorial1-Battle", //Second fight immediately
            "SamFake-Battle", //Not really a battle
        };
    }
}