using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using YiWei.TestingEvaluation.Entity.TT;

namespace YiWei.TestingEvaluation.Data
{
    public class MBTITesingData
    {
        private DbConnection _connection;

        public IEnumerable<MBTIQuestion> QueryQuestion(int tesingId)
        {
            using (_connection = DBHelper.GetOpenConnection())
            {
                IEnumerable<MBTIQuestion> list = _connection.GetList<MBTIQuestion>(new {TesingId = tesingId});

                return list;
            }
        }

        public bool InsertReport(MBTIReport report)
        {
            using (_connection = DBHelper.GetOpenConnection())
            {
                int? id = _connection.Insert(report);

                if (id.HasValue && id.Value > 0)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
