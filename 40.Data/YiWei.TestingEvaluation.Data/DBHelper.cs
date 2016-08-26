using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiWei.TestingEvaluation.Data
{
    using System.Configuration;
    using System.Data.SqlClient;

    //dapper帮助文档
    //https://github.com/ericdc1/Dapper.SimpleCRUD/
    //https://github.com/StackExchange/dapper-dot-net

    /// <summary>
    /// Dapper DBHelper
    /// </summary>
    public class DBHelper
    {
        private static readonly ConnectionStringSettings Connection = ConfigurationManager.ConnectionStrings["TCWirelessWeiXin"];
        private static readonly string ConnectionString = Connection.ConnectionString;

        public static SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
    }
}
