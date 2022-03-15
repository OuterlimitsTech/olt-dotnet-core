using OLT.Core;
using System.Collections.Generic;

namespace OLT.EPPlus.Tests.Assets
{
    public class TestFileBuilderRequest : IOltRequest
    {
        public List<PersonModel> Data { get; set; } = new List<PersonModel>();
    }
}