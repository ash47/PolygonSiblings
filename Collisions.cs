using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DX.Dynamics;
using Box2DX.Collision;
using Box2DX.Common;
using System.Diagnostics;

namespace Project
{
    public class Collisions : ContactListener
    {
        // The game to report back to
        private LabGame game;

        public Collisions(LabGame game)
        {
            // Store the game
            this.game = game;
        }

        public void BeginContact(Contact contact)
        {
            // Grab things that are colliding
            Object a = contact.FixtureA.Body.GetUserData();
            Object b = contact.FixtureB.Body.GetUserData();

            // Player ground collisions
            if (a is Player)
            {
                if (contact.FixtureA.IsSensor)
                {
                    // Grab the player
                    Player ply = (Player)a;

                    // Increase the number of grounds they are touching
                    ply.touchingGround++;
                }
            }

            if (a is Player)
            {
                if (contact.FixtureB.IsSensor)
                {
                    // Grab the player
                    Player ply = (Player)b;

                    // Increase the number of grounds they are touching
                    ply.touchingGround++;
                }
            }
        }

        public void EndContact(Contact contact)
        {
            // Grab things that are colliding
            Object a = contact.FixtureA.Body.GetUserData();
            Object b = contact.FixtureB.Body.GetUserData();

            // Player ground collisions
            if (a is Player)
            {
                if (contact.FixtureA.IsSensor)
                {
                    // Grab the player
                    Player ply = (Player)a;

                    // Increase the number of grounds they are touching
                    ply.touchingGround--;
                }
            }

            if (a is Player)
            {
                if (contact.FixtureB.IsSensor)
                {
                    // Grab the player
                    Player ply = (Player)b;

                    // Increase the number of grounds they are touching
                    ply.touchingGround--;
                }
            }
        }

        public void PreSolve(Contact contact, Manifold oldManifold)
        {

        }

        public void PostSolve(Contact contact, ContactImpulse impulse)
        {

        }
    }
}
