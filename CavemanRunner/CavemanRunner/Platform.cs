using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class Platform : GameObject
    {
        public Platform(CavemanRunner game, Texture2D texture, Vector2 position, int mass)
            : base(game, texture, position, new Vector2(-(float)game.tempo / 100, 0), mass, true)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            if(this.transform.Position.X <= 0)
            {
                ((CavemanRunner)Game).Components.Remove(this);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
