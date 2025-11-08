using System;
using System.Collections.Generic;
using System.Linq;

namespace Nidavellir.Utils
{
    public static class ListExtensions
    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
            {
                return default;
            }
            
            var randomIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randomIndex];
        }
        
        public static List<T> Shuffle<T>(this IList<T> list)
        {
            return list.OrderBy(_ => Guid.NewGuid()).ToList();
        }
    }
}