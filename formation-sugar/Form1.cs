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
        private readonly GameMap map;
        private readonly Dictionary<ICreature, Dictionary<MovementConditions, Animation>> animationsForCreatures;
        private readonly Timer timerForPlayerAnimation;
        private readonly Timer timerForCreaturesMovements;

        public Form1()
        {
            map = new GameMap("test1.txt");
            animationsForCreatures = new Dictionary<ICreature, Dictionary<MovementConditions, Animation>>();

            foreach (var creature in map.ListOfCreatures)
                AddAnimationsForCreature(creature);

            timerForPlayerAnimation = new Timer {Interval = 100, Enabled = true};
            timerForPlayerAnimation.Tick += (sender, args) =>
            {
                animationsForCreatures[map.Player][map.Player.MovementCondition].MoveNextSprite();
                Invalidate();
            };

            timerForCreaturesMovements = new Timer {Interval = 30, Enabled = true};
            timerForCreaturesMovements.Tick += CheckCreaturesForFalling;
            timerForCreaturesMovements.Tick += UpdatePlayerLocationOnMap;

            ClientSize = new Size(620, 360);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            GraphicsCreator.CreateGraphic(graphics, animationsForCreatures, map.ListOfCreatures);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            timerForCreaturesMovements.Interval = e.Modifiers == Keys.Shift ? 10 : 30;
            if (!ShouldButtonPressesBeProcessed())
                return;
            switch (e.KeyCode)
            {
                case Keys.D:
                    map.Player.ChangeConditionToRun(Direction.Right);
                    break;

                case Keys.A:
                    map.Player.ChangeConditionToRun(Direction.Left);
                    break;

                case Keys.S:
                    map.Player.ChangeConditionToSitting();
                    break;

                case Keys.W:
                    timerForPlayerAnimation.Interval = 200;
                    map.Player.ChangeConditionToJumping();
                    return;
            }

            timerForPlayerAnimation.Interval = 100;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (!map.Player.IsFalling() && !map.Player.IsJumping())
                map.Player.ChangeConditionToStanding();
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