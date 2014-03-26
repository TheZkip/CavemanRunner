using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class Renderer
    {
        public Texture2D Texture { get; set; }

        Dictionary<string, Animation> animations;
        bool isAnimating;
        bool animateAlways;
        Vector2 renderOffset;
        Animation activeAnimation = null;

        public Vector2 RenderOffset
        {
            get { return renderOffset; }
            set { renderOffset = value; }
        }

        public void Initialize ()
        {
            renderOffset = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public void PlayAnimation (string animationName)
        {
            if (animations.TryGetValue(animationName, out activeAnimation))
            {
                activeAnimation.Active = true;
            }
            else
            {
                activeAnimation = null;
            }
        }

        public void AddAnimation(string animationName, Animation animation)
        {
            animations.Add(animationName, animation);
        }
    }
}
