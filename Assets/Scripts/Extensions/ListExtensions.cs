using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Extensions
{
    public static class Resizing
    {
        public static List<T> Resize<T>(this List<T> list, int size, T defaultValue)
        {
            List<T> aux = new List<T>();
            int i;

            for(i = 0; i < Math.Min(list.Count, size); i++)
            {
                aux.Add(list[i]);
            }

            //In case the resizing has more values than the original
            while (i < size)
            {
                aux.Add(defaultValue);
                i++;
            }

            list = aux;
            return list;
        }

        public static List<T> Resize<T>(this List<T> list, int size) where T : new()
        {
            return Resize<T>(list, size, new T());
        }

        public static T[] Resize<T>(this T[] array, int size, T defaultValue = default(T))
        {
            List<T> aux = new(array);
            return Resize<T>(aux, size, defaultValue).ToArray();
        }

        public static T[] Resize<T>(this T[] array, int size) where T : new()
        {
            List<T> aux = new(array);
            return Resize<T>(aux, size, new T()).ToArray();
        }
    }
}