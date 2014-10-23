using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using System.Diagnostics;
using Windows.Devices.Sensors;

namespace Project
{
    public class Camera
    {
        public Matrix View;
        public Matrix Projection;
        public LabGame game;
        public Vector3 pos;
        public Vector3 oldPos;
        public Vector2 rot;

        // Ensures that all objects are being rendered from a consistent viewpoint
        public Camera(LabGame game) {
            this.rot = new Vector2(0, 0);

            // Store game
            this.game = game;

            // Store pos
            pos = new Vector3(0, 5, -20);
            
            View = Matrix.LookAtLH(pos, new Vector3(0, 0, 0), Vector3.UnitY);
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.01f, 1000.0f);
            
        }

        // If the screen is resized, the projection matrix will change
        public void Update(GameTime gameTime)
        {
            // Update camera rotation
            if (game.shouldCamRot && game.accelerometerReading != null)
            {
                this.rot.X -= (float) (game.accelerometerReading.AccelerationX) * gameTime.ElapsedGameTime.Milliseconds / 1000;
            }

            // Update our position
            calcPos();

            // Calculate new projection
            Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)game.GraphicsDevice.BackBuffer.Width / game.GraphicsDevice.BackBuffer.Height, 0.1f, 100.0f);
            View = Matrix.LookAtLH(this.pos, new Vector3(0, 0, 0), Vector3.UnitY);

            // Update game objects
            this.game.cameraUpdated();
        }

        public void calcPos()
        {
            // Default camera
            Vector3 front = new Vector3(0, 5, -20);
            Matrix rotation = Matrix.RotationY(this.rot.X);

            // Update postion
            this.pos = Vector3.TransformCoordinate(front, rotation);
        }
    }
}
