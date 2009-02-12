using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArcToSQL2008
{
    public partial class ProgressDialog : Form
    {
        BackgroundWorker _backgroundWorker;

        public ProgressDialog()
        {
            InitializeComponent();
        }

        public ProgressDialog(BackgroundWorker backgroundWorker)
        {
            InitializeComponent();
            _backgroundWorker = backgroundWorker;
        }

        private void btn_Cancel_Click( object sender, EventArgs e )
        {
            if( _backgroundWorker != null )
            {
                _backgroundWorker.CancelAsync();
                MessageBox.Show( "Export operation cancelled before completion.", "Info" );
            }
        }

        public void SetInfo( string info )
        {
            _lblInfo.Text = info;
        }

        public void AddError( string error )
        {
            _txtErrors.Text = _txtErrors + System.Environment.NewLine + error;
        }

        public void ClearErrors()
        {
            _txtErrors.Text = string.Empty;
        }
    }
}
