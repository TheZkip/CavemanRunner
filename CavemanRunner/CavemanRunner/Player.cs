using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class Player : GameObject
    {
        //float jumpThreshold;
        float jumpStrength = 40000f;
        float health = 0f;
        bool isSpecialInUse = false;
        bool isGrounded = false;
        bool jumping = false;
        CavemanRunner.CollisionID collisionID = CavemanRunner.CollisionID.Player;
        CavemanRunner.CollisionID[] collidingObjects = { CavemanRunner.CollisionID.Platform };

        public float Health { get { return health; } set { health = value; } }

        public void Update(GameTime gameTime)
        {
            if (isSpecialInUse)
            {

            }

            if (transform.Position.Y >= 300)
            {
                isGrounded = true;
                physics.Stop();
            }
            else if (transform.Position.Y < 300)
            {
                if (physics.Velocity.Y > 0)
                    jumping = false;

                isGrounded = false;
            }

            this.physics.AddForce(-this.transform.Position.X / 10 * Vector2.UnitX);
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void Jump ()
        {
            if (isGrounded && !jumping)
            {
                jumping = true;
                physics.AddForce(Vector2.UnitY * -jumpStrength);
                game.PlayBothBongos();
            }
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

        public void Move(int amount)
        {
            //this.transform.Position += Vector2.UnitX * amount;
        }
    }
}
