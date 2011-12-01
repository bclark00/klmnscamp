using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Klmsncamp.ViewModels;

namespace Klmsncamp.DAL
{
    public static class KlmsnExtensions
    {
        public static IEnumerable<string> EnumeratePropertyDifferences<T>(this T obj1, T obj2)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<string> changes = new List<string>();

            foreach (PropertyInfo pi in properties)
            {
                object value1 = typeof(T).GetProperty(pi.Name).GetValue(obj1, null);
                object value2 = typeof(T).GetProperty(pi.Name).GetValue(obj2, null);

                if (value1 != value2 && (value1 == null || !value1.Equals(value2)))
                {
                    changes.Add(string.Format("Property {0} changed from {1} to {2}", pi.Name, value1, value2));
                }
            }
            return changes;
        }

        public static bool BoolPropertyDifferences<T>(this T obj1, T obj2)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo pi in properties)
            {
                if (pi.PropertyType.IsValueType || pi.PropertyType.Name == "String")
                {
                    object value1 = typeof(T).GetProperty(pi.Name).GetValue(obj1, null);
                    object value2 = typeof(T).GetProperty(pi.Name).GetValue(obj2, null);

                    if (value1 != value2 && ((value1 == null || !value1.Equals(value2)) || (value2 == null || !value2.Equals(value1))))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static List<LogRequestIssueViewModel> LogPropertyDifferences<T>(this T obj1, T obj2)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var LogProps = new List<LogRequestIssueViewModel>();
            foreach (PropertyInfo pi in properties)
            {
                if (pi.PropertyType.IsValueType || pi.PropertyType.Name == "String")
                {
                    object value1 = typeof(T).GetProperty(pi.Name).GetValue(obj1, null);
                    object value2 = typeof(T).GetProperty(pi.Name).GetValue(obj2, null);

                    //if (value1 != value2 && (value1 == null || !value1.Equals(value2)))
                    if (value1 != value2 && ((value1 == null || !value1.Equals(value2)) || (value2 == null || !value2.Equals(value1))))
                    {
                        if (value1 == null)
                        {
                            LogProps.Add(new LogRequestIssueViewModel { PropertyName = pi.Name, OldValue = "--", NewValue = typeof(T).GetProperty(pi.Name).GetValue(obj2, null).ToString() });
                        }
                        else if (value2 == null)
                        {
                            LogProps.Add(new LogRequestIssueViewModel { PropertyName = pi.Name, OldValue = typeof(T).GetProperty(pi.Name).GetValue(obj1, null).ToString(), NewValue = "--" });
                        }
                        else
                        {
                            LogProps.Add(new LogRequestIssueViewModel { PropertyName = pi.Name, OldValue = typeof(T).GetProperty(pi.Name).GetValue(obj1, null).ToString(), NewValue = typeof(T).GetProperty(pi.Name).GetValue(obj2, null).ToString() });
                        }
                    }
                }
            }
            return LogProps;
        }
    }
}