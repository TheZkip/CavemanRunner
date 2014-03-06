using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CavemanRunner
{
    class Player : GameObject
    {
        //float jumpThreshold;
        float jumpStrength = 0f;
        float health = 0f;
        bool isSpecialInUse = false;
        bool isGrounded = false;

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

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }

        public void Jump ()
        {
            physics.AddForce(Vector2.UnitY * jumpStrength);
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
