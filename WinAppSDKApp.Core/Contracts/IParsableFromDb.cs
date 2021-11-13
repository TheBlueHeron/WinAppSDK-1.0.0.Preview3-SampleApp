using Microsoft.Data.SqlClient;

namespace WinAppSDKApp.Core.Data
{
    internal interface IParsableFromDb
    {
        void Parse(SqlDataReader r);
    }
}
