using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Extentions
{
    public static class CollectionExtensions
    {
        private static Random random = new();

        /// <summary>Returns the index of an element contained in a list if it is found, otherwise returns -1.</summary>
        public static int IndexOf<T>(this IReadOnlyList<T> list, T element) // IList doesn't implement IndexOf for some reason
        {
            for (var i = 0; i < list.Count; i++)
                if (list[i]?.Equals(element) ?? false)
                    return i;
            return -1;
        }

        /// <summary>Fluid method that joins the members of a collection using the specified separator between them.</summary>
        public static string Join<T>(this IEnumerable<T> values, string separator = "")
        {
            return string.Join(separator, values);
        }

        /// <summary>
        /// Extension method to shuffle an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns>Shuffled Array</returns>
        public static T[] Shuffle<T>(this T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int r = random.Next(n + 1);
                T t = array[r];
                array[r] = array[n];
                array[n] = t;
            }

            return array;
        }

        /// <summary>
        /// Converts an object into a JSON string
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ToJson(this object item)
        {
            var ser = new DataContractJsonSerializer(item.GetType());
            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, item);
                var sb = new StringBuilder();
                sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
                return sb.ToString();
            }
        }
        public static string ToJson(this IEnumerable collection, string rootName)
        {
            var ser = new DataContractJsonSerializer(collection.GetType());
            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, collection);
                var sb = new StringBuilder();
                sb.Append("{ \"").Append(rootName).Append("\": ");
                sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
                sb.Append(" }");
                return sb.ToString();
            }
        }
        /// <summary>
        /// Converts a JSON string into the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns>The converted object</returns>
        public static T FromJsonTo<T>(this string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                T jsonObject = (T)ser.ReadObject(ms);
                return jsonObject;
            }
        }
    }
}
