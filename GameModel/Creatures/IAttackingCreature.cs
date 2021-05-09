namespace Model.Creatures
{
    public interface IAttackingCreature : ICreature
    {
        public int DamageValue { get; }
        public int Health { get; }
        public void ChangeHealthBy(int deltaHealth);
    }
}