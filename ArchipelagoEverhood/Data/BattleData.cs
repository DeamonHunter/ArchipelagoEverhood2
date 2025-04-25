namespace ArchipelagoEverhood.Data
{
    public class BattleData
    {
        public readonly int LocationId;
        public readonly string SceneName;
        public readonly string VariableName;
        public readonly BattleType BattleType;
        public readonly int? IntegerCount;

        public bool InLogic = true; //Todo: Set when connecting.
        public bool Achieved;

        public BattleData(int locationId, string sceneName, string variableName, BattleType battleType, int? integerCount = null)
        {
            LocationId = locationId;
            SceneName = sceneName;
            VariableName = variableName;
            BattleType = battleType;
            IntegerCount = integerCount;
        }

        public BattleData Clone() => new BattleData(LocationId, SceneName, VariableName, BattleType, IntegerCount);
    }
}