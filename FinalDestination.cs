using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    class FinalDestination : Wall
    {
        // Size of the arena
        private readonly static int width = 28;
        private readonly static int height = 2;
        private readonly static int depth = 14;

        public FinalDestination(LabGame game, Vector3 position) : base(game, width, height, position)
        {
        }

        public override MyModel CreateWallModel()
        {
            float w = width / 2;
            float h = height / 2;
            float d = depth / 2;

            VertexPositionNormalTexture[] shapeArray = new VertexPositionNormalTexture[]{
                // Top - Middle
                new VertexPositionNormalTexture(new Vector3(-w/2, h, -d), new Vector3(0, 1, 0), new Vector2(0.25f, 1f)),
                new VertexPositionNormalTexture(new Vector3(-w/2, h, d), new Vector3(0, 1, 0), new Vector2(0.25f, 0f)),
                new VertexPositionNormalTexture(new Vector3(w/2, h, d), new Vector3(0, 1, 0), new Vector2(0.75f, 0f)),

                new VertexPositionNormalTexture(new Vector3(-w/2, h, -d), new Vector3(0, 1, 0), new Vector2(0.25f, 1f)),
                new VertexPositionNormalTexture(new Vector3(w/2, h, d), new Vector3(0, 1, 0), new Vector2(0.75f, 0f)),
                new VertexPositionNormalTexture(new Vector3(w/2, h, -d), new Vector3(0, 1, 0), new Vector2(0.75f, 1f)),

                // Left Side
                new VertexPositionNormalTexture(new Vector3(-w, h, d/2), new Vector3(0, 1, 0), new Vector2(0.0f, 0.25f)),
                new VertexPositionNormalTexture(new Vector3(-w/2, h, d), new Vector3(0, 1, 0), new Vector2(0.25f, 0.0f)),
                new VertexPositionNormalTexture(new Vector3(-w, h, -d/2), new Vector3(0, 1, 0), new Vector2(0.0f, 0.75f)),

                new VertexPositionNormalTexture(new Vector3(-w, h, -d/2), new Vector3(0, 1, 0), new Vector2(0.0f, 0.75f)),
                new VertexPositionNormalTexture(new Vector3(-w/2, h, d), new Vector3(0, 1, 0), new Vector2(0.25f, 0.0f)),
                new VertexPositionNormalTexture(new Vector3(-w/2, h, -d), new Vector3(0, 1, 0), new Vector2(0.25f, 1.0f)),

                // Right Side
                new VertexPositionNormalTexture(new Vector3(w, h, d/2), new Vector3(0, 1, 0), new Vector2(1.0f, 0.25f)),
                new VertexPositionNormalTexture(new Vector3(w, h, -d/2), new Vector3(0, 1, 0), new Vector2(1.0f, 0.75f)),
                new VertexPositionNormalTexture(new Vector3(w/2, h, d), new Vector3(0, 1, 0), new Vector2(0.75f, 0.0f)),
                
                new VertexPositionNormalTexture(new Vector3(w, h, -d/2), new Vector3(0, 1, 0), new Vector2(1.0f, 0.75f)),
                new VertexPositionNormalTexture(new Vector3(w/2, h, -d), new Vector3(0, 1, 0), new Vector2(0.75f, 1.0f)),
                new VertexPositionNormalTexture(new Vector3(w/2, h, d), new Vector3(0, 1, 0), new Vector2(0.75f, 0.0f)),
            };

            // Create the beast
            return game.assets.CreateTexturedObject("finaldestination.png", shapeArray);
        }
    }
}
