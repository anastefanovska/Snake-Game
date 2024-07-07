using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class GameOverForm : Form
    {
        private int score;
        private int highScore;

        public GameOverForm(int score, int highScore)
        {
            this.score = score;
            this.highScore = highScore;

            InitializeComponent();

            // Set form properties
            this.ClientSize = new Size(350, 450);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Create and add the snake PictureBox
            PictureBox snakePictureBox = new PictureBox
            {
                Image = Resource1.snake,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(75, 40),
                Size = new Size(200, 100)
            };
            this.Controls.Add(snakePictureBox);

            // Create and add the score label
            Label scoreLabel = new Label
            {
                Text = $"Score: {score}",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(75, 160),
                Size = new Size(200, 30)
            };
            this.Controls.Add(scoreLabel);

            // Create and add the high score label
            Label highScoreLabel = new Label
            {
                Text = $"High Score: {highScore}",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(75, 200),
                Size = new Size(200, 30)
            };
            this.Controls.Add(highScoreLabel);

            // Create and add the replay button
            Button replayButton = new Button
            {
                Text = "Replay",
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.Green,
                ForeColor = Color.White,
                Location = new Point(75, 250),
                Size = new Size(200, 50)
            };
            replayButton.Click += ReplayButton_Click;
            this.Controls.Add(replayButton);

            // Create and add the mode selection button
            Button modeButton = new Button
            {
                Text = "Mode Selection",
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.YellowGreen,
                ForeColor = Color.White,
                Location = new Point(75, 310),
                Size = new Size(200, 50)
            };
            modeButton.Click += ModeButton_Click;
            this.Controls.Add(modeButton);

            // Create and add the exit button
            Button exitButton = new Button
            {
                Text = "Exit",
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.Red,
                ForeColor = Color.White,
                Location = new Point(75, 370),
                Size = new Size(200, 50)
            };
            exitButton.Click += ExitButton_Click;
            this.Controls.Add(exitButton);
        }

        private void ReplayButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ModeButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
