using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KGHCashierPOS
{
    public partial class paymentControl1 : UserControl
    {
        // ============ MANAGERS ============
        private Dictionary<string, GameSession> _sessions;
        private DiscountManager discountManager;
        private PaymentCalculator calculator;
        private bool isPaymentMethodValid = false;
        private string currentOrderNumber = "";

        // ============ EVENT ============
        public event Action PaymentSuccessful;

        // ============ CONSTRUCTOR ============
        public paymentControl1()
        {
            InitializeComponent();
            discountManager = new DiscountManager();
            calculator = new PaymentCalculator();
            InitializeControls();
        }

        private void PaymentControl1_Load(object sender, EventArgs e)
        {
            calculator.Calculate(_sessions, discountManager.DiscountAmount);
            UpdateDisplays();
        }

        // ============ INITIALIZATION ============
        private void InitializeControls()
        {
            InitializeRichTextBox();
            InitializeDiscountComboBox();
            InitializePaymentMethod();
        }

        private void InitializeRichTextBox()
        {
            rtbSummary.ReadOnly = true;
            rtbSummary.Font = new Font("Courier New", 9);
            rtbSummary.BackColor = Color.White;
            rtbSummary.BorderStyle = BorderStyle.FixedSingle;
        }

        private void InitializeDiscountComboBox()
        {
            cboDiscountType.Items.Clear();
            cboDiscountType.Items.Add("None");
            cboDiscountType.Items.Add("Senior Citizen (20%)");
            cboDiscountType.Items.Add("PWD (20%)");
            cboDiscountType.SelectedIndex = 0;
            txtDiscountAmount.Enabled = false;
        }

        private void InitializePaymentMethod()
        {
            rbCash.Checked = true;
            rbGCash.Checked = false;

            txtGcashRef.Visible = false;
            txtGcashRef.Enabled = false;

            txtCashReceived.Visible = true;
            txtCashReceived.Enabled = true;
            lblChange.Visible = true;

            btnConfirmPayment.Enabled = false;
        }

        // ============ LOAD PAYMENT DATA ============
        public void LoadPaymentData(Dictionary<string, GameSession> sessions, decimal total, string orderNumber = "")
        {
            _sessions = sessions;
            currentOrderNumber = orderNumber;
            discountManager.ClearDiscount();

            // ⭐ Calculate subtotal from sessions (includes equipment)
            decimal calculatedTotal = 0;
            if (sessions != null && sessions.Count > 0)
            {
                foreach (var session in sessions.Values)
                {
                    calculatedTotal += session.TotalPrice + session.EquipmentCost;
                }
            }
            else
            {
                calculatedTotal = total;
            }

            discountManager.SetSubtotal(calculatedTotal);

            cboDiscountType.SelectedIndex = 0;
            txtDiscountAmount.Clear();
            txtDiscountAmount.Enabled = false;

            BuildTransactionSummary();
            calculator.Calculate(_sessions, 0);
            UpdateDisplays();
            InitializePaymentMethod();

            System.Diagnostics.Debug.WriteLine($"=== Payment Data Loaded ===");
            System.Diagnostics.Debug.WriteLine($"Sessions: {sessions?.Count ?? 0}");
            System.Diagnostics.Debug.WriteLine($"Calculated Total: {calculatedTotal:C}");
            System.Diagnostics.Debug.WriteLine($"Passed Total: {total:C}");
        }

        private void BuildTransactionSummary()
        {

            rtbSummary.Clear();
            StringBuilder summary = new StringBuilder();


            if (!string.IsNullOrEmpty(currentOrderNumber))
            {
                summary.AppendLine($"        Order #: {currentOrderNumber}");
                summary.AppendLine();
            }

            summary.AppendLine("        ════════════════════════════════════════════════");
            summary.AppendLine("            TRANSACTION DETAILS");
            summary.AppendLine("        ════════════════════════════════════════════════");
            summary.AppendLine();

            foreach (var session in _sessions.Values)
            {
                string duration = DurationFormatter.Format(session.TotalMinutes);

                session.StartTime = DateTime.Now.AddMinutes(3);
                session.EndTime = session.StartTime.AddMinutes(session.TotalMinutes);
                session.IsActive = true;

                decimal hourlyRate = session.TotalMinutes > 0
                    ? session.TotalPrice / (session.TotalMinutes / 60.0m)
                    : session.TotalPrice;

                summary.AppendLine($"       Game Type:        {session.GameName}");
                summary.AppendLine($"       Start Time:       {session.StartTime:hh:mm tt}");
                summary.AppendLine($"       End Time:         {session.EndTime:hh:mm tt}");
                summary.AppendLine($"       Duration:         {duration}");
                summary.AppendLine($"       Rate:             {PriceFormatter.Format(hourlyRate)}/hour");
                summary.AppendLine($"       Game Price:       {PriceFormatter.Format(session.TotalPrice)}");

                // ⭐ ADD EQUIPMENT DETAILS
                if (session.Equipment != null && session.Equipment.Count > 0)
                {
                    summary.AppendLine("       Equipment:");

                    foreach (var eq in session.Equipment)
                    {
                        /* if (eq.DefaultQuantity > 0)
                        {
                            summary.AppendLine($"     {eq.Name} x{eq.DefaultQuantity} (Included)");
                        } */

                        if (eq.RentalQuantity > 0)
                        {
                            summary.AppendLine($"  {eq.Name} ");
                        }
                    }

                    if (session.EquipmentCost > 0)
                    {
                        summary.AppendLine($"       Equipment Cost:   {PriceFormatter.Format(session.EquipmentCost)}");
                    }
                }

                summary.AppendLine("       ───────────────────────────────────────────────");
                summary.AppendLine($"       Subtotal:         {PriceFormatter.Format(session.TotalPrice + session.EquipmentCost)}");
                summary.AppendLine();
            }

            rtbSummary.Text = summary.ToString();
        }

        // ============ DISCOUNT HANDLING ============
        private void btnApplyDiscount_Click(object sender, EventArgs e)
        {
            if (cboDiscountType.SelectedItem == null)
            {
                MessageBox.Show("Please select a discount type.", "Apply Discount",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selected = cboDiscountType.SelectedItem.ToString();

            switch (selected)
            {
                case "None":
                    MessageBox.Show("No discount selected.", "Apply Discount",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Senior Citizen (20%)":
                case "PWD (20%)":
                    if (ValidateDiscountEligibility(selected))
                    {
                        discountManager.ApplyPercentageDiscount(0.20m, selected);
                        MessageBox.Show($"20% discount applied!\nDiscount: {PriceFormatter.Format(discountManager.DiscountAmount)}",
                            "Discount Applied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
            }

            calculator.Calculate(_sessions, discountManager.DiscountAmount);
            UpdateDisplays();
            System.Diagnostics.Debug.WriteLine($"Discount applied: {discountManager.DiscountAmount}, New Total: {calculator.GetFinalAmount()}");
        }

        private bool ValidateDiscountEligibility(string discountType)
        {
            if (discountType.Contains("Senior") || discountType.Contains("PWD"))
            {
                DialogResult result = MessageBox.Show(
                    "Has the customer presented a valid ID?",
                    "Discount Verification",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    cboDiscountType.SelectedIndex = 0;
                    return false;
                }
            }

            return true;
        }

        // ============ PAYMENT METHOD ============
        private void rbCash_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCash.Checked)
            {
                txtCashReceived.Visible = true;
                txtCashReceived.Enabled = true;
                lblChange.Visible = true;

                txtGcashRef.Visible = false;
                txtGcashRef.Enabled = false;
                txtGcashRef.Clear();

                isPaymentMethodValid = false;
                ValidateCashPayment();
            }
        }

        private void rbGCash_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGCash.Checked)
            {
                txtGcashRef.Visible = true;
                txtGcashRef.Enabled = true;
                txtGcashRef.Focus();

                txtCashReceived.Visible = false;
                txtCashReceived.Enabled = false;
                txtCashReceived.Clear();
                lblChange.Visible = false;

                isPaymentMethodValid = false;
                btnConfirmPayment.Enabled = false;
            }
        }

        private void txtCashReceived_TextChanged(object sender, EventArgs e)
        {
            ValidateCashPayment();
        }

        private void txtGcashRef_TextChanged(object sender, EventArgs e)
        {
            ValidateGCashReference();
        }

        private void ValidateCashPayment()
        {
            if (!rbCash.Checked) return;

            var result = PaymentValidator.ValidateCashPayment(
                txtCashReceived.Text,
                calculator.GetFinalAmount()
            );

            lblChange.Text = result.DisplayText;

            if (result.IsValid)
            {
                lblChange.ForeColor = Color.Green;
                btnConfirmPayment.Enabled = true;
                isPaymentMethodValid = true;
            }
            else
            {
                lblChange.ForeColor = Color.Red;
                btnConfirmPayment.Enabled = false;
                isPaymentMethodValid = false;
            }
        }

        private void ValidateGCashReference()
        {
            if (!rbGCash.Checked) return;

            var result = PaymentValidator.ValidateGCashReference(txtGcashRef.Text);

            if (result.IsValid)
            {
                txtGcashRef.BackColor = Color.LightGreen;
                btnConfirmPayment.Enabled = true;
                isPaymentMethodValid = true;
            }
            else
            {
                txtGcashRef.BackColor = result.Message == "Please enter GCash reference"
                    ? Color.White
                    : Color.LightCoral;
                btnConfirmPayment.Enabled = false;
                isPaymentMethodValid = false;
            }
        }

        // ============ PAYMENT PROCESSING ============
        private void btnConfirmPayment_Click(object sender, EventArgs e)
        {
            if (!isPaymentMethodValid)
            {
                MessageBox.Show("Please enter valid payment information!", "Invalid Payment",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string paymentMethod = rbCash.Checked ? "Cash" : "GCash";
            decimal cashReceived = 0;
            decimal change = 0;
            string reference = "";

            // Validate and get payment details
            if (paymentMethod == "Cash")
            {
                var validation = PaymentValidator.ValidateCashPayment(
                    txtCashReceived.Text,
                    calculator.GetFinalAmount()
                );

                if (!validation.IsValid)
                {
                    MessageBox.Show("Invalid cash payment!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cashReceived = validation.CashReceived;
                change = validation.Change;
                reference = cashReceived.ToString("0.00");
            }
            else
            {
                var validation = PaymentValidator.ValidateGCashReference(txtGcashRef.Text);

                if (!validation.IsValid)
                {
                    MessageBox.Show(validation.Message, "Invalid GCash Reference",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                reference = validation.GCashReference;

                // Check duplicate
                if (PaymentRepository.IsDuplicateGCashReference(reference))
                {
                    if (MessageBox.Show("This GCash reference was used before. Continue?",
                        "Duplicate Reference", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            // Confirm
            string confirmMsg = paymentMethod == "Cash"
                ? $"Cash: {PriceFormatter.Format(cashReceived)}\nChange: {PriceFormatter.Format(change)}\nTotal: {PriceFormatter.Format(calculator.GetFinalAmount())}"
                : $"GCash Ref: {reference}\nTotal: {PriceFormatter.Format(calculator.GetFinalAmount())}";

            if (MessageBox.Show($"{confirmMsg}\n\nConfirm payment?", "Confirm Payment",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            // Process
            ProcessPayment(paymentMethod, cashReceived, change, reference);
        }

        private void ProcessPayment(string method, decimal cashReceived, decimal change, string reference)
        {
            try
            {
                string receiptNo = "MPGH-" + DateTime.Now.ToString("yyyyMMddHHmmss");

                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");
                System.Diagnostics.Debug.WriteLine("PROCESSING PAYMENT");
                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");

                // ⭐ Save sessions and payments to database
                foreach (var session in _sessions.Values)
                {
                    System.Diagnostics.Debug.WriteLine($"\nProcessing session: {session.GameName}");
                    System.Diagnostics.Debug.WriteLine($"  Game Price: {session.TotalPrice:C}");
                    System.Diagnostics.Debug.WriteLine($"  Equipment Cost: {session.EquipmentCost:C}");
                    System.Diagnostics.Debug.WriteLine($"  Equipment Count: {session.Equipment?.Count ?? 0}");

                    // Save session (returns session_id)
                    int sessionId = PaymentRepository.SaveSession(session);

                    if (sessionId <= 0)
                    {
                        throw new Exception("Failed to save session - invalid session ID");
                    }

                    // Save payment for this session
                    PaymentRepository.SavePayment(new PaymentData
                    {
                        SessionId = sessionId,
                        PaymentMethod = method,
                        AmountPaid = calculator.Subtotal, // Total before discount
                        DiscountType = discountManager.DiscountType,
                        DiscountAmount = discountManager.DiscountAmount,
                        FinalAmount = calculator.GetFinalAmount(), // ⭐ Final amount from calculator
                        ReceiptNo = receiptNo,
                        Reference = reference,
                        PaymentDate = DateTime.Now
                    });
                }

                // Update order status if from order
                if (!string.IsNullOrEmpty(currentOrderNumber))
                {
                    OrderRepository.UpdateOrderStatus(currentOrderNumber, "Completed");
                    System.Diagnostics.Debug.WriteLine($"\n✓ Order {currentOrderNumber} marked as Completed");
                }

                // Generate receipt
                string receiptPath = ReceiptGenerator.GenerateReceipt(new ReceiptData
                {
                    Sessions = _sessions,
                    Subtotal = calculator.Subtotal,
                    DiscountAmount = discountManager.DiscountAmount,
                    DiscountType = discountManager.DiscountType,
                    FinalAmount = calculator.GetFinalAmount(),
                    PaymentMethod = method,
                    CashReceived = cashReceived,
                    Change = change,
                    GCashReference = reference
                });

                System.Diagnostics.Debug.WriteLine($"\n✓ Receipt generated: {receiptPath}");
                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");

                MessageBox.Show("Payment successful!\nReceipt generated.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                System.Diagnostics.Process.Start(receiptPath);

                ClearPaymentData();
                this.Visible = false;
                PaymentSuccessful?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing payment:\n{ex.Message}\n\nPlease try again.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");
                System.Diagnostics.Debug.WriteLine($"❌ Payment Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");
            }
        }

        // ============ UPDATE DISPLAYS ============
        private void UpdateDisplays()
        {
            // 1. Calculate the final total
            decimal finalTotal = calculator.Subtotal - discountManager.DiscountAmount;

            // 2. Update Subtotal
            if (lblSubtotal != null)
                lblSubtotal.Text = PriceFormatter.Format(calculator.Subtotal);

            // 3. Update Discount
            if (lblDiscountAmount != null)
            {
                lblDiscountAmount.Text = "-" + PriceFormatter.Format(discountManager.DiscountAmount);
                lblDiscountAmount.ForeColor = Color.Red;
            }

            // 4. Update Final Total (The new part!)
            if (lblTotalAmount != null)
            {
                lblTotalAmount.Text = PriceFormatter.Format(finalTotal);
                // Optional: Make it bold or a different color to stand out
                lblTotalAmount.ForeColor = Color.FromArgb(46, 125, 50); // Dark Green
            }

            // 5. Recalculate change based on the NEW total
            if (rbCash != null && rbCash.Checked)
            {
                ValidateCashPayment();
            }
        }

        // ============ CLEAR DATA ============
        private void ClearPaymentData()
        {
            _sessions?.Clear();
            discountManager.ClearDiscount();
            calculator.Clear();

            rtbSummary.Clear();
            cboDiscountType.SelectedIndex = 0;
            txtDiscountAmount.Clear();
            txtCashReceived.Clear();
            txtGcashRef.Clear();

            lblSubtotal.Text = "₱0.00";
            lblDiscountAmount.Text = "-₱0.00";
            lblTotalAmount.Text = "₱0.00";
            lblChange.Text = "₱0.00";

            rbCash.Checked = true;
            isPaymentMethodValid = false;
            btnConfirmPayment.Enabled = false;
            txtGcashRef.BackColor = Color.White;
            currentOrderNumber = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void btnPreviewReceipt_Click(object sender, EventArgs e)
        {
            if (_sessions == null || _sessions.Count == 0)
            {
                MessageBox.Show("No transactions to preview!", "Preview Receipt",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create preview form
            Form previewForm = new Form();
            previewForm.Text = "Receipt Preview";
            previewForm.Size = new Size(500, 700);
            previewForm.StartPosition = FormStartPosition.CenterParent;
            previewForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            previewForm.MaximizeBox = false;
            previewForm.MinimizeBox = false;
            previewForm.BackColor = Color.White;

            // Create RichTextBox for preview
            RichTextBox rtbPreview = new RichTextBox();
            rtbPreview.Dock = DockStyle.Fill;
            rtbPreview.Font = new Font("Courier New", 9);
            rtbPreview.ReadOnly = true;
            rtbPreview.BackColor = Color.White;
            rtbPreview.BorderStyle = BorderStyle.None;

            // Build receipt content
            StringBuilder receipt = new StringBuilder();

            receipt.AppendLine("          ═══════════════════════════════════════");
            receipt.AppendLine("                 MATCH POINT GAMING HUB");
            receipt.AppendLine("          ═══════════════════════════════════════");
            receipt.AppendLine();
            receipt.AppendLine("                    RECEIPT PREVIEW");
            receipt.AppendLine();
            receipt.AppendLine($"        Date: {DateTime.Now:MM/dd/yyyy hh:mm tt}");
            receipt.AppendLine($"        Cashier: ");
            receipt.AppendLine("        ───────────────────────────────────────");
            receipt.AppendLine();
            receipt.AppendLine("        TRANSACTION DETAILS");
            receipt.AppendLine("        ───────────────────────────────────────");
            receipt.AppendLine();

            // Add items
            foreach (var session in _sessions.Values)
            {
                string duration = DurationFormatter.Format(session.TotalMinutes);

                receipt.AppendLine($"        {session.GameName,-25}");
                receipt.AppendLine($"        Duration: {duration,-15} {PriceFormatter.Format(session.TotalPrice),10}");
                receipt.AppendLine();
            }

            receipt.AppendLine("        ───────────────────────────────────────");
            receipt.AppendLine();

            // Totals
            receipt.AppendLine($"        {"Subtotal:",-30} {PriceFormatter.Format(calculator.Subtotal),10}");

            if (discountManager.DiscountAmount > 0)
            {
                receipt.AppendLine($"        {"Discount (" + discountManager.DiscountType + "):",-30} -{PriceFormatter.Format(discountManager.DiscountAmount),9}");
            }

            receipt.AppendLine("        ═══════════════════════════════════════");
            receipt.AppendLine($"        {"TOTAL AMOUNT DUE:",-30} {PriceFormatter.Format(calculator.GetFinalAmount()),10}");
            receipt.AppendLine("        ═══════════════════════════════════════");
            receipt.AppendLine();

            // Payment method
            string paymentMethod = rbCash.Checked ? "Cash" : "GCash";
            receipt.AppendLine("        PAYMENT METHOD");
            receipt.AppendLine("        ───────────────────────────────────────");
            receipt.AppendLine($"        Payment Type: {paymentMethod}");

            if (paymentMethod == "Cash" && !string.IsNullOrWhiteSpace(txtCashReceived.Text))
            {
                if (decimal.TryParse(txtCashReceived.Text, out decimal cash))
                {
                    decimal change = cash - calculator.GetFinalAmount();
                    receipt.AppendLine($"        Amount Tendered: {PriceFormatter.Format(cash)}");
                    receipt.AppendLine($"        Change: {PriceFormatter.Format(change)}");
                }
            }
            else if (paymentMethod == "GCash" && !string.IsNullOrWhiteSpace(txtGcashRef.Text))
            {
                receipt.AppendLine($"        Reference No: {txtGcashRef.Text}");
            }
            else
            {
                receipt.AppendLine("        (Payment details not yet entered)");
            }

            receipt.AppendLine("        ═══════════════════════════════════════");
            receipt.AppendLine();
            receipt.AppendLine("              Thank you for playing!");
            receipt.AppendLine("              Please visit us again!");
            receipt.AppendLine();
            receipt.AppendLine("        ═══════════════════════════════════════");
            receipt.AppendLine("        This is a PREVIEW only. No payment");
            receipt.AppendLine("        has been processed yet.");
            receipt.AppendLine("        ═══════════════════════════════════════");

            rtbPreview.Text = receipt.ToString();

            // Close button
            Button btnClose = new Button();
            btnClose.Text = "Close Preview";
            btnClose.Dock = DockStyle.Bottom;
            btnClose.Height = 45;
            btnClose.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnClose.BackColor = Color.FromArgb(158, 158, 158);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Cursor = Cursors.Hand;
            btnClose.Click += (s, ev) => previewForm.Close();

            // Add controls to form
            previewForm.Controls.Add(rtbPreview);
            previewForm.Controls.Add(btnClose);

            // Show preview
            previewForm.ShowDialog();
        }
    }
}