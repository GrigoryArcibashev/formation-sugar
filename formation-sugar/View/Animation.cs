using System.Drawing;
using System.Linq;

namespace formation_sugar.View
{
    public class Animation
    {
        private readonly Bitmap[] sprites;
        private int numberOfCurrentSprite;
        
        public Bitmap Current => sprites[numberOfCurrentSprite];
        public int CountOfSprites { get; }

        public Animation(params string[] pathsToImages)
        {
            sprites = pathsToImages.Select(pathToImage => new Bitmap(pathToImage)).ToArray();
            CountOfSprites = pathsToImages.Length;
            numberOfCurrentSprite = 0;
        }

        public void MoveNextSprite()
        {
            numberOfCurrentSprite = (numberOfCurrentSprite + 1) % CountOfSprites;
        }

        public void ResetAnimation()
        {
            numberOfCurrentSprite = 0;
        }
    }
}