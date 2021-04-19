using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using formation_sugar.GameModel;
using formation_sugar.View;

namespace formation_sugar
{
    public sealed partial class Form1 : Form
    {
        private readonly Dictionary<ICreature, Dictionary<MovementConditions, Animation>> animationsForCreatures;
        private readonly Timer timerForPlayerMovement;
        private readonly GameMap map;
        private readonly Physics physics;
        
        public Form1()
        {
            map = new GameMap();
            map.CreateMap(1);
            
            ClientSize = new Size(620, 360);
            physics = new Physics(-5);
            animationsForCreatures = new Dictionary<ICreature, Dictionary<MovementConditions, Animation>>();
            
            foreach (var creature in map.Map)
            {
                AddAnimationsForCreature(creature);
            }

            new Timer {Interval = 125, Enabled = true}.Tick += delegate
            {
                animationsForCreatures[map.Player][map.Player.MovementsCondition].MoveNextSprite();
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
            switch (e.KeyCode)
            {
                case Keys.D when e.Modifiers == Keys.Shift:
                    timerForPlayerMovement.Interval = 10;
                    map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.RunningRight, Direction.Right);
                    break;
                
                case Keys.D:
                    timerForPlayerMovement.Interval = 30;
                    map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.RunningRight, Direction.Right);
                    break;
                
                case Keys.A:
                    map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.RunningLeft, Direction.Left);
                    break;
                
                case Keys.S:
                    map.Player.MovementsCondition = map.Player.Direction is Direction.Right
                        ? MovementConditions.SittingRight
                        : MovementConditions.SittingLeft;
                    break;
                
                case Keys.W:
                    map.Player.MovementsCondition = map.Player.Direction is Direction.Right
                        ? MovementConditions.JumpingRight
                        : MovementConditions.JumpingLeft;
                    break;
            }
        }
        
        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                default:
                    map.Player.MovementsCondition = map.Player.Direction is Direction.Right
                        ? MovementConditions.StandingRight
                        : MovementConditions.StandingLeft; 
                    break;
            }
        }

        private void UpdatePlayerLocation(object sender, EventArgs e)
        {
            switch (map.Player.MovementsCondition)
            {
                case MovementConditions.RunningRight:
                    map.Player.Location = new Point(map.Player.Location.X + 1, map.Player.Location.Y);
                    break;
                case MovementConditions.RunningLeft:
                    map.Player.Location = new Point(map.Player.Location.X - 1, map.Player.Location.Y);
                    break;
                case MovementConditions.JumpingRight:
                    physics.MoveCreatureByY(map.Player, (double)timerForPlayerMovement.Interval/100);
                    break;
                    
            }
        }

        private void AddAnimationsForCreature(ICreature creature)
        {
                animationsForCreatures.Add(creature, AnimationsForCreatures.GetAnimationFor(creature));
        }
    }
}