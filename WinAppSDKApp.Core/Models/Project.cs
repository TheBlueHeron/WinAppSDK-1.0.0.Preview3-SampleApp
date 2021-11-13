using WinAppSDKApp.Core.Contracts;
using Microsoft.Data.SqlClient;
using System;

namespace WinAppSDKApp.Core.Models
{
    /// <summary>
    /// Container for details on a project.
    /// </summary>
    public class Project
        : IParsableFromDb
    {
        #region Objects and variables



        #endregion

        #region Properties

        public string Description { get; set; }

        public DateTime EndDate { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        #endregion

        #region Public methods and functions

        /// <summary>
        /// Populates this object using the data in the <see cref="SqlDataReader"/>.
        /// </summary>
        /// <param name="r">The <see cref="SqlDataReader"/> containing the data</param>
        public void Parse(SqlDataReader r)
        {
            Id = r.GetInt32(0);
            Name = r.GetString(1);
            Description = r.GetString(2);
            StartDate = r.GetDateTime(3);
            EndDate = r.GetDateTime(4);
        }

        #endregion
    }
}