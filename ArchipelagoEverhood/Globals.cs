using ArchipelagoEverhood.Archipelago;
using ArchipelagoEverhood.Logic;
using ArchipelagoEverhood.Util;

namespace ArchipelagoEverhood
{
    public static class Globals
    {
        //Easy Access Everhood Data
        public static ServicesRoot? ServicesRoot;
        public static Main_TopdownRoot? TopdownRoot;
        public static Main_GameplayRoot? GameplayRoot;
        public static BattleVictoryResult? BattleVictoryResult;
        public static SceneManagerRoot SceneManagerRoot;

        //Patches
        public static readonly EverhoodBattles EverhoodBattles = new();
        public static readonly EverhoodChests EverhoodChests = new();
        public static readonly EverhoodOverrides EverhoodOverrides = new();

        //Archipelago Stuff
        public static ArchLogger Logging;
        public static ArchipelagoLogin LoginHandler;
        public static readonly ArchipelagoSessionHandler SessionHandler = new();
    }
}