using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CavemanRunner
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CavemanRunner : Game
    {
        public float scaleToReference;

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

        DrumSide.side previousDrumSide;
        Drum leftDrum, rightDrum;
        SoundEffect click, bongo1, bongo2;
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public TouchCollection touches;
        public bool jumpDoubleTap;
        Player player;
        GameState gameState;
        public int tempo = 60, tolerance = 100;
        int[] clickTimes = { 0, 0, 0, 0 };
        float distance;
        bool hit = false;

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
            scaleToReference = (float)GraphicsDevice.Viewport.Width / 800f;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player = new Player();
            player.Initialize(this, Content.Load<Texture2D>("Graphics/caveman"),
                Vector2.Zero, 100);
            player.renderer.SetAnchorPoint(Renderer.AnchorPoint.BottomMiddle);
            player.collider.SetAnchorPoint(Renderer.AnchorPoint.BottomMiddle);

            leftDrum = new Drum();
            leftDrum.Initialize(this, Content.Load<Texture2D>("Graphics/halfscreen"), Vector2.Zero, 100, true);
            leftDrum.drumSide = DrumSide.side.LEFT;
            leftDrum.transform.Position = Vector2.Zero; // new Vector2(GraphicsDevice.Viewport.Width / 4, GraphicsDevice.Viewport.Height / 4 * 3);
            leftDrum.renderer.SetAnchorPoint(Renderer.AnchorPoint.TopLeft);
            leftDrum.collider.SetAnchorPoint(Renderer.AnchorPoint.TopLeft);
            
            rightDrum = new Drum();
            rightDrum.Initialize(this, Content.Load<Texture2D>("Graphics/halfscreen"), Vector2.Zero, 100, true);
            rightDrum.drumSide = DrumSide.side.RIGHT;
            rightDrum.transform.Position = new Vector2(GraphicsDevice.Viewport.Width / 2, 0f);
            rightDrum.renderer.SetAnchorPoint(Renderer.AnchorPoint.TopLeft);
            rightDrum.collider.SetAnchorPoint(Renderer.AnchorPoint.TopLeft);

            platformPool.InitializeObjects(this, Content.Load<Texture2D>("Graphics/groundtile640"), new Vector2(-1, 0), 1, true);
            platformPool.ActivateNewObject().transform.Position = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height
                - platformPool.Objects[0].renderer.Texture.Height);

            foreach(Platform platform in platformPool.Objects)
            {
                platform.renderer.SetAnchorPoint(Renderer.AnchorPoint.TopLeft);
                platform.collider.SetAnchorPoint(Renderer.AnchorPoint.TopLeft);
            }
                

            click = Content.Load<SoundEffect>("Sounds/click");
            bongo1 = Content.Load<SoundEffect>("Sounds/bongo1");
            bongo2 = Content.Load<SoundEffect>("Sounds/bongo2");
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
            // check collision against platforms
            foreach (Platform platform in platformPool.Objects)
            {
                if (player.collider.CheckCollisions(platform.collider)
                    && player.physics.Velocity.Y > 0 && player.transform.Position.Y < platform.transform.Position.Y)
                {

                    player.SetGrounded();
                }
            }

            int millis = (int)Math.Round(gameTime.TotalGameTime.TotalMilliseconds);

            if (gameTime.TotalGameTime.TotalSeconds % 10 == 0)
            {
                tempo += Convert.ToInt32(gameTime.TotalGameTime.TotalSeconds);
            }

            if (millis % (60 * 1000 / tempo) == 0)
            {
                clickTimes[0] = Convert.ToInt32(millis);
                clickTimes[1] = clickTimes[0] + 60 * 1000 / tempo / 4;
                clickTimes[2] = clickTimes[1] + 60 * 1000 / tempo / 4;
                clickTimes[3] = clickTimes[2] + 60 * 1000 / tempo / 4;
                click.Play();
            }

            // jump on two finger tap
            touches = TouchPanel.GetState();
            if(touches.Count == 1 && touches[0].State == TouchLocationState.Pressed)
            {
                if (CheckDrumHit(touches, leftDrum))
                {
                    bongo1.Play();
                    if (CheckDrumTiming(leftDrum, gameTime))
                    {
                        previousDrumSide = leftDrum.drumSide;
                        //player.physics.AddForce(Vector2.UnitX * 2000);
                    }
                }
                else if (CheckDrumHit(touches, rightDrum))
                {
                    bongo2.Play();
                    if (CheckDrumTiming(rightDrum, gameTime))
                    {
                        previousDrumSide = rightDrum.drumSide;
                        //player.physics.AddForce(Vector2.UnitX * 2000);
                    }
                }
            }
            else if (touches.Count == 2 && CheckDrumHit(touches, leftDrum, rightDrum)
                && (touches[0].State == TouchLocationState.Pressed || touches[0].State == TouchLocationState.Moved)
                && (touches[1].State == TouchLocationState.Pressed || touches[1].State == TouchLocationState.Moved))
            {
                player.Jump();
                jumpDoubleTap = true;
            }
            else
            {
                jumpDoubleTap = false;
            }

            foreach(GameObject go in platformPool.Objects)
            {
                go.Update(gameTime);
                if(go.transform.Position.X < 0 - go.renderer.Texture.Width)
                {
                    platformPool.ReleaseObject((Platform)go);
                    platformPool.ActivateNewObject().transform.Position = new Vector2(GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height - platformPool.Objects[0].renderer.Texture.Height);
                    return;
                }
                go.physics.Velocity = -Vector2.UnitX * 160 / tempo;
            }

            leftDrum.Update(gameTime);
            rightDrum.Update(gameTime);
            player.Update(gameTime);
            base.Update(gameTime);
        }

        public void PlayBothBongos()
        {
            bongo1.Play();
            bongo2.Play();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (touches.Count == 1)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts/font"), touches[0].State.ToString(), Vector2.UnitY * 20, Color.Black);
                spriteBatch.End();
            }
            else if (touches.Count == 2)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts/font"), touches[0].State.ToString(), Vector2.UnitY * 20, Color.Black);
                spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts/font"), touches[0].State.ToString(), Vector2.UnitY * 40, Color.Black);
                spriteBatch.End();
            }

            if(hit)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts/font"), "HIT", Vector2.Zero, Color.Black);
                spriteBatch.End();
            }

            spriteBatch.Begin();
            spriteBatch.DrawString(Content.Load<SpriteFont>("Fonts/font"), player.transform.Position.ToString(),
                    new Vector2(0, 100), Color.Black);
            spriteBatch.End();

            player.Draw(gameTime);
            leftDrum.Draw(gameTime);
            rightDrum.Draw(gameTime);

            foreach (GameObject go in platformPool.Objects)
            {
                go.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        private bool CheckDrumTiming(Drum drum, GameTime gameTime)
        {
            hit = false;
            if (drum.drumSide != previousDrumSide)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds <= clickTimes[0] + tolerance ||
                    gameTime.TotalGameTime.TotalMilliseconds >= clickTimes[3] - tolerance ||
                    (gameTime.TotalGameTime.TotalMilliseconds >= clickTimes[1] - tolerance &&
                    gameTime.TotalGameTime.TotalMilliseconds <= clickTimes[1] + tolerance) ||
                    (gameTime.TotalGameTime.TotalMilliseconds >= clickTimes[2] - tolerance &&
                    gameTime.TotalGameTime.TotalMilliseconds <= clickTimes[2] + tolerance))
                {
                    hit = true;
                }
            }

            return hit;
        }

        private bool CheckDrumHit(TouchCollection touches, params Drum[] drums)
        {
            bool result = false;
            foreach (TouchLocation location in touches)
            {
                foreach (Drum drum in drums)
                {
                    if (location.Position.X > drum.collider.Bounds.Left && location.Position.X < drum.collider.Bounds.Right &&
                        location.Position.Y > drum.collider.Bounds.Top && location.Position.Y < drum.collider.Bounds.Bottom)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
    }
}
