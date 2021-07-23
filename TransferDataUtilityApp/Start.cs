using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferDataUtilityApp.CommonClass;
using System.Threading;
using System.Net;

namespace TransferDataUtilityApp
{
    public partial class Start : Form
    {
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

        public static string db = "";
        public static string dbpath = "";
        public static string accfle = "";
        public static string dbpass = "";

        public static string dest = "";
        public static string destdb = "";
        public static string usr = "";
        public static string pass = "";
        public static string id = "";
        public static string trndfr = "";

        static string dbFlag = "";
        static int Flag = 0;
        static string SDID = "0";


        //BusyIntegrate.InsertItems_BusyCompletedEventArgs obj = new InsertItemBusy();

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

        public Start()
        {
            InitializeComponent();
            connectionstr = ConfigurationManager.ConnectionStrings["AccessString"].ConnectionString;
            Conn = new OleDbConnection(connectionstr);

        }

        private void Start_Load(object sender, EventArgs e)
        {

            //int cnt = 0;
            //string strSQL = "SELECT count(*) as cnt FROM Enviro";
            //OleDbDataAdapter myCmd = new OleDbDataAdapter(strSQL, Conn);
            //Conn.Open();
            //DataSet dtSet = new DataSet();
            //myCmd.Fill(dtSet, "Enviro");
            //DataTable dTable = dtSet.Tables[0];
            //cnt = Convert.ToInt32(dTable.Rows[0]["cnt"].ToString());
            //if (cnt > 0)
            //{
            //    strSQL = "SELECT * FROM Enviro";
            //    myCmd = new OleDbDataAdapter(strSQL, Conn);
            //    dtSet = new DataSet();
            //    myCmd.Fill(dtSet, "Enviro");
            //    DataTable dTabledata = dtSet.Tables[0];
            //    string source = dTabledata.Rows[0]["Source"].ToString();
            //    if (source.ToLower() == "busy")
            //    {
            //        createBusyConnectionString(dTabledata);
            //        PostItemsToBusy();
            //        BusyAccountSyncData();
            //        BusySalesSyncData();
            //        BusyLedgerSyncData(dTabledata);
            //        Purchorderlist POL = PostPurchOrderToBusy();
            //        BusySalesOrderSyncData(POL);
            //    }
            //    else if (source.ToLower() == "marg")
            //    {
            //        connectionstring3 = "Data Source=" + dTabledata.Rows[0]["DestinationServer"].ToString() + ";Initial Catalog=" + dTabledata.Rows[0]["DestinationDatabase"].ToString() + ";user id=" + dTabledata.Rows[0]["DestinationUser"].ToString() + "; pwd=" + dTabledata.Rows[0]["DestinationPassword"].ToString() + ";";
            //        //   MargSyncRetailer(dTabledata);
            //        MargSyncItem(dTabledata);

            //    }
            //}
            //else
            //{
            //    MainApplication ma = new MainApplication();
            //    ma.Show();
            //    this.Hide();
            //}
            // Conn.Close();
            // Application.Exit();
        }

        public void CheckandtransferData()
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
                    strSQL = "SELECT * FROM Enviro";
                    myCmd = new OleDbDataAdapter(strSQL, Conn);
                    dtSet = new DataSet();
                    myCmd.Fill(dtSet, "Enviro");
                    DataTable dTabledata = dtSet.Tables[0];

                    db = dTabledata.Rows[0]["BusyDatabase"].ToString();
                    dbpath = dTabledata.Rows[0]["Busy_AccessDbPath"].ToString();
                    accfle = dTabledata.Rows[0]["Busy_AccessFileName1"].ToString();
                    dbpass = dTabledata.Rows[0]["Busy_AccessDbPassword"].ToString();

                    dest = dTabledata.Rows[0]["DestinationServer"].ToString();
                    destdb = dTabledata.Rows[0]["DestinationDatabase"].ToString();
                    usr = dTabledata.Rows[0]["DestinationUser"].ToString();
                    pass = dTabledata.Rows[0]["DestinationPassword"].ToString();
                    id = dTabledata.Rows[0]["DistributorID"].ToString();
                    trndfr = dTabledata.Rows[0]["Transferfor"].ToString();

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

        private void createBusyConnectionString(DataTable dt)
        {
            comboBox1.Items.Clear();
            //Start st = new Start();
            connectionstring1 = "";
            connectionstring2 = "";
            connectionstring3 = "";
            string compcode = "";
            string compfile = "";

            connectionstring3 = dt.Rows[0]["DestinationServer"].ToString().Substring(0, dt.Rows[0]["DestinationServer"].ToString().Length);

            //dest_sqlcon = new SqlConnection(connectionstring3);



            SDID = dt.Rows[0]["DistributorID"].ToString();

            if (dt.Rows[0]["BusyDatabase"].ToString().ToLower() == "ms access")
            {
                dbFlag = "A";
                string[] authorsList = dt.Rows[0]["Busy_AccessFileName1"].ToString().Split(new Char[] { ',', '\n' });
                comboBox1.Items.Add(" ");
                for (int i = 0; i < authorsList.Length; i++)
                {

                    //connectionstring1 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dt.Rows[0]["Busy_AccessDbPath"].ToString() + "\\" + authorsList[i].ToString() + ";Jet OLEDB:Database Password=" + dt.Rows[0]["Busy_AccessDbPassword"].ToString() + "";
                    //compcode = authorsList[i].Substring(0, authorsList[i].IndexOf('\\'));
                    //compfile = authorsList[i].ToString();

                    comboBox1.Items.Add(authorsList[i].ToString());

                    //label1.Text = "Transferring " + compfile.ToString() + " for " + compcode;
                    //st.progressBar1.Minimum = 0;
                    //st.progressBar1.Maximum = authorsList.Length;

                    //busy_Conn1 = new OleDbConnection(connectionstring1);

                    //busy_Conn1.Open();
                    //if (busy_Conn1.State == ConnectionState.Open)
                    //{
                    //    //st.ShowDialog();
                    //    //st.progressBar1.Value = i;

                    //    Flag = Flag + 1;

                    //    PostItemsToBusy(compcode, compfile);
                    //    BusyAccountSyncData(compcode, compfile);
                    //    BusySalesSyncData(compcode, compfile);
                    //    BusyLedgerSyncData(compcode, compfile);
                    //}
                    //busy_Conn1.Close();
                }
                //this.Hide();




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

        private void PostItemsToBusy(string CompCode, string CompFile)
        {
            List<Result1> rst = new List<Result1>();
            DataTable DTItems = new DataTable();
            decimal _amt = 0;
            int count = 0, count1 = 0, count2 = 0;
            string unt = null;
            int itmid = 0, itmgrpid = 0;
            string _query, _query1, _query2, _query3;
            ClsProcedure DB = new ClsProcedure();
            string str = "";
            int result = 0, result1 = 0;
            int success = 0, success1 = 0;
            int failure = 0, failure1 = 0;
            int itemid = 0;
            int err = 0, err1 = 0;
            string Parent = "";
            mandatorymsg ms = new mandatorymsg(); ;
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
                        _query2 = @"Select * from Master1 where ParentGrp = " + dt_grps.Rows[0]["Code"].ToString() + " OR Name='FREE GIFT'";
                        //_query2 = @"Select * from Master1 where Code = 1196"; 
                        DataTable dt_grp = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query2, dbFlag);

                        if (dt_grp.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt_grp.Rows.Count; i++)
                            {
                                using (WebClient client = new WebClient())
                                {
                                    string PID = "BU#" + dt_grp.Rows[i]["Code"].ToString();

                                    string url = connectionstring3 + "/Busy_Transfer.asmx/GetItemId?ITEMTYPE=" + "MATERIALGROUP" + "&SYNCID=" + PID.Replace("#", "%23");
                                    itemid = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                }

                                if (itemid == 0)
                                {
                                    string active = "0", promoted = "0", Itemcode = "BU#" + dt_grp.Rows[i]["Code"].ToString(), SyncId = "BU#" + dt_grp.Rows[i]["Code"].ToString(), ItemName = dt_grp.Rows[i]["Name"].ToString().Replace("'", "''"), ParentName = "1";

                                    using (WebClient client = new WebClient())
                                    {
                                        ms.msdmsg = ItemName + " in Insert as Item with SyncId" + SyncId + " for Group";
                                        str = ItemName + " in Insert as Item with SyncId" + SyncId + " for Group";
                                        msglist.Add(ms);
                                        //failure = failure + 1;
                                        LogError(str, "InsertItems_Busy", "PostItemsToBusy", CompFile);

                                        string url = connectionstring3 + "/Busy_Transfer.asmx/InsertItems_Busy?ItemName=" + ItemName.Replace("#", "%23").Replace("&", "-") + "&Unit=" + "" + "&Active=" + true + "&StdPack=" + "0" + "&Mrp=" + "0" + "&Dp=" + "0" + "&Rp=" + "0" + "&ParentName=" + ParentName.Replace("#", "%23") + "&ItemCode=" + Itemcode.Replace("#", "%23") + "&Syncid=" + SyncId.Replace("#", "%23") + "&ItemType=" + "MATERIALGROUP" + "&DispName=" + ItemName + "&PriceGroup=" + "" + "&primaryunit=" + "" + "&Secondaryunit=" + "" + "&PrimaryUnitfactor=" + "0" + "&SecondaryUnitfactor=" + "0" + "&MOQ=" + "0" + "&Promoted=" + false + "&cgstper=" + "0" + "&sgstper=" + "0" + "&igstper=" + "0" + "&Segment=" + "0" + "&ProductClass=" + "0" + "&Type=" + "INSERT";
                                        result = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                        //Busy_Transfer.Busy_Transfer obj = new Busy_Transfer.Busy_Transfer();
                                    }
                                }
                                else
                                {
                                    //MessageBox.Show(dt_grp.Rows[i]["Name"].ToString().Replace("'", "''"));
                                    string active = "0", promoted = "0", ItemID = itemid.ToString(), Itemcode = "BU#" + dt_grp.Rows[i]["Code"].ToString(), SyncId = "BU#" + dt_grp.Rows[i]["Code"].ToString(), ItemName = dt_grp.Rows[i]["Name"].ToString().Replace("'", "''"), ParentName = "1";

                                    using (WebClient client = new WebClient())
                                    {
                                        ms.msdmsg = ItemName + " in Update as Item with SyncId" + SyncId + " for Group";
                                        str = ItemName + " in Update as Item with SyncId" + SyncId + " for Group";
                                        msglist.Add(ms);
                                        //failure = failure + 1;
                                        LogError(str, "UpdateItems_Busy", "PostItemsToBusy", CompFile);

                                        string url = connectionstring3 + "/Busy_Transfer.asmx/UpdateItems_Busy?ItemId=" + Convert.ToInt32(ItemID) + "&ItemName=" + ItemName.Replace("#", "%23").Replace("&", "-") + "&Unit=" + "" + "&Active=" + true + "&StdPack=" + "0" + "&Mrp=" + "0" + "&Dp=" + "0" + "&Rp=" + "0" + "&ParentName=" + ParentName.Replace("#", "%23") + "&ItemCode=" + Itemcode.Replace("#", "%23") + "&Syncid=" + SyncId.Replace("#", "%23") + "&ItemType=" + "MATERIALGROUP" + "&DispName=" + ItemName + "&PriceGroup=" + "" + "&primaryunit=" + "" + "&Secondaryunit=" + "" + "&PrimaryUnitfactor=" + "0" + "&SecondaryUnitfactor=" + "0" + "&MOQ=" + "0" + "&Promoted=" + false + "&cgstper=" + "0" + "&sgstper=" + "0" + "&igstper=" + "0" + "&Segment=" + "0" + "&ProductClass=" + "0" + "&Type=" + "INSERT";
                                        result = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                    }
                                }

                                if (result > 0)
                                {
                                    success1 = success1 + 1;

                                    _query3 = @"Select * from Master1 left join Help1 on Master1.CM1=Help1.Code where Master1.ParentGrp = " + dt_grp.Rows[i]["Code"].ToString() + " and Help1.RecType=7";
                                    DataTable dt_gritm = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query3, dbFlag);

                                    if (dt_gritm.Rows.Count > 0)
                                    {
                                        for (int j = 0; j < dt_gritm.Rows.Count; j++)
                                        {
                                            using (WebClient client = new WebClient())
                                            {
                                                string PID = "BU#" + dt_grp.Rows[i]["Code"].ToString();
                                                Parent = "BU#" + dt_grp.Rows[i]["Code"].ToString();
                                                string url = connectionstring3 + "/Busy_Transfer.asmx/GetItemId?ITEMTYPE=" + "MATERIALGROUP" + "&SYNCID=" + PID.Replace("#", "%23");
                                                itmgrpid = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                            }

                                            if (itmgrpid > 0)
                                            {
                                                using (WebClient client = new WebClient())
                                                {
                                                    string PID = "BU#" + dt_gritm.Rows[j]["Master1.Code"].ToString();

                                                    string url = connectionstring3 + "/Busy_Transfer.asmx/GetItemId?ITEMTYPE=" + "ITEM" + "&SYNCID=" + PID.Replace("#", "%23");
                                                    itemid = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                                }

                                                if (itemid == 0)
                                                {
                                                    string Itemcode = "BU#" + dt_gritm.Rows[j]["Master1.Code"].ToString(), SyncId = "BU#" + dt_gritm.Rows[j]["Master1.Code"].ToString(), ItemName = dt_gritm.Rows[j]["Name"].ToString().Replace("'", "''"), ParentName = itmgrpid.ToString(), Unit = dt_gritm.Rows[j]["NameAlias"].ToString(), MRP = dt_gritm.Rows[j]["D2"].ToString(), DP = dt_gritm.Rows[j]["D4"].ToString(), RP = dt_gritm.Rows[j]["D3"].ToString();

                                                    using (WebClient client = new WebClient())
                                                    {
                                                        ms.msdmsg = ItemName + " in Insert as Item with SyncId" + SyncId + " for Item";
                                                        str = ItemName + " in Insert as Item with SyncId" + SyncId + " for Item";
                                                        msglist.Add(ms);
                                                        //failure = failure + 1;
                                                        LogError(str, "InsertItems_Busy", "PostItemsToBusy", CompFile);


                                                        string url = connectionstring3 + "/Busy_Transfer.asmx/InsertItems_Busy?ItemName=" + ItemName.Replace("#", "%23").Replace("&", "-") + "&Unit=" + Unit + "&Active=" + true + "&StdPack=" + "0" + "&Mrp=" + Convert.ToDecimal(MRP) + "&Dp=" + Convert.ToDecimal(DP) + "&Rp=" + Convert.ToDecimal(RP) + "&ParentName=" + ParentName.Replace("#", "%23") + "&ItemCode=" + Itemcode.Replace("#", "%23") + "&Syncid=" + SyncId.Replace("#", "%23") + "&ItemType=" + "ITEM" + "&DispName=" + ItemName + "&PriceGroup=" + "A" + "&primaryunit=" + "" + "&Secondaryunit=" + "" + "&PrimaryUnitfactor=" + "0" + "&SecondaryUnitfactor=" + "0" + "&MOQ=" + "0" + "&Promoted=" + false + "&cgstper=" + "0" + "&sgstper=" + "0" + "&igstper=" + "0" + "&Segment=" + "1" + "&ProductClass=" + "1" + "&Type=" + "INSERT";
                                                        result1 = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                                        //Busy_Transfer.Busy_Transfer obj = new Busy_Transfer.Busy_Transfer();
                                                    }

                                                    if (result1 > 0)
                                                    { count = count + 1; }
                                                    else if (result1 == -1)
                                                    {
                                                        ms.msdmsg = ItemName + " is not Inserted as Item, Duplicate Item Name ";
                                                        str = ItemName + " is not Inserted as Item, Duplicate Item Name";
                                                        msglist.Add(ms);
                                                        failure = failure + 1;
                                                        LogError(str, "Fail", "PostItemsToBusy", CompFile);
                                                        count2 = count2 + 1;
                                                    }
                                                    else if (result1 == -2)
                                                    {
                                                        ms.msdmsg = ItemName + " is not Inserted as Item,Duplicate Sync Id ";
                                                        str = ItemName + " is not Inserted as Item,Duplicate Sync Id ";
                                                        msglist.Add(ms);
                                                        failure = failure + 1;
                                                        LogError(str, "Fail", "PostItemsToBusy", CompFile);
                                                        count2 = count2 + 1;
                                                    }
                                                }
                                                else
                                                {
                                                    string Itemcode = "BU#" + dt_gritm.Rows[j]["Master1.Code"].ToString(), ItemID = itemid.ToString(), SyncId = "BU#" + dt_gritm.Rows[j]["Master1.Code"].ToString(), ItemName = dt_gritm.Rows[j]["Name"].ToString().Replace("'", "''"), ParentName = itmgrpid.ToString(), Unit = dt_gritm.Rows[j]["NameAlias"].ToString(), MRP = dt_gritm.Rows[j]["D2"].ToString(), DP = dt_gritm.Rows[j]["D4"].ToString(), RP = dt_gritm.Rows[j]["D3"].ToString();

                                                    //MessageBox.Show(ItemName.ToString() + " " + Unit.ToString()+" "+ SyncId);
                                                    using (WebClient client = new WebClient())
                                                    {
                                                        ms.msdmsg = ItemName + " in Update as Item with SyncId" + SyncId + " for Item";
                                                        str = ItemName + " in Update as Item with SyncId" + SyncId + " for Item";
                                                        msglist.Add(ms);
                                                        //failure = failure + 1;
                                                        LogError(str, "UpdateItems_Busy", "PostItemsToBusy", CompFile);

                                                        string url = connectionstring3 + "/Busy_Transfer.asmx/UpdateItems_Busy?ItemId=" + Convert.ToInt32(ItemID) + "&ItemName=" + ItemName.Replace("#", "%23").Replace("&", "-") + "&Unit=" + Unit + "&Active=" + true + "&StdPack=" + "0" + "&Mrp=" + Convert.ToDecimal(MRP) + "&Dp=" + Convert.ToDecimal(DP) + "&Rp=" + Convert.ToDecimal(RP) + "&ParentName=" + ParentName.Replace("#", "%23") + "&ItemCode=" + Itemcode.Replace("#", "%23") + "&Syncid=" + SyncId.Replace("#", "%23") + "&ItemType=" + "ITEM" + "&DispName=" + ItemName + "&PriceGroup=" + "A" + "&primaryunit=" + "" + "&Secondaryunit=" + "" + "&PrimaryUnitfactor=" + "0" + "&SecondaryUnitfactor=" + "0" + "&MOQ=" + "0" + "&Promoted=" + false + "&cgstper=" + "0" + "&sgstper=" + "0" + "&igstper=" + "0" + "&Segment=" + "1" + "&ProductClass=" + "1" + "&Type=" + "INSERT";
                                                        result1 = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                                        //Busy_Transfer.Busy_Transfer obj = new Busy_Transfer.Busy_Transfer();
                                                    }

                                                    if (result1 > 0)
                                                    { count1 = count1 + 1; }
                                                    else if (result1 == -1)
                                                    {
                                                        ms.msdmsg = ItemName + " is not updated as Item, Duplicate Item Name ";
                                                        str = ItemName + " is not updated as Item, Duplicate Item Name";
                                                        msglist.Add(ms);
                                                        failure = failure + 1;
                                                        LogError(str, "Fail", "PostItemsToBusy", CompFile);
                                                        count2 = count2 + 1;
                                                    }
                                                    else if (result1 == -2)
                                                    {
                                                        ms.msdmsg = ItemName + " is not updated as Item,Duplicate Sync Id ";
                                                        str = ItemName + " is not updated as Item,Duplicate Sync Id ";
                                                        msglist.Add(ms);
                                                        failure = failure + 1;
                                                        LogError(str, "Fail", "PostItemsToBusy", CompFile);
                                                        count2 = count2 + 1;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                result1 = 0;

                                                ms.msdmsg = "Group not Exist for ITEMTYPE=MATERIALGROUP & SYNCID=" + Parent.Replace("#", "%23");
                                                str = "Group not Exist for ITEMTYPE=MATERIALGROUP & SYNCID=" + Parent.Replace("#", "%23");
                                                msglist.Add(ms);
                                                failure = failure + 1;
                                                LogError(str, "Fail", "PostItemsToBusy", CompFile);
                                                count2 = count2 + 1;
                                            }
                                            if (result1 > 0)
                                            {
                                                success = success + 1;
                                            }
                                            else
                                            {
                                                failure = failure + 1;
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    result = 0;

                                    ms.msdmsg = "Group not Exist for ITEMTYPE=MATERIALGROUP & SYNCID=" + Parent.Replace("#", "%23");
                                    str = "Group not Exist for ITEMTYPE=MATERIALGROUP & SYNCID=" + Parent.Replace("#", "%23");
                                    msglist.Add(ms);
                                    failure1 = failure1 + 1;
                                    LogError(str, "Fail", "PostItemsToBusy", CompFile);
                                    count2 = count2 + 1;
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
                            ms.msdmsg = "No error";
                            msglist.Add(ms);
                        }

                        LogError(str, "Success", "PostItemsToBusy", CompFile);

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
            listBox1.Invoke(new MethodInvoker(() => listBox1.Items.Add(CompFile + " Items Transfered " + count + " Inserted " + count1 + " Updated ")));


        }

        private void BusySalesPerson(string CompCode, string CompFile)
        {
            //label1.Invoke(new MethodInvoker(() => label1.Text = CompFile + " Sales Person Transferring........"));
            List<Result1> rst = new List<Result1>();
            DataTable DTItems = new DataTable();
            decimal _amt = 0;
            int count = 0, count1 = 0, count2 = 0;
            string unt = null;
            int itmid = 0;
            string _query, _query1, _query2, _query3;
            ClsProcedure DB = new ClsProcedure();
            string str = "";
            int val = 0;
            int smid = 0;
            string SyncId = "";
            int retsave = 0, retval = 0;
            int result = 0, result1 = 0;
            int success = 0, success1 = 0;
            int failure = 0, failure1 = 0;
            int err = 0, err1 = 0;
            mandatorymsg ms = new mandatorymsg(); ;
            List<mandatorymsg> msglist = new List<mandatorymsg>();
            errorresult rs = new errorresult();
            try
            {
                if (dbFlag == "A")
                {
                    _query1 = @"Select * from Master1 where MasterType = 19";
                    DataTable dt_grps = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query1, dbFlag);
                    if (dt_grps.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_grps.Rows.Count; i++)
                        {
                            string active = "0";
                            using (WebClient client = new WebClient())
                            {
                                string PID = dt_grps.Rows[i]["Name"].ToString().ToUpper();
                                string EmpSyncId = "BU#" + dt_grps.Rows[i]["Code"].ToString();

                                string url = connectionstring3 + "/Busy_Transfer.asmx/GetLgn?LGNID=" + PID.Replace("#", "%23") + "&SYNC=" + EmpSyncId.Replace("#", "%23");
                                val = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                            }

                            string DesgID1 = "1";
                            if (val == 0)
                            {
                                using (WebClient client = new WebClient())
                                {
                                    SyncId = "BU#" + dt_grps.Rows[i]["Code"].ToString();

                                    string url = connectionstring3 + "/Busy_Transfer.asmx/GetSMID?SMID=" + SyncId.Replace("#", "%23") + "&UID=" + "";
                                    smid = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                }

                                if (smid == 0)
                                {
                                    active = "1";


                                    using (WebClient client = new WebClient())
                                    {
                                        ms.msdmsg = dt_grps.Rows[i]["Name"].ToString() + " in Insert as Sales Person on Login with SyncId" + SyncId;
                                        str = dt_grps.Rows[i]["Name"].ToString() + " in Insert as Sales Person on Login with SyncId" + SyncId;
                                        msglist.Add(ms);
                                        //failure = failure + 1;
                                        LogError(str, "", "BusySalesPerson", CompFile);

                                        string url = connectionstring3 + "/Busy_Transfer.asmx/Insert_Login?UserName=" + dt_grps.Rows[i]["Name"].ToString().Replace("#", "%23") + "&Password=" + "12345678" + "&email=" + "" + "&Roleid=" + Convert.ToInt32("20") + "&isAdmin=" + true + "&DeptId=" + "2" + "&DesgId=" + DesgID1 + "&empName=" + dt_grps.Rows[i]["Name"].ToString().Replace("#", "%23") + "&CostCentre=" + Convert.ToDecimal(0) + "&empSyncId=" + SyncId.Replace("#", "%23");
                                        retsave = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                    }

                                    if (retsave == -2)
                                    {
                                        ms.msdmsg = dt_grps.Rows[i]["Name"].ToString() + " is not Inserted as Duplicate EmpSyncId Exists";
                                        str = dt_grps.Rows[i]["Name"].ToString() + " is not Inserted as Duplicate EmpSyncId Exists";
                                        msglist.Add(ms);
                                        failure = failure + 1;
                                        LogError(str, "Fail", "BusySalesPerson", CompFile);
                                    }

                                    if (retsave > 0)
                                    {
                                        string UserID = retsave.ToString(), ReportTo = "291", DeptID = "2", DesgID = "1",
                                                RoleID = "20", EmailID = "", Employee = dt_grps.Rows[i]["PrintName"].ToString(), Rescentre = "None", CityID = "5893";

                                        string SalesPerType = "";

                                        using (WebClient client = new WebClient())
                                        {
                                            ms.msdmsg = dt_grps.Rows[i]["Name"].ToString() + " in Insert as Sales Person on MastSalesRep with SyncId" + SyncId;
                                            str = dt_grps.Rows[i]["Name"].ToString() + " in Insert as Sales Person on MastSalesRep with SyncId" + SyncId;
                                            msglist.Add(ms);
                                            //failure = failure + 1;
                                            LogError(str, "BusySalesPerson", "PostItemsToBusy", CompFile);

                                            string url = connectionstring3 + "/Busy_Transfer.asmx/Insert_SalesPerson?SMName=" + dt_grps.Rows[i]["Name"].ToString().Replace("#", "%23") + "&Pin=" + "" + "&SalesPerType=" + SalesPerType + "&DeviceNo=" + "0" + "&DSRAllowDays=" + "0" + "&isAdmin=" + active + "&Address1=" + "" + "&Address2=" + "" + "&CityId=" + CityID + "&Email=" + EmailID + "&Mobile=" + "" + "&Remarks=" + "" + "&RoleId=" + RoleID + "&UserId=" + Convert.ToInt32(UserID) + "&SyncId=" + SyncId.Replace("#", "%23") + "&UnderId=" + ReportTo.Replace("#", "%23") + "&GradeId=" + Convert.ToInt32(0) + "&DeptId=" + Convert.ToInt32(DeptID) + "&DesgId=" + Convert.ToInt32(DesgID) + "&DOB=" + "" + "&DOA=" + "" + "&ResCenID=" + Rescentre + "&BlockReason=" + "" + "&BlockBy=" + "0" + "&EmpName=" + Employee.Replace("#", "%23") + "&AllowChangeCity=" + false + "&MeetAllowDays=" + Convert.ToInt32(0) + "&MobileAccess=" + true + "&FromTime=" + "10:30" + "&ToTime=" + "19:00" + "&Interval=" + "300" + "&Uploadinterval=" + "300" + "&Accuracy=" + "100" + "&Sendpushntification=" + "Yes" + "&BatteryRecord=" + "Yes" + "&groupcode=" + "0" + "&retryinterval=" + "0" + "&gpsloc=" + Convert.ToBoolean(1) + "&mobileloc=" + Convert.ToBoolean(1) + "&sys_flag=" + "N" + "&Alarm=" + "Y" + "&alarmduration=" + "0" + "&sendsms=" + "Y" + "&sendsmsperson=" + "Y" + "&lat=" + "" + "&longi=" + "";
                                            retval = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                        }

                                        using (WebClient client = new WebClient())
                                        {

                                            string url = connectionstring3 + "/Busy_Transfer.asmx/UpdLgn?ACTIVE=" + active + "&ID=" + UserID + "&EMPSYNC=" + "";
                                            smid = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                        }

                                        if (retval == -1)
                                        {

                                            ms.msdmsg = dt_grps.Rows[i]["Name"].ToString() + " is not Inserted as Duplicate SalesPersons Exists";
                                            str = dt_grps.Rows[i]["Name"].ToString() + " is not Inserted as Duplicate SalesPersons Exists";
                                            msglist.Add(ms);
                                            failure = failure + 1;
                                            LogError(str, "Fail", "BusySalesPerson", CompFile);
                                        }
                                        else if (retval == -2)
                                        {
                                            ms.msdmsg = dt_grps.Rows[i]["Name"].ToString() + " is not Inserted as Duplicate SyncId Exists";
                                            str = dt_grps.Rows[i]["Name"].ToString() + " is not Inserted as Duplicate SyncId Exists";
                                            msglist.Add(ms);
                                            failure = failure + 1;
                                            LogError(str, "Fail", "BusySalesPerson", CompFile);
                                        }
                                        else
                                        {
                                            if (SyncId == "")
                                            {
                                                using (WebClient client = new WebClient())
                                                {

                                                    string url = connectionstring3 + "/Busy_Transfer.asmx/UpdLgn?ACTIVE=" + active + "&ID=" + UserID + "&EMPSYNC=" + retval;
                                                    smid = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                                }
                                            }
                                            count = count + 1;
                                            str = success + " record inserted successfully.";
                                            if (failure > 0)
                                                str = str + "," + failure + " record are failed";
                                            if (err > 0)
                                                str = str + ",Check log table";
                                            if (msglist.Count == 0)
                                            {
                                                ms.msdmsg = "No error";
                                                msglist.Add(ms);
                                            }

                                            LogError(str, "Success", "PostItemsToBusy", CompFile);

                                            rs.msg = str;
                                            rs.errormsg = msglist;
                                        }
                                    }
                                }
                                else
                                {
                                    ms.msdmsg = dt_grps.Rows[i]["Name"].ToString() + " Record Already Exist in Sales Person";
                                    str = dt_grps.Rows[i]["Name"].ToString() + " Record Already Exist in Sales Person";
                                    msglist.Add(ms);
                                    failure = failure + 1;
                                    LogError(str, "Fail", "BusySalesPerson", CompFile);
                                }
                            }
                            else
                            {
                                ms.msdmsg = dt_grps.Rows[i]["Name"].ToString() + " Record Already Exist in Login";
                                str = dt_grps.Rows[i]["Name"].ToString() + " Record Already Exist in Sales Person";
                                msglist.Add(ms);
                                failure = failure + 1;
                                LogError(str, "Fail", "BusySalesPerson", CompFile);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                str = ex.Message;
                err = err + 1;
                LogError(str, "Fail", "BusySalesPerson", CompFile);
                // msglist..Add(str);
                if (err > 0)
                    str = str + ",Check log table";
                rs.msg = str;
                rs.errormsg = msglist;

            }

            listBox1.Invoke(new MethodInvoker(() => listBox1.Items.Add(CompFile + " Sales Person Transfered " + count + " Inserted " + count1 + " Updated ")));
        }

        private void BusyAccountSyncData(string Compcode, string CompFile)
        {
            List<Result1> rst = new List<Result1>();
            DataTable dt = new DataTable();
            int DTadmin = 0;
            DataTable DTDist = new DataTable();
            DataTable DTRet = new DataTable();
            DataTable DTsm = new DataTable();
            string str = "";
            int count = 0, count1 = 0, count2 = 0, total = 0;
            int cnt = 0;
            int cityid = 0;
            int citytypeid = 0;
            int cityconveyancetype = 0;
            int distictid = 0;
            int regionid = 0;
            int val = 0;
            int Areaid = 0;
            int stateid = 0;
            int contid = 0;
            int beatid = 0;
            int roleid = 0;
            int prtid = 0;
            int smid = 0;
            ClsProcedure DB = new ClsProcedure();
            int result = 0;
            int roll = 0;
            int success = 0;
            int successupdated = 0;
            //DataTable dt_rol = new DataTable();
            int failure = 0;
            int err = 0;
            int uid = 0;
            int cout = 0;
            int invid = 0;
            int retsave = 0;
            int Roleid = 23;
            string SD_ID = "0";
            string DistType = "";
            string CityName = "BLANK";
            string _query1, _query2, _query, _query3;
            string typ = "Opening Balance";
            int opvoucherno = 1;
            mandatorymsg ms = new mandatorymsg(); ;
            List<mandatorymsg> msglist = new List<mandatorymsg>();
            errorresult rs = new errorresult();
            try
            {
                if (dbFlag == "A")
                {
                    string[] yrlist = CompFile.Split('.');

                    int yr = Convert.ToInt32(yrlist[0].Substring(yrlist[0].Length - 4));
                    string Docid1 = "BUOP" + Compcode.Substring(Compcode.Length - 4) + ' ' + yr.ToString();

                    //string[] yrlist = CompFile.Split('.');

                    //int yr = Convert.ToInt32(yrlist[0].Substring(yrlist[0].Length - 4));
                    int yr1 = yr + 1;

                    using (WebClient client = new WebClient())
                    {

                        string url = connectionstring3 + "/Busy_Transfer.asmx/DelLedger?YR=" + yr + "&COMPANYCODE=" + Compcode + "&TYP=" + typ;

                        invid = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                    }

                    string Date, docID;

                    using (WebClient client = new WebClient())
                    {
                        string url = connectionstring3 + "/Busy_Transfer.asmx/GetSMID?SMID=" + "" + "&UID=" + "";
                        DTadmin = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                    }

                    _query1 = @"Select Code from Master1 where Name = 'Sundry Debtors' and MasterType=1";
                    DataTable dt_grps = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query1, dbFlag);
                    if (dt_grps.Rows.Count > 0)
                    {
                        _query2 = @"SELECT Master1.Code AS Code, Master1.Name AS Name, MasterAddressInfo.Address1 AS Address1, MasterAddressInfo.Address2 AS Address2, MasterAddressInfo.Address3 AS Address3, MasterAddressInfo.TelNo AS TelNo, MasterAddressInfo.Fax AS Fax, MasterAddressInfo.Email AS Email, MasterAddressInfo.Mobile AS Mobile, MasterAddressInfo.PINCode AS PINCode, MasterAddressInfo.ITPAN AS ITPAN, MasterAddressInfo.GSTNo AS GSTNo, MasterAddressInfo.CountryCodeLong AS ContID, MasterAddressInfo.StateCodeLong AS StatID, Help1.NameAlias AS ContName, Help1_1.NameAlias AS StatName, Folio1.D1 AS OpBal, Master1.CM3 AS SalesID FROM(((Master1 LEFT JOIN MasterAddressInfo ON Master1.Code = MasterAddressInfo.MasterCode) LEFT JOIN Help1 ON MasterAddressInfo.CountryCodeLong = Help1.Code) LEFT JOIN Help1 AS Help1_1 ON MasterAddressInfo.StateCodeLong = Help1_1.Code) LEFT JOIN Folio1 ON Master1.Code = Folio1.MasterCode WHERE(((Master1.ParentGrp)In(Select Code from Master1 where Master1.ParentGrp = " + dt_grps.Rows[0]["Code"].ToString() + "  and Master1.MasterType = 1))) OR(((Master1.ParentGrp) = " + dt_grps.Rows[0]["Code"].ToString() + ") AND((Master1.MasterType) = 2) AND((MasterAddressInfo.MasterCode) <> 0) AND((Help1.RecType) = 55))";

                        LogError(_query2, "Query Execute", "BusyAccountSyncData", CompFile);
                        DataTable dt_grp = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query2, dbFlag);
                        if (dt_grp.Rows.Count > 0)
                        {
                            total = dt_grp.Rows.Count;

                            using (WebClient client = new WebClient())
                            {
                                string url = connectionstring3 + "/Busy_Transfer.asmx/GetRoll";
                                roll = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                            }

                            for (int i = 0; i < dt_grp.Rows.Count; i++)
                            {
                                using (WebClient client = new WebClient())
                                {
                                    string PID = dt_grp.Rows[i]["Name"].ToString().Replace("'", "''").ToUpper();
                                    string EmpSyncId = "BU#" + dt_grp.Rows[i]["Code"].ToString();

                                    string url = connectionstring3 + "/Busy_Transfer.asmx/GetLgn?LGNID=" + PID.Replace("#", "%23") + "&SYNC=" + EmpSyncId.Replace("#", "%23");
                                    val = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                }

                                using (WebClient client = new WebClient())
                                {
                                    string PID = "BU#" + dt_grp.Rows[i]["Code"].ToString();
                                    string CONT = dt_grp.Rows[i]["ContName"].ToString().Trim();
                                    string STAT = dt_grp.Rows[i]["StatName"].ToString().Trim();
                                    string url = connectionstring3 + "/Busy_Transfer.asmx/GetPID?PID=" + PID.Replace("#", "%23");
                                    prtid = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));

                                    string urlc = connectionstring3 + "/Busy_Transfer.asmx/GetCID?CONTID=" + CONT.Replace("#", "%23");
                                    contid = Convert.ToInt32(client.DownloadString(urlc).Replace(@"""", ""));

                                    string urls = connectionstring3 + "/Busy_Transfer.asmx/GetSID?STATID=" + STAT.Replace("#", "%23") + "&CONTRID=" + contid;
                                    stateid = Convert.ToInt32(client.DownloadString(urls).Replace(@"""", ""));
                                }

                                _query3 = @"SELECT MasterSupport.CM1 as CM1,Master1.Name as City FROM MasterSupport LEFT JOIN Master1 ON MasterSupport.CM1 = Master1.Code WHERE (((MasterSupport.[MasterCode])=" + dt_grp.Rows[i]["Code"].ToString() + "))";
                                LogError(_query3, "Query Execute", "BusyAccountSyncData", CompFile);
                                DataTable dt_cty = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query3, dbFlag);



                                if (dt_cty.Rows.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(dt_cty.Rows[0]["CM1"].ToString()) && Convert.ToInt32(dt_cty.Rows[0]["CM1"].ToString()) != 0)
                                    {

                                        using (WebClient client = new WebClient())
                                        {
                                            string DISTID = dt_cty.Rows[0]["City"].ToString().Trim();
                                            string CTID = dt_cty.Rows[0]["City"].ToString().Trim();

                                            string url = connectionstring3 + "/Busy_Transfer.asmx/GetDISTID?DISTID=" + DISTID.Replace("#", "%23") + "&STATEID=" + stateid;
                                            distictid = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));

                                            string urlc = connectionstring3 + "/Busy_Transfer.asmx/GetCTID?CITYID=" + CTID.Replace("#", "%23") + "&DISTID=" + distictid;
                                            cityid = Convert.ToInt32(client.DownloadString(urlc).Replace(@"""", ""));
                                        }
                                        CityName = dt_cty.Rows[0]["City"].ToString().Trim();
                                    }
                                }
                                else
                                {
                                    using (WebClient client = new WebClient())
                                    {
                                        //string DISTID = dt_cty.Rows[0]["City"].ToString().Trim();
                                        //string CTID = dt_cty.Rows[0]["City"].ToString().Trim();

                                        string url = connectionstring3 + "/Busy_Transfer.asmx/GetDISTID?DISTID=" + "BLANK" + "&STATEID=" + stateid;
                                        distictid = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));

                                        string urlc = connectionstring3 + "/Busy_Transfer.asmx/GetCTID?CITYID=" + "BLANK" + "&DISTID=" + distictid;
                                        cityid = Convert.ToInt32(client.DownloadString(urlc).Replace(@"""", ""));
                                    }
                                    CityName = "BLANK";
                                }

                                using (WebClient client = new WebClient())
                                {
                                    string AID = dt_grp.Rows[i]["Name"].ToString().Replace("'", "''");

                                    string url = connectionstring3 + "/Busy_Transfer.asmx/GetAREAID?AREAID=" + AID.Replace("#", "%23");
                                    Areaid = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                }

                                if (SDID != "0")
                                {
                                    SD_ID = SDID;
                                    DistType = "UNDERSD";
                                }
                                else
                                {
                                    SD_ID = "-1";
                                    DistType = "DIST";
                                }

                                if (val == 0)
                                {
                                    if (prtid == 0)
                                    {
                                        string Party = dt_grp.Rows[i]["Name"].ToString().Replace("'", "''"), Address1 = dt_grp.Rows[i]["Address1"].ToString(), Address2 = dt_grp.Rows[i]["Address2"].ToString() + dt_grp.Rows[i]["Address3"].ToString(), Pin = dt_grp.Rows[i]["PINCode"].ToString(), Email = dt_grp.Rows[i]["Email"].ToString(), Mobile = dt_grp.Rows[i]["Mobile"].ToString(), SyncId = "BU#" + dt_grp.Rows[i]["Code"].ToString(), RoleID = Convert.ToString(roll), PanNo = dt_grp.Rows[i]["ITPAN"].ToString(), Fax = dt_grp.Rows[i]["Fax"].ToString(), Phone = dt_grp.Rows[i]["TelNo"].ToString(), GST = dt_grp.Rows[i]["GSTNo"].ToString(), CityID = cityid.ToString(), Area_Id = Areaid.ToString(), AreaName = "AREA-" + dt_grp.Rows[i]["Name"].ToString().Replace("'", "''"), CntrNme = dt_grp.Rows[i]["ContName"].ToString(), CntrCde = contid.ToString(), StatNme = dt_grp.Rows[i]["StatName"].ToString(), StatCde = stateid.ToString(), RgnCde = regionid.ToString(), DistCde = distictid.ToString(), CtTypCde = citytypeid.ToString(), CtCnvncId = cityconveyancetype.ToString(), BtCde = beatid.ToString(), BeatName = "BEAT-" + dt_grp.Rows[i]["Name"].ToString().Replace("'", "''"), UserName = Party;

                                        using (WebClient client = new WebClient())
                                        {
                                            ms.msdmsg = dt_grp.Rows[i]["Name"].ToString() + " in Insert as Distributor on Login with SyncId" + SyncId;
                                            str = dt_grp.Rows[i]["Name"].ToString() + " in Insert as Distributor on Login with SyncId" + SyncId;
                                            msglist.Add(ms);
                                            //failure = failure + 1;
                                            LogError(str, "Distributor", "BusyAccountSyncData", CompFile);

                                            string url = connectionstring3 + "/Busy_Transfer.asmx/Insert_Login?UserName=" + Party.Replace("#", "%23") + "&Password=" + "12345" + "&email=" + "" + "&Roleid=" + Convert.ToInt32("23") + "&isAdmin=" + true + "&DeptId=" + "0" + "&DesgId=" + "0" + "&empName=" + Party.Replace("#", "%23") + "&CostCentre=" + Convert.ToDecimal(0) + "&empSyncId=" + SyncId.Replace("#", "%23");
                                            retsave = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                        }

                                        if (retsave == -2)
                                        {
                                            ms.msdmsg = dt_grps.Rows[i]["Name"].ToString() + " is not Inserted as Duplicate EmpSyncId Exists";
                                            str = dt_grps.Rows[i]["Name"].ToString() + " is not Inserted as Duplicate EmpSyncId Exists";
                                            msglist.Add(ms);
                                            //failure = failure + 1;
                                            LogError(str, "Fail", "BusyAccountSyncData", CompFile);
                                        }
                                        if (retsave > 0)
                                        {
                                            if (dt_grp.Rows[i]["SalesID"].ToString() != "" && dt_grp.Rows[i]["SalesID"].ToString() != "0")
                                            {
                                                using (WebClient client = new WebClient())
                                                {
                                                    string SID = "BU#" + dt_grp.Rows[i]["SalesID"].ToString();
                                                    string url = connectionstring3 + "/Busy_Transfer.asmx/GetSMID?SMID=" + SID.Replace("#", "%23") + "&UID=" + "";
                                                    smid = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                                }
                                            }
                                            else
                                            {
                                                smid = Convert.ToInt32(DTadmin);
                                            }

                                            using (WebClient client = new WebClient())
                                            {
                                                string url1 = connectionstring3 + "/Busy_Transfer.asmx/GetSMID?SMID=" + "" + "&UID=" + "1";
                                                uid = Convert.ToInt32(client.DownloadString(url1).Replace(@"""", ""));

                                                ms.msdmsg = Party + " in Insert as Distributor on Distributor with SyncId" + SyncId;
                                                str = Party + " in Insert as Distributor on Distributor with SyncId" + SyncId;
                                                msglist.Add(ms);
                                                //failure = failure + 1;

                                                LogError("Insert = " + connectionstring3 + "," + Compcode + "," + Party + "," + "" + "," + Address1 + "," + Address2 + "," + CityID + "," + Pin + "," + Email + "," + Mobile + "," + "" + "," + SyncId + "," + "" + "," + true + "," + Phone + "," + "" + "," + "" + "," + "" + "," + "" + "," + PanNo + "," + 0 + "," + 0 + "," + smid + "," + "" + "," + "" + "," + Convert.ToInt32(Area_Id) + "," + AreaName + "," + BeatName + "," + CntrNme + "," + StatNme + "," + CityName + "," + Convert.ToInt32(CntrCde) + "," + Convert.ToInt32(StatCde) + "," + Convert.ToInt32(RgnCde) + "," + Convert.ToInt32(DistCde) + "," + Convert.ToInt32(CtTypCde) + "," + Convert.ToInt32(CtCnvncId) + "," + Convert.ToInt32(BtCde) + "," + Roleid + "," + UserName + "," + DistType + "," + uid + "," + SD_ID + "," + 0 + "," + GST, "Variables", "BusyAccountSyncData", CompFile);

                                                string url = connectionstring3 + "/Busy_Transfer.asmx/InsertDistributorBusy?Compcode=" + Compcode.Replace("#", "%23") + "&PartyName=" + Party.Replace("#", "%23") + "&DistName=" + "" + "&Address1=" + Address1 + "&Address2=" + Address2 + "&CityId=" + CityID + "&Pin=" + Pin + "&Email=" + Email.Replace("#", "%23") + "&Mobile=" + Mobile + "&Remark=" + "" + "&SyncId=" + SyncId.Replace("#", "%23") + "&BlockReason=" + "" + "&Active=" + true + "&Phone=" + Phone + "&ContactPerson=" + "" + "&CSTNo=" + "" + "&VatTin=" + "" + "&ServiceTax=" + "" + "&PanNo=" + PanNo + "&CreditLimit=" + "0" + "&OutStanding=" + "0" + "&SMID=" + smid + "&DOA=" + "" + "&DOB=" + "" + "&Areaid=" + Convert.ToInt32(Area_Id) + "&AreaName=" + AreaName.Replace("#", "%23") + "&BeatName=" + BeatName.Replace("#", "%23") + "&TCountryName=" + CntrNme + "&TStateName=" + StatNme + "&CityName=" + CityName + "&countryid=" + Convert.ToInt32(CntrCde) + "&stateid=" + Convert.ToInt32(StatCde) + "&regionid=" + Convert.ToInt32(RgnCde) + "&distictid=" + Convert.ToInt32(DistCde) + "&citytypeid=" + Convert.ToInt32(CtTypCde) + "&cityconveyancetype=" + Convert.ToInt32(CtCnvncId) + "&Beatid=" + Convert.ToInt32(BtCde) + "&Roleid=" + Roleid + "&UserName=" + UserName.Replace("#", "%23") + "&DistType=" + DistType + "&createduserid=" + uid + "&UnderId=" + SD_ID + "&PartyType=" + "0" + "&Partygstin=" + GST;
                                                result = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                            }

                                            if (result > 0)
                                            {
                                                if (Convert.ToDecimal(dt_grp.Rows[i]["OpBal"].ToString()) != 0)
                                                {
                                                    decimal AmountDr = 0, AmountCr = 0, Amount = 0;
                                                    decimal Entryno = 0;
                                                    string[] authorsList = CompFile.Split('.');
                                                    Date = "01/Apr/" + authorsList[0].Substring(authorsList[0].Length - 4).ToString();
                                                    docID = "BUOP" + Compcode.Substring(Compcode.Length - 4).ToString() + " " + Convert.ToString(authorsList[0].Substring(authorsList[0].Length - 4).ToString()) + " " + Convert.ToString(opvoucherno);
                                                    if (Convert.ToDecimal(dt_grp.Rows[i]["OpBal"].ToString()) <= 0)
                                                    {
                                                        Amount = Convert.ToDecimal(dt_grp.Rows[i]["OpBal"].ToString()) * -1;
                                                        AmountDr = Convert.ToDecimal(dt_grp.Rows[i]["OpBal"].ToString()) * -1;
                                                        AmountCr = 0;
                                                    }
                                                    else
                                                    {
                                                        AmountDr = 0;
                                                        AmountCr = Convert.ToDecimal(dt_grp.Rows[i]["OpBal"].ToString());
                                                        Amount = Convert.ToDecimal(dt_grp.Rows[i]["OpBal"].ToString());
                                                    }

                                                    using (WebClient client = new WebClient())
                                                    {
                                                        string url = connectionstring3 + "/Busy_Transfer.asmx/GetCountLedger?DISTID=" + result + "&Vdate=" + Date;
                                                        cout = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                                    }

                                                    if (cout == 0)
                                                    {
                                                        using (WebClient client = new WebClient())
                                                        {
                                                            string url = connectionstring3 + "/Busy_Transfer.asmx/GetDistLed";
                                                            Entryno = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                                        }

                                                        using (WebClient client = new WebClient())
                                                        {
                                                            string url = connectionstring3 + "/Busy_Transfer.asmx/InsertDistLedger?docID=" + docID + "&Date=" + Date + "&result=" + result + "&Amount=" + Amount + "&AmountCr=" + AmountCr + "&AmountDr=" + AmountDr + "&open=" + "Opening Balance" + "&Compcode=" + Compcode + "&Entryno=" + Entryno;
                                                            result = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    decimal AmountDr = 0, AmountCr = 0, Amount = 0;
                                                    decimal Entryno = 0;
                                                    string[] authorsList = CompFile.Split('.');
                                                    Date = "01/Apr/" + authorsList[0].Substring(authorsList[0].Length - 4).ToString();
                                                    docID = "BUOP" + Compcode.Substring(Compcode.Length - 4).ToString() + " " + Convert.ToString(authorsList[0].Substring(authorsList[0].Length - 4).ToString()) + " " + Convert.ToString(opvoucherno);
                                                    AmountDr = 0;
                                                    AmountCr = 0;
                                                    Amount = 0;

                                                    using (WebClient client = new WebClient())
                                                    {
                                                        string url = connectionstring3 + "/Busy_Transfer.asmx/GetCountLedger?DISTID=" + result + "&Vdate=" + Date;
                                                        cout = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                                    }

                                                    if (cout == 0)
                                                    {
                                                        using (WebClient client = new WebClient())
                                                        {
                                                            string url = connectionstring3 + "/Busy_Transfer.asmx/GetDistLed";
                                                            Entryno = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                                        }

                                                        using (WebClient client = new WebClient())
                                                        {
                                                            string url = connectionstring3 + "/Busy_Transfer.asmx/InsertDistLedger?docID=" + docID + "&Date=" + Date + "&result=" + result + "&Amount=" + Amount + "&AmountCr=" + AmountCr + "&AmountDr=" + AmountDr + "&open=" + "Opening Balance" + "&Compcode=" + Compcode + "&Entryno=" + Entryno;
                                                            result = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                                        }
                                                    }
                                                }

                                                using (WebClient client = new WebClient())
                                                {
                                                    string url = connectionstring3 + "/Busy_Transfer.asmx/InsertBusyLog?DistName=" + Party.Replace("#", "%23") + "&DistSync=" + SyncId.Replace("#", "%23") + "&SmSync=" + smid + "&InsTime=" + "DateAdd(minute,330,getutcdate())";
                                                    result = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                                }

                                                success = success + 1;
                                                count = count + 1;
                                            }
                                            else if (retsave == -2)
                                            {
                                                ms.msdmsg = Party.Replace("#", "%23") + " is not Inserted as Duplicate SyncId Exists";
                                                str = Party.Replace("#", "%23") + " is not Inserted as Duplicate EmpSyncId Exists";
                                                msglist.Add(ms);
                                                //failure = failure + 1;
                                                LogError(str, "Fail", "BusyAccountSyncData", CompFile);
                                            }
                                            else if (result == -3)
                                            {
                                                ms.msdmsg = Party + " is not inserted,Duplicate Mobile No.";
                                                str = Party + " is not inserted,Duplicate Mobile No.";
                                                msglist.Add(ms);
                                                failure = failure + 1;
                                                LogError(str, "Fail", "BusyAccountSyncData", CompFile);
                                                count2 = count2 + 1;
                                            }
                                        }
                                    }
                                }
                                else
                                {

                                    string DistId = prtid.ToString(), Party = dt_grp.Rows[i]["Name"].ToString().Replace("'", "''"), Address1 = dt_grp.Rows[i]["Address1"].ToString(), Address2 = dt_grp.Rows[i]["Address2"].ToString() + dt_grp.Rows[i]["Address3"].ToString(), Pin = dt_grp.Rows[i]["PINCode"].ToString(), Email = dt_grp.Rows[i]["Email"].ToString(), Mobile = dt_grp.Rows[i]["Mobile"].ToString(), SyncId = "BU#" + dt_grp.Rows[i]["Code"].ToString(), RoleID = roll.ToString(), PanNo = dt_grp.Rows[i]["ITPAN"].ToString(), Fax = dt_grp.Rows[i]["Fax"].ToString(), Phone = dt_grp.Rows[i]["TelNo"].ToString(), GST = dt_grp.Rows[i]["GSTNo"].ToString(), CityID = cityid.ToString(), Area_Id = Areaid.ToString(), AreaName = "AREA-" + dt_grp.Rows[i]["Name"].ToString().Replace("'", "''"), CntrNme = dt_grp.Rows[i]["ContName"].ToString(), CntrCde = contid.ToString(), StatNme = dt_grp.Rows[i]["StatName"].ToString(), StatCde = stateid.ToString(), RgnCde = regionid.ToString(), DistCde = distictid.ToString(), CtTypCde = citytypeid.ToString(), CtCnvncId = cityconveyancetype.ToString(), BtCde = beatid.ToString(), BeatName = "BEAT-" + dt_grp.Rows[i]["Name"].ToString().Replace("'", "''"), UserName = Party;

                                    if (dt_grp.Rows[i]["SalesID"].ToString() != "" && dt_grp.Rows[i]["SalesID"].ToString() != "0")
                                    {
                                        using (WebClient client = new WebClient())
                                        {
                                            string SID = "BU#" + dt_grp.Rows[i]["SalesID"].ToString();
                                            string url = connectionstring3 + "/Busy_Transfer.asmx/GetSMID?SMID=" + SID.Replace("#", "%23") + "&UID=" + "";
                                            smid = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                        }

                                    }
                                    else
                                    {
                                        smid = Convert.ToInt32(DTadmin);
                                    }

                                    using (WebClient client = new WebClient())
                                    {
                                        string url1 = connectionstring3 + "/Busy_Transfer.asmx/GetSMID?SMID=" + "" + "&UID=" + "1";
                                        uid = Convert.ToInt32(client.DownloadString(url1).Replace(@"""", ""));

                                        ms.msdmsg = Party + " in Insert as Distributor on Distributor with SyncId" + SyncId;
                                        str = Party + " in Insert as Distributor on Distributor with SyncId" + SyncId;
                                        msglist.Add(ms);
                                        //failure = failure + 1;
                                        LogError("Update = " + Compcode + "," + Party + "," + "" + "," + Address1 + "," + Address2 + "," + CityID + "," + Pin + "," + Email + "," + Mobile + "," + "" + "," + SyncId + "," + "" + "," + true + "," + Phone + "," + "" + "," + "" + "," + "" + "," + "" + "," + PanNo + "," + 0 + "," + 0 + "," + smid + "," + "" + "," + "" + "," + Convert.ToInt32(Area_Id) + "," + AreaName + "," + BeatName + "," + CntrNme + "," + StatNme + "," + CityName + "," + Convert.ToInt32(CntrCde) + "," + Convert.ToInt32(StatCde) + "," + Convert.ToInt32(RgnCde) + "," + Convert.ToInt32(DistCde) + "," + Convert.ToInt32(CtTypCde) + "," + Convert.ToInt32(CtCnvncId) + "," + Convert.ToInt32(BtCde) + "," + Roleid + "," + UserName + "," + DistType + "," + uid + "," + SD_ID + "," + 0 + "," + GST, "Variable", "BusyAccountSyncData", CompFile);

                                        string url = connectionstring3 + "/Busy_Transfer.asmx/UpdateDistributorBusy?DistID=" + Convert.ToInt32(DistId) + "&Compcode=" + Compcode.Replace("#", "%23") + "&PartyName=" + Party.Replace("#", "%23") + "&DistName=" + "" + "&Address1=" + Address1 + "&Address2=" + Address2 + "&CityId=" + CityID + "&Pin=" + Pin + "&Email=" + Email.Replace("#", "%23") + "&Mobile=" + Mobile + "&Remark=" + "" + "&SyncId=" + SyncId.Replace("#", "%23") + "&BlockReason=" + "" + "&Active=" + true + "&Phone=" + Phone + "&ContactPerson=" + "" + "&CSTNo=" + "" + "&VatTin=" + "" + "&ServiceTax=" + "" + "&PanNo=" + PanNo + "&CreditLimit=" + "0" + "&OutStanding=" + "0" + "&SMID=" + smid + "&DOA=" + "" + "&DOB=" + "" + "&Areaid=" + Convert.ToInt32(Area_Id) + "&AreaName=" + AreaName.Replace("#", "%23") + "&BeatName=" + BeatName.Replace("#", "%23") + "&TCountryName=" + CntrNme + "&TStateName=" + StatNme + "&CityName=" + CityName + "&countryid=" + Convert.ToInt32(CntrCde) + "&stateid=" + Convert.ToInt32(StatCde) + "&regionid=" + Convert.ToInt32(RgnCde) + "&distictid=" + Convert.ToInt32(DistCde) + "&citytypeid=" + Convert.ToInt32(CtTypCde) + "&cityconveyancetype=" + Convert.ToInt32(CtCnvncId) + "&Beatid=" + Convert.ToInt32(BtCde) + "&Roleid=" + Roleid + "&UserName=" + UserName.Replace("#", "%23") + "&DistType=" + DistType + "&createduserid=" + uid + "&UnderId=" + SD_ID + "&PartyType=" + "0" + "&Partygstin=" + GST;
                                        result = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                    }


                                    if (result > 0)
                                    {
                                        if (Convert.ToDecimal(dt_grp.Rows[i]["OpBal"].ToString()) != 0)
                                        {
                                            decimal AmountDr = 0, AmountCr = 0, Amount = 0;
                                            decimal Entryno = 0;
                                            string[] authorsList = CompFile.Split('.');
                                            Date = "01/Apr/" + authorsList[0].Substring(authorsList[0].Length - 4).ToString();
                                            docID = "BUOP" + Compcode.Substring(Compcode.Length - 4).ToString() + " " + Convert.ToString(authorsList[0].Substring(authorsList[0].Length - 4).ToString()) + " " + Convert.ToString(opvoucherno);
                                            if (Convert.ToDecimal(dt_grp.Rows[i]["OpBal"].ToString()) <= 0)
                                            {
                                                Amount = Convert.ToDecimal(dt_grp.Rows[i]["OpBal"].ToString()) * -1;
                                                AmountDr = Convert.ToDecimal(dt_grp.Rows[i]["OpBal"].ToString()) * -1;
                                                AmountCr = 0;
                                            }
                                            else
                                            {
                                                AmountDr = 0;
                                                AmountCr = Convert.ToDecimal(dt_grp.Rows[i]["OpBal"].ToString());
                                                Amount = Convert.ToDecimal(dt_grp.Rows[i]["OpBal"].ToString());
                                            }
                                            using (WebClient client = new WebClient())
                                            {
                                                string url = connectionstring3 + "/Busy_Transfer.asmx/GetCountLedger?DISTID=" + result + "&Vdate=" + Date;
                                                cout = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                            }

                                            if (cout == 0)
                                            {
                                                using (WebClient client = new WebClient())
                                                {
                                                    string url = connectionstring3 + "/Busy_Transfer.asmx/GetDistLed";
                                                    Entryno = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                                }

                                                using (WebClient client = new WebClient())
                                                {
                                                    string url = connectionstring3 + "/Busy_Transfer.asmx/InsertDistLedger?docID=" + docID + "&Date=" + Date + "&result=" + result + "&Amount=" + Amount + "&AmountCr=" + AmountCr + "&AmountDr=" + AmountDr + "&open=" + "Opening Balance" + "&Compcode=" + Compcode + "&Entryno=" + Entryno;
                                                    result = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                                }

                                            }
                                        }

                                        success = success + 1;
                                        count1 = count1 + 1;
                                        //int bzyid=0;
                                        var obj = "";
                                        using (WebClient client = new WebClient())
                                        {
                                            string url = connectionstring3 + "/Busy_Transfer.asmx/GetBsyLog?SYNCID=" + SyncId.Replace("#", "%23") + "&SMSYNC=" + smid;
                                            obj = client.DownloadString(url).Replace(@"""", "").ToString();
                                        }

                                        if (Convert.ToInt32(obj.ToString()) > 0)
                                        {
                                            using (WebClient client = new WebClient())
                                            {
                                                string url = connectionstring3 + "/Busy_Transfer.asmx/InsertBusyLog?DistName=" + Party.Replace("#", "%23") + "&DistSync=" + SyncId.Replace("#", "%23") + "&SmSync=" + smid + "&InsTime=" + "DateAdd(minute,330,getutcdate())";
                                                result = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                            }
                                        }
                                    }
                                    else if (retsave == -2)
                                    {
                                        ms.msdmsg = Party.Replace("#", "%23") + " is not Updated as Duplicate SyncId Exists";
                                        str = Party.Replace("#", "%23") + " is not Updated as Duplicate EmpSyncId Exists";
                                        msglist.Add(ms);
                                        //failure = failure + 1;
                                        LogError(str, "Fail", "BusyAccountSyncData", CompFile);
                                    }
                                    else if (result == -3)
                                    {
                                        ms.msdmsg = Party + " is not Updated,Duplicate Mobile No.";
                                        str = Party + " is not Updated,Duplicate Mobile No.";
                                        msglist.Add(ms);
                                        failure = failure + 1;
                                        LogError(str, "Fail", "BusyAccountSyncData", CompFile);
                                        count2 = count2 + 1;
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
                            //mandatorymsg ms = new mandatorymsg();
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

                str = ex.Message + "/ " + connectionstring3.ToString();
                err = err + 1;
                LogError(str, "Fail", "BusyAccountSyncData", CompFile);
                // msglist..Add(str);
                if (err > 0)
                    str = str + ",Check log table";
                rs.msg = str;
                rs.errormsg = msglist;

            }

            if (count2 > 0)
            {
                //listBox1.Items.Add(CompFile + " Party Transfered " + total + " Total " + count + " Inserted " + count1 + " Updated " + count2 + " Failure ");
                listBox1.Invoke(new MethodInvoker(() => listBox1.Items.Add(CompFile + " Party Transfered " + total + " Total " + count + " Inserted " + count1 + " Updated " + count2 + " Failure ")));
            }
            else
            {
                //listBox1.Items.Add(CompFile + " Party Transfered " + total + " Total " + count + " Inserted " + count1 + " Updated ");
                listBox1.Invoke(new MethodInvoker(() => listBox1.Items.Add(CompFile + " Party Transfered " + total + " Total " + count + " Inserted " + count1 + " Updated ")));
            }
            //MessageBox.Show(CompFile + " Distributor Transfered");
            //comboBox1.Invoke(new Action(() => comboBox1.Text = String.Empty));
            //comboBox2.Invoke(new Action(() => comboBox2.Text = String.Empty));


            #region commented
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
            #endregion
        }

        private void BusySalesSyncData(string Compcode, string CompFile)
        {
            //label1.Invoke(new MethodInvoker(() => label1.Text = CompFile + " Sale Invoice Transferring........"));
            List<Result1> rst = new List<Result1>();
            DataTable dt = new DataTable();
            string str = "";
            int cnt = 0;
            int flg = 0, flg1 = 1;
            int DistInvId = 0;
            int DistInv1Id = 0;
            int DistInv2Id = 0;
            int success = 0;
            int successupdated = 0;
            int sno = 0;
            int failure = 0;
            int err = 0;
            int DisInvId = 0;
            int count = 0, count2 = 0;
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
                string[] yrlist = CompFile.Split('.');

                int yr = Convert.ToInt32(yrlist[0].Substring(yrlist[0].Length - 4));
                int yr1 = Convert.ToInt32(yr.ToString().Substring(yr.ToString().Length - 2));
                string yr2 = (yr + "-" + (yr + 1)).ToString();

                using (WebClient client = new WebClient())
                {
                    //string PID = dt_grps.Rows[i]["Name"].ToString().ToUpper();

                    string url = connectionstring3 + "/Busy_Transfer.asmx/DelData?YR2=" + yr2.ToString() + "&COMPCODE=" + Compcode.ToString();
                    sno = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                }

                //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete from TransDistInv where DistInvDocId like '%" + yr2.ToString() + "' and Compcode='" + Compcode.ToString() + "'", connectionstring3, "S");
                //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete from TransDistInv1 where DistInvDocId like '%" + yr2.ToString() + "' and Compcode='" + Compcode.ToString() + "'", connectionstring3, "S");
                //DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete from TransDistInv2 where DistInvDocId like '%" + yr2.ToString() + "' and Compcode='" + Compcode.ToString() + "'", connectionstring3, "S");

                _query = @"SELECT Tran1.VchNo AS VNO, Tran1.VchCode AS VCode, Tran1.VchSalePurcAmt AS VSPAmo, Tran1.MasterCode1 AS Code1, Tran1.Date AS VDate, Tran1.VchAmtBaseCur AS BlAmt, Master1_3.D1 AS SGST, Master1_3.D4 AS CGST,(SELECT SUM(D5) FROM Tran2 where Tran2.VchCode=Tran1.VchCode) AS ActAmt FROM (((Tran1 LEFT JOIN Master1 ON Tran1.MasterCode1 = Master1.Code) LEFT JOIN Master1 AS Master1_1 ON Master1.ParentGrp = Master1_1.Code) LEFT JOIN Master1 AS Master1_2 ON Master1_1.ParentGrp = Master1_2.Code)LEFT JOIN Master1 AS Master1_3 ON Tran1.CM1 = Master1_3.Code WHERE (((Master1_1.Name)='Sundry Debtors') AND ((Tran1.VchType)=9)) OR (((Tran1.VchType)=9) AND ((Master1_2.Name)='Sundry Debtors'))";
                DataTable dt_grps = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query, dbFlag);

                str = @"SELECT Tran2.D3 AS D3,Tran2.Value1 AS Value1,Tran2.MasterCode1 AS MasterCode1,Tran2.VchCode AS VchCode,Help1.NameAlias AS Unit,Master1.Name AS Name FROM (Tran2 LEFT JOIN Master1 ON Tran2.MasterCode1 = Master1.Code) LEFT JOIN Help1 ON Tran2.CM2 = Help1.Code WHERE (((Tran2.VchCode) In (SELECT Tran1.VchCode as VCode FROM((Tran1 LEFT JOIN Master1 ON Tran1.MasterCode1 = Master1.Code) LEFT JOIN Master1 AS Master1_1 ON Master1.ParentGrp = Master1_1.Code) LEFT JOIN Master1 AS Master1_2 ON Master1_1.ParentGrp = Master1_2.Code WHERE((Master1_1.Name = 'Sundry Debtors') or(Master1_2.Name = 'Sundry Debtors')) AND((Tran1.VchType) = 9))) AND ((Tran2.RecType)=2))";
                DataTable dt1 = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, str, dbFlag);

                //MessageBox.Show(dt_grps.Rows.Count.ToString() + " " + dt1.Rows.Count.ToString());
                if (dt_grps.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_grps.Rows.Count; i++)
                    {
                        creationtime = dt_grps.Rows[i]["VDate"].ToString();

                        DistInvDocId = "BUSY" + " " + Convert.ToString(Convert.ToDateTime(dt_grps.Rows[i]["VDate"].ToString()).Year) + " " + dt_grps.Rows[i]["VNO"].ToString().Trim();

                        using (WebClient client = new WebClient())
                        {
                            //string PID = dt_grps.Rows[i]["Name"].ToString().ToUpper();

                            string url = connectionstring3 + "/Busy_Transfer.asmx/GetDistId?DISTINVDOCID=" + DistInvDocId.ToString() + "&COMPCODE=" + Compcode.ToString();
                            DisInvId = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                        }
                        //_query = @"Select DistInvId from TransDistInv where DistInvDocId='" + DistInvDocId + "' and Compcode='" + Compcode + "'";
                        //DataTable dt_items = DbConnectionDAL.GetDataTable(CommandType.Text, _query, connectionstring3);

                        if (DisInvId == 0)
                        {
                            decimal sgstamo = Convert.ToDecimal(dt_grps.Rows[i]["VSPAmo"].ToString()) * (Convert.ToDecimal(dt_grps.Rows[i]["SGST"].ToString()) / 100);
                            decimal cgstamo = Convert.ToDecimal(dt_grps.Rows[i]["VSPAmo"].ToString()) * (Convert.ToDecimal(dt_grps.Rows[i]["CGST"].ToString()) / 100);
                            decimal taxamo = Math.Round(sgstamo, 2) + Math.Round(cgstamo, 2);
                            decimal round = Convert.ToDecimal(dt_grps.Rows[i]["BlAmt"].ToString()) - (Convert.ToDecimal(dt_grps.Rows[i]["VSPAmo"].ToString()) + Math.Round(sgstamo, 2) + Math.Round(cgstamo, 2));
                            decimal disc = 0;
                            if (!string.IsNullOrEmpty(dt_grps.Rows[i]["ActAmt"].ToString()))
                            { disc = Convert.ToDecimal(dt_grps.Rows[i]["ActAmt"].ToString()) - Convert.ToDecimal(dt_grps.Rows[i]["VSPAmo"].ToString()); }

                            sno = 0;
                            using (WebClient client = new WebClient())
                            {
                                string PID = "BU#" + dt_grps.Rows[i]["Code1"].ToString();

                                string url = connectionstring3 + "/Busy_Transfer.asmx/insertsaleinvheader_Busy?Docid=" + DistInvDocId + "&VDate=" + creationtime + "&taxamt=" + taxamo + "&Billamount=" + Convert.ToDecimal(dt_grps.Rows[i]["BlAmt"].ToString()) + "&Roundoff=" + round + "&discamo=" + disc + "&SyncId=" + PID.Replace("#", "%23") + "&compcode=" + Compcode;
                                DistInvId = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                            }
                            //DistInvId = DB.insertsaleinvheader_Busy(connectionstring3, DistInvDocId, creationtime, taxamo, Convert.ToDecimal(dt_grps.Rows[i]["BlAmt"].ToString()), round, disc, "BU#" + dt_grps.Rows[i]["Code1"].ToString().Trim(), Compcode);

                            //string sqlconn, string Docid, string VDate, decimal taxamt, decimal Billamount, decimal Roundoff,decimal discamo, string SyncId, string compcode = ""


                            if (dt1.Rows.Count > 0)
                            {
                                DataView dv = dt1.DefaultView;
                                dv.RowFilter = "VchCode ='" + dt_grps.Rows[i]["VCode"].ToString() + "'";
                                DataTable dt3 = dv.ToTable();
                                for (int j = 0; j < dt3.Rows.Count; j++)
                                {

                                    //if (Convert.ToInt32(dt1.Rows[j]["VchCode"].ToString().Trim()) == Convert.ToInt32(dt_grps.Rows[i]["VCode"].ToString().Trim()))
                                    //{

                                    //str = "select * from Master1 where code=" + dt3.Rows[j]["MasterCode1"].ToString() + "";

                                    //DataTable dt2 = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, str, dbFlag);
                                    //if (dt2.Rows.Count > 0)
                                    //{
                                    //    for (int k = 0; k < dt2.Rows.Count; k++)
                                    //    {

                                    sno = sno + 1;
                                    mandatorymsg ms = new mandatorymsg();


                                    if (DistInvId == -2)
                                    {

                                        ms.msdmsg = "SyncID -" + "BU#" + dt_grps.Rows[i]["Code1"].ToString() + " not exist on web portal for Bill no " + dt_grps.Rows[i]["VNO"].ToString();
                                        str = "SyncID -" + "BU#" + dt_grps.Rows[i]["Code1"].ToString() + " not exist on web portal for Bill no " + dt_grps.Rows[i]["VNO"].ToString();
                                        msglist.Add(ms);
                                        failure = failure + 1;

                                        LogError(str, "Fail", "BusySalesSyncData", CompFile);
                                        count2 = count2 + 1;
                                    }
                                    else
                                    {
                                        decimal qty = Convert.ToDecimal(dt3.Rows[j]["Value1"].ToString()) * -1;
                                        decimal amt = Convert.ToDecimal(dt3.Rows[j]["Value1"].ToString()) * -1 * Convert.ToDecimal(dt3.Rows[j]["D3"].ToString());

                                        using (WebClient client = new WebClient())
                                        {
                                            string SyncId = "BU#" + dt_grps.Rows[i]["Code1"].ToString();
                                            string ITEMID = "BU#" + dt3.Rows[j]["MasterCode1"].ToString().Trim();

                                            string url = connectionstring3 + "/Busy_Transfer.asmx/insertsaleinvdetail_Busy?DistInvId=" + DistInvId + "&sno=" + sno + "&Docid=" + DistInvDocId + "&VDate=" + creationtime + "&taxamt=" + "0" + "&Qty=" + qty + "&Rate=" + Convert.ToDecimal(dt3.Rows[j]["D3"].ToString()) + "&Amount=" + amt + "&SyncId=" + SyncId.Replace("#", "%23") + "&itemname=" + dt3.Rows[j]["Name"].ToString() + "&ItemMasterid=" + ITEMID.Replace("#", "%23") + "&Unit=" + dt3.Rows[j]["Unit"].ToString() + "&compcode=" + Compcode;
                                            DistInv1Id = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                        }

                                        //DistInv1Id = DB.insertsaleinvdetail_Busy(connectionstring3, DistInvId, sno, DistInvDocId, creationtime, 0, qty, Convert.ToDecimal(dt3.Rows[j]["D3"].ToString()), amt, "BU#" + dt_grps.Rows[i]["Code1"].ToString(), dt3.Rows[j]["Name"].ToString(), "BU#" + dt3.Rows[j]["MasterCode1"].ToString().Trim(), dt3.Rows[j]["Unit"].ToString(), Compcode);

                                        if (DistInv1Id > 0)
                                        {
                                            success = success + 1;
                                        }
                                        else if (DistInv1Id == -2)
                                        {

                                            ms.msdmsg = "SyncID -" + "BU#" + dt_grps.Rows[i]["Code1"].ToString() + " not exist on web portal for Bill no " + dt_grps.Rows[i]["VNO"].ToString();
                                            str = "SyncID -" + "BU#" + dt_grps.Rows[i]["Code1"].ToString() + " not exist on web portal for Bill no " + dt_grps.Rows[i]["VNO"].ToString();
                                            msglist.Add(ms);
                                            failure = failure + 1;

                                            LogError(str, "Fail", "BusySalesSyncData", CompFile);
                                            count2 = count2 + 1;
                                        }
                                    }
                                }

                                if (DistInvId > 0 && DistInv1Id > 0)
                                {
                                    if (sgstamo != 0 && cgstamo != 0)
                                    {
                                        using (WebClient client = new WebClient())
                                        {
                                            //string SyncId = "BU#" + dt_grps.Rows[i]["Code1"].ToString();
                                            //string ITEMID = "BU#" + dt3.Rows[j]["MasterCode1"].ToString().Trim();

                                            string url = connectionstring3 + "/Busy_Transfer.asmx/insertRetailersaleinvexpensedetail?Docid=" + DistInvDocId + "&Desc=" + "SGST" + "&Amt=" + sgstamo + "&compcode=" + Compcode;

                                            DistInv2Id = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                        }
                                        //DistInv2Id = DB.insertRetailersaleinvexpensedetail(connectionstring3, DistInvDocId, "SGST", sgstamo, Compcode);
                                        using (WebClient client = new WebClient())
                                        {
                                            //string SyncId = "BU#" + dt_grps.Rows[i]["Code1"].ToString();
                                            //string ITEMID = "BU#" + dt3.Rows[j]["MasterCode1"].ToString().Trim();

                                            string url = connectionstring3 + "/Busy_Transfer.asmx/insertRetailersaleinvexpensedetail?Docid=" + DistInvDocId + "&Desc=" + "CGST" + "&Amt=" + cgstamo + "&compcode=" + Compcode;

                                            DistInv2Id = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                        }
                                        //DistInv2Id = DB.insertRetailersaleinvexpensedetail(connectionstring3, DistInvDocId, "CGST", cgstamo, Compcode);
                                    }
                                    else if (sgstamo != 0 && cgstamo == 0)
                                    {
                                        using (WebClient client = new WebClient())
                                        {
                                            //string SyncId = "BU#" + dt_grps.Rows[i]["Code1"].ToString();
                                            //string ITEMID = "BU#" + dt3.Rows[j]["MasterCode1"].ToString().Trim();

                                            string url = connectionstring3 + "/Busy_Transfer.asmx/insertRetailersaleinvexpensedetail?Docid=" + DistInvDocId + "&Desc=" + "IGST" + "&Amt=" + sgstamo + "&compcode=" + Compcode;

                                            DistInv2Id = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                        }
                                        //DistInv2Id = DB.insertRetailersaleinvexpensedetail(connectionstring3, DistInvDocId, "IGST", sgstamo, Compcode);
                                    }

                                    if (round != 0)
                                    {
                                        using (WebClient client = new WebClient())
                                        {
                                            //string SyncId = "BU#" + dt_grps.Rows[i]["Code1"].ToString();
                                            //string ITEMID = "BU#" + dt3.Rows[j]["MasterCode1"].ToString().Trim();

                                            string url = connectionstring3 + "/Busy_Transfer.asmx/insertRetailersaleinvexpensedetail?Docid=" + DistInvDocId + "&Desc=" + "ROUND OFF" + "&Amt=" + round + "&compcode=" + Compcode;

                                            DistInv2Id = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                        }
                                        //DistInv2Id = DB.insertRetailersaleinvexpensedetail(connectionstring3, DistInvDocId, "ROUND OFF", round, Compcode);
                                    }

                                    if (disc != 0)
                                    {
                                        using (WebClient client = new WebClient())
                                        {
                                            //string SyncId = "BU#" + dt_grps.Rows[i]["Code1"].ToString();
                                            //string ITEMID = "BU#" + dt3.Rows[j]["MasterCode1"].ToString().Trim();

                                            string url = connectionstring3 + "/Busy_Transfer.asmx/insertRetailersaleinvexpensedetail?Docid=" + DistInvDocId + "&Desc=" + "DIS AMO" + "&Amt=" + disc + "&compcode=" + Compcode;

                                            DistInv2Id = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                                        }
                                        //DistInv2Id = DB.insertRetailersaleinvexpensedetail(connectionstring3, DistInvDocId, "DIS AMO", disc, Compcode);
                                    }

                                    if (DistInvId > 0 && DistInv1Id > 0)
                                    {
                                        count = count + 1;
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
                //msglist.Add(str);
                if (err > 0)
                    str = str + ",Check log table";
                rs.msg = str;
                rs.errormsg = msglist;

            }

            //listBox1.Items.Add(CompFile + " Sale Transfered " + count + " Inserted ");
            listBox1.Invoke(new MethodInvoker(() => listBox1.Items.Add(CompFile + " Sale Transfered " + count + " Inserted ")));

            #region commented
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

            #endregion

        }

        private void BusyLedgerSyncData(string companycode, string CompFile)
        {
            //label1.Invoke(new MethodInvoker(() => label1.Text = CompFile + " Ledger Transferring........"));
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
            int invid = 0;
            int count = 0, count2 = 0;
            string typ = "Opening Balance";
            mandatorymsg ms = new mandatorymsg();
            List<mandatorymsg> msglist = new List<mandatorymsg>();
            errorresult rs = new errorresult();
            try
            {
                string[] yrlist = CompFile.Split('.');

                int yr = Convert.ToInt32(yrlist[0].Substring(yrlist[0].Length - 4));
                int yr1 = yr + 1;

                //using (WebClient client = new WebClient())
                //{

                //    string url = connectionstring3+"/Busy_Transfer.asmx/DelLedger?YR=" + yr + "&COMPANYCODE=" + companycode + "&TYP=" + typ;

                //    invid = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                //}

                str = @"SELECT Max(Master1.Name) AS Name, Switch(Max(t2.VchType)=9,'SALES',Max(t2.VchType)=14,'RECEIPT',Max(t2.VchType)=16,'JOURNAL',Max(t2.VchType)=12,'SALESORDER',Max(t2.VchType)=18,'CRNT') AS Vchtype1, 'BU#'+LTrim(Str(Max(t2.MasterCode1))) AS Syncid, Max(t2.VchCode) AS VchCode, Max(t2.Date) AS CDate, Max(t2.VchNo) AS [No], Sum(t2.Value1) AS Value1, Max(t2.MasterCode1) AS MasterCode1
FROM ((Tran2 AS t2 LEFT JOIN Master1 ON t2.MasterCode1 = Master1.Code) LEFT JOIN Master1 AS Master1_1 ON Master1.ParentGrp = Master1_1.Code) LEFT JOIN Master1 AS Master1_2 ON Master1_1.ParentGrp = Master1_2.Code
WHERE (((t2.RecType)=1)) and (((Master1_1.Name) = 'Sundry Debtors')) OR(((Master1_2.name) = 'Sundry Debtors')) and ((Year ( t2.date)=" + yr + " AND (Month ( t2.date)>=4 And Month ( t2.date)<=12)) OR (Year ( t2.date)=" + (yr + 1) + " AND (Month ( t2.date)>=1 And Month ( t2.date)<=3))) Group By t2.VchCode,t2.MasterCode1";

                dt = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, str, dbFlag);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
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

                        if (dt.Rows[i]["VchType1"].ToString() != "")
                            Narration = "Vch Type-" + dt.Rows[i]["VchType1"].ToString();
                        else
                            Narration = "";

                        if (amtcr > 0) _amount = amtcr;
                        else if (amtdr > 0) _amount = amtdr * -1;

                        using (WebClient client = new WebClient())
                        {

                            string url = connectionstring3 + "/Busy_Transfer.asmx/insertdistledger_Busy?Syncid=" + dt.Rows[i]["Syncid"].ToString().Replace("#", "%23") + "&VDate=" + dt.Rows[i]["CDate"].ToString() + "&amtdr=" + amtdr + "&amtcr=" + amtcr + "&amt=" + _amount + "&narr=" + Narration + "&VID=" + dt.Rows[i]["VchCode"].ToString().Replace("#", "%23") + "&VTYPE=" + dt.Rows[i]["VchType1"].ToString().Replace("#", "%23") + "&compcode=" + companycode;

                            result = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                        }

                        if (result > 0)
                        {
                            success = success + 1;
                            amtcr = 0;
                            amtdr = 0;
                            _amount = 0;

                            count = count + 1;
                        }
                        else if (result == -1)
                        {
                            ms.msdmsg = "SyncID -" + dt.Rows[i]["Syncid"].ToString() + " not exist on web portal for Voucher Code " + dt.Rows[i]["VchCode"].ToString();
                            str = "SyncID -" + dt.Rows[i]["Syncid"].ToString() + " not exist on web portal for Voucher Code " + dt.Rows[i]["VchCode"].ToString();
                            msglist.Add(ms);
                            failure = failure + 1;
                            LogError(str, "Fail", "BusyLedgerSyncData", CompFile);
                            count2 = count2 + 1;
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
                    //mandatorymsg ms = new mandatorymsg();
                    ms.msdmsg = "No error";
                    msglist.Add(ms);
                }

                LogError(str, "Success", "BusyLedgerSyncData", CompFile);

                rs.msg = str;
                rs.errormsg = msglist;
                //listBox1.Items.Add(CompFile + " Ledger Transfered " + count + " Inserted ");
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
            if (count2 > 0)
            {
                //listBox1.Items.Add(CompFile + " Ledger Transfered " + count + " Inserted " + count2 + " Failure ");
                listBox1.Invoke(new MethodInvoker(() => listBox1.Items.Add(CompFile + " Ledger Transfered " + count + " Inserted " + count2 + " Failure ")));
            }
            else
            {
                //listBox1.Items.Add(CompFile + " Ledger Transfered " + count);
                listBox1.Invoke(new Action(() => listBox1.Items.Add(CompFile + " Ledger Transfered " + count)));
            }
            //return rs;

            //MessageBox.Show(CompFile + " Ledger Transfered");

        }

        private void BusyAgileSyncData(string companycode, string CompFile)
        {
            //label1.Invoke(new MethodInvoker(() => label1.Text = CompFile + " Ledger Transferring........"));
            List<Result1> rst = new List<Result1>();
            decimal _amount = 0;
            decimal amtdr = 0;
            decimal amtcr = 0;
            string docID;
            string Vtype = "";
            string Narration = "";
            string _query1 = "";
            ClsProcedure DB = new ClsProcedure();
            DataTable dt = new DataTable();
            DataTable dtsync = new DataTable();
            string str = "";
            decimal result = 0;
            int success = 0;
            int failure = 0;
            int err = 0;
            int sno = 0;
            int invid = 0;
            int count = 0, count2 = 0;
            string typ = "Opening Balance";
            mandatorymsg ms = new mandatorymsg();
            List<mandatorymsg> msglist = new List<mandatorymsg>();
            errorresult rs = new errorresult();
            try
            {
                string[] yrlist = CompFile.Split('.');

                int yr = Convert.ToInt32(yrlist[0].Substring(yrlist[0].Length - 4));
                int yr1 = yr + 1;
                string yr2 = (yr + "-" + (yr + 1)).ToString();
                DateTime start = new DateTime(yr, 4, 1);
                DateTime ninty = start.AddDays(90);
                DateTime nxtninty = ninty.AddDays(90);
                DateTime nxttoninty = nxtninty.AddDays(90);
                DateTime last = new DateTime(yr1, 3, 31);
                if (dbFlag == "A")
                {



                    using (WebClient client = new WebClient())
                    {
                        //string PID = dt_grps.Rows[i]["Name"].ToString().ToUpper();

                        string url = connectionstring3 + "/Busy_Transfer.asmx/DelAgileData?YR2=" + yr2.ToString() + "&COMPCODE=" + companycode.ToString();
                        sno = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));
                    }

                    using (WebClient client = new WebClient())
                    {
                        string url = connectionstring3 + "/Busy_Transfer.asmx/GetDistSync";
                        docID = client.DownloadString(url).Replace(@"""", "");
                    }
                    _query1 = @"select MasterCode1,Date,DueDate,[No] as Vch,Value1,Method from Tran3 where MasterCode1 in (" + docID + ")";
                    DataTable dt_code = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, _query1, dbFlag);

                    for (int i = 0; i < dt_code.Rows.Count; i++)
                    {
                        using (WebClient client = new WebClient())
                        {
                            string SyncId = "BU#" + dt_code.Rows[i]["MasterCode1"].ToString();
                            //PartySyncId,Date,DueDate,VchNo,Payment,CompCode
                            string url = connectionstring3 + "/Busy_Transfer.asmx/InsertAgile?PartySyncId=" + SyncId.Replace("#", "%23") + "&Date=" + dt_code.Rows[i]["Date"].ToString() + "&DueDate=" + dt_code.Rows[i]["DueDate"].ToString() + "&VchNo=" + dt_code.Rows[i]["Vch"].ToString().Trim() + "&Payment=" + dt_code.Rows[i]["Value1"].ToString() + "&CompCode=" + companycode + "&Mode=" + dt_code.Rows[i]["Method"].ToString().Trim() + "&YR=" + yr2.ToString().Trim();

                            result = Convert.ToInt32(client.DownloadString(url).Replace(@"""", ""));

                            if (result > 0)
                            {
                                count += 1;
                            }
                            else
                            {
                                count2 += 1;
                            }
                        }
                    }

                }

                if (count > 0)
                {
                    str = success + " record inserted successfully.";
                    if (failure > 0)
                        str = str + "," + failure + " record are failed";
                    if (err > 0)
                        str = str + ",Check log table";
                    if (msglist.Count == 0)
                    {
                        //mandatorymsg ms = new mandatorymsg();
                        ms.msdmsg = "No error";
                        msglist.Add(ms);
                    }

                    LogError(str, "Success", "BusyAgingSyncData", CompFile);

                    rs.msg = str;
                    rs.errormsg = msglist;
                }

                if (count2 > 0)
                {
                    str = success + " record inserted successfully.";
                    if (failure > 0)
                        str = str + "," + failure + " record are failed";
                    if (err > 0)
                        str = str + ",Check log table";
                    if (msglist.Count == 0)
                    {
                        //mandatorymsg ms = new mandatorymsg();
                        ms.msdmsg = "No error";
                        msglist.Add(ms);
                    }

                    LogError(str, "Fail", "BusyAgingSyncData", CompFile);

                    rs.msg = str;
                    rs.errormsg = msglist;
                }
                //listBox1.Items.Add(CompFile + " Ledger Transfered " + count + " Inserted ");
                // this.SucessEmailStatus();
            }
            catch (Exception ex)
            {
                //transactionScope.Dispose();

                str = ex.Message;
                err = err + 1;
                LogError(str, "Fail", "BusyAgingSyncData", CompFile);
                // msglist..Add(str);
                if (err > 0)
                    str = str + ",Check log table";
                rs.msg = str;
                rs.errormsg = msglist;

            }
            if (count > 0)
            {
                //listBox1.Items.Add(CompFile + " Ledger Transfered " + count + " Inserted " + count2 + " Failure ");
                listBox1.Invoke(new MethodInvoker(() => listBox1.Items.Add(CompFile + " Outstanding Aging Data Transfered " + count + " Inserted " + count2 + " Failure ")));
            }
            else
            {
                //listBox1.Items.Add(CompFile + " Ledger Transfered " + count);
                listBox1.Invoke(new MethodInvoker(() => listBox1.Items.Add(CompFile + " Outstanding Aging Data Transfered " + count + " Inserted " + count2 + " Failure ")));
            }
            //return rs;

            //MessageBox.Show(CompFile + " Ledger Transfered");

        }

        private void LogError(string str, string status, string functionname, string cmpnfile)
        {
            try
            {

                string query = "insert into Log (Description,Status,MethodName,ErrorTime,Company) values ('" + str.Replace("'", "''") + "','" + status + "','" + functionname + "',Date()+Time(),'" + cmpnfile + "')";
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
            catch (Exception ex)
            {
                //mess ex.Message;
            }
        }


        //        #region
        //        private void createBusyConnectionString(DataTable dt)
        //        {
        //            connectionstring1 = "";
        //            connectionstring2 = "";
        //            connectionstring3 = "";

        //            if (dt.Rows[0]["BusyDatabase"].ToString().ToLower() == "ms access")
        //            {
        //                dbFlag = "A";
        //                connectionstring1 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dt.Rows[0]["Busy_AccessDbPath"].ToString() + "\\" + dt.Rows[0]["Busy_AccessFileName1"].ToString() + ";Jet OLEDB:Database Password=" + dt.Rows[0]["Busy_AccessDbPassword"].ToString() + "";

        //                connectionstring2 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dt.Rows[0]["Busy_AccessDbPath"].ToString() + "\\" + dt.Rows[0]["Busy_AccessFileName2"].ToString() + ";Jet OLEDB:Database Password=" + dt.Rows[0]["Busy_AccessDbPassword"].ToString() + "";

        //                busy_Conn1 = new OleDbConnection(connectionstring1);
        //                busy_Conn2 = new OleDbConnection(connectionstring2);
        //            }
        //            else if (dt.Rows[0]["BusyDatabase"].ToString().ToLower() == "ms sql")
        //            {
        //                dbFlag = "S";
        //                connectionstring1 = "Data Source=" + dt.Rows[0]["Busy_SQLServerIP"].ToString() + ";Initial Catalog=" + dt.Rows[0]["Busy_SQLDbName1"].ToString() + ";user id=" + dt.Rows[0]["Busy_SQLUsername"].ToString() + "; pwd=" + dt.Rows[0]["Busy_SQLPassword"].ToString() + ";";

        //                connectionstring2 = "Data Source=" + dt.Rows[0]["Busy_SQLServerIP"].ToString() + ";Initial Catalog=" + dt.Rows[0]["Busy_SQLDbName2"].ToString() + ";user id=" + dt.Rows[0]["Busy_SQLUsername"].ToString() + "; pwd=" + dt.Rows[0]["Busy_SQLPassword"].ToString() + ";";

        //                busy_sqlcon1 = new SqlConnection(connectionstring1);
        //                busy_sqlcon2 = new SqlConnection(connectionstring2);
        //            }

        //            connectionstring3 = "Data Source=" + dt.Rows[0]["DestinationServer"].ToString() + ";Initial Catalog=" + dt.Rows[0]["DestinationDatabase"].ToString() + ";user id=" + dt.Rows[0]["DestinationUser"].ToString() + "; pwd=" + dt.Rows[0]["DestinationPassword"].ToString() + ";";

        //            dest_sqlcon = new SqlConnection(connectionstring3);
        //        }
        //        private void MargSyncRetailer(DataTable Dt)
        //        {
        //            DataTable Dtorderfilter = new DataTable();
        //            DataTable DtSaletypefilter = new DataTable();
        //            DataTable DtSupportfilter = new DataTable();
        //            DataTable Dtorder = new DataTable();
        //            DataTable Dtsaletype = new DataTable();
        //            DataTable Dtsupport = new DataTable();
        //            DataTable DtDistributor = new DataTable();
        //            DataTable DTadmin = new DataTable();
        //            DataTable DTBeat = new DataTable();
        //            DataTable DTState = new DataTable();
        //            errorresult rs = new errorresult();
        //            List<mandatorymsg> msglist = new List<mandatorymsg>();
        //            int success = 0;
        //            int failure = 0;
        //            int cnt = 0;
        //            int cityid = 0;
        //            int Areaid = 0;
        //            int regionid = 0;
        //            int distictid = 0;
        //            int beatid = 0;
        //            string str = "", Email = "", Areaname = "", Beatname = "", Statename = "";
        //            try
        //            {

        //                int citytypeid = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select Id From MastCityType Where Name='Other'", connectionstring3));
        //                int cityconveyancetype = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, "Select Id From MastCityType Where Name='OTHERS'", connectionstring3));
        //                DtDistributor = DbConnectionDAL.GetDataTable(CommandType.Text, "Select PartyName,MP.Areaid AS Areaid,cityid,MD.AreaId AS Distinctid,MS.AreaId AS Stateid,MR.AreaId AS Regionid from MastParty  MP LEFT JOIN MastArea MC ON MC.AreaId=MP.CityId LEFT JOIN MastArea MD ON MD.AreaId=MC.UnderId LEFT JOIN MastArea MS ON MS.AreaId=MD.UnderId LEFT JOIN MastArea MR ON MR.AreaId=MS.UnderId  where PartyID=" + Dt.Rows[0]["DistributorID"].ToString() + "", connectionstring3);
        //                DTadmin = DbConnectionDAL.GetDataTable(CommandType.Text, "Select * from MastSalesRep Where SMName='DIRECTOR'", connectionstring3);
        //                cityid = Convert.ToInt32(DtDistributor.Rows[0]["cityid"].ToString());
        //                regionid = Convert.ToInt32(DtDistributor.Rows[0]["Regionid"].ToString());
        //                distictid = Convert.ToInt32(DtDistributor.Rows[0]["Distinctid"].ToString());
        //                Areaid = Convert.ToInt32(DtDistributor.Rows[0]["Areaid"].ToString());
        //                ////////////////Get csv data/////////////////
        //                Dtorder = Convertcsvtodatatable("C:\\INDIADATA\\order.csv");
        //                Dtsaletype = Convertcsvtodatatable("C:\\INDIADATA\\saletype.csv");
        //                Dtsupport = Convertcsvtodatatable("C:\\INDIADATA\\support.csv");
        //                ////////////////Get csv data/////////////////
        //                //////////Get Sundry Debitor Data///////////////

        //                if (Dtorder.Rows.Count > 0)
        //                {
        //                    Dtorder.DefaultView.RowFilter = "SCODE='C6'";
        //                    Dtorderfilter = Dtorder.DefaultView.ToTable();
        //                }

        //                //////////Get Sundry Debitor Data///////////////


        //                if (Dtorderfilter.Rows.Count > 0)
        //                {
        //                    for (int i = 0; i < Dtorderfilter.Rows.Count; i++)
        //                    {
        //                        mandatorymsg ms = new mandatorymsg();
        //                        if (Dtsupport.Rows.Count > 0)
        //                        {
        //                            Dtsupport.DefaultView.RowFilter = "SNO =2 and LCODE='" + Dtorderfilter.Rows[i]["ORDNO"].ToString() + "'";
        //                            DtSupportfilter = Dtsupport.DefaultView.ToTable();
        //                            if (DtSupportfilter.Rows.Count > 0)
        //                            {

        //                                Email = DtSupportfilter.Rows[0]["REMARK"].ToString();

        //                            }
        //                        }
        //                        if (Dtsaletype.Rows.Count > 0)
        //                        {
        //                            Dtsaletype.DefaultView.RowFilter = "SGCODE='AREA' and SCODE='" + Dtorderfilter.Rows[i]["AREA"].ToString() + "'";
        //                            DtSaletypefilter = Dtsaletype.DefaultView.ToTable();
        //                            if (DtSaletypefilter.Rows.Count > 0)
        //                            {
        //                                Areaname = DtSaletypefilter.Rows[0]["PARNAM"].ToString();
        //                            }
        //                        }
        //                        if (Dtsaletype.Rows.Count > 0)
        //                        {
        //                            Dtsaletype.DefaultView.RowFilter = "SGCODE='ROUT' and SCODE='" + Dtorderfilter.Rows[i]["ROUT"].ToString() + "'";
        //                            DTBeat = Dtsaletype.DefaultView.ToTable();
        //                            if (DTBeat.Rows.Count > 0)
        //                            {
        //                                Beatname = DTBeat.Rows[0]["PARNAM"].ToString();
        //                            }
        //                        }

        //                        if (Dtsupport.Rows.Count > 0)
        //                        {
        //                            Dtsupport.DefaultView.RowFilter = "SNO =7 and LCODE='" + Dtorderfilter.Rows[i]["ORDNO"].ToString() + "'";
        //                            DTState = Dtsupport.DefaultView.ToTable();
        //                            if (DTState.Rows.Count > 0)
        //                            {
        //                                Dtsaletype.DefaultView.RowFilter = "SGCODE ='STATE1' and SCODE='" + DTState.Rows[0]["REMARK"].ToString().Substring(DTState.Rows[0]["REMARK"].ToString().Length - 6).Trim() + "'";
        //                                DTState = Dtsaletype.DefaultView.ToTable();
        //                                if (DTState.Rows.Count > 0)
        //                                {
        //                                    Statename = DTState.Rows[0]["PARNAM"].ToString();
        //                                }
        //                            }
        //                        }


        //                        cnt = DB.InsertParty_Marg(connectionstring3, Dtorderfilter.Rows[i]["PARNAM"].ToString(), DtDistributor.Rows[0]["PartyName"].ToString(), Dtorderfilter.Rows[i]["PARADD"].ToString(), Dtorderfilter.Rows[i]["PARADD1"].ToString(), cityid, Areaid, beatid, Convert.ToInt32(Dt.Rows[0]["DistributorID"].ToString()), Dtorderfilter.Rows[i]["DSM"].ToString(), Dtorderfilter.Rows[i]["PHONE4"].ToString(), Dtorderfilter.Rows[i]["PHONE1"].ToString(), "", Dtorderfilter.Rows[i]["ORDNO"].ToString(), true, "", 0, Dtorderfilter.Rows[i]["CONFIR"].ToString() + " " + Dtorderfilter.Rows[i]["CONLAS"].ToString(), Dtorderfilter.Rows[i]["STNO"].ToString(), Dtorderfilter.Rows[i]["CSTNO"].ToString(), "", Dtorderfilter.Rows[i]["ITNO"].ToString(), Convert.ToDecimal(Dtorderfilter.Rows[i]["LIMIT"].ToString()), 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), "", "", Email, "", Dtorderfilter.Rows[i]["GSTNO"].ToString(), Convert.ToInt32(DTadmin.Rows[0]["SMID"]), Dtorderfilter.Rows[i]["GSTNO"].ToString(), "", Statename, Areaname, Beatname, 0, regionid, distictid, cityid, cityconveyancetype, Convert.ToInt32(Dt.Rows[0]["DistributorID"].ToString()), "INDIA");

        //                        if (cnt > 0)
        //                        {

        //                            ms.msdmsg = Dtorderfilter.Rows[i]["PARNAM"].ToString() + " is inserted successfully";
        //                            msglist.Add(ms);
        //                            success = success + 1;

        //                        }
        //                        else if (cnt == -1)
        //                        {
        //                            ms.msdmsg = Dtorderfilter.Rows[i]["PARNAM"].ToString() + " is not inserted,Already exist in login Mast";
        //                            msglist.Add(ms);
        //                            failure = failure + 1;

        //                        }
        //                        else if (cnt == -3)
        //                        {
        //                            ms.msdmsg = Dtorderfilter.Rows[i]["PARNAM"].ToString() + " is not inserted,Duplicate Mobile No.";
        //                            msglist.Add(ms);
        //                            failure = failure + 1;

        //                        }
        //                        else if (cnt == -2)
        //                        {
        //                            int retval = DB.UpdateParty_Marg(connectionstring3, Dtorderfilter.Rows[i]["PARNAM"].ToString(), DtDistributor.Rows[0]["PartyName"].ToString(), Dtorderfilter.Rows[i]["PARADD"].ToString(), Dtorderfilter.Rows[i]["PARADD1"].ToString(), cityid, Areaid, beatid, Convert.ToInt32(Dt.Rows[0]["DistributorID"].ToString()), Dtorderfilter.Rows[i]["DSM"].ToString(), Dtorderfilter.Rows[i]["PHONE4"].ToString(), Dtorderfilter.Rows[i]["PHONE4"].ToString(), "", Convert.ToInt32(Dt.Rows[0]["DistributorID"].ToString()) + "#" + Dtorderfilter.Rows[i]["ORDNO"].ToString(), true, "", 0, Dtorderfilter.Rows[i]["CONFIR"].ToString() + " " + Dtorderfilter.Rows[i]["CONLAS"].ToString(), Dtorderfilter.Rows[i]["STNO"].ToString(), Dtorderfilter.Rows[i]["CSTNO"].ToString(), "", Dtorderfilter.Rows[i]["ITNO"].ToString(), Dtorderfilter.Rows[i]["LIMIT"].ToString() == "" ? 0 : Convert.ToDecimal(Dtorderfilter.Rows[i]["LIMIT"].ToString()), 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), "", "", Email, "", Dtorderfilter.Rows[i]["GSTNO"].ToString(), Convert.ToInt32(DTadmin.Rows[0]["SMid"].ToString()), Dtorderfilter.Rows[i]["GSTNO"].ToString(), "",
        //                             "INDIA", Statename, Areaname, Beatname, 0, regionid, distictid, citytypeid, cityconveyancetype, Convert.ToInt32(Dt.Rows[0]["DistributorID"].ToString()));


        //                            if (retval > 0)
        //                            {

        //                                ms.msdmsg = Dtorderfilter.Rows[i]["PARNAM"].ToString() + " is updated successfully";

        //                                msglist.Add(ms);
        //                                success = success + 1;
        //                            }
        //                            else
        //                            {
        //                                ms.msdmsg = Dtorderfilter.Rows[i]["PARNAM"].ToString() + " is not updated,Something went wrong";
        //                                msglist.Add(ms);
        //                                failure = failure + 1;
        //                                // LogError(ds.Tables[0].Rows[i]["PARTYNAME"].ToString() + " is not inserted,Something went wrong");
        //                            }

        //                        }
        //                        else
        //                        {
        //                            ms.msdmsg = Dtorderfilter.Rows[i]["PARNAM"].ToString() + " is not inserted,Something went wrong";
        //                            msglist.Add(ms);
        //                            failure = failure + 1;

        //                        }
        //                        Email = "";
        //                        Areaname = "";
        //                        Statename = "";
        //                        Beatname = "";

        //                    }
        //                    str += success + " record inserted successfully <br/>";
        //                    if (failure > 0)
        //                        str = str + "," + failure + " record are failed";
        //                    //if (err > 0)
        //                    //    str = str + ",Check log ";
        //                    if (msglist.Count == 0)
        //                    {
        //                        mandatorymsg ms = new mandatorymsg();
        //                        ms.msdmsg = "No error";
        //                        msglist.Add(ms);
        //                    }

        //                    rs.msg = str;
        //                    rs.errormsg = msglist;

        //                }


        //            }
        //            catch (Exception ex)
        //            {

        //                cnt = 0;
        //                str = ex.Message;
        //                //err = err + 1;
        //                //LogError(str);
        //                //// msglist..Add(str);
        //                //if (err > 0)
        //                str = str + ",Check log ";
        //                rs.msg = str;
        //                rs.errormsg = msglist;
        //            }

        //            if (str != "")
        //            {
        //                //    SendEmail("Regarding Log Detail of Retailer Transfer from tally", rs, str);
        //            }

        //        }

        //        private void MargSyncRetailerinvoice(DataTable Dt)
        //        {

        //        }
        //        private void MargSyncItem(DataTable Dt)
        //        {
        //            DataTable Dtitem = new DataTable();
        //            try
        //            {

        //                Dtitem = Convertcsvtodatatable("C:\\INDIADATA\\PRO.csv");
        //                Dtitem.TableName = "PRO";
        //                SqlConnection sqlconn = new SqlConnection(connectionstring3);
        //                sqlconn.Open();
        //                SqlBulkCopy sqlblk = new SqlBulkCopy(sqlconn);
        //                sqlblk.DestinationTableName = "PRO";

        //                sqlblk.ColumnMappings.Add("[SELECT]", "[SELECT]");

        //                sqlblk.ColumnMappings.Add("CODE", "CODE");
        //                sqlblk.ColumnMappings.Add("NAME", "NAME");
        //                sqlblk.ColumnMappings.Add("BILLNAME", "BILLNAME");
        //                sqlblk.ColumnMappings.Add("PRODUCT", "PRODUCT");
        //                sqlblk.ColumnMappings.Add("PACKING", "PACKING");
        //                sqlblk.ColumnMappings.Add("UNIT", "UNIT");

        //                sqlblk.ColumnMappings.Add("PACK", "PACK");


        //                sqlblk.ColumnMappings.Add("OPENING", "OPENING");
        //                sqlblk.ColumnMappings.Add("BALANCE", "BALANCE");


        //                sqlblk.ColumnMappings.Add("QTY", "QTY");
        //                sqlblk.ColumnMappings.Add("TQTY", "TQTY");
        //                sqlblk.ColumnMappings.Add("LPRATE", "LPRATE");

        //                sqlblk.ColumnMappings.Add("RATEA", "RATEA");
        //                sqlblk.ColumnMappings.Add("RATEB", "RATEB");
        //                sqlblk.ColumnMappings.Add("RATEC", "RATEC");
        //                sqlblk.ColumnMappings.Add("RATED", "RATED");


        //                sqlblk.ColumnMappings.Add("GCODE", "GCODE");
        //                sqlblk.ColumnMappings.Add("DEAL", "DEAL");
        //                sqlblk.ColumnMappings.Add("FREE", "FREE");
        //                sqlblk.ColumnMappings.Add("PRATE", "PRATE");
        //                sqlblk.ColumnMappings.Add("MRP", "MRP");


        //                sqlblk.ColumnMappings.Add("CGST", "CGST");
        //                sqlblk.ColumnMappings.Add("IGST", "IGST");

        //                sqlblk.WriteToServer(Dtitem);
        //                sqlconn.Close();

        //            }
        //            catch (Exception ex)
        //            {

        //                // throw;
        //            }


        //        }
        //        private DataTable Convertcsvtodatatable(string strFilePath)
        //        {
        //            DataTable dt = new DataTable();
        //            if (File.Exists(strFilePath))
        //            {


        //                StreamReader sr = new StreamReader(strFilePath);
        //                string[] headers = sr.ReadLine().Split(',');

        //                foreach (string header in headers)
        //                {
        //                    dt.Columns.Add(header);
        //                }
        //                try
        //                {
        //                    while (!sr.EndOfStream)
        //                    {
        //                        string[] rows = sr.ReadLine().Split(',');
        //                        DataRow dr = dt.NewRow();
        //                        for (int i = 0; i < headers.Length; i++)
        //                        {

        //                            dr[i] = rows[i];
        //                        }
        //                        dt.Rows.Add(dr);
        //                    }
        //                    sr.Close();
        //                }
        //                catch (Exception ex)
        //                {
        //                    ex.ToString();
        //                }
        //            }
        //            return dt;
        //            //  return Dt;

        //        }
        //        private void BusyAccountSyncData()
        //        {
        //            List<Result1> rst = new List<Result1>();
        //            DataTable dt = new DataTable();
        //            string str = "";
        //            int cnt = 0;
        //            int cityid = 0;
        //            int Areaid = 0;
        //            int roleid = 0;
        //            int success = 0;
        //            int successupdated = 0;
        //            int failure = 0;
        //            int err = 0;
        //            List<mandatorymsg> msglist = new List<mandatorymsg>();
        //            errorresult rs = new errorresult();
        //            try
        //            {
        //                cityid = Convert.ToInt32(DbConnectionDAL.GetScalarValueBusy(CommandType.Text, "SELECT AreaId from MastArea where AreaType='CITY' AND AreaName='Blank'", connectionstring3));
        //                Areaid = Convert.ToInt32(DbConnectionDAL.GetScalarValueBusy(CommandType.Text, "SELECT AreaId from MastArea where AreaType='AREA' AND AreaName='Blank'", connectionstring3));

        //                roleid = Convert.ToInt32(DbConnectionDAL.GetScalarValueBusy(CommandType.Text, "Select RoleId From MastRole Where RoleName='DISTRIBUTOR'", connectionstring3));


        //                //str = "select * from Master1 where MasterType=2";
        //                str = "SELECT m1.Code,m1.MasterType,m1.Name,m1.PrintName,m1.ParentGrp,Address1,Address2,Address3,Address4,TelNo,Fax,Email,Mobile,CST,TINNo,ITPAN,Contact,ServiceTaxNo,TelNoResi,GSTNo,CountryCodeLong,StateCodeLong,CityCodeLong,RegionCodeLong,AreaCodeLong,PINCode FROM Master1 m1 left join MasterAddressInfo ma1 on m1.Code=ma1.MasterCode where m1.MasterType=2  and ParentGrp in (select code from master1 where mastertype=1 and name='Sundry Debtors') order by m1.code";
        //                dt = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, str, dbFlag);
        //                if (dt.Rows.Count > 0)
        //                {
        //                    DataTable DTadmin = DbConnectionDAL.GetDataTableForBusyData(connectionstring3, CommandType.Text, "Select * from MastSalesRep Where SMName='DIRECTOR'", dbFlag);
        //                    for (int i = 0; i < dt.Rows.Count; i++)
        //                    {
        //                        //mandatorymsg ms = new mandatorymsg();
        //                        //int retval = DB.InsertDistributorsBusy(connectionstring3, dt.Rows[i]["Name"].ToString(), dt.Rows[i]["Address1"].ToString() + " " + dt.Rows[i]["Address2"].ToString(), dt.Rows[i]["Address3"].ToString() + " " + dt.Rows[i]["Address4"].ToString(), cityid.ToString(), dt.Rows[i]["PINCode"].ToString(), dt.Rows[i]["Email"].ToString(), dt.Rows[i]["Mobile"].ToString(), "", dt.Rows[i]["Code"].ToString(), "", dt.Rows[i]["Name"].ToString(), true, dt.Rows[i]["TelNo"].ToString(), roleid, dt.Rows[i]["Contact"].ToString(), dt.Rows[i]["CST"].ToString(), dt.Rows[i]["TINNo"].ToString(), dt.Rows[i]["ServiceTaxNo"].ToString(), dt.Rows[i]["ITPAN"].ToString(), 0, 0, 0, 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), dt.Rows[i]["TelNoResi"].ToString(), dt.Rows[i]["Fax"].ToString(), dt.Rows[i]["PrintName"].ToString(), Convert.ToInt32(DTadmin.Rows[0]["SMId"]), Convert.ToString(""), Convert.ToString(""), Areaid);
        //                        //if (retval == -1)
        //                        //{
        //                        //    //'Duplicate User Name Exists'
        //                        //    ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not inserted,Something went wrong";
        //                        //    msglist.Add(ms);
        //                        //    failure = failure + 1;
        //                        //    continue;
        //                        //}
        //                        //else if (retval == -2)
        //                        //{
        //                        //    //'Duplicate SyncId Exists'
        //                        //    int retval_update = DB.UpdateDistributorsBusy(connectionstring3, Convert.ToInt32(dt.Rows[i]["Code"].ToString()), dt.Rows[i]["Name"].ToString(), dt.Rows[i]["Address1"].ToString() + " " + dt.Rows[i]["Address2"].ToString(), dt.Rows[i]["Address3"].ToString() + " " + dt.Rows[i]["Address4"].ToString(), cityid.ToString(), dt.Rows[i]["PINCode"].ToString(), dt.Rows[i]["Email"].ToString(), dt.Rows[i]["Mobile"].ToString(), "", dt.Rows[i]["Code"].ToString(), "", DTadmin.Rows[0]["SMName"].ToString(), true, dt.Rows[i]["TelNo"].ToString(), roleid, dt.Rows[i]["Contact"].ToString(), dt.Rows[i]["CST"].ToString(), dt.Rows[i]["TINNo"].ToString(), dt.Rows[i]["ServiceTaxNo"].ToString(), dt.Rows[i]["ITPAN"].ToString(), 0, 0, 0, 0, Convert.ToInt32(DTadmin.Rows[0]["Userid"]), dt.Rows[i]["TelNoResi"].ToString(), dt.Rows[i]["Fax"].ToString(), dt.Rows[i]["PrintName"].ToString(), Convert.ToInt32(DTadmin.Rows[0]["SMId"]), Convert.ToString(""), Convert.ToString(""), Areaid);

        //                        //    if (retval_update == -1)
        //                        //    {
        //                        //        //'Duplicate User Name Exists'
        //                        //        ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not updated,Something went wrong";
        //                        //        msglist.Add(ms);
        //                        //        failure = failure + 1;
        //                        //        continue;
        //                        //    }
        //                        //    else if (retval_update == -2)
        //                        //    {
        //                        //        //'Duplicate SyncId Exists'
        //                        //        ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not updated,Something went wrong";
        //                        //        msglist.Add(ms);
        //                        //        failure = failure + 1;
        //                        //        continue;
        //                        //    }

        //                        //    else if (retval_update == -3)
        //                        //    {
        //                        //        //'Duplicate Email Exists'
        //                        //        ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not updated,Something went wrong";
        //                        //        msglist.Add(ms);
        //                        //        failure = failure + 1;
        //                        //        continue;
        //                        //    }
        //                        //    else if (retval_update == -5)
        //                        //    {
        //                        //        //'Duplicate Mobile Exists'
        //                        //        ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not updated,Something went wrong";
        //                        //        msglist.Add(ms);
        //                        //        failure = failure + 1;
        //                        //        continue;
        //                        //    }
        //                        //    else
        //                        //    {
        //                        //        successupdated = successupdated + 1;
        //                        //    }
        //                        //}
        //                        //else if (retval == -3)
        //                        //{
        //                        //    //'Duplicate Distributor Exists'
        //                        //    ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not inserted,Something went wrong";
        //                        //    msglist.Add(ms);
        //                        //    failure = failure + 1;
        //                        //    continue;
        //                        //}
        //                        //else if (retval == -4)
        //                        //{
        //                        //    //'Duplicate Email Exists'
        //                        //    ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not inserted,Something went wrong";
        //                        //    msglist.Add(ms);
        //                        //    failure = failure + 1;
        //                        //    continue;
        //                        //}
        //                        //else if (retval == -5)
        //                        //{
        //                        //    //'Duplicate Mobile Exists'
        //                        //    ms.msdmsg = dt.Rows[i]["Name"].ToString() + " is not inserted,Something went wrong";
        //                        //    msglist.Add(ms);
        //                        //    failure = failure + 1;
        //                        //    continue;
        //                        //}
        //                        //else
        //                        //{
        //                        //    success = success + 1;
        //                        //}
        //                    }
        //                }

        //                str = success + " record inserted successfully, " + successupdated + " record updated successfully";
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

        //                LogError(str, "Success", "BusyAccountSyncData");

        //                rs.msg = str;
        //                rs.errormsg = msglist;
        //                // this.SucessEmailStatus();
        //            }
        //            catch (Exception ex)
        //            {
        //                //transactionScope.Dispose();
        //                cnt = 0;
        //                str = ex.Message;
        //                err = err + 1;
        //                LogError(str, "Fail", "BusyAccountSyncData");
        //                // msglist..Add(str);
        //                if (err > 0)
        //                    str = str + ",Check log table";
        //                rs.msg = str;
        //                rs.errormsg = msglist;

        //            }
        //            //return rs;
        //        }

        //        private void LogError(string str, string status, string functionname)
        //        {
        //            string query = "insert into Log (Description,Status,MethodName,ErrorTime) values ('" + str.Replace("'", "''") + "','" + status + "','" + functionname + "',Date())";
        //            OleDbCommand cmd = new OleDbCommand();
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = query;
        //            cmd.Connection = Conn;
        //            if (Conn != null && Conn.State == ConnectionState.Closed)
        //            {
        //                Conn.Open();
        //            }
        //            cmd.ExecuteNonQuery();
        //            Conn.Close();
        //        }

        //        private void BusySalesSyncData()
        //        {
        //            List<Result1> rst = new List<Result1>();
        //            DataTable dt = new DataTable();
        //            string str = "";
        //            int cnt = 0;
        //            int DistInvId = 0;
        //            int DistInv1Id = 0;
        //            int success = 0;
        //            int successupdated = 0;
        //            int failure = 0;
        //            int err = 0;
        //            int sno = 0;
        //            string companycode = "";
        //            string DistInvDocId = "";
        //            string creationtime = "";
        //            List<mandatorymsg> msglist = new List<mandatorymsg>();
        //            errorresult rs = new errorresult();
        //            try
        //            {

        //                str = "select GUID from Company";
        //                DataTable dtcompany = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, str, dbFlag);
        //                if (dtcompany.Rows.Count > 0)
        //                {
        //                    companycode = dtcompany.Rows[0]["GUID"].ToString();

        //                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete from TransDistInv where Compcode='" + companycode + "'", connectionstring3, dbFlag);
        //                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete from TransDistInv1 where Compcode='" + companycode + "'", connectionstring3, dbFlag);

        //                    str = "   select convert(varchar, t1.Date, 23) + ' ' + convert(varchar, t1.CreationTime, 8) as CTime,t1.VchCode,t1.VchNo,t1.Date,t1.CreationTime,t1.VchAmtBaseCur,t1.VchSalePurcAmt,t1.MasterCode1,t1.MasterCode2,m1.Name from Tran1 t1 left join Master1 m1 on t1.MasterCode1=m1.Code  where VchType=9 order by t1.VchCode  asc";

        //                    dt = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, str, dbFlag);
        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        for (int i = 0; i < dt.Rows.Count; i++)
        //                        {
        //                            creationtime = dt.Rows[i]["CTime"].ToString();
        //                            str = "select * from Tran2 where VchCode=" + dt.Rows[i]["VchCode"].ToString() + " and RecType=2";

        //                            DataTable dt1 = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, str, dbFlag);
        //                            if (dt1.Rows.Count > 0)
        //                            {
        //                                for (int j = 0; j < dt1.Rows.Count; j++)
        //                                {
        //                                    str = "select * from Master1 where code=" + dt1.Rows[j]["MasterCode1"].ToString() + "";

        //                                    DataTable dt2 = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, str, dbFlag);
        //                                    if (dt2.Rows.Count > 0)
        //                                    {
        //                                        for (int k = 0; k < dt2.Rows.Count; k++)
        //                                        {
        //                                            if (!String.IsNullOrEmpty(dt2.Rows[j]["Alias"].ToString()))
        //                                            {
        //                                                sno = sno + 1;
        //                                                mandatorymsg ms = new mandatorymsg();
        //                                                DistInvDocId = "BUSY" + " " + Convert.ToString(Convert.ToDateTime(creationtime).Year) + " " + dt.Rows[i]["VchNo"].ToString();

        //                                                DistInvId = DB.insertsaleinvheader_Busy(connectionstring2, DistInvDocId, creationtime, 0, Convert.ToDecimal(dt.Rows[i]["VchSalePurcAmt"].ToString()), 0, dt.Rows[i]["MasterCode1"].ToString(), companycode);

        //                                                if (DistInvId == -2)
        //                                                {
        //                                                    ms.msdmsg = "Party -" + dt.Rows[i]["Name"].ToString() + " not exist on web portal for Bill no " + dt.Rows[i]["VchNo"].ToString();
        //                                                    msglist.Add(ms);
        //                                                    failure = failure + 1;

        //                                                }
        //                                                else
        //                                                {
        //                                                    decimal amt = Convert.ToDecimal(dt1.Rows[j]["Value1"].ToString()) * Convert.ToDecimal(dt2.Rows[j]["D3"].ToString());
        //                                                    DistInv1Id = DB.insertsaleinvdetail_Busy(connectionstring2, DistInvId, sno, DistInvDocId, creationtime, 0, Convert.ToDecimal(dt1.Rows[j]["Value1"].ToString()), Convert.ToDecimal(dt2.Rows[j]["D3"].ToString()), amt, dt.Rows[i]["MasterCode1"].ToString(), dt2.Rows[j]["Name"].ToString(), dt2.Rows[j]["Code"].ToString(), companycode);
        //                                                    if (DistInv1Id > 0)
        //                                                    {

        //                                                        success = success + 1;
        //                                                    }
        //                                                }
        //                                            }
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

        //                LogError(str, "Success", "BusySalesSyncData");

        //                rs.msg = str;
        //                rs.errormsg = msglist;
        //                // this.SucessEmailStatus();
        //            }
        //            catch (Exception ex)
        //            {
        //                //transactionScope.Dispose();
        //                cnt = 0;
        //                str = ex.Message;
        //                err = err + 1;
        //                LogError(str, "Fail", "BusySalesSyncData");
        //                // msglist..Add(str);
        //                if (err > 0)
        //                    str = str + ",Check log table";
        //                rs.msg = str;
        //                rs.errormsg = msglist;

        //            }
        //            //return rs;
        //        }

        //        private void BusyLedgerSyncData(DataTable dt_param)
        //        {
        //            List<Result1> rst = new List<Result1>();
        //            decimal _amount = 0;
        //            decimal amtdr = 0;
        //            decimal amtcr = 0;
        //            string docID;
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

        //                str = "select GUID from Company";
        //                DataTable dtcompany = DbConnectionDAL.GetDataTableForBusyData(connectionstring1, CommandType.Text, str, dbFlag);
        //                if (dtcompany.Rows.Count > 0)
        //                {

        //                    companycode = dtcompany.Rows[0]["GUID"].ToString();

        //                    DbConnectionDAL.ExecuteNonQuery(CommandType.Text, "Delete FROM TransDistributerLedger WHERE DistLedId NOT In(SELECT DistLedId FROM TransDistributerLedger WHERE Narration=' Opening Balance' OR COMPANYCODE !='" + companycode + "') ", connectionstring3, dbFlag);

        //                    str = "select SyncId from MastParty where SyncId='" + dt_param.Rows[0]["DistributorID"].ToString() + "'";

        //                    DataTable dt_syncid = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, str, dbFlag);


        //                    str = "   select MasterCode1,VchType,VchCode,convert(varchar, Date, 23) as CDate,VchNo,Value1,ShortNar from Tran2 where MasterCode1=" + dt_syncid.Rows[0]["SyncId"].ToString() + " order by Date  asc";

        //                    dt = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, str, dbFlag);
        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        for (int i = 0; i < dt.Rows.Count; i++)
        //                        {

        //                            if (dt.Rows[i]["VchType"].ToString() == "9")
        //                            {
        //                                Vtype = "Sales";
        //                            }
        //                            else if (dt.Rows[i]["VchType"].ToString() == "19")
        //                            {
        //                                Vtype = "Payment";
        //                            }
        //                            else if (dt.Rows[i]["VchType"].ToString() == "14")
        //                            {
        //                                Vtype = "Receipt";
        //                            }
        //                            else if (dt.Rows[i]["VchType"].ToString() == "12")
        //                            {
        //                                Vtype = "Sales Order";
        //                            }
        //                            else if (dt.Rows[i]["VchType"].ToString() == "16")
        //                            {
        //                                Vtype = "Journal";
        //                            }

        //                            if (dt.Rows[i]["Value1"].ToString() != "")
        //                            {
        //                                if (Vtype == "Sales" || Vtype == "Payment")
        //                                {
        //                                    amtdr = Convert.ToDecimal(dt.Rows[i]["Value1"].ToString());
        //                                }
        //                                else
        //                                {
        //                                    amtcr = Convert.ToDecimal(dt.Rows[i]["Value1"].ToString());
        //                                }
        //                            }

        //                            docID = "BS" + Vtype.Substring(0, 3).ToUpper() + companycode.Substring(companycode.Length - 3) + " " + Convert.ToString(Convert.ToDateTime(dt.Rows[i]["CDate"].ToString()).Year) + " " + dt.Rows[i]["VchCode"].ToString();

        //                            if (Vtype != "")
        //                                Narration = dt.Rows[i]["ShortNar"].ToString() + "  (Vch Type-" + Vtype + ")";
        //                            else
        //                                Narration = dt.Rows[i]["ShortNar"].ToString();
        //                            string _query = "Select isnull(PartyId,0) from MastParty where SyncId='" + dt_syncid.Rows[0]["SyncId"].ToString() + "' and compcode='" + companycode + "' And partydist=1";
        //                            int _DistID = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query, connectionstring3));
        //                            if (_DistID != 0)
        //                            {
        //                                if (amtcr > 0) _amount = amtcr * -1;
        //                                else if (amtdr > 0) _amount = amtdr;

        //                                result = DB.insertdistledger_Busy(connectionstring3, _DistID, docID,
        //                                    dt.Rows[i]["CDate"].ToString(),
        //                                    amtdr, amtcr, _amount, Narration, dt.Rows[i]["VchCode"].ToString(), Vtype,
        //                                    companycode);

        //                                if (result > 0)
        //                                {
        //                                    success = success + 1;
        //                                    amtcr = 0;
        //                                    amtdr = 0;
        //                                    _amount = 0;


        //                                }
        //                                else
        //                                {
        //                                    failure = failure + 1;
        //                                    amtcr = 0;
        //                                    amtdr = 0;
        //                                    _amount = 0;

        //                                }



        //                            }
        //                            amtcr = 0;
        //                            amtdr = 0;
        //                            _amount = 0;
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

        //        private Purchorderlist PostPurchOrderToBusy()
        //        {
        //            DataTable DTpurchorder = new DataTable();
        //            DataTable DTpurchorder1 = new DataTable();
        //            List<Transpurchorder> purchorder = new List<Transpurchorder>();
        //            Purchorderlist POL = new Purchorderlist();
        //            decimal _amt = 0;
        //            string _query;



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

        //            return POL;

        //        }

        //        private void BusySalesOrderSyncData(Purchorderlist pol_lst)
        //        {
        //            List<Result1> rst = new List<Result1>();
        //            decimal _amount = 0;
        //            decimal amtdr = 0;
        //            decimal amtcr = 0;
        //            string docID;
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

        //        private void PostItemsToBusy()
        //        {
        //            DataTable DTItems = new DataTable();
        //            decimal _amt = 0;
        //            string _query;

        //            _query = @"Select * from MastItem where ItemId <> 1 and ItemType='ITEM'";
        //            DataTable dt_items = DbConnectionDAL.GetDataTable(CommandType.Text, _query, connectionstring3);
        //            for (int i = 0; i < dt_items.Rows.Count; i++)
        //            {
        //                if (dbFlag == "S")
        //                {
        //                    string query_cnt = "select code from Master1 where Alias='" + dt_items.Rows[i]["ItemId"].ToString() + "' and MasterType=6";
        //                    DataTable dt_cnt = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, query_cnt, dbFlag);
        //                    if (dt_cnt.Rows.Count <= 0)
        //                    {
        //                        int code = Convert.ToInt32(DbConnectionDAL.GetScalarValueBusy(CommandType.Text, "SELECT max(Code) from Master1", connectionstring2));

        //                        query_cnt = "select code from Master1 where Alias='" + dt_items.Rows[i]["UnderId"].ToString() + "' and MasterType=5";
        //                        DataTable dt_itemgrp_cnt = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, query_cnt, dbFlag);
        //                        if (dt_itemgrp_cnt.Rows.Count > 0)
        //                        {

        //                            string query_unit = "select code from Master1 where Name='" + dt_cnt.Rows[i]["Unit"].ToString() + "'";
        //                            DataTable dt_unit = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, query_unit, dbFlag);

        //                            _query = "insert into master1 (Code,MasterType,Alias,PrintName,ParentGrp,Stamp,CM1,CM1,CM3,CM4,CM8,CM10,D2,CreatedBy,CreationTime) values(" + (code + 1) + ",6,'" + dt_cnt.Rows[i]["ItemName"].ToString() + "','" + dt_cnt.Rows[i]["ItemName"].ToString() + "',1283,1," + Convert.ToInt32(code) + "," + Convert.ToInt32(code) + ",403," + Convert.ToInt32(code) + "," + dt_cnt.Rows[i]["MRP"].ToString() + ",'Tanvi',getDate())";

        //                            int cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query, connectionstring2));
        //                            if (cnt > 0)
        //                            {

        //                            }
        //                        }
        //                        else
        //                        {
        //                            //create item group
        //                            _query = @"Select * from MastItem where ItemType='MATERIALGROUP' and ItemId=" + dt_items.Rows[i]["UnderId"].ToString() + "";
        //                            DataTable dt_itemgrpdetails = DbConnectionDAL.GetDataTable(CommandType.Text, _query, connectionstring3);
        //                            if (dt_itemgrpdetails.Rows.Count > 0)
        //                            {

        //                                _query = "insert into master1 (Code,MasterType,Alias,PrintName,CreatedBy,CreationTime) values(" + (code + 1) + ",5,'" + dt_itemgrpdetails.Rows[i]["ItemName"].ToString() + "','" + dt_itemgrpdetails.Rows[i]["ItemName"].ToString() + "','Tanvi',getDate())";

        //                                int cnt = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query, connectionstring2));
        //                                if (cnt > 0)
        //                                {

        //                                    string query_unit = "select code from Master1 where Name='" + dt_cnt.Rows[i]["Unit"].ToString() + "'";
        //                                    DataTable dt_unit = DbConnectionDAL.GetDataTableForBusyData(connectionstring2, CommandType.Text, query_unit, dbFlag);

        //                                    _query = "insert into master1 (Code,MasterType,Alias,PrintName,ParentGrp,Stamp,CM1,CM1,CM3,CM4,CM8,CM10,D2,CreatedBy,CreationTime) values(" + (code + 2) + ",6,'" + dt_cnt.Rows[i]["ItemName"].ToString() + "','" + dt_cnt.Rows[i]["ItemName"].ToString() + "'," + dt_cnt.Rows[i]["Underid"].ToString() + ",1," + Convert.ToInt32(code) + "," + Convert.ToInt32(code) + ",403," + Convert.ToInt32(code) + "," + dt_cnt.Rows[i]["MRP"].ToString() + ",'Tanvi',getDate())";

        //                                    int cnt1 = Convert.ToInt32(DbConnectionDAL.GetScalarValue(CommandType.Text, _query, connectionstring2));
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {

        //                }
        //            }

        //        }

        //        #endregion

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                // Wait 50 milliseconds.  
                Thread.Sleep(500);
                // Report progress.  
                //backgroundWorker1.ReportProgress(i);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar  
            //progressBar1.Value = e.ProgressPercentage;
            // Set the text.  
            this.Text = "Progress: " + e.ProgressPercentage.ToString() + "%";
        }

        private void Start_Shown(object sender, EventArgs e)
        {
            //backgroundWorker1.WorkerReportsProgress = true;
            //backgroundWorker1.RunWorkerAsync();
            CheckandtransferData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != String.Empty && comboBox1.SelectedIndex > 0)
            {
                string strSQL = "SELECT * FROM Enviro";
                OleDbDataAdapter myCmd = new OleDbDataAdapter(strSQL, Conn);
                DataSet dtSet = new DataSet();
                myCmd.Fill(dtSet, "Enviro");
                DataTable dTabledata = dtSet.Tables[0];
                string source = dTabledata.Rows[0]["Source"].ToString();
                connectionstring1 = "";

                connectionstring1 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dTabledata.Rows[0]["Busy_AccessDbPath"].ToString() + "\\" + comboBox1.Text + ";Jet OLEDB:Database Password=" + dTabledata.Rows[0]["Busy_AccessDbPassword"].ToString() + "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button3.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            pictureBox1.Visible = true;
            backgroundWorker2.RunWorkerAsync();
            //if ((comboBox1.Text != String.Empty && comboBox1.SelectedIndex > 0) && comboBox2.Text != String.Empty)
            //{
            //    button1.Enabled = false;
            //    button3.Enabled = false;
            //    comboBox1.Enabled = false;
            //    comboBox2.Enabled = false;

            //    string compcode = comboBox1.Text.Substring(0, comboBox1.Text.IndexOf('\\'));
            //    string compfile = comboBox1.Text.ToString();

            //    if (comboBox2.SelectedIndex == 0)
            //    {
            //        busy_Conn1 = new OleDbConnection(connectionstring1);

            //        busy_Conn1.Open();
            //        if (busy_Conn1.State == ConnectionState.Open)
            //        {
            //            PostItemsToBusy(compcode, compfile);
            //        }
            //        busy_Conn1.Close();
            //    }
            //    else if (comboBox2.SelectedIndex == 1)
            //    {
            //        busy_Conn1 = new OleDbConnection(connectionstring1);

            //        busy_Conn1.Open();
            //        if (busy_Conn1.State == ConnectionState.Open)
            //        {
            //            BusySalesPerson(compcode, compfile);
            //        }
            //        busy_Conn1.Close();
            //    }
            //    else if (comboBox2.SelectedIndex == 2)
            //    {
            //        busy_Conn1 = new OleDbConnection(connectionstring1);

            //        busy_Conn1.Open();
            //        if (busy_Conn1.State == ConnectionState.Open)
            //        {
            //            BusyAccountSyncData(compcode, compfile);
            //        }
            //        busy_Conn1.Close();
            //    }
            //    else if (comboBox2.SelectedIndex == 3)
            //    {
            //        busy_Conn1 = new OleDbConnection(connectionstring1);

            //        busy_Conn1.Open();
            //        if (busy_Conn1.State == ConnectionState.Open)
            //        {
            //            BusySalesSyncData(compcode, compfile);
            //        }
            //        busy_Conn1.Close();
            //    }
            //    else if (comboBox2.SelectedIndex == 4)
            //    {
            //        busy_Conn1 = new OleDbConnection(connectionstring1);

            //        busy_Conn1.Open();
            //        if (busy_Conn1.State == ConnectionState.Open)
            //        {
            //            BusyLedgerSyncData(compcode, compfile);
            //        }
            //        busy_Conn1.Close();
            //    }
            //    label1.Text = "Transfer Complete";
            //    button1.Enabled = true;
            //    button3.Enabled = true;
            //    comboBox1.Enabled = true;
            //    comboBox2.Enabled = true;
            //}
            //else
            //{
            //    MessageBox.Show("Please select FIle and Modules");
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string strSQL = "SELECT * FROM Enviro";
            OleDbDataAdapter myCmd = new OleDbDataAdapter(strSQL, Conn);
            DataSet dtSet = new DataSet();
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

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(500);

            string strSQL = "SELECT * FROM Enviro";
            OleDbDataAdapter myCmd = new OleDbDataAdapter(strSQL, Conn);
            DataSet dtSet = new DataSet();
            myCmd.Fill(dtSet, "Enviro");
            DataTable dTabledata = dtSet.Tables[0];

           

            label1.Invoke(new MethodInvoker(() => label1.Text = String.Empty));
            //;
            string cmbbx1 = "", cmbbx2 = "", cmbbxv1 = "", cmbbxv2 = "";
            comboBox1.Invoke(new MethodInvoker(() => cmbbx1 = comboBox1.Text));
            comboBox2.Invoke(new MethodInvoker(() => cmbbx2 = comboBox2.Text));
            comboBox1.Invoke(new MethodInvoker(() => cmbbxv1 = comboBox1.SelectedIndex.ToString()));
            comboBox2.Invoke(new MethodInvoker(() => cmbbxv2 = comboBox2.SelectedIndex.ToString()));
            if (cmbbx1 != "" && cmbbx2 != "")
            {
                string compcode = cmbbx1.Substring(0, cmbbx1.IndexOf('\\'));
                string compfile = cmbbx1.ToString();
                connectionstring1 = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dTabledata.Rows[0]["Busy_AccessDbPath"].ToString() + "\\" + compfile.ToString() + ";Jet OLEDB:Database Password=" + dTabledata.Rows[0]["Busy_AccessDbPassword"].ToString() + "";
                if (cmbbxv2 == "0")
                {
                    busy_Conn1 = new OleDbConnection(connectionstring1);

                    busy_Conn1.Open();
                    if (busy_Conn1.State == ConnectionState.Open)
                    {
                        label1.Invoke(new MethodInvoker(() => label1.Text = compfile + " Item Transferring........"));
                        PostItemsToBusy(compcode, compfile);
                    }
                    busy_Conn1.Close();
                }
                else if (cmbbxv2 == "1")
                {
                    busy_Conn1 = new OleDbConnection(connectionstring1);

                    busy_Conn1.Open();
                    if (busy_Conn1.State == ConnectionState.Open)
                    {
                        label1.Invoke(new MethodInvoker(() => label1.Text = compfile + " Sales Person Transferring........"));
                        BusySalesPerson(compcode, compfile);
                    }
                    busy_Conn1.Close();
                }
                else if (cmbbxv2 == "2")
                {
                    busy_Conn1 = new OleDbConnection(connectionstring1);

                    busy_Conn1.Open();
                    if (busy_Conn1.State == ConnectionState.Open)
                    {
                        label1.Invoke(new MethodInvoker(() => label1.Text = compfile + " Distributor Transferring........"));
                        BusyAccountSyncData(compcode, compfile);
                    }
                    busy_Conn1.Close();
                }
                else if (cmbbxv2 == "3")
                {
                    busy_Conn1 = new OleDbConnection(connectionstring1);

                    busy_Conn1.Open();
                    if (busy_Conn1.State == ConnectionState.Open)
                    {
                        label1.Invoke(new MethodInvoker(() => label1.Text = compfile + " Invoice Transferring........"));
                        BusySalesSyncData(compcode, compfile);
                    }
                    busy_Conn1.Close();
                }
                else if (cmbbxv2 == "4")
                {
                    busy_Conn1 = new OleDbConnection(connectionstring1);

                    busy_Conn1.Open();
                    if (busy_Conn1.State == ConnectionState.Open)
                    {
                        label1.Invoke(new MethodInvoker(() => label1.Text = compfile + " Ledger Transferring........"));
                        BusyLedgerSyncData(compcode, compfile);
                    }
                    busy_Conn1.Close();
                }
                else if (cmbbxv2 == "5")
                {
                    busy_Conn1 = new OleDbConnection(connectionstring1);

                    busy_Conn1.Open();
                    if (busy_Conn1.State == ConnectionState.Open)
                    {
                        label1.Invoke(new MethodInvoker(() => label1.Text = compfile + " Aging Transferring........"));
                        BusyAgileSyncData(compcode, compfile);
                    }
                    busy_Conn1.Close();
                }
                pictureBox1.Invoke(new MethodInvoker(() => pictureBox1.Visible = false));
                comboBox1.Invoke(new MethodInvoker(() => comboBox1.Text = String.Empty));
                comboBox2.Invoke(new MethodInvoker(() => comboBox2.Text = String.Empty));
                label1.Invoke(new MethodInvoker(() => label1.Text = "Transfer Complete"));
                //;
            }
            else
            {
                MessageBox.Show("Please select FIle and Modules");
            }


        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            button1.Invoke(new MethodInvoker(() => button1.Enabled = true));
            button3.Invoke(new MethodInvoker(() => button3.Enabled = true));
            comboBox1.Invoke(new MethodInvoker(() => comboBox1.Enabled = true));
            comboBox2.Invoke(new MethodInvoker(() => comboBox2.Enabled = true));
            //button1.Enabled = true;
            //button3.Enabled = true;
            //comboBox1.Enabled = true;
            //comboBox2.Enabled = true;
        }
    }
}
