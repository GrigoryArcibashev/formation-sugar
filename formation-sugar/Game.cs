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
        private Timer timerForCreaturesActions;
        private Timer timerForCreaturesAnimations;
        private Timer timerForHearthAnimation;
        private List<Timer> timers;
        private Dictionary<ICreature, Dictionary<(MovementCondition, Direction), Animation>> animationsForCreatures;
        private Label playerHealthPoints;
        private Label score;
        private bool wIsPressed;
        private bool aIsPressed;
        private bool dIsPressed;
        private bool rIsPressed;
        private bool nIsPressed;
        private bool spaceIsPressed;

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

                case Keys.N:
                    nIsPressed = true;
                    break;

                case Keys.R:
                    rIsPressed = true;
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (map.Player.IsDead())
                return;

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

                case Keys.N:
                    nIsPressed = false;
                    break;

                case Keys.R:
                    rIsPressed = false;
                    break;
            }

            /*
            if (map.Player.IsFallingOrJumping())
                map.Player.ChangeMovementConditionAndDirectionTo(map.Player.MovementCondition, Direction.NoMovement);
            else
                map.Player.ChangeMovementConditionAndDirectionTo(MovementCondition.Standing,
                    map.Player.Direction == Direction.NoMovement ? Direction.Right : map.Player.Direction);*/
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
            timerForCreaturesActions = new Timer {Interval = 80, Enabled = true};
            timerForCreaturesActions.Tick += delegate
            {
                CheckGameStatus(
                    nextLevel: GameStatusChecker.GameWon(map) || nIsPressed,
                    resetLevel: GameStatusChecker.GameOver(map) || rIsPressed);
                map.CheckCreaturesForFalling();
                ProcessKeystrokes();
                CreatureLocationAndConditionsUpdater.UpdateLocationAndCondition(map);
                map.RemoveCreaturesFromMapIfTheyAreDead();
                map.MakeEnemiesAttackingOrRunning();
                UpdateInfoAboutGame();
            };
        }

        private void UpdateInfoAboutGame()
        {
            playerHealthPoints.Text = map.Player.Health.ToString();
            score.Text = @"Score: " + map.TotalScore;
        }

        private void ResetTimerForCreaturesAnimations()
        {
            timerForCreaturesAnimations = new Timer {Interval = 85, Enabled = true};
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
                new Dictionary<ICreature, Dictionary<(MovementCondition, Direction), Animation>>();
            foreach (var creature in map.ListOfCreatures)
                AddAnimationsForCreature(creature);
        }

        private void AddAnimationsForCreature(ICreature creature)
        {
            animationsForCreatures.Add(creature, AnimationsForCreatures.GetAnimationFor(creature));
        }

        private void InitializeInterface()
        {
            playerHealthPoints = new Label
            {
                Text = map.Player.Health.ToString(),
                Location = new Point(40, 10),
                Font = new Font(FontFamily.GenericMonospace, 12.0f, FontStyle.Bold),
                BackColor = Color.Transparent,
                ForeColor = Color.Aqua
            };

            score = new Label
            {
                Text = @"Score: " + map.TotalScore,
                Location = new Point(ClientSize.Width / 2, 10),
                Size = new Size(300, 30),
                Font = new Font(FontFamily.GenericMonospace, 12.0f, FontStyle.Bold),
                BackColor = Color.Transparent,
                ForeColor = Color.Aqua
            };

            Controls.Add(playerHealthPoints);
            Controls.Add(score);
        }

        private void ProcessKeystrokes()
        {
            if (!nIsPressed && !rIsPressed)
                ProcessorPlayerMovementKeys.ProcessPlayerMovementKeys(
                    map,
                    wIsPressed,
                    dIsPressed,
                    aIsPressed,
                    spaceIsPressed);
        }

        private void CheckGameStatus(bool resetLevel = false, bool nextLevel = false)
        {
            if (!GameStatusChecker.CheckGameStatus(map, resetLevel, nextLevel))
                return;
            StopAllTimers();
            InitializeGame();
        }

        private void StopAllTimers()
        {
            foreach (var timer in timers)
                timer.Enabled = false;
        }
    }
}