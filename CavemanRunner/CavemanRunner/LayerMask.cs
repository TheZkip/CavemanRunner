using System;
using System.Collections.Generic;


namespace CavemanRunner
{
    public static class LayerMask
    {
        public enum Layer
        {
            Player,
            Platform,
            HealthCollectible,
            ScoreCollectible
        }

        bool[,] layerMatrix = {
                                { true, true, false, true },
                                { true, true, false, true },
                                { true, true, false, true },
                                { true, true, false, true }
                              };

        public bool GetLayerCollision(Layer a, Layer b)
        {
            return layerMatrix[(int)a, (int)b];
        }
    }
}
