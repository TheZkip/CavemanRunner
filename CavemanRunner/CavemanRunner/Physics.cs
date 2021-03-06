﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    public class Physics
    {
        public static float Gravity = 20f;
        public static float DefaultMass = 1f;

        GameObject gameObject;
        float mass;
        Vector2 velocity;
        bool isStatic;
        bool useGravity;
        float lastUpdate = 0f;
        IList<Vector2> forcesToApply;

        public Physics ()
        {
            forcesToApply = new List<Vector2>();
            mass = DefaultMass;
            velocity = new Vector2(0f, 0f);
        }

        public Physics (float mass, bool isStatic)
        {
            this.mass = mass;
            this.isStatic = isStatic;
            forcesToApply = new List<Vector2>();
            velocity = new Vector2(0f, 0f);
        }

        public Physics(float mass, bool isStatic, Vector2 velocity)
        {
            this.mass = mass;
            this.isStatic = isStatic;
            forcesToApply = new List<Vector2>();
            this.Velocity = velocity;
        }

        public void Initialize (GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public float Mass { get { return mass; } set { mass = value; } }
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public bool IsStatic { get { return isStatic; } set { isStatic = value; } }
        public bool UseGravity { get { return useGravity; } set { useGravity = value; } }

        void ApplyForce (Vector2 force, GameTime gameTime)
        {
            velocity.X += (force.X / mass) * gameTime.ElapsedGameTime.Milliseconds * 0.001f;
            velocity.Y += (force.Y / mass) * gameTime.ElapsedGameTime.Milliseconds * 0.001f;
        }

        public void AddForce (Vector2 force)
        {
            forcesToApply.Add(force);
        }

        public void Stop ()
        {
            velocity = Vector2.Zero;
        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            // bail out if object is static
            if (isStatic)
                return;

            // apply gravity
            if (useGravity)
            {
                if (gameObject is Player && gameObject.physics.velocity.Y > 0)
                    ApplyForce(new Vector2(0f, Gravity * mass * 3), gameTime);
                else
                    ApplyForce(new Vector2(0f, Gravity * mass), gameTime);
            }
                

            // apply all added forces
            foreach (Vector2 force in forcesToApply)
                ApplyForce(force, gameTime);

            // clear forces
            forcesToApply.Clear();
        }
    }
}
