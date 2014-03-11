using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class GameObject
    {
        // private fields
        public Renderer renderer;
        public Transform transform;
        public Physics physics;
        public Collider collider;
        SpriteBatch spriteBatch;

        public void Initialize(CavemanRunner game, Texture2D texture)
        {
            renderer = new Renderer();
            renderer.Texture = texture;

            physics = new Physics();

            collider = new Collider();
            collider.Bounds = renderer.Texture.Bounds;

            transform = new Transform();

            spriteBatch = game.spriteBatch;
        }

        public void Initialize(CavemanRunner game, Texture2D texture, Vector2 velocity, int mass, bool isStatic = false)
        {
            renderer = new Renderer();
            renderer.Texture = texture;
            renderer.Initialize();

            physics = new Physics(mass, isStatic, velocity);

            collider = new Collider();
            collider.Bounds = renderer.Texture.Bounds;

            transform = new Transform();

            spriteBatch = game.spriteBatch;
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
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(renderer.Texture, transform.Position - renderer.RenderOffset, Color.White);
            spriteBatch.End();
        }
    }
}
