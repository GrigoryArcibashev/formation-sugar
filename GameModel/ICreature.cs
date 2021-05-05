namespace Model
{
    public interface ICreature
    {
        MovementConditions MovementCondition { get; }
        string GetTypeAsString();
    }
}