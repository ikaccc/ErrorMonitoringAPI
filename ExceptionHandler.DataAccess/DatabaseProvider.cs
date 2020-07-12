using System;
using System.Data;
using System.Data.SqlClient;

namespace ExceptionHandler.DataAccess
{
    /// <summary>
    /// Engine that the provider will use
    /// </summary>
    [Obsolete("There is no need to use enums with X Access Layer")]
    public enum DatabaseEngine
    {
        SQLServer = 1,
        MySQL = 2,
        MSAccess = 3
    }

    /// <summary>
    /// Manages the Database Provider
    /// </summary>
    [Obsolete("Please use the X Access Layer instead")]
    public class DataBaseProvider
    {
        /// <summary>
        /// The m_database engine
        /// </summary>
        private static DatabaseEngine m_databaseEngine;

        /// <summary>
        /// The m_connection
        /// </summary>
        private IDbConnection m_connection;

        /// <summary>
        /// The m_transaction
        /// </summary>
        private IDbTransaction m_transaction;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataBaseProvider"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        private DataBaseProvider(string connectionString)
        {
            switch (m_databaseEngine)
            {
                default:
                case DatabaseEngine.SQLServer:
                    m_connection = new SqlConnection(connectionString);
                    break;
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the database engine.
        /// </summary>
        /// <value>
        /// The database engine.
        /// </value>
        public DatabaseEngine DatabaseEngine
        {
            get => m_databaseEngine;
            private set => m_databaseEngine = value;
        }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>
        /// The transaction.
        /// </value>
        public IDbTransaction Transaction
        {
            get => m_transaction;
            set => m_transaction = value;
        }

        #endregion Properties

        #region Provider Factory

        /// <summary>
        /// Creates the database provider.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>Database Provider</returns>
        [Obsolete("Please use Library.CreateDatabaseProvider instead")]
        public static DataBaseProvider CreateDatabaseProvider(string connectionString)
        {
            DataBaseProvider _dataAccessProvider = new DataBaseProvider(connectionString);
            return (_dataAccessProvider);
        }

        /// <summary>
        /// Creates the database provider.
        /// </summary>
        /// <param name="databaseEngine">The database engine.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>Database Provider</returns>
        [Obsolete("Please use Library.CreateDatabaseProvider instead")]
        public static DataBaseProvider CreateDatabaseProvider(DatabaseEngine databaseEngine, string connectionString)
        {
            m_databaseEngine = databaseEngine;
            DataBaseProvider _dataAccessProvider = new DataBaseProvider(connectionString);

            return (_dataAccessProvider);
        }

        #endregion Provider Factory

        #region Parameter Factory

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <returns>DB Parameter</returns>
        public IDbDataParameter GetParameter(string name, DbType type)
        {
            IDbDataParameter _newParameter = null;

            switch (m_databaseEngine)
            {
                case DatabaseEngine.SQLServer:

                    _newParameter = new SqlParameter
                    {
                        ParameterName = name,
                        DbType = type
                    };
                    break;
            }

            return _newParameter;
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>DB Parameter</returns>
        public IDbDataParameter GetParameter(string name, DbType type, ParameterDirection direction)
        {
            IDbDataParameter _newParameter = null;

            switch (m_databaseEngine)
            {
                case DatabaseEngine.SQLServer:

                    _newParameter = new SqlParameter
                    {
                        ParameterName = name,
                        DbType = type
                    };

                    break;
            }

            _newParameter.Direction = direction;

            return _newParameter;
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>DB Parameter</returns>
        public IDbDataParameter GetParameter(string name, SqlDbType type, ParameterDirection direction)
        {
            IDbDataParameter _newParameter = null;

            switch (m_databaseEngine)
            {
                case DatabaseEngine.SQLServer:

                    _newParameter = new SqlParameter
                    {
                        ParameterName = name,
                        DbType = (DbType)type
                    };

                    break;
            }

            _newParameter.Direction = direction;

            return _newParameter;
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="size">The size.</param>
        /// <param name="direction">The direction.</param>
        /// <returns>DB Parameter</returns>
        public IDbDataParameter GetParameter(string name, DbType type, short size, ParameterDirection direction)
        {
            IDbDataParameter _newParameter = null;

            switch (m_databaseEngine)
            {
                case DatabaseEngine.SQLServer:

                    _newParameter = new SqlParameter
                    {
                        ParameterName = name,
                        DbType = type,
                        Size = size
                    };
                    break;
            }

            _newParameter.Direction = direction;

            return _newParameter;
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>DB Parameter</returns>
        public IDbDataParameter GetParameter(string name, DbType type, object value)
        {
            IDbDataParameter _newParameter = null;

            switch (m_databaseEngine)
            {
                case DatabaseEngine.SQLServer:

                    _newParameter = new SqlParameter(name, type)
                    {
                        Value = value
                    };
                    break;
            }

            _newParameter.Value = value;

            return _newParameter;
        }

        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>DB Parameter</returns>
        public IDbDataParameter GetParameter(string name, SqlDbType type, object value)
        {
            IDbDataParameter _newParameter = null;

            switch (m_databaseEngine)
            {
                case DatabaseEngine.SQLServer:

                    _newParameter = new SqlParameter(name, type)
                    {
                        Value = value
                    };
                    break;
            }

            _newParameter.Value = value;

            return _newParameter;
        }

        #endregion Parameter Factory

        #region "Connection Methods"

        /// <summary>
        /// Opens the connection.
        /// </summary>
        public void OpenConnection()
        {
            if ((m_connection != null))
            {
                //check if connection is closed before opening
                if (m_connection.State == ConnectionState.Closed)
                {
                    m_connection.Open();
                }
            }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void CloseConnection()
        {
            if (m_connection != null)
            {
                //check if connection is open before closing
                if (m_connection.State == ConnectionState.Open)
                {
                    m_connection.Close();
                }
            }
        }

        #endregion "Connection Methods"

        #region Data Access Methods

        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>Data Reader of  SQL Statement</returns>
        public IDataReader GetDataReader(string sqlStatement, params IDbDataParameter[] Parameters)
        {
            //instantiate new command object
            IDbCommand _sqlCommand = m_connection.CreateCommand();

            //check if the command needs a transaction

            if ((m_transaction != null))
            {
                _sqlCommand.Transaction = m_transaction;
            }

            //instantiate DataReader
            IDataReader _resultSet = default(IDataReader);

            try
            {
                //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = sqlStatement;

                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.Text;

                foreach (IDbDataParameter _parameter in Parameters)
                {
                    //add the parameters to the parameters collection of the command object
                    _sqlCommand.Parameters.Add(_parameter);
                }

                //set the reader to return a datareader
                _resultSet = _sqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }

            return _resultSet;
        }

        /// <summary>
        /// Executes the procedure.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>Number of Effected Rows</returns>
        public int ExecuteProcedure(string procedureName, params IDataParameter[] Parameters)
        {
            //instantiate new command object
            IDbCommand _sqlCommand = m_connection.CreateCommand();

            //instantiate the result
            int _result = 0;

            //check if the command needs a transaction
            if ((m_transaction != null))
            {
                _sqlCommand.Transaction = m_transaction;
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
                _result = _sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
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
        /// Executes the procedure with result set.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="Parameters">The parameters.</param>
        /// <returns>Data Reader of  Stored Procedures</returns>
        public IDataReader ExecuteProcedureWithResultSet(string procedureName, params IDataParameter[] Parameters)
        {
            //instantiate new command object
            IDbCommand _sqlCommand = m_connection.CreateCommand();

            //instantiate the result
            IDataReader _resultSets = default(IDataReader);

            //check if the command needs a transaction
            if ((m_transaction != null))
            {
                _sqlCommand.Transaction = m_transaction;
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }

            return _resultSets;
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Number of Effected Rows</returns>
        public int ExecuteNonQuery(string sqlStatement, params object[] parameters)
        {
            //instantiate new command object
            IDbCommand _sqlCommand = m_connection.CreateCommand();

            //instantiate the result
            int _result = 0;

            //check if the command needs a transaction
            if ((m_transaction != null))
            {
                _sqlCommand.Transaction = m_transaction;
            }

            try
            {
                //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = sqlStatement;

                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.Text;

                foreach (IDbDataParameter _parameter in parameters)
                {
                    //add the parameters to the parameters collection of the command object
                    _sqlCommand.Parameters.Add(_parameter);
                }

                //execute the query without returning results
                _result = _sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
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
        /// <returns>The scalar item</returns>
        public object GetScalar(string sqlStatement, params object[] parameters)
        {
            //instantiate new command object
            IDbCommand _sqlCommand = m_connection.CreateCommand();

            //check if the command needs a transaction

            if ((m_transaction != null))
            {
                _sqlCommand.Transaction = m_transaction;
            }

            //instantiate an object for the result
            object _resultSet = null;

            try
            {
                //set CommandText property of the command object to the SQL Statement supplied
                _sqlCommand.CommandText = sqlStatement;

                //set the CommandType property to text
                _sqlCommand.CommandType = CommandType.Text;

                foreach (SqlParameter _parameter in parameters)
                {
                    //add the parameters to the parameters collection of the command object
                    _sqlCommand.Parameters.Add(_parameter);
                }

                //set the reader to return a datareader
                _resultSet = _sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //dispose of the command object
                _sqlCommand.Dispose();
            }

            return _resultSet;
        }

        #endregion Data Access Methods
    }
}
