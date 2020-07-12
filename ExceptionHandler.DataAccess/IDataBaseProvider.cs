using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace ExceptionHandler.DataAccess
{
    /// <summary>
    /// Manages the Database Provider
    /// </summary>
    public interface IDataBaseProvider
    {
        #region "Transaction Factory"

        /// <summary>
        /// Creates the transaction.
        /// </summary>
        void CreateTransaction();

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls the back transaction.
        /// </summary>
        void RollBackTransaction();

        #endregion "Transaction Factory"

        #region "Parameter Factory"

        /// <summary>
        /// Gets the empty parameter.
        /// </summary>
        /// <returns>Empty DB Parameter</returns>
        DbParameter GetEmptyParameter();

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>DB Parameter</returns>
        DbParameter GetParameter(string name);

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>DB Parameter</returns>
        DbParameter GetParameter(string name, object value);

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>DB Parameters</returns>
        DbParameter GetParameter(string name, ParameterDirection direction);

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <returns>DB Parameter</returns>
        DbParameter GetParameter(string name, DbType type);

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>DB Parameter</returns>
        DbParameter GetParameter(string name, DbType type, ParameterDirection direction);

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="size">The size.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>DB Parameter</returns>
        DbParameter GetParameter(string name, DbType type, short size, ParameterDirection direction);

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>DB Parameter</returns>
        DbParameter GetParameter(string name, DbType type, object value);

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="Assign">The method that assigns values to the parameter.</param>
        /// <returns>DB Parameter</returns>
        DbParameter GetParameter(Action<DbParameter> Assign);

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The Parameters</returns>
        IEnumerable<DbParameter> GetParameters(IEnumerable<KeyValuePair<string, object>> items);

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="items">The class to extract from</param>
        /// <returns>The Parameters</returns>
        IEnumerable<DbParameter> GetParameters<T>(T item) where T : class;

        #endregion "Parameter Factory"

        #region "Connection Methods"

        /// <summary>
        /// Opens the connection.
        /// </summary>
        void OpenConnection();

        /// <summary>
        /// Closes the connection.
        /// </summary>
        void CloseConnection();

        #endregion "Connection Methods"

        #region "Data Access Methods"

        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>Data Reader for the SQL Statement</returns>
        [Obsolete("Use GetData instead")]
        IDataReader GetDataReader(string sqlStatement, params DbParameter[] Parameters);

        /// <summary>
        /// Executes the procedure with result set.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>Data Reader for the Stored Procedure</returns>
        [Obsolete("Use ExecuteProcedureWithResult instead")]
        IDataReader ExecuteProcedureWithResultSet(string procedureName, params DbParameter[] Parameters);

        IDataRecord[] GetData(string sqlStatement, params DbParameter[] Parameters);
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>The array of data for the SQL Statement</returns>
        Task<IDataRecord[]> GetDataAsync(string sqlStatement, params DbParameter[] Parameters);

        /// <summary>
        /// Executes the procedure with result.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>The array of data for the Stored Procedure</returns>
        IDataRecord[] ExecuteProcedureWithResult(string procedureName, params DbParameter[] Parameters);

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>The array of data for the SQL Statement</returns>
        Task<T[]> GetDataAsync<T>(string sqlStatement, params DbParameter[] Parameters) where T : new();


        T[] GetData<T>(string sqlStatement, params DbParameter[] Parameters) where T : new();

        /// <summary>
        /// Executes the procedure with result.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>The array of data for the Stored Procedure</returns>
        T[] ExecuteProcedureWithResult<T>(string procedureName, params DbParameter[] Parameters) where T : new();

        Task<IDataRecord[]> ExecuteProcedureWithResultAsync(string procedureName, params DbParameter[] Parameters);

        /// <summary>
        /// Executes the procedure.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>The number of Effected Rows</returns>
        int ExecuteProcedure(string procedureName, params DbParameter[] Parameters);

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of Effected Rows</returns>
        int ExecuteNonQuery(string sqlStatement, params DbParameter[] parameters);

        Task<int> ExecuteNonQueryAsync(string sqlStatement, params DbParameter[] parameters);

        /// <summary>
        /// Gets the scalar.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The scalar object</returns>
        [Obsolete("Use GetScalar<T> to avoid needless conversions")]
        object GetScalar(string sqlStatement, params DbParameter[] parameters);

        /// <summary>
        /// Gets the scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar</typeparam>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The Scalar Item</returns>
        T GetScalar<T>(string sqlStatement, params DbParameter[] parameters) where T : IConvertible;

        Task<T> GetScalarAsync<T>(string sqlStatement, T defaultValue, params DbParameter[] parameters) where T : IConvertible;
        /// <summary>
        /// Gets the scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar</typeparam>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The Scalar Item</returns>
        T GetScalar<T>(string sqlStatement, T defaultValue, params DbParameter[] parameters) where T : IConvertible;

        /// <summary>
        /// Executes the procedure with scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar</typeparam>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The Scalar Item</returns>
        T ExecuteProcedureWithScalar<T>(string sqlStatement, params DbParameter[] parameters) where T : IConvertible;

        Task<T> ExecuteProcedureWithScalarAsync<T>(string sqlStatement, params DbParameter[] parameters)
            where T : IConvertible;

        /// <summary>
        /// Executes the procedure with scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar</typeparam>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The Scalar Item</returns>
        T ExecuteProcedureWithScalar<T>(string sqlStatement, T defaultValue, params DbParameter[] parameters) where T : IConvertible;

        /// <summary>
        /// Executes the procedure with scalar.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="parametersList">The parameters list.</param>
        /// <returns>The Scalar Item</returns>
        T ExecuteProcedureWithScalar<T>(string sqlStatement, T defaultValue = default(T), IEnumerable<DbParameter> parametersList = null) where T : IConvertible;

        #endregion "Data Access Methods"
    }
}