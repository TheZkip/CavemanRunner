using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CavemanRunner
{
    class Player : GameObject
    {
        float jumpThreshold;
        float jumpStrength;
        float health;
        bool isSwinging;

        public float Health { get { return health; } set { health = value; } }

        public override void Initialize()
        {

            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }

        public void Jump ()
        {
            physics.ApplyForce(Vector2.UnitY * jumpStrength);
        }

        void StartSwinging ()
        {
            // TODO: change animation
            isSwinging = true;
        }

        void StopSwinging ()
        {
            // TODO: change animation
            isSwinging = false;
        }
    }
}
