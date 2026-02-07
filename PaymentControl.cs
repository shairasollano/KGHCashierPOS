using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using KGHCashierPOS;
using iTextSharp.text;
using iTextSharp.text.pdf;

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

        // ============ CONSTRUCTOR ============
        public paymentControl1()
        {
            InitializeComponent();
            InitializeDiscountComboBox();
        }

        private void PaymentControl1_Load(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        // ============ INITIALIZATION ============
        private void InitializeDiscountComboBox()
        {
            cboDiscountType.Items.Clear();
            cboDiscountType.Items.Add("None");
            cboDiscountType.Items.Add("Senior Citizen (20%)");
            cboDiscountType.Items.Add("PWD (20%)");
            cboDiscountType.Items.Add("Member (10%)");
            cboDiscountType.Items.Add("Promo Code");
            cboDiscountType.Items.Add("Custom Amount");

            cboDiscountType.SelectedIndex = 0;
            txtDiscountAmount.Enabled = false;
        }

        // ============ LOAD PAYMENT DATA ============
        public void LoadPaymentData(Dictionary<string, GameSession> sessions, decimal total)
        {
            _sessions = sessions;
            _totalAmount = total;

            lvSummary.Items.Clear();

            foreach (var s in sessions.Values)
            {
                string duration = s.TotalMinutes >= 60
                    ? $"{s.TotalMinutes / 60} hr"
                    : $"{s.TotalMinutes} min";

                ListViewItem item = new ListViewItem(s.GameName);
                item.SubItems.Add(duration);
                item.SubItems.Add("₱" + s.TotalPrice.ToString("0.00"));

                lvSummary.Items.Add(item);
            }

            CalculateTotals();

            txtCashReceived.Clear();
            txtGcashRef.Clear();
            lblChange.Text = "₱0.00";
        }

        // ============ DISCOUNT EVENT HANDLERS ============
        private void cboDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDiscountType.SelectedItem == null)
                return;

            string selectedDiscount = cboDiscountType.SelectedItem.ToString();

            // Validate eligibility first
            if (!ValidateDiscountEligibility(selectedDiscount))
                return;

            switch (selectedDiscount)
            {
                case "None":
                    txtDiscountAmount.Enabled = false;
                    txtDiscountAmount.Clear();
                    discountAmount = 0;
                    break;

                case "Senior Citizen (20%)":
                case "PWD (20%)":
                    txtDiscountAmount.Enabled = false;
                    txtDiscountAmount.Clear();
                    ApplyPercentageDiscount(0.20m);
                    MessageBox.Show($"20% Senior/PWD discount applied!\nDiscount: ₱{discountAmount:N2}",
                        "Discount Applied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Member (10%)":
                    txtDiscountAmount.Enabled = false;
                    txtDiscountAmount.Clear();
                    ApplyPercentageDiscount(0.10m);
                    MessageBox.Show($"10% Member discount applied!\nDiscount: ₱{discountAmount:N2}",
                        "Discount Applied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "Promo Code":
                    txtDiscountAmount.Enabled = true;
                    txtDiscountAmount.Clear();
                    txtDiscountAmount.Focus();
                    discountAmount = 0;
                    break;

                case "Custom Amount":
                    txtDiscountAmount.Enabled = true;
                    txtDiscountAmount.Clear();
                    txtDiscountAmount.Focus();
                    discountAmount = 0;
                    break;
            }

            // Update the display immediately
            CalculateTotals();
        }


        private void txtDiscountAmount_TextChanged(object sender, EventArgs e)
        {
            string selectedDiscount = cboDiscountType.SelectedItem?.ToString();

            if (selectedDiscount == "Custom Amount")
            {
                if (decimal.TryParse(txtDiscountAmount.Text, out decimal customAmount))
                {
                    subtotalAmount = CalculateSubtotal();

                    if (customAmount > subtotalAmount)
                    {
                        MessageBox.Show("Discount cannot exceed subtotal amount!",
                            "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtDiscountAmount.Clear();
                        discountAmount = 0;
                    }
                    else
                    {
                        discountAmount = customAmount;
                    }
                }
                else
                {
                    discountAmount = 0;
                }

                CalculateTotals();
            }
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

        // ============ DISCOUNT CALCULATION ============
        private void ApplyPercentageDiscount(decimal percentage)
        {
            subtotalAmount = CalculateSubtotal();
            discountAmount = subtotalAmount * percentage;

            // Update label with red color to highlight discount
            if (lblDiscountAmount != null)
            {
                lblDiscountAmount.Text = "-₱" + discountAmount.ToString("N2");
                lblDiscountAmount.ForeColor = System.Drawing.Color.Red;
            }
        }
        private decimal CalculateSubtotal()
        {
            decimal subtotal = 0;

            foreach (ListViewItem item in lvSummary.Items)
            {
                if (item.SubItems.Count > 2)
                {
                    string amountText = item.SubItems[2].Text.Replace("₱", "").Replace(",", "").Trim();
                    if (decimal.TryParse(amountText, out decimal amount))
                    {
                        subtotal += amount;
                    }
                }
            }

            return subtotal;
        }

        private void CalculateTotals()
        {
            // Calculate subtotal
            subtotalAmount = CalculateSubtotal();

            // Apply discount
            finalAmount = subtotalAmount - discountAmount;

            // Update ALL labels - MAKE SURE THESE LABEL NAMES MATCH YOUR FORM
            if (lblSubtotal != null)
                lblSubtotal.Text = "₱" + subtotalAmount.ToString("N2");

            if (lblDiscountAmount != null)
                lblDiscountAmount.Text = "-₱" + discountAmount.ToString("N2");

            if (lblTotalAmount != null)
                lblTotalAmount.Text = "₱" + finalAmount.ToString("N2");

            // Also update the change if cash is entered
            if (rbCash != null && rbCash.Checked && !string.IsNullOrEmpty(txtCashReceived.Text))
            {
                if (decimal.TryParse(txtCashReceived.Text, out decimal cash))
                {
                    decimal change = cash - finalAmount;
                    lblChange.Text = change >= 0 ? "₱" + change.ToString("N2") : "Insufficient";
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
                txtCashReceived.Visible = true;
                txtCashReceived.Enabled = true;
                lblChange.Visible = true;

                txtGcashRef.Visible = false;
                txtGcashRef.Enabled = false;
            }
        }

        private void rbGCash_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGCash.Checked)
            {
                txtGcashRef.Visible = true;
                txtGcashRef.Enabled = true;

                txtCashReceived.Visible = false;
                txtCashReceived.Enabled = false;
                lblChange.Visible = false;
            }
        }

        private void txtCashReceived_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtCashReceived.Text, out decimal cashReceived))
            {
                decimal change = cashReceived - finalAmount;

                if (change >= 0)
                {
                    lblChange.Text = "₱" + change.ToString("N2");
                    btnConfirmPayment.Enabled = true;
                }
                else
                {
                    lblChange.Text = "Insufficient";
                    btnConfirmPayment.Enabled = false;
                }
            }
            else
            {
                lblChange.Text = "₱0.00";
            }
        }

        // ============ PAYMENT PROCESSING ============
        private void btnConfirmPayment_Click(object sender, EventArgs e)
        {
            string paymentMethod = GetSelectedPaymentMethod();
            string reference = "";
            decimal cashAmount = 0;

            // Validate payment method
            if (paymentMethod == "Cash")
            {
                if (string.IsNullOrWhiteSpace(txtCashReceived.Text))
                {
                    MessageBox.Show("Please enter cash received amount.", "Payment Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cashAmount = decimal.Parse(txtCashReceived.Text);

                if (cashAmount < finalAmount)
                {
                    MessageBox.Show("Insufficient cash.", "Payment Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    return;
                }

                reference = txtGcashRef.Text;
            }
            else
            {
                MessageBox.Show("Please select a payment method.", "Payment Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Show confirmation
            DialogResult result = MessageBox.Show(
                $"Total Amount: ₱{finalAmount:N2}\n" +
                $"Payment Method: {paymentMethod}\n\n" +
                "Confirm this payment?",
                "Confirm Payment",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
                return;

            // Process payment
            try
            {
                string receiptNo = "MPGH-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string discountType = cboDiscountType.SelectedItem.ToString();

                // Save to database
                foreach (var session in _sessions.Values)
                {
                    int sessionId = SaveSession(session);
                    SavePayment(
                        sessionId,
                        paymentMethod,
                        subtotalAmount,
                        discountAmount,
                        finalAmount,
                        discountType,
                        receiptNo,
                        reference
                    );
                }

                // Generate receipt
                decimal change = paymentMethod == "Cash" ? cashAmount - finalAmount : 0;
                GenerateReceiptPDF(paymentMethod, cashAmount.ToString("0.00"), change, reference);

                // Log activity
                LogActivity("Payment Processed", $"{paymentMethod} - ₱{finalAmount:N2}");

                MessageBox.Show("Payment successful!\nReceipt has been generated.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing payment: {ex.Message}", "Payment Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                     discount_amount, final_amount, receipt_no, reference_no, payment_date)
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

                // TODO: Implement CMS integration here if needed
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

        private void btnPreviewReceipt_Click(object sender, EventArgs e)
        {
            // Validate that there are items
            if (lvSummary.Items.Count == 0)
            {
                MessageBox.Show("No items to preview!", "Preview Receipt",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create preview form
            Form previewForm = new Form();
            previewForm.Text = "Receipt Preview";
            previewForm.Size = new System.Drawing.Size(400, 700);
            previewForm.StartPosition = FormStartPosition.CenterParent;
            previewForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            previewForm.MaximizeBox = false;
            previewForm.MinimizeBox = false;

            // Create RichTextBox for preview
            RichTextBox rtbPreview = new RichTextBox();
            rtbPreview.Dock = DockStyle.Fill;
            rtbPreview.Font = new System.Drawing.Font("Courier New", 9);
            rtbPreview.ReadOnly = true;
            rtbPreview.BackColor = System.Drawing.Color.White;

            // Build receipt content
            System.Text.StringBuilder receipt = new System.Text.StringBuilder();

            receipt.AppendLine("═══════════════════════════════════");
            receipt.AppendLine("         MATCH POINT GAMING HUB");
            receipt.AppendLine("═══════════════════════════════════");
            receipt.AppendLine();
            receipt.AppendLine("        RECEIPT PREVIEW");
            receipt.AppendLine();
            receipt.AppendLine($"Date: {DateTime.Now:MM/dd/yyyy hh:mm tt}");
            receipt.AppendLine($"Cashier: {Environment.UserName}");
            receipt.AppendLine("───────────────────────────────────");
            receipt.AppendLine();
            receipt.AppendLine("TRANSACTION DETAILS");
            receipt.AppendLine("───────────────────────────────────");
            receipt.AppendLine();

            // Items
            receipt.AppendLine(string.Format("{0,-18} {1,-8} {2,10}", "Item", "Time", "Amount"));
            receipt.AppendLine("───────────────────────────────────");

            foreach (ListViewItem item in lvSummary.Items)
            {
                string gameName = item.Text.Length > 18 ? item.Text.Substring(0, 15) + "..." : item.Text;
                string time = item.SubItems[1].Text;
                string amount = item.SubItems[2].Text;

                receipt.AppendLine(string.Format("{0,-18} {1,-8} {2,10}", gameName, time, amount));
            }

            receipt.AppendLine("───────────────────────────────────");
            receipt.AppendLine();

            // Totals
            receipt.AppendLine(string.Format("{0,-25} {1,12}", "Subtotal:", lblSubtotal.Text));

            if (discountAmount > 0)
            {
                string discountType = cboDiscountType.SelectedItem?.ToString() ?? "None";
                receipt.AppendLine(string.Format("{0,-25} {1,12}",
                    $"Discount ({discountType}):", lblDiscountAmount.Text));
            }

            receipt.AppendLine("═══════════════════════════════════");
            receipt.AppendLine(string.Format("{0,-25} {1,12}", "TOTAL AMOUNT DUE:", lblTotalAmount.Text));
            receipt.AppendLine("═══════════════════════════════════");
            receipt.AppendLine();

            // Payment method
            string paymentMethod = GetSelectedPaymentMethod();
            receipt.AppendLine("PAYMENT METHOD");
            receipt.AppendLine("───────────────────────────────────");
            receipt.AppendLine($"Payment Type: {paymentMethod}");

            if (paymentMethod == "Cash" && !string.IsNullOrEmpty(txtCashReceived.Text))
            {
                receipt.AppendLine($"Amount Tendered: ₱{txtCashReceived.Text}");
                receipt.AppendLine($"Change: {lblChange.Text}");
            }
            else if (paymentMethod == "GCash" && !string.IsNullOrEmpty(txtGcashRef.Text))
            {
                receipt.AppendLine($"Reference No: {txtGcashRef.Text}");
            }

            receipt.AppendLine("═══════════════════════════════════");
            receipt.AppendLine();
            receipt.AppendLine("      Thank you for playing!");
            receipt.AppendLine("      Please visit us again!");
            receipt.AppendLine();
            receipt.AppendLine("This is a PREVIEW only. No payment");
            receipt.AppendLine("has been processed yet.");
            receipt.AppendLine("═══════════════════════════════════");

            rtbPreview.Text = receipt.ToString();

            // Add close button
            Button btnClose = new Button();
            btnClose.Text = "Close Preview";
            btnClose.Dock = DockStyle.Bottom;
            btnClose.Height = 40;
            btnClose.Click += (s, ev) => previewForm.Close();

            // Add controls to form
            previewForm.Controls.Add(rtbPreview);
            previewForm.Controls.Add(btnClose);

            // Show preview
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

            Document document = new Document(new Rectangle(226.77f, 566.93f));
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

            PdfPCell headerCell1 = new PdfPCell(new Phrase("Item", boldFont));
            headerCell1.Border = Rectangle.NO_BORDER;
            headerCell1.PaddingBottom = 5f;
            itemsTable.AddCell(headerCell1);

            PdfPCell headerCell2 = new PdfPCell(new Phrase("Time", boldFont));
            headerCell2.Border = Rectangle.NO_BORDER;
            headerCell2.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell2.PaddingBottom = 5f;
            itemsTable.AddCell(headerCell2);

            PdfPCell headerCell3 = new PdfPCell(new Phrase("Amount", boldFont));
            headerCell3.Border = Rectangle.NO_BORDER;
            headerCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
            headerCell3.PaddingBottom = 5f;
            itemsTable.AddCell(headerCell3);

            foreach (ListViewItem item in lvSummary.Items)
            {
                PdfPCell nameCell = new PdfPCell(new Phrase(item.Text, normalFont));
                nameCell.Border = Rectangle.NO_BORDER;
                nameCell.PaddingBottom = 2f;
                itemsTable.AddCell(nameCell);

                PdfPCell timeCell = new PdfPCell(new Phrase(item.SubItems[1].Text, normalFont));
                timeCell.Border = Rectangle.NO_BORDER;
                timeCell.HorizontalAlignment = Element.ALIGN_CENTER;
                timeCell.PaddingBottom = 2f;
                itemsTable.AddCell(timeCell);

                PdfPCell amountCell = new PdfPCell(new Phrase(item.SubItems[2].Text, normalFont));
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

            // Show discount if applied
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

            document.Add(new Paragraph(" "));

            Paragraph powered = new Paragraph("Powered by Match Point POS System", smallFont);
            powered.Alignment = Element.ALIGN_CENTER;
            document.Add(powered);

            document.Close();

            System.Diagnostics.Process.Start(filePath);
        }

        private void btnApplyDiscount_Click(object sender, EventArgs e)
        {
            string selectedDiscount = cboDiscountType.SelectedItem?.ToString();

            if (selectedDiscount == null || selectedDiscount == "None")
            {
                MessageBox.Show("Please select a discount type first.", "Apply Discount",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // For promo code - validate
            if (selectedDiscount == "Promo Code")
            {
                if (string.IsNullOrWhiteSpace(txtDiscountAmount.Text))
                {
                    MessageBox.Show("Please enter a promo code.", "Apply Discount",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDiscountAmount.Focus();
                    return;
                }

                ValidatePromoCode(txtDiscountAmount.Text);
            }
            // For custom amount
            else if (selectedDiscount == "Custom Amount")
            {
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
                    MessageBox.Show("Discount cannot exceed subtotal amount!", "Invalid Discount",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                discountAmount = customAmount;
                CalculateTotals();

                MessageBox.Show($"Custom discount of ₱{customAmount:N2} applied!", "Discount Applied",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // For percentage discounts (already applied on selection)
            else
            {
                MessageBox.Show($"Discount already applied: {lblDiscountAmount.Text}", "Discount Applied",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}