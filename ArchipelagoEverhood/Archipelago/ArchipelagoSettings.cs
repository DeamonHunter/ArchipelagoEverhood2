using System.Collections.Generic;
using ArchipelagoEverhood.Data;

namespace ArchipelagoEverhood.Archipelago
{
    public class ArchipelagoSettings
    {
        public string Seed { get; }
        
        public SoulColor SoulColor { get; } = SoulColor.None;
        public bool DoorKeys { get; }
        public long PowerGemAmount { get; } = 3;
        public bool ColorSanity { get; }
        public bool PreventDragon { get; }
        public float HealthMultiplier { get; } = 1f;
        
        public ArchipelagoSettings(string seed, Dictionary<string, object> slotData)
        {
            Seed = seed;
            if (slotData.TryGetValue("SoulColor", out var colorObj))
                SoulColor = (SoulColor)((long)colorObj);
            
            if (slotData.TryGetValue("DoorKeys", out var doorKeys))
                DoorKeys = (bool)doorKeys;
            
            if (slotData.TryGetValue("DragonGems", out var powerGemObj))
                PowerGemAmount = (long)powerGemObj;

            if (slotData.TryGetValue("Colorsanity", out var color))
                ColorSanity = (bool)color;
            
            if (slotData.TryGetValue("PreventDragon", out var dragon))
                PreventDragon = (bool)dragon;
            
            if (slotData.TryGetValue("HealthMultiplier", out var healthMultiplier))
                HealthMultiplier = ((long)healthMultiplier / 100f);
        }

        public override string ToString()
        {
            return $"Seed: {Seed}. Settings: (SoulColor: {SoulColor}), (DoorKeys: {DoorKeys}), (DragonGems: {PowerGemAmount}), (ColorSanity: {ColorSanity}), (PreventDragon: {PreventDragon})";
        }
    }
}