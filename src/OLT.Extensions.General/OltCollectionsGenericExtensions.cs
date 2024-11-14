using System.Text;


namespace System.Collections.Generic
{

    public static class OltCollectionsGenericExtensions
    {

        /// <summary>
        /// Executes <see cref="string.Join(string,string[])"/> on the current <see cref="IEnumerable{T}"/> of strings.
        /// </summary>
        /// <param name="list">The current <see cref="IEnumerable{T}"/> of strings.</param>
        /// <param name="separator">A <see cref="string"/> containing the value that will be placed between each <see cref="string"/> in the collection.</param>
        /// <returns>A <see cref="string"/> containing the joined strings.</returns>
        public static string Join(this IEnumerable<string> list, string separator)
        {
            return string.Join(separator, new List<string>(list).ToArray());
        }     
    }
}