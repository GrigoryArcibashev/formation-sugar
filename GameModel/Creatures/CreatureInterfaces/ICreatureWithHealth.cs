namespace Model.Creatures.CreatureInterfaces
{
    public interface ICreatureWithHealth : ICreature
    {
        public int Health { get; }
        public void ChangeHealthBy(int deltaHealth);
        public bool IsDead();
    }
}