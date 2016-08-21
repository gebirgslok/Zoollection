using System;

namespace Zoollection
{
    public static class ArrayExtensions
    {
        public static T[] ExtractSubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
