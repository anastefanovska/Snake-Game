using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class SnakeForm : Form
    {
        private const int GRID_SIZE = 20;
        private const int CELL_SIZE = 30;
        private const int PADDING = 50;

        private State state;
        private Timer gameTimer;
        private bool gameStarted;
        private int highScore;
        private GameMode selectedMode;

        public SnakeForm()
        {
            InitializeComponent();
            this.ClientSize = new Size(GRID_SIZE * CELL_SIZE + 2 * PADDING, GRID_SIZE * CELL_SIZE + 2 * PADDING + 50);
            this.Paint += Snake_Paint;
            this.KeyDown += Snake_KeyDown;
            this.BackColor = Color.LightGreen;
            this.DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;

            ShowModeSelectionForm();

            InitializeGame();
        }

        private void InitializeGame()
        {
            state = new State(GRID_SIZE, GRID_SIZE, (GameMode)selectedMode);
            gameTimer = new Timer();
            gameTimer.Interval = 100;
            gameTimer.Tick += GameTimer_Tick;
            gameStarted = false;
        }

        private void Snake_Paint(object sender, PaintEventArgs e)
        {
            if (!gameStarted)
            {
                DrawStartMessage(e.Graphics);
            }
            else
            {
                DrawGrid(e.Graphics);
                DrawSnake(e.Graphics);
                DrawFood(e.Graphics);
                DrawScore(e.Graphics);

                if (state.GameOver)
                {
                    gameTimer.Stop();
                    if (state.Score > highScore)
                    {
                        highScore = state.Score;
                    }
                    ShowGameOverForm();
                }
            }
        }

        private void Snake_KeyDown(object sender, KeyEventArgs e)
        {
            if (!gameStarted)
            {
                gameStarted = true;
                gameTimer.Start();
            }
            else if (state.GameOver)
            {
                // Do nothing here
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        state.ChangeDirection(Direction.Up);
                        break;
                    case Keys.Down:
                        state.ChangeDirection(Direction.Down);
                        break;
                    case Keys.Left:
                        state.ChangeDirection(Direction.Left);
                        break;
                    case Keys.Right:
                        state.ChangeDirection(Direction.Right);
                        break;
                }
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (state.GameOver)
            {
                gameTimer.Stop();
                if (state.Score > highScore)
                {
                    highScore = state.Score;
                }
                Invalidate();
                return;
            }

            state.Move();
            AdjustGameSpeed();
            Invalidate();
        }

        private void AdjustGameSpeed()
        {
            int newInterval = 100 - (state.Score * 2); // Decrease interval by 2ms per score
            if (newInterval < 20) // Set a minimum interval to avoid being too fast
            {
                newInterval = 20;
            }
            gameTimer.Interval = newInterval;
        }

        private void DrawGrid(Graphics g)
        {
            using (Pen gridPen = new Pen(Color.Gray, 1))
            {
                for (int i = 0; i <= GRID_SIZE; i++)
                {
                    g.DrawLine(gridPen, PADDING + i * CELL_SIZE, PADDING, PADDING + i * CELL_SIZE, PADDING + GRID_SIZE * CELL_SIZE);
                    g.DrawLine(gridPen, PADDING, PADDING + i * CELL_SIZE, PADDING + GRID_SIZE * CELL_SIZE, PADDING + i * CELL_SIZE);
                }
            }
        }

        private void DrawSnake(Graphics g)
        {
            using (Brush snakeBrush = new SolidBrush(Color.DarkGreen))
            {
                foreach (var position in state.SnakePositions())
                {
                    g.FillRectangle(snakeBrush, PADDING + position.Column * CELL_SIZE, PADDING + position.Row * CELL_SIZE, CELL_SIZE, CELL_SIZE);
                }
            }
        }

        private void DrawFood(Graphics g)
        {
            using (Brush foodBrush = new SolidBrush(Color.Red))
            {
                for (int r = 0; r < state.Rows; r++)
                {
                    for (int c = 0; c < state.Columns; c++)
                    {
                        if (state.Grid[r, c] == GridValue.Food)
                        {
                            g.FillEllipse(foodBrush, PADDING + c * CELL_SIZE, PADDING + r * CELL_SIZE, CELL_SIZE, CELL_SIZE);
                        }
                        else if (state.Grid[r, c] == GridValue.Poison)
                        {
                            g.FillEllipse(Brushes.Maroon, PADDING + c * CELL_SIZE, PADDING + r * CELL_SIZE, CELL_SIZE, CELL_SIZE);
                        }
                    }
                }
            }
        }

        private void DrawScore(Graphics g)
        {
            using (Brush scoreBrush = new SolidBrush(Color.Red))
            using (Font scoreFont = new Font("Arial", 16))
            {
                g.DrawString("Score: " + state.Score, scoreFont, scoreBrush, PADDING, PADDING - 30);
                g.DrawString("High Score: " + highScore, scoreFont, scoreBrush, PADDING + 200, PADDING - 30);
            }
        }

        private void DrawStartMessage(Graphics g)
        {
            using (Brush textBrush = new SolidBrush(Color.White))
            using (Font textFont = new Font("Arial", 16))
            {
                string message = "PRESS ANY KEY TO START";
                SizeF textSize = g.MeasureString(message, textFont);
                g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 0, 0)), 0, 0, ClientSize.Width, ClientSize.Height);
                g.DrawString(message, textFont, textBrush, (ClientSize.Width - textSize.Width) / 2, (ClientSize.Height - textSize.Height) / 2);
            }
        }

        private void ShowGameOverForm()
        {
            using (var gameOverForm = new GameOverForm(state.Score, highScore))
            {
                var result = gameOverForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    RestartGame();
                }
                else if (result == DialogResult.Retry)
                {
                    using (var modeSelectionForm = new ModeSelectionForm())
                    {
                        if (modeSelectionForm.ShowDialog() == DialogResult.OK)
                        {
                            selectedMode = modeSelectionForm.SelectedMode;
                            RestartGame();
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void ShowModeSelectionForm()
        {
            using (var modeSelectionForm = new ModeSelectionForm())
            {
                if (modeSelectionForm.ShowDialog() == DialogResult.OK)
                {
                    selectedMode = modeSelectionForm.SelectedMode;
                    RestartGame();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void RestartGame()
        {
            InitializeGame();
            Invalidate();
        }
    }
}
