namespace ButtonHunterApp.Model
{
   public class GameEventArgs : EventArgs
   {
      public GameEvent GameEvent { get; private set; }

      public GameEventArgs(GameEvent gameEvent)
      {
         GameEvent = gameEvent;
      }
   }
}
