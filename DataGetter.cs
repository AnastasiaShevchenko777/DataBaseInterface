using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicTest;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Dapper;


namespace DynamicTest
{
    class DataGetter
    {
        public DataTable CreateDataTable(string query)//Create Data table by sql query
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = DBUtils.GetGHIAConnection())
            {
                conn.Open();
                OracleDataAdapter da = new OracleDataAdapter();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = query;
                da.SelectCommand = cmd;
                da.Fill(dt);
            }
            return dt;
        }
        public static List<WaterObject> GetWaterObjList(string flowsID, char exp)//Select water object from database by id and it`s flow order;
        {
            using (OracleConnection conn = DBUtils.GetKPH2012Connection())
            {
                List<WaterObject> output = conn.Query<WaterObject>($"select nwo, id from cod_wo where exponent = " + exp + " and flows = " + flowsID).ToList();
                return output;
            }
        }
        public DataTable GetFromKPX(List<string> idarray)//Get information about each array element of water objects by ot`s id;
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = DBUtils.GetKPH2012Connection())
            {
                conn.Open();
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "MYPACKAGE.GetMyTableByIDs";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("p_outRefCursor", OracleDbType.RefCursor)).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(new OracleParameter("p_MyIDList", OracleDbType.Varchar2)).CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                cmd.Parameters[1].Value = idarray.ToArray();
                OracleDataAdapter da = new OracleDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
                conn.Close();
            }
            return dt;
        }
        public static DataTable GetWaterObjWithAnyID()
        {
            DataTable dt = new DataTable();
            using (OracleConnection conn = DBUtils.GetKPH2012Connection())
            {
                try
                {
                    string query = "select nwo,id from cod_wo where exponent=1";
                    OracleDataAdapter da = new OracleDataAdapter(query, conn);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured!");
                }
            }
            return dt;
        }

        public DataSet GetAllWaterObj()
        {
            DataSet ds = new DataSet();
            using (OracleConnection conn = DBUtils.GetGHIAConnection())
            {
                try
                {
                    string query = "select distinct VO from KPH2012.KPH_NEW order by vo";
                    OracleDataAdapter da = new OracleDataAdapter(query, conn);
                    da.Fill(ds, "WaterObj");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured!");
                }
            }
            return ds;
        }

        public DataSet GetObservationPoint(string curWo)
        {
            DataSet ds = new DataSet();
            using (OracleConnection conn = DBUtils.GetGHIAConnection())
            {
                try
                {
                    string query = "select distinct PNABL from KPH2012.KPH_NEW where VO like '" + curWo + "' order by PNABL";
                    OracleDataAdapter da = new OracleDataAdapter(query, conn);
                    da.Fill(ds, "Pnabl1");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured!");
                }
            }
            return ds;
        }
    }
}