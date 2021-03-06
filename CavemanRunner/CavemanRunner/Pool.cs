﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CavemanRunner
{
    class Pool<T> where T : GameObject, new()
    {
        private List<T> objects;

        public List<T> Objects
        {
            get { return objects; }
            set { objects = value; }
        }

        private List<T> reservedObjects;

        public Pool(int poolSize)
        {
            objects = new List<T>();
            reservedObjects = new List<T>();

            for(int i = 0; i < poolSize; i++)
            {
                reservedObjects.Add(new T());
            }
        }

        public void InitializeObjects(CavemanRunner game, Texture2D texture, Renderer.AnchorPoint anchor)
        {
            foreach(T o in reservedObjects)
            {
                o.Initialize(game, texture, anchor);
                o.collider.SetSize(texture.Bounds.Width, texture.Bounds.Height);
            }
        }

        public void InitializeObjects(CavemanRunner game, Texture2D texture, Vector2 velocity, int mass,
            bool isStatic = false, Renderer.AnchorPoint anchor = Renderer.AnchorPoint.Center, float colliderScaleRatio = 1f)
        {
            foreach (T o in reservedObjects)
            {
                o.Initialize(game, texture, velocity, mass, isStatic, anchor);
                o.collider.SetSize((int)(texture.Bounds.Width * colliderScaleRatio), (int)(texture.Bounds.Height * colliderScaleRatio));
            }
        }

        public void ReleaseObject(T o)
        {
            objects.Remove(o);
            reservedObjects.Add(o);
        }

        public GameObject ActivateNewObject()
        {
            objects.Add(reservedObjects[0]);
            reservedObjects.RemoveAt(0);

            return (objects[objects.Count - 1] as GameObject);
        }
    }
}
