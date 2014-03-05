using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class Player : GameObject
    {
        float jumpThreshold;
        float jumpStrength;
        float health;
        bool isSwinging;

        public Player(CavemanRunner game, Texture2D texture, Vector2 position)
            : base(game, texture, position)
        {

        }

        public float Health { get { return health; } set { health = value; } }

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }

        void Jump ()
        {

        }

        void StartSwinging ()
        {
            // TODO: change animation
            isSwinging = true;
        }

        void StopSwinging ()
        {
            // TODO: change animation
            isSwinging = false;
        }
    }
}
