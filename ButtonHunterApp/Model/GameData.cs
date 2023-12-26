namespace ButtonHunterApp.Model
{
   public class GameData
   {
      public const string StartingButtonText = "Catch me if you can";
      public const int StartingInterval = 1500;
      public const int TimeDecrease = 50;
      public const int StartingLives = 5;
      public const int InitialButtonWidth = 120;
      public const int InitialButtonHeight = 120;
      public const double SizeDecreaseProportion = 0.98d;
      
      public int HighScore { get; set; } = 0;
      public int CatchCount { get; set; }
      public int TimeUntilNextJump { get; set; }
      public int ActualLives { get; set; }
      public bool IsGameOver => ActualLives <= 0;
   }
}
