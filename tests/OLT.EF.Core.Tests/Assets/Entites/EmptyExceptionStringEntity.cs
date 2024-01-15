using OLT.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace OLT.EF.Core.Tests.Assets.Entites
{
    public class EmptyExceptionStringEntity : IOltEntity
    {
        [Key]
        [StringLength(50)]
        public string TableKey { get; set; } = default!;

        private string _title = "  ";
        public string Title
        {
            get { return _title; }
            set { throw new Exception("CheckNullableStringFields"); }  //This forces an exception from the empty string to null context processes
        }

        public static EmptyExceptionStringEntity FakerEntity()
        {
            return new EmptyExceptionStringEntity
            {
                TableKey = $"Key_{Guid.NewGuid()}",
            };
        }
    }

}
