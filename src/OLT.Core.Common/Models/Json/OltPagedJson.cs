using System.Collections.Generic;

namespace OLT.Core
{
    public class OltPagedJson<TModel> : IOltPaged<TModel>
    {
        public virtual int Size { get; set; }
        public virtual int Page { get; set; }

        public virtual int Count { get; set; }
        public virtual IEnumerable<TModel> Data { get; set; }

    }
}
