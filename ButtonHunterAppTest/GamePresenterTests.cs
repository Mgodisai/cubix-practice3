using ButtonHunterApp.Model;
using ButtonHunterApp.Presenter;
using ButtonHunterApp.View;
using Moq;
using System.Drawing;

namespace ButtonHunterAppTest
{
   [TestClass]
   public class GamePresenterTests
   {
      private Mock<IGameView> mockView;
      private GameData model;

      [TestInitialize]
      public void Initialize()
      {
         mockView = new Mock<IGameView>();
         model = new GameData();
         _ = new GamePresenter(mockView.Object, model);
      }

      [TestMethod]
      public void Initial_Game_Setup_Correctly()
      {
         // Assert
         mockView.Verify(v => v.InitializeGameUI(GameData.InitialButtonWidth, GameData.InitialButtonHeight, GameData.StartingButtonText), Times.Once());
         Assert.AreEqual(GameData.StartingLives, model.ActualLives);
      }

      [TestMethod]
      public void HandleButtonClick_IncrementsScore_And_MovesButton()
      {
         // Arrange
         int initialScore = model.CatchCount;
         mockView.Setup(a => a.InitializeGameUI(GameData.InitialButtonWidth, GameData.InitialButtonHeight, GameData.StartingButtonText));
         mockView.Setup(v => v.GetClientSize()).Returns(new Size(500, 500));

         // Act
         mockView.Raise(m => m.ButtonClicked += null, new GameEventArgs(GameEvent.ButtonClick));

         // Assert
         Assert.AreEqual(initialScore + 1, model.CatchCount);
         mockView.Verify(v => v.UpdateButton(It.IsAny<Point>(), GameData.SizeDecreaseProportion), Times.AtLeastOnce());
      }

      [TestMethod]
      public void TimerTick_HandlesButtonMove_LivesDecrease_AndGameStops()
      {
         // Arrange
         mockView.Setup(v => v.GetClientSize()).Returns(new Size(500, 500));
         model.ActualLives = 1;

         // Act
         mockView.Raise(v => v.TimerTick += null, EventArgs.Empty);

         // Assert
         mockView.Verify(v => v.UpdateButton(It.IsAny<Point>(), GameData.SizeDecreaseProportion), Times.Once());
         Assert.AreEqual(0, model.ActualLives);
         Assert.IsTrue(model.IsGameOver);
         mockView.Verify(v => v.StopGame(), Times.Once());
      }

      [TestMethod]
      public void Game_Can_Restart_After_GameOver()
      {
         // Arrange
         int newHighScore = 15;
         model.ActualLives = 0;
         model.CatchCount = newHighScore;
         model.HighScore = 14;

         // Act
         mockView.Raise(v => v.ButtonClicked += null, new GameEventArgs(GameEvent.Restart));

         // Assert
         Assert.AreEqual(GameData.StartingLives, model.ActualLives);
         Assert.AreEqual(0, model.CatchCount);
         Assert.IsFalse(model.IsGameOver);
         Assert.AreEqual(GameData.StartingInterval, model.TimeUntilNextJump);
         Assert.AreEqual(newHighScore, model.HighScore);
         mockView.Verify(v => v.ResetButton(GameData.InitialButtonWidth, GameData.InitialButtonHeight, GameData.StartingButtonText), Times.AtLeastOnce());
      }

      [TestMethod]
      public void OnTimerTick_MovesButton()
      {
         // Arrange
         var initialPosition = new Point(100, 100);
         mockView.Setup(v => v.GetClientSize()).Returns(new Size(500, 500));
         mockView.Setup(v => v.UpdateButton(It.IsAny<Point>(), It.IsAny<double>()))
                 .Callback<Point, double>((p, d) => initialPosition = p);

         // Act
         mockView.Raise(v => v.TimerTick += null, EventArgs.Empty);

         // Assert
         Assert.AreNotEqual(100, initialPosition.X);
         Assert.AreNotEqual(100, initialPosition.Y);
      }

      [TestMethod]
      public void TimerTick_DecreasesTimeUntilNextJump()
      {
         // Arrange
         mockView.Setup(v => v.GetClientSize()).Returns(new Size(500, 500)); 
         int initialTime = model.TimeUntilNextJump;

         // Act
         mockView.Raise(v => v.TimerTick += null, EventArgs.Empty);

         // Assert
         Assert.IsTrue(model.TimeUntilNextJump < initialTime);
         mockView.Verify(v => v.SetTimerInterval(It.IsAny<int>()), Times.AtLeastOnce());
      }
   }
}