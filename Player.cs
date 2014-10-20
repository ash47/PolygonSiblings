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
using NLua;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;
    // Player class.
    class Player : GameObject
    {
        //private float speed = 0.006f;
        private float projectileSpeed = 20;

        // The ID of the player controlling us
        private int playerID;

        // Our character class
        private CharacterClass character;

        // The physics body
        private Body body;

        // Data for this player
        public LuaTable data;

        // The amount of pieces of ground we are touching
        public int touchingGround = 0;

        public Player(LabGame game, int playerID, int characterIndex)
        {
            this.game = game;
            type = GameObjectType.Player;
            myModel = game.assets.GetModel("player", CreatePlayerModel);
            pos = new SharpDX.Vector3(0, 0, 0);
            GetParamsFromModel();
            this.character = game.getCharacterClass(characterIndex);
            this.playerID = playerID;

            // Define the dynamic body. We set its position and call the body factory.
            BodyDef bodyDef = new BodyDef();
            bodyDef.Position.Set(0.0f, 4.0f);
            body = game.getWorld().CreateBody(bodyDef);
            bodyDef.FixedRotation = true;

            // Define a box shape for our dynamic body.
            PolygonDef shapeDef = new PolygonDef();
            shapeDef.SetAsBox(0.5f, 0.5f);
            shapeDef.Density = 1.0f;
            shapeDef.Friction = 1f;

            // Add the shape to the body.
            body.CreateFixture(shapeDef);

            // Now tell the dynamic body to compute it's mass properties base
            // on its shape.
            body.SetMassFromShapes();

            // Set userdata
            body.SetUserData(this);

            // CREATE FOOT SENSOR
            PolygonDef sensorShape = new PolygonDef();
            sensorShape.SetAsBox(0.5f, 0.1f, new Vec2(0f, -0.5f), 0);
            sensorShape.Density = 1.0f;
            sensorShape.IsSensor = true;

            Fixture sensorFixture = body.CreateFixture(sensorShape);
            

            // Init Character
            if (this.character != null)
            {
                // Build argument list
                object[] args = new object[1];
                args[0] = this;

                // Call the update function
                this.character.init.Call(args);
            }
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
            // Update our position
            Vec2 physPos = body.GetPosition();
            pos.X = physPos.X;
            pos.Y = physPos.Y;

            // Ensure we found our character
            if (this.character != null)
            {
                // Build argument list
                object[] args = new object[2];
                args[0] = gameTime;
                args[1] = this;

                // Call the update function
                this.character.update.Call(args);
            }

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

        // Returns the body
        public Body getBody()
        {
            return this.body;
        }

        // Gets the playerID
        public int getPlayerID()
        {
            return this.playerID;
        }
    }
}