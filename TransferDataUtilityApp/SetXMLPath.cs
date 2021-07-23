using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransferDataUtilityApp
{
    public partial class MargConfig : Form
    {
        OleDbConnection Conn;
        OleDbCommand Cmd;
        string connectionstr;
        public MargConfig()
        {
            InitializeComponent();
            connectionstr = ConfigurationManager.ConnectionStrings["AccessString"].ConnectionString;
            Conn = new OleDbConnection(connectionstr);
            OleDbDataAdapter myCmd;
            string strSQL = "SELECT * FROM Enviro";
            myCmd = new OleDbDataAdapter(strSQL, Conn);
            DataSet dtSet = new DataSet();
            myCmd.Fill(dtSet, "Enviro");
            DataTable dTabledata = dtSet.Tables[0];
            if (dTabledata.Rows.Count > 0)
            {
                txtdist.Text = dTabledata.Rows[0]["DistributorID"].ToString();
                textBox1.Text = dTabledata.Rows[0]["MargXMLPath"].ToString();

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool Isvalid = true;
            try
            {

                if (textBox1.Text == "")
                {
                    // MessageBox.Show("Please Enter XML File Path.");
                    MessageBox.Show("Please Enter XML File Path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Isvalid = false;

                }
                if (txtdist.Text == "")
                {
                    MessageBox.Show("Please Enter Distributor Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Isvalid = false;
                }

                if (Isvalid)
                {
                    string updatequery = "Update Enviro set MargXMLPath='" + textBox1.Text + "',DistributorID=" + Convert.ToInt32(txtdist.Text.ToString()) + ",DestinationServer='" + txtdestinationserver.Text + "',DestinationDatabase='" + txtdestinationdatabase.Text + "',DestinationUser='" + txtdestinationuser.Text + "',DestinationPassword='" + txtdestinationpassword.Text + "'";

                    string insertquery = "insert into Enviro (Source,MargXMLPath,DistributorID,DestinationServer,DestinationDatabase,DestinationUser,DestinationPassword) values ('Marg','" + textBox1.Text + "','" + txtdist.Text + "','" + txtdestinationserver.Text + "','" + txtdestinationdatabase.Text + "','" + txtdestinationuser.Text + "','" + txtdestinationpassword.Text + "')";
                    int cnt = 0;

                    string strSQL = "SELECT count(*) as cnt FROM Enviro";
                    OleDbDataAdapter myCmd = new OleDbDataAdapter(strSQL, Conn);
                    Conn.Open();
                    DataSet dtSet = new DataSet();
                    myCmd.Fill(dtSet, "Enviro");
                    DataTable dTable = dtSet.Tables[0];
                    cnt = Convert.ToInt32(dTable.Rows[0]["cnt"].ToString());
                    //Conn.Close();


                    OleDbCommand cmd = new OleDbCommand();
                    cmd.CommandType = CommandType.Text;
                    if (cnt > 0)
                        cmd.CommandText = updatequery;
                    else
                        cmd.CommandText = insertquery;
                    cmd.Connection = Conn;
                    //Conn.Open();
                    cmd.ExecuteNonQuery();







                    DialogResult result = System.Windows.Forms.MessageBox.Show("Data Saved Succefully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
