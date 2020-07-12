using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ExceptionHandler.DataAccess
{
    public class SqlAccessLayer : IDataBaseProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlAccessLayer"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        private SqlAccessLayer(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            Transaction = null;
        }

        #region "Properties"

        /// <summary>
        /// The transaction
        /// </summary>
        internal SqlTransaction Transaction;

        /// <summary>
        /// The connection
        /// </summary>
        internal SqlConnection Connection;

        #endregion "Properties"

        #region "Provider Factory"

        /// <summary>
        /// Creates the database provider.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static IDataBaseProvider CreateDatabaseProvider(string connectionString)
        {
            return new SqlAccessLayer(connectionString);
        }

        #endregion "Provider Factory"

        #region "Transaction Factory"

        /// <summary>
        /// Creates the transaction.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Transaction is not committed or rolled back</exception>
        public void CreateTransaction()
        {
            if (Transaction != null) throw new InvalidOperationException("Transaction is not committed or rolled back");
            Transaction = Connection.BeginTransaction();
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            if (Transaction != null) Transaction.Commit();
            Transaction = null;
        }

        /// <summary>
        /// Rolls the back transaction.
        /// </summary>
        public void RollBackTransaction()
        {
            if (Transaction != null) Transaction.Rollback();
            Transaction = null;
        }

        /// <summary>
        /// Resets the transaction.
        /// </summary>
        public void ResetTransaction()
        {
            Transaction = null;
        }

        #endregion "Transaction Factory"

        #region "Parameter Factory"

        /// <summary>
        /// Gets the empty parameter.
        /// </summary>
        /// <returns>
        /// Empty DB Parameter
        /// </returns>
        public DbParameter GetEmptyParameter()
        {
            return new SqlParameter();
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// DB Parameter
        /// </returns>
        public DbParameter GetParameter(string name)
        {
            return new SqlParameter { ParameterName = name };
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// DB Parameter
        /// </returns>
        public DbParameter GetParameter(string name, object value)
        {
            return new SqlParameter { ParameterName = name, Value = value };
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>
        /// DB Parameters
        /// </returns>
        public DbParameter GetParameter(string name, ParameterDirection direction)
        {
            return new SqlParameter { ParameterName = name, Direction = direction };
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// DB Parameter
        /// </returns>
        public DbParameter GetParameter(string name, DbType type)
        {
            return new SqlParameter(name, type);
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>
        /// DB Parameter
        /// </returns>
        public DbParameter GetParameter(string name, DbType type, ParameterDirection direction)
        {
            return new SqlParameter { ParameterName = name, DbType = type, Direction = direction };
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="size">The size.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>
        /// DB Parameter
        /// </returns>
        public DbParameter GetParameter(string name, DbType type, short size, ParameterDirection direction)
        {
            return new SqlParameter { ParameterName = name, DbType = type, Size = size, Direction = direction };
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// DB Parameter
        /// </returns>
        public DbParameter GetParameter(string name, DbType type, object value)
        {
            return new SqlParameter { ParameterName = name, DbType = type, Value = value };
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="Assign">The method that assigns values to the parameter.</param>
        /// <returns>DB Parameter</returns>
        public DbParameter GetParameter(Action<DbParameter> Assign)
        {
            DbParameter toReturn = new SqlParameter();
            Assign(toReturn);
            return toReturn;
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// DB Parameter
        /// </returns>
        public IEnumerable<DbParameter> GetParameters(IEnumerable<KeyValuePair<string, object>> items)
        {
            return items.Select(x => GetParameter(x.Key, x.Value));
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="items">The class to extract from</param>
        /// <returns>The Parameters</returns>
        public IEnumerable<DbParameter> GetParameters<T>(T item)
            where T : class
        {
            return typeof(T)
                    .GetProperties()
                    .Where(p => Attribute.IsDefined(p, typeof(ProcedureParameter)))
                    .Select(pi =>
                    {

                        ProcedureParameter pp = pi.GetCustomAttribute<ProcedureParameter>();

                        DbParameter param = (pp.Output) ? GetParameter(pp.Name, ParameterDirection.Output) : GetParameter(pp.Name);

                        param.Value = pi.GetValue(item);

                        return param;
                    });
        }

        #endregion "Parameter Factory"

        #region "Connection Methods"

        /// <summary>
        /// Opens the connection.
        /// </summary>
        public void OpenConnection()
        {
            if (Connection != null) if (Connection.State == ConnectionState.Closed) Connection.Open();
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void CloseConnection()
        {
            if (Connection != null) if (Connection.State == ConnectionState.Open) Connection.Close();
        }

        #endregion "Connection Methods"

        #region "Data Access Methods"

        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>
        /// Data Reader for the SQL Statement
        /// </returns>
        [Obsolete("Use GetData Instead")]
        public IDataReader GetDataReader(string sqlStatement, params DbParameter[] Parameters)
        {
            try
            {
                SqlCommand _sqlCommand = Connection.CreateCommand();
                if (Transaction != null) _sqlCommand.Transaction = Transaction;
                _sqlCommand.CommandText = sqlStatement;
                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.Parameters.AddRange(Parameters);
                return _sqlCommand.ExecuteReader();
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
        }

        /// <summary>
        /// Executes the procedure with result set.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>
        /// Data Reader for the Stored Procedure
        /// </returns>
        [Obsolete("Use ExecuteProcedureWithResult instead")]
        public IDataReader ExecuteProcedureWithResultSet(string procedureName, params DbParameter[] Parameters)
        {
            //instantiate new command object
            IDbCommand _sqlCommand = Connection.CreateCommand();

            //instantiate the result
            IDataReader _resultSets;

            //check if the command needs a transaction
            if (!(Transaction == null))
            {
                _sqlCommand.Transaction = Transaction;
            }

            try
            {
                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = procedureName;

                foreach (IDbDataParameter _parameter in Parameters)
                {
                    //add the parameters to the parameters collection of the command object
                    _sqlCommand.Parameters.Add(_parameter);
                }

                //execute the query without returning results
                _resultSets = _sqlCommand.ExecuteReader();
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }

            return _resultSets;
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>
        /// The array of data for the SQL Statement
        /// </returns>
        public async Task<IDataRecord[]> GetDataAsync(string sqlStatement, params DbParameter[] Parameters)
        {
            try
            {
                SqlCommand _sqlCommand = Connection.CreateCommand();
                if (Transaction != null) _sqlCommand.Transaction = Transaction;
                _sqlCommand.CommandText = sqlStatement;
                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.Parameters.AddRange(Parameters);
                var sqlComandRes = await _sqlCommand.ExecuteReaderAsync();
                return sqlComandRes.OfType<IDataRecord>().ToArray();
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
        }

        public IDataRecord[] GetData(string sqlStatement, params DbParameter[] Parameters)
        {
            try
            {
                SqlCommand _sqlCommand = Connection.CreateCommand();
                if (Transaction != null) _sqlCommand.Transaction = Transaction;
                _sqlCommand.CommandText = sqlStatement;
                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.Parameters.AddRange(Parameters);
                return _sqlCommand.ExecuteReader().OfType<IDataRecord>().ToArray();
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
        }

        /// <summary>
        /// Executes the procedure with result.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>
        /// The array of data for the Stored Procedure
        /// </returns>
        public IDataRecord[] ExecuteProcedureWithResult(string procedureName, params DbParameter[] Parameters)
        {
            //instantiate new command object
            SqlCommand _sqlCommand = Connection.CreateCommand() as SqlCommand;

            //instantiate the result
            SqlDataReader _resultSets;

            //check if the command needs a transaction
            if (!(Transaction == null))
            {
                _sqlCommand.Transaction = Transaction;
            }

            try
            {
                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = procedureName;

                foreach (IDbDataParameter _parameter in Parameters)
                {
                    //add the parameters to the parameters collection of the command object
                    _sqlCommand.Parameters.Add(_parameter);
                }

                //execute the query without returning results
                _resultSets = _sqlCommand.ExecuteReader();
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }

            return _resultSets.OfType<IDataRecord>().ToArray();
        }

        public async Task<IDataRecord[]> ExecuteProcedureWithResultAsync(string procedureName, params DbParameter[] Parameters)
        {
            //instantiate new command object
            SqlCommand _sqlCommand = Connection.CreateCommand() as SqlCommand;

            //instantiate the result
            SqlDataReader _resultSets;

            //check if the command needs a transaction
            if (!(Transaction == null))
            {
                _sqlCommand.Transaction = Transaction;
            }

            try
            {
                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = procedureName;

                foreach (IDbDataParameter _parameter in Parameters)
                {
                    //add the parameters to the parameters collection of the command object
                    _sqlCommand.Parameters.Add(_parameter);
                }

                //execute the query without returning results
                _resultSets = await _sqlCommand.ExecuteReaderAsync();
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }

            return _resultSets.OfType<IDataRecord>().ToArray();
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>
        /// The array of data for the SQL Statement
        /// </returns>
        public async Task<T[]> GetDataAsync<T>(string sqlStatement, params DbParameter[] Parameters) where T : new()
        {
            try
            {
                SqlCommand _sqlCommand = Connection.CreateCommand();
                if (Transaction != null) _sqlCommand.Transaction = Transaction;
                _sqlCommand.CommandText = sqlStatement;
                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.Parameters.AddRange(Parameters);
                var sqlCommandRes = await _sqlCommand.ExecuteReaderAsync();
                return sqlCommandRes.OfType<IDataRecord>().ToT<T>().ToArray();
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
        }

        public T[] GetData<T>(string sqlStatement, params DbParameter[] Parameters) where T : new()
        {
            try
            {
                SqlCommand _sqlCommand = Connection.CreateCommand();
                if (Transaction != null) _sqlCommand.Transaction = Transaction;
                _sqlCommand.CommandText = sqlStatement;
                _sqlCommand.CommandType = CommandType.Text;
                _sqlCommand.Parameters.AddRange(Parameters);
                return _sqlCommand.ExecuteReader().OfType<IDataRecord>().ToT<T>().ToArray();
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
        }

        /// <summary>
        /// Executes the procedure with result.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>
        /// The array of data for the Stored Procedure
        /// </returns>
        public T[] ExecuteProcedureWithResult<T>(string procedureName, params DbParameter[] Parameters) where T : new()
        {
            //instantiate new command object
            SqlCommand _sqlCommand = Connection.CreateCommand() as SqlCommand;

            //instantiate the result
            SqlDataReader _resultSets;

            //check if the command needs a transaction
            if (!(Transaction == null))
            {
                _sqlCommand.Transaction = Transaction;
            }

            try
            {
                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = procedureName;

                foreach (IDbDataParameter _parameter in Parameters)
                {
                    //add the parameters to the parameters collection of the command object
                    _sqlCommand.Parameters.Add(_parameter);
                }

                //execute the query without returning results
                _resultSets = _sqlCommand.ExecuteReader();
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }

            return _resultSets.OfType<IDataRecord>().ToT<T>().ToArray();
        }

        /// <summary>
        /// Executes the procedure.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>
        /// The number of Effected Rows
        /// </returns>
        public int ExecuteProcedure(string procedureName, params DbParameter[] Parameters)
        {
            //instantiate new command object
            IDbCommand _sqlCommand = Connection.CreateCommand();

            //instantiate the result
            int _result;

            //check if the command needs a transaction
            if (Transaction != null) _sqlCommand.Transaction = Transaction;

            try
            {
                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = procedureName;

                foreach (IDbDataParameter _parameter in Parameters)
                {
                    //add the parameters to the parameters collection of the command object
                    _sqlCommand.Parameters.Add(_parameter);
                }

                //execute the query without returning results
                _result = _sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                RollBackTransaction();
                throw;
            }
            finally
            {
                _sqlCommand.Parameters.Clear();
                //dispose of the command object
                _sqlCommand.Dispose();
            }

            return _result;
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The number of Effected Rows
        /// </returns>
        public int ExecuteNonQuery(string sqlStatement, params DbParameter[] parameters)
        {
            //instantiate new command object
            SqlCommand _sqlCommand = Connection.CreateCommand();

            //instantiate the result
            int _result;

            //check if the command needs a transaction
            if (Transaction != null) _sqlCommand.Transaction = Transaction;

            try
            {
                //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = sqlStatement;

                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.Text;

                //add the parameters to the parameters collection of the command object
                _sqlCommand.Parameters.AddRange(parameters);

                //execute the query and assign the number of effected rows to the result
                _result = _sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }

            return _result;
        }

        public async Task<int> ExecuteNonQueryAsync(string sqlStatement, params DbParameter[] parameters)
        {
            //instantiate new command object
            SqlCommand _sqlCommand = Connection.CreateCommand();

            //instantiate the result
            int _result;

            //check if the command needs a transaction
            if (Transaction != null) _sqlCommand.Transaction = Transaction;

            try
            {
                //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = sqlStatement;

                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.Text;

                //add the parameters to the parameters collection of the command object
                _sqlCommand.Parameters.AddRange(parameters);

                //execute the query and assign the number of effected rows to the result
                _result = await _sqlCommand.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }

            return _result;
        }

        /// <summary>
        /// Gets the scalar.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The scalar object
        /// </returns>
        [Obsolete("Use GetScalar<T> to avoid needless conversions")]
        public object GetScalar(string sqlStatement, params DbParameter[] parameters)
        {
            //instantiate new command object
            SqlCommand _sqlCommand = (SqlCommand)Connection.CreateCommand();

            //check if the command needs a transaction

            if (!(Transaction == null))
            {
                _sqlCommand.Transaction = (SqlTransaction)Transaction;
            }

            try
            {   //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = sqlStatement;

                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.Text;

                //add the parameters to the parameters collection of the command object
                _sqlCommand.Parameters.AddRange(parameters);

                //return result
                return _sqlCommand.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }
        }

        /// <summary>
        /// Gets the scalar.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The scalar object
        /// </returns>
        [Obsolete("Use GetScalar<T> to avoid needless conversions")]
        public object GetScalar(string sqlStatement, params SqlParameter[] parameters)
        {
            //instantiate new command object
            SqlCommand _sqlCommand = (SqlCommand)Connection.CreateCommand();

            //check if the command needs a transaction

            if (!(Transaction == null))
            {
                _sqlCommand.Transaction = (SqlTransaction)Transaction;
            }

            try
            {   //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = sqlStatement;

                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.Text;

                //add the parameters to the parameters collection of the command object
                _sqlCommand.Parameters.AddRange(parameters);

                //return result
                return _sqlCommand.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }
        }

        /// <summary>
        /// Gets the scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar</typeparam>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The Scalar Item
        /// </returns>
        public T GetScalar<T>(string sqlStatement, params DbParameter[] parameters) where T : IConvertible
        {
            return GetScalar<T>(sqlStatement, default(T), parameters);
        }

        /// <summary>
        /// Gets the scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar</typeparam>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The Scalar Item
        /// </returns>
        public T GetScalar<T>(string sqlStatement, T defaultValue, params DbParameter[] parameters) where T : IConvertible
        {
            //instantiate new command object
            SqlCommand _sqlCommand = (SqlCommand)Connection.CreateCommand();

            //check if the command needs a transaction

            if (!(Transaction == null))
            {
                _sqlCommand.Transaction = (SqlTransaction)Transaction;
            }

            try
            {   //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = sqlStatement;

                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.Text;

                //add the parameters to the parameters collection of the command object
                _sqlCommand.Parameters.AddRange(parameters);

                //return result
                return (T)Convert.ChangeType(_sqlCommand.ExecuteScalar() ?? defaultValue, typeof(T));
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }
        }

        /// <summary>
        /// Gets the scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar</typeparam>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The Scalar Item
        /// </returns>
        public async Task<T> GetScalarAsync<T>(string sqlStatement, T defaultValue, params DbParameter[] parameters) where T : IConvertible
        {
            //instantiate new command object
            SqlCommand _sqlCommand = (SqlCommand)Connection.CreateCommand();

            //check if the command needs a transaction

            if (!(Transaction == null))
            {
                _sqlCommand.Transaction = (SqlTransaction)Transaction;
            }

            try
            {   //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = sqlStatement;

                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.Text;

                //add the parameters to the parameters collection of the command object
                _sqlCommand.Parameters.AddRange(parameters);

                //return result
                return (T)Convert.ChangeType(await _sqlCommand.ExecuteScalarAsync() ?? defaultValue, typeof(T));
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }
        }

        /// <summary>
        /// Executes the procedure with scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar</typeparam>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The Scalar Item
        /// </returns>
        public T ExecuteProcedureWithScalar<T>(string sqlStatement, params DbParameter[] parameters) where T : IConvertible
        {
            return ExecuteProcedureWithScalar<T>(sqlStatement, default(T), parametersList: null, parameters: parameters);
        }

        /// <summary>
        /// Executes the procedure with scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar</typeparam>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The Scalar Item
        /// </returns>
        public async Task<T> ExecuteProcedureWithScalarAsync<T>(string sqlStatement, params DbParameter[] parameters) where T : IConvertible
        {
            return await ExecuteProcedureWithScalarAsync<T>(sqlStatement, default(T), parametersList: null, parameters: parameters);
        }

        /// <summary>
        /// Executes the procedure with scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar</typeparam>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The Scalar Item
        /// </returns>
        public T ExecuteProcedureWithScalar<T>(string sqlStatement, T defaultValue, params DbParameter[] parameters) where T : IConvertible
        {
            return ExecuteProcedureWithScalar<T>(sqlStatement, defaultValue, null, parameters);
        }

        /// <summary>
        /// Executes the procedure with scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar</typeparam>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="parametersList">The parameters list.</param>
        /// <returns>
        /// The Scalar Item
        /// </returns>
        public T ExecuteProcedureWithScalar<T>(string sqlStatement, T defaultValue = default(T), IEnumerable<DbParameter> parametersList = null) where T : IConvertible
        {
            return ExecuteProcedureWithScalar<T>(sqlStatement, defaultValue, parametersList);
        }

        /// <summary>
        /// Executes the procedure with scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar</typeparam>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="parametersList">The parameters list.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The Scalar Item
        /// </returns>
        private T ExecuteProcedureWithScalar<T>(string sqlStatement, T defaultValue, IEnumerable<DbParameter> parametersList, params DbParameter[] parameters) where T : IConvertible
        {
            //instantiate new command object
            SqlCommand _sqlCommand = (SqlCommand)Connection.CreateCommand();

            //check if the command needs a transaction

            if (!(Transaction == null))
            {
                _sqlCommand.Transaction = (SqlTransaction)Transaction;
            }

            try
            {   //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = sqlStatement;

                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                //add the parameters to the parameters collection of the command object
                _sqlCommand.Parameters.AddRange(parameters);

                if (parametersList != null) _sqlCommand.Parameters.AddRange(parametersList.ToArray());

                //return result
                return (T)Convert.ChangeType(_sqlCommand.ExecuteScalar() ?? defaultValue, typeof(T));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }
        }

        /// <summary>
        /// Executes the procedure with scalar.
        /// </summary>
        /// <typeparam name="T">The type of the scalar</typeparam>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="parametersList">The parameters list.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// The Scalar Item
        /// </returns>
        private async Task<T> ExecuteProcedureWithScalarAsync<T>(string sqlStatement, T defaultValue, IEnumerable<DbParameter> parametersList, params DbParameter[] parameters) where T : IConvertible
        {
            //instantiate new command object
            SqlCommand _sqlCommand = (SqlCommand)Connection.CreateCommand();

            //check if the command needs a transaction

            if (!(Transaction == null))
            {
                _sqlCommand.Transaction = (SqlTransaction)Transaction;
            }

            try
            {   //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = sqlStatement;

                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.StoredProcedure;

                //add the parameters to the parameters collection of the command object
                _sqlCommand.Parameters.AddRange(parameters);

                if (parametersList != null) _sqlCommand.Parameters.AddRange(parametersList.ToArray());

                //return result
                return (T)Convert.ChangeType(await _sqlCommand.ExecuteScalarAsync() ?? defaultValue, typeof(T));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }
        }

        #endregion "Data Access Methods"

        #region "Other Methods"

        private IEnumerable<T> IDataRecordToT<T>(IEnumerable<IDataRecord> records)
            where T : new()
        {
            return records.ToT<T>();
        }

        #endregion "Other Methods"
    }
}
