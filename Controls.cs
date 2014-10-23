using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit.Input;
using SharpDX.XInput;
using System.Diagnostics;

namespace Project
{
    public class Controls
    {
        // The game we are attached to
        private LabGame game;

        private Controller xc1;
        private Gamepad x1;

        public Controls(LabGame game)
        {
            this.game = game;

            // Grab xbox controller
            xc1 = new Controller();
        }

        public void update()
        {
            if (xc1.IsConnected)
            {
                x1 = xc1.GetState().Gamepad;
            }
        }

        public Boolean leftDown(int playerID)
        {
            if(game.controlTypes[playerID] == 2)
            {
                return game.keyboardState.IsKeyDown(Keys.Left);
            }
            else if (game.controlTypes[playerID] == 1)
            {
                return game.onscreenControls[0];
            }
            else if(game.controlTypes[playerID] == 3)
            {
                return x1.LeftThumbX < -1000;
            }

            return false;
        }

        public Boolean rightDown(int playerID)
        {
            if (game.controlTypes[playerID] == 2)
            {
                return game.keyboardState.IsKeyDown(Keys.Right);
            }
            else if (game.controlTypes[playerID] == 1)
            {
                return game.onscreenControls[3];
            }
            else if (game.controlTypes[playerID] == 3)
            {
                Debug.WriteLine(x1.LeftThumbX);
                return x1.LeftThumbX > 1000;
            }

            return false;
        }

        public Boolean jumpDown(int playerID)
        {
            if (game.controlTypes[playerID] == 2)
            {
                return game.keyboardState.IsKeyDown(Keys.Up);
            }
            else if (game.controlTypes[playerID] == 1)
            {
                return game.onscreenControls[1];
            }
            else if (game.controlTypes[playerID] == 3)
            {
                return ((int)x1.Buttons & (int)GamepadButtonFlags.A) > 0;
            }
            
            return false;
        }

        public Boolean attackDown(int playerID)
        {
            if (game.controlTypes[playerID] == 2)
            {
                return game.keyboardState.IsKeyDown(Keys.A);
            }
            else if (game.controlTypes[playerID] == 1)
            {
                return game.onscreenControls[2];
            }
            else if (game.controlTypes[playerID] == 3)
            {
                return ((int)x1.Buttons & (int)GamepadButtonFlags.X) > 0;
            }

            return false;
        }
    }
}
