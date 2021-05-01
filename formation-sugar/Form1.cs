﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using formation_sugar.GameModel;
using formation_sugar.View;

namespace formation_sugar
{
    public sealed partial class Form1 : Form
    {
        private readonly GameMap map;
        private readonly Dictionary<ICreature, Dictionary<MovementConditions, Animation>> animationsForCreatures;
        private readonly Timer timerForPlayerAnimation;
        private readonly Timer timerForPlayerMovement;

        public Form1()
        {
            map = new GameMap(1);
            animationsForCreatures = new Dictionary<ICreature, Dictionary<MovementConditions, Animation>>();
            
            foreach (var creature in map.CreaturesToDraw)
            {
                AddAnimationsForCreature(creature);
            }
            
            timerForPlayerAnimation = new Timer {Interval = 100, Enabled = true};
            timerForPlayerAnimation.Tick += (sender, args) =>
            {
                animationsForCreatures[map.Player][map.Player.MovementCondition].MoveNextSprite();
                Invalidate();
            };

            timerForPlayerMovement = new Timer {Interval = 30, Enabled = true};
            timerForPlayerMovement.Tick += UpdatePlayerLocation;
            
            ClientSize = new Size(620, 360);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            GraphicsCreator.CreateGraphic(graphics, animationsForCreatures, map.CreaturesToDraw);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            timerForPlayerMovement.Interval = e.Modifiers == Keys.Shift ? 10 : 30;

            if (map.Player.IsPlayerFalling() || map.Player.IsPlayerJumping())
            {
                return;
            }
            
            switch (e.KeyCode)
            {
                case Keys.D:
                    map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.RunningRight, Direction.Right);
                    break;

                case Keys.A:
                    map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.RunningLeft, Direction.Left);
                    break;

                case Keys.S:
                    map.Player.MovementCondition = map.Player.Direction is Direction.Right
                        ? MovementConditions.SittingRight
                        : MovementConditions.SittingLeft;
                    break;

                case Keys.W:
                    timerForPlayerAnimation.Interval = 200;
                        map.Player.MovementCondition = map.Player.Direction is Direction.Right
                            ? MovementConditions.JumpingRight
                            : MovementConditions.JumpingLeft;
                    return;
            }
            
            timerForPlayerAnimation.Interval = 100;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (!map.Player.IsPlayerFalling() && !map.Player.IsPlayerJumping())
                map.Player.MovementCondition = map.Player.Direction is Direction.Right
                    ? MovementConditions.StandingRight
                    : MovementConditions.StandingLeft;
        }

        private void UpdatePlayerLocation(object sender, EventArgs e)
        {
            PlayerLocationUpdater.UpdatePlayerLocation(map, ClientSize.Height);
        }
        
        private void AddAnimationsForCreature(ICreature creature)
        {
            animationsForCreatures.Add(creature, AnimationsForCreatures.GetAnimationFor(creature));
        }
    }
}