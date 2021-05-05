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
            map = new GameMap("level1.txt");
            timerForCreaturesMovements = new Timer {Interval = 30, Enabled = true};
            timerForCreaturesMovements.Tick += CheckCreaturesForFalling;
            timerForCreaturesMovements.Tick += UpdatePlayerLocationOnMap;

            animationsForCreatures = new Dictionary<ICreature, Dictionary<(MovementConditions, Direction), Animation>>();
            foreach (var creature in map.ListOfCreatures)
                AddAnimationsForCreature(creature);
            new Timer {Interval = 50, Enabled = true}.Tick += (sender, args) =>
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
            if (!ShouldButtonPressesBeProcessed())
                return;

            timerForCreaturesMovements.Interval = e.Modifiers == Keys.Shift ? 10 : 30;
            switch (e.KeyCode)
            {
                case Keys.D:
                    map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Running, Direction.Right);
                    break;

                case Keys.A:
                    map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Running, Direction.Left);
                    break;

                case Keys.S:
                    map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Sitting, map.Player.Direction);
                    break;

                case Keys.W:
                    map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Jumping, map.Player.Direction);
                    return;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (ShouldButtonPressesBeProcessed())
                map.Player.ChangeMovementConditionAndDirectionTo(MovementConditions.Standing, map.Player.Direction);
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