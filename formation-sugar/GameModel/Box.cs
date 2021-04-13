using System.Drawing;

namespace formation_sugar.GameModel
{
    public class Box : ICreature
    {
      //  public Image boxImage = new Bitmap()
        public Point Location { get; set; }
        public double Velocity { get; set; }
        public int Health { get; }
        public void ChangeHealthBy(int deltaHealth)
        {
            throw new System.NotImplementedException();
        }
    }
}