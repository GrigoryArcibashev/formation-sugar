using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using formation_sugar.View;
using formation_sugar.GameModel;

namespace formation_sugar
{
    public sealed partial class Form1 : Form
    {
        private readonly Dictionary<ICreature, Dictionary<MovementConditions, Animation>> animationsForCreatures;
        private readonly Timer timerForPlayerMovement;
        private readonly Timer timerForPlayerAnimation;
        private readonly GameMap map;
        private readonly Physics physics;
        
        public Form1()
        {
            map = new GameMap();
            map.CreateMap(1);
            
            ClientSize = new Size(620, 360);
            physics = new Physics(-10);
            animationsForCreatures = new Dictionary<ICreature, Dictionary<MovementConditions, Animation>>();
            
            foreach (var creature in map.Map)
            {
                AddAnimationsForCreature(creature);
            }

            timerForPlayerAnimation = new Timer {Interval = 100, Enabled = true};
            timerForPlayerAnimation.Tick += delegate
            {
                animationsForCreatures[map.Player][map.Player.MovementCondition].MoveNextSprite();
                Invalidate();
            };
            
            timerForPlayerMovement = new Timer {Interval = 30, Enabled = true};
            timerForPlayerMovement.Tick += UpdatePlayerLocation;
            
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            InitializeComponent();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            GraphicsCreator.CreateGraphic(graphics, animationsForCreatures, map.Map);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            timerForPlayerMovement.Interval = e.Modifiers == Keys.Shift ? 10 : 30;
            timerForPlayerAnimation.Interval = 100;
            
            switch (e.KeyCode)
            {
                case Keys.D when !map.Player.IsPlayerJumping():
                    map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.RunningRight, Direction.Right);
                    break;
                
                case Keys.A when !map.Player.IsPlayerJumping():
                    map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.RunningLeft, Direction.Left);
                    break;
                
                case Keys.S when !map.Player.IsPlayerJumping():
                    map.Player.MovementCondition = map.Player.Direction is Direction.Right
                        ? MovementConditions.SittingRight
                        : MovementConditions.SittingLeft;
                    break;
                
                case Keys.W:
                    timerForPlayerAnimation.Interval = 200;
                    map.Player.MovementCondition = map.Player.Direction is Direction.Right
                        ? MovementConditions.JumpingRight
                        : MovementConditions.JumpingLeft;
                    break;
            }
        }
        
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (!map.Player.IsPlayerFalling() && !map.Player.IsPlayerJumping())
            {
                map.Player.MovementCondition = map.Player.Direction is Direction.Right
                        ? MovementConditions.StandingRight
                        : MovementConditions.StandingLeft;
            }
        }

        private void UpdatePlayerLocation(object sender, EventArgs e)
        {
            switch (map.Player.MovementCondition)
            {
                case MovementConditions.RunningRight:
                    map.Player.Location = new Point(map.Player.Location.X + 1, map.Player.Location.Y);
                    break;
                
                case MovementConditions.RunningLeft:
                    map.Player.Location = new Point(map.Player.Location.X - 1, map.Player.Location.Y);
                    break;
                
                case MovementConditions.JumpingRight:
                    if (map.Player.Velocity < 0)
                    {
                        map.Player.MovementCondition = MovementConditions.FallingRight;
                    }
                    
                    physics.MoveCreatureByY(map.Player, (double) 5 / 500);
                    break;
                
                case MovementConditions.JumpingLeft:
                    if (map.Player.Velocity < 0)
                    {
                        map.Player.MovementCondition = MovementConditions.FallingLeft;
                    }
                    
                    physics.MoveCreatureByY(map.Player, (double) 5 / 500);
                    break;
                
                case MovementConditions.FallingRight:
                    physics.MoveCreatureByY(map.Player, (double) 5 / 500);
                    break;
                
                case MovementConditions.FallingLeft:
                    physics.MoveCreatureByY(map.Player, (double) 5 / 500);
                    break;
            }
            
            IfPlayerOnTheBoard();
        }

        private void AddAnimationsForCreature(ICreature creature)
        {
                animationsForCreatures.Add(creature, AnimationsForCreatures.GetAnimationFor(creature));
        }

        private void IfPlayerOnTheBoard()
        {
            if (map.Player.Location.Y >= ClientSize.Height - 30 && map.Player.IsPlayerFalling())
            {
                map.Player.RecoverVelocity();
                map.Player.MovementCondition = MovementConditions.StandingRight;
            }
        }
    }
}