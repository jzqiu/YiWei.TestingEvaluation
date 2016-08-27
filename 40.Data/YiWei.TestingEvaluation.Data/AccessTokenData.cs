using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using YiWei.TestingEvaluation.Entity.TT;

namespace YiWei.TestingEvaluation.Data
{
    public class AccessTokenData
    {
        private DbConnection _connection;

        public SysAccessToken GetToken(int id)
        {
            using (_connection = DBHelper.GetOpenConnection())
            {
                var token = _connection.Get<SysAccessToken>(new {TId = id}) ?? new SysAccessToken();
                
                return token;
            }
        }

        public void Update(SysAccessToken token)
        {
            using (_connection = DBHelper.GetOpenConnection())
            {
                _connection.Update(token);
            }
        }
    }
}
