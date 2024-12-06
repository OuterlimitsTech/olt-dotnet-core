//using System;
//using System.ComponentModel;

//namespace OLT.Extensions.General.Tests
//{
//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
//    public class CustomTestAttribute : Attribute
//    {
//        public string Value { get; private set; }


//        private CustomTestAttribute() { }
//        public CustomTestAttribute(string value)
//        {
//            this.Value = value;
//        }
//    }

//    public class TestAttributeClassBase
//    {
//        [Description("Base Class Value1")]
//        [CustomTest("Value1 - Item1")]
//        [CustomTest("Value1 - Item2")]
//        [CustomTest("Value1 - Item3")]
//        public virtual string Value1 { get; set; }

//        [Description("Base Class Value2")]
//        [CustomTest("Value2 - Item1")]
//        [CustomTest("Value2 - Item2")]
//        public virtual string Value2 { get; set; }

//        [Description("Base Class Value3")]
//        [CustomTest("Value3 - Item1")]
//        [CustomTest("Value3 - Item2")]
//        public virtual string Value3 { get; set; }

//        public virtual string Value4 { get; set; }
//    }

//    public class TestAttributeClass : TestAttributeClassBase
//    {
//        [Description("Test Class Value1")]
//        [CustomTest("Value1 - Item800")]
//        [CustomTest("Value1 - Item900")]
//        public override string Value1 { get; set; }

//        public override string Value2 { get; set; }

//        public string Value5 { get; set; }
//    }
//}