using System;
using System.Drawing;
using System.Windows.Forms;
using formation_sugar.GameModel;

namespace formation_sugar
{
    public sealed partial class Form1 : Form
    {
        private readonly int[,] map;
        private readonly Player player;
        private readonly Box box;
        private Point delta;
        const int width=10;
        const int height=15;
        
        public Form1()
        {
            InitializeComponent();
            
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
             
            ClientSize = new Size(620, 360);
            map = new[,] {{9,9,9,9,9,9,9,9,9,9,1,1,1,1,1 },
                {9,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {9,1,1,1,1,1,1,1,1,1 ,1,1,1,1,1},
                {9,1,1,1,1,1,1,1,1,1 ,1,1,1,1,1},
                {9,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {9,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {9,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {9,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {9,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                {9,9,9,9,9,9,9,9,9,9,1,1,1,1,1 },
            };

            player = new Player(new Point(100, ClientSize.Height - 100), 100, 10);
            box = new Box(new Point(150, ClientSize.Height - 100), 100);
 
            new Timer { Interval = 125, Enabled = true }.Tick += delegate { player.Sprite.GotoNextFrame(); Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            CreateMap(e.Graphics);
            e.Graphics.DrawImageUnscaled(box.boxImage, box.Location);
            
            if (!player.Sprite.Flipped)
            {
                e.Graphics.DrawImage(player.Sprite.Image,
                    new Rectangle(player.Location, player.Sprite.FrameSize), 
                    new Rectangle(new Point(player.Sprite.CurrentFrameLocation.X, player.Sprite.CurrentFrameLocation.Y + 37 * player.MovementCondition), player.Sprite.FrameSize), 
                    GraphicsUnit.Pixel); 
            }
            else
            {
                e.Graphics.DrawImage(player.Sprite.Image,
                    new Rectangle(player.Location, player.Sprite.FrameSize),
                    new Rectangle(
                        new Point(player.Sprite.Image.Width - player.Sprite.CurrentFrameLocation.X - 50,
                            player.Sprite.CurrentFrameLocation.Y + 37 * player.MovementCondition),
                        player.Sprite.FrameSize),
                    GraphicsUnit.Pixel);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    player.Location = new Point(player.Location.X + 1, player.Location.Y);
                    if (player.Sprite.Flipped)
                    {
                        player.Sprite.Flip();    
                    }
                    
                    player.ChangeMovementConditionToRunning();
                    break;
                
                case Keys.A:
                    player.Location = new Point(player.Location.X - 1, player.Location.Y);
                    if (!player.Sprite.Flipped)
                    {
                        player.Sprite.Flip();    
                    }
                    
                    player.ChangeMovementConditionToRunning();
                    break;
                
                case Keys.S:
                    player.ChangeMovementConditionToSitting();
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                default:
                    player.ChangeMovementConditionToStanding();
                    break;
            }
        }
        
        private void CreateMap(Graphics gr)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (map[i, j] == 1)
                    {
                        gr.DrawImage(box.boxImage, j * 24, i * 24, new Rectangle(new Point(0, 0), new Size(80, 80)), GraphicsUnit.Pixel);
                    }
                    else if (map[i, j] == 9)
                    {
                        gr.DrawImage(box.boxImage, j * 24, i * 24, new Rectangle(new Point(0, 0), new Size(80, 80)), GraphicsUnit.Pixel);
                    }
                }
            }
        }
    }
}