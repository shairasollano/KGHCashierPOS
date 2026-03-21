namespace KGHCashierPOS
{
    partial class OrderForm
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblUser = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnPayCashier = new System.Windows.Forms.Button();
            this.lbDisplay = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ordernum = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblSelect = new System.Windows.Forms.Label();
            this.btnBilliards = new System.Windows.Forms.Button();
            this.btnTableTennis = new System.Windows.Forms.Button();
            this.btnScooter = new System.Windows.Forms.Button();
            this.btnBadminton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkExtend = new System.Windows.Forms.CheckBox();
            this.btn30min = new System.Windows.Forms.Button();
            this.btn1hour = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblOrderNum = new System.Windows.Forms.Label();
            this.timerDateTime1 = new System.Windows.Forms.Timer(this.components);
            this.btnClear = new System.Windows.Forms.Button();
            this.equipmentRentalControl1 = new KGHCashierPOS.EquipmentRentalControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(35)))));
            this.panel1.Controls.Add(this.lblUser);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblDate);
            this.panel1.Controls.Add(this.lblTime);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(-8, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1271, 71);
            this.panel1.TabIndex = 1;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.ForeColor = System.Drawing.SystemColors.Control;
            this.lblUser.Location = new System.Drawing.Point(1083, 47);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(58, 20);
            this.lblUser.TabIndex = 35;
            this.lblUser.Text = "lblUser";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nirmala Text", 8F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(967, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 21);
            this.label2.TabIndex = 7;
            this.label2.Text = "Logged in as: ";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Nirmala Text", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.SystemColors.Control;
            this.lblDate.Location = new System.Drawing.Point(73, 10);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(54, 28);
            this.lblDate.TabIndex = 6;
            this.lblDate.Text = "date";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Nirmala Text", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTime.Location = new System.Drawing.Point(73, 34);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(55, 28);
            this.lblTime.TabIndex = 5;
            this.lblTime.Text = "time";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(9, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(59, 63);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Font = new System.Drawing.Font("Nirmala Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalValue.Location = new System.Drawing.Point(934, 575);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(120, 32);
            this.lblTotalValue.TabIndex = 44;
            this.lblTotalValue.Text = "totalValue";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Nirmala Text", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(838, 575);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 32);
            this.label6.TabIndex = 43;
            this.label6.Text = "TOTAL: ";
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.Color.Red;
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRemove.Location = new System.Drawing.Point(440, 497);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(159, 61);
            this.btnRemove.TabIndex = 41;
            this.btnRemove.Text = "REMOVE SELECTED";
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnPayCashier
            // 
            this.btnPayCashier.BackColor = System.Drawing.Color.LimeGreen;
            this.btnPayCashier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayCashier.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPayCashier.Location = new System.Drawing.Point(844, 625);
            this.btnPayCashier.Name = "btnPayCashier";
            this.btnPayCashier.Size = new System.Drawing.Size(362, 51);
            this.btnPayCashier.TabIndex = 40;
            this.btnPayCashier.Text = "PAY TO THE CASHIER";
            this.btnPayCashier.UseVisualStyleBackColor = false;
            this.btnPayCashier.Click += new System.EventHandler(this.btnPayCashier_Click);
            // 
            // lbDisplay
            // 
            this.lbDisplay.FormattingEnabled = true;
            this.lbDisplay.ItemHeight = 20;
            this.lbDisplay.Location = new System.Drawing.Point(835, 128);
            this.lbDisplay.Name = "lbDisplay";
            this.lbDisplay.Size = new System.Drawing.Size(371, 444);
            this.lbDisplay.TabIndex = 39;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Nirmala Text", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(829, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(202, 32);
            this.label5.TabIndex = 38;
            this.label5.Text = "GAME SELECTED";
            // 
            // ordernum
            // 
            this.ordernum.AutoSize = true;
            this.ordernum.Font = new System.Drawing.Font("Miriam Libre", 9.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ordernum.Location = new System.Drawing.Point(99, 103);
            this.ordernum.Name = "ordernum";
            this.ordernum.Size = new System.Drawing.Size(101, 26);
            this.ordernum.TabIndex = 37;
            this.ordernum.Text = "Order #: ";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(190)))), ((int)(((byte)(95)))));
            this.panel3.Controls.Add(this.lblSelect);
            this.panel3.Controls.Add(this.btnBilliards);
            this.panel3.Controls.Add(this.btnTableTennis);
            this.panel3.Controls.Add(this.btnScooter);
            this.panel3.Controls.Add(this.btnBadminton);
            this.panel3.Location = new System.Drawing.Point(57, 297);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(371, 349);
            this.panel3.TabIndex = 47;
            // 
            // lblSelect
            // 
            this.lblSelect.AutoSize = true;
            this.lblSelect.Font = new System.Drawing.Font("Nirmala Text", 9F, System.Drawing.FontStyle.Bold);
            this.lblSelect.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblSelect.Location = new System.Drawing.Point(19, 13);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(131, 25);
            this.lblSelect.TabIndex = 7;
            this.lblSelect.Text = "SELECT GAME";
            // 
            // btnBilliards
            // 
            this.btnBilliards.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(35)))));
            this.btnBilliards.BackgroundImage = global::KGHCashierPOS.Properties.Resources.BILLIARDS;
            this.btnBilliards.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBilliards.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnBilliards.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnBilliards.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnBilliards.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBilliards.Location = new System.Drawing.Point(188, 47);
            this.btnBilliards.Name = "btnBilliards";
            this.btnBilliards.Size = new System.Drawing.Size(151, 134);
            this.btnBilliards.TabIndex = 1;
            this.btnBilliards.UseVisualStyleBackColor = false;
            this.btnBilliards.Click += new System.EventHandler(this.btnBilliards_Click);
            // 
            // btnTableTennis
            // 
            this.btnTableTennis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(35)))));
            this.btnTableTennis.BackgroundImage = global::KGHCashierPOS.Properties.Resources.TABLETENNIS;
            this.btnTableTennis.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTableTennis.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnTableTennis.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnTableTennis.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnTableTennis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTableTennis.Location = new System.Drawing.Point(188, 187);
            this.btnTableTennis.Name = "btnTableTennis";
            this.btnTableTennis.Size = new System.Drawing.Size(151, 134);
            this.btnTableTennis.TabIndex = 2;
            this.btnTableTennis.UseVisualStyleBackColor = false;
            this.btnTableTennis.Click += new System.EventHandler(this.btnTableTennis_Click);
            // 
            // btnScooter
            // 
            this.btnScooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(35)))));
            this.btnScooter.BackgroundImage = global::KGHCashierPOS.Properties.Resources.SCOOTER;
            this.btnScooter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnScooter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnScooter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnScooter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnScooter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScooter.Location = new System.Drawing.Point(31, 187);
            this.btnScooter.Name = "btnScooter";
            this.btnScooter.Size = new System.Drawing.Size(151, 134);
            this.btnScooter.TabIndex = 6;
            this.btnScooter.UseVisualStyleBackColor = false;
            this.btnScooter.Click += new System.EventHandler(this.btnScooter_Click);
            // 
            // btnBadminton
            // 
            this.btnBadminton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(35)))));
            this.btnBadminton.BackgroundImage = global::KGHCashierPOS.Properties.Resources.BADMINTON;
            this.btnBadminton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBadminton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnBadminton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnBadminton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnBadminton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBadminton.Location = new System.Drawing.Point(31, 47);
            this.btnBadminton.Name = "btnBadminton";
            this.btnBadminton.Size = new System.Drawing.Size(151, 134);
            this.btnBadminton.TabIndex = 8;
            this.btnBadminton.UseVisualStyleBackColor = false;
            this.btnBadminton.Click += new System.EventHandler(this.btnBadminton_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(190)))), ((int)(((byte)(95)))));
            this.panel2.Controls.Add(this.chkExtend);
            this.panel2.Controls.Add(this.btn30min);
            this.panel2.Controls.Add(this.btn1hour);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(434, 297);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(330, 181);
            this.panel2.TabIndex = 11;
            // 
            // chkExtend
            // 
            this.chkExtend.AutoSize = true;
            this.chkExtend.Font = new System.Drawing.Font("Nirmala Text", 10F, System.Drawing.FontStyle.Bold);
            this.chkExtend.Location = new System.Drawing.Point(21, 162);
            this.chkExtend.Name = "chkExtend";
            this.chkExtend.Size = new System.Drawing.Size(179, 32);
            this.chkExtend.TabIndex = 21;
            this.chkExtend.Text = "Extend Session";
            this.chkExtend.UseVisualStyleBackColor = true;
            this.chkExtend.Visible = false;
            // 
            // btn30min
            // 
            this.btn30min.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(35)))));
            this.btn30min.BackgroundImage = global::KGHCashierPOS.Properties.Resources._30MINS;
            this.btn30min.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn30min.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btn30min.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btn30min.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btn30min.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn30min.Location = new System.Drawing.Point(19, 47);
            this.btn30min.Name = "btn30min";
            this.btn30min.Size = new System.Drawing.Size(140, 116);
            this.btn30min.TabIndex = 10;
            this.btn30min.UseVisualStyleBackColor = false;
            this.btn30min.Click += new System.EventHandler(this.btn30min_Click);
            // 
            // btn1hour
            // 
            this.btn1hour.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(35)))));
            this.btn1hour.BackgroundImage = global::KGHCashierPOS.Properties.Resources._1HOUR;
            this.btn1hour.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn1hour.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btn1hour.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btn1hour.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btn1hour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn1hour.Location = new System.Drawing.Point(165, 47);
            this.btn1hour.Name = "btn1hour";
            this.btn1hour.Size = new System.Drawing.Size(140, 116);
            this.btn1hour.TabIndex = 11;
            this.btn1hour.UseVisualStyleBackColor = false;
            this.btn1hour.Click += new System.EventHandler(this.btn1hour_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Nirmala Text", 9F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(16, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "TIME DURATION";
            // 
            // lblOrderNum
            // 
            this.lblOrderNum.AutoSize = true;
            this.lblOrderNum.Font = new System.Drawing.Font("Miriam Libre", 9.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderNum.Location = new System.Drawing.Point(193, 103);
            this.lblOrderNum.Name = "lblOrderNum";
            this.lblOrderNum.Size = new System.Drawing.Size(101, 26);
            this.lblOrderNum.TabIndex = 48;
            this.lblOrderNum.Text = "Order #: ";
            // 
            // timerDateTime1
            // 
            this.timerDateTime1.Enabled = true;
            this.timerDateTime1.Interval = 1000;
            this.timerDateTime1.Tick += new System.EventHandler(this.timerDateTime1_Tick);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.DarkOrange;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(605, 497);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(159, 61);
            this.btnClear.TabIndex = 49;
            this.btnClear.Text = "CLEAR";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // equipmentRentalControl1
            // 
            this.equipmentRentalControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.equipmentRentalControl1.Location = new System.Drawing.Point(402, 122);
            this.equipmentRentalControl1.Name = "equipmentRentalControl1";
            this.equipmentRentalControl1.Size = new System.Drawing.Size(498, 548);
            this.equipmentRentalControl1.TabIndex = 50;
            this.equipmentRentalControl1.Visible = false;
            // 
            // OrderForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1256, 721);
            this.Controls.Add(this.equipmentRentalControl1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblOrderNum);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lblTotalValue);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnPayCashier);
            this.Controls.Add(this.lbDisplay);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ordernum);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OrderForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnPayCashier;
        private System.Windows.Forms.ListBox lbDisplay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label ordernum;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblSelect;
        private System.Windows.Forms.Button btnBilliards;
        private System.Windows.Forms.Button btnTableTennis;
        private System.Windows.Forms.Button btnScooter;
        private System.Windows.Forms.Button btnBadminton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkExtend;
        private System.Windows.Forms.Button btn30min;
        private System.Windows.Forms.Button btn1hour;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblOrderNum;
        private System.Windows.Forms.Timer timerDateTime1;
        private System.Windows.Forms.Button btnClear;
        private EquipmentRentalControl equipmentRentalControl1;
    }
}