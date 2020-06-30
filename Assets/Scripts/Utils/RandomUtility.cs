using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utils
{
    static class RandomUtility
    {
        public static T RandomizeFrom<T>(IEnumerable<T> collection)
        {
            var index = UnityEngine.Random.Range(0, collection.Count() - 1);
            var randomItem = collection.ElementAt(index);
            return randomItem;
        }
    }
}
