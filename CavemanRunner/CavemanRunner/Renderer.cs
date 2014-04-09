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
        public AnchorPoint Anchor { get { return anchorPoint; } }

        public Animation activeAnimation = null;

        private AnchorPoint anchorPoint = AnchorPoint.Center;
        Dictionary<string, Animation> animations;
        bool isAnimating;
        bool animateAlways;
        Vector2 renderOffset;
        public GameObject gameObject;

        public Vector2 RenderOffset
        {
            get { return renderOffset; }
            set { renderOffset = value; }
        }

        public void Initialize (GameObject owner, AnchorPoint anchor)
        {
            this.gameObject = owner;
            animations = new Dictionary<string, Animation>();
            SetAnchorPoint(anchor);
        }

       public void SetAnchorPoint (AnchorPoint anchor)
       {
           anchorPoint = anchor;

           if (anchorPoint == AnchorPoint.Center)
               renderOffset = new Vector2((float)gameObject.collider.Bounds.Width / 2, (float)gameObject.collider.Bounds.Height / 2);
           else if (anchorPoint == AnchorPoint.TopLeft)
               renderOffset = Vector2.Zero;
           else if (anchorPoint == AnchorPoint.BottomMiddle)
               renderOffset = new Vector2((float)gameObject.collider.Bounds.Width / 2, gameObject.collider.Bounds.Height);
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
            if (activeAnimation != null)
            {
                // Removed RenderOffset from here! Is it needed? YES!
                spriteBatch.Draw(activeAnimation.spriteStrip, gameObject.transform.Position - this.renderOffset, this.activeAnimation.sourceRect,
                    Color.White, 0f, Vector2.Zero, gameObject.transform.Scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(this.Texture, gameObject.transform.Position - this.RenderOffset, this.Texture.Bounds,
                    Color.White, 0f, Vector2.Zero, gameObject.transform.Scale, SpriteEffects.None, 0f);
            }
        }

        public void AddAnimation(string animationName, Texture2D animationTexture, int frameWidth, int frameHeight, int frameCount,
            int frametime, Color color, float scale, bool looping, bool setActive = false, bool tieToTempo = false)
        {
            Animation newAnimation = new Animation();
            newAnimation.Initialize(this, animationTexture, frameWidth, frameHeight, frameCount, frametime, color, scale, looping, tieToTempo);
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
