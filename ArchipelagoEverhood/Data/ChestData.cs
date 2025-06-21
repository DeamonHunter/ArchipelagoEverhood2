namespace ArchipelagoEverhood.Data
{
    public class ChestData
    {
        public readonly int LocationId;
        public readonly string SceneName;
        public readonly string? VariableName;
        public readonly string ItemName;
        public readonly ChestType Type;
        public readonly RewardConditions RewardConditions;
        public readonly bool ExpectedValue;

        public bool InLogic = true; //Todo: Set when connecting.
        public bool Achieved;
        public bool Shown;

        public ChestData(int locationId, string sceneName, string itemName, string? variableName, ChestType type, 
            RewardConditions rewardConditions = RewardConditions.None, bool expectedValue = true)
        {
            LocationId = locationId;
            SceneName = sceneName;
            ItemName = itemName;
            VariableName = variableName;
            Type = type;
            RewardConditions = rewardConditions;
            ExpectedValue = expectedValue;
        }

        public ChestData Clone() => new ChestData(LocationId, SceneName, ItemName, VariableName, Type, RewardConditions, ExpectedValue);
    }
}