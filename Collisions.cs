﻿using System;
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

        // Collision Categories
        public readonly static ushort CAT_WALL = 0x0001;
        public readonly static ushort CAT_PLAYER = 0x0002;
        public readonly static ushort CAT_ITEM = 0x0004;
        public readonly static ushort CAT_PHYSOBJECT = 0x0008;
        public readonly static ushort CAT_GROUNDSENSOR = 0x0010;

        // Collision Masks
        public readonly static ushort MASK_WALL = 0xFFFF;
        public readonly static ushort MASK_PLAYER = 0x001;//(ushort) (CAT_WALL | CAT_PHYSOBJECT);
        public readonly static ushort MASK_ITEM = CAT_WALL;
        public readonly static ushort MASK_PHYSOBJECT = (ushort)(CAT_WALL | CAT_PHYSOBJECT | CAT_PLAYER);
        public readonly static ushort MASK_GROUNDSENSOR = CAT_WALL;

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
                    SensorInfo si = (SensorInfo)contact.FixtureA.UserData;
                    if(si.sort == 1)
                    {
                        if(!contact.FixtureB.IsSensor)
                        {
                            // Grab the player
                            Player ply = (Player)a;

                            // Increase the number of grounds they are touching
                            ply.touchingGround++;
                        }
                    }
                    else if(si.sort == 2)
                    {
                        if(b is Player && b != a)
                        {
                            // Grab the player
                            Player ply = (Player)b;

                            // Add damage
                            //ply.addDamage((ushort)si.value1);

                            Vec2 ang = contact.FixtureB.Body.GetPosition() - contact.FixtureA.Body.GetPosition();
                            ang.Normalize();
                            ang *= si.value2;

                            ply.attack(ang, (ushort)si.value1);
                        }
                    }
                }
                else
                {
                    if (b is Player)
                    {
                        if(!contact.FixtureB.IsSensor)
                        {
                            float vela = contact.FixtureA.Body.GetLinearVelocity().Length();
                            float velb = contact.FixtureB.Body.GetLinearVelocity().Length();

                            float diff = (float) (System.Math.Abs(vela - velb));

                            // If one is travelling much faster
                            if(diff > 5)
                            {
                                ushort damage = (ushort) (diff * 3);

                                if(vela > velb)
                                {
                                    // Add the damage
                                    ((Player)(b)).addDamage(damage);
                                }
                                else
                                {
                                    // Add the damage
                                    ((Player)(a)).addDamage(damage);
                                }
                            }
                        }
                    }
                }
            }

            if (b is Player)
            {
                if (contact.FixtureB.IsSensor)
                {
                    SensorInfo si = (SensorInfo)contact.FixtureB.UserData;
                    if (si.sort == 1)
                    {
                        if (!contact.FixtureA.IsSensor)
                        {
                            // Grab the player
                            Player ply = (Player)b;

                            // Increase the number of grounds they are touching
                            ply.touchingGround++;
                        }
                        
                    }
                    else if (si.sort == 2)
                    {
                        if (a is Player && b != a)
                        {
                            // Grab the player
                            Player ply = (Player)a;

                            // Add damage
                            //ply.addDamage((ushort)si.value1);

                            Vec2 ang = contact.FixtureA.Body.GetPosition() - contact.FixtureB.Body.GetPosition();
                            ang.Normalize();
                            ang *= si.value2;

                            ply.attack(ang, (ushort)si.value1);
                        }
                    }
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
                    SensorInfo si = (SensorInfo)contact.FixtureA.UserData;
                    if (si.sort == 1)
                    {
                        if (!contact.FixtureB.IsSensor)
                        {
                            // Grab the player
                            Player ply = (Player)a;

                            // Increase the number of grounds they are touching
                            ply.touchingGround--;
                        }
                    }
                }
            }

            if (b is Player)
            {
                if (contact.FixtureB.IsSensor)
                {
                    SensorInfo si = (SensorInfo)contact.FixtureB.UserData;
                    if (si.sort == 1)
                    {
                        if (!contact.FixtureA.IsSensor)
                        {
                            // Grab the player
                            Player ply = (Player)b;

                            // Increase the number of grounds they are touching
                            ply.touchingGround--;
                        }
                    }
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
