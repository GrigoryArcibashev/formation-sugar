namespace GameModel
{
    public interface ICreature
    {
        MovementConditions MovementCondition { get; }
        string GetTypeAsString();
    }
}