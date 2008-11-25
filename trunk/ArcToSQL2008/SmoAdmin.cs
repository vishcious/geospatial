using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace ArcToSQL2008
{
    public partial class SmoAdmin : Form
    {
        public SmoAdmin()
        {
            InitializeComponent();
        }

        #region Server Stuff

        private SMOHelper smoHelper = null;
        public string ConnectionString
        {
            get;
            set;
        }
        public string TableName
        {
            get;
            set;
        }

        private void SmoAdmin_Load(object sender, EventArgs e)
        {

            grpDatabase.Enabled = false;
            txtSQLServerName.Text = "";
            chkUseWindowsAuthentication.Checked = true;
            txtSQLServerName.Focus();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            grpDatabase.Enabled = false;
            if (txtSQLServerName.Text.Length == 0)
                return;
            if (!chkUseWindowsAuthentication.Checked && txtUserName.Text.Length == 0 && txtPassword.Text.Length == 0)
                return;

            smoHelper = new SMOHelper(txtSQLServerName.Text, txtUserName.Text, txtPassword.Text, chkUseWindowsAuthentication.Checked);
            smoHelper.Connect();
            if (smoHelper.Server != null)
            {
                cboDatabase.DataSource = smoHelper.GetDatabaseNameList();
                cboDatabase.SelectedIndex = -1;
                cboDatabase.Focus();
                grpDatabase.Enabled = true;
            }
        }

        private void chkUseWindowsAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            txtUserName.Enabled = !chkUseWindowsAuthentication.Checked;
            txtPassword.Enabled = !chkUseWindowsAuthentication.Checked;
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_OK_Click( object sender, EventArgs e )
        {
            if( cboDatabase.SelectedIndex >= 0 )
            {
                Database db = smoHelper.Server.Databases[ cboDatabase.SelectedValue.ToString() ];
                ConnectionString = smoHelper.Server.ConnectionContext.ConnectionString + ";Initial Catalog=" + db.Name;
                TableName = txtTableName.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("A database must be selected to export to.", "Information", MessageBoxButtons.OK);
            }
        }

        private void cboDatabase_SelectedIndexChanged( object sender, EventArgs e )
        {
            UpdateButtonStatus();
        }

        private void txtTableName_TextChanged( object sender, EventArgs e )
        {
            UpdateButtonStatus();
        }

        private void UpdateButtonStatus()
        {
            if( cboDatabase.SelectedIndex != -1 && !(string.IsNullOrEmpty(txtTableName.Text)))
            {
                btn_OK.Enabled = true;
            }
            else
            {
                btn_OK.Enabled = false;
            }
        }
    }
}