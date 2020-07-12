using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace ExceptionHandler.DataAccess
{
    /// <summary>
    /// Manages auto mapping from type to DbType
    /// </summary>
    public static class DataMapping
    {
        /// <summary>
        /// The type map
        /// </summary>
        private static ReadOnlyDictionary<Type, DbType> typeMap;

        /// <summary>
        /// Gets the type map.
        /// </summary>
        /// <value>
        /// The type map.
        /// </value>
        public static ReadOnlyDictionary<Type, DbType> TypeMap
        {
            get
            {
                if (typeMap == null)
                {
                    Dictionary<Type, DbType> _typeMap = new Dictionary<Type, DbType>
                    {
                        [typeof(byte)] = DbType.Byte,
                        [typeof(sbyte)] = DbType.SByte,
                        [typeof(short)] = DbType.Int16,
                        [typeof(ushort)] = DbType.UInt16,
                        [typeof(int)] = DbType.Int32,
                        [typeof(uint)] = DbType.UInt32,
                        [typeof(long)] = DbType.Int64,
                        [typeof(ulong)] = DbType.UInt64,
                        [typeof(float)] = DbType.Single,
                        [typeof(double)] = DbType.Double,
                        [typeof(decimal)] = DbType.Decimal,
                        [typeof(bool)] = DbType.Boolean,
                        [typeof(string)] = DbType.String,
                        [typeof(char)] = DbType.StringFixedLength,
                        [typeof(Guid)] = DbType.Guid,
                        [typeof(DateTime)] = DbType.DateTime,
                        [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
                        [typeof(byte[])] = DbType.Binary,
                        [typeof(byte?)] = DbType.Byte,
                        [typeof(sbyte?)] = DbType.SByte,
                        [typeof(short?)] = DbType.Int16,
                        [typeof(ushort?)] = DbType.UInt16,
                        [typeof(int?)] = DbType.Int32,
                        [typeof(uint?)] = DbType.UInt32,
                        [typeof(long?)] = DbType.Int64,
                        [typeof(ulong?)] = DbType.UInt64,
                        [typeof(float?)] = DbType.Single,
                        [typeof(double?)] = DbType.Double,
                        [typeof(decimal?)] = DbType.Decimal,
                        [typeof(bool?)] = DbType.Boolean,
                        [typeof(char?)] = DbType.StringFixedLength,
                        [typeof(Guid?)] = DbType.Guid,
                        [typeof(DateTime?)] = DbType.DateTime,
                        [typeof(DateTimeOffset?)] = DbType.DateTimeOffset
                    };
                    typeMap = new ReadOnlyDictionary<Type, DbType>(_typeMap);
                }
                return typeMap;
            }
        }

        /// <summary>
        /// Maps the type.
        /// </summary>
        /// <param name="toMap">To map.</param>
        /// <returns>The DbType that is mapped to the current type</returns>
        public static DbType MapType(Type toMap)
        {
            return TypeMap[toMap];
        }
    }
}
