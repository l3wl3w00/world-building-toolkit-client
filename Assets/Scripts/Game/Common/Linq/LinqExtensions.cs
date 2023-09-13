using System;
using System.Collections.Generic;

namespace WorldBuilder.Client.Game.Common.Linq
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> actionOnT)
        {
            foreach (var item in enumerable)
            {
                actionOnT?.Invoke(item);
            }
        }
    }
}