using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Toolkit.Input;

namespace Project
{
    class Controls
    {
        // The game we are attached to
        private LabGame game;

        public Controls(LabGame game)
        {
            this.game = game;
        }

        public Boolean leftDown(int playerID)
        {
            if (playerID != 0) return false;
            return game.keyboardState.IsKeyDown(Keys.Left);
        }

        public Boolean rightDown(int playerID)
        {
            if (playerID != 0) return false;
            return game.keyboardState.IsKeyDown(Keys.Right);
        }

        public Boolean jumpDown(int playerID)
        {
            if (playerID != 0) return false;
            return game.keyboardState.IsKeyDown(Keys.Up);
        }
    }
}
