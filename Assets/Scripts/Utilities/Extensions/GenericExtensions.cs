using System.Collections.Generic;

public static class GenericExtensions
{
    /// <summary>
    /// Remove an item from a list and fill it's space with other items from the list.
    /// </summary>
    public static void RemoveAndFill<T>(this List<T> list, T item)
    {
        int length = list.Count;
        int index = list.IndexOf(item);

        if (index >= 0)
        {
            list.RemoveAt(index);
            for (int i = index + 1; i < list.Count; i++)
            {
                list[i - 1] = list[i];
                list.RemoveAt(i);
            }

            list.Capacity = length - 1;
        }
    }

    ///<summary>
    /// Populate an entire array with one value
    ///</summary>
    public static List<T> Populate<T>(this List<T> list, T value)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = value;
        }
        return list;
    }

    public static T[] Populate<T>(this T[] arr, T value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = value;
        }
        return arr;
    }
}