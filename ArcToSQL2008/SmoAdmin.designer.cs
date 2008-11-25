namespace ArcToSQL2008
{
    partial class SmoAdmin
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkUseWindowsAuthentication = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSQLServerName = new System.Windows.Forms.TextBox();
            this.grpDatabase = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboDatabase = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btn_OK = new System.Windows.Forms.Button();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.grpDatabase.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.chkUseWindowsAuthentication );
            this.groupBox1.Controls.Add( this.label3 );
            this.groupBox1.Controls.Add( this.txtPassword );
            this.groupBox1.Controls.Add( this.label2 );
            this.groupBox1.Controls.Add( this.txtUserName );
            this.groupBox1.Controls.Add( this.btnConnect );
            this.groupBox1.Controls.Add( this.label1 );
            this.groupBox1.Controls.Add( this.txtSQLServerName );
            this.groupBox1.Location = new System.Drawing.Point( 12, 12 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 413, 131 );
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // chkUseWindowsAuthentication
            // 
            this.chkUseWindowsAuthentication.AutoSize = true;
            this.chkUseWindowsAuthentication.Location = new System.Drawing.Point( 114, 45 );
            this.chkUseWindowsAuthentication.Name = "chkUseWindowsAuthentication";
            this.chkUseWindowsAuthentication.Size = new System.Drawing.Size( 166, 17 );
            this.chkUseWindowsAuthentication.TabIndex = 2;
            this.chkUseWindowsAuthentication.Text = "Use Windows Authentication:";
            this.chkUseWindowsAuthentication.UseVisualStyleBackColor = true;
            this.chkUseWindowsAuthentication.CheckedChanged += new System.EventHandler( this.chkUseWindowsAuthentication_CheckedChanged );
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point( 12, 101 );
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size( 56, 13 );
            this.label3.TabIndex = 5;
            this.label3.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point( 114, 94 );
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size( 184, 20 );
            this.txtPassword.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point( 12, 75 );
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size( 63, 13 );
            this.label2.TabIndex = 3;
            this.label2.Text = "User Name:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point( 114, 68 );
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size( 184, 20 );
            this.txtUserName.TabIndex = 4;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point( 323, 92 );
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size( 75, 23 );
            this.btnConnect.TabIndex = 7;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler( this.btnConnect_Click );
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 12, 24 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 96, 13 );
            this.label1.TabIndex = 0;
            this.label1.Text = "SQL Server Name:";
            // 
            // txtSQLServerName
            // 
            this.txtSQLServerName.Location = new System.Drawing.Point( 114, 19 );
            this.txtSQLServerName.Name = "txtSQLServerName";
            this.txtSQLServerName.Size = new System.Drawing.Size( 184, 20 );
            this.txtSQLServerName.TabIndex = 1;
            // 
            // grpDatabase
            // 
            this.grpDatabase.Controls.Add( this.label5 );
            this.grpDatabase.Controls.Add( this.txtTableName );
            this.grpDatabase.Controls.Add( this.label4 );
            this.grpDatabase.Controls.Add( this.cboDatabase );
            this.grpDatabase.Location = new System.Drawing.Point( 13, 150 );
            this.grpDatabase.Name = "grpDatabase";
            this.grpDatabase.Size = new System.Drawing.Size( 412, 85 );
            this.grpDatabase.TabIndex = 2;
            this.grpDatabase.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point( 11, 22 );
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size( 56, 13 );
            this.label4.TabIndex = 0;
            this.label4.Text = "Database:";
            // 
            // cboDatabase
            // 
            this.cboDatabase.DisplayMember = "Name";
            this.cboDatabase.FormattingEnabled = true;
            this.cboDatabase.Location = new System.Drawing.Point( 113, 19 );
            this.cboDatabase.Name = "cboDatabase";
            this.cboDatabase.Size = new System.Drawing.Size( 284, 21 );
            this.cboDatabase.TabIndex = 1;
            this.cboDatabase.ValueMember = "Name";
            this.cboDatabase.SelectedIndexChanged += new System.EventHandler( this.cboDatabase_SelectedIndexChanged );
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point( 335, 256 );
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size( 75, 23 );
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler( this.btnClose_Click );
            // 
            // btn_OK
            // 
            this.btn_OK.Enabled = false;
            this.btn_OK.Location = new System.Drawing.Point( 235, 256 );
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size( 75, 23 );
            this.btn_OK.TabIndex = 10;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler( this.btn_OK_Click );
            // 
            // txtTableName
            // 
            this.txtTableName.Location = new System.Drawing.Point( 113, 53 );
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size( 284, 20 );
            this.txtTableName.TabIndex = 2;
            this.txtTableName.TextChanged += new System.EventHandler( this.txtTableName_TextChanged );
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point( 14, 53 );
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size( 79, 13 );
            this.label5.TabIndex = 3;
            this.label5.Text = "Export to Table";
            // 
            // SmoAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 436, 292 );
            this.Controls.Add( this.btn_OK );
            this.Controls.Add( this.btnClose );
            this.Controls.Add( this.grpDatabase );
            this.Controls.Add( this.groupBox1 );
            this.Name = "SmoAdmin";
            this.Text = "SmoAdmin";
            this.Load += new System.EventHandler( this.SmoAdmin_Load );
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            this.grpDatabase.ResumeLayout( false );
            this.grpDatabase.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkUseWindowsAuthentication;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSQLServerName;
        private System.Windows.Forms.GroupBox grpDatabase;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboDatabase;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTableName;
    }
}