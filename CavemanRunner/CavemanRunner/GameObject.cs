using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    public class GameObject
    {
        // private fields
        public Renderer renderer;
        public Transform transform;
        public Physics physics;
        public Collider collider;

        protected CavemanRunner game;

        SpriteBatch spriteBatch;

        public void Initialize(CavemanRunner game, Texture2D texture, Renderer.AnchorPoint anchor)
        {
            this.game = game;

            renderer = new Renderer();
            renderer.Texture = texture;
			renderer.SetAnchorPoint(anchor);
            if(physics == null)
                physics = new Physics();

            collider = new Collider();
            collider.Bounds = renderer.Texture.Bounds;
            collider.SetAnchorPoint(anchor);

            transform = new Transform();
            transform.Scale = Vector2.One * game.scaleToReference;

            spriteBatch = game.spriteBatch;
        }

        public void Initialize(CavemanRunner game, Texture2D texture, Vector2 velocity, int mass,
            bool isStatic = false, Renderer.AnchorPoint anchor = Renderer.AnchorPoint.Center)
        {

            renderer.SetAnchorPoint(anchor);            physics = new Physics(mass, isStatic, velocity);

            this.Initialize(game, texture);
        }

        public void Update(GameTime gameTime)
        {
            // update physics
            if (physics != null)
                physics.Update(gameTime, transform.Position);

            // update transform
            transform.Position += physics.Velocity;

            // set collider to position
            collider.SetPosition(transform.Position);
            //collider.CheckCollisions();

            renderer.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            renderer.Draw(spriteBatch);
        }
    }
}
