using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public MainPage()
        {
            this.InitializeComponent();

            Uri newuri = new Uri("ms-appx:///Assets/mymusic.wav");
            myMusicPlayer.Source = newuri;
            myMusicPlayer.Play();

        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Play));
        }

        private void CreditsButton_Click(object sender, RoutedEventArgs e)
        {

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


    }
}