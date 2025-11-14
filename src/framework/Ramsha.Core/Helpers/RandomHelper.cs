using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Core;

public static class RandomHelper
{
    private static readonly Random Rnd = new Random();


    public static int GetRandom(int minValue, int maxValue)
    {
        lock (Rnd)
        {
            return Rnd.Next(minValue, maxValue);
        }
    }


    public static int GetRandom(int maxValue)
    {
        lock (Rnd)
        {
            return Rnd.Next(maxValue);
        }
    }

    public static int GetRandom()
    {
        lock (Rnd)
        {
            return Rnd.Next();
        }
    }

    public static T GetRandomOf<T>(params T[] objs)
    {
        return objs[GetRandom(0, objs.Length)];
    }


    public static T GetRandomOfList<T>(IList<T> list)
    {
        return list[GetRandom(0, list.Count)];
    }
    public static List<T> GenerateRandomizedList<T>(IEnumerable<T> items)
    {
        var currentList = new List<T>(items);
        var randomList = new List<T>();

        while (currentList.Any())
        {
            var randomIndex = GetRandom(0, currentList.Count);
            randomList.Add(currentList[randomIndex]);
            currentList.RemoveAt(randomIndex);
        }

        return randomList;
    }
}
