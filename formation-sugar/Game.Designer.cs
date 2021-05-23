using System.Drawing;
using System.Windows.Forms;
using View;
using View.Animations;

namespace formation_sugar
{
    sealed partial class Game
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
        
        private void InitializeComponent()
        {
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer 
                | ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint,
                true);
            UpdateStyles();
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(1500, 1000);
            this.WindowState = FormWindowState.Maximized;
            this.MaximumSize = ClientSize;
            this.MinimumSize = ClientSize;
            this.MaximizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Text = "Game";
            this.BackgroundImage = Background.BackGroundForTheGame;
        }
    }
}