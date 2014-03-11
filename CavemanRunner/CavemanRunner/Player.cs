using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class Player : GameObject
    {
        //float jumpThreshold;
        float jumpStrength = 5000f;
        float health = 0f;
        bool isSpecialInUse = false;
        bool isGrounded = false;

        public Player(CavemanRunner game, Texture2D texture, Vector2 position, Vector2 velocity, int mass)
            : base(game, texture, position, velocity, mass)
        {
            game.Components.Add(this);
        }

        public float Health { get { return health; } set { health = value; } }

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            if (isSpecialInUse)
            {

            }

            if (transform.Position.Y > 300)
            {
                transform.Position = Vector2.UnitY * 300;
                isGrounded = true;
                physics.Stop();
            }
            else if (transform.Position.Y < 300)
            {
                isGrounded = false;
            }
               

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }

        public void Jump ()
        {
            if (isGrounded)
                physics.AddForce(Vector2.UnitY * -jumpStrength);
        }

        void StartSpecial ()
        {
            // TODO: change animation
            isSpecialInUse = true;
        }

        void StopSpecial ()
        {
            // TODO: change animation
            isSpecialInUse = false;
        }
    }
}
