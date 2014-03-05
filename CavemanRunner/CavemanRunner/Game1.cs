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
        GameComponentCollection scrollables;
        Player player;
        GameState gameState;
        int score;
        float distance;
        Texture2D platformTexture;
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
            player = new Player(this, Content.Load<Texture2D>("/Graphics/caveman"), new Vector2(0, 0));
            scrollables = new GameComponentCollection();
            platformTile = new GameObject(this, Content.Load<Texture2D>("/Graphics/groundtile"), new Vector2(100, 100));

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
            platformTexture = Content.Load<Texture2D>("\\Graphics\\groundtile");
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
            Components.GetEnumerator().Reset();
            while(Components.GetEnumerator().MoveNext())
            {
                ((GameObject)Components.GetEnumerator().Current).Update(gameTime);
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

            Components.GetEnumerator().Reset();
            while (Components.GetEnumerator().MoveNext())
            {
                ((GameObject)Components.GetEnumerator().Current).Draw(gameTime);
            }

            base.Draw(gameTime);
        }
    }
}
