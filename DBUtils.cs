using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace DynamicTest
{
    class DBUtils
    {
        public static OracleConnection GetGHIAConnection()
        {
            string host = "***";
            int port = 1521;
            string sid = "ORCL";
            string user = "****";
            string password = "****";

            return DBOracleUtils.GetDBConnection(host, port, sid, user, password);
        }

        public static OracleConnection GetKPH2012Connection()
        {
            string host = "****";
            int port = 1521;
            string sid = "ORCL";
            string user = "****";
            string password = "****";

            return DBOracleUtils.GetDBConnection(host, port, sid, user, password);
        }
    }
}
