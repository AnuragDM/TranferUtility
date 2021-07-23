namespace DAL
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.OleDb;
    using System.Configuration;

  //using Microsoft.ApplicationBlocks.Data;
    using System.Xml;
    using System.IO;
    using TransferDataUtilityApp.CommonClass;
    public sealed class DbConnectionDAL
    {
        // public static string sqlConnectionstring = ConfigurationManager.AppSettings["ConnectionString"].ToString();
    //    public static string sqlConnectionstring = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        //static SqlConnection cn = new SqlConnection(sqlConnectionstring);
        public static OleDbConnection myConn;
        public static string getConStrDmLicense
        {
            get
            {
                string constrDmLicense = "data source=103.231.40.154,1565; user id=dmlicense_user; pwd=SaG@e321; initial catalog=dmlicense;";
                return constrDmLicense;
            }
        }

        public static string getConStrNavision
        {
            get
            {
                string constrNavision = @"data source=10.197.1.14\STMDDB01,21443;  initial catalog=GoldieeMasaleNew2016; user id=notify; pwd=notify@123;";
                //string constrNavision = @"data source=192.168.1.232;  initial catalog=GoldieeMasaleTestDB; user id=notify; pwd=notify@123;";              
                return constrNavision;
            }
        }

        private static SqlParameter[] ConvertToSqlParameter(DbParameter[] dbParam)
        {
            return DbParameter.GetSqlParameter(dbParam);
        }

        public static int GetIntScalarVal(string command,string conn)
        {
            int output = 0;
            SqlConnection cn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(command, cn);
            //  OleDbDataReader dreader;
            cmd.CommandType = CommandType.Text;
            cn.Open();
            try
            {
                output = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (SqlException)
            {
                output = 1;
                //You can write to eventlog if you want, but most web hosting won't allow it.
                // WriteToEventLog(command + "\n" + x.ToString(), 2);
            }
            cn.Close();
            cmd = null;
            cn = null;
            return output;
        }

        public static int GetDemoLicenseIntScalarVal(string command)
        {
            int output = 0;
            SqlConnection cn = new SqlConnection(getConStrDmLicense);
            SqlCommand cmd = new SqlCommand(command, cn);
            //  OleDbDataReader dreader;
            cmd.CommandType = CommandType.Text;
            cn.Open();
            try
            {
                output = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (SqlException)
            {
                output = -1;
                //You can write to eventlog if you want, but most web hosting won't allow it.
                // WriteToEventLog(command + "\n" + x.ToString(), 2);
            }
            cn.Close();
            cmd = null;
            cn = null;
            return output;
        }

        public static int GetIntScalarValNavision(string command)
        {
            int output = 0;
            SqlConnection cn = new SqlConnection(getConStrNavision);
            SqlCommand cmd = new SqlCommand(command, cn);
            //  OleDbDataReader dreader;
            cmd.CommandType = CommandType.Text;
            cn.Open();
            try
            {
                output = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (SqlException)
            {
                output = 1;
                //You can write to eventlog if you want, but most web hosting won't allow it.
                // WriteToEventLog(command + "\n" + x.ToString(), 2);
            }
            cn.Close();
            cmd = null;
            cn = null;
            return output;
        }

        public static string GetStringScalarVal(string command,string conn)
        {
            string output = "";
            SqlConnection cn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(command, cn);
            //  OleDbDataReader dreader;
            cmd.CommandType = CommandType.Text;
            cn.Open();
            try
            {
                output = Convert.ToString(cmd.ExecuteScalar());
            }
            catch
            {
                //You can write to eventlog if you want, but most web hosting won't allow it.
                // WriteToEventLog(command + "\n" + x.ToString(), 2);
            }
            cn.Close();
            cmd = null;
            cn = null;

            return output;
        }

        public static void ExecuteNonQuery(CommandType cmdType, string cmdText, string conn,string Flag)
        {
            if (Flag == "S")
                SqlHelper.ExecuteNonQuery(conn, cmdType, cmdText);
            else
            {
                myConn = new OleDbConnection(conn);
                OleDbCommand delcmd = new OleDbCommand();
                delcmd.CommandText = cmdText;
                delcmd.Connection = myConn;
                delcmd.ExecuteNonQuery();
            }
        }

        public static void ExecuteNonQueryforlicence(string sqlConnectionstringlicence, CommandType cmdType, string cmdText)
        {
            SqlHelper.ExecuteNonQuery(sqlConnectionstringlicence, cmdType, cmdText);
        }

        public static DataTable getFromDataTable(string conn,string command, params  SqlParameter[] parameters)
        {
            SqlConnection cn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(command, cn);
            DataTable dt = new DataTable();
            IDataReader dr;

            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                foreach (SqlParameter item in parameters)
                    cmd.Parameters.Add(item);

            cn.Open();
            try
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr != null)
                { dt.Load(dr); }
                dr.Close();
            }
            catch
            {

            }
            cmd = null;
            cn.Close();
            return dt;
        }

        public static void ExecuteNonQuery(CommandType cmdType, string cmdText, DbParameter[] dbParam,string conn)
        {
            SqlParameter[] spParam = ConvertToSqlParameter(dbParam);
            SqlHelper.ExecuteNonQuery(conn, cmdType, cmdText, spParam);
            DbParameter.SetDbParameterValueFromSql(spParam, dbParam);
        }

        public static SqlDataReader GetDataReader(CommandType cmdType, string cmdText,string conn)
        {
            return SqlHelper.ExecuteReader(conn, cmdType, cmdText);
        }

        public static SqlDataReader GetDataReader(CommandType cmdType, string cmdText, DbParameter[] dbParam,string conn)
        {
            SqlParameter[] spParam = ConvertToSqlParameter(dbParam);
            SqlDataReader dReader = SqlHelper.ExecuteReader(conn, cmdType, cmdText, spParam);
            DbParameter.SetDbParameterValueFromSql(spParam, dbParam);
            return dReader;
        }

        public static DataSet GetDataSet(CommandType cmdType, string cmdText,string conn)
        {
            return SqlHelper.ExecuteDataset(conn, cmdType, cmdText);
        }

        public static DataSet GetDataSet(CommandType cmdType, string cmdText, DbParameter[] dbParam,string conn)
        {
            SqlParameter[] spParam = ConvertToSqlParameter(dbParam);
            DataSet dsetTable = SqlHelper.ExecuteDataset(conn, cmdType, cmdText, spParam);
            DbParameter.SetDbParameterValueFromSql(spParam, dbParam);
            return dsetTable;
        }
     


        public static DataTable GetDataTable(CommandType cmdType, string cmdText,string conn)
        {
            return SqlHelper.ExecuteDataTable(conn, cmdType, cmdText);
        }

        public static DataTable GetDataTable(CommandType cmdType, string cmdText, DbParameter[] dbParam,string conn)
        {
            SqlParameter[] spParam = ConvertToSqlParameter(dbParam);
            DataTable dtblTable = SqlHelper.ExecuteDataTable(conn, cmdType, cmdText, spParam);
            DbParameter.SetDbParameterValueFromSql(spParam, dbParam);
            return dtblTable;
        }

        public static DataTable GetDataTableForBusyData(string sqlConnectionstringBusyDb, CommandType cmdType, string cmdText,string Flag)
        {
            if (Flag == "S")
                return SqlHelper.ExecuteDataTable(sqlConnectionstringBusyDb, cmdType, cmdText);
            else
            {
                myConn = new OleDbConnection(sqlConnectionstringBusyDb);
                OleDbDataAdapter myCmd = new OleDbDataAdapter(cmdText, myConn);
                myConn.Open();
                DataTable dTable = new DataTable();
                myCmd.Fill(dTable);
                return dTable;
            }
        }

        public static object GetScalarValue(CommandType cmdType, string cmdText,string conn)
        {
            return SqlHelper.ExecuteScalar(conn, cmdType, cmdText);
        }
        public static object GetScalarValueBusy(CommandType cmdType, string cmdText,string sqlconn)
        {
            return SqlHelper.ExecuteScalar(sqlconn, cmdType, cmdText);
        }

        public static DataTable getFromDataTable(string command,string conn)
        {
            SqlConnection con = new SqlConnection(conn);
            SqlDataAdapter da = new SqlDataAdapter(command, con);            
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;            
        }

        public static DataTable getFromDataTableDmLicence(string command)
        {
            SqlConnection con = new SqlConnection(getConStrDmLicense);
            SqlDataAdapter da = new SqlDataAdapter(command, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public static DataTable getFromDataTableNavision(string command)
        {
            SqlConnection con = new SqlConnection(getConStrNavision);
            SqlDataAdapter da = new SqlDataAdapter(command, con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public static int ExecuteQuery(string command,string conn)
        {
            int msg = 0;

            SqlConnection cn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(command, cn);
            cmd.CommandText = command;
            cn.Open();
            try
            {
                msg = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            cn.Close();
            cmd = null;
            cn = null;
            return msg;
        }

        public static int ExecuteQuerynavision(string command)
        {
            int msg = 0;

            SqlConnection cn = new SqlConnection(getConStrNavision);
            SqlCommand cmd = new SqlCommand(command, cn);
            cmd.CommandText = command;
            cn.Open();
            try
            {
                msg = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            cn.Close();
            cmd = null;
            cn = null;
            return msg;
        }

        public static object GetScalarValue(CommandType cmdType, string cmdText, DbParameter[] dbParam,string conn)
        {
            SqlParameter[] spParam = ConvertToSqlParameter(dbParam);
            object objValue = SqlHelper.ExecuteScalar(conn, cmdType, cmdText, spParam);
            DbParameter.SetDbParameterValueFromSql(spParam, dbParam);
            return objValue;
        }

        //public static XmlDocument getXML(string Query, string tblnm, string dsnm)
        //{
        //    string conn="";
        //    DataTable dst = getFromDataTable(Query);
        //    DataSet dds = new DataSet();
        //    dds.Tables.Add(dst);
        //    dds.Tables[0].TableName = tblnm;
        //    dds.DataSetName = dsnm;
        //    StringWriter sw = new StringWriter();
        //    XmlTextWriter xtw = new XmlTextWriter(sw);
        //    XmlDocument xd = new XmlDocument();
        //    dds.WriteXml(xtw, XmlWriteMode.IgnoreSchema);
        //    string str = sw.ToString();
        //    xd.LoadXml(str);

        //    XmlNode nDate = xd.CreateElement("DateTime");
        //    nDate.InnerText = (DateTime.Now.Subtract(Convert.ToDateTime("01/01/1980"))).TotalMilliseconds.ToString();
        //    //nDate.Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
        //    XmlElement y = xd.DocumentElement;
        //    XmlNode x = xd.GetElementsByTagName(tblnm)[0];
        //    y.InsertBefore(nDate, x);

        //    //XmlNode y = xd.GetElementsByTagName(dsnm)[0].ChildNodes[1];
        //    //y.InnerText = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");

        //    sw.Close();
        //    xtw.Close();
        //    dds.Clear();
        //    dst.Clear();
        //    return xd;
        //}

        public static int ExecuteScaler(string command,string conn)
        {
            int modified = 0;

            SqlConnection cn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand(command, cn);
            cmd.CommandText = command;
            cn.Open();
            try
            {
                modified = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            cn.Close();
            cmd = null;
            cn = null;
            return modified;
        }
        

    }
}

