namespace Model.Creatures
{
    public interface ICreature
    {
        MovementConditions MovementCondition { get; }
        string GetTypeAsString();
    }
}