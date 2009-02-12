namespace ArcToSQL2008
{
    partial class ProgressDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._btnCancel = new System.Windows.Forms.Button();
            this._txtErrors = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._lblInfo = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnCancel
            // 
            this._btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._btnCancel.Location = new System.Drawing.Point( 373, 230 );
            this._btnCancel.Margin = new System.Windows.Forms.Padding( 6 );
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size( 135, 28 );
            this._btnCancel.TabIndex = 0;
            this._btnCancel.Text = "Cancel Export";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler( this.btn_Cancel_Click );
            // 
            // _txtErrors
            // 
            this._txtErrors.BackColor = System.Drawing.Color.FromArgb( ( ( int ) ( ( ( byte ) ( 255 ) ) ) ), ( ( int ) ( ( ( byte ) ( 255 ) ) ) ), ( ( int ) ( ( ( byte ) ( 192 ) ) ) ) );
            this.tableLayoutPanel1.SetColumnSpan( this._txtErrors, 2 );
            this._txtErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtErrors.Location = new System.Drawing.Point( 6, 71 );
            this._txtErrors.Margin = new System.Windows.Forms.Padding( 6 );
            this._txtErrors.Multiline = true;
            this._txtErrors.Name = "_txtErrors";
            this._txtErrors.ReadOnly = true;
            this._txtErrors.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtErrors.Size = new System.Drawing.Size( 502, 147 );
            this._txtErrors.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 71.59533F ) );
            this.tableLayoutPanel1.ColumnStyles.Add( new System.Windows.Forms.ColumnStyle( System.Windows.Forms.SizeType.Percent, 28.40467F ) );
            this.tableLayoutPanel1.Controls.Add( this._btnCancel, 1, 2 );
            this.tableLayoutPanel1.Controls.Add( this._txtErrors, 0, 1 );
            this.tableLayoutPanel1.Controls.Add( this._lblInfo, 0, 0 );
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point( 0, 0 );
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Absolute, 65F ) );
            this.tableLayoutPanel1.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Percent, 100F ) );
            this.tableLayoutPanel1.RowStyles.Add( new System.Windows.Forms.RowStyle( System.Windows.Forms.SizeType.Absolute, 40F ) );
            this.tableLayoutPanel1.Size = new System.Drawing.Size( 514, 264 );
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // _lblInfo
            // 
            this._lblInfo.AutoSize = true;
            this._lblInfo.BackColor = System.Drawing.Color.Silver;
            this.tableLayoutPanel1.SetColumnSpan( this._lblInfo, 2 );
            this._lblInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this._lblInfo.Location = new System.Drawing.Point( 6, 3 );
            this._lblInfo.Margin = new System.Windows.Forms.Padding( 6, 3, 6, 3 );
            this._lblInfo.Name = "_lblInfo";
            this._lblInfo.Size = new System.Drawing.Size( 502, 59 );
            this._lblInfo.TabIndex = 2;
            this._lblInfo.Text = "Importing to SQL 2008";
            this._lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProgressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 514, 264 );
            this.Controls.Add( this.tableLayoutPanel1 );
            this.Name = "ProgressDialog";
            this.Text = "ProgressDialog";
            this.tableLayoutPanel1.ResumeLayout( false );
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button _btnCancel;
        private System.Windows.Forms.TextBox _txtErrors;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label _lblInfo;
    }
}