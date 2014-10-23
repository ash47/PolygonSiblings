// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using SharpDX;
using SharpDX.Toolkit;
using System;
using System.Collections.Generic;
using Windows.UI.Input;
using Windows.UI.Core;
using Windows.Devices.Sensors;
using Box2DX.Dynamics;
using Box2DX.Collision;
using Box2DX.Common;
using NLua;
using System.Diagnostics;


namespace Project
{
    // Use this namespace here in case we need to use Direct3D11 namespace as well, as this
    // namespace will override the Direct3D11.
    using SharpDX.Toolkit.Graphics;
    using SharpDX.Toolkit.Input;

    public class LabGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        public List<GameObject> gameObjects;
        private Stack<GameObject> addedGameObjects;
        private Stack<GameObject> removedGameObjects;
        private KeyboardManager keyboardManager;
        public KeyboardState keyboardState;
        public AccelerometerReading accelerometerReading;
        public GameInput input;
        public int score;
        public MainPage mainPage;

        // Stores all the registered characters
        private List<CharacterClass> characters;

        // TASK 4: Use this to represent difficulty
        public float difficulty;

        // Represents the camera's position and orientation
        public Camera camera;

        // Graphics assets
        public Assets assets;

        // Random number generator
        public Random random;

        // Number of players left
        public int playersLeft;

        // Lua stuff
        Lua lua;

        // The physics world
        World world;

        // Should the camera rotate
        public Boolean shouldCamRot;

        // Physics settings
        private static int velocityIterations = 8;
        private static int positionIterations = 1;

        // The control types
        public int[] controlTypes;

        // Used for the onscreen controls
        public Boolean[] onscreenControls;

        public Controls controls;

        public bool started = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="LabGame" /> class.
        /// </summary>
        public LabGame(MainPage mainPage)
        {
            // Creates a graphics manager. This is mandatory.
            graphicsDeviceManager = new GraphicsDeviceManager(this);

            // Setup the relative directory to the executable directory
            // for loading contents with the ContentManager
            Content.RootDirectory = "Content";

            // Create the keyboard manager
            keyboardManager = new KeyboardManager(this);
            assets = new Assets(this);
            random = new Random();
            input = new GameInput();

            // Initialise event handling.
            input.gestureRecognizer.Tapped += Tapped;
            input.gestureRecognizer.ManipulationStarted += OnManipulationStarted;
            input.gestureRecognizer.ManipulationUpdated += OnManipulationUpdated;
            input.gestureRecognizer.ManipulationCompleted += OnManipulationCompleted;

            this.mainPage = mainPage;

            score = 0;
            difficulty = 1;
        }

        protected override void LoadContent()
        {
            // Initialise game object containers.
            gameObjects = new List<GameObject>();
            addedGameObjects = new Stack<GameObject>();
            removedGameObjects = new Stack<GameObject>();
            characters = new List<CharacterClass>();

            // Control types
            controlTypes = new int[4];
            onscreenControls = new Boolean[4];
            shouldCamRot = false;
            
            // Init controls
            this.controls = new Controls(this);

            // Setup Lua
            initLua();

            // Add a platform
            //gameObjects.Add(new Wall(this, 30, 1, new Vector3(0, 3, 0)));

            // Create an input layout from the vertices
            base.LoadContent();
        }

        protected override void Initialize()
        {
            Window.Title = "Polygon Siblings";
            camera = new Camera(this);

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (started)
            {

                // Instruct the world to perform a single step of simulation. It is
                // generally best to keep the time step and iterations fixed.
                world.Step(1 / 60f, velocityIterations, positionIterations);

                keyboardState = keyboardManager.GetState();
                flushAddedAndRemovedGameObjects();
                camera.Update(gameTime);

                // Update onscreen controls
                mainPage.updateControls();
    
                // Update Accelerometer
                if (input.accelerometer != null)
                {
                    accelerometerReading = input.accelerometer.GetCurrentReading();
                }

                // Update xbox controls
                controls.update();

                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].Update(gameTime);
                }

                if (keyboardState.IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                    this.Dispose();
                    App.Current.Exit();
                }
                // Handle base.Update
            }
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            if (started)
            {
                // Clears the screen with the Color.CornflowerBlue
                GraphicsDevice.Clear(SharpDX.Color.CornflowerBlue);

                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].Draw(gameTime);
                }
            }
            else
            {
                // Clears the screen with the Color.CornflowerBlue
                GraphicsDevice.Clear(SharpDX.Color.Black);
            }
            // Handle base.Draw
            base.Draw(gameTime);
        }
        // Count the number of game objects for a certain type.
        public int Count(GameObjectType type)
        {
            int count = 0;
            foreach (var obj in gameObjects)
            {
                if (obj.type == type) { count++; }
            }
            return count;
        }

        // Add a new game object.
        public void Add(GameObject obj)
        {
            if (!gameObjects.Contains(obj) && !addedGameObjects.Contains(obj))
            {
                addedGameObjects.Push(obj);
            }
        }

        // Remove a game object.
        public void Remove(GameObject obj)
        {
            if (gameObjects.Contains(obj) && !removedGameObjects.Contains(obj))
            {
                removedGameObjects.Push(obj);
            }
        }

        // Process the buffers of game objects that need to be added/removed.
        private void flushAddedAndRemovedGameObjects()
        {
            while (addedGameObjects.Count > 0) { gameObjects.Add(addedGameObjects.Pop()); }
            while (removedGameObjects.Count > 0) { gameObjects.Remove(removedGameObjects.Pop()); }
        }

        public void OnManipulationStarted(GestureRecognizer sender, ManipulationStartedEventArgs args)
        {
            // Pass Manipulation events to the game objects.

        }

        public void Tapped(GestureRecognizer sender, TappedEventArgs args)
        {
            // Pass Manipulation events to the game objects.
            foreach (var obj in gameObjects)
            {
                obj.Tapped(sender, args);
            }
        }

        public void OnManipulationUpdated(GestureRecognizer sender, ManipulationUpdatedEventArgs args)
        {
            cameraUpdated();
        }

        public void cameraUpdated()
        {
            //camera.pos.Z = camera.pos.Z * args.Delta.Scale;
            // Update camera position for all game objects
            foreach (var obj in gameObjects)
            {
                if (obj.basicEffect != null) { obj.basicEffect.View = camera.View; }
                //obj.OnManipulationUpdated(sender, args);
            }
        }

        public void OnManipulationCompleted(GestureRecognizer sender, ManipulationCompletedEventArgs args)
        {
        }

        // Sets up Lua
        private void initLua()
        {
            // Setup Lua
            lua = new Lua();

            // Give lua full access to our classes
            lua.LoadCLRPackage();

            // Copy some variables into Lua
            lua["game"] = this;
            lua["util"] = new LuaGlobals();
            lua["controls"] = this.controls;

            // Run the setup
            lua.DoFile("Content/lua/init.lua");
        }

        public void callLuaFunction(String name, object[] args)
        {
            // Tell lua to update
            lua.GetFunction(name).Call(args);
        }

        // Registers a character
        public void registerCharacter(String name, LuaFunction init, LuaFunction update)
        {
            // Create the class
            CharacterClass cl = new CharacterClass();
            cl.name = name;
            cl.update = update;
            cl.init = init;

            // Store it
            characters.Add(cl);
        }

        // Gets a character by index
        public CharacterClass getCharacterClass(int index)
        {
            // Ensure a valid index
            if (index < 0 || index >= characters.Count) return null;

            // Return the correct character
            return characters[index];
        }

        // Returns the world
        public World getWorld()
        {
            return this.world;
        }

        // Updates the damage for the given player
        public void updateDamage(int playerID, ushort damage, int lives)
        {
            mainPage.UpdateDamage(playerID, damage, lives);
        }

        public void startMatch()
        {
            // Cleanup old world
            if(world != null)
            {
                world.Dispose();
            }

            // Create the physics world
            AABB worldAABB = new AABB();
            worldAABB.LowerBound.Set(-100.0f);
            worldAABB.UpperBound.Set(100.0f);
            world = new World(worldAABB, new Vec2(0.0f, -10.0f), true);

            // Setup collision handler
            world.SetContactListener(new Collisions(this));

            // Reset number of players left
            playersLeft = 0;

            // Create a players
            for (int i = 0; i < 4; i++)
            {
                if (controlTypes[i] < 3)
                {
                    Player player = new Player(this, i, 0, new Vec2(-4 + 2 * i, 0));
                    gameObjects.Add(player);
                }
            }

            // Create the level
            Wall wall = new FinalDestination(this, new Vector3(0, -4, 0));
            gameObjects.Add(wall);
        }

        public void setControls(int p1, int p2, int p3, int p4)
        {
            this.controlTypes[0] = p1;
            this.controlTypes[1] = p2;
            this.controlTypes[2] = p3;
            this.controlTypes[3] = p4;
        }

        public Boolean isAI(int playerID)
        {
            return this.controlTypes[playerID] == 0;
        }

        public float getDistance(Player ply1, Player ply2)
        {
            return (ply1.getPosition() - ply2.getPosition()).Length();
        }

        public Player findClosePlayer(Player ply)
        {
            float dist = 999;

            Player returnPly = null;

            // Find a closer player
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] is Player)
                {
                    Player otherPly = (Player)gameObjects[i];

                    // Ensure it isn't themselves
                    if (ply != otherPly)
                    {
                        float theirDist = getDistance(ply, otherPly);

                        // Check distance
                        if(theirDist < dist)
                        {
                            // We have a new winner!
                            dist = theirDist;
                            returnPly = otherPly;
                        }
                    }
                }
            }

            return returnPly;
        }

        // Returns a random number
        public float getRandom()
        {
            return random.NextFloat(0f, 1f);
        }

        // Checks for victory
        public void checkVictory()
        {
            if(playersLeft <= 1)
            {
                // Find the winner
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i] is Player)
                    {
                        Player ply = (Player) gameObjects[i];

                        // Finish the game
                        mainPage.StopGame(ply.getPlayerID());
                    }
                }
            }
        }
    }
}
