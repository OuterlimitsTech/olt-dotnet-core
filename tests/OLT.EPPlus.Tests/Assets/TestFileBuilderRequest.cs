using OLT.Core;
using System;
using System.Collections.Generic;

namespace OLT.EPPlus.Tests.Assets
{
    [Obsolete]
    public class TestFileBuilderRequest : IOltRequest
    {
        public List<PersonModel> Data { get; set; } = new List<PersonModel>();
    }
}