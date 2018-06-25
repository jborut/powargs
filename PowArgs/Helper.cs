using PowArgs.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PowArgs
{
    public static class Helper<T> where T : new()
    {
        public static string[] GetHelpText()
        {
            T obj = new T();

            List<string> helpLines = new List<string>();

            Dictionary<string, PropArg> propArgs = PropArgMatcher<T>.MatchPropertirsAndArguments(obj);

            foreach (var propArg in propArgs)
            {
                string line = string.Empty;

                if (!propArg.Value.Argument.Required)
                {
                    line += "[";
                }

                line += "-" + propArg.Key + " <";

                if (propArg.Value.Property.PropertyType == typeof(int))
                {
                    line += "int";
                }
                else if (propArg.Value.Property.PropertyType == typeof(float))
                {
                    line += "float";
                }
                else if (propArg.Value.Property.PropertyType == typeof(decimal))
                {
                    line += "decimal";
                }
                else if (propArg.Value.Property.PropertyType == typeof(string))
                {
                    line += "string";
                }
                else if (propArg.Value.Property.PropertyType == typeof(bool))
                {
                    line += "bool";
                }

                line += ">";

                if (propArg.Value.Property.GetValue(obj) != null)
                {
                    line += " (" + propArg.Value.Property.GetValue(obj).ToString() + ")";
                }

                if (!propArg.Value.Argument.Required)
                {
                    line += "]";
                }

                line = line.PadRight(40);

                line += propArg.Value.Argument.Description;

                helpLines.Add(line);
            }

            return helpLines.ToArray();
        }
    }
}
