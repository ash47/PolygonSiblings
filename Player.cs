using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
using Windows.UI.Input;
using Windows.UI.Core;
using Box2DX.Dynamics;
using Box2DX.Common;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;
    // Player class.
    class Player : GameObject
    {
        //private float speed = 0.006f;
        private float projectileSpeed = 20;

        // The physics body
        private Body body;

        public Player(LabGame game)
        {
            this.game = game;
            type = GameObjectType.Player;
            myModel = game.assets.GetModel("player", CreatePlayerModel);
            pos = new SharpDX.Vector3(0, game.boundaryBottom + 0.5f, 0);
            GetParamsFromModel();

            // Define the dynamic body. We set its position and call the body factory.
            BodyDef bodyDef = new BodyDef();
            bodyDef.Position.Set(0.0f, 4.0f);
            body = game.getWorld().CreateBody(bodyDef);

            // Define another box shape for our dynamic body.
            PolygonDef shapeDef = new PolygonDef();
            shapeDef.SetAsBox(0.5f, 0.5f);

            // Set the box density to be non-zero, so it will be dynamic.
            shapeDef.Density = 1.0f;

            // Override the default friction.
            shapeDef.Friction = 0.3f;

            // Add the shape to the body.
            body.CreateFixture(shapeDef);

            // Now tell the dynamic body to compute it's mass properties base
            // on its shape.
            body.SetMassFromShapes();
        }

        public MyModel CreatePlayerModel()
        {
            return game.assets.CreateTexturedCube("player.png", 1f);
        }

        // Method to create projectile texture to give to newly created projectiles.
        private MyModel CreatePlayerProjectileModel()
        {
            return game.assets.CreateTexturedCube("player projectile.png", new Vector3(0.3f, 0.2f, 0.25f));
        }

        // Shoot a projectile.
        private void fire()
        {
            game.Add(new Projectile(game,
                game.assets.GetModel("player projectile", CreatePlayerProjectileModel),
                pos,
                new Vector3(0, projectileSpeed, 0),
                GameObjectType.Enemy
            ));
        }

        // Frame update.
        public override void Update(GameTime gameTime)
        {
            //if (game.keyboardState.IsKeyDown(Keys.Space)) { fire(); }

            // TASK 1: Determine velocity based on accelerometer reading
           // pos.X += (float)game.accelerometerReading.AccelerationX;

            // Keep within the boundaries.
            //if (pos.X < game.boundaryLeft) { pos.X = game.boundaryLeft; }
            //if (pos.X > game.boundaryRight) { pos.X = game.boundaryRight; }

            // Update our position
            Vec2 physPos = body.GetPosition();
            pos.X = physPos.X;
            pos.Y = physPos.Y;

            basicEffect.World = Matrix.Translation(pos);
        }

        // React to getting hit by an enemy bullet.
        public void Hit()
        {
            game.Exit();
        }

        public override void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
            fire();
        }

        public override void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            pos.X += (float)args.Delta.Translation.X / 100;
        }
    }
}