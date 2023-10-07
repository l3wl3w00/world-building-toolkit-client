#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Linq
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> actionOnT)
        {
            foreach (var item in enumerable) actionOnT.Invoke(item);
        }

        public static void IndexedForEach<T>(this IEnumerable<T> enumerable, Action<int, T> actionOnT)
        {
            var i = 0;
            foreach (var item in enumerable)
            {
                actionOnT.Invoke(i, item);
                i++;
            }
        }

        public static IEnumerable<List<T>> DivideInto<T>(this IEnumerable<T> enumerable, int count)
        {
            return enumerable
                .Select((value, index) => new { Value = value, Group = index / count })
                .GroupBy(item => item.Group, item => item.Value)
                .Select(group => group.ToList());
        }

        public static IEnumerable<T> ToEnumerable<T>(this (T, T, T) tuple)
        {
            yield return tuple.Item1;
            yield return tuple.Item2;
            yield return tuple.Item3;
        }
    }
}