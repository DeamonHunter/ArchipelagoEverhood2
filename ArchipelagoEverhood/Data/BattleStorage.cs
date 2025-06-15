using System.Collections.Generic;

namespace ArchipelagoEverhood.Data
{
    public static class BattleStorage
    {
        private const int _battleStartId = 500;

        public static readonly List<BattleData> Battles = new()
        {
            //Intro
            new BattleData(_battleStartId + 0, "Tutorial2-Battle", "GL_Intro_RavenIntro", 0), //Todo: This one is touchy? "GL_RavenBattle_Dead" is set after battle.

            //Blue Route - Pre City
            new BattleData(_battleStartId + 1, "SpringHead-Battle", "GL_1A_ND_SpringHeadDead", 20), //20xp + Druffle
            //Blue Route - Neon Jungle Room 1
            new BattleData(_battleStartId + 2, "SpringHead-Battle", "GL_1A_NJ_SpringHeadDead", 20), //20xp + Druffle
            new BattleData(_battleStartId + 3, "SpringHead-Battle", "GL_2A_NJ_SpringHeadDead", 20), //20xp + Druffle
            new BattleData(_battleStartId + 4, "SpringHead-Battle", "GL_3A_NJ_SpringHeadDead", 40), //Double Battle.
            new BattleData(_battleStartId + 5, "DarkPiranha-Battle", "GL_1A_NJ_DarkPiranhaDead", 30), //30xp
            new BattleData(_battleStartId + 6, "DarkPiranha-Battle", "GL_2A_NJ_DarkPiranhaDead", 30), //30xp
            //Blue Route - Neon Jungle Room 2
            new BattleData(_battleStartId + 7, "Homonculus-Battle", "GL_1A_NJ_HomonculusEncounter", 35), //35xp
            new BattleData(_battleStartId + 8, "SpringHead-Battle", "GL_1B_NJ_SpringHeadDead", 20), //20xp + Druffle
            new BattleData(_battleStartId + 9, "SpringHead-Battle", "GL_2B_NJ_SpringHeadDead", 20), //20xp + Druffle
            new BattleData(_battleStartId + 10, "SpringHead-Battle", "GL_3B_NJ_SpringHeadDead", 20), //20xp + Druffle
            new BattleData(_battleStartId + 11, "NeonString-Battle", "GL_1B_NJ_NeonStringDead", 50), //50xp
            new BattleData(_battleStartId + 12, "DarkPiranha-Battle", "GL_1B_NJ_DarkPiranhaDead", 30), //30xp
            //Blue Route - Neon Jungle Room 3
            new BattleData(_battleStartId + 13, "HomonculusCowboy-Battle", "GL_2A_NJ_HomonculusEncounter", 36), //36xp
            new BattleData(_battleStartId + 14, "SpringHead-Battle", "GL_1C_NJ_SpringHeadDead", 40), //Double Battle. 40xp + Druffle
            new BattleData(_battleStartId + 15, "DarkPiranha-Battle", "GL_1C_NJ_DarkPiranhaDead", 60), //Double Battle. 60xp
            new BattleData(_battleStartId + 16, "NeonString-Battle", "GL_1C_NJ_NeonStringDead", 50), //50xp
            //Blue Route - Misc
            new BattleData(_battleStartId + 17, "TODO: DOESN'T END NORMALLY", "GL_NTJ_TunnelBattle", 0), // [Dot]Use GL_ND_WokeUpAfterBattle as the end of the battle?
            //Green Route - Year 0 Starting Area
            new BattleData(_battleStartId + 18, "Hyena-Battle", "GL_0A_M1m_HyenaDead", 25), //25xp (Screech) //Todo: Trash battle? This one is required.
            new BattleData(_battleStartId + 19, "Hyena-Battle", "GL_1A_M1m_HyenaDead", 25), //25xp (Warcry)
            new BattleData(_battleStartId + 20, "SharkJailor-Battle", "GL_1A_M1m_SharkJailorDead", 25), //25xp (Bloodnose)
            new BattleData(_battleStartId + 21, "SharkJailor-Battle", "GL_M1m_StartRileyRescue", 50), //Double Fight 50xp (Howler & Razor) //Todo: Trash battle? This one is required.
            new BattleData(_battleStartId + 22, "LaTigre-Battle", "GL_M1m_ElTigreStart", 100), //100xp (Feugo)
            //Green Route - Year 0 Hallway
            new BattleData(_battleStartId + 23, "AntMachine-Battle", "GL_B1_M1Chasers", 50), //50xp 
            new BattleData(_battleStartId + 24, "ChameleonBad-Battle", "GL_B1_M1_EncounterDead", 2), //2xp (Anxious Chase)
            //Todo: Remove Trigger on Marzian_Part1Hero_MinesHallway/GAMEPLAY/EastGate/EastDoor-Interact/TopDownFlowchartTrigger Or Allow re-entry into this zone.
            new BattleData(_battleStartId + 25, "SharkJailor-Battle", "GL_B2_M1_EncounterDead", 75), //Triple Battle. 75xp (Howler & Razor & Maggot) //Todo: Trash battle? This one is required.
            new BattleData(_battleStartId + 26, "Gorilla-Battle", "GL_M1_GorillaDefeated", 200), //200xp (The Dimension Master)
            //Green Route - Year 1000 
            new BattleData(_battleStartId + 27, "Portal-Battle", "GL_PortalBattleFinished", 400), //400xp (Portal)
            //Green Route - Year 2000 
            new BattleData(_battleStartId + 28, "StoneguardGrunt-Battle", "GL_StonegruntBattle1ADead", 150), //150xp (Blue Stonegrunt) //Todo: I think this can appear in 2nd hub?
            //Red Route - Desert
            new BattleData(_battleStartId + 29, "Tomato-Battle", "GL_1A_EWb_Tomato1Dead", 15), //15xp (Red Onion)
            new BattleData(_battleStartId + 30, "Leek-Battle", "GL_1A_EWb_LeekWest_Dead", 76), //76xp (Leek)
            new BattleData(_battleStartId + 31, "Brocoli-Battle", "GL_1A_EWb_BrocoliEast_Dead", 76), //76xp (Bro-ccoli)
            new BattleData(_battleStartId + 32, "Paprika-Battle", "GL_1A_EWb_PaprikaEast_Dead", 100), //100xp (Bell Pepper)
            new BattleData(_battleStartId + 33, "TomatoRush-Battle", "GL_1A_EWb_TomatoRampageDead", 25), //25xp + Tomato Seed (Tomato Rush Lower Left)
            new BattleData(_battleStartId + 34, "TomatoRush-Battle", "GL_2A_EWb_TomatoRampageDead", 25), //25xp + Tomato Seed (Tomato Rush Lower Middle)
            new BattleData(_battleStartId + 35, "TomatoRush-Battle", "GL_3A_EWb_TomatoRampageDead", 25), //25xp + Tomato Seed (Tomato Rush Lower Right)
            new BattleData(_battleStartId + 36, "TomatoRush-Battle", "GL_4A_EWb_TomatoRampageDead", 25), //25xp + Tomato Seed (Tomato Rush Upper Left)
            new BattleData(_battleStartId + 37, "TomatoRush-Battle", "GL_5A_EWb_TomatoRampageDead", 25), //25xp + Tomato Seed (Tomato Rush Upper Middle)
            new BattleData(_battleStartId + 38, "TomatoRush-Battle", "GL_6A_EWb_TomatoRampageDead", 25), //25xp + Tomato Seed (Tomato Rush Upper Right)
            new BattleData(_battleStartId + 39, "Melon-Battle", "GL_01_EWb_MelonEncounterStart", 100), //100xp (Melon) //Todo: Does this actually give the xp? [Dot] Said it gave xp in the log but need to test.
            new BattleData(_battleStartId + 40, "Chili-Battle", "GL_1A_EWb_Chili_Dead", 15), //15xp (Chili)
            //Red Route - Castle
            new BattleData(_battleStartId + 41, "Capsicum-Battle", "GL_EWcd_FirstBattleStart", 70), //70xp (Capsicum)
            new BattleData(_battleStartId + 42, "Carrot-Battle", "GL_EWcd_SecondBattleStart", 45), //45xp (Carrot Mage)
            new BattleData(_battleStartId + 43, "JuiceMaster-Battle", "GL_EW_TomatoKingBattleStart", 150), //150xp (Juice Master #4671)
            //Floor 23 - Human Party
            new BattleData(_battleStartId + 44, "TODO: DOESN'T END NORMALLY", "GL_HH1_AngryWizardDead", 0), //??xp (Yellow Toad) [Dot] Use GL_HH1_SmegaFinished as the end of battle?
            new BattleData(_battleStartId + 45, "Processor_INT-Battle", "GL_1A_SSApr_DataDead", 64), //64xp (INT Bug) //Todo: Is this really trash. [Dot] I believe this is skippable, need to test.
            new BattleData(_battleStartId + 46, "Rasputin-Battle", "GL_RasputinDead", 100), //100xp (Rasputin)
            //Floor Gold - The VIP Area
            new BattleData(_battleStartId + 47, "Bobo_Toxicated-Battle", "GL_HH2_BoboDefeated", 80), //80xp (Drunk Bobo)
            //Floor Pinecone - The Simple Life
            new BattleData(_battleStartId + 48, "Squirrel-Battle", "GL_1A_HH4_SquirrelDead", 400), //400xp (A lot of Squirrels)
            //Todo: Move Upwards
            new BattleData(_battleStartId + 50, "Hyena-Battle", "GL_B3_M1EncounterDead", 50), //50xp Opus and Screech

            //Year 3000 (unlocked after Dragon)
            new BattleData(_battleStartId + 51, "Thriller-Battle", "GL_1A_M4_ThrillerDead", 50), // Required, before switch
            new BattleData(_battleStartId + 52, "Lurker-Battle", "GL_1A_M4_LurkerDead", 50), //In Cave, Required
            new BattleData(_battleStartId + 53, "Lurker-Battle", "GL_2A_M4_LurkerDead", 50), //Bottom, guarding chest.
            new BattleData(_battleStartId + 54, "Wheeler-Battle", "GL_1A_M4_WheelerDead", 50), // Requiured, at door after switch
            new BattleData(_battleStartId + 55, "Lurker-Battle", "GL_1C_M4_Lurker", 50),
            new BattleData(_battleStartId + 56, "Wheeler-Battle", "GL_1C_M4_Wheeler", 50),
            new BattleData(_battleStartId + 57, "Thriller-Battle", "GL_1C_M4_Thriller", 50), // Before second switch, required
            //Year 4000 (can access immediately after Year 3000)
            new BattleData(_battleStartId + 58, "Lurker-Battle", "GL_1C_M5_Lurker", 50), // Required
            new BattleData(_battleStartId + 59, "Marzian-Battle", "GL_1A_M5_MarzianDead", 50), // Required
            new BattleData(_battleStartId + 60, "Chameleon-Battle", "GL_1A_M5_ChameleonDead", 75), // Required
            new BattleData(_battleStartId + 61, "SharkJailor-Battle", "GL_1A_M5_SharkDead", 25), // Required, Irvine fight
            new BattleData(_battleStartId + 62, "LaTigre-Battle", "GL_1A_M5_TigreDead", 100), // Required, Sam fight. Got xp screen
            //Mushroom Forest (unlocked after Dragon) 
            new BattleData(_battleStartId + 63, "BrownSlowMushroom-Battle", "GL_MBs_1A_SlowBrownMushroomBattle", 50), // Sun path, required
            new BattleData(_battleStartId + 64, "HydraMushroom-Battle", "HydraMushroomAwaken", 50), // Moon path, believe this is a trash fight
            new BattleData(_battleStartId + 65, "SmellyGasMushroom-Battle", "SmellyGasMushroom-Battle", 50), // Moon path, required, fight as Cube
            new BattleData(_battleStartId + 66, "BrownSlowMushroom-Battle", "GL_1B_MBm_SlowBrownDead", 50), // Moon path, required, fight as Cube
            new BattleData(_battleStartId + 67, "MB_SunKnight-Battle", "GL_1B_MBm_SunKnightDead", 100), // Moon path, required, fight as Cube
            new BattleData(_battleStartId + 68, "GL_MB_BureauGone", "GL_MushroomBureauFinished", 100), // Mushroom Bureau Fight after getting an Insignia, Judge Mushroom fight. Does not end normally
            //Lucy's Room, 266 888 (code given after Mushroom Forest)
            new BattleData(_battleStartId + 69, "Lucy-Battle", "GL_HH_LucyEncounter", 1000), // Is a hard fight but not required to beat game, would block Duality... so progression?
            //Irvine's Pocket dimension (unlocked after Mushroom Forest)
            new BattleData(_battleStartId + 70, "Jest3D-Battle", "GL_1A_3D_Jest3DDead", 100),
            new BattleData(_battleStartId + 71, "Jest3D-Battle", "GL_2A_3D_Jest3DDead", 100),
            new BattleData(_battleStartId + 72, "DoopyDragon3D-Battle", "GL_1A_3D_DoopyDragonDead", 35),
            new BattleData(_battleStartId + 73, "Vanguard3D-Battle", "GL_1A_3D_VanguardDead", 250),
            //Lab (unlocked after Mushroom Forest). gives code 888 688 for Colloseum
            new BattleData(_battleStartId + 74, "GL_1A_LAB_DoorOfTheDeadBattle", "GL_1A_LAB_DoorOfTheDeadBattle", 0), // Result for winning, no victory screen no xp, leads to Katana
            new BattleData(_battleStartId + 75, "TheLab_Ghost_Chunky-Battle", "GL_1A_LAB_ChunkyBattle", 35), // Required to leave lab. If we have a return to hub option could be Trash
            new BattleData(_battleStartId + 76, "TheLab_Ghost_Junkie-Battle", "GL_2A_LAB_JunkieBattle", 35), // Required to leave lab. If we have a return to hub option could be Trash
            new BattleData(_battleStartId + 77, "TheLab_Ghost_Frankie-Battle", "GL_3A_LAB_FrankieBattle", 35), // Required to leave lab. If we have a return to hub option could be Trash
            //Omega Room (Omega Key)
            new BattleData(_battleStartId + 78, "CarrotCazok-Battle", "GL_1A_HH5_CarrotDead", 50),
            new BattleData(_battleStartId + 79, "GhostCazok-Battle", "GL_2A_HH5_GhostDead", 50),
            new BattleData(_battleStartId + 80, "Vanguard3DCazok-Battle", "GL_3A_HH5_VanguardDead", 50),
            new BattleData(_battleStartId + 81, "Cazok-Battle", "GL_HH5_CazokDead", 950),
            //Smega Station (unlocked after Mushroom Forest)
            new BattleData(_battleStartId + 82, "Motherboard_INT-Battle", "GL_1A_SSmb_DataDead", 64),
            new BattleData(_battleStartId + 83, "Motherboard_Char-Battle", "GL_2A_SSmb_DataDead", 64),
            new BattleData(_battleStartId + 84, "Motherboard_BOOL-Battle", "GL_3A_SSmb_DataDead", 192), // Triple fight
            new BattleData(_battleStartId + 85, "Motherboard_While-Battle", "GL_4A_SSmb_DataDead", 64),
            //Smega Station - Towards RAM
            new BattleData(_battleStartId + 86, "RAM_INT-Battle", "GL_1A_SSram_DataDead", 64),
            new BattleData(_battleStartId + 87, "RAM_BOOL-Battle", "GL_2A_SSram_DataDead", 64),
            new BattleData(_battleStartId + 88, "RAM_While-Battle", "GL_5A_SSram_DataDead", 64),
            new BattleData(_battleStartId + 89, "RAM_While-Battle", "GL_1B_SSram_DataDead", 128),
            new BattleData(_battleStartId + 90, "RAM_INT-Battle", "GL_4B_SSram_DataDead", 64),
            new BattleData(_battleStartId + 91, "RAM_Char-Battle", "GL_5B_SSram_DataDead", 64),
            new BattleData(_battleStartId + 92, "RAM_Float-Battle", "GL_2C_SSram_DataDead", 64),
            new BattleData(_battleStartId + 93, "RAM_Char-Battle", "GL_3C_SSram_DataDead", 64),
            new BattleData(_battleStartId + 94, "Matrix-Battle", "GL_5C_SSram_DataDead", 64),
            //Smega Station - Towards Processor
            new BattleData(_battleStartId + 95, "ProcessorGate-Battle", "GL_SSmb_ProcessorDead", 250),
            new BattleData(_battleStartId + 96, "Processor_INT-Battle", "GL_2A_SSpr_DataDead", 64),
            new BattleData(_battleStartId + 97, "Processor_float-Battle", "GL_3A_SSpr_DataDead", 64),
            new BattleData(_battleStartId + 98, "Matrix-Battle", "GL_5A_SSpr_DataDead", 64),
            new BattleData(_battleStartId + 99, "Processor_BOOL-Battle", "GL_6A_SSpr_DataDead", 64),
            new BattleData(_battleStartId + 100, "Processor_Char-Battle", "GL_8A_SSpr_DataDead", 64),
            //Smega Station - Towards Processor 2nd room
            new BattleData(_battleStartId + 101, "Processor_While-Battle", "GL_2B_SSpr_DataDead", 128), // Don't remember if this is a Trash or MajorBattle
            //Smega Station - Towards Processor, Boss room
            new BattleData(_battleStartId + 102, "IrvineCorrupt-Battle", "GL_SSPR_IrvineBattleStart", 1200),
            //Death Mountain. Fights respawn upon re-entering but give 0xp (unlocked after Mushroom Forest)
            new BattleData(_battleStartId + 103, "Rock-Battle", "GL_1A_BI_RockDead", 200),
            new BattleData(_battleStartId + 104, "Rock-Battle", "GL_2A_BI_RockDead", 200),
            new BattleData(_battleStartId + 105, "MimicChest-Battle", "GL_1A_BI_MimicChestDead", 250),
            new BattleData(_battleStartId + 106, "MimicApple-Battle", "GL_1A_BI_MimicAppleDead", 300),
            new BattleData(_battleStartId + 107, "Harpy-Battle", "GL_1A_BI_HarpyDead", 250),
            //Death Mountain Part 2
            new BattleData(_battleStartId + 108, "Zombie-Battle", "GL_1A_BI_ZombieDead", 300),
            new BattleData(_battleStartId + 109, "Zombie-Battle", "GL_2A_BI_ZombieDead", 300),
            new BattleData(_battleStartId + 110, "Zombie-Battle", "GL_3A_BI_ZombieDead", 300),
            new BattleData(_battleStartId + 111, "Rock-Battle", "GL_4A_BI_RockDead", 200),
            new BattleData(_battleStartId + 112, "Harpy-Battle", "GL_2A_BI_HarpyDead", 250),
            new BattleData(_battleStartId + 113, "CrabMachine-Battle", "GL_1A_BI_GeniusDead", 1000),
            //Pandemonium Tutorial (accessed after Death Mountain)
            new BattleData(_battleStartId + 114, "Dmitri-Battle", "GL_TS_DmitriBattle", 650), // 1st attempt is as Irvine
            //Everhood 1 - Cursed Castle (Accessed after Pandemonium Tutorial)
            new BattleData(_battleStartId + 115, "Yellow-Battle", "GL_DE_YellowDead", 800),
            new BattleData(_battleStartId + 116, "CursedCastle-Battle", "GL_DE_CursedCastleDead", 800),
            //Everhood 1 - Circus Door
            new BattleData(_battleStartId + 117, "Pink-Battle", "GL_DE_PinkBattle", 600), // Optional, but gives soul weapon
            //Everhood 1 - Midnight Town 
            new BattleData(_battleStartId + 118, "SlotMachine-Battle", "GL_BrownMageExperiment", 500),
            //Pandemonium - Deep Sea
            new BattleData(_battleStartId + 119, "JudgeCreation-Battle", "GL_Pa_EnterPandemonium", 0),
            //The Colosseum, without using a code is accessed at Pandemonium (otherwise accessed at the Lab with 888 688)
            new BattleData(_battleStartId + 120, "JeanDArc-Battle", "GL_GauntletQuest1_Finished", 350),
            //new BattleData(_battleStartId + 121, "JeanDArc-Battle", "GauntletQuest1count", 0, 1), //"GL_GauntletQuest_Refresh" DeamonHunter's version
            new BattleData(_battleStartId + 121, "Molly-Battle", "GL_GauntletQuest2_Finished", 888),
            new BattleData(_battleStartId + 122, "RAM_ForLoop-Battle", "GL_GauntletQuest3_Finished", 500),
            new BattleData(_battleStartId + 123, "DragonPart3-Battle", "GL_DoubleDsDragonArenaBattle", 0),
            new BattleData(_battleStartId + 124, "MushroomHouse-Battle", "GL_GauntletQuest5_Finishe", 200), // Post ShadePostCredits, may need MushroomDefeated Gives 2 power gems.
            //Pandemonium (requires Death Coin)
            new BattleData(_battleStartId + 125, "GL_SEnd_Init", "GL_BoboDefeatedFirstTime", 0), // Also have GL_BoboDefeated. Doesn't have a normal end to fight
            new BattleData(_battleStartId + 126, "ShadeDemon-Battle", "GL_SEnd_ShadeBattleStart", 0),
            new BattleData(_battleStartId + 127, "SunBattle_ReadyToDie", "GL_SEnd_SunBattle", 0),
            //Riley's Kingdom (after Pandemonium)
            new BattleData(_battleStartId + 128, "VoidRaven-RileyFortress-Battle", "GL_RSEnd_Harpy1ADefeated", 0),
            new BattleData(_battleStartId + 129, "VoidRaven-RileyFortress-Battle", "GL_RSEnd_Harpy2ADefeated", 0),
            new BattleData(_battleStartId + 130, "VoidRaven-RileyFortress-Battle", "GL_RSEnd_Harpy3ADefeated", 0),
            new BattleData(_battleStartId + 131, "VoidRaven-RileyFortress-Battle", "GL_RSEnd_Harpy4ADefeated", 0),
            new BattleData(_battleStartId + 132, "VoidRaven-RileyFortress-Battle", "GL_RSEnd_Harpy5ADefeated", 0),
            new BattleData(_battleStartId + 133, "Evren2-Battle", "GL_RESend_EvrenBattle", 700),
            new BattleData(_battleStartId + 134, "GL_SEnd_CreditsHasBeenViewed", "GL_RileyBattle", 0), // Fight doesn't end normally, wasn't good markers for this. Mainly saw [Command] StartRileyBattle : Stop
            
            //Dragon
            new BattleData(_battleStartId + 135, "DragonPart2-Battle", "GL_DragonDead", 0), // First Goal. Add if not goaling here.
            new BattleData(_battleStartId + 136, "PostCreditsShade-Battle", "GL_GameFinished", 0), // Second Goal. Add if not goaling here. Todo: Fix variable
        };

        public static readonly HashSet<string> SkipBattles = new()
        {
            "Tutorial1-Battle", //Second fight immediately
            "SamFake-Battle", //Not really a battle
            "DragonPart1-Battle"
        };

        //Todo: Determine Goal [Dot] A Goal should be ShadePostCredits-Battle.
        public static readonly HashSet<string> VictoryBattles = new()
        {
            "DragonPart2-Battle"
            // new BattleData(_battleStartId + X, "ShadePostCredits-Battle", "GL_PostCredits_ShadeBattleDefeated", 0),
            // CatGodHairball-Battle //777xp
        };
    }
}