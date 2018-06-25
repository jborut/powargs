using PowArgs.Attributes;
using PowArgs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PowArgs
{
    public static class Parser<T> where T : new()
    {
        private const string ArgumentNameSwitch = "-";

        public static T Parse(string[] args, string argumentNameSwitch = ArgumentNameSwitch)
        {
            var obj = new T();

            Dictionary<string, PropArg> propArgs = PropArgMatcher<T>.MatchPropertirsAndArguments(obj);
            int requiredCount = propArgs.Count(propArg => propArg.Value.Argument.Required);

            int foundCounter = 0;
            int requiredCounter = 0;

            List<string> errors = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith(argumentNameSwitch)) // named argument
                {
                    string propName = args[i].Substring(1);

                    if (propArgs.ContainsKey(propName))
                    {
                        var prop = propArgs[propName].Property;

                        if (prop.PropertyType != typeof(bool) && i < args.Length - 1)
                        {
                            string argVal = args[i + 1];

                            if (SetValue(prop, obj, argVal, ref requiredCounter))
                            {
                                foundCounter++;
                                i++;
                                continue;
                            }
                            else
                            {
                                errors.Add($"Error assigning value of {argVal} to property {propName}");
                            }
                        }
                        else if (prop.PropertyType == typeof(bool))
                        {
                            // peek next entry
                            string val = (i < args.Length - 1 && !args[i + 1].StartsWith(argumentNameSwitch)) ? args[i + 1] : "true";
                            if (SetValue(prop, obj, val, ref requiredCounter))
                            {
                                foundCounter++;
                                if (i < args.Length - 1 && !args[i + 1].StartsWith(argumentNameSwitch))
                                {
                                    i++;
                                }
                            }
                            else
                            {
                                errors.Add($"Error assigning value of {val} to property {propName}");
                            }

                            continue;
                        }
                    }
                    else
                    {
                        errors.Add($"Unknown named property {args[i].Substring(1)}");
                    }
                }
                else // position argument
                {
                    var prop = propArgs.ElementAt(foundCounter).Value.Property;

                    if (SetValue(prop, obj, args[i], ref requiredCounter))
                    {
                        foundCounter++;
                    }
                    else
                    {
                        errors.Add($"Error assigning value of {args[i]} to property {args[i].Substring(1)}");
                    }
                }
            }

            if (requiredCounter != requiredCount)
            {
                throw new Exception("Error parsing argument: not all required arguments found.");
            }

            if (errors.Count() > 0)
            {
                throw new Exception("Error parsing arguments: not all properties could be set\n" + string.Join("\n", errors));
            }

            return obj;
        }

        private static bool IsRequired(PropertyInfo property)
        {
            return (((ArgumentAttribute)property.GetCustomAttributes(typeof(ArgumentAttribute), false).Single()).Required);
        }

        private static bool SetValue(PropertyInfo property, T obj, string arg, ref int requiredCounter)
        {
            bool set = false;
            try
            {
                if (property.PropertyType == typeof(int) && Int32.TryParse(arg, out int val1))
                {
                    property.SetValue(obj, val1);
                    set = true;
                }
                else if (property.PropertyType == typeof(float) && float.TryParse(arg, out float val2))
                {
                    property.SetValue(obj, val2);
                    set = true;
                }
                else if (property.PropertyType == typeof(decimal) && decimal.TryParse(arg, out decimal val3))
                {
                    property.SetValue(obj, val3);
                    set = true;
                }
                else if (property.PropertyType == typeof(bool) && bool.TryParse(arg, out bool val4))
                {
                    property.SetValue(obj, val4);
                    set = true;
                }
                else if (property.PropertyType == typeof(string))
                {
                    property.SetValue(obj, arg);
                    set = true;
                }
            }
            catch
            {
                return false;
            }

            if (IsRequired(property))
            {
                requiredCounter++;
            }

            return set;
        }
    }
}
