using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ArchipelagoEverhood.Archipelago
{
    public class EverhoodOverrides
    {
        public bool Overriding;

        public Dictionary<string, int> OriginalXpLevels = new();

        public void ArchipelagoConnected()
        {
            if (Overriding)
            {
                Debug.LogError("We are somehow overriding twice? Did we log in again?");
                return;
            }

            Overriding = true;
            Globals.ServicesRoot = GameObject.FindObjectsByType<ServicesRoot>(FindObjectsInactive.Include, FindObjectsSortMode.None).First();

            var infProjExperience = typeof(InfinityProjectExperience);
            var xpRewardInfo = infProjExperience.GetField("xpRewardInfo", BindingFlags.Instance | BindingFlags.NonPublic);
            OriginalXpLevels = (Dictionary<string, int>)xpRewardInfo.GetValue(Globals.ServicesRoot.InfinityProjectExperience);

            var overrideDict = new Dictionary<string, int>();
            xpRewardInfo.SetValue(Globals.ServicesRoot.InfinityProjectExperience, overrideDict);
        }

        public void ArchipelagoDisconnected()
        {
            if (Overriding)
            {
                Debug.LogError("We are somehow overriding twice? Did we log in again?");
                return;
            }

            var infProjExperience = typeof(InfinityProjectExperience);
            var xpRewardInfo = infProjExperience.GetField("xpRewardInfo", BindingFlags.Instance | BindingFlags.NonPublic);
            OriginalXpLevels = (Dictionary<string, int>)xpRewardInfo.GetValue(Globals.ServicesRoot.InfinityProjectExperience);
            xpRewardInfo.SetValue(Globals.ServicesRoot.InfinityProjectExperience, OriginalXpLevels);
        }
    }
}