using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    public class Collider
    {
        private Renderer.AnchorPoint anchor = Renderer.AnchorPoint.Center;
        private Rectangle bounds;
        private LayerMask.Layer layer;
        private GameObject gameObject;

        public Rectangle Bounds
        {
            get { return bounds; } // new Rectangle(bounds.X - bounds.Width / 2, bounds.Y - bounds.Height / 2, bounds.Width, bounds.Height); }
            set { bounds = value; }
        }

        public void Initialize (GameObject owner)
        {
            gameObject = owner;
            anchor = gameObject.renderer.Anchor;
            bounds = new Rectangle(gameObject.renderer.Texture.Bounds.X, gameObject.renderer.Texture.Bounds.Y,
                (int)(bounds.Width * gameObject.game.scaleToReference), (int)(bounds.Height * gameObject.game.scaleToReference));
        }

        public void SetSize(int width, int height)
        {
            bounds.Width = (int)(width * gameObject.game.scaleToReference);
            bounds.Height = (int)(height * gameObject.game.scaleToReference);
        }

        public void SetPosition (Vector2 position)
        {
            if (anchor == Renderer.AnchorPoint.Center)
            {
                bounds.X = (int)(position.X - bounds.Width / 2);
                bounds.Y = (int)(position.Y - bounds.Height / 2);
            }
            else if (anchor == Renderer.AnchorPoint.TopLeft)
            {
                bounds.X = (int)position.X;
                bounds.Y = (int)position.Y;
            }
            else if (anchor == Renderer.AnchorPoint.BottomMiddle)
            {
                bounds.X = (int)(position.X - bounds.Width / 2);
                bounds.Y = (int)(position.Y - bounds.Height);
            }
        }

        public bool CheckCollisions (Collider other)
        {
            if (other.bounds.Intersects(Bounds))
                return true;
            else
                return false;
        }

        public void Draw (CavemanRunner game)
        {
            game.spriteBatch.Draw(game.halfScreen, bounds, Color.White);
        }
    }
}
