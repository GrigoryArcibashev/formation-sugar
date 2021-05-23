namespace Model.Creatures.CreatureInterfaces
{
    public interface ICreatureWithHealth : ICreature
    {
        public void ChangeHealthBy(int deltaHealth);
    }
}