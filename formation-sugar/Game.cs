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
            // завести булевые премменные на каждую клавишу
            switch (e.KeyCode)
            {
                case Keys.D:
                    map.Player.ChangeMovementConditionAndDirectionTo(
                        map.Player.IsFallingOrJumping() ? map.Player.MovementCondition : MovementConditions.Running,
                        Direction.Right);
                    break;

                case Keys.A:
                    map.Player.ChangeMovementConditionAndDirectionTo(
                        map.Player.IsFallingOrJumping() ? map.Player.MovementCondition : MovementConditions.Running,
                        Direction.Left);
                    break;

                case Keys.S:
                    if (!map.Player.IsFallingOrJumping())
                        map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Sitting, map.Player.Direction);
                    break;

                case Keys.W:
                    if (!map.Player.IsFallingOrJumping())
                        map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Jumping,
                            map.Player.MovementCondition is MovementConditions.Running
                                ? map.Player.Direction
                                : Direction.NoMovement);
                    break;
            }
        }


        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (map.Player.IsFallingOrJumping())
                map.Player.ChangeMovementConditionAndDirectionTo(map.Player.MovementCondition, Direction.NoMovement);
            else
            {
                var direction = map.Player.Direction == Direction.NoMovement ? Direction.Right : map.Player.Direction;
                map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Standing, direction);
            }
        }

        private void PerformActionsWithCreatures(object sender, EventArgs eventArgs)
        {
            map.CheckCreaturesForFalling();
            PlayerLocationUpdater.UpdatePlayerLocation(map);
            map.CheckEnemies();
        }

        private void AddAnimationsForCreature(ICreature creature)
        {
            animationsForCreatures.Add(creature, AnimationsForCreatures.GetAnimationFor(creature));
        }
    }
}