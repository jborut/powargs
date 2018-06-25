using PowArgs.Attributes;
using PowArgs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PowArgs
{
    static class PropArgMatcher<T> where T : new()
    {
        public static Dictionary<string, PropArg> MatchPropertirsAndArguments(T obj)
        {
            SortedDictionary<string, PropArg> result = new SortedDictionary<string, PropArg>();

            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                if (property.GetCustomAttribute(typeof(ArgumentAttribute)) is ArgumentAttribute argument)
                {
                    argument.PropName = property.Name;
                    result.Add(property.Name, new PropArg(property, argument));
                }
            }

            // make sure that we have them in same order as they are defined in class
            List<KeyValuePair<string, PropArg>> orderedList = result.ToList();
            orderedList.Sort((pair1, pair2) => pair1.Value.Argument.Order.CompareTo(pair2.Value.Argument.Order));

            return orderedList.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
