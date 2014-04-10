using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    public class Animation
    {
        // the "owner" Renderer of this animation
        Renderer renderer;
        // The image representing the collection of images used for animation
        public Texture2D spriteStrip;
        // The scale used to display the sprite strip
        float scale;
        // The time since we last updated the frame
        int elapsedTime;
        // The time we display a frame until the next one
        int frameTime;
        // The number of frames that the animation contains
        int frameCount;
        // The index of the current frame we are displaying
        int currentFrame;
        // The color of the frame we will be displaying
        Color color;

        // The area of the image strip we want to display
        public Rectangle sourceRect = new Rectangle();
        // Width of a given frame
        public int FrameWidth;
        // Height of a given frame
        public int FrameHeight;
        // The state of the Animation
        public bool Active;
        // Determines if the animation will keep playing or deactivate after one run
        public bool Looping;

        // Tie this animation to tempo?
        public bool TieToTempo = false;
        private int originalFrameTime;

        public void Initialize(Renderer renderer, Texture2D texture, int frameWidth, int frameHeight, int frameCount, int frametime, Color color, float scale, bool looping, bool tieToTempo)
        {
            // Keep a local copy of the values passed in
            this.renderer = renderer;
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = originalFrameTime = frametime;
            this.scale = scale;

            Looping = looping;
            spriteStrip = texture;
            TieToTempo = tieToTempo;

            // Set the time to zero
            elapsedTime = 0;
            currentFrame = 0;

            // Set the Animation to active by default
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (!Active) return;

            // update frameTime if the animation is tied to tempo
            if (TieToTempo)
                frameTime = (int)(renderer.gameObject.game.currentTempo * originalFrameTime / (float)renderer.gameObject.game.startingTempo);
            
            // Update the elapsed time
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (elapsedTime > frameTime)
            {
                // Move to the next frame
                currentFrame++;

                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;

                    // If we are not looping deactivate the animation
                    if (!Looping)
                        Active = false;
                }

                // Reset the elapsed time to zero
                elapsedTime = 0;
            }

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the Frame width
            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight); 
        }
    }
}
