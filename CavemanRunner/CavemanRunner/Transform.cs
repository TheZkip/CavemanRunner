using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CavemanRunner
{
    public class Transform
    {
        GameObject gameObject;
        Transform parent = null;
        //IList<Transform> children = new List<Transform>();
        Vector2 position = Vector2.Zero;
        Vector2 localPosition;
        float rotation = 0f;
        Vector2 scale = Vector2.One;

        public Transform (GameObject gameObject, Transform parent = null)
        {
            this.gameObject = gameObject;
            this.parent = parent;
            this.scale = Vector2.One * gameObject.game.scaleToReference;
        }

        public Transform Parent { get { return parent; } set { parent = value; } }
        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 LocalPosition { get { return localPosition; } set { localPosition = value; } }
        public float Rotation { get { return rotation; } set { rotation = value; } }
        public Vector2 Scale { get { return scale; } set { scale = value; } }

        public void Update ()
        {
            // update transform
            if (parent != null)
                //position += gameObject.physics.Velocity + parent.gameObject.physics.Velocity;
                position = parent.Position + localPosition;
            else
                position += gameObject.physics.Velocity;

            //position += localPosition;
        }
    }
}
