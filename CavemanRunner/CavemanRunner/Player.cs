using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class Player : GameObject
    {
        //float jumpThreshold;
        float jumpStrength = 90000f;
        float health = 0f;
        int currentClubs = 0, maxClubs = 3;
        bool isSpecialInUse = false;
        bool isGrounded = false;
        bool jumping = false;
        CavemanRunner.CollisionID collisionID = CavemanRunner.CollisionID.Player;
        CavemanRunner.CollisionID[] collidingObjects = { CavemanRunner.CollisionID.Platform };

        public float Health { get { return health; } set { health = value; } }
        public int CurrentClubs { get { return currentClubs; } }
        public bool IsGrounded { get { return isGrounded; } }
        public bool HasClub { get { return currentClubs > 0; } }

        public void Update(GameTime gameTime)
        {
            if (isSpecialInUse)
            {

            }

            if (transform.Position.Y > game.GraphicsDevice.Viewport.Height + collider.Bounds.Height * 2)
            {
                transform.Position = new Vector2(transform.Position.X, 0);
                physics.Velocity = Vector2.Zero;
            }
            
            base.Update(gameTime);
            //this.physics.AddForce(-this.transform.Position.X / 10 * Vector2.UnitX);
        }

        public void Draw(GameTime gameTime)
        {
            game.spriteBatch.DrawString(game.font, isGrounded.ToString(), Vector2.UnitY * 60, Color.Black);

            base.Draw(gameTime);
        }

        public void Jump ()
        {
            if (isGrounded && !jumping)
            {
                renderer.StopAnimation();
                physics.UseGravity = true;
                isGrounded = false;
                jumping = true;
                physics.AddForce(Vector2.UnitY * -jumpStrength);
                game.PlayBothBongos();
            }
        }

        public void SetGrounded(bool grounded)
        {
            if (grounded)
            {
                physics.UseGravity = false;
                jumping = false;
                isGrounded = true;
                physics.Stop();
				renderer.ResumeAnimation();
            }
            else
            {
                physics.UseGravity = true;
                jumping = true;
                isGrounded = false;
            }
        }

        public void AddClub ()
        {
            if (++currentClubs > maxClubs)
                currentClubs = maxClubs;
        }

        public void RemoveClub ()
        {
            if (--currentClubs < 0)
                currentClubs = 0;
        }

        public void Move(int amount)
        {
            //this.transform.Position += Vector2.UnitX * amount;
        }
    }
}
