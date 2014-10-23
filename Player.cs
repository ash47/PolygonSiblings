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
using System.Diagnostics;

namespace Project
{
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;
    // Player class.
    public class Player : GameObject
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

        // The amount of damage this player has
        private ushort damage;

        // Our actual rotation
        private float rot = 0;

        // How much left to rotate
        private float toRotate = 0;

        // Our direction
        private float direction = 0;

        // The amount of pieces of ground we are touching
        public int touchingGround = 0;

        // Our scaling factor
        private float scaling = 1.0f;

        // Number of lives
        private int lives;

        // Our attack collison
        private Fixture attackFixture;

        public Player(LabGame game, int playerID, int characterIndex, Vec2 spawnPos)
        {
            this.game = game;
            this.playerID = playerID;
            type = GameObjectType.Player;
            myModel = game.assets.GetModel("player"+playerID, CreatePlayerModel);
            pos = new SharpDX.Vector3(spawnPos.X, spawnPos.Y, 0);
            GetParamsFromModel();
            this.character = game.getCharacterClass(characterIndex);

            // Set lives
            this.setLives(3);

            // Add to the number of players left
            game.playersLeft++;

            // Reset damage
            setDamage(0);

            // Define the dynamic body. We set its position and call the body factory.
            BodyDef bodyDef = new BodyDef();
            bodyDef.Position.Set(spawnPos.X, spawnPos.Y);
            body = game.getWorld().CreateBody(bodyDef);
            bodyDef.FixedRotation = true;

            // Define a box shape for our dynamic body.
            PolygonDef shapeDef = new PolygonDef();
            shapeDef.SetAsBox(0.5f, 0.5f);
            shapeDef.Density = 1.0f;
            shapeDef.Friction = 0.1f;
            shapeDef.Filter.CategoryBits = Collisions.CAT_PLAYER;
            shapeDef.Filter.MaskBits = Collisions.MASK_PLAYER;
            shapeDef.Filter.GroupIndex = 0;

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
            sensorShape.Filter.CategoryBits = Collisions.CAT_GROUNDSENSOR;
            sensorShape.Filter.MaskBits = Collisions.MASK_GROUNDSENSOR;
            sensorShape.Filter.GroupIndex = 0;

            SensorInfo si = new SensorInfo();
            si.sort = 1;

            Fixture sensorFixture = body.CreateFixture(sensorShape);
            sensorFixture.UserData = si;

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
            String tx = "whiteply.png";
            if (playerID == 0)
            {
                tx = "whiteply.png";
            }
            else if (playerID == 1)
            {
                tx = "redply.png";
            } if (playerID == 2)
            {
                tx = "greenply.png";
            } if (playerID == 3)
            {
                tx = "blueply.png";
            }

            return game.assets.CreateTexturedCube(tx, 1f);
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

            if(pos.Y < -10)
            {
                this.die();
            }

            // How fast to rotate
            float rotSpeed = 1/60f * 6;

            // Fancy Rotations
            Vec2 vel = this.body.GetLinearVelocity();
            if(vel.X < -0.1)
            {
                if(direction == 0)
                {
                    toRotate += (float)(System.Math.PI) / 2;
                }
                else if (direction == 1)
                {
                    toRotate += (float)(System.Math.PI);
                }

                direction = -1;
            }
            else if(vel.X > 0.1)
            {
                if (direction == 0)
                {
                    toRotate -= (float)(System.Math.PI) / 2;
                }
                else if (direction == -1)
                {
                    toRotate -= (float)(System.Math.PI);
                }

                direction = 1;
            }
            else
            {
                if (direction == -1)
                {
                    toRotate -= (float)(System.Math.PI) / 2;
                }
                else if (direction == 1)
                {
                    toRotate += (float) (System.Math.PI) / 2;
                }

                direction = 0;
            }

            if (toRotate > 0)
            {
                toRotate -= rotSpeed;
                rot += rotSpeed;

                if (toRotate < 0)
                {
                    rot += toRotate;
                    toRotate = 0;
                }
            }
            else if (toRotate < 0)
            {
                toRotate += rotSpeed;
                rot -= rotSpeed;

                if (toRotate > 0)
                {
                    rot += toRotate;
                    toRotate = 0;
                }
            }

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

            basicEffect.World = Matrix.Scaling(this.scaling) * Matrix.RotationY(this.rot) * Matrix.Translation(this.pos);
        }

        // React to getting hit by an enemy bullet.
        public void Hit()
        {
            game.Exit();
        }

        public override void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
            //fire();

            //this.attack(new Vec2(5, 10), 10);
        }

        public override void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            //pos.X += (float)args.Delta.Translation.X / 100;
        }

        // Applies a force at the center of the player
        public void ApplyForce(Vec2 force)
        {
            this.body.ApplyImpulse(force, this.body.GetPosition());
        }

        // Gets the playerID
        public int getPlayerID()
        {
            return this.playerID;
        }

        // Returns the amount of damage this player has
        public ushort getDamage()
        {
            return this.damage;
        }

        // Adds to the current damage
        public void addDamage(ushort damage)
        {
            // Increase damage
            this.damage += damage;

            // Update display
            game.updateDamage(this.playerID, getDamage(), getLives());
        }

        // Driectly sets the damage
        public void setDamage(ushort damage)
        {
            // Set damage
            this.damage = damage;

            // Update display
            game.updateDamage(this.playerID, getDamage(), getLives());
        }

        // Returns the damage multiplier
        public int getDamageMultiplier()
        {
            return (this.getDamage() + 100) / 100;
        }

        // Applys an attack to this player
        public void attack(Vec2 power, ushort damage)
        {
            // Add to the damage
            addDamage(damage);
            
            // Apply the force
            this.body.ApplyImpulse(power * getDamageMultiplier(), this.body.GetPosition());
        }

        // Sets our scale
        public void setScale(float scale)
        {
            this.scaling = scale;
        }

        // Creates an attack radius
        public void createAttack(float width, float height, Vec2 offset, float angle, int damage, float power)
        {
            removeAttack();

            PolygonDef sensorShape = new PolygonDef();
            sensorShape.SetAsBox(width, height, offset, angle);
            sensorShape.Density = 1.0f;
            sensorShape.IsSensor = true;

            SensorInfo si = new SensorInfo();
            si.sort = 2;
            si.value1 = damage;
            si.value2 = power;

            attackFixture = body.CreateFixture(sensorShape);
            attackFixture.UserData = si;
        }

        public void removeAttack()
        {
            if (attackFixture != null)
            {
                body.DestroyFixture(attackFixture);
                attackFixture = null;
            }
        }

        public void setLives(int lives)
        {
            this.lives = lives;

            game.updateDamage(this.playerID, getDamage(), getLives());
        }

        public int getLives()
        {
            return this.lives;
        }

        public Vec2 getPosition()
        {
            return this.body.GetPosition();
        }

        public void die()
        {
            this.setLives(this.getLives() - 1);

            if(this.getLives() <= 0)
            {
                // One less player
                game.playersLeft--;

                this.body.Dispose();
                game.gameObjects.Remove(this);

                // Check for victory
                game.checkVictory();
            }
            else
            {
                // Reset position
                this.body.SetPosition(new Vec2(0, 6));
                this.body.SetLinearVelocity(new Vec2(0, 0));

                // No damage
                this.setDamage(0);
            }
        }
    }
}