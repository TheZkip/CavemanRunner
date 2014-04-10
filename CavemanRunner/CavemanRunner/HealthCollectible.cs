using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class HealthCollectible : GameObject, ICollectible
    {
        int value = 1;
        public int Value { get { return this.value; } set { this.value = value; } }

        public void Collect ()
        {

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
