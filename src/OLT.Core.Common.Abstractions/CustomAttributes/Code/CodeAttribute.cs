﻿using OLT.Constants;
using System;
using System.Linq;
using System.Reflection;

namespace OLT.Core
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class CodeAttribute : Attribute
    {
        public string Code { get; private set; }

        public CodeAttribute(string code)
        {
            this.Code = code;
        }
    }
}