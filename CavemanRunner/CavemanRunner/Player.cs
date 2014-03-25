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
        CavemanRunner.CollisionID collisionID = CavemanRunner.CollisionID.Player;
        CavemanRunner.CollisionID[] collidingObjects = { CavemanRunner.CollisionID.Platform };

        public float Health { get { return health; } set { health = value; } }

        public void Update(GameTime gameTime)
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

        public void Draw(GameTime gameTime)
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
