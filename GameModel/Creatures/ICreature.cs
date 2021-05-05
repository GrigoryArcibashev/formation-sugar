namespace Model.Creatures
{
    public interface ICreature
    {
        MovementConditions MovementCondition { get; }
        Direction Direction { get; }
        string GetTypeAsString();
    }
}