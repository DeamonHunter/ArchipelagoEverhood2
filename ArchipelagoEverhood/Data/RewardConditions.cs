using System;

namespace ArchipelagoEverhood.Data
{
    [Flags]
    public enum RewardConditions
    {
        None = 0,
        ForceShowDialogue = 1 << 0,
        RewardOnVariable = 1 << 1,
        DontSetVariableOnGiven = 1 << 2
    }
}