using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Windows.Media.Playback;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BrickBreaker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Play : Page
    {
        Pong pong;
        public Play()
        {
            this.InitializeComponent();

            pong = new Pong();
            Window.Current.CoreWindow.KeyDown += Canvas_KeyDown;
            Window.Current.CoreWindow.KeyUp += Canvas_KeyUp;
            Uri newuri = new Uri("ms-appx:///Assets/mysound.wav");
            myPlayer.Source = newuri;
            myPlayer.Play();

        }

        private void Canvas_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.Left)
            {
                pong.SetPaddleTravelingLeftward(true);
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Right)
            {
                pong.SetPaddleTravelingRightward(true);
            }
        }

        private void Canvas_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.Left)
            {
                pong.SetPaddleTravelingLeftward(false);
            }
            else if (e.VirtualKey == Windows.System.VirtualKey.Right)
            {
                pong.SetPaddleTravelingRightward(false);
            }
        }

        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            pong.DrawGame(args.DrawingSession);
        }
        private async void Canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            pong.UpdateAsync();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ScoreBox.Text = pong.score.ToString();
            });
            
        }

       /* private void Canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            pong.Update();
        }*/

        private void PlayPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(MainPage));
        }
        
    }
}
