using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class Obstacle : GameObject
    {
        bool isDestructible;

        public bool IsDestructible
        {
            get { return isDestructible; }
            set { isDestructible = value; }
        }

        public Obstacle(CavemanRunner game, Texture2D texture, Vector2 position)
            : base(game, texture, position)
        {

        }

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
    }
}
