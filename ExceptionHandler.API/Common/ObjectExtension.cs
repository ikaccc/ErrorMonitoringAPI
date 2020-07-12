using System;
using System.Reflection;
using System.Text;

namespace ExceptionHandler.API.Common
{
    public static class ObjectExtension
    {
        private const string defaultIndentation = "  ";

        public static string RenderAsString(this object obj, string indentation = null)
        {
            return obj.RenderAsString(indentation, new StringBuilder());
        }

        private static string RenderAsString(this object obj, string indentation, StringBuilder stringBuilder)
        {
            if (stringBuilder == null)
                stringBuilder = new StringBuilder();
            Type type = obj.GetType();
            stringBuilder.AppendLine((indentation ?? string.Empty) + type.FullName + ":");
            indentation = !string.IsNullOrEmpty(indentation) ? indentation + indentation : "  ";
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.FlattenHierarchy))
            {
                if (property.PropertyType.IsPrimitive)
                    stringBuilder.AppendLine(string.Format("{0}{1} = {2}", (object)indentation, (object)property.Name, property.GetValue(obj)));
                else
                    stringBuilder.Append(property.GetValue(obj).RenderAsString(indentation, stringBuilder));
            }
            return stringBuilder.ToString();
        }
    }
}
