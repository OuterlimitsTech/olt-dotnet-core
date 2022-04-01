using FluentDateTimeOffset;
using System;

namespace OLT.Core
{
    public abstract class OltSearcherDateRange<TEntity> : OltSearcher<TEntity>, IOltEntityQueryBuilder<TEntity, OltDateRange>
       where TEntity : class, IOltEntity
    {
        protected OltSearcherDateRange()
        {

        }

        protected OltSearcherDateRange(OltDateRange value) : this(value.Start, value.End)
        {

        }

        protected OltSearcherDateRange(DateTimeOffset start, DateTimeOffset end)
        {
            Value.Start = start;
            Value.End = end;
        }


        public virtual OltDateRange Value { get; set; } = new OltDateRange();
                

        /// <summary>
        /// Adds 1 Second to End to allow a less than check
        /// </summary>
        protected virtual DateTimeOffset QueryEnd => Value.End.AddSeconds(1);
    }
}
