namespace ArchipelagoEverhood.Data
{
    public class ChestData
    {
        public readonly int LocationId;
        public readonly string SceneName;
        public readonly string? VariableName;
        public readonly string? ItemName;
        public readonly ChestType Type;
        public readonly bool ForceSayDialogue;
        
        public bool InLogic = true; //Todo: Set when connecting.
        public bool Achieved;

        public ChestData(int locationId, string sceneName, string? itemName, string? variableName, ChestType type, bool forceSayDialogue = false)
        {
            LocationId = locationId;
            SceneName = sceneName;
            ItemName = itemName;
            VariableName = variableName;
            Type = type;
            ForceSayDialogue = forceSayDialogue;
        }

        public ChestData Clone() => new ChestData(LocationId, SceneName, ItemName, VariableName, Type, ForceSayDialogue);
    }
}