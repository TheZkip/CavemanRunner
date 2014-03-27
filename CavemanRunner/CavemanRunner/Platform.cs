using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class Platform : GameObject
    {
        public static float top = 2 / 8f;
        public static float middle = 1 / 2f;
        public static float bottom = 7 / 9f;
        private static Random r = new Random();

        public void Initialize(CavemanRunner game, Texture2D texture, Renderer.AnchorPoint anchor)
        {
            bottom = game.GraphicsDevice.Viewport.Height - this.collider.Bounds.Height;
            top = game.GraphicsDevice.Viewport.Height / 4;
            middle = bottom - top;
        }

        public void Initialize(CavemanRunner game, Texture2D texture, Vector2 velocity, int mass,
            bool isStatic = false, Renderer.AnchorPoint anchor = Renderer.AnchorPoint.Center)
        {
            this.Initialize(game, texture, anchor);
        }

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public static float RandomHeight(int viewPortHeight)
        {
            float value = 0;
            int rand = r.Next(0, 3);
            switch(rand)
            {
                case 0:
                    value = viewPortHeight * bottom;
                    break;
                case 1:
                    value = viewPortHeight * middle;
                    break;
                case 2:
                    value = viewPortHeight * top;
                    break;
            }
            return value;
        }
    }
}
