using OLT.Core;
using System;
using System.Text;

namespace OLT.Builder.File.Tests.Assets
{
    public class TestCsvServiceBuilder : OltFileBuilderService
    {
        public TestCsvServiceBuilder()
        {
            var csvString = new StringBuilder();
            csvString.AppendLine("\"PersonId\",\"FirstName\",\"LastName\"");
            PersonModel.FakerList(3).ForEach(person =>
            {
                csvString.AppendLine($"\"{person.Email}\",\"{person.First}\",\"{person.Last}\"");

            });
            Data = new TestCsvData(csvString);
        }

        public override string BuilderName => nameof(TestCsvServiceBuilder);
        public string FileName => $"{BuilderName} [{DateTimeOffset.Now:s}].csv";
        public TestCsvData Data { get; }

        public override IOltFileBase64 Build<TRequest>(TRequest request)
        {
            return new OltFileBase64
            {
                FileName = this.FileName,
                ContentType = MimeMapping.MimeUtility.GetMimeMapping(this.FileName),
                FileBase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(Data.CsvString.ToString()))
            };
        }
    }
}
