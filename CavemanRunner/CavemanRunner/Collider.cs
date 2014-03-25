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

        public Rectangle Bounds
        {
            get { return bounds; } // new Rectangle(bounds.X - bounds.Width / 2, bounds.Y - bounds.Height / 2, bounds.Width, bounds.Height); }
            set { bounds = value; }
        }

        public void SetAnchorPoint (Renderer.AnchorPoint anchor)
        {
            this.anchor = anchor;
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
        }

        public bool CheckCollisions (Collider other)
        {
            if (other.bounds.Intersects(Bounds))
                return true;
            else
                return false;
        }
    }
}
