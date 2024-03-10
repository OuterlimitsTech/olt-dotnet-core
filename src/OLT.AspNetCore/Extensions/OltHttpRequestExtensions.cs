using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace OLT.Core
{
    public static class OltHttpRequestExtensions
    {

        /// <summary>
        /// Retrieve the raw body as a string from the Request.Body stream
        /// </summary>
        /// <param name="request">Request instance to apply to</param>
        /// <param name="encoding">Optional - Encoding, defaults to UTF8</param>
        /// <returns></returns>
        public static Task<string> GetRawBodyStringAsync(this HttpRequest request, Encoding? encoding = null)
        {
            ArgumentNullException.ThrowIfNull(request);
            return GetRawBodyStringInternalAsync(request, encoding);
        }

        private static async Task<string> GetRawBodyStringInternalAsync(this HttpRequest request, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            using StreamReader reader = new StreamReader(request.Body, encoding);
            return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// Retrieves the raw body as a byte array from the Request.Body stream
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Task<byte[]> GetRawBodyBytesAsync(this HttpRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            return GetRawBodyBytesInternalAsync(request);
        }

        private static async Task<byte[]> GetRawBodyBytesInternalAsync(this HttpRequest request)
        {
            await using var ms = new MemoryStream(2048);
            await request.Body.CopyToAsync(ms);
            return ms.ToArray();
        }

        /// <summary>
        /// Parses <see cref="RouteValueDictionary"/>, <see cref="IQueryCollection" />, and <see cref="IFormCollection"/> to <see cref="Dictionary{TKey, TValue}"/>
        /// </summary>
        /// <remarks>
        /// Duplicate keys will be merged into a single <see cref="StringValues"/>
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static OltGenericParameter ToOltGenericParameter(this HttpRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var dictionaries = new List<Dictionary<string, StringValues>>();

            try
            {
                var values = request.RouteValues?.ToDictionary(k => k.Key, v => new StringValues(v.Value?.ToString()));
                if (values != null)
                {
                    dictionaries.Add(values);
                }                
            }
            catch
            {
                // Ignore
            }

            try
            {
                var values = request.Query?.ToDictionary(k => k.Key, v => v.Value);
                if (values != null)
                {
                    dictionaries.Add(values);
                }
            }
            catch
            {
                // Ignore
            }

           
            var merged = Merge(dictionaries).ToDictionary(x => x.Key, y => y.Value.ToString());

            return new OltGenericParameter(merged);
        }

        /// <summary>
        /// Parses <see cref="RouteValueDictionary"/> to <see cref="Dictionary{TKey, TValue}"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static OltGenericParameter ToOltGenericParameter(this RouteValueDictionary value)
        {
            ArgumentNullException.ThrowIfNull(value);
            var values = value.ToDictionary(k => k.Key, v => v.Value?.ToString() ?? string.Empty);
            return new OltGenericParameter(values);
        }

        /// <summary>
        /// Parses <see cref="IQueryCollection"/> to <see cref="Dictionary{TKey, TValue}"/>
        /// </summary>
        /// <remarks>
        /// Duplicate keys will be merged into a single <see cref="StringValues"/>
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static OltGenericParameter ToOltGenericParameter(this IQueryCollection value)
        {
            ArgumentNullException.ThrowIfNull(value);
            var values = value.ToDictionary(k => k.Key, v => v.Value.ToString());
            return new OltGenericParameter(values);
        }

        /// <summary>
        /// Parses <see cref="IFormCollection"/> to <see cref="Dictionary{TKey, TValue}"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static OltGenericParameter ToOltGenericParameter(this IFormCollection value)
        {
            ArgumentNullException.ThrowIfNull(value);
            return new OltGenericParameter(value.ToDictionary(k => k.Key, v => v.Value.ToString()));
        }

        private static Dictionary<string, StringValues> Merge(IEnumerable<Dictionary<string, StringValues>> dictionaries)
        {
            Dictionary<string, StringValues> result = new Dictionary<string, StringValues>();

            foreach (Dictionary<string, StringValues> dict in dictionaries)
            {
                result
                    .Union(dict)
                    .GroupBy(g => g.Key)
                    .ToList()
                    .ForEach(item =>
                    {
                        if (!dict.ContainsKey(item.Key))
                        {
                            return;
                        }

                        var newValues = dict[item.Key];
                        if (result.ContainsKey(item.Key))
                        {
                            var currentValues = result[item.Key];
                            var concat = newValues.Concat(currentValues).ToArray();
                            result[item.Key] = new StringValues(concat);
                        }
                        else
                        {
                            result.Add(item.Key, new StringValues(newValues.ToArray()));
                        }

                    });
            }

            return result;
        }
    }
}
