using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ButtonAnimationWPF
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      private Thickness originalThickness;
      private readonly Storyboard borderAnimationStoryboard;
      public MainWindow()
      {
         InitializeComponent();
         originalThickness = MyButton.BorderThickness;
         borderAnimationStoryboard = new Storyboard();
         InitializeRotateTransform();
         InitializeBorderAnimation();
      }

      private void InitializeRotateTransform()
      {
         RotateTransform rotateTransform = new()
         {
            CenterX = MyButton.Width / 2,
            CenterY = MyButton.Height / 2
         };
         MyButton.RenderTransform = rotateTransform;
      }

      private void InitializeBorderAnimation()
      {
         ThicknessAnimation borderAnimation = new()
         {
            From = originalThickness,
            To = new Thickness(100),
            Duration = TimeSpan.FromSeconds(10)
         };

         Storyboard.SetTarget(borderAnimation, MyButton);
         Storyboard.SetTargetProperty(borderAnimation, new PropertyPath(Button.BorderThicknessProperty));
         borderAnimationStoryboard.Children.Add(borderAnimation);
      }


      private void RotateButton()
      {
         if (MyButton.RenderTransform is RotateTransform rotateTransform)
         {
            DoubleAnimation animation = new()
            {
               To = rotateTransform.Angle + 15,
               Duration = TimeSpan.FromSeconds(1)
            };

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, animation);
         }
      }

      private void ResetRotation()
      {
         if (MyButton.RenderTransform is RotateTransform rotateTransform)
         {
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, null);
            rotateTransform.Angle = 0;
         }
      }

      private void MyButton_MouseEnterHandler(object sender, MouseEventArgs e)
      {
         if (sender is Button)
         {
            borderAnimationStoryboard.Begin();
         }
      }

      private void MyButton_MouseLeaveHandler(object sender, MouseEventArgs e)
      {
         if (sender is Button MyButton)
         {
            borderAnimationStoryboard.Stop();
            MyButton.BorderThickness = originalThickness;
            ResetRotation();
         }
      }

      private void MyButton_ClickHandler(object sender, RoutedEventArgs e)
      {
         if (sender is Button)
         {
            RotateButton();
         }
      }
   }
}
