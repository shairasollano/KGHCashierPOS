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
            this.lvSummary = new System.Windows.Forms.ListView();
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
            this.lblDiscount = new System.Windows.Forms.Label();
            this.lblTotalAmountTitle = new System.Windows.Forms.Label();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.rbCash = new System.Windows.Forms.RadioButton();
            this.rbGCash = new System.Windows.Forms.RadioButton();
            this.lblPaymentMethodTitle = new System.Windows.Forms.Label();
            this.btnPreviewReceipt = new System.Windows.Forms.Button();
            this.txtDiscountAmount = new System.Windows.Forms.TextBox();
            this.btnApplyDiscount = new System.Windows.Forms.Button();
            this.lblDiscountAmount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvSummary
            // 
            this.lvSummary.HideSelection = false;
            this.lvSummary.Location = new System.Drawing.Point(75, 23);
            this.lvSummary.Name = "lvSummary";
            this.lvSummary.Size = new System.Drawing.Size(312, 474);
            this.lvSummary.TabIndex = 69;
            this.lvSummary.UseCompatibleStateImageBehavior = false;
            // 
            // lblSubtotal
            // 
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Location = new System.Drawing.Point(154, 518);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(84, 20);
            this.lblSubtotal.TabIndex = 70;
            this.lblSubtotal.Text = "lblSubtotal";
            // 
            // lblPaymentTitle
            // 
            this.lblPaymentTitle.AutoSize = true;
            this.lblPaymentTitle.Location = new System.Drawing.Point(71, 518);
            this.lblPaymentTitle.Name = "lblPaymentTitle";
            this.lblPaymentTitle.Size = new System.Drawing.Size(77, 20);
            this.lblPaymentTitle.TabIndex = 71;
            this.lblPaymentTitle.Text = "Subtotal: ";
            // 
            // txtCashReceived
            // 
            this.txtCashReceived.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCashReceived.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.txtCashReceived.Location = new System.Drawing.Point(437, 116);
            this.txtCashReceived.Name = "txtCashReceived";
            this.txtCashReceived.Size = new System.Drawing.Size(258, 44);
            this.txtCashReceived.TabIndex = 72;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Nirmala Text", 10F);
            this.label1.Location = new System.Drawing.Point(432, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 28);
            this.label1.TabIndex = 73;
            this.label1.Text = "Cash Received";
            // 
            // lblChange
            // 
            this.lblChange.AutoSize = true;
            this.lblChange.Font = new System.Drawing.Font("Nirmala Text", 10F);
            this.lblChange.Location = new System.Drawing.Point(525, 178);
            this.lblChange.Name = "lblChange";
            this.lblChange.Size = new System.Drawing.Size(100, 28);
            this.lblChange.TabIndex = 74;
            this.lblChange.Text = "lblChange";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nirmala Text", 10F);
            this.label2.Location = new System.Drawing.Point(432, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 28);
            this.label2.TabIndex = 75;
            this.label2.Text = "Change: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Nirmala Text", 10F);
            this.label3.Location = new System.Drawing.Point(432, 228);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(230, 28);
            this.label3.TabIndex = 76;
            this.label3.Text = "Gcash Reference Number";
            // 
            // txtGcashRef
            // 
            this.txtGcashRef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGcashRef.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.txtGcashRef.Location = new System.Drawing.Point(437, 259);
            this.txtGcashRef.Name = "txtGcashRef";
            this.txtGcashRef.Size = new System.Drawing.Size(258, 44);
            this.txtGcashRef.TabIndex = 77;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Crimson;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(436, 484);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(258, 54);
            this.btnCancel.TabIndex = 98;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirmPayment
            // 
            this.btnConfirmPayment.BackColor = System.Drawing.Color.LimeGreen;
            this.btnConfirmPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmPayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmPayment.Location = new System.Drawing.Point(436, 424);
            this.btnConfirmPayment.Name = "btnConfirmPayment";
            this.btnConfirmPayment.Size = new System.Drawing.Size(258, 54);
            this.btnConfirmPayment.TabIndex = 96;
            this.btnConfirmPayment.Text = "PROCESS PAYMENT";
            this.btnConfirmPayment.UseVisualStyleBackColor = false;
            this.btnConfirmPayment.Click += new System.EventHandler(this.btnConfirmPayment_Click);
            // 
            // cboDiscountType
            // 
            this.cboDiscountType.FormattingEnabled = true;
            this.cboDiscountType.Items.AddRange(new object[] {
            "None",
            "PWD",
            "Senior",
            "Promo Code"});
            this.cboDiscountType.Location = new System.Drawing.Point(437, 346);
            this.cboDiscountType.Name = "cboDiscountType";
            this.cboDiscountType.Size = new System.Drawing.Size(138, 28);
            this.cboDiscountType.TabIndex = 99;
            // 
            // lblDiscountTitle
            // 
            this.lblDiscountTitle.AutoSize = true;
            this.lblDiscountTitle.Location = new System.Drawing.Point(71, 551);
            this.lblDiscountTitle.Name = "lblDiscountTitle";
            this.lblDiscountTitle.Size = new System.Drawing.Size(80, 20);
            this.lblDiscountTitle.TabIndex = 101;
            this.lblDiscountTitle.Text = "Discount: ";
            // 
            // lblDiscount
            // 
            this.lblDiscount.AutoSize = true;
            this.lblDiscount.Location = new System.Drawing.Point(157, 551);
            this.lblDiscount.Name = "lblDiscount";
            this.lblDiscount.Size = new System.Drawing.Size(87, 20);
            this.lblDiscount.TabIndex = 102;
            this.lblDiscount.Text = "lblDiscount";
            // 
            // lblTotalAmountTitle
            // 
            this.lblTotalAmountTitle.AutoSize = true;
            this.lblTotalAmountTitle.Location = new System.Drawing.Point(71, 587);
            this.lblTotalAmountTitle.Name = "lblTotalAmountTitle";
            this.lblTotalAmountTitle.Size = new System.Drawing.Size(178, 20);
            this.lblTotalAmountTitle.TabIndex = 103;
            this.lblTotalAmountTitle.Text = "TOTAL AMOUNT DUE: ";
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Location = new System.Drawing.Point(242, 587);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(94, 20);
            this.lblTotalAmount.TabIndex = 104;
            this.lblTotalAmount.Text = "finalAmount";
            // 
            // rbCash
            // 
            this.rbCash.AutoSize = true;
            this.rbCash.Location = new System.Drawing.Point(742, 146);
            this.rbCash.Name = "rbCash";
            this.rbCash.Size = new System.Drawing.Size(71, 24);
            this.rbCash.TabIndex = 105;
            this.rbCash.TabStop = true;
            this.rbCash.Text = "Cash";
            this.rbCash.UseVisualStyleBackColor = true;
            // 
            // rbGCash
            // 
            this.rbGCash.AutoSize = true;
            this.rbGCash.Location = new System.Drawing.Point(742, 176);
            this.rbGCash.Name = "rbGCash";
            this.rbGCash.Size = new System.Drawing.Size(89, 24);
            this.rbGCash.TabIndex = 106;
            this.rbGCash.TabStop = true;
            this.rbGCash.Text = "G-Cash";
            this.rbGCash.UseVisualStyleBackColor = true;
            // 
            // lblPaymentMethodTitle
            // 
            this.lblPaymentMethodTitle.AutoSize = true;
            this.lblPaymentMethodTitle.Location = new System.Drawing.Point(739, 116);
            this.lblPaymentMethodTitle.Name = "lblPaymentMethodTitle";
            this.lblPaymentMethodTitle.Size = new System.Drawing.Size(129, 20);
            this.lblPaymentMethodTitle.TabIndex = 107;
            this.lblPaymentMethodTitle.Text = "Payment Method";
            // 
            // btnPreviewReceipt
            // 
            this.btnPreviewReceipt.BackColor = System.Drawing.Color.SkyBlue;
            this.btnPreviewReceipt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviewReceipt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreviewReceipt.Location = new System.Drawing.Point(436, 544);
            this.btnPreviewReceipt.Name = "btnPreviewReceipt";
            this.btnPreviewReceipt.Size = new System.Drawing.Size(258, 54);
            this.btnPreviewReceipt.TabIndex = 108;
            this.btnPreviewReceipt.Text = "PREVIEW RECEIPT";
            this.btnPreviewReceipt.UseVisualStyleBackColor = false;
            this.btnPreviewReceipt.Click += new System.EventHandler(this.btnPreviewReceipt_Click);
            // 
            // txtDiscountAmount
            // 
            this.txtDiscountAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDiscountAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.txtDiscountAmount.Location = new System.Drawing.Point(725, 339);
            this.txtDiscountAmount.Name = "txtDiscountAmount";
            this.txtDiscountAmount.Size = new System.Drawing.Size(258, 44);
            this.txtDiscountAmount.TabIndex = 109;
            // 
            // btnApplyDiscount
            // 
            this.btnApplyDiscount.Location = new System.Drawing.Point(778, 419);
            this.btnApplyDiscount.Name = "btnApplyDiscount";
            this.btnApplyDiscount.Size = new System.Drawing.Size(143, 65);
            this.btnApplyDiscount.TabIndex = 110;
            this.btnApplyDiscount.Text = "Apply Discount";
            this.btnApplyDiscount.UseVisualStyleBackColor = true;
            this.btnApplyDiscount.Click += new System.EventHandler(this.btnApplyDiscount_Click);
            // 
            // lblDiscountAmount
            // 
            this.lblDiscountAmount.AutoSize = true;
            this.lblDiscountAmount.Location = new System.Drawing.Point(581, 349);
            this.lblDiscountAmount.Name = "lblDiscountAmount";
            this.lblDiscountAmount.Size = new System.Drawing.Size(69, 20);
            this.lblDiscountAmount.TabIndex = 111;
            this.lblDiscountAmount.Text = "discount";
            // 
            // paymentControl1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.Controls.Add(this.lblDiscountAmount);
            this.Controls.Add(this.btnApplyDiscount);
            this.Controls.Add(this.txtDiscountAmount);
            this.Controls.Add(this.btnPreviewReceipt);
            this.Controls.Add(this.lblPaymentMethodTitle);
            this.Controls.Add(this.rbGCash);
            this.Controls.Add(this.rbCash);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.lblTotalAmountTitle);
            this.Controls.Add(this.lblDiscount);
            this.Controls.Add(this.lblDiscountTitle);
            this.Controls.Add(this.cboDiscountType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirmPayment);
            this.Controls.Add(this.txtGcashRef);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblChange);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCashReceived);
            this.Controls.Add(this.lblPaymentTitle);
            this.Controls.Add(this.lblSubtotal);
            this.Controls.Add(this.lvSummary);
            this.Location = new System.Drawing.Point(0, 20);
            this.Name = "paymentControl1";
            this.Size = new System.Drawing.Size(1256, 655);
            this.Load += new System.EventHandler(this.PaymentControl1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListView lvSummary;
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
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.Label lblTotalAmountTitle;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.RadioButton rbCash;
        private System.Windows.Forms.RadioButton rbGCash;
        private System.Windows.Forms.Label lblPaymentMethodTitle;
        private System.Windows.Forms.Button btnPreviewReceipt;
        private System.Windows.Forms.TextBox txtDiscountAmount;
        private System.Windows.Forms.Button btnApplyDiscount;
        private System.Windows.Forms.Label lblDiscountAmount;
    }
}
