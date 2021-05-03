namespace formation_sugar.GameModel
{
    public interface ICreature
    {
        MovementConditions MovementCondition { get; }
        string GetTypeAsString();
    }
}