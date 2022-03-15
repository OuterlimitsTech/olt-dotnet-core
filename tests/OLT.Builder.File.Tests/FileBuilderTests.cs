using Microsoft.Extensions.DependencyInjection;
using OLT.Builder.File.Tests.Assets;
using OLT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace OLT.Builder.File.Tests
{
    public class FileBuilderTests : BaseUnitTests
    {

        [Fact]
        public void BuildFile()
        {
            var builder = new TestCsvBuilder();
            var expectedText = builder.Data.CsvString.ToString();
            var expected = Convert.ToBase64String(Encoding.ASCII.GetBytes(expectedText));
            var result = builder.Build(new TestCsvBuilderRequest());
            var csvText = Encoding.ASCII.GetString(Convert.FromBase64String(result.FileBase64));
            Assert.Equal(expectedText, csvText);
            Assert.Equal(expected, result.FileBase64);
        }

        [Fact]
        public void BuildTypedFile()
        {
            var builder = new TestCsvBuilderTyped();
            var expectedText = builder.Data.CsvString.ToString();
            var expected = Convert.ToBase64String(Encoding.ASCII.GetBytes(expectedText));
            var result = builder.Build(new TestCsvBuilderRequest());
            var csvText = Encoding.ASCII.GetString(Convert.FromBase64String(result.FileBase64));
            Assert.Equal(expectedText, csvText);
            Assert.Equal(expected, result.FileBase64);
        }


        [Fact]
        public void BuilderTests()
        {

            using (var provider = BuildProvider())
            {
                var builders = provider.GetServices<IOltFileBuilder>().ToList();
                var fileBuilderManager = provider.GetService<IOltFileBuilderManager>();

                var builderName = nameof(TestCsvBuilder);
                var builder = builders.FirstOrDefault(p => p.BuilderName == builderName) as TestCsvBuilder;
                var result = fileBuilderManager.Generate(new TestCsvBuilderRequest(), builderName);
                var expectedText = builder.Data.CsvString.ToString();
                var expected = Convert.ToBase64String(Encoding.ASCII.GetBytes(expectedText));
                var csvText = Encoding.ASCII.GetString(Convert.FromBase64String(result.FileBase64));

                Assert.Equal(expectedText, csvText);
                Assert.Equal(expected, result.FileBase64);

                Assert.Throws<OltFileBuilderNotFoundException>(() => fileBuilderManager.Generate(new TestCsvBuilderRequest(), Faker.Lorem.GetFirstWord()));
            }
        }


        [Fact]
        public void BuilderTypedTests()
        {

            using (var provider = BuildProvider())
            {
                var builders = provider.GetServices<IOltFileBuilder>().ToList();
                var fileBuilderManager = provider.GetService<IOltFileBuilderManager>();

                var builderName = nameof(TestCsvBuilderTyped);
                var builder = builders.FirstOrDefault(p => p.BuilderName == builderName) as TestCsvBuilderTyped;
                var result = fileBuilderManager.Generate(new TestCsvBuilderRequest(), builderName);
                var expectedText = builder.Data.CsvString.ToString();
                var expected = Convert.ToBase64String(Encoding.ASCII.GetBytes(expectedText));
                var csvText = Encoding.ASCII.GetString(Convert.FromBase64String(result.FileBase64));

                Assert.Equal(expectedText, csvText);
                Assert.Equal(expected, result.FileBase64);
            }


        }

        [Fact]
        public void ServiceTests()
        {

            using (var provider = BuildProvider())
            {
                var fileBuilderManager = provider.GetService<IOltFileBuilderManager>();

                var builderName = nameof(TestCsvServiceBuilder);
                var builder = fileBuilderManager.GetBuilders().FirstOrDefault(p => p.BuilderName == builderName) as TestCsvServiceBuilder;
                var result = fileBuilderManager.Generate(new TestCsvBuilderRequest(), builderName);

                var expectedText = builder.Data.CsvString.ToString();
                var expected = Convert.ToBase64String(Encoding.ASCII.GetBytes(expectedText));
                var csvText = Encoding.ASCII.GetString(Convert.FromBase64String(result.FileBase64));

                Assert.Equal(expectedText, csvText);
                Assert.Equal(expected, result.FileBase64);
            }
        }

    }
}