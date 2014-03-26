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

        public Animation activeAnimation = null;

        private AnchorPoint anchorPoint = AnchorPoint.Center;
        Dictionary<string, Animation> animations;
        bool isAnimating;
        bool animateAlways;
        Vector2 renderOffset;
        GameObject parent;

        public Vector2 RenderOffset
        {
            get { return renderOffset; }
            set { renderOffset = value; }
        }

        public void Initialize (GameObject parent)
        {
            this.parent = parent;
            SetAnchorPoint(AnchorPoint.Center);
            animations = new Dictionary<string, Animation>();
        }

       public void SetAnchorPoint (AnchorPoint anchor)
       {
           anchorPoint = anchor;

           if (anchorPoint == AnchorPoint.Center)
               renderOffset = new Vector2((float)Texture.Width / 2, (float)Texture.Height / 2);
           else if (anchorPoint == AnchorPoint.TopLeft)
               renderOffset = Vector2.Zero;
           else if (anchorPoint == AnchorPoint.BottomMiddle)
               renderOffset = new Vector2((float)Texture.Width / 2, Texture.Height);
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

        public void Update(GameTime gameTime)
        {
            if(activeAnimation != null)
            {
                activeAnimation.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (activeAnimation != null)
            {
                // Removed RenderOffset from here! Is it needed?
                spriteBatch.Draw(activeAnimation.spriteStrip, parent.transform.Position, this.activeAnimation.sourceRect,
                    Color.White, 0f, Vector2.Zero, parent.transform.Scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(this.Texture, parent.transform.Position - this.RenderOffset, this.Texture.Bounds,
                    Color.White, 0f, Vector2.Zero, parent.transform.Scale, SpriteEffects.None, 0f);
            }
            spriteBatch.End();
        }

        public void AddAnimation(string animationName, Texture2D animationTexture, int frameWidth, int frameHeight, int frameCount,
            int frametime, Color color, float scale, bool looping, bool setActive = false)
        {
            Animation newAnimation = new Animation();
            newAnimation.Initialize(animationTexture, frameWidth, frameHeight, frameCount, frametime, color, scale, looping);
            newAnimation.Active = setActive;
            animations.Add(animationName, newAnimation);
            if(setActive)
                PlayAnimation(animationName);
        }

        public void StopAnimation()
        {
            if (activeAnimation != null)
                activeAnimation.Active = false;
        }

        public void ResumeAnimation()
        {
            if (activeAnimation != null)
                activeAnimation.Active = true;
        }
    }
}
