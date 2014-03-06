﻿using System;
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

        float mass;
        Vector2 velocity;
        Rectangle collider;
        bool isStatic;
        float lastUpdate = 0f;
        IList<Vector2> forcesToApply;

        public Physics ()
        {
            forcesToApply = new List<Vector2>();
            mass = DefaultMass;
            velocity = new Vector2(0f, 0f);
            collider = new Rectangle();
        }

        public Physics (float mass, bool isStatic)
        {
            this.mass = mass;
            this.isStatic = isStatic;
            forcesToApply = new List<Vector2>();
            velocity = new Vector2(0f, 0f);
            collider = new Rectangle();
        }

        public float Mass { get { return mass; } set { mass = value; } }
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public Rectangle Collider { get { return collider; } set { collider = value; } }
        public bool IsStatic { get { return isStatic; } set { isStatic = value; } }

        void ApplyForce (Vector2 force, GameTime gameTime)
        {
            velocity.X += (force.X / mass) * gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            velocity.Y += (force.Y / mass) * gameTime.ElapsedGameTime.Milliseconds * 0.001f;
        }

        public void AddForce (Vector2 force)
        {
            forcesToApply.Add(force);
        }

        public void Update(GameTime gameTime)
        {
            // bail out if object is static
            if (isStatic)
                return;

            // apply gravity
            ApplyForce(new Vector2(0f, Gravity), gameTime);

            // apply all added forces
            foreach (Vector2 force in forcesToApply)
                ApplyForce(force, gameTime);

            // clear forces
            forcesToApply.Clear();
        }
    }
}