using System;
using System.Collections.Generic;

namespace CavemanRunner
{
    class PoolingSystem
    {
        private IList<GameObject> availableList = new List<GameObject>();
        private IList<GameObject> inUseList = new List<GameObject>();
        private GameObject tempGameObject;

        public PoolingSystem (CavemanRunner game, int initialSize)
        {
            for (int i = 0; i < initialSize; i++)
            {
                //availableList.Add(new GameObject(game));
            }
        }

        public GameObject GetElement ()
        {
            tempGameObject = availableList[0];
            availableList.RemoveAt(0);

            inUseList.Add(tempGameObject);

            return tempGameObject;
        }
    }
}
