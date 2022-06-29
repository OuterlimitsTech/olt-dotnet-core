using Newtonsoft.Json;

namespace OLT.Core
{
    public class OltNewtonsoftCacheSerializer : OltDisposable, IOltCacheSerializer
    {
        /// <summary>
        /// Convert <typeparamref name="T"/> to string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);            
        }


        /// <summary>
        /// Convert string to <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
