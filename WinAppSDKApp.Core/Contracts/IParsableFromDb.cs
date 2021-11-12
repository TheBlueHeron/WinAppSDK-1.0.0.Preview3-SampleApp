using Microsoft.Data.SqlClient;

namespace GoogleMapper.Core.Contracts
{
    internal interface IParsableFromDb
    {
        void Parse(SqlDataReader r);
    }
}
