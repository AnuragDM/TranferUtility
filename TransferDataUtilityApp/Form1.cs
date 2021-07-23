//using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferDataUtilityApp.CommonClass;

namespace TransferDataUtilityApp
{
    public partial class Form1 : Form
    {

        OleDbConnection Conn;
        OleDbCommand Cmd;
        OleDbDataAdapter Da;
        OdbcConnection Odbcconn;
        OdbcDataAdapter odbcdatad;
        SqlConnection sqlconn;
        SqlCommand sqlcmd;
        SqlDataAdapter sqlda;
        //ClsProcedure cs = new ClsProcedure();
        string connectionstr;
        public Form1()
        {
            InitializeComponent();

            connectionstr = ConfigurationManager.ConnectionStrings["AccessString"].ConnectionString;
            Conn = new OleDbConnection(connectionstr);
        }

        private void btn_Click(object sender, EventArgs e)
        {
            try
            {
                string _sql = "";
                int cnt = 0;
                _sql = "Select Count(*) from Enviro";
                using (Cmd = new OleDbCommand(_sql, Conn))
                {
                    if (Conn.State == ConnectionState.Open)
                    { }
                    else
                    { Conn.Open(); }
                    cnt = Convert.ToInt32(Cmd.ExecuteScalar());
                    if (cnt > 0)
                    {
                        //_sql = "update enviro set Source='Tally',Tally_Server='" + txtserver.Text + "',Tally_Port='" + txtport.Text + "',Tally_Version='" + cmdtallyversion.SelectedItem.ToString() + "',Transferfor='" + cmdtransferfor.SelectedItem.ToString() + "',DestinationServer='" + txtdestserver.Text + "',DestinationDatabase='" + txtdestidatabase.Text + "',DestinationUser='" + txtdestuser.Text + "',DestinationPassword='" + txtdestpassword.Text + "'";
                        Transferdata();



                    }
                    else
                    {
                        _sql = "insert into enviro (Source,Tally_Server,Tally_Port,Tally_Version,Transferfor,DestinationServer,DestinationDatabase,DestinationUser,DestinationPassword) values('Tally','" + txtserver.Text + "','" + txtport.Text + "','" + cmdtallyversion.SelectedItem.ToString() + "','" + cmdtransferfor.SelectedItem.ToString() + "','" + txtdestserver.Text + "','" + txtdestidatabase.Text + "','" + txtdestuser.Text + "','" + txtdestpassword.Text + "')";
                        Cmd = new OleDbCommand(_sql, Conn);
                        Cmd.ExecuteNonQuery();

                    }

                    //Cmd.ExecuteNonQuery();
                }

                //DataTable dt = new DataTable();
                //string tallyodbcconnection = "DRIVER={Tally ODBC Driver};SERVER=(192.168.2.23);PORT=9000";

                //OdbcConnection conn = new OdbcConnection(tallyodbcconnection);

                //string str = "Select $MasterID,$Name from Ledger";
                //OdbcDataAdapter da = new OdbcDataAdapter(str,conn);
                //da.Fill(dt);



            }
            catch (Exception)
            {

                throw;
            }

        }
        private void Transferdata()
        {
            string _sql = "SELECT * from Enviro";
            string Driver = "";
            DataTable dt = new DataTable();
            try
            {


                using (Cmd = new OleDbCommand(_sql, Conn))
                {
                    if (Conn.State == ConnectionState.Open)
                    { }
                    else
                    { Conn.Open(); }

                    //    cmd.CommandText = _sql;
                    Da = new OleDbDataAdapter(Cmd);
                    Da.Fill(dt);

                    if (dt.Rows.Count > 0 && dt.Rows[0]["Source"].ToString().ToUpper().Trim() == "TALLY")
                    {
                        if (dt.Rows[0]["Tally_Version"].ToString() == "32-bit")
                        {
                            Driver = "Tally ODBC Driver";
                        }
                        else
                        {
                            Driver = "Tally ODBC Driver64";
                        }
                        string tallyodbcconnection = "DRIVER={" + Driver + "};SERVER=(" + dt.Rows[0]["Tally_Server"].ToString() + ");PORT=" + dt.Rows[0]["Tally_Port"].ToString() + "";

                        Odbcconn = new OdbcConnection(tallyodbcconnection);
                        sqlconn = new SqlConnection("Data Source=" + dt.Rows[0]["DestinationServer"].ToString() + ";Initial Catalog=" + dt.Rows[0]["DestinationDatabase"].ToString() + ";User ID=" + dt.Rows[0]["DestinationUser"].ToString() + ";Password=" + dt.Rows[0]["DestinationPassword"].ToString() + "");
                        GetDistributor(Odbcconn, sqlconn, Convert.ToInt32(dt.Rows[0]["DistributorID"].ToString()));
                    }
                    else
                    {

                    }

                }
            }
            catch (Exception ex)
            {

                using (Cmd = new OleDbCommand("Insert into Log (Error,MethodName,ErrorTime) values('" + ex.ToString() + "','Transferdata','" + DateTime.Now.ToString() + "') ", Conn))
                {
                    if (Conn.State == ConnectionState.Open)
                    { }
                    else
                    { Conn.Open(); }
                    Cmd.ExecuteNonQuery();
                }

            }


        }
        private void GetDistributor(OdbcConnection Tallyconn,SqlConnection Destinationconn,int underID)
        {
            DataTable dtDistributor = new DataTable();
            int cityid = 0, regionid = 0, citytypeid = 0, cityconveyancetype = 0, distictid = 0,countryid = 0,stateid = 0,Areaid = 0, roleid = 0, opvoucherno = 1,result = 0;
            string _sql = "";
            string str = "Select $MasterID,$Address,$Email,$PinCode,$Creditlimit,$Name,$Parent,$OpeningBalance,$Ledgermobile,$Ledgerphone,$Addressnative,$Ledgercontact,$Partygstin,$Ledgerfax,$IncomeTaxNumber,$Ledstatename,$CountryName,##SVCurrentCompany from Ledger where $Parent=$$GroupSundryDebtors";
            OdbcDataAdapter da = new OdbcDataAdapter(str, Tallyconn);
            da.Fill(dtDistributor);

            DataTable dtcompany = new DataTable();
            string str1 = "Select $CompanyNumber From Company ";
            da = new OdbcDataAdapter(str1, Tallyconn);
            da.Fill(dtcompany);
            using (sqlcmd = new SqlCommand("SELECT AreaId from MastArea where AreaType='CITY' AND AreaName='Blank'", Destinationconn))
            {
                if (Destinationconn.State == ConnectionState.Open)
                { }
                else
                { Destinationconn.Open(); }
                cityid = Convert.ToInt32(sqlcmd.ExecuteScalar());
                sqlcmd = new SqlCommand("SELECT AreaId from MastArea where AreaType='REGION' AND AreaName='Blank'", Destinationconn);
                regionid = Convert.ToInt32(sqlcmd.ExecuteScalar());
                sqlcmd = new SqlCommand("Select AreaId from MastArea where Areatype='DISTRICT' And AreaName='Blank'", Destinationconn);
                distictid = Convert.ToInt32(sqlcmd.ExecuteScalar());
                sqlcmd = new SqlCommand("Select Id From MastCityType Where Name='Other'", Destinationconn);
                citytypeid = Convert.ToInt32(sqlcmd.ExecuteScalar());
                sqlcmd = new SqlCommand("Select Id From MastCityType Where Name='OTHERS'", Destinationconn);
                cityconveyancetype = Convert.ToInt32(sqlcmd.ExecuteScalar());
                sqlcmd = new SqlCommand("Select RoleId From MastRole Where RoleName='DISTRIBUTOR'", Destinationconn);
                roleid = Convert.ToInt32(sqlcmd.ExecuteScalar());
                DataTable DTadmin=new DataTable();
                sqlda = new SqlDataAdapter("Select * from MastSalesRep Where SMName='DIRECTOR'", Destinationconn);
                sqlda.Fill(DTadmin);

                for (int i = 0; i < dtDistributor.Rows.Count; i++)
                {
                    sqlcmd = new SqlCommand("SELECT AreaId from MastArea where AreaType='AREA' AND AreaName='Area-" + dtDistributor.Rows[i]["$Name"].ToString() + "'", Destinationconn);
                    Areaid = Convert.ToInt32(sqlcmd.ExecuteScalar());
                    if (dtDistributor.Rows[i]["$Creditlimit"].ToString() == "")
                        dtDistributor.Rows[i]["$Creditlimit"] = "0";

                //   cs.InsertDistributors_Tally( Destinationconn.ConnectionString


                }

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtport_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtserver_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
