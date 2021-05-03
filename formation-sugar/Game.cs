using System;
using System.Collections.Generic;
using System.Windows.Forms;
using formation_sugar.GameModel;
using formation_sugar.View;

namespace formation_sugar
{
    public sealed partial class Game : Form
    {
        private readonly GameMap map;
        private readonly Dictionary<ICreature, Dictionary<MovementConditions, Animation>> animationsForCreatures;
        private readonly Timer timerForCreaturesMovements;

        public Game()
        {
            map = new GameMap("level1.txt");
            timerForCreaturesMovements = new Timer {Interval = 30, Enabled = true};
            timerForCreaturesMovements.Tick += CheckCreaturesForFalling;
            timerForCreaturesMovements.Tick += UpdatePlayerLocationOnMap;

            animationsForCreatures = new Dictionary<ICreature, Dictionary<MovementConditions, Animation>>();
            foreach (var creature in map.ListOfCreatures)
                AddAnimationsForCreature(creature);
            new Timer {Interval = 50, Enabled = true}.Tick += (sender, args) =>
            {
                animationsForCreatures[map.Player][map.Player.MovementCondition].MoveNextSprite();
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
                    map.Player.ChangeConditionToRun(Direction.Right);
                    break;

                case Keys.A:
                    map.Player.ChangeConditionToRun(Direction.Left);
                    break;

                case Keys.S:
                    map.Player.ChangeConditionToSitting();
                    break;

                case Keys.W:
                    map.Player.ChangeConditionToJumping();
                    return;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (ShouldButtonPressesBeProcessed())
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