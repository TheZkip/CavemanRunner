using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    public class Renderer
    {
        public enum AnchorPoint
        {
            Center = 0,
            TopLeft,
            BottomMiddle
        }

        public Texture2D Texture { get; set; }
        public int Depth { get; set; }

        private AnchorPoint anchorPoint = AnchorPoint.Center;
        Dictionary<string, Animation> animations;
        bool isAnimating;
        bool animateAlways;
        Vector2 renderOffset;

        public Vector2 RenderOffset
        {
            get { return renderOffset; }
            set { renderOffset = value; }
        }

        public void Initialize ()
        {

            SetAnchorPoint(AnchorPoint.Center);
        }

       public void SetAnchorPoint (AnchorPoint anchor)
       {
           anchorPoint = anchor;

           if (anchorPoint == AnchorPoint.Center)
               renderOffset = new Vector2((float)Texture.Width / 2, (float)Texture.Height / 2);
           else if (anchorPoint == AnchorPoint.TopLeft)
               renderOffset = Vector2.Zero;
           else if (anchorPoint == AnchorPoint.BottomMiddle)
               renderOffset = new Vector2((float)Texture.Width / 2, 0f);
       }

        public void PlayAnimation (string animationName)
        {
            Animation animation;
            if (animations.TryGetValue(animationName, out animation))
                animation.Active = true;
        }
    }
}
