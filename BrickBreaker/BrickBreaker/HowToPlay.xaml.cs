using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BrickBreaker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HowToPlay : Page
    {
        private int instructionCount;
        public HowToPlay()
        {
            this.InitializeComponent();
            instructionCount = 1;
            Uri newuri = new Uri("ms-appx:///Assets/mysound.wav");
            myPlayer.Source = newuri;
            myPlayer.Play();
        }

        private void HowToPlayPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            instructionCount++;
            if (instructionCount == 2)
            {
                string nextImageString = Directory.GetCurrentDirectory() + @"/Instructions/instructions2.PNG";
                InstructionImage.Source = new BitmapImage(new Uri(nextImageString));
            }
            else if (instructionCount == 3)
            {
                string nextImageString = Directory.GetCurrentDirectory() + @"/Instructions/instructions3.PNG";
                InstructionImage.Source = new BitmapImage(new Uri(nextImageString));
            }
        }


        private void BackButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
