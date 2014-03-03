using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class Renderer
    {
        Texture2D texture;
        Dictionary<string, Animation> animations;
        bool isAnimating;
        bool animateAlways;
        // isVisible ei tarvita, sillä GameComponentissa on jo vastaava

        public void PlayAnimation (string animationName)
        {
            Animation animation;
            if (animations.TryGetValue(animationName, out animation))
                animation.Active = true;
        }
    }
}
