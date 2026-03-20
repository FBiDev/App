using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App.Core
{
    public static class ListExtension
    {
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || source.Count() == 0;
        }

        public static bool HasValue<T>(this IEnumerable<T> source)
        {
            return source.IsEmpty() == false;
        }

        /// <summary>
        /// First item, or if null, return new T
        /// </summary>
        public static T First<T>(this List<T> source) where T : class, new()
        {
            if (source.Count() == 0)
            {
                return new T();
            }

            return source.ElementAt(0);
        }

        /// <summary>
        /// First item or default value
        /// </summary>
        public static T FirstOrDefault<T>(this List<T> source) where T : class, new()
        {
            if (source.Count == 0)
            {
                return null;
            }

            return source.ElementAt(0);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }
        }

        public static Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> function)
        {
            return Task.WhenAll(
                from item in source
                select Task.Run(() => function(item)));
        }

        public static void Move<T>(this List<T> source, T item, int index)
        {
            var oldIndex = source.IndexOf(item);

            if (oldIndex > -1)
            {
                source.RemoveAt(oldIndex);

                if (index > oldIndex)
                {
                    index--;
                }

                if (index > source.Count)
                {
                    index = source.Count;
                }

                if (index < 0)
                {
                    index = 0;
                }

                source.Insert(index, item);
            }
        }

        public static void MoveToStart<T>(this List<T> source, T item)
        {
            source.Move(item, 0);
        }

        public static void MoveToStart<T>(this List<T> source, IEnumerable<T> items)
        {
            items.ToList().ForEach(x => source.Move(x, 0));
        }

        public static void MoveToEnd<T>(this List<T> source, T item)
        {
            source.Move(item, source.Count);
        }

        public static void MoveToEnd<T>(this List<T> source, IEnumerable<T> items)
        {
            items.ToList().ForEach(x => source.Move(x, source.Count));
        }

        /// <summary>
        /// Insert new item at start of the list
        /// </summary>
        public static List<T> PrependNewItem<T>(this List<T> source)
        {
            if (typeof(T) == typeof(string))
            {
                source.Insert(0, (T)(object)string.Empty);
            }
            else
            {
                source.Insert(0, Activator.CreateInstance<T>());
            }

            return source;
        }

        public static IEnumerable<T> OrderByNatural<T>(this IEnumerable<T> source, Func<T, string> selector, StringComparer stringComparer = null)
        {
            var regex = new Regex(@"\d+", RegexOptions.Compiled);

            int maxDigits = source
                .SelectMany(i => regex.Matches(selector(i)).Cast<Match>()
                    .Select(digitChunk => (int?)digitChunk.Value.Length)).Max() ?? 0;

            return source.OrderBy(
                i => regex.Replace(
                    selector(i),
                    match => match.Value.PadLeft(maxDigits, '0')),
                stringComparer ?? StringComparer.CurrentCulture);
        }
    }
}