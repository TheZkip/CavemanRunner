using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class Physics
    {
        public static float Gravity = -9.81f;
        public static float DefaultMass = 1f;
        //public static float 

        float mass;
        Vector2 velocity;
        Rectangle collider;
        bool isStatic;
        float lastUpdate = 0f;

        public float Mass { get { return mass; } set { mass = value; } }
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public Rectangle Collider { get { return collider; } set { collider = value; } }
        public bool IsStatic { get { return isStatic; } set { isStatic = value; } }

        public void ApplyForce (Vector2 force, GameTime gameTime)
        {
            velocity.X += (force.X / mass) * gameTime.ElapsedGameTime.Seconds;
            velocity.Y += (force.Y / mass) * gameTime.ElapsedGameTime.Seconds;
        }

        public void Update(GameTime gameTime)
        {
            // apply gravity
            if (isStatic)
                return;
            else
                ApplyForce(new Vector2(0f, Gravity), gameTime);
        }
    }
}
