using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransferDataUtilityApp
{
    public partial class MainApplication : Form
    {
        OleDbConnection Conn;
        OleDbCommand Cmd;
        string connectionstr;
        public MainApplication()
        {
            InitializeComponent();
            connectionstr = ConfigurationManager.ConnectionStrings["AccessString"].ConnectionString;
            Conn = new OleDbConnection(connectionstr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selectedvalue = comboBox1.SelectedIndex;
            if (selectedvalue >= 0)
            {
                if (selectedvalue == 0)
                {
                    BusyConfigSettings bs = new BusyConfigSettings();              
                    bs.Show();
                    this.Hide();
                }
                else if (selectedvalue == 1)
                {
                    MargConfig bs = new MargConfig();
                    bs.Show();
                    this.Hide();
                }
                else if (selectedvalue == 2)
                {
                    Form1 fm = new Form1();
                    this.Hide();
                    fm.Show();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please Select the Source Application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MainApplication_Load(object sender, EventArgs e)
        {
            
        }

       
    }
}
