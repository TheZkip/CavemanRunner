using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CavemanRunner
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CavemanRunner : Game
    {
        public delegate void EndGameHandler(int score);
        public event EndGameHandler EndGameEvent;

        public static float toleranceAtStartingTempo = 80;

        public float scaleToReference;
        public SpriteFont font;
        public Texture2D halfScreen;

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
        int patternIterator = 0; // needs to be implemented in better manner 
        List<List<int[]>> patterns = new List<List<int[]>>();
        List<int[]> pattern = new List<int[]>();

        Texture2D background;
        ParallaxingBackground treesBack = new ParallaxingBackground();
        ParallaxingBackground treesFront = new ParallaxingBackground();
        DrumSide.side previousDrumSide;
        Drum leftDrum, rightDrum;
        SoundEffect click, bongo1, bongo2;
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public TouchCollection touches;
        public bool jumpDoubleTap;
        Player player;
        GameObject dino;
        GameState gameState = GameState.InGame;
        public int currentRunningSpeed, startingRunningSpeed = 200, maxRunningSpeed = 280, currentTolerance = 200, successCounter = 0;
        int[] clickTimes = { 0, 0, 0, 0 };
        float distance = 0f, timeSinceLastHit = 0f;
        bool hit = false, newPlatforms = false;

        // pools
        Pool<Platform> platformPool;
        List<Platform> removePlatforms;

        Pool<HealthCollectible> healthCollectiblePool;
        List<HealthCollectible> removeHealthCollectibles;

        Pool<Obstacle> obstaclePool;
        List<Obstacle> removeObstacles;

        Pool<GameObject> clubEffectPool;
        List<GameObject> removeClubEffects;

        public CavemanRunner()
        {
            currentRunningSpeed = startingRunningSpeed;
            clickTimes[0] = 0 * 60 * 1000 / currentRunningSpeed;
            clickTimes[1] = 1 * 60 * 1000 / currentRunningSpeed;
            clickTimes[2] = 2 * 60 * 1000 / currentRunningSpeed;
            clickTimes[3] = 3 * 60 * 1000 / currentRunningSpeed;

            platformPool = new Pool<Platform>(40);
            healthCollectiblePool = new Pool<HealthCollectible>(10);
            obstaclePool = new Pool<Obstacle>(20);
            clubEffectPool = new Pool<GameObject>(3);
            removePlatforms = new List<Platform>();
            removeObstacles = new List<Obstacle>();
            removeHealthCollectibles = new List<HealthCollectible>();
            removeClubEffects = new List<GameObject>();

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
            font = Content.Load<SpriteFont>("Fonts/font");
            scaleToReference = (float)GraphicsDevice.Viewport.Width / 800f;
            halfScreen = Content.Load<Texture2D>("Graphics/halfscreen");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player = new Player();
            player.Initialize(this, Content.Load<Texture2D>("Graphics/caveman"),
                Vector2.Zero, 100, false, Renderer.AnchorPoint.BottomMiddle);
            player.collider.SetSize(player.renderer.Texture.Width / 5, (int)(player.renderer.Texture.Height * 0.8f));
            player.renderer.AddAnimation("running", player.renderer.Texture, 75, 75, 4, startingRunningSpeed * 2, Color.White,
                player.transform.Scale.X, true, true, true);
            player.transform.Position = new Vector2(graphics.GraphicsDevice.Viewport.Width/4 * scaleToReference, 200 * scaleToReference);
            player.renderer.SetAnchorPoint(Renderer.AnchorPoint.BottomMiddle);

            dino = new GameObject();
            dino.Initialize(this, Content.Load<Texture2D>("Graphics/trex"),
                Vector2.UnitX, 1000, false, Renderer.AnchorPoint.BottomMiddle);
            dino.collider.SetSize(dino.renderer.Texture.Width / 4, dino.renderer.Texture.Height);
            dino.renderer.AddAnimation("running", dino.renderer.Texture, 436, 300, 4, startingRunningSpeed * 2, Color.White,
                dino.transform.Scale.X, true, true, true);
            dino.transform.Position = new Vector2(-dino.collider.Bounds.Width / 2, Platform.bottom * GraphicsDevice.Viewport.Height);
            dino.renderer.SetAnchorPoint(Renderer.AnchorPoint.BottomMiddle);

            leftDrum = new Drum();
            leftDrum.Initialize(this, halfScreen, Vector2.Zero, 100, true, Renderer.AnchorPoint.TopLeft);
            leftDrum.transform.Scale = Vector2.One * 2;
            leftDrum.collider.SetSize(leftDrum.renderer.Texture.Width, leftDrum.renderer.Texture.Height);
            leftDrum.drumSide = DrumSide.side.LEFT;
            leftDrum.transform.Position = Vector2.Zero;
            
            rightDrum = new Drum();
            rightDrum.Initialize(this, halfScreen, Vector2.Zero, 100, true, Renderer.AnchorPoint.TopLeft);
            rightDrum.transform.Scale = Vector2.One * 2;
            rightDrum.collider.SetSize(rightDrum.renderer.Texture.Width * 2, rightDrum.renderer.Texture.Height * 2);
            rightDrum.drumSide = DrumSide.side.RIGHT;
            rightDrum.transform.Position = new Vector2(GraphicsDevice.Viewport.Width / 2, 0f);

            // init pools for platorms, "club" collectibles and obstacles
            platformPool.InitializeObjects(this, Content.Load<Texture2D>("Graphics/grass_fourth"), new Vector2(-1, 0), 1, true, Renderer.AnchorPoint.TopLeft);

            for (int i = 0; i < 10; i++)
            {
                platformPool.ActivateNewObject().transform.Position = new Vector2(platformPool.Objects[0].collider.Bounds.Width * i, GraphicsDevice.Viewport.Height
                    * Platform.bottom);
                
            }

            healthCollectiblePool.InitializeObjects(this, Content.Load<Texture2D>("Graphics/scoreCollectible"), Vector2.Zero, 1, true, Renderer.AnchorPoint.BottomMiddle);
            obstaclePool.InitializeObjects(this, Content.Load<Texture2D>("Graphics/obstacle"), Vector2.Zero, 1, true, Renderer.AnchorPoint.BottomMiddle, 0.8f);
            //clubEffectPool.InitializeObjects(this, Content.Load<Texture2D>("Graphics/clubEffect"), Vector2.Zero, 1, true, Renderer.AnchorPoint.BottomMiddle);

            // background texture and parallaxers
            background = Content.Load<Texture2D>("Graphics/background");
            treesBack.Initialize(Content, "Graphics/trees_dark", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 10);
            treesFront.Initialize(Content, "Graphics/trees_light", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 20);
            
            // sound effects
            click = Content.Load<SoundEffect>("Sounds/click");
            bongo1 = Content.Load<SoundEffect>("Sounds/bongo1");
            bongo2 = Content.Load<SoundEffect>("Sounds/bongo2");
        }

        public void GenerateLevelFromString(string level)
        {
            StringReader reader = new StringReader(level);
            List<int[]> tempList = new List<int[]>();
            int[] row = new int[5];
            int i = 0;
            while(reader.Peek() >= 0)
            {
                char temp = (char)reader.Read();
                if(Char.IsDigit(temp))
                {
                    row[i] = Convert.ToInt32(Char.GetNumericValue(temp));
                    i++;
                    Console.WriteLine(row);
                }
                else if(temp == '\n')
                {
                    tempList.Add(row);
                    row = new int[5];
                    i = 0;
                }
            }
            patterns.Add(tempList);
            pattern = patterns[0];
        }

        void ChoosePatternByRandom()
        {
            Random r = new Random();
            int rnd = r.Next(0, patterns.Count);
            pattern = patterns[rnd];
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
            // bail out if we are not in game
            if (gameState != GameState.InGame)
                return;

            //// recalculate touch tolerance
            //currentTolerance = (int)((toleranceAtStartingTempo) * ((float)startingTempo / (float)currentTempo));

            // update background
            treesBack.Update(gameTime);
            treesFront.Update(gameTime);

            CheckPlatformCollision();
            CheckObstacleCollision();
            CheckCollectibleCollision();

            //int millis = (int)Math.Round(gameTime.TotalGameTime.TotalMilliseconds);

            //if (clickTimes[3] < millis)
            //{
            //    clickTimes[0] = Convert.ToInt32(millis);
            //    clickTimes[1] = clickTimes[0] + 60 * 1000 / currentTempo / 4;
            //    clickTimes[2] = clickTimes[1] + 60 * 1000 / currentTempo / 4;
            //    clickTimes[3] = clickTimes[2] + 60 * 1000 / currentTempo / 4;
            //    click.Play();
            //    dino.physics.Velocity += Vector2.UnitX * 0.015f;
            //}

            CheckTapInput(gameTime);

            RemoveObjectsOutOfView(gameTime);

            leftDrum.Update(gameTime);
            rightDrum.Update(gameTime);
            player.Update(gameTime);

            // "roof"
            if (player.transform.Position.Y < player.renderer.Texture.Height)
                player.physics.Velocity *= -0.1f;

            dino.physics.Velocity = Vector2.UnitX * ((maxRunningSpeed - currentRunningSpeed) / (float)maxRunningSpeed);
            dino.Update(gameTime);
            if (dino.transform.Position.X < -dino.collider.Bounds.Width / 2)
            {
                dino.transform.Position = new Vector2(-dino.collider.Bounds.Width / 2, Platform.bottom * GraphicsDevice.Viewport.Height);
                //dino.physics.Velocity = Vector2.Zero;
            }

            //float dinoSpeedHelper = 0;
            //dinoSpeedHelper = Math.Max(dino.physics.Velocity.X, -0.5f);
            //dinoSpeedHelper = Math.Min(dino.physics.Velocity.X, 0.5f);
            //dino.physics.Velocity = Vector2.UnitX * dinoSpeedHelper;

            base.Update(gameTime);

            CheckDinoCollision();
        }

        void SpawnNewPlatforms()
        {
            Vector2 newPosition = new Vector2();
            newPosition.X = platformPool.Objects[platformPool.Objects.Count - 1].transform.Position.X
                        + platformPool.Objects[platformPool.Objects.Count - 1].collider.Bounds.Width;

            for (int i = 0; i < pattern[patternIterator].Length; i++)
            {
                if (pattern[patternIterator][i] > 0)
                {
                    // create the platform
                    if(i == 0)
                        newPosition.Y = GraphicsDevice.Viewport.Height * Platform.bottom;
                    else if(i == 1)
                        newPosition.Y = GraphicsDevice.Viewport.Height * Platform.bottomMiddle;
                    else if(i == 2)
                        newPosition.Y = GraphicsDevice.Viewport.Height * Platform.middle;
                    else if (i == 3)
                        newPosition.Y = GraphicsDevice.Viewport.Height * Platform.topMiddle;
                    else if (i == 4)
                        newPosition.Y = GraphicsDevice.Viewport.Height * Platform.top;

                    GameObject tempPlatform = platformPool.ActivateNewObject();
                    
                    tempPlatform.transform.Position = newPosition;

                    // create an obstacle if pattern[patternIterator][i] == 2
                    if (pattern[patternIterator][i] == 2)
                    {
                        Obstacle tempObstacle = obstaclePool.ActivateNewObject() as Obstacle;
                        tempObstacle.transform.Parent = tempPlatform.transform;
                        tempObstacle.transform.LocalPosition = new Vector2(tempPlatform.renderer.Texture.Width / 2, 5);
                    }

                    // create a collectible if 3
                    else if (pattern[patternIterator][i] == 3)
                    {
                        HealthCollectible tempHealth = healthCollectiblePool.ActivateNewObject() as HealthCollectible;
                        tempHealth.transform.Parent = tempPlatform.transform;
                        tempHealth.transform.LocalPosition = new Vector2(tempPlatform.renderer.Texture.Width / 2, 5);
                    }
                }
            }

            patternIterator++;

            if(patternIterator == pattern.Count)
            {
                ChoosePatternByRandom();
                patternIterator = 0;
            }
        }

        void CheckDinoCollision ()
        {
            if (dino.transform.Position.X >= player.transform.Position.X - player.collider.Bounds.Width * 2)
            {
                dino.transform.Position = new Vector2(-dino.collider.Bounds.Width / 2, Platform.bottom * GraphicsDevice.Viewport.Height);
                //dino.physics.Velocity = Vector2.Zero;
            }
        }

        void CheckPlatformCollision ()
        {
            // check collision against platforms
            for (int i = 0; i < platformPool.Objects.Count; i++)
            {
                if (player.collider.CheckCollisions(platformPool.Objects[i].collider))
                {
                    if (player.physics.Velocity.Y >= 0
                        && player.transform.Position.Y <= platformPool.Objects[i].transform.Position.Y + player.physics.Velocity.Y + 1)
                    {
                        player.transform.Position = new Vector2(player.transform.Position.X,
                            platformPool.Objects[i].transform.Position.Y + 1f);
                        player.SetGrounded(true);
                        break;
                    }
                }
                else
                {
                    player.SetGrounded(false);
                }
            }
        }

        void CheckObstacleCollision ()
        {
            // check collision against Obstacles
            for (int i = 0; i < obstaclePool.Objects.Count; i++)
            {
                if (player.collider.CheckCollisions(obstaclePool.Objects[i].collider))
                {
                    //if (player.HasClub)
                    //{
                        //player.RemoveClub();
                    currentRunningSpeed -= 10;
                    //clubEffectPool.ActivateNewObject().transform.Parent = obstaclePool.Objects[i].transform.Parent;
                    obstaclePool.ReleaseObject(obstaclePool.Objects[i]);
                        // TODO: play sound effect, visual effect etc
                    //}
                    //else
                    //{
                    //    foreach (Platform p in platformPool.Objects)
                    //        p.physics.Velocity = Vector2.Zero;
                    //    // end game
                    //}
                }
            }
        }

        void CheckCollectibleCollision ()
        {
            // check collision against collectibles
            for (int i = 0; i < healthCollectiblePool.Objects.Count; i++)
            {
                if (player.collider.CheckCollisions(healthCollectiblePool.Objects[i].collider))
                {
                    (healthCollectiblePool.Objects[i] as HealthCollectible).Collect();
                    healthCollectiblePool.ReleaseObject(healthCollectiblePool.Objects[i]);
                    //player.AddClub();
                    // TODO: play sound effect, visual effect etc
                }
            }
        }

        void RemoveObjectsOutOfView (GameTime gameTime)
        {
            // remove platforms that are out of view
            foreach (GameObject go in platformPool.Objects)
            {
                go.Update(gameTime);

                if (go.transform.Position.X < 0 - go.renderer.Texture.Width)
                    removePlatforms.Add((Platform)go);

                go.physics.Velocity = Vector2.UnitX * ((-1 * GraphicsDevice.Viewport.Width) / (8 * 60f / (float)currentRunningSpeed))
                    * gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            }

            CreateNewPlatforms();

            foreach (Platform p in removePlatforms)
                platformPool.ReleaseObject(p);
            
            removePlatforms.Clear();

            // remove obstacles that are out of view
            foreach (GameObject go in obstaclePool.Objects)
            {
                go.Update(gameTime);

                if (go.transform.Position.X < 0 - go.renderer.Texture.Width)
                {
                    removeObstacles.Add((Obstacle)go);
                    go.transform.Parent = null;
                }
            }

            foreach (Obstacle o in removeObstacles)
                obstaclePool.ReleaseObject(o);

            removeObstacles.Clear();

            // remove collectibles that are out of view
            foreach (GameObject go in healthCollectiblePool.Objects)
            {
                go.Update(gameTime);
                
                if (go.transform.Position.X < 0 - go.renderer.Texture.Width)
                {
                    removeHealthCollectibles.Add((HealthCollectible)go);
                    go.transform.Parent = null;
                }
            }

            foreach (HealthCollectible h in removeHealthCollectibles)
                healthCollectiblePool.ReleaseObject(h);

            removeHealthCollectibles.Clear();
        }

        void CreateNewPlatforms ()
        {
            // make new platforms
            if (platformPool.Objects[0].transform.Position.X < 0
                - platformPool.Objects[0].renderer.Texture.Width)
            {
                newPlatforms = true;
            }
            else
            {
                newPlatforms = false;
            }

            if (newPlatforms)
            {
                SpawnNewPlatforms();
            }
        }

        void CheckTapInput (GameTime gameTime)
        {
            // jump on two finger tap
            touches = TouchPanel.GetState();

            // reduce speed if no input has been given in a while
            timeSinceLastHit += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastHit > currentTolerance)
            {
                currentRunningSpeed--;
                timeSinceLastHit = 0f;
            }
               
            if (!player.IsGrounded)
            {
                previousDrumSide = DrumSide.side.NONE;
                return;
            }

            if (touches.Count == 1 && touches[0].State == TouchLocationState.Pressed)
            {
                if (CheckDrumHit(touches, leftDrum) && (previousDrumSide != DrumSide.side.LEFT || previousDrumSide == DrumSide.side.NONE))
                {
                    bongo1.Play();
                    //if (CheckDrumTiming(leftDrum, gameTime))
                    //{
                    //    successCounter++;
                    //    if (successCounter % 10 == 0)
                    //    {
                    //        currentTempo += 1;
                    //        successCounter = 0;
                    //    }
                    //    dino.physics.Velocity -= Vector2.UnitX * 0.1f;
                    //    previousDrumSide = leftDrum.drumSide;
                    //    //player.physics.AddForce(Vector2.UnitX * 2000);
                    //}
                    //else
                    //{
                    //    dino.physics.Velocity += Vector2.UnitX * 0.08f;
                    //}

                    timeSinceLastHit = 0f;
                    currentRunningSpeed++;
                    previousDrumSide = leftDrum.drumSide;
                    dino.transform.Position -= Vector2.UnitX * Math.Max(1, maxRunningSpeed / (currentRunningSpeed));
                }
                else if (CheckDrumHit(touches, rightDrum) && (previousDrumSide != DrumSide.side.RIGHT || previousDrumSide == DrumSide.side.NONE))
                {
                    bongo2.Play();
                    //if (CheckDrumTiming(rightDrum, gameTime))
                    //{
                    //    successCounter++;
                    //    if(successCounter % 10 == 0)
                    //    {
                    //        currentTempo += 2;
                    //        successCounter = 0;
                    //    }
                    //    dino.physics.Velocity -= Vector2.UnitX * 0.1f;
                    //    previousDrumSide = rightDrum.drumSide;
                    //    //player.physics.AddForce(Vector2.UnitX * 2000);
                    //}
                    //else
                    //{
                    //    dino.physics.Velocity += Vector2.UnitX * 0.25f;
                    //}

                    timeSinceLastHit = 0f;
                    currentRunningSpeed++;
                    previousDrumSide = rightDrum.drumSide;
                    dino.transform.Position -= Vector2.UnitX * Math.Max(1, maxRunningSpeed / (currentRunningSpeed));
                }
            }
            else if (touches.Count == 2 && CheckDrumHit(touches, leftDrum, rightDrum)
                && (touches[0].State == TouchLocationState.Pressed || touches[0].State == TouchLocationState.Moved)
                && (touches[1].State == TouchLocationState.Pressed || touches[1].State == TouchLocationState.Moved))
            {
                player.Jump();
            }

            // clamp tempo to maxTempo
            if (currentRunningSpeed > maxRunningSpeed)
                currentRunningSpeed = maxRunningSpeed;
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
            spriteBatch.Begin();

            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            treesBack.Draw(spriteBatch);
            treesFront.Draw(spriteBatch);

            dino.Draw(gameTime);

            if (touches.Count == 1)
            {
                spriteBatch.DrawString(font, touches[0].State.ToString(), Vector2.UnitY * 20, Color.Black);
            }
            else if (touches.Count == 2)
            {
                spriteBatch.DrawString(font, touches[0].State.ToString(), Vector2.UnitY * 20, Color.Black);
                spriteBatch.DrawString(font, touches[0].State.ToString(), Vector2.UnitY * 40, Color.Black);
            }

            if(hit)
            {
                spriteBatch.DrawString(font, "HIT", Vector2.Zero, Color.Black);
            }

            spriteBatch.DrawString(font, "Player: " + player.transform.Position.ToString(),
                    new Vector2(0, 100), Color.Black);
            spriteBatch.DrawString(font, "Dino: " + dino.transform.Position.ToString() + ", " + dino.physics.Velocity.ToString(),
                   new Vector2(0, 120), Color.Black);
            spriteBatch.DrawString(font, "Current tempo: " + currentRunningSpeed.ToString(),
                   new Vector2(0, 140), Color.Black);
            //spriteBatch.DrawString(font, "Current tolerance: " + currentTolerance.ToString(),
            //      new Vector2(0, 160), Color.Black);
            spriteBatch.DrawString(font, "Current clubs: " + player.CurrentClubs.ToString(),
                 new Vector2(0, 160), Color.Black);

            leftDrum.Draw(gameTime);
            rightDrum.Draw(gameTime);

            foreach (GameObject go in platformPool.Objects)
                go.Draw(gameTime);
            foreach (GameObject go in healthCollectiblePool.Objects)
                go.Draw(gameTime);
            foreach (GameObject go in obstaclePool.Objects)
                go.Draw(gameTime);

            player.Draw(gameTime);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private bool CheckDrumTiming(Drum drum, GameTime gameTime)
        {
            hit = false;
            if (drum.drumSide != previousDrumSide)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds <= clickTimes[0] + currentTolerance ||
                    gameTime.TotalGameTime.TotalMilliseconds >= clickTimes[3] - currentTolerance ||
                    (gameTime.TotalGameTime.TotalMilliseconds >= clickTimes[1] - currentTolerance &&
                    gameTime.TotalGameTime.TotalMilliseconds <= clickTimes[1] + currentTolerance) ||
                    (gameTime.TotalGameTime.TotalMilliseconds >= clickTimes[2] - currentTolerance &&
                    gameTime.TotalGameTime.TotalMilliseconds <= clickTimes[2] + currentTolerance))
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

        public void PauseGame()
        {
            gameState = GameState.Paused;
        }

        public void UnpauseGame()
        {
            gameState = GameState.InGame;
        }
    }
}
