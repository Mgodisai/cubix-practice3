using ButtonHunterApp.Model;
using ButtonHunterApp.View;

namespace ButtonHunterApp.Presenter
{
   public class GamePresenter
   {
      private readonly IGameView view;
      private readonly GameData model;
      private readonly Random random = new Random();

      public GamePresenter(IGameView view, GameData model)
      {
         this.view = view;
         this.model = model;
         this.view.InitializeGameUI(GameData.InitialButtonWidth, GameData.InitialButtonHeight, GameData.StartingButtonText);
         this.view.ButtonClicked += OnButtonClicked;
         this.view.TimerTick += OnTimerTick;
         InitGame();
      }

      private void InitGame()
      {
         if (model.CatchCount > model.HighScore)
         {
            model.HighScore = model.CatchCount;
         }
         model.ActualLives = GameData.StartingLives;
         model.TimeUntilNextJump = GameData.StartingInterval;
         model.CatchCount = 0;
         view.ResetButton(GameData.InitialButtonWidth, GameData.InitialButtonHeight, GameData.StartingButtonText);
         view.SetTimerInterval(model.TimeUntilNextJump);
         view.UpdateStatus(model.ActualLives, model.HighScore);
      }

      private void OnButtonClicked(object sender, GameEventArgs e)
      {
         switch (e.GameEvent)
         {
            case GameEvent.ButtonClick:
               HandleButtonClick();
               break;
            case GameEvent.Restart:
               InitGame();
               break;
         }
      }

      private void HandleButtonClick()
      {
         
         if (model.IsGameOver)
         {
            view.HandleGameOver();
         } else
         {
            view.StartGame();
            view.UpdateCatchCount(++model.CatchCount);
            MoveButton();
            DecreaseTime();
         }
      }

      private void OnTimerTick(object sender, EventArgs e)
      {
         MoveButton();
         DecreaseTime();
         model.ActualLives--;
         view.UpdateStatus(model.ActualLives, model.HighScore);
         if (model.IsGameOver)
         {
            view.StopGame();
         }
      }

      private void MoveButton()
      {
         var newX = random.Next(0, view.GetClientSize().Width - GameData.InitialButtonWidth);
         var newY = random.Next(0, view.GetClientSize().Height - GameData.InitialButtonHeight);
         view.UpdateButton(new Point(newX, newY), GameData.SizeDecreaseProportion);
      }

      private void DecreaseTime()
      {
         model.TimeUntilNextJump -= GameData.TimeDecrease; 
         view.SetTimerInterval(Math.Max(1,model.TimeUntilNextJump));
      }
   }
}
