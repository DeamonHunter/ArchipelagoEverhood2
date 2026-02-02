using ArchipelagoEverhood.Archipelago;
using ArchipelagoEverhood.Logic;
using ArchipelagoEverhood.Util;
using UnityEngine;

namespace ArchipelagoEverhood
{
    public static class Globals
    {
        //Easy Access Everhood Data
        public static ServicesRoot? ServicesRoot;
        public static Main_TopdownRoot? TopdownRoot;
        public static Main_GameplayRoot? GameplayRoot;
        public static SceneManagerRoot? SceneManagerRoot;

        //Easy Access Everhood Stuff
        public static GameObject ExitToHubButton;
        public static int CurrentTopdownLevel;
        
        //Easy Access Patch Stuff
        public static Texture2D? BlackHalfMoonTexture { get; set; }

        //Victory Screen Stuff
        public static BattleVictoryResult? BattleVictoryResult;
        public static Canvas? VictoryScreenCanvas;

        //Patches
        public static readonly EverhoodBattles EverhoodBattles = new();
        public static readonly EverhoodChests EverhoodChests = new();
        public static readonly EverhoodOverrides EverhoodOverrides = new();
        public static readonly EverhoodDoors EverhoodDoors = new();

        //Archipelago Stuff
        public static ArchLogger Logging;
        public static ArchipelagoLogin LoginHandler;
        public static readonly ArchipelagoSessionHandler SessionHandler = new();
        public static bool SaveRequested { get; set; }
    }
}