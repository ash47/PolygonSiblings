using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DX.Dynamics;
using Box2DX.Common;
using SharpDX;
using SharpDX.Toolkit;

namespace Project
{
    class Wall : GameObject
    {
        // The physics body
        private Body body;

        // The width of the wall
        private float width;

        // The height of the wall
        private float height;

        public Wall(LabGame game, float width, float height, Vector3 position)
        {
            // Store vars
            this.game = game;
            this.width = width;
            this.height = height;

            // Setup type and model
            type = GameObjectType.Wall;
            myModel = game.assets.GetModel("wall", CreateWallModel);
            pos = new SharpDX.Vector3(position.X, position.Y, 0);
            GetParamsFromModel();

            // Define the dynamic body. We set its position and call the body factory.
            BodyDef bodyDef = new BodyDef();
            bodyDef.Position.Set(position.X, position.Y);
            body = game.getWorld().CreateBody(bodyDef);

            // Define another box shape for our dynamic body.
            PolygonDef shapeDef = new PolygonDef();
            shapeDef.SetAsBox(width / 2, height / 2);

            // Add the shape to the body.
            body.CreateFixture(shapeDef);
        }

        public MyModel CreateWallModel()
        {
            return game.assets.CreateTexturedCube("player.png", new Vector3(this.width, this.height, 1));
        }

        // Frame update
        public override void Update(GameTime gameTime)
        {
            // Update our position
            Vec2 physPos = body.GetPosition();
            pos.X = physPos.X;
            pos.Y = physPos.Y;

            basicEffect.World = Matrix.Translation(pos);
        }

        // Returns the body
        public Body getBody() {
            return this.body;
        }
    }
}
