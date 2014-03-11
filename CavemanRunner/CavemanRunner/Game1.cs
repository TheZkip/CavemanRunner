using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections;
using System.Collections.Generic;

namespace CavemanRunner
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CavemanRunner : Game
    {
        public enum GameState
        {
            Menu = 0,
            InGame,
            Paused,
            EndRun,
            PreRun
        }

        public enum CollisionID
        {
            Player,
            Platform,
            HeathCollectible,
            ScoreCollectible,
            Obstacle
        }

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public TouchCollection touches;
        public bool jumpDoubleTap;
        Player player;
        GameState gameState;
        int score;
        public int tempo = 20;
        float distance;

        Pool<Platform> platformPool;

        public CavemanRunner()
        {
            platformPool = new Pool<Platform>(10);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
<<<<<<< HEAD
            player = new Player(this, Content.Load<Texture2D>("Graphics/caveman"), new Vector2(100, 100), new Vector2(0, 0), 10);
            platformTile = new Platform(this, Content.Load<Texture2D>("Graphics/groundtile"),
                new Vector2(GraphicsDevice.Viewport.Width - 200, GraphicsDevice.Viewport.Height - 200), 100);
=======
            player = new Player();
            player.Initialize(this, Content.Load<Texture2D>("Graphics/caveman"),
                new Vector2(100, 100), 100);
            platformPool.InitializeObjects(this, Content.Load<Texture2D>("Graphics/groundtile"));
            platformPool.ActivateNewObject(new Vector2(GraphicsDevice.Viewport.Width, 100));
>>>>>>> origin/juhvo's-implementations
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
<<<<<<< HEAD
            // jump on two finger tap
            touches = TouchPanel.GetState();
            if (touches.Count == 2)
=======
            foreach(Player o in Components)
>>>>>>> origin/juhvo's-implementations
            {
                if (jumpDoubleTap == false)
                {
                    player.Jump();
                }

                jumpDoubleTap = true;
            }
<<<<<<< HEAD
            else
            {
                jumpDoubleTap = false;
            }

            foreach(GameObject o in Components)
            {
                o.Update(gameTime);
            }

=======
            foreach(GameObject go in platformPool.Objects)
            {
                go.Update(gameTime);
            }
>>>>>>> origin/juhvo's-implementations
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            foreach (GameObject go in platformPool.Objects)
            {
                go.Draw(gameTime);
            }
            base.Draw(gameTime);
        }
    }
}
