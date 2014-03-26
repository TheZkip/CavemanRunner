using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CavemanRunner
{
    public class Transform
    {
        Vector2 position;
        float rotation;
        Vector2 scale;
        public Vector2 Position { get { return position; } set { position = value; } }
        public float Rotation { get { return rotation; } set { rotation = value; } }
        public Vector2 Scale { get { return scale; } set { scale = value; } }
    }
}
