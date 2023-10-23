using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace OLT.Core
{
    [Obsolete("Will be removed in 8.x")]
    public class OltFileBuilderManager : OltDisposable, IOltFileBuilderManager
    {
        private readonly Dictionary<string, IOltFileBuilder> _builders;
        public OltFileBuilderManager(IServiceProvider serviceProvider)
        {
            _builders = serviceProvider.GetServices<IOltFileBuilder>().ToDictionary(builder => builder.BuilderName, builder => builder);
        }

        public List<IOltFileBuilder> GetBuilders()
        {
            return _builders.Values.ToList();
        }

        /// <summary>
        /// Locates builder <paramref name="name"/> by using <see cref="IOltBuilder.BuilderName"/> and calls <see cref="IOltFileBuilder.Build{TRequest}(TRequest)"/>
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="OltFileBuilderNotFoundException"></exception>
        public virtual IOltFileBase64 Generate<TRequest>(TRequest request, string name)
            where TRequest : IOltRequest
        {
            if (_builders.TryGetValue(name, out IOltFileBuilder builder))
            {
                return builder.Build(request);
            }
            throw new OltFileBuilderNotFoundException(name);            
        }        

    }
}