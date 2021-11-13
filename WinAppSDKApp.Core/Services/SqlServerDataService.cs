using WinAppSDKApp.Core.Models;
using System.Configuration;
using Microsoft.Data.SqlClient;
using WinAppSDKApp.Core.Data;
using System.Collections.Generic;
using System.Data;
using System;
using System.Threading.Tasks;
using WinAppSDKApp.Core.Logging;

namespace WinAppSDKApp.Core.Services
{
    /// <summary>
    /// Handles communication with the SQL Server data source.
    /// The connection string should be defined in the *.config file with a key value of <see cref="ConnectionConfigKey"/>.
    /// <seealso>https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/UWP/services/sql-server-data-service.md </seealso>
    /// </summary>
    public class SqlServerDataService
    {
        #region Objects and variables

        /// <summary>
        /// The *.config key to use for storing the connection string.
        /// </summary>
        public const string ConnectionConfigKey = "SqlConn";

        private const string clauseId = " WHERE (Id={0})";

        private const string qryGetProject = qryGetProjects + clauseId;
        private const string qryGetProjects = "SELECT Id,Name,Description,StartDate,EndDate FROM Projects";

        private string _conn;
        private ILoggingService _logger;

        #endregion

        #region Construction

        public SqlServerDataService(ILoggingService logger)
        {
            _logger = logger;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the connection string as defined in the configuration file.
        /// </summary>
        private string Connection
        {
            get
            {
                if (string.IsNullOrEmpty(_conn))
                {
                    _conn = ConfigurationManager.ConnectionStrings[ConnectionConfigKey]?.ConnectionString;
                }
                return _conn;
            }
        }

        #endregion

        #region Add



        #endregion

        #region Delete



        #endregion

        #region Exists



        #endregion

        #region Get

        /// <summary>
        /// Returns the <see cref="Project"/> with the given Id asynchronously.
        /// </summary>
        /// <param name="id">Id of the <see cref="Project"/></param>
        /// <returns>A <see cref="Project"/></returns>
        public async Task<Project> GetProjectAsync(int id)
        {
            return await GetObjectAsync<Project>(string.Format(qryGetProject, id));
        }

        /// <summary>
        /// Returns all <see cref="Project"/>s asynchronously.
        /// </summary>
        /// <returns>A list of <see cref="Project"/> objects</returns>
        public async Task<List<Project>> GetProjectsAsync()
        {
            return await GetObjectsAsync<Project>(qryGetProjects);
        }

        #endregion

        #region Update



        #endregion

        #region Generic

        /// <summary>
        /// Returns an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object, which must implement <see cref="IParsableFromDb"/></typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>A <typeparamref name="T"/></returns>
        private async Task<T> GetObjectAsync<T>(string query)
            where T : IParsableFromDb, new()
        {
            T item = new T();

            using (SqlDataReader reader = await ExecuteReaderAsync(query, true))
            {
                if (reader.HasRows)
                {
                    _ = reader.Read();
                    item.Parse(reader);
                }
            }

            return item;
        }

        /// <summary>
        /// Returns a collection of objects of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object, which must implement <see cref="IParsableFromDb"/></typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>A <see cref="List{T}"/></returns>
        private async Task<List<T>> GetObjectsAsync<T>(string query)
            where T : IParsableFromDb, new()
        {
            List<T> lst = new List<T>();

            using (SqlDataReader reader = await ExecuteReaderAsync(query, false))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        T item = new T();
                        item.Parse(reader);
                        lst.Add(item);
                    }
                }
            }

            return lst;
        }

        /// <summary>
        /// Executes the given query and returns the results as a <see cref="SqlDataReader" />.
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="singleRow">Optimize query execution to return a single row</param>
        /// <param name="hasResult">Determines whether the query returned any results</param>
        /// <returns><see cref="SqlDataReader" /></returns>
        /// <remarks>
        /// 1. In order to read data rows (if <paramref name="hasResult"/> = true), <see cref="SqlDataReader.Read"/> should be called for each row
        /// 2. The caller is responsible for closing the <see cref="SqlDataReader">reader</see>!
        /// </remarks>
        private async Task<SqlDataReader> ExecuteReaderAsync(string query, bool singleRow)
        {
            SqlDataReader reader = null;
            CommandBehavior behavior = CommandBehavior.CloseConnection;

            using (SqlCommand cmd = new SqlCommand(query, new SqlConnection(Connection)))
            {
                if (singleRow)
                {
                    behavior |= CommandBehavior.SingleRow;
                }

                try
                {
                    cmd.Connection.Open();
                    reader = cmd.ExecuteReader(behavior);
                }
                catch (Exception ex)
                {
                    await _logger.Log(ex.Message);
                }
            }

            return reader; // closing the reader will close the connection if behavior includes CommandBehavior.CloseConnection
        }

        #endregion
    }
}