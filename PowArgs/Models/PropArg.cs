using PowArgs.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PowArgs.Models
{
    sealed class PropArg
    {
        public PropertyInfo Property { get; private set; }
        public ArgumentAttribute Argument { get; private set; }

        public PropArg(PropertyInfo propInfo, ArgumentAttribute argAttribute)
        {
            Property = propInfo;
            Argument = argAttribute;
        }
    }
}
