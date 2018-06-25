using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PowArgs.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ArgumentAttribute : Attribute
    {
        public string PropName { get; set; }
        public string Description { get; private set; }
        public bool Required { get; private set; }
        public int Order { get; private set; }

        public ArgumentAttribute(string description, bool required = false, [CallerLineNumber]int order = 0)
        {
            Description = description;
            Required = required;
            Order = order;
        }
    }
}
