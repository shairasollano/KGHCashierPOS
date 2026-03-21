namespace KGHCashierPOS
{
    partial class paymentControl1
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.lblPaymentTitle = new System.Windows.Forms.Label();
            this.txtCashReceived = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblChange = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGcashRef = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirmPayment = new System.Windows.Forms.Button();
            this.cboDiscountType = new System.Windows.Forms.ComboBox();
            this.lblDiscountTitle = new System.Windows.Forms.Label();
            this.lblTotalAmountTitle = new System.Windows.Forms.Label();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.rbCash = new System.Windows.Forms.RadioButton();
            this.rbGCash = new System.Windows.Forms.RadioButton();
            this.lblPaymentMethodTitle = new System.Windows.Forms.Label();
            this.btnPreviewReceipt = new System.Windows.Forms.Button();
            this.txtDiscountAmount = new System.Windows.Forms.TextBox();
            this.btnApplyDiscount = new System.Windows.Forms.Button();
            this.lblDiscountAmount = new System.Windows.Forms.Label();
            this.rtbSummary = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Font = new System.Drawing.Font("Nirmala UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtotal.Location = new System.Drawing.Point(103, 13);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(116, 30);
            this.lblSubtotal.TabIndex = 70;
            this.lblSubtotal.Text = "lblSubtotal";
            // 
            // lblPaymentTitle
            // 
            this.lblPaymentTitle.AutoSize = true;
            this.lblPaymentTitle.Font = new System.Drawing.Font("Nirmala UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentTitle.Location = new System.Drawing.Point(12, 11);
            this.lblPaymentTitle.Name = "lblPaymentTitle";
            this.lblPaymentTitle.Size = new System.Drawing.Size(104, 30);
            this.lblPaymentTitle.TabIndex = 71;
            this.lblPaymentTitle.Text = "Subtotal: ";
            // 
            // txtCashReceived
            // 
            this.txtCashReceived.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCashReceived.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.txtCashReceived.Location = new System.Drawing.Point(146, 220);
            this.txtCashReceived.Name = "txtCashReceived";
            this.txtCashReceived.Size = new System.Drawing.Size(258, 44);
            this.txtCashReceived.TabIndex = 72;
            this.txtCashReceived.TextChanged += new System.EventHandler(this.txtCashReceived_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Nirmala Text", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(146, 186);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 28);
            this.label1.TabIndex = 73;
            this.label1.Text = "Cash Received";
            // 
            // lblChange
            // 
            this.lblChange.AutoSize = true;
            this.lblChange.Font = new System.Drawing.Font("Nirmala Text", 10F);
            this.lblChange.ForeColor = System.Drawing.Color.Red;
            this.lblChange.Location = new System.Drawing.Point(223, 286);
            this.lblChange.Name = "lblChange";
            this.lblChange.Size = new System.Drawing.Size(100, 28);
            this.lblChange.TabIndex = 74;
            this.lblChange.Text = "lblChange";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nirmala Text", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(146, 286);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 28);
            this.label2.TabIndex = 75;
            this.label2.Text = "Change: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Nirmala Text", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(147, 356);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(251, 28);
            this.label3.TabIndex = 76;
            this.label3.Text = "Gcash Reference Number";
            // 
            // txtGcashRef
            // 
            this.txtGcashRef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGcashRef.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.txtGcashRef.Location = new System.Drawing.Point(151, 387);
            this.txtGcashRef.Name = "txtGcashRef";
            this.txtGcashRef.Size = new System.Drawing.Size(258, 44);
            this.txtGcashRef.TabIndex = 77;
            this.txtGcashRef.TextChanged += new System.EventHandler(this.txtGcashRef_TextChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Crimson;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Nirmala Text", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(465, 448);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(277, 61);
            this.btnCancel.TabIndex = 98;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirmPayment
            // 
            this.btnConfirmPayment.BackColor = System.Drawing.Color.LimeGreen;
            this.btnConfirmPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmPayment.Font = new System.Drawing.Font("Nirmala Text", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmPayment.Location = new System.Drawing.Point(465, 381);
            this.btnConfirmPayment.Name = "btnConfirmPayment";
            this.btnConfirmPayment.Size = new System.Drawing.Size(277, 61);
            this.btnConfirmPayment.TabIndex = 96;
            this.btnConfirmPayment.Text = "PROCESS PAYMENT";
            this.btnConfirmPayment.UseVisualStyleBackColor = false;
            this.btnConfirmPayment.Click += new System.EventHandler(this.btnConfirmPayment_Click);
            // 
            // cboDiscountType
            // 
            this.cboDiscountType.Font = new System.Drawing.Font("Nirmala Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboDiscountType.FormattingEnabled = true;
            this.cboDiscountType.Items.AddRange(new object[] {
            "None",
            "PWD",
            "Senior",
            "Promo Code"});
            this.cboDiscountType.Location = new System.Drawing.Point(437, 219);
            this.cboDiscountType.Name = "cboDiscountType";
            this.cboDiscountType.Size = new System.Drawing.Size(168, 40);
            this.cboDiscountType.TabIndex = 99;
            // 
            // lblDiscountTitle
            // 
            this.lblDiscountTitle.AutoSize = true;
            this.lblDiscountTitle.Font = new System.Drawing.Font("Nirmala UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscountTitle.Location = new System.Drawing.Point(12, 43);
            this.lblDiscountTitle.Name = "lblDiscountTitle";
            this.lblDiscountTitle.Size = new System.Drawing.Size(107, 30);
            this.lblDiscountTitle.TabIndex = 101;
            this.lblDiscountTitle.Text = "Discount: ";
            // 
            // lblTotalAmountTitle
            // 
            this.lblTotalAmountTitle.AutoSize = true;
            this.lblTotalAmountTitle.Font = new System.Drawing.Font("Nirmala UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmountTitle.Location = new System.Drawing.Point(12, 76);
            this.lblTotalAmountTitle.Name = "lblTotalAmountTitle";
            this.lblTotalAmountTitle.Size = new System.Drawing.Size(179, 30);
            this.lblTotalAmountTitle.TabIndex = 103;
            this.lblTotalAmountTitle.Text = "TOTAL AMOUNT:";
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Nirmala UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.Location = new System.Drawing.Point(177, 76);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(130, 30);
            this.lblTotalAmount.TabIndex = 104;
            this.lblTotalAmount.Text = "finalAmount";
            // 
            // rbCash
            // 
            this.rbCash.AutoSize = true;
            this.rbCash.Font = new System.Drawing.Font("Nirmala UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCash.Location = new System.Drawing.Point(176, 29);
            this.rbCash.Name = "rbCash";
            this.rbCash.Size = new System.Drawing.Size(84, 34);
            this.rbCash.TabIndex = 105;
            this.rbCash.TabStop = true;
            this.rbCash.Text = "Cash";
            this.rbCash.UseVisualStyleBackColor = true;
            this.rbCash.CheckedChanged += new System.EventHandler(this.rbCash_CheckedChanged);
            // 
            // rbGCash
            // 
            this.rbGCash.AutoSize = true;
            this.rbGCash.Font = new System.Drawing.Font("Nirmala UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbGCash.Location = new System.Drawing.Point(176, 63);
            this.rbGCash.Name = "rbGCash";
            this.rbGCash.Size = new System.Drawing.Size(108, 34);
            this.rbGCash.TabIndex = 106;
            this.rbGCash.TabStop = true;
            this.rbGCash.Text = "G-Cash";
            this.rbGCash.UseVisualStyleBackColor = true;
            this.rbGCash.CheckedChanged += new System.EventHandler(this.rbGCash_CheckedChanged);
            // 
            // lblPaymentMethodTitle
            // 
            this.lblPaymentMethodTitle.AutoSize = true;
            this.lblPaymentMethodTitle.Font = new System.Drawing.Font("Nirmala Text", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentMethodTitle.Location = new System.Drawing.Point(38, 19);
            this.lblPaymentMethodTitle.Name = "lblPaymentMethodTitle";
            this.lblPaymentMethodTitle.Size = new System.Drawing.Size(100, 84);
            this.lblPaymentMethodTitle.TabIndex = 107;
            this.lblPaymentMethodTitle.Text = "Select \r\nPayment \r\nMethod";
            // 
            // btnPreviewReceipt
            // 
            this.btnPreviewReceipt.BackColor = System.Drawing.Color.SkyBlue;
            this.btnPreviewReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviewReceipt.Font = new System.Drawing.Font("Nirmala Text", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreviewReceipt.Location = new System.Drawing.Point(465, 516);
            this.btnPreviewReceipt.Name = "btnPreviewReceipt";
            this.btnPreviewReceipt.Size = new System.Drawing.Size(277, 61);
            this.btnPreviewReceipt.TabIndex = 108;
            this.btnPreviewReceipt.Text = "PREVIEW RECEIPT";
            this.btnPreviewReceipt.UseVisualStyleBackColor = false;
            this.btnPreviewReceipt.Click += new System.EventHandler(this.btnPreviewReceipt_Click);
            // 
            // txtDiscountAmount
            // 
            this.txtDiscountAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDiscountAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.txtDiscountAmount.Location = new System.Drawing.Point(437, 303);
            this.txtDiscountAmount.Name = "txtDiscountAmount";
            this.txtDiscountAmount.Size = new System.Drawing.Size(218, 44);
            this.txtDiscountAmount.TabIndex = 109;
            // 
            // btnApplyDiscount
            // 
            this.btnApplyDiscount.BackColor = System.Drawing.Color.Gold;
            this.btnApplyDiscount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyDiscount.Font = new System.Drawing.Font("Nirmala Text", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApplyDiscount.Location = new System.Drawing.Point(616, 219);
            this.btnApplyDiscount.Name = "btnApplyDiscount";
            this.btnApplyDiscount.Size = new System.Drawing.Size(128, 40);
            this.btnApplyDiscount.TabIndex = 110;
            this.btnApplyDiscount.Text = "Apply Discount";
            this.btnApplyDiscount.UseVisualStyleBackColor = false;
            this.btnApplyDiscount.Click += new System.EventHandler(this.btnApplyDiscount_Click);
            // 
            // lblDiscountAmount
            // 
            this.lblDiscountAmount.AutoSize = true;
            this.lblDiscountAmount.Font = new System.Drawing.Font("Nirmala UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscountAmount.Location = new System.Drawing.Point(110, 43);
            this.lblDiscountAmount.Name = "lblDiscountAmount";
            this.lblDiscountAmount.Size = new System.Drawing.Size(94, 30);
            this.lblDiscountAmount.TabIndex = 111;
            this.lblDiscountAmount.Text = "discount";
            // 
            // rtbSummary
            // 
            this.rtbSummary.Location = new System.Drawing.Point(35, 30);
            this.rtbSummary.Margin = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.rtbSummary.Name = "rtbSummary";
            this.rtbSummary.Size = new System.Drawing.Size(345, 547);
            this.rtbSummary.TabIndex = 112;
            this.rtbSummary.Text = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(35)))));
            this.panel1.Controls.Add(this.rtbSummary);
            this.panel1.Location = new System.Drawing.Point(24, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(417, 612);
            this.panel1.TabIndex = 113;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(190)))), ((int)(((byte)(95)))));
            this.panel2.Controls.Add(this.lblPaymentTitle);
            this.panel2.Controls.Add(this.lblDiscountAmount);
            this.panel2.Controls.Add(this.lblSubtotal);
            this.panel2.Controls.Add(this.lblTotalAmountTitle);
            this.panel2.Controls.Add(this.lblTotalAmount);
            this.panel2.Controls.Add(this.lblDiscountTitle);
            this.panel2.Location = new System.Drawing.Point(43, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(307, 117);
            this.panel2.TabIndex = 114;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(190)))), ((int)(((byte)(95)))));
            this.panel3.Controls.Add(this.rbCash);
            this.panel3.Controls.Add(this.rbGCash);
            this.panel3.Controls.Add(this.lblPaymentMethodTitle);
            this.panel3.Location = new System.Drawing.Point(382, 30);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(360, 117);
            this.panel3.TabIndex = 115;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(35)))));
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Controls.Add(this.lblChange);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.txtDiscountAmount);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.cboDiscountType);
            this.panel4.Controls.Add(this.btnPreviewReceipt);
            this.panel4.Controls.Add(this.btnApplyDiscount);
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.btnConfirmPayment);
            this.panel4.Controls.Add(this.txtCashReceived);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.txtGcashRef);
            this.panel4.Location = new System.Drawing.Point(465, 24);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(777, 612);
            this.panel4.TabIndex = 115;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Nirmala Text", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(433, 279);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(231, 21);
            this.label7.TabIndex = 118;
            this.label7.Text = "Enter Promo Code (Optional)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Nirmala Text", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(433, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 21);
            this.label6.TabIndex = 117;
            this.label6.Text = "For Discount:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Nirmala Text", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(40, 371);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 42);
            this.label5.TabIndex = 116;
            this.label5.Text = "For G-Cash \r\nPayment:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Nirmala Text", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(40, 220);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 42);
            this.label4.TabIndex = 111;
            this.label4.Text = "For Cash \r\nPayment:";
            // 
            // paymentControl1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Location = new System.Drawing.Point(0, 20);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "paymentControl1";
            this.Size = new System.Drawing.Size(1278, 650);
            this.Load += new System.EventHandler(this.PaymentControl1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.Label lblPaymentTitle;
        private System.Windows.Forms.TextBox txtCashReceived;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGcashRef;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfirmPayment;
        private System.Windows.Forms.ComboBox cboDiscountType;
        private System.Windows.Forms.Label lblDiscountTitle;
        private System.Windows.Forms.Label lblTotalAmountTitle;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.RadioButton rbCash;
        private System.Windows.Forms.RadioButton rbGCash;
        private System.Windows.Forms.Label lblPaymentMethodTitle;
        private System.Windows.Forms.Button btnPreviewReceipt;
        private System.Windows.Forms.TextBox txtDiscountAmount;
        private System.Windows.Forms.Button btnApplyDiscount;
        private System.Windows.Forms.Label lblDiscountAmount;
        private System.Windows.Forms.RichTextBox rtbSummary;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
    }
}
