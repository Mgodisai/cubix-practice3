using ButtonHunterApp.Model;

namespace ButtonHunterApp.View
{
   public interface IGameView
   {
      event EventHandler<GameEventArgs> ButtonClicked;
      event EventHandler TimerTick;
      void InitializeGameUI(int width, int height, string text);
      void UpdateButton(Point newPosition, double prop);
      void UpdateCatchCount(int count);
      void SetTimerInterval(int interval);
      Size GetClientSize();
      void StartGame();
      void StopGame();
      void ResetButton(int width, int height, string text);
      void UpdateStatus(int lives, int highScore);
      void HandleGameOver();
   }
}
