﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using formation_sugar.GameModel;

namespace formation_sugar
{
    public sealed partial class Form1 : Form
    {
        private readonly Player player;
        private readonly Box box;
        public Form1()
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent;
            BackgroundImage = new Bitmap(Path.Combine(currentDirectory?.FullName!, @"Sprites\forest.png"));
            
            InitializeComponent();
            
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
             
            ClientSize = new Size(620, 360);

            player = new Player(new Point(100, ClientSize.Height - 100), 100, 10);
            box = new Box(new Point(150, ClientSize.Height - 100), 100);
 
            new Timer { Interval = 125, Enabled = true }.Tick += delegate { player.Sprite.GotoNextFrame(); Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImageUnscaled(box.BoxImage, box.Location);
            
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
                    player.Location = new Point(player.Location.X + 3, player.Location.Y);
                    if (player.Sprite.Flipped)
                    {
                        player.Sprite.Flip();    
                    }
                    
                    player.ChangeMovementConditionToRunning();
                    break;
                
                case Keys.A:
                    player.Location = new Point(player.Location.X - 3, player.Location.Y);
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
    }
}