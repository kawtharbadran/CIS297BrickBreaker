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
using Windows.ApplicationModel.Core;
using System.ComponentModel;
using Windows.Gaming.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BrickBreaker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Play : Page
    {
        public Pong pong;
        private CanvasBitmap spriteSheet;
        private Gamepad controller;

        public Play()
        {
            this.InitializeComponent();
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
            var spriteBatch = args.DrawingSession.CreateSpriteBatch();
            pong.DrawGame(args.DrawingSession, spriteBatch);
            spriteBatch.Dispose();
        }

        private async void Canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            pong.Update();

            if (Gamepad.Gamepads.Count > 0)
            {
                controller = Gamepad.Gamepads.First();
                var reading = controller.GetCurrentReading();
                if (reading.Buttons.HasFlag(GamepadButtons.B))
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(MainPage)); });
                }
            }
            if (pong.gameOver)
            {
              await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(GameOver)); });
            }
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ScoreBox.Text = pong.score.ToString();
            });
            if (pong.DrawableItemsCount <= 5)//if only walls, ball, and paddle are left, the game ends
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Frame.Navigate(typeof(GameOver)); });
            }
        }

       /* private void Canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            pong.Update();
        }*/

        private void PlayPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
        
        private void PlayPage_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {

        }

        private void PlayPage_CreateResources_1(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());

        }
        private async Task CreateResourcesAsync(CanvasAnimatedControl sender)
        {
            spriteSheet = await CanvasBitmap.LoadAsync(sender, "Assets/iconSheet.PNG");
            pong = new Pong(spriteSheet);
        }

        private void PlayPage_Unloaded(object sender, RoutedEventArgs e)
        {/*
            PlayPage.RemoveFromVisualTree();
            PlayPage = null;
            */
        }
    }
}
