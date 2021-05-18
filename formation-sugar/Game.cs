using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Model;
using Model.Creatures.CreatureInterfaces;
using View;
using View.Animations;

namespace formation_sugar
{
    public sealed partial class Game : Form
    {
        private readonly GameMap map;
        private readonly Dictionary<ICreature, Dictionary<(MovementConditions, Direction), Animation>> animationsForCreatures;
        private bool wIsPressed;
        private bool aIsPressed;
        private bool dIsPressed;
        private bool spaceIsPressed;
        private readonly Label playerHealthPoints;

        public Game()
        {
            map = new GameMap();
            map.LoadNextMap();

            var timerForCreaturesActions = new Timer {Interval = 100, Enabled = true};
            timerForCreaturesActions.Tick += PerformActionsWithCreatures;

            animationsForCreatures = new Dictionary<ICreature, Dictionary<(MovementConditions, Direction), Animation>>();
            foreach (var creature in map.ListOfCreatures)
                AddAnimationsForCreature(creature);

            var timerForCreatureAnimations = new Timer {Interval = 100, Enabled = true};
            timerForCreatureAnimations.Tick += (sender, args) =>
            {
                foreach (var creature in map.ListOfCreatures)
                    animationsForCreatures[creature][(creature.MovementCondition, creature.Direction)].MoveNextSprite();
                Invalidate();
            };

            var timerForHearthAnimation = new Timer {Interval = 1000, Enabled = true};
            timerForHearthAnimation.Tick += (sender, args) =>
            {
                PlayerHealthAnimation.HearthAnimation.MoveNextSprite();
                timerForHearthAnimation.Interval = Math.Max(150, map.Player.Health * 5);
            };

            playerHealthPoints = new Label
            {
                Text = map.Player.Health.ToString(),
                Location = new Point(40, 10),
                Font = new Font(FontFamily.GenericMonospace, 10.0f, FontStyle.Bold)
            };
            Controls.Add(playerHealthPoints);

            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            GraphicsCreator.CreateGraphicForCreatures(graphics, animationsForCreatures, map);
            GraphicsCreator.CreateGraphicForPlayersHealth(graphics, PlayerHealthAnimation.HearthAnimation);
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

                case Keys.W:
                    wIsPressed = true;
                    break;

                case Keys.Space:
                    spaceIsPressed = true;
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (map.Player.IsDead())
            {
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.D:
                    dIsPressed = false;
                    break;

                case Keys.A:
                    aIsPressed = false;
                    break;

                case Keys.W:
                    wIsPressed = false;
                    break;

                case Keys.Space:
                    spaceIsPressed = false;
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
            if (map.Player.IsDead())
                return;

            if (dIsPressed || aIsPressed)
                map.Player.ChangeMovementConditionAndDirectionTo(
                    map.Player.IsFallingOrJumping() ? map.Player.MovementCondition : MovementConditions.Running,
                    dIsPressed ? Direction.Right : Direction.Left);

            if (wIsPressed && !map.Player.IsFallingOrJumping())
                map.Player.ChangeMovementConditionAndDirectionTo(
                    MovementConditions.Jumping,
                    map.Player.MovementCondition is MovementConditions.Running
                        ? map.Player.Direction
                        : Direction.NoMovement);

            if (spaceIsPressed && !map.Player.IsFallingOrJumping())
                map.Player.ChangeMovementConditionAndDirectionTo(
                    MovementConditions.Attacking,
                    map.Player.Direction is Direction.NoMovement ? Direction.Right : map.Player.Direction);
        }

        private void PerformActionsWithCreatures(object sender, EventArgs eventArgs)
        {
            map.CheckCreaturesForFalling();
            ProcessKeystrokes();
            CreatureLocationAndConditionsUpdater.UpdateLocationAndCondition(map);
            map.RemoveEnemiesFromMapIfTheyAreDead();
            map.MakeEnemiesAttackingOrRunning();
            playerHealthPoints.Text = map.Player.Health.ToString();
        }

        private void AddAnimationsForCreature(ICreature creature)
        {
            animationsForCreatures.Add(creature, AnimationsForCreatures.GetAnimationFor(creature));
        }
    }
}