using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        Player player;
        GameState gameState;
        int score;
        float distance;
        Texture2D platformTexture, playerTexture;
        GameObject platformTile;

        public CavemanRunner()
        {
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
            player = new Player(this, Content.Load<Texture2D>("Graphics/caveman"), new Vector2(100, 100));
            platformTile = new GameObject(this, Content.Load<Texture2D>("Graphics/groundtile"), new Vector2(100, 100));
            Components.Add(player);
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
            foreach(GameObject o in Components)
            {
                o.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (GameObject o in Components)
            {
                o.Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
