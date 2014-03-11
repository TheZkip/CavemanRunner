using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    public class Collider
    {
        private Rectangle bounds;
        private bool[,] layerMatrix;

        public Rectangle Bounds
        {
            get { return bounds; } // new Rectangle(bounds.X - bounds.Width / 2, bounds.Y - bounds.Height / 2, bounds.Width, bounds.Height); }
            set { bounds = value; }
        }

        public void SetPosition (Vector2 position)
        {
            bounds.X = (int)(position.X - bounds.Width / 2);
            bounds.Y = (int)(position.Y - bounds.Height / 2);
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
