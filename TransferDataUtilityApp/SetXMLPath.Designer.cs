namespace TransferDataUtilityApp
{
    partial class MargConfig
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtdist = new System.Windows.Forms.TextBox();
            this.txtdestinationpassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtdestinationuser = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtdestinationdatabase = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtdestinationserver = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(161, 30);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(167, 20);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(243, 213);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 28);
            this.button1.TabIndex = 3;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "XML Path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 162);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Enter Distributor ID";
            // 
            // txtdist
            // 
            this.txtdist.Location = new System.Drawing.Point(161, 159);
            this.txtdist.Name = "txtdist";
            this.txtdist.Size = new System.Drawing.Size(167, 20);
            this.txtdist.TabIndex = 23;
            // 
            // txtdestinationpassword
            // 
            this.txtdestinationpassword.Location = new System.Drawing.Point(161, 134);
            this.txtdestinationpassword.Name = "txtdestinationpassword";
            this.txtdestinationpassword.Size = new System.Drawing.Size(168, 20);
            this.txtdestinationpassword.TabIndex = 31;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 137);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(130, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Enter Database Password";
            // 
            // txtdestinationuser
            // 
            this.txtdestinationuser.Location = new System.Drawing.Point(161, 108);
            this.txtdestinationuser.Name = "txtdestinationuser";
            this.txtdestinationuser.Size = new System.Drawing.Size(167, 20);
            this.txtdestinationuser.TabIndex = 29;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Enter Destination User";
            // 
            // txtdestinationdatabase
            // 
            this.txtdestinationdatabase.Location = new System.Drawing.Point(161, 82);
            this.txtdestinationdatabase.Name = "txtdestinationdatabase";
            this.txtdestinationdatabase.Size = new System.Drawing.Size(167, 20);
            this.txtdestinationdatabase.TabIndex = 27;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 85);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Enter Destination Database";
            // 
            // txtdestinationserver
            // 
            this.txtdestinationserver.Location = new System.Drawing.Point(161, 56);
            this.txtdestinationserver.Name = "txtdestinationserver";
            this.txtdestinationserver.Size = new System.Drawing.Size(167, 20);
            this.txtdestinationserver.TabIndex = 25;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 59);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(122, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Enter Destination Server";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "DISTRIBUTOR",
            "SUPER STOCKIST"});
            this.comboBox2.Location = new System.Drawing.Point(162, 186);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(167, 21);
            this.comboBox2.TabIndex = 33;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(19, 189);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 13);
            this.label13.TabIndex = 32;
            this.label13.Text = "Transfer For";
            // 
            // MargConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 255);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtdestinationpassword);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtdestinationuser);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtdestinationdatabase);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtdestinationserver);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtdist);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "MargConfig";
            this.Text = "Marg configuration ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtdist;
        private System.Windows.Forms.TextBox txtdestinationpassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtdestinationuser;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtdestinationdatabase;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtdestinationserver;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label13;
    }
}