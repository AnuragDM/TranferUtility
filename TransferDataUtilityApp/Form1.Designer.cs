namespace TransferDataUtilityApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtserver = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtport = new System.Windows.Forms.TextBox();
            this.btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdtallyversion = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdtransferfor = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtdestpassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtdestuser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtdestidatabase = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtdestserver = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtserver
            // 
            this.txtserver.Location = new System.Drawing.Point(249, 49);
            this.txtserver.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtserver.MaxLength = 200;
            this.txtserver.Name = "txtserver";
            this.txtserver.Size = new System.Drawing.Size(180, 22);
            this.txtserver.TabIndex = 2;
            this.txtserver.TextChanged += new System.EventHandler(this.txtserver_TextChanged);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServer.Location = new System.Drawing.Point(56, 49);
            this.lblServer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(54, 16);
            this.lblServer.TabIndex = 3;
            this.lblServer.Text = "Server";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(56, 84);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Port";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtport
            // 
            this.txtport.Location = new System.Drawing.Point(249, 81);
            this.txtport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtport.MaxLength = 6;
            this.txtport.Name = "txtport";
            this.txtport.Size = new System.Drawing.Size(180, 22);
            this.txtport.TabIndex = 4;
            this.txtport.TextChanged += new System.EventHandler(this.txtport_TextChanged);
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(59, 392);
            this.btn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(112, 28);
            this.btn.TabIndex = 6;
            this.btn.Text = "Save";
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(56, 117);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Tally Version";
            // 
            // cmdtallyversion
            // 
            this.cmdtallyversion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmdtallyversion.FormattingEnabled = true;
            this.cmdtallyversion.Items.AddRange(new object[] {
            "32-bit",
            "64-bit"});
            this.cmdtallyversion.Location = new System.Drawing.Point(249, 114);
            this.cmdtallyversion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmdtallyversion.Name = "cmdtallyversion";
            this.cmdtallyversion.Size = new System.Drawing.Size(180, 24);
            this.cmdtallyversion.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(56, 150);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Transfer For";
            // 
            // cmdtransferfor
            // 
            this.cmdtransferfor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmdtransferfor.FormattingEnabled = true;
            this.cmdtransferfor.Items.AddRange(new object[] {
            "Super Stockist",
            "Distributor"});
            this.cmdtransferfor.Location = new System.Drawing.Point(249, 146);
            this.cmdtransferfor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmdtransferfor.Name = "cmdtransferfor";
            this.cmdtransferfor.Size = new System.Drawing.Size(180, 24);
            this.cmdtransferfor.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(56, 304);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 16);
            this.label8.TabIndex = 25;
            this.label8.Text = "DistributorID";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(249, 304);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.MaxLength = 5;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(180, 22);
            this.textBox1.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(56, 274);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 16);
            this.label6.TabIndex = 23;
            this.label6.Text = "Destination Password";
            // 
            // txtdestpassword
            // 
            this.txtdestpassword.Location = new System.Drawing.Point(249, 274);
            this.txtdestpassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtdestpassword.MaxLength = 30;
            this.txtdestpassword.Name = "txtdestpassword";
            this.txtdestpassword.Size = new System.Drawing.Size(180, 22);
            this.txtdestpassword.TabIndex = 22;
            this.txtdestpassword.UseSystemPasswordChar = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(56, 242);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 16);
            this.label7.TabIndex = 21;
            this.label7.Text = "Destination User";
            // 
            // txtdestuser
            // 
            this.txtdestuser.Location = new System.Drawing.Point(249, 242);
            this.txtdestuser.Margin = new System.Windows.Forms.Padding(4);
            this.txtdestuser.MaxLength = 100;
            this.txtdestuser.Name = "txtdestuser";
            this.txtdestuser.Size = new System.Drawing.Size(180, 22);
            this.txtdestuser.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(56, 212);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(158, 16);
            this.label4.TabIndex = 19;
            this.label4.Text = "Destination Database";
            // 
            // txtdestidatabase
            // 
            this.txtdestidatabase.Location = new System.Drawing.Point(249, 212);
            this.txtdestidatabase.Margin = new System.Windows.Forms.Padding(4);
            this.txtdestidatabase.MaxLength = 50;
            this.txtdestidatabase.Name = "txtdestidatabase";
            this.txtdestidatabase.Size = new System.Drawing.Size(180, 22);
            this.txtdestidatabase.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(56, 180);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 16);
            this.label5.TabIndex = 17;
            this.label5.Text = "Destination Server";
            // 
            // txtdestserver
            // 
            this.txtdestserver.Location = new System.Drawing.Point(249, 180);
            this.txtdestserver.Margin = new System.Windows.Forms.Padding(4);
            this.txtdestserver.MaxLength = 200;
            this.txtdestserver.Name = "txtdestserver";
            this.txtdestserver.Size = new System.Drawing.Size(180, 22);
            this.txtdestserver.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 447);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtdestpassword);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtdestuser);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtdestidatabase);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtdestserver);
            this.Controls.Add(this.cmdtransferfor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmdtallyversion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtport);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.txtserver);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtserver;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtport;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmdtallyversion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmdtransferfor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtdestpassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtdestuser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtdestidatabase;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtdestserver;
    }
}

