using iTextSharp.text;
using iTextSharp.text.pdf;
using KGHCashierPOS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KGHCashierPOS
{
    public partial class paymentControl1 : UserControl
    {
        // ============ VARIABLES ============
        private Dictionary<string, GameSession> _sessions;
        private decimal _totalAmount;
        private decimal discountAmount = 0;
        private decimal subtotalAmount = 0;
        private decimal finalAmount = 0;
        private bool isPaymentMethodValid = false;

        // ============ CONSTRUCTOR ============
        public paymentControl1()
        {
            InitializeComponent();
            InitializeRichTextBox(); 
            InitializeDiscountComboBox();
            InitializePaymentValidation();
        }

        private void PaymentControl1_Load(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        public event Action PaymentSuccessful;

        private void InitializePaymentValidation()
        {
            // Set Cash as default
            rbCash.Checked = true;
            rbGCash.Checked = false;

            // Hide GCash fields initially 
            txtGcashRef.Visible = false;
            txtGcashRef.Enabled = false;

            // Show cash fields
            txtCashReceived.Visible = true;
            txtCashReceived.Enabled = true;
            lblChange.Visible = true;

            // Disable confirm button initially
            btnConfirmPayment.Enabled = false;

        }


        // ============ LOAD PAYMENT DATA ============
        public void LoadPaymentData(Dictionary<string, GameSession> sessions, decimal total)
        {
            _sessions = sessions;
            _totalAmount = total;

            // Reset discount
            discountAmount = 0;
            cboDiscountType.SelectedIndex = 0;
            txtDiscountAmount.Clear();
            txtDiscountAmount.Enabled = false;

            // Build transaction summary
            rtbSummary.Clear();
            StringBuilder summary = new StringBuilder();

            summary.AppendLine("        ════════════════════════════════════════════════");
            summary.AppendLine("            TRANSACTION DETAILS");
            summary.AppendLine("        ════════════════════════════════════════════════");
            summary.AppendLine();

            foreach (var session in sessions.Values)
            {
                string duration;
                if (session.TotalMinutes >= 60)
                {
                    int hours = session.TotalMinutes / 60;
                    int minutes = session.TotalMinutes % 60;
                    duration = minutes > 0
                        ? $"{hours} hours {minutes} minutes"
                        : $"{hours} hour" + (hours > 1 ? "s" : "");
                }
                else
                {
                    duration = $"{session.TotalMinutes} minutes";
                }

                // ADDED TIME 3 MINUTE INCREMENT

                session.StartTime = DateTime.Now.AddMinutes(3);
                session.EndTime = session.StartTime.AddMinutes(session.TotalMinutes);
                session.IsActive = true;

                decimal hours_decimal = session.TotalMinutes / 60.0m;
                decimal hourlyRate = hours_decimal > 0 ? session.TotalPrice / hours_decimal : session.TotalPrice;

                summary.AppendLine($"       Game Type:        {session.GameName}");

                summary.AppendLine($"       Start Time:       {session.StartTime:hh:mm tt}");

                summary.AppendLine($"       End Time:         {session.EndTime:hh:mm tt}");

                summary.AppendLine($"       Duration:         {duration}");
                summary.AppendLine($"       Rate:             ₱ {hourlyRate:N2}/hour");
                summary.AppendLine("       ───────────────────────────────────────────────");
                summary.AppendLine($"       Subtotal:         ₱ {session.TotalPrice:N2}");
                summary.AppendLine();
            }

            rtbSummary.Text = summary.ToString();

            // Calculate totals
            CalculateTotals();

            // IMPORTANT: Initialize payment validation
            InitializePaymentValidation();

            System.Diagnostics.Debug.WriteLine($"=== PAYMENT DATA LOADED ===");
            System.Diagnostics.Debug.WriteLine($"Sessions: {sessions.Count}");
            System.Diagnostics.Debug.WriteLine($"Final Amount: ₱{finalAmount:N2}");

        }


        // ============ INITIALIZATION ============
        private void InitializeRichTextBox()
        {
            rtbSummary.ReadOnly = true;
            rtbSummary.Font = new System.Drawing.Font("Courier New", 9);
            rtbSummary.BackColor = System.Drawing.Color.White;
            rtbSummary.BorderStyle = BorderStyle.FixedSingle;
        }

        private void InitializeDiscountComboBox()
        {
            cboDiscountType.Items.Clear();
            cboDiscountType.Items.Add("None");
            cboDiscountType.Items.Add("Senior Citizen (20%)");
            cboDiscountType.Items.Add("PWD (20%)");
            // cboDiscountType.Items.Add("Member (10%)");
            // cboDiscountType.Items.Add("Promo Code");
            // cboDiscountType.Items.Add("Custom Amount");

            cboDiscountType.SelectedIndex = 0;
            txtDiscountAmount.Enabled = false;
        }



        private void txtDiscountAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string selectedDiscount = cboDiscountType.SelectedItem?.ToString();

                if (selectedDiscount == "Promo Code")
                {
                    ValidatePromoCode(txtDiscountAmount.Text);
                    e.Handled = true;
                }
            }
        }

        // ============ APPLY DISCOUNT BUTTON ============
        private void btnApplyDiscount_Click(object sender, EventArgs e)
        {
            if (cboDiscountType.SelectedItem == null)
            {
                MessageBox.Show("Please select a discount type first.", "Apply Discount",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedDiscount = cboDiscountType.SelectedItem.ToString();

            switch (selectedDiscount)
            {
                case "None":
                    MessageBox.Show("No discount selected.", "Apply Discount",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Senior Citizen (20%)":
                case "PWD (20%)":
                    if (!ValidateDiscountEligibility(selectedDiscount))
                        return;

                    subtotalAmount = CalculateSubtotal();
                    discountAmount = subtotalAmount * 0.20m;

                    MessageBox.Show($"20% discount applied!\nDiscount Amount: ₱ {discountAmount:N2}",
                        "Discount Applied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Member (10%)":
                    subtotalAmount = CalculateSubtotal();
                    discountAmount = subtotalAmount * 0.10m;

                    MessageBox.Show($"10% discount applied!\nDiscount Amount: ₱ {discountAmount:N2}",
                        "Discount Applied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Promo Code":
                    if (string.IsNullOrWhiteSpace(txtDiscountAmount.Text))
                    {
                        MessageBox.Show("Please enter a promo code.", "Apply Discount",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDiscountAmount.Focus();
                        return;
                    }

                    ValidatePromoCode(txtDiscountAmount.Text);
                    return;

                case "Custom Amount":
                    if (string.IsNullOrWhiteSpace(txtDiscountAmount.Text))
                    {
                        MessageBox.Show("Please enter a discount amount.", "Apply Discount",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDiscountAmount.Focus();
                        return;
                    }

                    if (!decimal.TryParse(txtDiscountAmount.Text, out decimal customAmount))
                    {
                        MessageBox.Show("Please enter a valid amount.", "Apply Discount",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDiscountAmount.Focus();
                        return;
                    }

                    subtotalAmount = CalculateSubtotal();

                    if (customAmount > subtotalAmount)
                    {
                        MessageBox.Show($"Discount amount (₱{customAmount:N2}) cannot exceed subtotal (₱ {subtotalAmount:N2})!",
                            "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    discountAmount = customAmount;

                    MessageBox.Show($"Custom discount applied!\nDiscount Amount: ₱{discountAmount:N2}",
                        "Discount Applied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }

            CalculateTotals();
        }

        // ============ DISCOUNT CALCULATION ============
        private void ApplyPercentageDiscount(decimal percentage)
        {
            subtotalAmount = CalculateSubtotal();
            discountAmount = subtotalAmount * percentage;

            if (lblDiscountAmount != null)
            {
                lblDiscountAmount.Text = "- ₱ " + discountAmount.ToString("N2");
                lblDiscountAmount.ForeColor = System.Drawing.Color.Red;
            }
        }

        private decimal CalculateSubtotal()
        {
            decimal subtotal = 0;

            // Calculate from _sessions instead of RichTextBox
            if (_sessions != null)
            {
                foreach (var session in _sessions.Values)
                {
                    subtotal += session.TotalPrice;
                }
            }

            return subtotal;
        }

        private void CalculateTotals()
        {
            // Calculate subtotal
            subtotalAmount = CalculateSubtotal();

            // Apply discount (NO TAX)
            finalAmount = subtotalAmount - discountAmount;

            // Update labels
            if (lblSubtotal != null)
            {
                lblSubtotal.Text = "₱ " + subtotalAmount.ToString("N2");
            }

            if (lblDiscountAmount != null)
            {
                lblDiscountAmount.Text = "- ₱ " + discountAmount.ToString("N2");
                lblDiscountAmount.ForeColor = System.Drawing.Color.Red;
            }

            if (lblTotalAmount != null)
            {
                lblTotalAmount.Text = "₱ " + finalAmount.ToString("N2");
            }

            // Update change if cash is entered
            if (rbCash != null && rbCash.Checked && !string.IsNullOrEmpty(txtCashReceived.Text))
            {
                if (decimal.TryParse(txtCashReceived.Text, out decimal cash))
                {
                    decimal change = cash - finalAmount;
                    lblChange.Text = change >= 0 ? "₱ " + change.ToString("N2") : "Insufficient";
                }
            }
        }

        private bool ValidateDiscountEligibility(string discountType)
        {
            if (discountType.Contains("Senior") || discountType.Contains("PWD"))
            {
                DialogResult result = MessageBox.Show(
                    "Has the customer presented a valid ID for this discount?",
                    "Discount Verification",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    cboDiscountType.SelectedIndex = 0;
                    return false;
                }

                LogActivity("Discount Applied", $"{discountType} - ID Verified");
            }

            return true;
        }

        private void ValidatePromoCode(string promoCode)
        {
            if (string.IsNullOrWhiteSpace(promoCode))
            {
                MessageBox.Show("Please enter a promo code", "Promo Code",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal promoDiscount = 0;

            switch (promoCode.ToUpper())
            {
                case "WELCOME10":
                    promoDiscount = CalculateSubtotal() * 0.10m;
                    MessageBox.Show("Promo code applied: 10% discount!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "NEWUSER20":
                    promoDiscount = CalculateSubtotal() * 0.20m;
                    MessageBox.Show("Promo code applied: 20% discount!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "FREEGAME":
                    promoDiscount = 100;
                    MessageBox.Show("Promo code applied: ₱100 discount!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                default:
                    MessageBox.Show("Invalid promo code!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDiscountAmount.Clear();
                    return;
            }

            discountAmount = promoDiscount;
            CalculateTotals();
        }

        // ============ PAYMENT METHOD HANDLERS ============
        private void rbCash_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCash.Checked)
            {
                // Show cash controls
                txtCashReceived.Visible = true;
                txtCashReceived.Enabled = true;
                lblChange.Visible = true;

                // Hide GCash controls (BEST OPTION - completely hidden)
                txtGcashRef.Visible = false;
                txtGcashRef.Enabled = false;
                txtGcashRef.Clear();

                // Clear validation flag
                isPaymentMethodValid = false;

                // Revalidate cash amount
                ValidateCashPayment();

                System.Diagnostics.Debug.WriteLine("Payment method: CASH selected");
            }
        }

        private void rbGCash_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGCash.Checked)
            {
                // Show GCash controls
                txtGcashRef.Visible = true;
                txtGcashRef.Enabled = true;
                txtGcashRef.Focus();

                // Hide cash controls
                txtCashReceived.Visible = false;
                txtCashReceived.Enabled = false;
                txtCashReceived.Clear();
                lblChange.Visible = false;
                lblChange.Text = "₱0.00";

                // Clear validation flag
                isPaymentMethodValid = false;

                // Disable button until valid reference
                btnConfirmPayment.Enabled = false;

                System.Diagnostics.Debug.WriteLine("Payment method: GCASH selected");
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
            if (!rbCash.Checked)
                return;

            // Check if input is empty
            if (string.IsNullOrWhiteSpace(txtCashReceived.Text))
            {
                lblChange.Text = "₱ 0.00";
                lblChange.ForeColor = System.Drawing.Color.White;
                btnConfirmPayment.Enabled = false;
                isPaymentMethodValid = false;
                return;
            }

            // Try to parse the cash amount
            if (decimal.TryParse(txtCashReceived.Text, out decimal cashReceived))
            {
                // Check if cash is sufficient
                if (cashReceived >= finalAmount)
                {
                    decimal change = cashReceived - finalAmount;
                    lblChange.Text = "₱ " + change.ToString("N2");
                    lblChange.ForeColor = System.Drawing.Color.Green;
                    btnConfirmPayment.Enabled = true;
                    isPaymentMethodValid = true;

                    System.Diagnostics.Debug.WriteLine($"Cash valid: ₱{cashReceived:N2} >= ₱{finalAmount:N2}");
                }
                else
                {
                    lblChange.Text = "Insufficient";
                    lblChange.ForeColor = System.Drawing.Color.Red;
                    btnConfirmPayment.Enabled = false;
                    isPaymentMethodValid = false;

                    System.Diagnostics.Debug.WriteLine($"Cash insufficient: ₱{cashReceived:N2} < ₱{finalAmount:N2}");
                }
            }
            else
            {
                lblChange.Text = "Invalid amount";
                lblChange.ForeColor = System.Drawing.Color.Red;
                btnConfirmPayment.Enabled = false;
                isPaymentMethodValid = false;
            }
        }

        private void ValidateGCashReference()
        {
            if (!rbGCash.Checked)
                return;

            string reference = txtGcashRef.Text.Trim();

            // Check if empty
            if (string.IsNullOrWhiteSpace(reference))
            {
                btnConfirmPayment.Enabled = false;
                isPaymentMethodValid = false;
                txtGcashRef.BackColor = System.Drawing.Color.White;
                return;
            }

            // Validate GCash reference format
            // GCash reference numbers are typically 13 digits
            bool isValid = ValidateGCashFormat(reference);

            if (isValid)
            {
                txtGcashRef.BackColor = System.Drawing.Color.LightGreen;
                btnConfirmPayment.Enabled = true;
                isPaymentMethodValid = true;

                System.Diagnostics.Debug.WriteLine($"GCash reference valid: {reference}");
            }
            else
            {
                txtGcashRef.BackColor = System.Drawing.Color.LightCoral;
                btnConfirmPayment.Enabled = false;
                isPaymentMethodValid = false;

                System.Diagnostics.Debug.WriteLine($"GCash reference invalid: {reference}");
            }
        }

        private bool ValidateGCashFormat(string reference)
        {
            // Remove any spaces or dashes
            reference = reference.Replace(" ", "").Replace("-", "");

            // Check if all characters are digits
            if (!reference.All(char.IsDigit))
            {
                return false;
            }

            // GCash reference format validation
            // Standard GCash reference: 13 digits
            // Example: 1234567890123
            if (reference.Length == 13)
            {
                return true;
            }

            // Some GCash variants use 12 or 14 digits
            // Adjust based on your requirements
            if (reference.Length >= 12 && reference.Length <= 14)
            {
                return true;
            }

            return false;
        }

        private string GetSelectedPaymentMethod()
        {
            if (rbCash != null && rbCash.Checked)
                return "Cash";
            else if (rbGCash != null && rbGCash.Checked)
                return "GCash";
            else
                return "Cash";
        }


        // ============ PAYMENT PROCESSING ============
        private void btnConfirmPayment_Click(object sender, EventArgs e)
        {
            // Double-check validation before processing
            if (!isPaymentMethodValid)
            {
                MessageBox.Show("Please enter valid payment information!", "Invalid Payment",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string paymentMethod = GetSelectedPaymentMethod();
            string reference = "";
            decimal cashAmount = 0;

            // Validate based on payment method
            if (paymentMethod == "Cash")
            {
                if (string.IsNullOrWhiteSpace(txtCashReceived.Text))
                {
                    MessageBox.Show("Please enter cash received amount.", "Payment Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCashReceived.Focus();
                    return;
                }

                if (!decimal.TryParse(txtCashReceived.Text, out cashAmount))
                {
                    MessageBox.Show("Invalid cash amount format!", "Payment Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCashReceived.Focus();
                    return;
                }

                if (cashAmount < finalAmount)
                {
                    MessageBox.Show($"Insufficient cash!\n\nReceived: ₱{cashAmount:N2}\nRequired: ₱{finalAmount:N2}",
                        "Payment Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCashReceived.Focus();
                    return;
                }

                reference = cashAmount.ToString("0.00");
            }
            else if (paymentMethod == "GCash")
            {
                if (string.IsNullOrWhiteSpace(txtGcashRef.Text))
                {
                    MessageBox.Show("Please enter GCash reference number.", "Payment Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGcashRef.Focus();
                    return;
                }

                reference = txtGcashRef.Text.Trim();

                // Final validation
                if (!ValidateGCashFormat(reference))
                {
                    MessageBox.Show("Invalid GCash reference number!\n\n" +
                        "GCash reference must be 13 digits.\n" +
                        "Example: 1234567890123",
                        "Invalid Reference", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGcashRef.Focus();
                    return;
                }

                // Optional: Check for duplicate reference
                if (CheckDuplicateGCashReference(reference))
                {
                    DialogResult result = MessageBox.Show(
                        "This GCash reference number has been used before!\n\n" +
                        "Are you sure you want to continue?",
                        "Duplicate Reference",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.No)
                    {
                        txtGcashRef.Focus();
                        txtGcashRef.SelectAll();
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a payment method.", "Payment Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Show confirmation dialog
            string confirmationMessage = paymentMethod == "Cash"
                ? $"Payment Method: Cash\n" +
                  $"Amount Received: ₱{cashAmount:N2}\n" +
                  $"Total: ₱{finalAmount:N2}\n" +
                  $"Change: ₱{(cashAmount - finalAmount):N2}\n\n" +
                  "Confirm this payment?"
                : $"Payment Method: GCash\n" +
                  $"Reference Number: {reference}\n" +
                  $"Total: ₱{finalAmount:N2}\n\n" +
                  "Confirm this payment?";

            DialogResult confirmResult = MessageBox.Show(
                confirmationMessage,
                "Confirm Payment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmResult == DialogResult.No)
                return;

            // Process payment
            try
            {
                string receiptNo = "MPGH-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string discountType = cboDiscountType.SelectedItem?.ToString() ?? "None";

                // Save to database
                foreach (var session in _sessions.Values)
                {
                    int sessionId = SaveSession(session);
                    SavePayment(sessionId, paymentMethod, subtotalAmount, discountAmount,
                        finalAmount, discountType, receiptNo, reference);
                }

                // Generate receipt
                decimal change = paymentMethod == "Cash" ? cashAmount - finalAmount : 0;
                GenerateReceiptPDF(paymentMethod, cashAmount.ToString("0.00"), change, reference);

                // Log activity
                LogActivity("Payment Processed", $"{paymentMethod} - ₱{finalAmount:N2}");

                MessageBox.Show("Payment successful!\nReceipt has been generated.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear payment data
                ClearPaymentData();

                // Hide payment control
                this.Visible = false;

                // ⭐ ADD THIS LINE - RAISE THE EVENT ⭐
                PaymentSuccessful?.Invoke();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing payment: {ex.Message}", "Payment Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private bool CheckDuplicateGCashReference(string reference)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT COUNT(*) 
                FROM payments 
                WHERE payment_method = 'GCash' 
                AND amount_tendered = @reference";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@reference", reference);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking duplicate: {ex.Message}");
                return false;
            }
        }

        // ============ RESET PAYMENT FIELDS ============
        private void ClearPaymentData()
        {
            // Clear sessions
            _sessions?.Clear();
            _totalAmount = 0;
            discountAmount = 0;
            subtotalAmount = 0;
            finalAmount = 0;

            // Clear display
            rtbSummary.Clear();

            // Reset discount
            if (cboDiscountType != null)
            {
                cboDiscountType.SelectedIndex = 0;
            }

            txtDiscountAmount.Clear();
            txtDiscountAmount.Enabled = false;

            // Clear payment fields
            txtCashReceived.Clear();
            txtGcashRef.Clear();

            // Reset labels
            if (lblSubtotal != null)
                lblSubtotal.Text = "₱0.00";

            if (lblDiscountAmount != null)
                lblDiscountAmount.Text = "-₱0.00";

            if (lblTotalAmount != null)
                lblTotalAmount.Text = "₱0.00";

            if (lblChange != null)
                lblChange.Text = "₱0.00";

            // Reset payment method to Cash
            if (rbCash != null)
                rbCash.Checked = true;

            // Reset validation
            isPaymentMethodValid = false;
            btnConfirmPayment.Enabled = false;

            // Reset background colors
            txtGcashRef.BackColor = System.Drawing.Color.White;

            System.Diagnostics.Debug.WriteLine("=== PAYMENT DATA CLEARED ===");
        }



        // UPDATE ORDER STATUS TO COMPLETED
        private void UpdateOrderStatus(string orderNumber)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    string query = "UPDATE orders SET status = 'Completed' WHERE order_number = @orderNo";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderNo", orderNumber);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating order status: {ex.Message}");
            }
        }

        // ============ DATABASE OPERATIONS ============
        private int SaveSession(GameSession session)
        {
            int sessionId = 0;

            using (var conn = new MySqlConnection(Database.ConnectionString))
            {
                conn.Open();

                string query = @"
                    INSERT INTO sessions
                    (game_name, start_time, end_time, total_minutes, total_price, status)
                    VALUES
                    (@game, @start, @end, @minutes, @price, 'Completed');
                    SELECT LAST_INSERT_ID();";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@game", session.GameName);
                cmd.Parameters.AddWithValue("@start", session.StartTime);
                cmd.Parameters.AddWithValue("@end", session.EndTime);
                cmd.Parameters.AddWithValue("@minutes", session.TotalMinutes);
                cmd.Parameters.AddWithValue("@price", session.TotalPrice);

                sessionId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return sessionId;
        }

        private void SavePayment(
            int sessionId,
            string method,
            decimal amountPaid,
            decimal discount,
            decimal finalAmount,
            string discountType,
            string receiptNo,
            string reference)
        {
            using (var conn = new MySqlConnection(Database.ConnectionString))
            {
                conn.Open();

                string query = @"
                    INSERT INTO payments
                    (session_id, payment_method, amount_paid, discount_type,
                     discount_amount, final_amount, receipt_no, amount_tendered, payment_date)
                    VALUES
                    (@sid, @method, @amt, @dtype, @disc, @final, @rno, @ref, NOW())";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sid", sessionId);
                cmd.Parameters.AddWithValue("@method", method);
                cmd.Parameters.AddWithValue("@amt", amountPaid);
                cmd.Parameters.AddWithValue("@dtype", discountType);
                cmd.Parameters.AddWithValue("@disc", discount);
                cmd.Parameters.AddWithValue("@final", finalAmount);
                cmd.Parameters.AddWithValue("@rno", receiptNo);
                cmd.Parameters.AddWithValue("@ref", reference);

                cmd.ExecuteNonQuery();
            }
        }


        // ============ UTILITY METHODS ============
        private void LogActivity(string activity, string details)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}] {activity}: {details}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Logging error: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
       
        
        // ============ PREVIEW RECEIPT ============
        private void btnPreviewReceipt_Click(object sender, EventArgs e)
        {
            if (_sessions == null || _sessions.Count == 0)
            {
                MessageBox.Show("No transactions to preview!", "Preview Receipt",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form previewForm = new Form();
            previewForm.Text = "          Receipt Preview";
            previewForm.Size = new System.Drawing.Size(450, 700);
            previewForm.StartPosition = FormStartPosition.CenterParent;
            previewForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            previewForm.MaximizeBox = false;
            previewForm.MinimizeBox = false;

            RichTextBox rtbPreview = new RichTextBox();
            rtbPreview.Dock = DockStyle.Fill;
            rtbPreview.Font = new System.Drawing.Font("Courier New", 9);
            rtbPreview.ReadOnly = true;
            rtbPreview.BackColor = System.Drawing.Color.White;

            StringBuilder receipt = new StringBuilder();

            receipt.AppendLine("          ═══════════════════════════════════════");
            receipt.AppendLine("                 MATCH POINT GAMING HUB");
            receipt.AppendLine("          ═══════════════════════════════════════");
            receipt.AppendLine();
            receipt.AppendLine("        RECEIPT PREVIEW");
            receipt.AppendLine();
            receipt.AppendLine($"        Date: {DateTime.Now:MM/dd/yyyy hh:mm tt}");
            receipt.AppendLine($"        Cashier: {Environment.UserName}");
            receipt.AppendLine("        ───────────────────────────────────────");
            receipt.AppendLine();
            receipt.AppendLine("        TRANSACTION DETAILS");
            receipt.AppendLine("        ───────────────────────────────────────");
            receipt.AppendLine();

            foreach (var session in _sessions.Values)
            {
                string duration;
                if (session.TotalMinutes >= 60)
                {
                    int hours = session.TotalMinutes / 60;
                    int minutes = session.TotalMinutes % 60;
                    duration = minutes > 0
                        ? $"{hours} hr {minutes} min"
                        : $"{hours} hr";
                }
                else
                {
                    duration = $"        {session.TotalMinutes} min";
                }

                receipt.AppendLine($"        {session.GameName,-20} {duration,-12} ₱{session.TotalPrice,8:N2}");
            }

            receipt.AppendLine("        ───────────────────────────────────────");
            receipt.AppendLine();
            receipt.AppendLine($"{"        Subtotal:",-30} ₱{subtotalAmount,8:N2}");

            if (discountAmount > 0)
            {
                string discountType = cboDiscountType.SelectedItem?.ToString() ?? "None";
                receipt.AppendLine($"{$"        Discount ({discountType}):",-30} -₱{discountAmount,7:N2}");
            }

            receipt.AppendLine("        ═══════════════════════════════════════");
            receipt.AppendLine($"{"        TOTAL AMOUNT DUE:",-30} ₱{finalAmount,8:N2}");
            receipt.AppendLine("        ═══════════════════════════════════════");
            receipt.AppendLine();

            string paymentMethod = GetSelectedPaymentMethod();
            receipt.AppendLine("        PAYMENT METHOD");
            receipt.AppendLine("        ───────────────────────────────────────");
            receipt.AppendLine($"        Payment Type: {paymentMethod}");

            if (paymentMethod == "Cash" && !string.IsNullOrEmpty(txtCashReceived.Text))
            {
                receipt.AppendLine($"Amount Tendered: ₱{txtCashReceived.Text}");
                receipt.AppendLine($"Change: {lblChange.Text}");
            }
            else if (paymentMethod == "GCash" && !string.IsNullOrEmpty(txtGcashRef.Text))
            {
                receipt.AppendLine($"Reference No: {txtGcashRef.Text}");
            }

            receipt.AppendLine("        ═══════════════════════════════════════");
            receipt.AppendLine();
            receipt.AppendLine("              Thank you for playing!");
            receipt.AppendLine("              Please visit us again!");
            receipt.AppendLine();
            receipt.AppendLine("        This is a PREVIEW only. No payment");
            receipt.AppendLine("        has been processed yet.");
            receipt.AppendLine("        ═══════════════════════════════════════");

            rtbPreview.Text = receipt.ToString();

            Button btnClose = new Button();
            btnClose.Text = "Close Preview";
            btnClose.Dock = DockStyle.Bottom;
            btnClose.Height = 40;
            btnClose.Click += (s, ev) => previewForm.Close();

            previewForm.Controls.Add(rtbPreview);
            previewForm.Controls.Add(btnClose);

            previewForm.ShowDialog();
        }


        // ============ RECEIPT GENERATION ============
        private void GenerateReceiptPDF(
            string paymentMethod,
            string cashReceived,
            decimal change,
            string gcashRef)
        {
            string receiptNo = "MPGH-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string folderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "MatchPointReceipts"
            );
            Directory.CreateDirectory(folderPath);
            string filePath = Path.Combine(folderPath, receiptNo + ".pdf");

            Document document = new Document(new Rectangle(226.77f, 546.93f));
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.SetMargins(10f, 10f, 10f, 10f);
            document.Open();

            Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
            Font subHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
            Font smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);
            Font totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);

            // HEADER
            Paragraph title = new Paragraph("MATCH POINT", headerFont);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            Paragraph subtitle = new Paragraph("GAMING HUB", subHeaderFont);
            subtitle.Alignment = Element.ALIGN_CENTER;
            document.Add(subtitle);

            Paragraph address = new Paragraph("123 Gaming Street, City\nTel: (02) 1234-5678", smallFont);
            address.Alignment = Element.ALIGN_CENTER;
            document.Add(address);

            document.Add(new Paragraph(" "));

            Paragraph officialReceipt = new Paragraph("OFFICIAL RECEIPT", boldFont);
            officialReceipt.Alignment = Element.ALIGN_CENTER;
            document.Add(officialReceipt);

            document.Add(new Paragraph("═══════════════════════════", normalFont));

            document.Add(new Paragraph($"Receipt No: {receiptNo}", normalFont));
            document.Add(new Paragraph($"Date: {DateTime.Now:MM/dd/yyyy hh:mm tt}", normalFont));
            document.Add(new Paragraph($"Cashier: {Environment.UserName}", normalFont));

            document.Add(new Paragraph("═══════════════════════════", normalFont));
            document.Add(new Paragraph(" "));

            // TRANSACTION DETAILS
            Paragraph transactionHeader = new Paragraph("TRANSACTION DETAILS", boldFont);
            document.Add(transactionHeader);
            document.Add(new Paragraph("───────────────────────────", smallFont));

            // Items table
            PdfPTable itemsTable = new PdfPTable(3);
            itemsTable.WidthPercentage = 100;
            itemsTable.SetWidths(new float[] { 2f, 1.5f, 1.5f });
            itemsTable.DefaultCell.Border = Rectangle.NO_BORDER;
            itemsTable.DefaultCell.PaddingBottom = 3f;

            PdfPCell headerCell1 = new PdfPCell(new Phrase("Game", boldFont));
            headerCell1.Border = Rectangle.NO_BORDER;
            headerCell1.PaddingBottom = 5f;
            itemsTable.AddCell(headerCell1);

            PdfPCell headerCell2 = new PdfPCell(new Phrase("Duration", boldFont));
            headerCell2.Border = Rectangle.NO_BORDER;
            headerCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell2.PaddingBottom = 5f;
            itemsTable.AddCell(headerCell2);

            PdfPCell headerCell3 = new PdfPCell(new Phrase("Amount", boldFont));
            headerCell3.Border = Rectangle.NO_BORDER;
            headerCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            headerCell3.PaddingBottom = 5f;
            itemsTable.AddCell(headerCell3);

            foreach (var session in _sessions.Values)
            {
                string duration = session.TotalMinutes >= 60
                    ? $"{session.TotalMinutes / 60} hr"
                    : $"{session.TotalMinutes} min";

                PdfPCell nameCell = new PdfPCell(new Phrase(session.GameName, normalFont));
                nameCell.Border = Rectangle.NO_BORDER;
                nameCell.PaddingBottom = 2f;
                itemsTable.AddCell(nameCell);

                PdfPCell timeCell = new PdfPCell(new Phrase(duration, normalFont));
                timeCell.Border = Rectangle.NO_BORDER;
                timeCell.HorizontalAlignment = Element.ALIGN_CENTER;
                timeCell.PaddingBottom = 2f;
                itemsTable.AddCell(timeCell);

                PdfPCell amountCell = new PdfPCell(new Phrase("₱" + session.TotalPrice.ToString("N2"), normalFont));
                amountCell.Border = Rectangle.NO_BORDER;
                amountCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                amountCell.PaddingBottom = 2f;
                itemsTable.AddCell(amountCell);
            }

            document.Add(itemsTable);
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("───────────────────────────", smallFont));

            // TOTALS
            PdfPTable totalsTable = new PdfPTable(2);
            totalsTable.WidthPercentage = 100;
            totalsTable.SetWidths(new float[] { 3f, 2f });
            totalsTable.DefaultCell.Border = Rectangle.NO_BORDER;
            totalsTable.DefaultCell.PaddingBottom = 3f;

            totalsTable.AddCell(new Phrase("Subtotal:", normalFont));
            PdfPCell subtotalCell = new PdfPCell(new Phrase("₱" + subtotalAmount.ToString("N2"), normalFont));
            subtotalCell.Border = Rectangle.NO_BORDER;
            subtotalCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            totalsTable.AddCell(subtotalCell);

            if (discountAmount > 0)
            {
                string discountLabel = $"Discount ({cboDiscountType.SelectedItem}):";
                totalsTable.AddCell(new Phrase(discountLabel, normalFont));
                PdfPCell discountCell = new PdfPCell(new Phrase("-₱" + discountAmount.ToString("N2"), normalFont));
                discountCell.Border = Rectangle.NO_BORDER;
                discountCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalsTable.AddCell(discountCell);
            }

            document.Add(totalsTable);
            document.Add(new Paragraph("═══════════════════════════", normalFont));

            // TOTAL
            PdfPTable totalTable = new PdfPTable(2);
            totalTable.WidthPercentage = 100;
            totalTable.SetWidths(new float[] { 3f, 2f });
            totalTable.DefaultCell.Border = Rectangle.NO_BORDER;
            totalTable.DefaultCell.PaddingTop = 3f;
            totalTable.DefaultCell.PaddingBottom = 3f;

            PdfPCell totalLabelCell = new PdfPCell(new Phrase("TOTAL AMOUNT DUE:", totalFont));
            totalLabelCell.Border = Rectangle.NO_BORDER;
            totalLabelCell.PaddingTop = 3f;
            totalLabelCell.PaddingBottom = 3f;
            totalTable.AddCell(totalLabelCell);

            PdfPCell totalAmountCell = new PdfPCell(new Phrase("₱" + finalAmount.ToString("N2"), totalFont));
            totalAmountCell.Border = Rectangle.NO_BORDER;
            totalAmountCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            totalAmountCell.PaddingTop = 3f;
            totalAmountCell.PaddingBottom = 3f;
            totalTable.AddCell(totalAmountCell);

            document.Add(totalTable);
            document.Add(new Paragraph("═══════════════════════════", normalFont));
            document.Add(new Paragraph(" "));

            // PAYMENT INFO
            document.Add(new Paragraph("PAYMENT METHOD", boldFont));
            document.Add(new Paragraph("───────────────────────────", smallFont));
            document.Add(new Paragraph($"Payment Type: {paymentMethod}", normalFont));

            if (paymentMethod == "Cash")
            {
                document.Add(new Paragraph($"Amount Tendered: ₱{cashReceived}", normalFont));
                document.Add(new Paragraph($"Change: ₱{change.ToString("N2")}", normalFont));
            }
            else if (paymentMethod == "GCash")
            {
                document.Add(new Paragraph($"Reference No: {gcashRef}", normalFont));
            }

            document.Add(new Paragraph("═══════════════════════════", normalFont));
            document.Add(new Paragraph(" "));

            // FOOTER
            Paragraph thankYou = new Paragraph("Thank you for playing!", boldFont);
            thankYou.Alignment = Element.ALIGN_CENTER;
            document.Add(thankYou);

            Paragraph visitAgain = new Paragraph("Please visit us again!", normalFont);
            visitAgain.Alignment = Element.ALIGN_CENTER;
            document.Add(visitAgain);

            document.Add(new Paragraph(" "));

            Paragraph footer = new Paragraph("This serves as your official receipt.\nPlease keep for your records.", smallFont);
            footer.Alignment = Element.ALIGN_CENTER;
            document.Add(footer);

            document.Close();

            System.Diagnostics.Process.Start(filePath);
        }
    }

}