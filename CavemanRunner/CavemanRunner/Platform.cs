using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class Platform : GameObject
    {
        private static Random r = new Random();
        private Texture2D[] textures;

        public static float bottom = 7 / 8f;
        public static float bottomMiddle = 6 / 8f;
        public static float middle = 5 / 8f;
        public static float topMiddle = 4 / 8f;
        public static float top = 3 / 8f;

        public void Initialize(CavemanRunner game, Texture2D texture, Renderer.AnchorPoint anchor)
        {
            base.Initialize(game, texture, anchor);
        }

        public void Initialize(CavemanRunner game, Texture2D[] textureArray, Vector2 velocity, int mass,
            bool isStatic = false, Renderer.AnchorPoint anchor = Renderer.AnchorPoint.Center)
        {
            textures = new Texture2D[textureArray.Length];
            this.Initialize(game, textures[0], anchor);
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
