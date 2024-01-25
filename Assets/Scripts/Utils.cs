using System;
using System.Collections.Generic;
using System.Linq;



public static class Utils
{
    /// <summary>
    /// Returns all enum values for the specified Enum type.
    /// </summary>
    public static List<T> GetEnumValues<T>() where T : Enum
    {
        return new List<T>((T[])Enum.GetValues(typeof(T)));
    }

    public static bool MasterVolumeSet { get; set; }
    public static float MasterVolume { get; set; }

    /// <summary>
    /// Returns a random element from the array.
    /// </summary>
    static public T GetRandom<T>(this T[] list)
    {
        int randomIndex = UnityEngine.Random.Range(0, list.Length);
        return list[randomIndex];
    }

    /// <summary>
    /// Returns a random element from the array.
    /// </summary>
    static public T GetRandom<T>(this IEnumerable<T> list)
    {
        int size = list.Count();
        int randomIndex = UnityEngine.Random.Range(0, size);
        return list.ElementAt(randomIndex);
    }

    /// <summary>
    /// Returns a random element from the array.
    /// </summary>
    static public T GetRandom<T>(this T[,] list)
    {
        int randomIndex1 = UnityEngine.Random.Range(0, list.GetLength(0));
        int randomIndex2 = UnityEngine.Random.Range(0, list.GetLength(1));
        return list[randomIndex1, randomIndex2];
    }
}
