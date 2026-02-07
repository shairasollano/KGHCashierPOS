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
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.lblPaymentTitle = new System.Windows.Forms.Label();
            this.txtCashReceived = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblChange = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGCashRef = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnConfirmPayment = new System.Windows.Forms.Button();
            this.cmbDiscountType = new System.Windows.Forms.ComboBox();
            this.lblDiscountAmount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvSummary
            // 
            this.lvSummary.HideSelection = false;
            this.lvSummary.Location = new System.Drawing.Point(75, 23);
            this.lvSummary.Name = "lvSummary";
            this.lvSummary.Size = new System.Drawing.Size(312, 561);
            this.lvSummary.TabIndex = 69;
            this.lvSummary.UseCompatibleStateImageBehavior = false;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Location = new System.Drawing.Point(219, 600);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(104, 20);
            this.lblTotalAmount.TabIndex = 70;
            this.lblTotalAmount.Text = "Total Amount";
            // 
            // lblPaymentTitle
            // 
            this.lblPaymentTitle.AutoSize = true;
            this.lblPaymentTitle.Location = new System.Drawing.Point(71, 600);
            this.lblPaymentTitle.Name = "lblPaymentTitle";
            this.lblPaymentTitle.Size = new System.Drawing.Size(142, 20);
            this.lblPaymentTitle.TabIndex = 71;
            this.lblPaymentTitle.Text = "Payment Summary";
            // 
            // txtCashReceived
            // 
            this.txtCashReceived.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCashReceived.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.txtCashReceived.Location = new System.Drawing.Point(810, 81);
            this.txtCashReceived.Name = "txtCashReceived";
            this.txtCashReceived.Size = new System.Drawing.Size(258, 44);
            this.txtCashReceived.TabIndex = 72;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Nirmala Text", 10F);
            this.label1.Location = new System.Drawing.Point(805, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 28);
            this.label1.TabIndex = 73;
            this.label1.Text = "Cash Received";
            // 
            // lblChange
            // 
            this.lblChange.AutoSize = true;
            this.lblChange.Font = new System.Drawing.Font("Nirmala Text", 10F);
            this.lblChange.Location = new System.Drawing.Point(898, 143);
            this.lblChange.Name = "lblChange";
            this.lblChange.Size = new System.Drawing.Size(100, 28);
            this.lblChange.TabIndex = 74;
            this.lblChange.Text = "lblChange";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nirmala Text", 10F);
            this.label2.Location = new System.Drawing.Point(805, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 28);
            this.label2.TabIndex = 75;
            this.label2.Text = "Change: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Nirmala Text", 10F);
            this.label3.Location = new System.Drawing.Point(805, 193);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(230, 28);
            this.label3.TabIndex = 76;
            this.label3.Text = "Gcash Reference Number";
            // 
            // txtGCashRef
            // 
            this.txtGCashRef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGCashRef.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.txtGCashRef.Location = new System.Drawing.Point(810, 224);
            this.txtGCashRef.Name = "txtGCashRef";
            this.txtGCashRef.Size = new System.Drawing.Size(258, 44);
            this.txtGCashRef.TabIndex = 77;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Crimson;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(810, 395);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(258, 54);
            this.btnCancel.TabIndex = 98;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.SkyBlue;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(810, 455);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(258, 54);
            this.button3.TabIndex = 97;
            this.button3.Text = "PRINT RECEIPT";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // btnConfirmPayment
            // 
            this.btnConfirmPayment.BackColor = System.Drawing.Color.LimeGreen;
            this.btnConfirmPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmPayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmPayment.Location = new System.Drawing.Point(810, 335);
            this.btnConfirmPayment.Name = "btnConfirmPayment";
            this.btnConfirmPayment.Size = new System.Drawing.Size(258, 54);
            this.btnConfirmPayment.TabIndex = 96;
            this.btnConfirmPayment.Text = "CONFIRM PAYMENT";
            this.btnConfirmPayment.UseVisualStyleBackColor = false;
            this.btnConfirmPayment.Click += new System.EventHandler(this.btnConfirmPayment_Click);
            // 
            // cmbDiscountType
            // 
            this.cmbDiscountType.FormattingEnabled = true;
            this.cmbDiscountType.Items.AddRange(new object[] {
            "None",
            "PWD",
            "Senior"});
            this.cmbDiscountType.Location = new System.Drawing.Point(497, 213);
            this.cmbDiscountType.Name = "cmbDiscountType";
            this.cmbDiscountType.Size = new System.Drawing.Size(138, 28);
            this.cmbDiscountType.TabIndex = 99;
            // 
            // lblDiscountAmount
            // 
            this.lblDiscountAmount.AutoSize = true;
            this.lblDiscountAmount.Location = new System.Drawing.Point(493, 248);
            this.lblDiscountAmount.Name = "lblDiscountAmount";
            this.lblDiscountAmount.Size = new System.Drawing.Size(127, 20);
            this.lblDiscountAmount.TabIndex = 100;
            this.lblDiscountAmount.Text = "discount amount";
            // 
            // paymentControl1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.Controls.Add(this.lblDiscountAmount);
            this.Controls.Add(this.cmbDiscountType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnConfirmPayment);
            this.Controls.Add(this.txtGCashRef);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblChange);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCashReceived);
            this.Controls.Add(this.lblPaymentTitle);
            this.Controls.Add(this.lblTotalAmount);
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
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Label lblPaymentTitle;
        private System.Windows.Forms.TextBox txtCashReceived;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGCashRef;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnConfirmPayment;
        private System.Windows.Forms.ComboBox cmbDiscountType;
        private System.Windows.Forms.Label lblDiscountAmount;
    }
}
