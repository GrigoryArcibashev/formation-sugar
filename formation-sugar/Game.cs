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
        private GameMap map;
        private Dictionary<ICreature, Dictionary<(MovementConditions, Direction), Animation>> animationsForCreatures;
        private bool wIsPressed;
        private bool aIsPressed;
        private bool dIsPressed;
        private bool spaceIsPressed;
        private Label playerHealthPoints;
        private Label score;
        private Timer timerForCreaturesActions;
        private Timer timerForCreaturesAnimations;
        private Timer timerForHearthAnimation;
        private List<Timer> timers;

        public Game()
        {
            InitializeComponent();
            map = new GameMap();
            InitializeGame();
            InitializeInterface();
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
        
        private void InitializeGame()
        {
            ResetTimerForCreaturesActions();
            ResetTimerForCreaturesAnimations();
            ResetTimerForHeathAnimation();
            timers = new List<Timer> {timerForCreaturesActions, timerForCreaturesAnimations, timerForHearthAnimation};

            AddAnimations();
        }

        private void ResetTimerForCreaturesActions()
        {
            timerForCreaturesActions = new Timer {Interval = 60, Enabled = true};
            timerForCreaturesActions.Tick += delegate
            {
                CheckGameStatus();
                map.CheckCreaturesForFalling();
                ProcessKeystrokes();
                CreatureLocationAndConditionsUpdater.UpdateLocationAndCondition(map);
                map.RemoveCreaturesFromMapIfTheyAreDead();
                map.MakeEnemiesAttackingOrRunning();
                playerHealthPoints.Text = map.Player.Health.ToString();
                score.Text = Text = @"Score: " + map.Score;
            };
        }
        
        private void ResetTimerForCreaturesAnimations()
        {
            timerForCreaturesAnimations = new Timer {Interval = 100, Enabled = true};
            timerForCreaturesAnimations.Tick += delegate
            {
                foreach (var creature in map.ListOfCreatures)
                    animationsForCreatures[creature][(creature.MovementCondition, creature.Direction)].MoveNextSprite();

                Invalidate();
            };
        }

        private void ResetTimerForHeathAnimation()
        {
            timerForHearthAnimation = new Timer {Interval = 1000, Enabled = true};
            timerForHearthAnimation.Tick += delegate
            {
                PlayerHealthAnimation.HearthAnimation.MoveNextSprite();
                timerForHearthAnimation.Interval = Math.Max(150, map.Player.Health * 5);
            };
        }

        private void AddAnimations()
        {
            animationsForCreatures =
                new Dictionary<ICreature, Dictionary<(MovementConditions, Direction), Animation>>();
            foreach (var creature in map.ListOfCreatures)
                AddAnimationsForCreature(creature);
        }

        private void InitializeInterface()
        {
            playerHealthPoints = new Label
            {
                Text = map.Player.Health.ToString(),
                Location = new Point(40, 10),
                Font = new Font(FontFamily.GenericMonospace, 12.0f, FontStyle.Bold)
            };

            score = new Label
            {
                Text = @"Score: " + map.Score,
                Location = new Point(ClientSize.Width / 2, 10),
                Size = new Size(300, 30),
                Font = new Font(FontFamily.GenericMonospace, 12.0f, FontStyle.Bold)
            };

            Controls.Add(playerHealthPoints);
            Controls.Add(score);
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

        private void AddAnimationsForCreature(ICreature creature)
        {
            animationsForCreatures.Add(creature, AnimationsForCreatures.GetAnimationFor(creature));
        }

        private void CheckGameStatus()
        {
            if (GameOver())
            {
                MapCreator.ResetLevel();
                map = new GameMap();
            }

            else if (GameWon())
                map.LoadNextMap(map.Score);
            else
                return;

            StopAllTimers();
            InitializeGame();
        }

        private void StopAllTimers()
        {
            foreach (var timer in timers)
                timer.Enabled = false;
        }

        private bool GameWon()
        {
            return map.Finish.MovementCondition is MovementConditions.Dying;
        }

        private bool GameOver()
        {
            return map.Player.MovementCondition is MovementConditions.Dying;
        }
    }
}