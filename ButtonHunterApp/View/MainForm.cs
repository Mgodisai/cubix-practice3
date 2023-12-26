using ButtonHunterApp.Model;
using ButtonHunterApp.Presenter;
using ButtonHunterApp.View;
using Timer = System.Windows.Forms.Timer;

namespace Practice3
{
   public partial class MainForm : Form, IGameView
   {
      public event EventHandler<GameEventArgs> ButtonClicked;
      public event EventHandler TimerTick;
      private Button GameButton;
      private Timer GameTimer;
      private StatusStrip GameStatusStrip;
      private ToolStripLabel GameStatusLabel;

      public MainForm() => InitializeComponent();

      private void MainForm_Load(object sender, EventArgs e)
      {
         _ = new GamePresenter(this, new GameData());
      }

      public void InitializeGameUI(int buttonWidth, int buttonHeight, string buttonText)
      {
         CreateButton(buttonWidth, buttonHeight, buttonText);
         CreateTimer();
         CreateStatusStrip();
      }

      private void CreateButton(int width, int height, string text)
      {
         GameButton = new Button();
         ResetButton(width, height,text);
         this.Controls.Add(GameButton);
         GameButton.Click += GameButton_Click;
      }

      private void CreateTimer()
      {
         GameTimer = new Timer();
         GameTimer.Tick += GameTimer_Tick;
      }

      private void CreateStatusStrip()
      {
         GameStatusStrip = new StatusStrip();
         GameStatusStrip.BackColor = Color.LightSlateGray;
         GameStatusLabel = new ToolStripLabel();
         GameStatusStrip.Items.Add(GameStatusLabel);
         this.Controls.Add(GameStatusStrip);
      }

      public Size GetClientSize() => ClientSize;

      public void UpdateButton(Point newPosition, double prop) {
         GameButton.Location = newPosition;
         GameButton.Width = (int)Math.Round(GameButton.Width * prop);
         GameButton.Height = (int)Math.Round(GameButton.Height * prop);
      }

      public void UpdateCatchCount(int count) => GameButton.Text = $"Catches: {count}";

      public void SetTimerInterval(int interval) => GameTimer.Interval = interval;

      public void ResetButton(int width, int height, string text)
      {
         GameButton.Width = width;
         GameButton.Height = height;
         GameButton.Text = text;
         GameButton.BackColor = Color.LimeGreen;
      }

      private void GameButton_Click(object? sender, EventArgs e)
      {
            ButtonClicked?.Invoke(this, new GameEventArgs(GameEvent.ButtonClick));
      }

      public void HandleGameOver()
      {
         var result = MessageBox.Show("Would you like to restart?", "New game?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
         if (result == DialogResult.Yes)
            ButtonClicked?.Invoke(this, new GameEventArgs(GameEvent.Restart));
      }

      private void GameTimer_Tick(object? sender, EventArgs e)
      {
         TimerTick?.Invoke(this, EventArgs.Empty);
      }

      public void StartGame()
      {
         if (!GameTimer.Enabled)
         {
            GameButton.BackColor = Color.Yellow;
            GameTimer.Enabled = true;
         }
      }

      public void StopGame()
      {
         GameButton.BackColor = Color.Red;
         GameTimer.Enabled = false;
      }

      public void UpdateStatus(int lives, int highScore)
      {
         GameStatusLabel.Text = $"Lives: {lives} | High Score: {highScore}";
      }
   }
}
