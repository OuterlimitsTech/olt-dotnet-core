using OLT.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OLT.EF.Core.SqlServer.Tests.Assests.Entites
{
    public class PersonEntity : OltEntityIdDeletable
    {
        public string NameFirst { get; set; }
        public string NameMiddle { get; set; }
        public string NameLast { get; set; }
    }

}
