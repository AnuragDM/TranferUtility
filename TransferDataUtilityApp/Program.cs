using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferDataUtilityApp.CommonClass;

namespace TransferDataUtilityApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        static OleDbConnection Conn;
        static OleDbConnection busy_Conn1;
        static OleDbConnection busy_Conn2;
        static SqlConnection busy_sqlcon1;
        static SqlConnection busy_sqlcon2;
        static SqlConnection dest_sqlcon;
        static string connectionstr;

        static string connectionstring1 = "";
        static string connectionstring2 = "";
        static string connectionstring3 = "";

        static string dbFlag = "";
        static int Flag = 0;
        static string SDID = "";


        public class errorresult
        {
            public string msg;
            public List<mandatorymsg> errormsg;
        }
        public class mandatorymsg
        {
            public string msdmsg;
        }

        public class Result1
        {
            public string Message;
        }

        public class Purchorderlist
        {
            public List<Transpurchorder> result { get; set; }
        }
        public class Transpurchorder
        {
            public string Docid { get; set; }
            public int Pordid { get; set; }
            public string Distid { get; set; }
            public string Partyname { get; set; }
            public string VDate { get; set; }

            public string Remark { get; set; }

            public DateTime createddate { get; set; }
            public decimal totalamount { get; set; }
            public string Vno { get; set; }
            public string referenceno { get; set; }
            public List<Transpurchorder1> purchorder1 { get; set; }

        }

        public class Transpurchorder1
        {
            public string Docid { get; set; }

            public int sno { get; set; }

            public string itemid { get; set; }

            public decimal Quantity { get; set; }
            public decimal Amount { get; set; }

            public string Unit { get; set; }
            public decimal Rate { get; set; }

            public DateTime VDate { get; set; }



        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //connectionstr = ConfigurationManager.ConnectionStrings["AccessString"].ConnectionString;
            //Conn = new OleDbConnection(connectionstr);
            //CheckandtransferData();

            Application.Run(new Start());
        }

        public static void CheckandtransferData()
        {
            int cnt = 0;
            string strSQL = "SELECT count(*) as cnt FROM Enviro";
            OleDbDataAdapter myCmd = new OleDbDataAdapter(strSQL, Conn);
            Conn.Open();
            DataSet dtSet = new DataSet();
            myCmd.Fill(dtSet, "Enviro");
            DataTable dTable = dtSet.Tables[0];
            cnt = Convert.ToInt32(dTable.Rows[0]["cnt"].ToString());

            if (cnt > 0)
            {
                string message = "Do you want to update database path?";
                string title = "Confirm Window";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);

                if (result == DialogResult.Yes)
                {
                    BusyConfigSettings ma = new BusyConfigSettings();
                    ma.ShowDialog();
                }
                else
                {
                    strSQL = "SELECT * FROM Enviro";
                    myCmd = new OleDbDataAdapter(strSQL, Conn);
                    dtSet = new DataSet();
                    myCmd.Fill(dtSet, "Enviro");
                    DataTable dTabledata = dtSet.Tables[0];
                    string source = dTabledata.Rows[0]["Source"].ToString();
                    //MessageBox.Show(source.ToString());
                    if (source.ToLower() == "busy")
                    {
                        createBusyConnectionString(dTabledata);

                        //PostItemsToBusy();
                        //BusyAccountSyncData();
                        //BusySalesSyncData();
                        //BusyLedgerSyncData(dTabledata);
                        //Purchorderlist POL = PostPurchOrderToBusy();
                        //MessageBox.Show(POL.ToString());
                        //BusySalesOrderSyncData(POL);
                    }
                    else if (source.ToLower() == "marg")
                    {
                        connectionstring3 = "Data Source=" + dTabledata.Rows[0]["DestinationServer"].ToString() + ";Initial Catalog=" + dTabledata.Rows[0]["DestinationDatabase"].ToString() + ";user id=" + dTabledata.Rows[0]["DestinationUser"].ToString() + "; pwd=" + dTabledata.Rows[0]["DestinationPassword"].ToString() + ";";
                        //MargSyncRetailer(dTabledata);
                        //MargSyncItem(dTabledata);
                        //MargSyncRetailerinvoice(dTabledata);

                    }
                }
            }
            else
            {
                MainApplication ma = new MainApplication();
                ma.Show();
                //    this.Hide();
            }
        }


        private static void createBusyConnectionString(DataTable dt)
        {
            //Start st = new Start();
            connectionstring1 = "";
            connectionstring2 = "";
            connectionstring3 = "";
            string compcode = "";
            string compfile = "";

            connectionstring3 = "Data Source=" + dt.Rows[0]["DestinationServer"].ToString() + ";Initial Catalog=" + dt.Rows[0]["DestinationDatabase"].ToString() + ";user id=" + dt.Rows[0]["DestinationUser"].ToString() + "; pwd=" + dt.Rows[0]["DestinationPassword"].ToString() + ";";

            dest_sqlcon = new SqlConnection(connectionstring3);

            SDID = dt.Rows[0]["DistributorID"].ToString();
            
            if (dt.Rows[0]["BusyDatabase"].ToString().ToLower() == "ms access")
            {
                dbFlag = "A";
                string[] authorsList = dt.Rows[0]["Busy_AccessFileName1"].ToString().Split(new Char[] { ',', '\n' });
              
                for (int i = 0; i < authorsList.Length; i++)
                {

                    connectionstring1 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dt.Rows[0]["Busy_AccessDbPath"].ToString() + "\\" + authorsList[i].ToString() + ";Jet OLEDB:Database Password=" + dt.Rows[0]["Busy_AccessDbPassword"].ToString() + "";
                    compcode = authorsList[i].Substring(0, authorsList[i].IndexOf('\\'));
                    compfile = authorsList[i].ToString();

                    //st.label1.Text = "Transferring " + compfile.ToString()+" for "+ compcode;
                    //st.progressBar1.Minimum = 0;
                    //st.progressBar1.Maximum = authorsList.Length;

                    busy_Conn1 = new OleDbConnection(connectionstring1);

                    busy_Conn1.Open();
                    if (busy_Conn1.State == ConnectionState.Open)
                    {
                        //st.ShowDialog();
                        //st.progressBar1.Value = i;

                        Flag = Flag + 1;
                        //PostItemsToBusy(compfile);
                        //BusyAccountSyncData(compcode, compfile);
                        //BusySalesSyncData(compcode, compfile);
                        BusyLedgerSyncData(compcode, compfile);

                        //st.Hide();
                    }
                    busy_Conn1.Close();
                }
                //st.Hide();

                //connectionstring2 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dt.Rows[0]["Busy_AccessDbPath"].ToString() + "\\" + dt.Rows[0]["Busy_AccessFileName2"].ToString() + ";Jet OLEDB:Database Password=" + dt.Rows[0]["Busy_AccessDbPassword"].ToString() + "";


                //busy_Conn1 = new OleDbConnection(connectionstring1);
                //busy_Conn2 = new OleDbConnection(connectionstring2);
            }
            else if (dt.Rows[0]["BusyDatabase"].ToString().ToLower() == "ms sql")
            {
                dbFlag = "S";
                connectionstring1 = "Data Source=" + dt.Rows[0]["Busy_SQLServerIP"].ToString() + ";Initial Catalog=" + dt.Rows[0]["Busy_SQLDbName1"].ToString() + ";user id=" + dt.Rows[0]["Busy_SQLUsername"].ToString() + "; pwd=" + dt.Rows[0]["Busy_SQLPassword"].ToString() + ";";

                connectionstring2 = "Data Source=" + dt.Rows[0]["Busy_SQLServerIP"].ToString() + ";Initial Catalog=" + dt.Rows[0]["Busy_SQLDbName2"].ToString() + ";user id=" + dt.Rows[0]["Busy_SQLUsername"].ToString() + "; pwd=" + dt.Rows[0]["Busy_SQLPassword"].ToString() + ";";

                //MessageBox.Show(connectionstring1.ToString());
                busy_sqlcon1 = new SqlConnection(connectionstring1);
                busy_sqlcon2 = new SqlConnection(connectionstring2);
            }
            else
            {
                Application.Run(new BusyConfigSettings());
            }
        }


        private static void PostItemsToBusy(string CompFile)
        {
            List<Result1> rst = new List<Result1>();
            DataTable DTItems = new DataTable();
            decimal _amt = 0;
            int count = 0;
            string unt = null;
            string _query, _query1, _query2, _query3;
            ClsProcedure DB = new ClsProcedure();
            string str = "";
            int result = 0, result1 = 0;
            int success = 0, success1 = 0;
            int failure = 0, failure1 = 0;
            int err = 0, err1 = 0;

            List<mandatorymsg> msglist = new List<mandatorymsg>();
            errorresult rs = new errorresult();
            try
            {
                if (dbFlag == "A")
                {
                    _query1 = @"Select * from Master1 where Name = 'Finished Goods'";
                    DataTable dt_grps = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query1, dbFlag);
                    if (dt_grps.Rows.Count > 0)
                    {
                        _query2 = @"Select * from Master1 where ParentGrp = " + dt_grps.Rows[0]["Code"].ToString() + "";
                        //_query2 = @"Select * from Master1 where Code = 1196"; 
                        DataTable dt_grp = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query2, dbFlag);

                        if (dt_grp.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt_grp.Rows.Count; i++)
                            {
                                _query = @"Select ItemId from MastItem where ItemType='MATERIALGROUP' and [SyncId]='" + "BU#" + dt_grp.Rows[i]["Code"].ToString() + "'";
                                DataTable dt_items = DbConnectionDAL.GetDataTable(CommandType.Text, _query, connectionstring3);

                                if (dt_items.Rows.Count == 0)
                                {
                                    string active = "0", promoted = "0", Itemcode = "BU#" + dt_grp.Rows[i]["Code"].ToString(), SyncId = "BU#" + dt_grp.Rows[i]["Code"].ToString(), ItemName = dt_grp.Rows[i]["Name"].ToString().Replace("'", "''"), ParentName = "1";

                                    result = DB.InsertItems_Busy(connectionstring3, ItemName, "", true, 0, 0, 0, 0, ParentName, Itemcode, SyncId, "MATERIALGROUP", ItemName, "", "", "", 0, 0, 0, false, 0, 0, 0, "0", "0");
                                }
                                else
                                {
                                    //MessageBox.Show(dt_grp.Rows[i]["Name"].ToString().Replace("'", "''"));
                                    string active = "0", promoted = "0", ItemID = dt_items.Rows[0]["ItemId"].ToString(), Itemcode = "BU#" + dt_grp.Rows[i]["Code"].ToString(), SyncId = "BU#" + dt_grp.Rows[i]["Code"].ToString(), ItemName = dt_grp.Rows[i]["Name"].ToString().Replace("'", "''"), ParentName = "1";

                                    result = DB.UpdateItems_Busy(connectionstring3, Convert.ToInt32(ItemID), ItemName, "", true, 0, 0, 0, 0, ParentName, Itemcode, SyncId, "MATERIALGROUP", ItemName, "", "", "", 0, 0, 0, false, 0, 0, 0, "0", "0");

                                }

                                //SELECT Master1.*,Help1.NameAlias
                                //FROM Master1 LEFT JOIN Help1 ON Master1.CM1 = Help1.Code where Master1.RecType = 7 and Master1.ParentGrp = " + dt_grp.Rows[i]["Code"].ToString() + "

                                _query3 = @"Select * from Master1 left join Help1 on Master1.CM1=Help1.Code where Master1.ParentGrp = " + dt_grp.Rows[i]["Code"].ToString() + " and Help1.RecType=7";
                                DataTable dt_gritm = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query3, dbFlag);

                                if (dt_gritm.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dt_gritm.Rows.Count; j++)
                                    {
                                        //MessageBox.Show(dt_gritm.Rows[j]["Name"].ToString() + " " + dt_gritm.Rows[j]["NameAlias"].ToString());
                                        _query = @"Select ItemId from MastItem where ItemType='ITEM' and ItemName=UPPER('" + dt_gritm.Rows[j]["Name"].ToString().Replace("'", "''") + "')";
                                        DataTable dt_items1 = DbConnectionDAL.GetDataTable(CommandType.Text, _query, connectionstring3);

                                        if (dt_items1.Rows.Count == 0)
                                        {
                                            string Itemcode = "BU#" + dt_gritm.Rows[j]["Master1.Code"].ToString(), SyncId = "BU#" + dt_gritm.Rows[j]["Master1.Code"].ToString(), ItemName = dt_gritm.Rows[j]["Name"].ToString().Replace("'", "''"), ParentName = "1", Unit = dt_gritm.Rows[j]["NameAlias"].ToString(), MRP = dt_gritm.Rows[j]["D2"].ToString(), DP = dt_gritm.Rows[j]["D4"].ToString(), RP = dt_gritm.Rows[j]["D3"].ToString();


                                            //MessageBox.Show(ItemName.ToString() + " " + Unit.ToString());
                                            result1 = DB.InsertItems_Busy(connectionstring3, ItemName, Unit, true, 0, Convert.ToDecimal(MRP), Convert.ToDecimal(DP), Convert.ToDecimal(RP), ParentName, Itemcode, SyncId, "ITEM", ItemName, "A", "", "", 0, 0, 0, false, 0, 0, 0, "1", "1");
                                        }
                                        else
                                        {
                                            string Itemcode = "BU#" + dt_gritm.Rows[j]["Master1.Code"].ToString(), ItemID = dt_items1.Rows[0]["ItemId"].ToString(), SyncId = "BU#" + dt_gritm.Rows[j]["Master1.Code"].ToString(), ItemName = dt_gritm.Rows[j]["Name"].ToString().Replace("'", "''"), ParentName = "1", Unit = dt_gritm.Rows[j]["NameAlias"].ToString(), MRP = dt_gritm.Rows[j]["D2"].ToString(), DP = dt_gritm.Rows[j]["D4"].ToString(), RP = dt_gritm.Rows[j]["D3"].ToString();

                                            //MessageBox.Show(ItemName.ToString() + " " + Unit.ToString()+" "+ SyncId);
                                            result1 = DB.UpdateItems_Busy(connectionstring3, Convert.ToInt32(ItemID), ItemName, Unit, true, 0, Convert.ToDecimal(MRP), Convert.ToDecimal(DP), Convert.ToDecimal(RP), ParentName, Itemcode, SyncId, "ITEM", ItemName, "A", "", "", 0, 0, 0, false, 0, 0, 0, "1", "1");

                                        }
                                        if (result1 > 0)
                                        {
                                            success1 = success1 + 1;
                                        }
                                        else
                                        {
                                            failure1 = failure1 + 1;
                                        }
                                    }
                                }

                                str = success1 + " record inserted successfully.";
                                if (failure1 > 0)
                                    str = str + "," + failure1 + " record are failed";
                                if (err1 > 0)
                                    str = str + ",Check log table";
                                if (msglist.Count == 0)
                                {
                                    mandatorymsg ms = new mandatorymsg();
                                    ms.msdmsg = "No error";
                                    msglist.Add(ms);
                                }

                                LogError(str, "Success", "PostItemsToBusy", CompFile);

                                rs.msg = str;
                                rs.errormsg = msglist;

                                if (result > 0)
                                {
                                    success = success + 1;
                                }
                                else
                                {
                                    failure = failure + 1;
                                }
                            }
                        }
                        str = success + " record inserted successfully.";
                        if (failure > 0)
                            str = str + "," + failure + " record are failed";
                        if (err > 0)
                            str = str + ",Check log table";
                        if (msglist.Count == 0)
                        {
                            mandatorymsg ms = new mandatorymsg();
                            ms.msdmsg = "No error";
                            msglist.Add(ms);
                        }

                        LogError(str, "Success", "PostItemsToBusy",CompFile);

                        rs.msg = str;
                        rs.errormsg = msglist;
                    }
                }
            }
            catch (Exception ex)
            {
                //transactionScope.Dispose();

                str = ex.Message;
                err = err + 1;
                LogError(str, "Fail", "PostItemsToBusy", CompFile);
                // msglist..Add(str);
                if (err > 0)
                    str = str + ",Check log table";
                rs.msg = str;
                rs.errormsg = msglist;

            }
            //_query = @"Select * from Master1 where ParentGrp = '" + dt_grps.Rows[0]["Code"].ToString() + "' and MasterType='5'";
            //_query = @"Select * from Master1 where ParentGrp = 1975 and MasterType='6'";
            //DataTable dt_items = DbConnectionDAL.GetDataTable(CommandType.Text, _query, connectionstring1);
            //DataTable dt_items = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query1, dbFlag);



            //for (int i = 0; i < dt_items.Rows.Count; i++)
            //{

            //    string query_cnt = "select code from Master1 where Alias='" + dt_items.Rows[i]["ItemId"].ToString() + "' and MasterType=6";
            //    DataTable dt_cnt = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, query_cnt, dbFlag);
            //    if (dt_cnt.Rows.Count <= 0)
            //    {
            //int code = Convert.ToInt32(DbConnectionDAL.GetScalarValueBusy(CommandType.Text, "SELECT max(Code) from Master1", connectionstring2));

            //query_cnt = "select code from Master1 where Alias='" + dt_items.Rows[i]["UnderId"].ToString() + "' and MasterType=5";
            //DataTable dt_itemgrp_cnt = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, query_cnt, dbFlag);
            //if (dt_itemgrp_cnt.Rows.Count > 0)
            //{

            //    string query_unit = "select code from Master1 where Name='" + dt_cnt.Rows[i]["Unit"].ToString() + "'";
            //    DataTable dt_unit = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, query_unit, dbFlag);

            //    _query = "insert into master1 (Code,MasterType,Alias,PrintName,ParentGrp,Stamp,CM1,CM1,CM3,CM4,CM8,CM10,D2,CreatedBy,CreationTime) values(" + (code + 1) + ",6,'" + dt_cnt.Rows[i]["ItemName"].ToString() + "','" + dt_cnt.Rows[i]["ItemName"].ToString() + "',1283,1," + Convert.ToInt32(code) + "," + Convert.ToInt32(code) + ",403," + Convert.ToInt32(code) + "," + dt_cnt.Rows[i]["MRP"].ToString() + ",'Tanvi',getDate())";

            //    int cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query, connectionstring2));
            //    if (cnt > 0)
            //    {

            //    }
            //}
            //else
            //{
            //    //create item group
            //    _query = @"Select * from MastItem where ItemType='MATERIALGROUP' and ItemId=" + dt_items.Rows[i]["UnderId"].ToString() + "";
            //    DataTable dt_itemgrpdetails = DbConnectionDAL.GetDataTable(CommandType.Text, _query, connectionstring3);
            //    if (dt_itemgrpdetails.Rows.Count > 0)
            //    {

            //        _query = "insert into master1 (Code,MasterType,Alias,PrintName,CreatedBy,CreationTime) values(" + (code + 1) + ",5,'" + dt_itemgrpdetails.Rows[i]["ItemName"].ToString() + "','" + dt_itemgrpdetails.Rows[i]["ItemName"].ToString() + "','Tanvi',getDate())";

            //        int cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query, connectionstring2));
            //        if (cnt > 0)
            //        {

            //            string query_unit = "select code from Master1 where Name='" + dt_cnt.Rows[i]["Unit"].ToString() + "'";
            //            DataTable dt_unit = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, query_unit, dbFlag);

            //            _query = "insert into master1 (Code,MasterType,Alias,PrintName,ParentGrp,Stamp,CM1,CM1,CM3,CM4,CM8,CM10,D2,CreatedBy,CreationTime) values(" + (code + 2) + ",6,'" + dt_cnt.Rows[i]["ItemName"].ToString() + "','" + dt_cnt.Rows[i]["ItemName"].ToString() + "'," + dt_cnt.Rows[i]["Underid"].ToString() + ",1," + Convert.ToInt32(code) + "," + Convert.ToInt32(code) + ",403," + Convert.ToInt32(code) + "," + dt_cnt.Rows[i]["MRP"].ToString() + ",'Tanvi',getDate())";

            //            int cnt1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query, connectionstring2));
            //        }
            //    }
            //}
            //    }
            //}
            MessageBox.Show("Done");
        }

        private static void BusyAccountSyncData(string Compcode, string CompFile)
        {
            List<Result1> rst = new List<Result1>();
            DataTable dt = new DataTable();
            string str = "";
            int cnt = 0;
            int cityid = 0;
            int citytypeid = 0;
            int cityconveyancetype = 0;
            int distictid = 0;
            int regionid = 0;
            int Areaid = 0;
            int stateid = 0;
            int contid = 0;
            int roleid = 0;
            ClsProcedure DB = new ClsProcedure();
            int result = 0;
            int success = 0;
            int successupdated = 0;
            int failure = 0;
            int err = 0;
            string _query1, _query2, _query, _query3;
            //MessageBox.Show("run2");
            List<mandatorymsg> msglist = new List<mandatorymsg>();
            errorresult rs = new errorresult();
            try
            {
                if (dbFlag == "A")
                {
                    cityid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='CITY' AND AreaName='Blank'", connectionstring3));

                    citytypeid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select Id From MastCityType Where Name='Other'", connectionstring3));

                    cityconveyancetype = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select Id From MastCityType Where Name='OTHERS'", connectionstring3));

                    distictid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select AreaId from MastArea where Areatype='DISTRICT' And AreaName='Blank'", connectionstring3));

                    regionid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='REGION' AND AreaName='Blank'", connectionstring3));

                    stateid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='STATE' AND AreaName='Blank'", connectionstring3));

                    contid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='COUNTRY' AND AreaName='Blank'", connectionstring3));

                    _query1 = @"Select Code from Master1 where Name = 'Sundry Debtors' and MasterType=1";
                    DataTable dt_grps = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query1, dbFlag);
                    if (dt_grps.Rows.Count > 0)
                    {
                        _query2 = @"SELECT Master1.Code as Code,Master1.Name as Name,MasterAddressInfo.Address1 as Address1,MasterAddressInfo.Address2 as Address2,MasterAddressInfo.Address3 as Address3,MasterAddressInfo.TelNo as TelNo,MasterAddressInfo.Fax as Fax,MasterAddressInfo.Email as Email,MasterAddressInfo.Mobile as Mobile,MasterAddressInfo.PINCode as PINCode,MasterAddressInfo.ITPAN as ITPAN,MasterAddressInfo.GSTNo as GSTNo,MasterAddressInfo.CountryCodeLong as ContID,MasterAddressInfo.StateCodeLong as StatID,Help1.NameAlias as ContName,Help1_1.NameAlias as StatName
FROM ((Master1 LEFT JOIN MasterAddressInfo ON Master1.Code = MasterAddressInfo.MasterCode) LEFT JOIN Help1 ON MasterAddressInfo.CountryCodeLong = Help1.Code) LEFT JOIN Help1 AS Help1_1 ON MasterAddressInfo.StateCodeLong = Help1_1.Code WHERE (((Master1.ParentGrp) In (Select Code from Master1 where Master1.ParentGrp=" + dt_grps.Rows[0]["Code"].ToString() + "  and Master1.MasterType=1))) OR (((Master1.ParentGrp)=" + dt_grps.Rows[0]["Code"].ToString() + ") AND ((Master1.MasterType)=2) AND ((MasterAddressInfo.MasterCode)<>0) AND ((Help1.RecType)=55))";

                        DataTable dt_grp = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query2, dbFlag);
                        if (dt_grp.Rows.Count > 0)
                        {
                            _query3 = @"Select RoleId from MastRole where RoleType='Distributor'";
                            DataTable dt_rol = DbConnectionDAL.GetDataTable(CommandType.Text, _query3, connectionstring3);

                            for (int i = 0; i < dt_grp.Rows.Count; i++)
                            {
                                _query = @"Select PartyId from MastParty where PartyDist=1 and SyncId = 'BU#" + dt_grp.Rows[i]["Code"].ToString() + "' and Compcode='" + Compcode + "'";
                                DataTable dt_items = DbConnectionDAL.GetDataTable(CommandType.Text, _query, connectionstring3);

                                if (dt_items.Rows.Count == 0)
                                {
                                    Areaid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "SELECT AreaId from MastArea where AreaType='AREA' AND AreaName='Area-" + dt_grp.Rows[i]["Name"].ToString().Replace("'", "''") + "'", connectionstring3));

                                    string Distributor = dt_grp.Rows[i]["Name"].ToString().Replace("'", "''"), Address1 = dt_grp.Rows[i]["Address1"].ToString(), Address2 = dt_grp.Rows[i]["Address2"].ToString().Trim() + dt_grp.Rows[i]["Address3"].ToString().Trim(), Pin = dt_grp.Rows[i]["PINCode"].ToString(), Email = dt_grp.Rows[i]["Email"].ToString(), Mobile = dt_grp.Rows[i]["Mobile"].ToString(), SyncId = "BU#" + dt_grp.Rows[i]["Code"].ToString(), RoleID = dt_rol.Rows[0]["RoleId"].ToString(), PanNo = dt_grp.Rows[i]["ITPAN"].ToString(), Fax = dt_grp.Rows[i]["Fax"].ToString(), Phone = dt_grp.Rows[i]["TelNo"].ToString(), GST = dt_grp.Rows[i]["GSTNo"].ToString(), CityID = cityid.ToString(), SD_ID = SDID.ToString(), Area_Id = Areaid.ToString(), CntrNme = dt_grp.Rows[i]["ContName"].ToString(), CntrCde = contid.ToString(), StatNme = dt_grp.Rows[i]["StatName"].ToString(), StatCde = stateid.ToString(), RgnCde = regionid.ToString(), DistCde = distictid.ToString(), CtTypCde = citytypeid.ToString(), CtCnvncId = cityconveyancetype.ToString(), UserName = "BU#" + dt_grp.Rows[i]["Code"].ToString();



                                    result = DB.InsertDistributorsBusy
                                        (connectionstring3, Compcode, Distributor, Address1, Address2, CityID, Pin, Email, Mobile, "", SyncId, "", UserName, true, Phone, Convert.ToInt32(RoleID), "", "", "", "", PanNo, 0, 0, 0, 0, "", Fax, Distributor, 0, "", "", Convert.ToInt32(Area_Id), Convert.ToInt32(SD_ID), CntrNme, StatNme, Convert.ToInt32(CntrCde), Convert.ToInt32(StatCde), Convert.ToInt32(RgnCde), Convert.ToInt32(DistCde), Convert.ToInt32(CtTypCde), Convert.ToInt32(CtCnvncId), GST, "", 0);
                                }
                                else
                                {
                                    string DistId = dt_items.Rows[0]["PartyId"].ToString(), Distributor = dt_grp.Rows[i]["Name"].ToString().Replace("'", "''"), Address1 = dt_grp.Rows[i]["Address1"].ToString(), Address2 = dt_grp.Rows[i]["Address2"].ToString().Trim() + dt_grp.Rows[i]["Address3"].ToString().Trim(), Pin = dt_grp.Rows[i]["PINCode"].ToString(), Email = dt_grp.Rows[i]["Email"].ToString(), Mobile = dt_grp.Rows[i]["Mobile"].ToString(), SyncId = "BU#" + dt_grp.Rows[i]["Code"].ToString(), RoleID = dt_rol.Rows[0]["RoleId"].ToString(), PanNo = dt_grp.Rows[i]["ITPAN"].ToString(), Fax = dt_grp.Rows[i]["Fax"].ToString(), Phone = dt_grp.Rows[i]["TelNo"].ToString(), GST = dt_grp.Rows[i]["GSTNo"].ToString(), CityID = cityid.ToString(), SD_ID = SDID.ToString(), Area_Id = Areaid.ToString(), CntrNme = dt_grp.Rows[i]["ContName"].ToString(), CntrCde = contid.ToString(), StatNme = dt_grp.Rows[i]["StatName"].ToString(), StatCde = stateid.ToString(), RgnCde = regionid.ToString(), DistCde = distictid.ToString(), CtTypCde = citytypeid.ToString(), CtCnvncId = cityconveyancetype.ToString(), UserName = "BU#" + dt_grp.Rows[i]["Code"].ToString();

                                    result = DB.UpdateDistributorsBusy(connectionstring3, Compcode, Convert.ToInt32(DistId), Distributor, Address1, Address2, CityID, Pin, Email, Mobile, "", SyncId, "", UserName, true, Phone, Convert.ToInt32(RoleID), "", "", "", "", PanNo, 0, 0, 0, 0, "", Fax, Distributor, 0, "", "", Convert.ToInt32(Area_Id), Convert.ToInt32(SD_ID), CntrNme, StatNme, Convert.ToInt32(CntrCde), Convert.ToInt32(StatCde), Convert.ToInt32(RgnCde), Convert.ToInt32(DistCde), Convert.ToInt32(CtTypCde), Convert.ToInt32(CtCnvncId), GST, "", 0);
                                }

                                if (result > 0)
                                {
                                    success = success + 1;
                                }
                                else
                                {
                                    failure = failure + 1;
                                }
                            }
                        }
                        str = success + " record inserted successfully.";
                        if (failure > 0)
                            str = str + "," + failure + " record are failed";
                        if (err > 0)
                            str = str + ",Check log table";
                        if (msglist.Count == 0)
                        {
                            mandatorymsg ms = new mandatorymsg();
                            ms.msdmsg = "No error";
                            msglist.Add(ms);
                        }

                        LogError(str, "Success", "BusyAccountSyncData", CompFile);

                        rs.msg = str;
                        rs.errormsg = msglist;

                    }
                }
            }
            catch (Exception ex)
            {
                //transactionScope.Dispose();

                str = ex.Message;
                err = err + 1;
                LogError(str, "Fail", "BusyAccountSyncData", CompFile);
                // msglist..Add(str);
                if (err > 0)
                    str = str + ",Check log table";
                rs.msg = str;
                rs.errormsg = msglist;

            }

            //    cityid = Convert.ToInt32(DbConnectionDAL.GetScalarValueBusy(CommandType.Text, "SELECT AreaId from MastArea where AreaType='CITY' AND AreaName='Blank'", connectionstring3));
            //    Areaid = Convert.ToInt32(DbConnectionDAL.GetScalarValueBusy(CommandType.Text, "SELECT AreaId from MastArea where AreaType='AREA' AND AreaName='Blank'", connectionstring3));

            //    roleid = Convert.ToInt32(DbConnectionDAL.GetScalarValueBusy(CommandType.Text, "Select RoleId From MastRole Where RoleName='DISTRIBUTOR'", connectionstring3));


            //    //str = "select * from Master1 where MasterType=2";
            //    str = "SELECT m1.Code,m1.MasterType,m1.Name,m1.PrintName,m1.ParentGrp,Address1,Address2,Address3,Address4,TelNo,Fax,Email,Mobile,CST,TINNo,ITPAN,Contact,ServiceTaxNo,TelNoResi,GSTNo,CountryCodeLong,StateCodeLong,CityCodeLong,RegionCodeLong,AreaCodeLong,PINCode FROM Master1 m1 left join MasterAddressInfo ma1 on m1.Code=ma1.MasterCode where m1.MasterType=2  and ParentGrp in (select code from master1 where mastertype=1 and name='Sundry Debtors') order by m1.code";
            //    dt = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, str, dbFlag);
            //    if (dt.Rows.Count > 0)
            //    {
            //        DataTable DTadmin = DbConnectionDAL.GetDataTableForBusyData(connectionstring3, CommandType.Text, "Select * from MastSalesRep Where SMName='DIRECTOR'", dbFlag);
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            mandatorymsg ms = new mandatorymsg();
            //            int retval = DB.InsertDistributorsBusy(connectionstring3, dt.Rows[i]["Name"].ToString(), dt.Rows[i]["Address1"].ToString() + " " + dt.Rows[i]["Address2"].ToString(), dt.Rows[i]["Address3"].ToString() + " " + dt.Rows[i]["Address4"].ToString(), cityid.ToString(), dt.Rows[i]["PINCode"].ToString(), dt.Rows[i]["Email"].ToString(), dt.Rows[i]["Mobile"].ToString(), "", dt.Rows[i]["Code"].ToString(), "", dt.Rows[i]["Name"].ToString(), true, dt.Rows[i]["TelNo"].ToString(), roleid, dt.Rows[i]["Contact"].ToString(), dt.Rows[i]["CST"].ToString(), dt.Rows[i]["TINNo"].ToString(), dt.Rows[i]["ServiceTaxNo"].ToString(), dt.Rows[i]["ITPAN"].ToString(), 0, 0, 0, 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), dt.Rows[i]["TelNoResi"].ToString(), dt.Rows[i]["Fax"].ToString(), dt.Rows[i]["PrintName"].ToString(), Convert.ToInt32(DTadmin.Rows[0]["SMId"]), Convert.ToString(""), Convert.ToString(""), Areaid);
            //            if (retval == -1)
            //            {
            //                //'Duplicate User Name Exists'
            //                ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not inserted,Something went wrong";
            //                msglist.Add(ms);
            //                failure = failure + 1;
            //                continue;
            //            }
            //            else if (retval == -2)
            //            {
            //                //'Duplicate SyncId Exists'
            //                int retval_update = DB.UpdateDistributorsBusy(connectionstring3, Convert.ToInt32(dt.Rows[i]["Code"].ToString()), dt.Rows[i]["Name"].ToString(), dt.Rows[i]["Address1"].ToString() + " " + dt.Rows[i]["Address2"].ToString(), dt.Rows[i]["Address3"].ToString() + " " + dt.Rows[i]["Address4"].ToString(), cityid.ToString(), dt.Rows[i]["PINCode"].ToString(), dt.Rows[i]["Email"].ToString(), dt.Rows[i]["Mobile"].ToString(), "", dt.Rows[i]["Code"].ToString(), "", DTadmin.Rows[0]["SMName"].ToString(), true, dt.Rows[i]["TelNo"].ToString(), roleid, dt.Rows[i]["Contact"].ToString(), dt.Rows[i]["CST"].ToString(), dt.Rows[i]["TINNo"].ToString(), dt.Rows[i]["ServiceTaxNo"].ToString(), dt.Rows[i]["ITPAN"].ToString(), 0, 0, 0, 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), dt.Rows[i]["TelNoResi"].ToString(), dt.Rows[i]["Fax"].ToString(), dt.Rows[i]["PrintName"].ToString(), Convert.ToInt32(DTadmin.Rows[0]["SMId"]), Convert.ToString(""), Convert.ToString(""), Areaid);

            //                if (retval_update == -1)
            //                {
            //                    //'Duplicate User Name Exists'
            //                    ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not updated,Something went wrong";
            //                    msglist.Add(ms);
            //                    failure = failure + 1;
            //                    continue;
            //                }
            //                else if (retval_update == -2)
            //                {
            //                    //'Duplicate SyncId Exists'
            //                    ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not updated,Something went wrong";
            //                    msglist.Add(ms);
            //                    failure = failure + 1;
            //                    continue;
            //                }

            //                else if (retval_update == -3)
            //                {
            //                    //'Duplicate Email Exists'
            //                    ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not updated,Something went wrong";
            //                    msglist.Add(ms);
            //                    failure = failure + 1;
            //                    continue;
            //                }
            //                else if (retval_update == -5)
            //                {
            //                    //'Duplicate Mobile Exists'
            //                    ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not updated,Something went wrong";
            //                    msglist.Add(ms);
            //                    failure = failure + 1;
            //                    continue;
            //                }
            //                else
            //                {
            //                    successupdated = successupdated + 1;
            //                }
            //            }
            //            else if (retval == -3)
            //            {
            //                //'Duplicate Distributor Exists'
            //                ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not inserted,Something went wrong";
            //                msglist.Add(ms);
            //                failure = failure + 1;
            //                continue;
            //            }
            //            else if (retval == -4)
            //            {
            //                //'Duplicate Email Exists'
            //                ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not inserted,Something went wrong";
            //                msglist.Add(ms);
            //                failure = failure + 1;
            //                continue;
            //            }
            //            else if (retval == -5)
            //            {
            //                //'Duplicate Mobile Exists'
            //                ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not inserted,Something went wrong";
            //                msglist.Add(ms);
            //                failure = failure + 1;
            //                continue;
            //            }
            //            else
            //            {
            //                success = success + 1;
            //            }
            //        }
            //    }

            //    str = success + " record inserted successfully, " + successupdated + " record updated successfully";
            //    if (failure > 0)
            //        str = str + "," + failure + " record are failed";
            //    if (err > 0)
            //        str = str + ",Check log table";
            //    if (msglist.Count == 0)
            //    {
            //        mandatorymsg ms = new mandatorymsg();
            //        ms.msdmsg = "No error";
            //        msglist.Add(ms);
            //    }

            //    LogError(str, "Success", "BusyAccountSyncData");

            //    rs.msg = str;
            //    rs.errormsg = msglist;
            //    // this.SucessEmailStatus();
            //}
            //catch (Exception ex)
            //{
            //    //transactionScope.Dispose();
            //    cnt = 0;
            //    str = ex.Message;
            //    err = err + 1;
            //    LogError(str, "Fail", "BusyAccountSyncData");
            //    // msglist..Add(str);
            //    if (err > 0)
            //        str = str + ",Check log table";
            //    //rs.msg = str;
            //    //rs.errormsg = msglist;

            //}
            //return rs;

        }

        private static void BusySalesSyncData(string Compcode, string CompFile)
        {
            List<Result1> rst = new List<Result1>();
            DataTable dt = new DataTable();
            string str = "";
            int cnt = 0;
            int DistInvId = 0;
            int DistInv1Id = 0;
            int success = 0;
            int successupdated = 0;
            int failure = 0;
            int err = 0;
            int sno = 0;
            string companycode = "";
            string DistInvDocId = "";
            string creationtime = "";
            string _query1, _query2, _query, _query3;
            //MessageBox.Show("run3");
            ClsProcedure DB = new ClsProcedure();
            List<mandatorymsg> msglist = new List<mandatorymsg>();
            errorresult rs = new errorresult();
            try
            {

                _query1 = @"Select * from Master1 where Name = 'Finished Goods'";
                DataTable dt_grp = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query1, dbFlag);
                if (dt_grp.Rows.Count > 0)
                {
                    //_query2 = @"Select Code from Master1 where ParentGrp = " + dt_grp.Rows[0]["Code"].ToString() + "";
                    ////_query2 = @"Select * from Master1 where Code = 1196"; 
                    //DataTable dt_itm = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query2, dbFlag);
                    //if (dt_itm.Rows.Count > 0)
                    //{
                    _query = @"Select Tran1.VchNo as VNO,Tran1.VchCode as VCode,Tran1.CreationTime as CrTime,Tran1.VchSalePurcAmt as VSPAmo,Tran1.MasterCode1 as Code1,Tran1.MasterCode2 as Code2,Tran1.Date as VDate,tran1.VchAmtBaseCur as BlAmt from Tran1 where Tran1.VchNo IN(Select DISTINCT Tran2.VchNo from Tran2 WHERE (((Tran2.MasterCode1) In (Select Master1.Code from Master1 left join Help1 on Master1.CM1=Help1.Code where Master1.ParentGrp IN (Select Code from Master1 where ParentGrp = " + dt_grp.Rows[0]["Code"].ToString() + ") and Help1.RecType=7))) AND ((Tran2.VchType)=9))";
                    DataTable dt_grps = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query, dbFlag);
                    if (dt_grps.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt_grps.Rows.Count; i++)
                        {
                            creationtime = dt_grps.Rows[i]["VDate"].ToString();

                            DistInvDocId = "BUSY" + " " + Convert.ToString(Convert.ToDateTime(dt_grps.Rows[i]["VDate"].ToString()).Year) + " " + dt_grps.Rows[i]["VNO"].ToString().Trim();


                            _query = @"Select DistInvId from TransDistInv where DistInvDocId='" + DistInvDocId + "' and Compcode='" + Compcode + "'";
                            DataTable dt_items = DbConnectionDAL.GetDataTable(CommandType.Text, _query, connectionstring3);

                            if (dt_items.Rows.Count == 0)
                            {
                                _query = @"Select PartyId from MastParty where SyncId='" + "BU#" + dt_grps.Rows[i]["Code1"].ToString() + "'";
                                DataTable dt_item = DbConnectionDAL.GetDataTable(CommandType.Text, _query, connectionstring3);

                                if (dt_item.Rows.Count > 0)
                                {
                                    decimal sgstamo = Convert.ToDecimal(dt_grps.Rows[i]["VSPAmo"].ToString()) * Convert.ToDecimal(dt_grps.Rows[i]["SGST"].ToString());
                                    decimal cgstamo = Convert.ToDecimal(dt_grps.Rows[i]["VSPAmo"].ToString()) * Convert.ToDecimal(dt_grps.Rows[i]["CGST"].ToString());
                                    decimal taxamo = sgstamo + cgstamo;
                                    decimal round = Convert.ToDecimal(dt_grps.Rows[i]["BlAmt"].ToString()) - (Convert.ToDecimal(dt_grps.Rows[i]["VSPAmo"].ToString()) + Math.Round(sgstamo, 2) + Math.Round(cgstamo, 2));
                                    decimal disc = Convert.ToDecimal(dt_grps.Rows[i]["ActAmt"].ToString()) - Convert.ToDecimal(dt_grps.Rows[i]["VSPAmo"].ToString());
                                    sno = 0;
                                    DistInvId = DB.insertsaleinvheader_Busy(connectionstring3, DistInvDocId, creationtime, taxamo, Convert.ToDecimal(dt_grps.Rows[i]["BlAmt"].ToString()), round, disc, "BU#" + dt_grps.Rows[i]["Code1"].ToString(), Compcode);

                                    str = "select * from Tran2 where VchCode=" + dt_grps.Rows[i]["VCode"].ToString() + " and RecType=2";
                                    DataTable dt1 = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, str, dbFlag);
                                    if (dt1.Rows.Count > 0)
                                    {
                                        for (int j = 0; j < dt1.Rows.Count; j++)
                                        {

                                            str = "select * from Master1 where code=" + dt1.Rows[j]["MasterCode1"].ToString() + "";

                                            DataTable dt2 = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, str, dbFlag);
                                            if (dt2.Rows.Count > 0)
                                            {
                                                for (int k = 0; k < dt2.Rows.Count; k++)
                                                {
                                                    if (!String.IsNullOrEmpty(dt2.Rows[k]["Alias"].ToString()))
                                                    {
                                                        sno = sno + 1;
                                                        mandatorymsg ms = new mandatorymsg();


                                                        if (DistInvId == -2)
                                                        {
                                                            ms.msdmsg = "SyncID -" + "BU#" + dt_grps.Rows[i]["Code1"].ToString() + " not exist on web portal for Bill no " + dt_grps.Rows[i]["VNO"].ToString().Trim();
                                                            msglist.Add(ms);
                                                            failure = failure + 1;

                                                        }
                                                        else
                                                        {
                                                            decimal amt = Convert.ToDecimal(dt1.Rows[j]["Value1"].ToString()) * -1 * Convert.ToDecimal(dt1.Rows[j]["D3"].ToString());
                                                            DistInv1Id = DB.insertsaleinvdetail_Busy(connectionstring3, DistInvId, sno, DistInvDocId, creationtime, 0, Convert.ToDecimal(dt1.Rows[j]["Value1"].ToString()), Convert.ToDecimal(dt1.Rows[j]["D3"].ToString()), amt, "BU#" + dt_grps.Rows[i]["Code1"].ToString(), dt2.Rows[k]["Name"].ToString(), "BU#" + dt2.Rows[k]["Code"].ToString().Trim(), Compcode);

                                                            if (DistInv1Id > 0)
                                                            {
                                                                success = success + 1;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

                str = success + " record inserted successfully.";
                if (failure > 0)
                    str = str + "," + failure + " record are failed";
                if (err > 0)
                    str = str + ",Check log table";
                if (msglist.Count == 0)
                {
                    mandatorymsg ms = new mandatorymsg();
                    ms.msdmsg = "No error";
                    msglist.Add(ms);
                }

                LogError(str, "Success", "BusySalesSyncData", CompFile);

                rs.msg = str;
                rs.errormsg = msglist;
                // this.SucessEmailStatus();
            }
            catch (Exception ex)
            {
                //transactionScope.Dispose();
                cnt = 0;
                str = ex.Message;
                err = err + 1;
                LogError(str, "Fail", "BusySalesSyncData", CompFile);
                // msglist..Add(str);
                if (err > 0)
                    str = str + ",Check log table";
                rs.msg = str;
                rs.errormsg = msglist;

            }
            //return rs;

            //    str = "select GUID from Company";
            //    DataTable dtcompany = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, str, dbFlag);
            //    if (dtcompany.Rows.Count > 0)
            //    {
            //        companycode = dtcompany.Rows[0]["GUID"].ToString();

            //        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete from TransDistInv where Compcode='" + companycode + "'", connectionstring3, dbFlag);
            //        DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete from TransDistInv1 where Compcode='" + companycode + "'", connectionstring3, dbFlag);

            //        str = "   select convert(varchar, t1.Date, 23) + ' ' + convert(varchar, t1.CreationTime, 8) as CTime,t1.VchCode,t1.VchNo,t1.Date,t1.CreationTime,t1.VchAmtBaseCur,t1.VchSalePurcAmt,t1.MasterCode1,t1.MasterCode2,m1.Name from Tran1 t1 left join Master1 m1 on t1.MasterCode1=m1.Code  where VchType=9 order by t1.VchCode  asc";

            //        dt = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, str, dbFlag);
            //        if (dt.Rows.Count > 0)
            //        {
            //            for (int i = 0; i < dt.Rows.Count; i++)
            //            {
            //                creationtime = dt.Rows[i]["CTime"].ToString();
            //                str = "select * from Tran2 where VchCode=" + dt.Rows[i]["VchCode"].ToString() + " and RecType=2";

            //                DataTable dt1 = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, str, dbFlag);
            //                if (dt1.Rows.Count > 0)
            //                {
            //                    for (int j = 0; j < dt1.Rows.Count; j++)
            //                    {
            //                        str = "select * from Master1 where code=" + dt1.Rows[j]["MasterCode1"].ToString() + "";

            //                        DataTable dt2 = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, str, dbFlag);
            //                        if (dt2.Rows.Count > 0)
            //                        {
            //                            for (int k = 0; k < dt2.Rows.Count; k++)
            //                            {
            //                                if (!String.IsNullOrEmpty(dt2.Rows[j]["Alias"].ToString()))
            //                                {
            //                                    sno = sno + 1;
            //                                    mandatorymsg ms = new mandatorymsg();
            //                                  

            //                                    if (DistInvId == -2)
            //                                    {
            //                                        ms.msdmsg = "Party -" + dt.Rows[i]["Name"].ToString() + " not exist on web portal for Bill no " + dt.Rows[i]["VchNo"].ToString();
            //                                        msglist.Add(ms);
            //                                        failure = failure + 1;

            //                                    }
            //                                    else
            //                                    {
            //                                        decimal amt = Convert.ToDecimal(dt1.Rows[j]["Value1"].ToString()) * Convert.ToDecimal(dt2.Rows[j]["D3"].ToString());
            //                                        DistInv1Id = DB.insertsaleinvdetail_Busy(connectionstring2, DistInvId, sno, DistInvDocId, creationtime, 0, Convert.ToDecimal(dt1.Rows[j]["Value1"].ToString()), Convert.ToDecimal(dt2.Rows[j]["D3"].ToString()), amt, dt.Rows[i]["MasterCode1"].ToString(), dt2.Rows[j]["Name"].ToString(), dt2.Rows[j]["Code"].ToString(), companycode);
            //                                        if (DistInv1Id > 0)
            //                                        {

            //                                            success = success + 1;
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }

            //    }
        }

        private static void BusyLedgerSyncData(string companycode, string CompFile)
        {
            List<Result1> rst = new List<Result1>();
            decimal _amount = 0;
            decimal amtdr = 0;
            decimal amtcr = 0;
            string docID;
            string Vtype = "";
            string Narration = "";
            ClsProcedure DB = new ClsProcedure();
            DataTable dt = new DataTable();
            string str = "";
            int result = 0;
            int success = 0;
            int failure = 0;
            int err = 0;
            int sno = 0;
            // string companycode = "";
            //MessageBox.Show("run4");
            List<mandatorymsg> msglist = new List<mandatorymsg>();
            errorresult rs = new errorresult();
            try
            {

                //str = "select GUID from Company";
                //DataTable dtcompany = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, str, dbFlag);
                //if (dtcompany.Rows.Count > 0)
                //{

                //companycode = dtcompany.Rows[0]["GUID"].ToString();

                //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete FROM TransDistributerLedger WHERE DistLedId NOT In(SELECT DistLedId FROM TransDistributerLedger WHERE Narration=' Opening Balance' OR COMPANYCODE !='" + companycode + "') ", connectionstring3, dbFlag);




                str = "SELECT Master1_1.Name, Master1_2.name AS Expr1, 'BU#'+LTrim(Str(MasterCode1))  AS Syncid, Switch(VchType=9,'SALES',VchType=14,'RECEIPT',VchType=16,'JOURNAL',VchType=12,'SALES ORDER') AS Vchtype1, t2.VchCode, t2.Date AS CDate, t2.VchNo, t2.Value1, t2.ShortNar FROM ((Tran2 AS t2 INNER JOIN Master1 ON t2.MasterCode1 = Master1.Code) INNER JOIN Master1 AS Master1_1 ON Master1.ParentGrp = Master1_1.Code) INNER JOIN Master1 AS Master1_2 ON Master1_1.ParentGrp = Master1_2.Code WHERE ((Master1_1.Name='Sundry Debtors') or (Master1_2.Name='Sundry Debtors'))";

                dt = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, str, dbFlag);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        //if (dt.Rows[i]["VchType"].ToString() == "9")
                        //{
                        //    Vtype = "Sales";
                        //}
                        //else if (dt.Rows[i]["VchType"].ToString() == "19")
                        //{
                        //    Vtype = "Payment";
                        //}
                        //else if (dt.Rows[i]["VchType"].ToString() == "14")
                        //{
                        //    Vtype = "Receipt";
                        //}
                        //else if (dt.Rows[i]["VchType"].ToString() == "12")
                        //{
                        //    Vtype = "Sales Order";
                        //}
                        //else if (dt.Rows[i]["VchType"].ToString() == "16")
                        //{
                        //    Vtype = "Journal";
                        //}

                        if (dt.Rows[i]["Value1"].ToString() != "")
                        {
                            if (Convert.ToDecimal(dt.Rows[i]["Value1"].ToString()) < 0)
                            {
                                amtdr = Convert.ToDecimal(dt.Rows[i]["Value1"].ToString()) * -1;
                            }
                            else
                            {
                                amtcr = Convert.ToDecimal(dt.Rows[i]["Value1"].ToString());
                            }
                        }

                        //docID = "BS" + Vtype.Substring(0, 3).ToUpper() + companycode.Substring(companycode.Length - 3) + " " + Convert.ToString(Convert.ToDateTime(dt.Rows[i]["CDate"].ToString()).Year) + " " + dt.Rows[i]["VchCode"].ToString();

                        if (dt.Rows[i]["VchType1"].ToString() != "")
                            Narration = dt.Rows[i]["ShortNar"].ToString() + "  (Vch Type-" + dt.Rows[i]["VchType1"].ToString() + ")";
                        else
                            Narration = dt.Rows[i]["ShortNar"].ToString();
                        //string _query = "Select isnull(PartyId,0) from MastParty where SyncId='" + dt_syncid.Rows[0]["SyncId"].ToString() + "' and compcode='" + companycode + "' And partydist=1";
                        //int _DistID = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query, connectionstring3));

                        if (amtcr > 0) _amount = amtcr;
                        else if (amtdr > 0) _amount = amtdr * -1;

                        result = DB.insertdistledger_Busy(connectionstring3, dt.Rows[i]["Syncid"].ToString().Trim(),
                            dt.Rows[i]["CDate"].ToString(),
                            amtdr, amtcr, _amount, Narration, dt.Rows[i]["VchCode"].ToString(), dt.Rows[i]["VchType1"].ToString(),
                            companycode);

                        if (result > 0)
                        {
                            success = success + 1;
                            amtcr = 0;
                            amtdr = 0;
                            _amount = 0;


                        }
                        else
                        {
                            failure = failure + 1;
                            amtcr = 0;
                            amtdr = 0;
                            _amount = 0;

                        }




                        amtcr = 0;
                        amtdr = 0;
                        _amount = 0;
                    }
                }

                str = success + " record inserted successfully.";
                if (failure > 0)
                    str = str + "," + failure + " record are failed";
                if (err > 0)
                    str = str + ",Check log table";
                if (msglist.Count == 0)
                {
                    mandatorymsg ms = new mandatorymsg();
                    ms.msdmsg = "No error";
                    msglist.Add(ms);
                }

                LogError(str, "Success", "BusyLedgerSyncData", CompFile);

                rs.msg = str;
                rs.errormsg = msglist;
                // this.SucessEmailStatus();
            }
            catch (Exception ex)
            {
                //transactionScope.Dispose();

                str = ex.Message;
                err = err + 1;
                LogError(str, "Fail", "BusyLedgerSyncData", CompFile);
                // msglist..Add(str);
                if (err > 0)
                    str = str + ",Check log table";
                rs.msg = str;
                rs.errormsg = msglist;

            }
            //return rs;
            MessageBox.Show("Done" + Flag.ToString());
        }

        private static void LogError(string str, string status, string functionname, string cmpnfile)
        {
            string query = "insert into Log (Description,Status,MethodName,ErrorTime,Company) values ('" + str.Replace("'", "''") + "','" + status + "','" + functionname + "',Date(),'"+ cmpnfile+"')";
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            cmd.Connection = Conn;
            if (Conn != null && Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
            cmd.ExecuteNonQuery();
            Conn.Close();
        }

        //private static void MargSyncRetailer(DataTable Dt)
        //{
        //    ClsProcedure DB = new ClsProcedure();
        //    DataTable Dtorderfilter = new DataTable();
        //    DataTable DtSaletypefilter = new DataTable();
        //    DataTable DtSupportfilter = new DataTable();
        //    DataTable Dtorder = new DataTable();
        //    DataTable Dtsaletype = new DataTable();
        //    DataTable Dtsupport = new DataTable();
        //    DataTable DtDistributor = new DataTable();
        //    DataTable DTadmin = new DataTable();
        //    DataTable DTBeat = new DataTable();
        //    DataTable DTState = new DataTable();
        //    errorresult rs = new errorresult();
        //    List<mandatorymsg> msglist = new List<mandatorymsg>();
        //    int success = 0;
        //    int failure = 0;
        //    int cnt = 0;
        //    int cityid = 0;
        //    int Areaid = 0;
        //    int regionid = 0;
        //    int distictid = 0;
        //    int beatid = 0;
        //    string str = "", Email = "", Areaname = "", Beatname = "", Statename = "";
        //    try
        //    {

        //        int citytypeid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select Id From MastCityType Where Name='Other'", connectionstring3));
        //        int cityconveyancetype = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select Id From MastCityType Where Name='OTHERS'", connectionstring3));
        //        DtDistributor = DbConnectionDAL.GetDataTable(CommandType.Text, "Select PartyName,MP.Areaid AS Areaid,cityid,MD.AreaId AS Distinctid,MS.AreaId AS Stateid,MR.AreaId AS Regionid from MastParty  MP LEFT JOIN MastArea MC ON MC.AreaId=MP.CityId LEFT JOIN MastArea MD ON MD.AreaId=MC.UnderId LEFT JOIN MastArea MS ON MS.AreaId=MD.UnderId LEFT JOIN MastArea MR ON MR.AreaId=MS.UnderId  where PartyID=" + Dt.Rows[0]["DistributorID"].ToString() + "", connectionstring3);
        //        DTadmin = DbConnectionDAL.GetDataTable(CommandType.Text, "Select * from MastSalesRep Where SMName='DIRECTOR'", connectionstring3);
        //        cityid = Convert.ToInt32(DtDistributor.Rows[0]["cityid"].ToString());
        //        regionid = Convert.ToInt32(DtDistributor.Rows[0]["Regionid"].ToString());
        //        distictid = Convert.ToInt32(DtDistributor.Rows[0]["Distinctid"].ToString());
        //        Areaid = Convert.ToInt32(DtDistributor.Rows[0]["Areaid"].ToString());
        //        ////////////////Get csv data/////////////////
        //        Dtorder = Convertcsvtodatatable("C:\\INDIADATA\\order.csv");
        //        Dtsaletype = Convertcsvtodatatable("C:\\INDIADATA\\saletype.csv");
        //        Dtsupport = Convertcsvtodatatable("C:\\INDIADATA\\support.csv");
        //        ////////////////Get csv data/////////////////
        //        //////////Get Sundry Debitor Data///////////////

        //        if (Dtorder.Rows.Count > 0)
        //        {
        //            Dtorder.DefaultView.RowFilter = "SCODE='C6'";
        //            Dtorderfilter = Dtorder.DefaultView.ToTable();
        //        }

        //        //////////Get Sundry Debitor Data///////////////


        //        if (Dtorderfilter.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < Dtorderfilter.Rows.Count; i++)
        //            {
        //                mandatorymsg ms = new mandatorymsg();
        //                if (Dtsupport.Rows.Count > 0)
        //                {
        //                    Dtsupport.DefaultView.RowFilter = "SNO =2 and LCODE='" + Dtorderfilter.Rows[i]["ORDNO"].ToString() + "'";
        //                    DtSupportfilter = Dtsupport.DefaultView.ToTable();
        //                    if (DtSupportfilter.Rows.Count > 0)
        //                    {

        //                        Email = DtSupportfilter.Rows[0]["REMARK"].ToString();

        //                    }
        //                }
        //                if (Dtsaletype.Rows.Count > 0)
        //                {
        //                    Dtsaletype.DefaultView.RowFilter = "SGCODE='AREA' and SCODE='" + Dtorderfilter.Rows[i]["AREA"].ToString() + "'";
        //                    DtSaletypefilter = Dtsaletype.DefaultView.ToTable();
        //                    if (DtSaletypefilter.Rows.Count > 0)
        //                    {
        //                        Areaname = DtSaletypefilter.Rows[0]["PARNAM"].ToString();
        //                    }
        //                }
        //                if (Dtsaletype.Rows.Count > 0)
        //                {
        //                    Dtsaletype.DefaultView.RowFilter = "SGCODE='ROUT' and SCODE='" + Dtorderfilter.Rows[i]["ROUT"].ToString() + "'";
        //                    DTBeat = Dtsaletype.DefaultView.ToTable();
        //                    if (DTBeat.Rows.Count > 0)
        //                    {
        //                        Beatname = DTBeat.Rows[0]["PARNAM"].ToString();
        //                    }
        //                }

        //                if (Dtsupport.Rows.Count > 0)
        //                {
        //                    Dtsupport.DefaultView.RowFilter = "SNO =7 and LCODE='" + Dtorderfilter.Rows[i]["ORDNO"].ToString() + "'";
        //                    DTState = Dtsupport.DefaultView.ToTable();
        //                    if (DTState.Rows.Count > 0)
        //                    {
        //                        Dtsaletype.DefaultView.RowFilter = "SGCODE ='STATE1' and SCODE='" + DTState.Rows[0]["REMARK"].ToString().Substring(DTState.Rows[0]["REMARK"].ToString().Length - 6).Trim() + "'";
        //                        DTState = Dtsaletype.DefaultView.ToTable();
        //                        if (DTState.Rows.Count > 0)
        //                        {
        //                            Statename = DTState.Rows[0]["PARNAM"].ToString();
        //                        }
        //                    }
        //                }


        //                cnt = DB.InsertParty_Marg(connectionstring3, Dtorderfilter.Rows[i]["PARNAM"].ToString(), DtDistributor.Rows[0]["PartyName"].ToString(), Dtorderfilter.Rows[i]["PARADD"].ToString(), Dtorderfilter.Rows[i]["PARADD1"].ToString(), cityid, Areaid, beatid, Convert.ToInt32(Dt.Rows[0]["DistributorID"].ToString()), Dtorderfilter.Rows[i]["DSM"].ToString(), Dtorderfilter.Rows[i]["PHONE4"].ToString(), Dtorderfilter.Rows[i]["PHONE1"].ToString(), "", Dtorderfilter.Rows[i]["ORDNO"].ToString(), true, "", 0, Dtorderfilter.Rows[i]["CONFIR"].ToString() + " " + Dtorderfilter.Rows[i]["CONLAS"].ToString(), Dtorderfilter.Rows[i]["STNO"].ToString(), Dtorderfilter.Rows[i]["CSTNO"].ToString(), "", Dtorderfilter.Rows[i]["ITNO"].ToString(), Convert.ToDecimal(Dtorderfilter.Rows[i]["LIMIT"].ToString()), 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), "", "", Email, "", Dtorderfilter.Rows[i]["GSTNO"].ToString(), Convert.ToInt32(DTadmin.Rows[0]["SMID"]), Dtorderfilter.Rows[i]["GSTNO"].ToString(), "", Statename, Areaname, Beatname, 0, regionid, distictid, cityid, cityconveyancetype, Convert.ToInt32(Dt.Rows[0]["DistributorID"].ToString()), "INDIA");

        //                if (cnt > 0)
        //                {

        //                    ms.msdmsg = Dtorderfilter.Rows[i]["PARNAM"].ToString() + " is inserted successfully";
        //                    msglist.Add(ms);
        //                    success = success + 1;

        //                }
        //                else if (cnt == -1)
        //                {
        //                    ms.msdmsg = Dtorderfilter.Rows[i]["PARNAM"].ToString() + " is not inserted,Already exist in login Mast";
        //                    msglist.Add(ms);
        //                    failure = failure + 1;

        //                }
        //                else if (cnt == -3)
        //                {
        //                    ms.msdmsg = Dtorderfilter.Rows[i]["PARNAM"].ToString() + " is not inserted,Duplicate Mobile No.";
        //                    msglist.Add(ms);
        //                    failure = failure + 1;

        //                }
        //                else if (cnt == -2)
        //                {
        //                    int retval = DB.UpdateParty_Marg(connectionstring3, Dtorderfilter.Rows[i]["PARNAM"].ToString(), DtDistributor.Rows[0]["PartyName"].ToString(), Dtorderfilter.Rows[i]["PARADD"].ToString(), Dtorderfilter.Rows[i]["PARADD1"].ToString(), cityid, Areaid, beatid, Convert.ToInt32(Dt.Rows[0]["DistributorID"].ToString()), Dtorderfilter.Rows[i]["DSM"].ToString(), Dtorderfilter.Rows[i]["PHONE4"].ToString(), Dtorderfilter.Rows[i]["PHONE4"].ToString(), "", Convert.ToInt32(Dt.Rows[0]["DistributorID"].ToString()) + "#" + Dtorderfilter.Rows[i]["ORDNO"].ToString(), true, "", 0, Dtorderfilter.Rows[i]["CONFIR"].ToString() + " " + Dtorderfilter.Rows[i]["CONLAS"].ToString(), Dtorderfilter.Rows[i]["STNO"].ToString(), Dtorderfilter.Rows[i]["CSTNO"].ToString(), "", Dtorderfilter.Rows[i]["ITNO"].ToString(), Dtorderfilter.Rows[i]["LIMIT"].ToString() == "" ? 0 : Convert.ToDecimal(Dtorderfilter.Rows[i]["LIMIT"].ToString()), 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), "", "", Email, "", Dtorderfilter.Rows[i]["GSTNO"].ToString(), Convert.ToInt32(DTadmin.Rows[0]["SMid"].ToString()), Dtorderfilter.Rows[i]["GSTNO"].ToString(), "",
        //                     "INDIA", Statename, Areaname, Beatname, 0, regionid, distictid, citytypeid, cityconveyancetype, Convert.ToInt32(Dt.Rows[0]["DistributorID"].ToString()));


        //                    if (retval > 0)
        //                    {

        //                        ms.msdmsg = Dtorderfilter.Rows[i]["PARNAM"].ToString() + " is updated successfully";

        //                        msglist.Add(ms);
        //                        success = success + 1;
        //                    }
        //                    else
        //                    {
        //                        ms.msdmsg = Dtorderfilter.Rows[i]["PARNAM"].ToString() + " is not updated,Something went wrong";
        //                        msglist.Add(ms);
        //                        failure = failure + 1;
        //                        // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Something went wrong");
        //                    }

        //                }
        //                else
        //                {
        //                    ms.msdmsg = Dtorderfilter.Rows[i]["PARNAM"].ToString() + " is not inserted,Something went wrong";
        //                    msglist.Add(ms);
        //                    failure = failure + 1;

        //                }
        //                Email = "";
        //                Areaname = "";
        //                Statename = "";
        //                Beatname = "";

        //            }
        //            str += success + " record inserted successfully <br/>";
        //            if (failure > 0)
        //                str = str + "," + failure + " record are failed";
        //            //if (err > 0)
        //            //    str = str + ",Check log ";
        //            if (msglist.Count == 0)
        //            {
        //                mandatorymsg ms = new mandatorymsg();
        //                ms.msdmsg = "No error";
        //                msglist.Add(ms);
        //            }

        //            rs.msg = str;
        //            rs.errormsg = msglist;

        //        }


        //    }
        //    catch (Exception ex)
        //    {

        //        cnt = 0;
        //        str = ex.Message;
        //        //err = err + 1;
        //        //LogError(str);
        //        //// msglist..Add(str);
        //        //if (err > 0)
        //        str = str + ",Check log ";
        //        rs.msg = str;
        //        rs.errormsg = msglist;
        //    }

        //    if (str != "")
        //    {
        //        //    SendEmail("Regarding Log Detail of Retailer Transfer from tally", rs, str);
        //    }

        //}

        //private static void MargSyncRetailerinvoice(DataTable Dt)
        //{
        //    DataTable Dtallinvoice = Convertcsvtodatatable("C:\\INDIADATA\\PENDINGS.csv");
        //    DataTable DtGetallItem = Convertcsvtodatatable("C:\\INDIADATA\\DIS.csv");
        //    string DistInvDocId = "";
        //    int DistInvId = 0;
        //    int DistInv1Id = 0;
        //    int DistInv2Id = 0;
        //    decimal Taxamt = 0;
        //    int failure = 0;
        //    int success = 0;
        //    int sno = 0;
        //    string str = "";
        //    decimal CGSTAmt = 0;
        //    decimal SGSTAmt = 0;
        //    decimal Discount = 0;
        //    errorresult rs = new errorresult();
        //    ClsProcedure DB = new ClsProcedure();
        //    DataTable DtFilterInvoice = new DataTable();
        //    DataTable DtFilterItem = new DataTable();
        //    DataTable Dtextra = new DataTable();
        //    Dtextra.Columns.Add("Description");
        //    Dtextra.Columns.Add("Amount");
        //    List<mandatorymsg> msglist = new List<mandatorymsg>();
        //    if (Dtallinvoice.Rows.Count > 0)
        //    {
        //        Dtallinvoice.DefaultView.RowFilter = "ACGROUP='C6' and VCN not in('*','')";
        //        DtFilterInvoice = Dtallinvoice.DefaultView.ToTable();
        //    }

        //    DbConnectionDAL.ExecuteQuery("Delete from TransRetailerInv2 where RetInvDocId in (Select RetInvDocId from TransRetailerInv where MargImport='Y' and DistID=" + Dt.Rows[0]["DistributorID"] + ")", connectionstring3);
        //    DbConnectionDAL.ExecuteQuery("Delete from TransRetailerInv1 where RetInvDocId in (Select RetInvDocId from TransRetailerInv where MargImport='Y' and DistID=" + Dt.Rows[0]["DistributorID"] + ")", connectionstring3);
        //    DbConnectionDAL.ExecuteQuery("Delete from TransRetailerInv where MargImport='Y' and DistID=" + Dt.Rows[0]["DistributorID"] + "", connectionstring3);
        //    if (DtFilterInvoice.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < DtFilterInvoice.Rows.Count; i++)
        //        {
        //            mandatorymsg ms = new mandatorymsg();
        //            if (DtGetallItem.Rows.Count > 0)
        //            {
        //                DtGetallItem.DefaultView.RowFilter = "VCN='" + DtFilterInvoice.Rows[i]["VCN"].ToString() + "'";
        //                DtFilterItem = DtGetallItem.DefaultView.ToTable();
        //                for (int Z = 0; Z < DtFilterItem.Rows.Count; Z++)
        //                {
        //                    Taxamt += Convert.ToDecimal(DtFilterItem.Rows[Z]["SSTAAMO"]);
        //                }
        //                // Taxamt = DtFilterItem.AsEnumerable().Sum(row => row.Field<decimal>("SSTAAMO"));
        //                //Taxamt = Convert.ToDecimal(DtFilterItem.Compute("Sum(Convert(SSTAAMO, 'System.Decimal')", string.Empty));
        //            }
        //            CultureInfo provider = CultureInfo.InvariantCulture;

        //            DateTime dateTime10 = DateTime.ParseExact(DtFilterInvoice.Rows[i]["DDATE"].ToString(), "dd/MM/yyyy", null);


        //            //   string dd=DateTime.Parse(DtFilterInvoice.Rows[i]["DDATE"].ToString(),"dd/MM/yyyy")
        //            DistInvDocId = "Marg" + " " + Convert.ToString(dateTime10.Year) + " " + DtFilterInvoice.Rows[i]["VCN"].ToString().Substring(4, DtFilterInvoice.Rows[i]["VCN"].ToString().Length - 4);
        //            DistInvId = DB.insertRetailersaleinvheader_Marg(connectionstring3, DistInvDocId, dateTime10.ToString(), Taxamt, Convert.ToDecimal(DtFilterInvoice.Rows[i]["FINAL"]), 0, Convert.ToDecimal(DtFilterInvoice.Rows[i]["ITEMISS"]), Dt.Rows[0]["DistributorID"].ToString() + "#" + DtFilterInvoice.Rows[i]["ORD"].ToString(), Convert.ToInt32(Dt.Rows[0]["DistributorID"]));

        //            if (DistInvId == -2)
        //            {
        //                ms.msdmsg = "Party -" + DtFilterInvoice.Rows[i]["NAME"].ToString() + " not exist on web portal for Bill no " + DtFilterInvoice.Rows[i]["VCN"].ToString();
        //                msglist.Add(ms);
        //                failure = failure + 1;

        //            }
        //            else
        //            {

        //                for (int j = 0; j < DtFilterItem.Rows.Count; j++)
        //                {

        //                    sno = sno + 1;
        //                    DistInv1Id = DB.insertRetailersaleinvdetail_Marg(connectionstring3, DistInvId, sno, DistInvDocId, dateTime10.ToString(), Convert.ToDecimal(DtFilterItem.Rows[j]["SSTAAMO"].ToString()), Convert.ToDecimal(DtFilterItem.Rows[j]["QTY"].ToString()), Convert.ToDecimal(DtFilterItem.Rows[j]["RATE"].ToString()), Convert.ToDecimal(DtFilterItem.Rows[j]["QTY"].ToString()) * Convert.ToDecimal(DtFilterItem.Rows[j]["RATE"].ToString()), Dt.Rows[0]["DistributorID"].ToString() + "#" + DtFilterInvoice.Rows[i]["ORD"].ToString(), Convert.ToDecimal(DtFilterItem.Rows[j]["DISC1"].ToString()), Convert.ToDecimal(DtFilterItem.Rows[j]["SDISCOUNT"].ToString()), Convert.ToDecimal(DtFilterItem.Rows[j]["CODE"].ToString()), "", Convert.ToInt32(Dt.Rows[0]["DistributorID"]));
        //                    if (DistInv1Id > 0)
        //                    {

        //                        CGSTAmt = CGSTAmt + Convert.ToDecimal(DtFilterItem.Rows[j]["CGSTAMO"].ToString());
        //                        SGSTAmt = SGSTAmt + Convert.ToDecimal(DtFilterItem.Rows[j]["CGSTAMO"].ToString());
        //                        Discount = Discount + Convert.ToDecimal(DtFilterItem.Rows[j]["SDISCOUNT"].ToString());

        //                    }
        //                }

        //                Dtextra.Rows.Clear();
        //                Dtextra.Rows.Add("CGST Amount", CGSTAmt);
        //                Dtextra.Rows.Add("SGST Amount", SGSTAmt);

        //                Dtextra.Rows.Add("Discount", Discount);
        //                for (int j = 0; j < Dtextra.Rows.Count; j++)
        //                {
        //                    DistInv2Id = DB.insertsaleinvexpensedetail_Marg(connectionstring3, DistInvDocId, Dtextra.Rows[j][0].ToString(), Convert.ToDecimal(Dtextra.Rows[j][1].ToString()));
        //                }
        //            }
        //            Taxamt = 0;
        //            CGSTAmt = 0;
        //            SGSTAmt = 0;
        //            Discount = 0;
        //            success = success + 1;
        //        }

        //        str += success + " record inserted successfully <br/>";
        //        if (failure > 0)
        //            str = str + "," + failure + " record are failed";
        //        //if (err > 0)
        //        //    str = str + ",Check log ";
        //        if (msglist.Count == 0)
        //        {
        //            mandatorymsg ms = new mandatorymsg();
        //            ms.msdmsg = "No error";
        //            msglist.Add(ms);
        //        }

        //        rs.msg = str;
        //        rs.errormsg = msglist;


        //    }
        //}

        //private static void MargSyncItem(DataTable Dt)
        //{
        //    DataTable Dtitem = new DataTable();
        //    try
        //    {

        //        Dtitem = Convertcsvtodatatable("C:\\INDIADATA\\PRO.csv");
        //        Dtitem.TableName = "PRO";
        //        SqlConnection sqlconn = new SqlConnection(connectionstring3);
        //        sqlconn.Open();
        //        SqlCommand cmd = new SqlCommand("Truncate Table PRO ", sqlconn);
        //        cmd.ExecuteNonQuery();
        //        SqlBulkCopy sqlblk = new SqlBulkCopy(sqlconn);
        //        sqlblk.DestinationTableName = "PRO";

        //        sqlblk.ColumnMappings.Add("[SELECT]", "[SELECT]");

        //        sqlblk.ColumnMappings.Add("CODE", "CODE");
        //        sqlblk.ColumnMappings.Add("NAME", "NAME");
        //        sqlblk.ColumnMappings.Add("BILLNAME", "BILLNAME");
        //        sqlblk.ColumnMappings.Add("PRODUCT", "PRODUCT");
        //        sqlblk.ColumnMappings.Add("PACKING", "PACKING");
        //        sqlblk.ColumnMappings.Add("UNIT", "UNIT");

        //        sqlblk.ColumnMappings.Add("PACK", "PACK");


        //        sqlblk.ColumnMappings.Add("OPENING", "OPENING");
        //        sqlblk.ColumnMappings.Add("BALANCE", "BALANCE");


        //        sqlblk.ColumnMappings.Add("QTY", "QTY");
        //        sqlblk.ColumnMappings.Add("TQTY", "TQTY");
        //        sqlblk.ColumnMappings.Add("LPRATE", "LPRATE");

        //        sqlblk.ColumnMappings.Add("RATEA", "RATEA");
        //        sqlblk.ColumnMappings.Add("RATEB", "RATEB");
        //        sqlblk.ColumnMappings.Add("RATEC", "RATEC");
        //        sqlblk.ColumnMappings.Add("RATED", "RATED");


        //        sqlblk.ColumnMappings.Add("GCODE", "GCODE");
        //        sqlblk.ColumnMappings.Add("DEAL", "DEAL");
        //        sqlblk.ColumnMappings.Add("FREE", "FREE");
        //        sqlblk.ColumnMappings.Add("PRATE", "PRATE");
        //        sqlblk.ColumnMappings.Add("MRP", "MRP");


        //        sqlblk.ColumnMappings.Add("CGST", "CGST");
        //        sqlblk.ColumnMappings.Add("IGST", "IGST");

        //        sqlblk.WriteToServer(Dtitem);
        //        sqlconn.Close();

        //    }
        //    catch (Exception ex)
        //    {

        //        // throw;
        //    }


        //}
        //private static DataTable Convertcsvtodatatable(string strFilePath)
        //{
        //    DataTable dt = new DataTable();
        //    if (File.Exists(strFilePath))
        //    {


        //        StreamReader sr = new StreamReader(strFilePath);
        //        string[] headers = sr.ReadLine().Split(',');

        //        foreach (string header in headers)
        //        {
        //            dt.Columns.Add(header);
        //        }
        //        try
        //        {
        //            while (!sr.EndOfStream)
        //            {
        //                string[] rows = sr.ReadLine().Split(',');
        //                DataRow dr = dt.NewRow();
        //                for (int i = 0; i < headers.Length; i++)
        //                {

        //                    dr[i] = rows[i];
        //                }
        //                dt.Rows.Add(dr);
        //            }
        //            sr.Close();
        //        }
        //        catch (Exception ex)
        //        {
        //            ex.ToString();
        //        }
        //    }
        //    return dt;
        //    //  return Dt;

        //}








        //        private static Purchorderlist PostPurchOrderToBusy()
        //        {
        //            DataTable DTpurchorder = new DataTable();
        //            DataTable DTpurchorder1 = new DataTable();
        //            List<Transpurchorder> purchorder = new List<Transpurchorder>();
        //            Purchorderlist POL = new Purchorderlist();
        //            decimal _amt = 0;
        //            string _query;
        //            //MessageBox.Show("run5");
        //            ClsProcedure DB = new ClsProcedure();

        //            _query = @"Select POrdId,PODocid,VDate,SyncId as distid,Remarks,CreatedDate,SUBSTRING([PartyName],1, CHARINDEX([Compcode],[PartyName])-3) As PartyName,CompCode from [TransPurchOrder] TP
        //            Left join MastParty MP on MP.PartyId=TP.DistId where Isnull(SyncId,'')<>''  "; //and TP.POrdId>" + pid + "
        //            DTpurchorder = DbConnectionDAL.GetDataTable(CommandType.Text, _query, connectionstring3);
        //            for (int i = 0; i < DTpurchorder.Rows.Count; i++)
        //            {
        //                Transpurchorder TP = new Transpurchorder();
        //                _query = @"Select PODocid,VDate, MI.ItemName as ItemName,MI.ItemId As item,(Case when Isnull(Rate,0)=0 then Isnull(MI.DP,0) else Isnull(Rate,0) end)  As Rate ,Qty
        //,SNo,MI.unit As Unit from TransPurchOrder1 TP1
        //                   Left join  MastItem MI on MI.ItemId=TP1.ItemId  where Isnull(SyncId,'')<>'' and PODocid='" + DTpurchorder.Rows[i]["PODocid"].ToString() + "' ";
        //                DTpurchorder1 = DbConnectionDAL.GetDataTable(CommandType.Text, _query, connectionstring3);

        //                TP.Docid = DTpurchorder.Rows[i]["PODocid"].ToString();
        //                TP.VDate = Convert.ToDateTime(DTpurchorder.Rows[i]["VDate"].ToString()).ToString("dd-MM-yyyy");
        //                TP.Remark = DTpurchorder.Rows[i]["Remarks"].ToString();
        //                TP.Distid = DTpurchorder.Rows[i]["distid"].ToString();
        //                TP.Partyname = DTpurchorder.Rows[i]["PartyName"].ToString();
        //                TP.createddate = Convert.ToDateTime(DTpurchorder.Rows[i]["CreatedDate"].ToString());
        //                TP.Pordid = Convert.ToInt32(DTpurchorder.Rows[i]["POrdId"].ToString());
        //                TP.Vno = "Sales/" + DTpurchorder.Rows[i]["POrdId"].ToString();
        //                TP.referenceno = "Sales/" + DTpurchorder.Rows[i]["POrdId"].ToString() + "/" + DTpurchorder.Rows[i]["distid"].ToString();
        //                List<Transpurchorder1> purchorder1 = new List<Transpurchorder1>();
        //                for (int j = 0; j < DTpurchorder1.Rows.Count; j++)
        //                {
        //                    Transpurchorder1 TP1 = new Transpurchorder1();
        //                    TP1.Docid = DTpurchorder1.Rows[j]["PODocid"].ToString();
        //                    TP1.VDate = Convert.ToDateTime(DTpurchorder1.Rows[j]["VDate"].ToString());
        //                    TP1.itemid = DTpurchorder1.Rows[j]["ItemName"].ToString().Trim();
        //                    TP1.Quantity = Convert.ToDecimal(DTpurchorder1.Rows[j]["Qty"].ToString());
        //                    TP1.Rate = Convert.ToDecimal(DTpurchorder1.Rows[j]["Rate"].ToString());
        //                    TP1.sno = Convert.ToInt32(DTpurchorder1.Rows[j]["SNo"].ToString());
        //                    TP1.Amount = (Convert.ToDecimal(DTpurchorder1.Rows[j]["Qty"].ToString()) * Convert.ToDecimal(DTpurchorder1.Rows[j]["Rate"].ToString()));
        //                    _amt = _amt + TP1.Amount;
        //                    TP1.Unit = DTpurchorder1.Rows[j]["Unit"].ToString();
        //                    purchorder1.Add(TP1);

        //                }
        //                TP.totalamount = _amt;
        //                TP.purchorder1 = purchorder1;
        //                purchorder.Add(TP);

        //                POL.result = purchorder;
        //                _amt = 0;
        //            }
        //            //MessageBox.Show(POL.ToString());
        //            return POL;

        //        }

        //        private static void BusySalesOrderSyncData(Purchorderlist pol_lst)
        //        {

        //            List<Result1> rst = new List<Result1>();
        //            decimal _amount = 0;
        //            decimal amtdr = 0;
        //            decimal amtcr = 0;
        //            string docID;
        //            ClsProcedure DB = new ClsProcedure();
        //            string Vtype = "";
        //            string Narration = "";
        //            DataTable dt = new DataTable();
        //            string str = "";
        //            int result = 0;
        //            int success = 0;
        //            int failure = 0;
        //            int err = 0;
        //            int sno = 0;
        //            string companycode = "";
        //            List<mandatorymsg> msglist = new List<mandatorymsg>();
        //            errorresult rs = new errorresult();
        //            try
        //            {
        //                //MessageBox.Show(pol_lst.result.Count.ToString() + " " + dbFlag.ToString());
        //                if (pol_lst.result.Count > 0)
        //                {
        //                    if (dbFlag == "A")
        //                    {

        //                        OleDbDataAdapter oledbAdapter = new OleDbDataAdapter();
        //                        busy_Conn2.Open();


        //                        for (int i = 0; i < pol_lst.result.Count; i++)
        //                        {

        //                            string vch_query = "select max(VchCode) as cnt from Tran1";

        //                            DataTable dt_vchcode = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, vch_query, dbFlag);
        //                            int vcode = 0;// Convert.ToInt32(dt_vchcode.Rows[0]["cnt"].ToString());
        //                            string VCH_NO = "VCH" + pol_lst.result[i].Docid.Split(' ')[1] + pol_lst.result[i].Docid.Split(' ')[2];
        //                            //MessageBox.Show(VCH_NO.ToString());

        //                            string _query = "INSERT INTO Tran1 (VchCode,VchType,[Date],VchNo,VchSeriesCode,MasterCode1,MasterCode2,Stamp,CM1,VchAmtBaseCur,VchSalePurcAmt,OrgVchAmtBaseCur,[CreationTime],InputType) VALUES(" + vcode + ",12,'" + pol_lst.result[i].createddate + "','" + VCH_NO + "',261," + pol_lst.result[i].Distid + ",201,1,1219," + pol_lst.result[i].totalamount + "," + pol_lst.result[i].totalamount + "," + pol_lst.result[i].totalamount + ",'" + pol_lst.result[i].createddate + "',1)";

        //                            oledbAdapter.InsertCommand = new OleDbCommand(_query, busy_Conn2);
        //                            oledbAdapter.InsertCommand.ExecuteNonQuery();

        //                            if (pol_lst.result[i].purchorder1.Count > 0)
        //                            {
        //                                for (int j = 0; j < pol_lst.result[i].purchorder1.Count; j++)
        //                                {
        //                                    string query_unit = "select code from Master1 where Name='" + pol_lst.result[i].purchorder1[j].Unit + "'";
        //                                    DataTable dt_unit = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, query_unit, dbFlag);

        //                                    _query = "INSERT INTO Tran2(RecType,VchCode,MasterCode1,MasterCode2,SrNo,VchType,[Date],VchNo,VchSeriesCode,Value1,Value2,Value3,D1,D2,D3,D4,D5,D6,D8,D18,CM1,CM2,CM3,CM4) VALUES(4," + vcode + "," + pol_lst.result[i].purchorder1[j].itemid + ",201,1,12,'" + pol_lst.result[i].purchorder1[j].VDate + "','" + VCH_NO + "',261," + pol_lst.result[i].purchorder1[j].Quantity + "," + pol_lst.result[i].purchorder1[j].Quantity + ",pol_lst.result[i].totalamount," + pol_lst.result[i].purchorder1[j].Quantity + "," + pol_lst.result[i].purchorder1[j].Rate + "," + pol_lst.result[i].purchorder1[j].Rate + "," + pol_lst.result[i].purchorder1[j].Rate + "," + pol_lst.result[i].totalamount + "," + pol_lst.result[i].purchorder1[j].Rate + "," + pol_lst.result[i].purchorder1[j].Rate + "," + pol_lst.result[i].Distid + "," + Convert.ToInt32(dt_unit.Rows[0]["code"].ToString()) + "," + Convert.ToInt32(dt_unit.Rows[0]["code"].ToString()) + ",1219)";

        //                                    oledbAdapter.InsertCommand = new OleDbCommand(_query, busy_Conn2);
        //                                    oledbAdapter.InsertCommand.ExecuteNonQuery();
        //                                }
        //                            }



        //                        }
        //                        busy_Conn2.Close();
        //                    }
        //                    else
        //                    {

        //                        for (int i = 0; i < pol_lst.result.Count; i++)
        //                        {
        //                            string VCH_NO = "VCH" + pol_lst.result[i].Docid.Split(' ')[1] + pol_lst.result[i].Docid.Split(' ')[2];
        //                            int vcode = Convert.ToInt32(DbConnectionDAL.GetScalarValueBusy(CommandType.Text, "SELECT max(VchCode) from Tran1", connectionstring2));

        //                            string _query = "INSERT INTO Tran1 (VchCode,VchType,Date,VchNo,VchSeriesCode,MasterCode1,MasterCode2,Stamp,CM1,VchAmtBaseCur,VchSalePurcAmt,OrgVchAmtBaseCur,CreationTime,InputType) VALUES(" + (vcode + 1) + ",12,'" + pol_lst.result[i].createddate + "','" + VCH_NO + "',261," + pol_lst.result[i].Distid + ",201,1,1219," + pol_lst.result[i].totalamount + "," + pol_lst.result[i].totalamount + "," + pol_lst.result[i].totalamount + ",'" + pol_lst.result[i].createddate + "',1)";

        //                            int cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query, connectionstring2));
        //                            if (cnt > 0)
        //                            {
        //                                if (pol_lst.result[i].purchorder1.Count > 0)
        //                                {
        //                                    for (int j = 0; j < pol_lst.result[i].purchorder1.Count; j++)
        //                                    {
        //                                        string query_unit = "select code from Master1 where Name='" + pol_lst.result[i].purchorder1[j].Unit + "'";
        //                                        DataTable dt_unit = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, query_unit, dbFlag);

        //                                        _query = "INSERT INTO Tran2(RecType,VchCode,MasterCode1,MasterCode2,SrNo,VchType,Date,VchNo,VchSeriesCode,Value1,Value2,Value3,D1,D2,D3,D4,D5,D6,D8,D18,CM1,CM2,CM3,CM4) VALUES(4," + vcode + "," + pol_lst.result[i].purchorder1[j].itemid + ",201,1,12,'" + pol_lst.result[i].purchorder1[j].VDate + "','" + VCH_NO + "',261," + pol_lst.result[i].purchorder1[j].Quantity + "," + pol_lst.result[i].purchorder1[j].Quantity + ",pol_lst.result[i].totalamount," + pol_lst.result[i].purchorder1[j].Quantity + "," + pol_lst.result[i].purchorder1[j].Rate + "," + pol_lst.result[i].purchorder1[j].Rate + "," + pol_lst.result[i].purchorder1[j].Rate + "," + pol_lst.result[i].totalamount + "," + pol_lst.result[i].purchorder1[j].Rate + "," + pol_lst.result[i].purchorder1[j].Rate + "," + pol_lst.result[i].Distid + "," + Convert.ToInt32(dt_unit.Rows[0]["code"].ToString()) + "," + Convert.ToInt32(dt_unit.Rows[0]["code"].ToString()) + ",1219)";

        //                                        cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query, connectionstring2));
        //                                        if (cnt > 0)
        //                                        {

        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                str = success + " record inserted successfully.";
        //                if (failure > 0)
        //                    str = str + "," + failure + " record are failed";
        //                if (err > 0)
        //                    str = str + ",Check log table";
        //                if (msglist.Count == 0)
        //                {
        //                    mandatorymsg ms = new mandatorymsg();
        //                    ms.msdmsg = "No error";
        //                    msglist.Add(ms);
        //                }

        //                LogError(str, "Success", "BusyLedgerSyncData");

        //                rs.msg = str;
        //                rs.errormsg = msglist;
        //                // this.SucessEmailStatus();
        //            }
        //            catch (Exception ex)
        //            {
        //                //transactionScope.Dispose();

        //                str = ex.Message;
        //                err = err + 1;
        //                LogError(str, "Fail", "BusyLedgerSyncData");
        //                // msglist..Add(str);
        //                if (err > 0)
        //                    str = str + ",Check log table";
        //                rs.msg = str;
        //                rs.errormsg = msglist;

        //            }
        //            //return rs;
        //        }

    }
}
