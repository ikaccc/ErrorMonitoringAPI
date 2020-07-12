
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace ExceptionHandler.DataAccess
{
    public static class ExtensionMethods
    {
        internal static IEnumerable<T> ToT<T>(this IEnumerable<IDataRecord> records)
            where T : new()
        {
            foreach (IDataRecord dr in records)
            {
                //Get list of all the names
                var Names = Enumerable.Range(0, dr.FieldCount).Select(x => dr.GetName(x));

                //Declare T
                T toReturn = new T();

                //Declare Type of T
                Type GenericType = typeof(T);

                //Declare List that holds all the properties that are implemented
                List<PropertyInfo> props = new List<PropertyInfo>();

                //Add the Properties of the type itself
                props.AddRange(GenericType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

                //Add the Properties of any interface that the type implements
                foreach (Type InterfaceType in GenericType.GetInterfaces())
                    props.AddRange(InterfaceType.GetProperties());

                //For each property which has a ResultParameter in props
                foreach (PropertyInfo pi in props
                            .Where(p => Attribute.IsDefined(p, typeof(ResultParameter))))
                {
                    //Extract the ResultParameter

                    ResultParameter rp = pi.GetCustomAttribute<ResultParameter>();
                    var a = dr[rp.Name].GetType();
                    //Fill value either by Name or by Column
                    if (rp.IsName)
                        if (Names.Contains(rp.Name))
                            pi.SetValue(toReturn, Convert.IsDBNull(dr[rp.Name]) ? null : dr[rp.Name]);
                        else
                            System.Diagnostics.Debug.WriteLine(string.Format("Parameter {0} was skipped", rp.Name));
                    else

                        pi.SetValue(toReturn, dr[rp.Column]);
                }
                System.Diagnostics.Debug.Flush();
                yield return toReturn;
            }
        }

        internal static T ToT<T>(this IDataRecord record)
            where T : new()
        {
            //Get list of all the names
            var Names = Enumerable.Range(0, record.FieldCount).Select(x => record.GetName(x));

            //Declare T
            T toReturn = new T();

            //Declare Type of T
            Type GenericType = typeof(T);

            //Declare List that holds all the properties that are implemented
            List<PropertyInfo> props = new List<PropertyInfo>();

            //Add the Properties of the type itself
            props.AddRange(GenericType.GetProperties());

            //Add the Properties of any interface that the type implements
            foreach (Type InterfaceType in GenericType.GetInterfaces())
                props.AddRange(InterfaceType.GetProperties());

            //For each property which has a ResultParameter in props
            foreach (PropertyInfo pi in props
                        .Where(p => Attribute.IsDefined(p, typeof(ResultParameter))))
            {
                //Extract the ResultParameter

                ResultParameter rp = pi.GetCustomAttribute<ResultParameter>();


                //Fill value either by Name or by Column
                if (rp.IsName)
                    if (Names.Contains(rp.Name))
                        pi.SetValue(toReturn, record[rp.Name]);
                    else
                        System.Diagnostics.Debug.WriteLine(string.Format("Parameter {0} was skipped", rp.Name));
                else
                    pi.SetValue(toReturn, record[rp.Column]);
            }
            System.Diagnostics.Debug.Flush();
            return toReturn;
        }

        internal static bool HasColumn(this IDataRecord record, string name)
        {
            return Enumerable.Range(0, record.FieldCount).Select(x => record.GetName(x)).Contains(name);
        }

        /// <summary>
        /// Gets the structured parameter.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>DB Parameter that can pass a data table</returns>
        public static DbParameter GetStructuredParameter(this IDataBaseProvider provider, string name, DataTable value)
        {
            return new SqlParameter { ParameterName = name, SqlDbType = SqlDbType.Structured, Value = value };
        }

        /// <summary>
        /// Converts item to the specified type
        /// </summary>
        /// <typeparam name="T">Type to convert to</typeparam>
        /// <param name="itm">This convertible item</param>
        /// <returns>Item as the specified type</returns>
        public static T ToT<T>(this IConvertible itm) where T : IConvertible
        {
            return (T)((itm == null)
                       ? default(T)
                       : System.Convert.ChangeType(itm, typeof(T)));
        }

        /// <summary>
        /// Reads a value and converts it to the given type
        /// </summary>
        /// <typeparam name="T">Type to be converted to</typeparam>
        /// <param name="reader">The sql data reader to be read</param>
        /// <param name="column">Column in the reader</param>
        /// <param name="defaultValue">The default value if null</param>
        /// <returns>The converted value</returns>
        public static T Read<T>(this System.Data.SqlClient.SqlDataReader reader, string column, T defaultValue = default(T)) where T : IConvertible
        {
            var value = reader[column];

            return (T)((DBNull.Value.Equals(value))
                       ? defaultValue
                       : System.Convert.ChangeType(value, typeof(T)));
        }

        /// <summary>
        /// Reads a value and converts it to the given type
        /// </summary>
        /// <typeparam name="T">Type to be converted to</typeparam>
        /// <param name="reader">The data record to be read</param>
        /// <param name="column">Column in the reader</param>
        /// <param name="defaultValue">The default value if null</param>
        /// <returns>The converted value</returns>
        public static T Read<T>(this System.Data.IDataRecord reader, string column, T defaultValue = default(T)) where T : IConvertible
        {
            var value = reader[column];

            return (T)((DBNull.Value.Equals(value))
                       ? defaultValue
                       : System.Convert.ChangeType(value, typeof(T)));
        }

        /// <summary>
        /// Reads a value and converts it to the given type
        /// </summary>
        /// <typeparam name="T">Type to be converted to</typeparam>
        /// <param name="reader">The data record to be read</param>
        /// <param name="column">Column in the reader</param>
        /// <param name="defaultValue">The default value if null</param>
        /// <returns>The converted value</returns>
        public static Guid ReadGuid<Guid>(this System.Data.IDataRecord reader, string column)
        {
            var value = reader[column];

            return (Guid)((DBNull.Value.Equals(value))
                ? default(Guid)
                : System.Convert.ChangeType(value, typeof(Guid)));
        }

        /// <summary>
        /// Returns distinct elements from a sequence by using the struct equality comparer to compare values.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the elements of item to filter by.</typeparam>
        /// <param name="list">The sequence to remove duplicate elements from.</param>
        /// <param name="lookup">The item to filter by.</param>
        /// <returns>An IEnumerable<T> that contains distinct elements from the source sequence.</returns>
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> list, Func<T, TKey> lookup) where TKey : struct
        {
            return list.Distinct(new StructEqualityComparer<T, TKey>(lookup));
        }
    }

    /// <summary>
    /// Compares classes by their internal structure
    /// </summary>
    /// <typeparam name="T">Class type</typeparam>
    /// <typeparam name="TKey">Property to compare</typeparam>
    internal class StructEqualityComparer<T, TKey> : IEqualityComparer<T> where TKey : struct
    {
        /// <summary>
        /// Function to extract the value of the chosen property
        /// </summary>
        private Func<T, TKey> lookup;

        /// <summary>
        /// Initializes the class
        /// </summary>
        /// <param name="lookup">The function to extract the property</param>
        public StructEqualityComparer(Func<T, TKey> lookup)
        {
            this.lookup = lookup;
        }

        /// <summary>
        /// Compares the two classes in accordance to the value
        /// </summary>
        /// <param name="x">First Class to compare</param>
        /// <param name="y">Second Class to Compare</param>
        /// <returns>If classes are equal or not</returns>
        public bool Equals(T x, T y)
        {
            return lookup(x).Equals(lookup(y));
        }

        /// <summary>
        /// Returns the hash code of the chosen property of the object
        /// </summary>
        /// <param name="obj">Object to get hash code of the property from</param>
        /// <returns>Hash Code of property</returns>
        public int GetHashCode(T obj)
        {
            return lookup(obj).GetHashCode();
        }
    }
}
