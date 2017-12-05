namespace Server
{
    partial class ServerForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelSend = new System.Windows.Forms.Label();
            this.textSend = new System.Windows.Forms.TextBox();
            this.labelAddress = new System.Windows.Forms.Label();
            this.textStatus = new System.Windows.Forms.TextBox();
            this.textAddress = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.textPort = new System.Windows.Forms.TextBox();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.NickName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 6;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel.Controls.Add(this.buttonConnect, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.labelSend, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.textSend, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.labelAddress, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textStatus, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.textAddress, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelPort, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.textPort, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonDisconnect, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonSend, 4, 2);
            this.tableLayoutPanel.Controls.Add(this.dataGridView, 4, 1);
            this.tableLayoutPanel.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(760, 537);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Font = new System.Drawing.Font("굴림", 12F);
            this.buttonConnect.Location = new System.Drawing.Point(563, 3);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(94, 29);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "시작";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // labelSend
            // 
            this.labelSend.Font = new System.Drawing.Font("굴림", 12F);
            this.labelSend.Location = new System.Drawing.Point(3, 502);
            this.labelSend.Name = "labelSend";
            this.labelSend.Size = new System.Drawing.Size(94, 35);
            this.labelSend.TabIndex = 6;
            this.labelSend.Text = "입 력";
            this.labelSend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textSend
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textSend, 3);
            this.textSend.Font = new System.Drawing.Font("굴림", 12F);
            this.textSend.Location = new System.Drawing.Point(103, 505);
            this.textSend.Name = "textSend";
            this.textSend.Size = new System.Drawing.Size(454, 26);
            this.textSend.TabIndex = 7;
            this.textSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textSend_KeyDown);
            // 
            // labelAddress
            // 
            this.labelAddress.Font = new System.Drawing.Font("굴림", 12F);
            this.labelAddress.Location = new System.Drawing.Point(3, 0);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(94, 33);
            this.labelAddress.TabIndex = 1;
            this.labelAddress.Text = "서버 주소";
            this.labelAddress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textStatus
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textStatus, 4);
            this.textStatus.Location = new System.Drawing.Point(3, 38);
            this.textStatus.Multiline = true;
            this.textStatus.Name = "textStatus";
            this.textStatus.ReadOnly = true;
            this.textStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textStatus.Size = new System.Drawing.Size(554, 461);
            this.textStatus.TabIndex = 9;
            // 
            // textAddress
            // 
            this.textAddress.Font = new System.Drawing.Font("굴림", 12F);
            this.textAddress.Location = new System.Drawing.Point(103, 3);
            this.textAddress.Name = "textAddress";
            this.textAddress.Size = new System.Drawing.Size(246, 26);
            this.textAddress.TabIndex = 3;
            // 
            // labelPort
            // 
            this.labelPort.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelPort.Location = new System.Drawing.Point(355, 0);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(94, 33);
            this.labelPort.TabIndex = 2;
            this.labelPort.Text = "포트 번호";
            this.labelPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textPort
            // 
            this.textPort.Font = new System.Drawing.Font("굴림", 12F);
            this.textPort.Location = new System.Drawing.Point(455, 3);
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(102, 26);
            this.textPort.TabIndex = 4;
            this.textPort.Text = "8080";
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Font = new System.Drawing.Font("굴림", 12F);
            this.buttonDisconnect.Location = new System.Drawing.Point(663, 3);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(94, 29);
            this.buttonDisconnect.TabIndex = 10;
            this.buttonDisconnect.Text = "종료";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // buttonSend
            // 
            this.tableLayoutPanel.SetColumnSpan(this.buttonSend, 2);
            this.buttonSend.Font = new System.Drawing.Font("굴림", 12F);
            this.buttonSend.Location = new System.Drawing.Point(563, 505);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(194, 29);
            this.buttonSend.TabIndex = 8;
            this.buttonSend.Text = "보내기";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NickName});
            this.tableLayoutPanel.SetColumnSpan(this.dataGridView, 2);
            this.dataGridView.Location = new System.Drawing.Point(563, 38);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(194, 461);
            this.dataGridView.TabIndex = 11;
            // 
            // NickName
            // 
            this.NickName.HeaderText = "닉네임";
            this.NickName.Name = "NickName";
            this.NickName.ReadOnly = true;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tableLayoutPanel);
            this.MaximizeBox = false;
            this.Name = "ServerForm";
            this.Text = "Chatting by inseoul - Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label labelSend;
        private System.Windows.Forms.TextBox textSend;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Label labelAddress;
        private System.Windows.Forms.TextBox textStatus;
        private System.Windows.Forms.TextBox textAddress;
        private System.Windows.Forms.TextBox textPort;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn NickName;
    }
}

