using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class Obstacle : GameObject
    {
        bool isDestructible = true;

        public bool IsDestructible
        {
            get { return isDestructible; }
            set { isDestructible = value; }
        }

        public void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }
    }
}
