﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests.Contexts;

#nullable disable

namespace OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests.Migrations.SqlContext
{
    [DbContext(typeof(TestSqlContext))]
    [Migration("20241115155109_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OLT.Core.EntityFrameworkCore.SqlServer.Abstractions.Tests.Contexts.Entities.TestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("TestEntitiesId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 10L, 5);

                    b.HasKey("Id");

                    b.ToTable("TestEntities", "dbo");
                });
#pragma warning restore 612, 618
        }
    }
}
