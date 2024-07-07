using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class ModeSelectionForm : Form
    {
        public GameMode SelectedMode { get; private set; }

        private Button btnStandard;
        private Button btnBorderless;
        private Button btnPoisonApple;

        public ModeSelectionForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Select Mode";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            btnStandard = new Button()
            {
                Text = "Standard",
                BackColor = Color.LightGreen,
                Location = new Point(50, 50),
                Size = new Size(300, 50)
            };
            btnStandard.Click += BtnStandard_Click;

            btnBorderless = new Button()
            {
                Text = "Borderless",
                BackColor = Color.LightBlue,
                Location = new Point(50, 120),
                Size = new Size(300, 50)
            };
            btnBorderless.Click += BtnBorderless_Click;

            btnPoisonApple = new Button()
            {
                Text = "Poisoned Apple",
                BackColor = Color.LightCoral,
                Location = new Point(50, 190),
                Size = new Size(300, 50)
            };
            btnPoisonApple.Click += BtnPoisonApple_Click;

            this.Controls.Add(btnStandard);
            this.Controls.Add(btnBorderless);
            this.Controls.Add(btnPoisonApple);
        }

        private void BtnStandard_Click(object sender, EventArgs e)
        {
            SelectedMode = GameMode.Standard;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnBorderless_Click(object sender, EventArgs e)
        {
            SelectedMode = GameMode.Borderless;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnPoisonApple_Click(object sender, EventArgs e)
        {
            SelectedMode = GameMode.PoisonedApple;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
