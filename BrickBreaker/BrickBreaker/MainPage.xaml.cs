using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Gaming.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace BrickBreaker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Gamepad controller = null;

        public MainPage()
        {
            this.InitializeComponent();
            Uri newuri = new Uri("ms-appx:///Assets/mymusic.wav");
            myMusicPlayer.Source = newuri;
            myMusicPlayer.Play();
            if (Gamepad.Gamepads.Count > 0)
            {
                controller = Gamepad.Gamepads.First();
                var reading = controller.GetCurrentReading();
                if (reading.Buttons.HasFlag(GamepadButtons.A))
                {
                    this.Frame.Navigate(typeof(Play));
                }
                else if (reading.Buttons.HasFlag(GamepadButtons.B))
                {
                    Application.Current.Exit();
                }
                else if (reading.Buttons.HasFlag(GamepadButtons.X))
                {
                    this.Frame.Navigate(typeof(Credits));
                }
                else if (reading.Buttons.HasFlag(GamepadButtons.Y))
                {
                    this.Frame.Navigate(typeof(HowToPlay));
                }
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Play));
        }

        private void CreditsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Credits));
        }
        private void How_to_PlayBttn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HowToPlay));
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void MainPage1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MainPage1_Unloaded(object sender, RoutedEventArgs e)
        {
            MainPage1.RemoveFromVisualTree();
            MainPage1 = null;
        }
    }
}