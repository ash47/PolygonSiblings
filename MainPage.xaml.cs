// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using SharpDX;
using System.Diagnostics;

namespace Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public readonly LabGame game;
        public MainMenu mainMenu;
        public MainPage()
        {
            InitializeComponent();
            game = new LabGame(this);
            game.Run(this);
            mainMenu = new MainMenu(this);
            this.Children.Add(mainMenu);
        }

        public void UpdateDamage(int playerID, ushort damage, int lives)
        {
            string msg = "Player "+(playerID+1)+"\n" + damage + "%\n"+lives+" lives";

            if (playerID == 0)
            {
                txtDamage1.Text = msg;
            }
            else if (playerID == 1)
            {
                txtDamage2.Text = msg;
            }
            else if (playerID == 2)
            {
                txtDamage3.Text = msg;
            }
            else if (playerID == 3)
            {
                txtDamage4.Text = msg;
            }
        }

        // TASK 2: Starts the game.  Not that it seems easier to simply move the game.Run(this) command to this function,
        // however this seems to result in a reduction in texture quality on some machines.  Not sure why this is the case
        // but this is an easy workaround.  Not we are also making the command button invisible after it is clicked
        public void StartGame()
        {
            this.Children.Remove(mainMenu);
            game.startMatch();
            game.started = true;
        }

        public void StopGame(int winnerID)
        {
            game.started = false;
            mainMenu = new MainMenu(this);
            this.Children.Add(mainMenu);

            this.txtWinner.Text = "Player " + (winnerID+1) + " wins!";
        }

        private void txtDamage1_SelectionChanged(object sender, RoutedEventArgs e)
        {
            
        }

        public void updateControls()
        {
            game.onscreenControls[0] = this.btnCmdLeft.IsPressed;
            game.onscreenControls[1] = this.btnCmdJump.IsPressed;
            game.onscreenControls[2] = this.btnCmdAttack.IsPressed;
            game.onscreenControls[3] = this.btnCmdRight.IsPressed;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(game != null)
            {
                game.shouldCamRot = comboBox1.SelectedIndex == 1;
                Debug.WriteLine(game.shouldCamRot);
            }
        }
    }
}
