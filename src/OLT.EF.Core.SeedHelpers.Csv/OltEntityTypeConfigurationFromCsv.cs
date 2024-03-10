using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CsvHelper;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OLT.Core
{
    public abstract class OltEntityTypeConfigurationFromCsv<TEntity, TCsvModel> : OltEntityTypeConfiguration<TEntity>
        where TEntity : class, IOltEntity, IOltEntityId, new()
        where TCsvModel : class, IOltCsvSeedModel<TEntity>
    {
        
        protected abstract string ResourceName { get; }
        protected abstract Assembly ResourceAssembly { get; }

        /// <summary>
        /// Loads embedded resource using <seealso cref="CsvReader"/>
        /// </summary>
        /// <returns>Csv Records</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileLoadException"></exception>
        /// <exception cref="FileNotFoundException"></exception>        
        public virtual List<TCsvModel> Load()
        {
            using Stream stream = ResourceAssembly.GetEmbeddedResourceStreamSafe(ResourceName);
            using StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            CsvReader csvReader = new CsvReader(reader, CultureInfo.CurrentCulture);
            return csvReader.GetRecords<TCsvModel>().ToList();
        }


        protected virtual void Map(TEntity entity, TCsvModel csvRecord)
        {
            if (entity is IOltEntitySortable sortableEntity)
            {
                sortableEntity.SortOrder = (short)9999;
            }

            if (entity is IOltEntityAudit auditEntity)
            {
                auditEntity.CreateUser = DefaultUsername;
                auditEntity.CreateDate = DefaultCreateDate;
            }

            csvRecord.Map(entity);

        }

        protected virtual List<TEntity> Map()
        {
            var list = new List<TEntity>();
            var csvRecords = Load();
            foreach (TCsvModel csvRecord in csvRecords)
            {
                var entity = new TEntity();
                Map(entity, csvRecord);
                list.Add(entity);
            }
            return list;
        }

        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            var list = Map();
            if (list.Any())
            {
                builder.HasData(list);
            }            
        }

    }
}