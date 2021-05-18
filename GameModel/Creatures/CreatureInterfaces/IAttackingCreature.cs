namespace Model.Creatures.CreatureInterfaces
{
    public interface IAttackingCreature : IMovingCreature, ICreatureWithHealth
    {
        public int DamageValue { get; }
    }
}