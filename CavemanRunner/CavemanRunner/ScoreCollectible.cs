using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class ScoreCollectible : GameObject, ICollectible
    {
        int value;
        public int Value { get { return this.value; } set { this.value = value; } }

        public void Do ()
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
