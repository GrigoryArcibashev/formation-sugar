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
        private readonly Timer timerForCreaturesMovements;

        public Game()
        {
            map = new GameMap();
            timerForCreaturesMovements = new Timer {Interval = 100, Enabled = true};
            timerForCreaturesMovements.Tick += CheckCreaturesForFalling;
            timerForCreaturesMovements.Tick += UpdatePlayerLocationOnMap;

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
            /*if (!ShouldButtonPressesBeProcessed())
                return;
            timerForCreaturesMovements.Interval = e.Modifiers == Keys.Shift ? 10 : 30;*/
            switch (e.KeyCode)
            {
                case Keys.D:
                    if (map.Player.IsFalling() || map.Player.IsJumping())
                        map.Player.ChangeMovementConditionAndDirectionTo(map.Player.MovementCondition, Direction.Right);
                    else
                        map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Running, Direction.Right);
                    break;

                case Keys.A:
                    if (map.Player.IsFalling() || map.Player.IsJumping())
                        map.Player.ChangeMovementConditionAndDirectionTo(map.Player.MovementCondition, Direction.Left);
                    else
                        map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Running, Direction.Left);
                    break;

                case Keys.S:
                    if (ShouldButtonPressesBeProcessed())
                        map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Sitting, map.Player.Direction);
                    break;

                case Keys.W:
                    if (ShouldButtonPressesBeProcessed())
                        map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Jumping, Direction.NoMovement);
                    return;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (map.Player.IsFalling() || map.Player.IsJumping())
                map.Player.ChangeMovementConditionAndDirectionTo(map.Player.MovementCondition, Direction.NoMovement);
            else
            {
                var direction = map.Player.Direction == Direction.NoMovement ? Direction.Right : map.Player.Direction;
                map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Standing, direction);
            }
        }

        private bool ShouldButtonPressesBeProcessed()
        {
            return !map.Player.IsFalling() && !map.Player.IsJumping();
        }

        private void CheckCreaturesForFalling(object sender, EventArgs eventArgs)
        {
            map.CheckCreaturesForFalling();
        }

        private void UpdatePlayerLocationOnMap(object sender, EventArgs e)
        {
            PlayerLocationUpdater.UpdatePlayerLocation(map);
        }

        private void AddAnimationsForCreature(ICreature creature)
        {
            animationsForCreatures.Add(creature, AnimationsForCreatures.GetAnimationFor(creature));
        }
    }
}