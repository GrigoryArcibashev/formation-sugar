using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Model;
using Model.Creatures;
using View;
using View.AnimationsForCreatures;

namespace formation_sugar
{
    public sealed partial class Game : Form
    {
        private readonly GameMap map;
        private readonly Dictionary<ICreature, Dictionary<(MovementConditions, Direction), Animation>> animationsForCreatures;
        private bool wIsPressed;
        private bool sIsPressed;
        private bool aIsPressed;
        private bool dIsPressed;
        private bool leftMouseButtonIsPressed;

        public Game()
        {
            map = new GameMap();
            var timerForCreaturesActions = new Timer {Interval = 100, Enabled = true};
            timerForCreaturesActions.Tick += PerformActionsWithCreatures;

            animationsForCreatures = new Dictionary<ICreature, Dictionary<(MovementConditions, Direction), Animation>>();
            foreach (var creature in map.ListOfCreatures)
                AddAnimationsForCreature(creature);
            new Timer {Interval = 100, Enabled = true}.Tick += (sender, args) =>
            {
                animationsForCreatures[map.Player][(map.Player.MovementCondition, map.Player.Direction)].MoveNextSprite();
                Invalidate();
            };

            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            GraphicsCreator.CreateGraphic(graphics, animationsForCreatures, map);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    dIsPressed = true;
                    break;

                case Keys.A:
                    aIsPressed = true;
                    break;

                case Keys.S:
                    sIsPressed = true;
                    break;

                case Keys.W:
                    wIsPressed = true;
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    dIsPressed = false;
                    break;

                case Keys.A:
                    aIsPressed = false;
                    break;

                case Keys.S:
                    sIsPressed = false;
                    break;

                case Keys.W:
                    wIsPressed = false;
                    break;
            }

            if (map.Player.IsFallingOrJumping())
                map.Player.ChangeMovementConditionAndDirectionTo(map.Player.MovementCondition, Direction.NoMovement);
            else
                map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Standing,
                    map.Player.Direction == Direction.NoMovement ? Direction.Right : map.Player.Direction);
        }

        private void ProcessKeystrokes()
        {
            if (dIsPressed || aIsPressed)
                map.Player.ChangeMovementConditionAndDirectionTo(
                    map.Player.IsFallingOrJumping() ? map.Player.MovementCondition : MovementConditions.Running,
                    dIsPressed ? Direction.Right : Direction.Left);
            if (wIsPressed && !map.Player.IsFallingOrJumping())
                    map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Jumping,
                        map.Player.MovementCondition is MovementConditions.Running
                            ? map.Player.Direction
                            : Direction.NoMovement);
            if (sIsPressed && !map.Player.IsFallingOrJumping())
                    map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Sitting, map.Player.Direction);
        }

        private void PerformActionsWithCreatures(object sender, EventArgs eventArgs)
        {
            map.CheckCreaturesForFalling();
            ProcessKeystrokes();
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            map.CheckEnemies();
        }

        private void AddAnimationsForCreature(ICreature creature)
        {
            animationsForCreatures.Add(creature, AnimationsForCreatures.GetAnimationFor(creature));
        }
    }
}