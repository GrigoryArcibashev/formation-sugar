namespace Model.Creatures
{
    public interface ICreatureWithHealth
    {
        public int Health { get; }
        void ChangeHealthBy(int deltaHealth);
    }
}