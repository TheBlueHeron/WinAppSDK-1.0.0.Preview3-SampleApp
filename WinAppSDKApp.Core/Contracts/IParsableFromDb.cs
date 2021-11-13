using Microsoft.Data.SqlClient;

namespace WinAppSDKApp.Core.Contracts
{
    internal interface IParsableFromDb
    {
        void Parse(SqlDataReader r);
    }
}
