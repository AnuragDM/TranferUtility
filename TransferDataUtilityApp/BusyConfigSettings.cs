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
using System.IO;

namespace TransferDataUtilityApp
{
    public partial class BusyConfigSettings : Form
    {
        OleDbConnection Conn;
        OleDbCommand Cmd;
        string connectionstr;
        string accsfile = "";

        public BusyConfigSettings()
        {
            InitializeComponent();
            connectionstr = ConfigurationManager.ConnectionStrings["AccessString"].ConnectionString;
            Conn = new OleDbConnection(connectionstr);
            //MessageBox.Show(textBox1.Text);
            comboBox1.SelectedIndex = 0;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        //public static string After(this string value, string a)
        //{
        //    int posA = value.LastIndexOf(a);
        //    if (posA == -1)
        //    {
        //        return "";
        //    }
        //    int adjustedPosA = posA + a.Length;
        //    if (adjustedPosA >= value.Length)
        //    {
        //        return "";
        //    }
        //    return value.Substring(adjustedPosA);
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                bool valid = false;
                string updatequery = "";
                string insertquery = "";
                //if (textBox9.Text == String.Empty)
                //{
                //    textBox9.Text = "0";
                //}
                int selectedvalue = comboBox1.SelectedIndex;
                if (selectedvalue >= 0)
                {
                    if (selectedvalue == 0)
                    {
                        string cleaned = textBox2.Text.Replace("\n", "").Replace("\r", "");

                        if (textBox1.Text == "")
                        {
                            System.Windows.Forms.MessageBox.Show("Please Enter Database Path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (textBox2.Text == "")
                        {
                            System.Windows.Forms.MessageBox.Show("Please Enter Database File Name 1.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        //else if (textBox3.Text == "")
                        //{
                        //    System.Windows.Forms.MessageBox.Show("Please Enter Database File Name 2.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //}
                        else if (textBox4.Text == "")
                        {
                            System.Windows.Forms.MessageBox.Show("Please Enter Database Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        else
                        {
                            valid = true;
                            updatequery = "Update Enviro set Source='Busy',Transferfor='" + comboBox2.SelectedItem.ToString() + "',BusyDatabase='MS ACCESS',Busy_AccessDbPath='" + textBox1.Text + "',Busy_AccessFileName1='" + cleaned + "',Busy_AccessDbPassword='" + textBox4.Text + "',DestinationServer='" + textBox6.Text + "',DistributorID=" + Convert.ToInt32(textBox10.Text.ToString()) + "";
                            insertquery = "insert into Enviro (Source,Transferfor,BusyDatabase,Busy_AccessDbPath,Busy_AccessFileName1,Busy_AccessDbPassword,DestinationServer,DistributorID) values ('Busy','" + comboBox2.SelectedItem.ToString() + "','MS ACCESS','" + textBox1.Text + "','" + textBox2.Text + "','" + textBox4.Text + "','" + textBox6.Text + "'," + Convert.ToInt32(textBox10.Text.ToString()) + ")";

                        }
                    }
                    else if (selectedvalue == 1)
                    {
                        if (textBox1.Text == "")
                        {
                            MessageBox.Show("Please Enter Server IP.");
                            System.Windows.Forms.MessageBox.Show("Please Enter Database Path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (textBox2.Text == "")
                        {
                            System.Windows.Forms.MessageBox.Show("Please Enter Username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (textBox3.Text == "")
                        {
                            System.Windows.Forms.MessageBox.Show("Please Enter Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (textBox4.Text == "")
                        {
                            System.Windows.Forms.MessageBox.Show("Please Enter Database Name 1.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (textBox5.Text == "")
                        {
                            System.Windows.Forms.MessageBox.Show("Please Enter Database Name 2.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            valid = true;
                            updatequery = "Update Enviro set Source='Busy',Transferfor='" + comboBox2.SelectedItem.ToString() + "',BusyDatabase='MS SQL',Busy_SQLServerIP='" + textBox1.Text + "',Busy_SQLUsername='" + textBox2.Text + "',Busy_SQLPassword='" + textBox3.Text + "',Busy_SQLDbName1='" + textBox4.Text + "',Busy_SQLDbName2='" + textBox5.Text + "',DestinationServer='" + textBox6.Text + "',DistributorID=" + Convert.ToInt32(textBox10.Text.ToString()) + "";
                            insertquery = "insert into Enviro (Source,Transferfor,BusyDatabase,Busy_SQLServerIP,Busy_SQLUsername,Busy_SQLPassword,Busy_SQLDbName1,Busy_SQLDbName2,DestinationServer,DistributorID) values ('Busy','" + comboBox2.SelectedItem.ToString() + "','MS SQL','" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "'," + Convert.ToInt32(textBox10.Text.ToString()) + ")";

                        }
                    }

                    if (valid)
                    {
                        if (textBox6.Text == "")
                        {
                            System.Windows.Forms.MessageBox.Show("Please Enter Destination Server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        //else if (textBox7.Text == "")
                        //{
                        //    System.Windows.Forms.MessageBox.Show("Please Enter Destination Database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //}
                        //else if (textBox8.Text == "")
                        //{
                        //    System.Windows.Forms.MessageBox.Show("Please Enter Destination User.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //}
                        //else if (textBox9.Text == "")
                        //{
                        //    System.Windows.Forms.MessageBox.Show("Please Enter Destination Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //}
                        else if (textBox10.Text == "")
                        {
                            System.Windows.Forms.MessageBox.Show("Please Enter DistributorID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (comboBox2.SelectedIndex < 0)
                        {
                            System.Windows.Forms.MessageBox.Show("Please Select Transfer For.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
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

                            Conn.Close();
                            if (result == DialogResult.OK)
                            {
                                this.Hide();
                            }
                        }
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please Select the Database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                System.Windows.Forms.MessageBox.Show("Some Error Occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ClearControls()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            //textB5.Text = "";
            textBox6.Text = "";
            //textBox7.Text = "";
            //textBox8.Text = "";
            //textBox9.Text = "";
            textBox10.Text = "";
        }
        private void BusyConfigSettings_Load(object sender, EventArgs e)
        {
            string s = null;
            if (Start.db != "")
            {
                comboBox1.Text = Start.db.ToString();
                textBox1.Text = Start.dbpath.ToString();
                textBox4.Text = Start.dbpass.ToString();
                textBox6.Text = Start.dest.ToString();
                //textBox7.Text = Start.destdb.ToString();
                //textBox8.Text = Start.usr.ToString();
                //textBox9.Text = Start.pass.ToString();
                textBox10.Text = Start.id.ToString();
                comboBox2.Text = Start.trndfr.ToString();
                //MessageBox.Show(Start.accfle.ToString());
                string[] authorsList = Start.accfle.ToString().Split(',');
                for (int i = 0; i < authorsList.Length; i++)
                {
                    if (s == null)
                    {
                        s = authorsList[i].ToString();
                    }
                    else
                    {
                        s = s + ",\n" + authorsList[i].ToString(); ;
                    }
                }
                textBox2.Text = s.ToString();
            }
            else { ClearControls(); }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedvalue = comboBox1.SelectedIndex;
            if (selectedvalue >= 0)
            {
                if (selectedvalue == 0)
                {
                    label3.Text = "Enter Database Path";
                    label4.Text = "Enter Database File Name 1";
                    label5.Text = "Enter Database File Name 2";
                    label6.Text = "Enter Database Password";
                    label14.Visible = false;
                    textBox5.Visible = false;

                    ClearControls();
                }
                else if (selectedvalue == 1)
                {
                    label3.Text = "Enter Server IP";
                    label4.Text = "Enter Username";
                    label5.Text = "Enter Password";
                    label6.Text = "Enter Database Name 1";
                    label14.Visible = true;
                    textBox5.Visible = true;

                    ClearControls();

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            //if (textBox9.Text == String.Empty)
            //{
            //    textBox9.Text = "0";
            //}
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(accsfile.ToString());
            if (textBox1.Text != String.Empty)
            {
                if (accsfile == "")
                {
                    int selectedvalue = comboBox1.SelectedIndex;
                    if (selectedvalue >= 0)
                    {
                        if (selectedvalue == 0)
                        {
                            string s = null;
                            string d = "_Backup";
                            //string[] fileArray = Directory.GetFiles(@"\");
                            //string[] fileArray = Directory.GetFiles(textBox1.Text + @"\", "*.bds");
                            string[] fileArray = Directory.GetFiles(textBox1.Text + @"\", "db*.bds", SearchOption.AllDirectories);

                            for (int i = 0; i < fileArray.Length; i++)
                            {
                                string[] authorsList = fileArray[i].ToString().Split('\\');

                                if ((authorsList[authorsList.Length - 1].ToString().Trim() != "db.bds") && !authorsList[authorsList.Length - 1].Contains(d))
                                {
                                    if (s == null)
                                    {
                                        s = authorsList[authorsList.Length - 2].ToString() + "\\" + authorsList[authorsList.Length - 1].ToString();
                                    }
                                    else
                                    {
                                        s = s + ",\n" + authorsList[authorsList.Length - 2].ToString() + "\\" + authorsList[authorsList.Length - 1].ToString();
                                    }
                                }
                            }

                            textBox2.Text = s.ToString();
                            textBox3.ReadOnly = true;
                        }
                    }
                }
                else
                {
                    textBox2.Text = accsfile.ToString();
                    textBox3.ReadOnly = true;
                }
            }
            else
            {
                textBox2.Text = String.Empty;
                textBox3.ReadOnly = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != String.Empty)
            {
                int selectedvalue = comboBox1.SelectedIndex;
                if (selectedvalue >= 0)
                {
                    if (selectedvalue == 0)
                    {
                        string s = null;
                        string d = "_Backup";
                        //string[] fileArray = Directory.GetFiles(@"\");
                        //string[] fileArray = Directory.GetFiles(textBox1.Text + @"\", "*.bds");
                        string[] fileArray = Directory.GetFiles(textBox1.Text + @"\", "db*.bds", SearchOption.AllDirectories);

                        for (int i = 0; i < fileArray.Length; i++)
                        {
                            string[] authorsList = fileArray[i].ToString().Split('\\');

                            if ((authorsList[authorsList.Length - 1].ToString().Trim() != "db.bds") && !authorsList[authorsList.Length - 1].Contains(d))
                            {
                                if (s == null)
                                {
                                    s = authorsList[authorsList.Length - 2].ToString() + "\\" + authorsList[authorsList.Length - 1].ToString();
                                }
                                else
                                {
                                    s = s + ",\n" + authorsList[authorsList.Length - 2].ToString() + "\\" + authorsList[authorsList.Length - 1].ToString();
                                }
                            }
                        }

                        textBox2.Text = s.ToString();
                        textBox3.ReadOnly = true;
                    }
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please Select the Database Path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox10.Text == String.Empty)
            {
                textBox10.Text = "0";
            }
        }
    }
}
