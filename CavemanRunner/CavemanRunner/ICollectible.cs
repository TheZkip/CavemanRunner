using System;
using System.Collections.Generic;

namespace CavemanRunner
{
    interface ICollectible
    {
        int value;
        public int Value { get { return this.value; } set { this.value = value; } }

        public void Do ()
        {

        }
    }
}
