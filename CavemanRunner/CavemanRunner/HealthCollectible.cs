using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class HealthCollectible : GameObject, ICollectible
    {
        int value;
        public int Value { get { return this.value; } set { this.value = value; } }

        public HealthCollectible(CavemanRunner game, Texture2D texture, Vector2 position)
            : base(game, texture, position)
        {

        }

        public void Do ()
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
