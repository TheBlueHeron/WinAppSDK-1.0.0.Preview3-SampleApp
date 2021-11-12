using GoogleMapper.Core.Models;
using System.Configuration;
using Microsoft.Data.SqlClient;
using GoogleMapper.Core.Contracts;
using System.Collections.Generic;
using System.Data;
using System;
using System.Threading.Tasks;

namespace GoogleMapper.Core.Services
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
        /// Returns the <see cref="Project"/> with the given Id.
        /// </summary>
        /// <param name="id">Id of the <see cref="Project"/></param>
        /// <returns>A <see cref="Project"/></returns>
        public Project GetProject(int id)
        {
            return GetObject<Project>(string.Format(qryGetProject, id));
        }

        /// <summary>
        /// Returns the <see cref="Project"/> with the given Id asynchronously.
        /// </summary>
        /// <param name="id">Id of the <see cref="Project"/></param>
        /// <returns>A <see cref="Project"/></returns>
        public async Task<Project> GetProjectAsync(int id)
        {
            Project p = null;

            await Task.Run(() =>
            {
                p = GetProject(id);
            });

            return p;
        }

        /// <summary>
        /// Returns all <see cref="Project"/>s.
        /// </summary>
        /// <returns>A list of <see cref="Project"/> objects</returns>
        public List<Project> GetProjects()
        {
            return GetObjects<Project>(qryGetProjects);
        }

        /// <summary>
        /// Returns all <see cref="Project"/>s asynchronously.
        /// </summary>
        /// <returns>A list of <see cref="Project"/> objects</returns>
        public async Task<List<Project>> GetProjectsAsync()
        {
            List<Project> data = null;

            await Task.Run(() =>
            {
                data = GetProjects();
            });

            return data;
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
        private T GetObject<T>(string query)
            where T : IParsableFromDb, new()
        {
            T item = new T();

            using (SqlDataReader reader = ExecuteReader(query, true, out bool hasResults))
            {
                if (hasResults)
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
        private List<T> GetObjects<T>(string query)
            where T : IParsableFromDb, new()
        {
            List<T> lst = new List<T>();

            using (SqlDataReader reader = ExecuteReader(query, false, out bool hasResults))
            {
                if (hasResults)
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
        private SqlDataReader ExecuteReader(string query, bool singleRow, out bool hasResult)
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
                    hasResult = reader.HasRows;
                }
                catch (Exception ex)
                {
                    // log
                    hasResult = false;
                }
            }

            return reader; // closing the reader will close the connection if behavior includes CommandBehavior.CloseConnection
        }

        #endregion
    }
}