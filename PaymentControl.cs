
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

        private Dictionary<string, GameSession> _sessions;
        private decimal _totalAmount;
       
        public paymentControl1()
        {
            InitializeComponent();
        }

        private void PaymentControl1_Load(object sender, EventArgs e)
        {

        }


        // =========================
        // LOAD PAYMENT DATA
        // =========================
        public void LoadPaymentData(
            Dictionary<string, GameSession> sessions,
            decimal total)
        {
            _sessions = sessions;
            _totalAmount = total;

            lvSummary.Items.Clear();

            foreach (var s in sessions.Values)
            {
                string duration =
                    s.TotalMinutes >= 60
                    ? $"{s.TotalMinutes / 60} hr"
                    : $"{s.TotalMinutes} min";

                ListViewItem item = new ListViewItem(s.GameName);
                item.SubItems.Add(duration);
                item.SubItems.Add("₱" + s.TotalPrice.ToString("0.00"));

                lvSummary.Items.Add(item);
            }

            lblTotalAmount.Text = "₱" + total.ToString("0.00");
            lblChange.Text = "₱0.00";

            txtCashReceived.Clear();
            txtGCashRef.Clear();
        }

        /*
         * 
         * Dictionary<string, GameSession> sessions,
        decimal total)
        {
        _activeSessions = sessions;   // 👈 SAVE IT HERE

        lvSummary.Items.Clear();

        foreach (var s in sessions.Values)
        {
        string duration =
            s.TotalMinutes >= 60
            ? $"{s.TotalMinutes / 60} hr"
            : $"{s.TotalMinutes} min";

        ListViewItem item = new ListViewItem(s.GameName);
        item.SubItems.Add(duration);
        item.SubItems.Add("₱" + s.TotalPrice.ToString("0.00"));

        lvSummary.Items.Add(item);
        }

        lblTotalAmount.Text = "₱" + total.ToString("0.00");

         */

        // =========================
        // CONFIRM PAYMENT
        // =========================
        private void btnConfirmPayment_Click(object sender, EventArgs e)
        {
            decimal total = decimal.Parse(
            lblTotalAmount.Text.Replace("₱", "")
        );

            string method = "";
            string refInfo = "";

            if (!string.IsNullOrWhiteSpace(txtCashReceived.Text))
            {
                decimal cash = decimal.Parse(txtCashReceived.Text);

                if (cash < total)
                {
                    MessageBox.Show("Insufficient cash.");
                    return;
                }

                lblChange.Text = "₱" + (cash - total).ToString("0.00");
                method = "Cash";
                refInfo = cash.ToString();
            }
            else if (!string.IsNullOrWhiteSpace(txtGCashRef.Text))
            {
                method = "GCash";
                refInfo = txtGCashRef.Text;
            }
            else
            {
                MessageBox.Show("Select payment method.");
                return;
            }

            // DISCOUNT DEFAULT

            cmbDiscountType.SelectedIndex = 0;
            lblDiscountAmount.Text = "₱0.00";

            decimal discountedTotal = ApplyDiscount(_totalAmount);

            decimal discountAmount = 0;
            string discountType = "None";

            if (cmbDiscountType.SelectedItem.ToString() == "Senior" ||
                cmbDiscountType.SelectedItem.ToString() == "PWD")
            {
                discountType = cmbDiscountType.SelectedItem.ToString();
                discountAmount = _totalAmount * 0.20m;
            }

            decimal finalAmount = _totalAmount - discountAmount;

            string receiptNo = "MPGH-" + DateTime.Now.ToString("yyyyMMddHHmmss");




            // SAVE DATA

            foreach (var session in _sessions.Values)
            {
                int sessionId = SaveSession(session);
                SavePayment(
                sessionId,
                method,
                _totalAmount,     // amount paid before discount
                discountAmount,   // discount value
                finalAmount,      // final amount after discount
                discountType,     // Senior / PWD / None
                receiptNo,        // receipt number
                refInfo           // cash amount or GCash ref
            );
            }

            if (method == "Cash")
            {
                GenerateReceiptPDF(
                    "Cash",
                    txtCashReceived.Text,
                    decimal.Parse(lblChange.Text.Replace("₱", "")),
                    ""
                );
            }
            else
            {
                GenerateReceiptPDF(
                    "GCash",
                    "",
                    0,
                    txtGCashRef.Text
                );
            }


            MessageBox.Show("Payment successful!");
            this.Visible = false;
        }

        // METHOD: APPLY DISCOUNT

        private decimal ApplyDiscount(decimal total)
        {
            if (cmbDiscountType.SelectedItem.ToString() == "Senior" ||
                cmbDiscountType.SelectedItem.ToString() == "PWD")
            {
                decimal discount = total * 0.20m;
                lblDiscountAmount.Text = "₱" + discount.ToString("0.00");
                return total - discount;
            }

            lblDiscountAmount.Text = "₱0.00";
            return total;
        }

        // DATABASE: SAVE SESSION

        private int SaveSession(GameSession session)
        {
            int sessionId = 0;

            using (var conn = new MySql.Data.MySqlClient.MySqlConnection(
                Database.ConnectionString))
            {
                conn.Open();

                string query = @"
            INSERT INTO sessions
            (game_name, start_time, end_time, total_minutes, total_price, status)
            VALUES
            (@game, @start, @end, @minutes, @price, 'Completed');
            SELECT LAST_INSERT_ID();";

                var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@game", session.GameName);
                cmd.Parameters.AddWithValue("@start", session.StartTime);
                cmd.Parameters.AddWithValue("@end", session.EndTime);
                cmd.Parameters.AddWithValue("@minutes", session.TotalMinutes);
                cmd.Parameters.AddWithValue("@price", session.TotalPrice);

                sessionId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return sessionId;
        }

        // SAVE PAYMENT

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




        // =========================
        // CANCEL PAYMENT
        // =========================
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }



        // =========================
        // GENERATE PDF RECEIPT
        // =========================
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

            // Use smaller page size for thermal receipt look
            Document document = new Document(new Rectangle(226.77f, 516.93f)); // 80mm x 200mm
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.SetMargins(10f, 10f, 10f, 10f);
            document.Open();

            // Define fonts
            Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
            Font subHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
            Font smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);
            Font totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);

            // ============ HEADER ============
            Paragraph title = new Paragraph("MATCH POINT", headerFont);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            Paragraph subtitle = new Paragraph("GAMING HUB", subHeaderFont);
            subtitle.Alignment = Element.ALIGN_CENTER;
            document.Add(subtitle);

            Paragraph address = new Paragraph("123 Gaming Street, City\nTel: (02) 1234-5678", smallFont);
            address.Alignment = Element.ALIGN_CENTER;
            document.Add(address);

            document.Add(new Paragraph(" ")); // Space

            Paragraph officialReceipt = new Paragraph("OFFICIAL RECEIPT", boldFont);
            officialReceipt.Alignment = Element.ALIGN_CENTER;
            document.Add(officialReceipt);

            // Separator line
            document.Add(new Paragraph("═══════════════════════════", normalFont));

            // Receipt details
            document.Add(new Paragraph($"Receipt No: {receiptNo}", normalFont));
            document.Add(new Paragraph($"Date: {DateTime.Now:MM/dd/yyyy hh:mm tt}", normalFont));
            document.Add(new Paragraph($"Cashier: {Environment.UserName}", normalFont));

            document.Add(new Paragraph("═══════════════════════════", normalFont));
            document.Add(new Paragraph(" ")); // Space

            // ============ TRANSACTION DETAILS ============
            Paragraph transactionHeader = new Paragraph("TRANSACTION DETAILS", boldFont);
            document.Add(transactionHeader);
            document.Add(new Paragraph("───────────────────────────", smallFont));

            // Create table for items
            PdfPTable itemsTable = new PdfPTable(3);
            itemsTable.WidthPercentage = 100;
            itemsTable.SetWidths(new float[] { 2f, 1.5f, 1.5f });
            itemsTable.DefaultCell.Border = Rectangle.NO_BORDER;
            itemsTable.DefaultCell.PaddingBottom = 3f;

            // Table headers
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

            decimal subtotal = 0;

            // Add items from ListView
            foreach (ListViewItem item in lvSummary.Items)
            {
                // Game name
                PdfPCell nameCell = new PdfPCell(new Phrase(item.Text, normalFont));
                nameCell.Border = Rectangle.NO_BORDER;
                nameCell.PaddingBottom = 2f;
                itemsTable.AddCell(nameCell);

                // Duration/Time
                PdfPCell timeCell = new PdfPCell(new Phrase(item.SubItems[1].Text, normalFont));
                timeCell.Border = Rectangle.NO_BORDER;
                timeCell.HorizontalAlignment = Element.ALIGN_CENTER;
                timeCell.PaddingBottom = 2f;
                itemsTable.AddCell(timeCell);

                // Amount
                string amountText = item.SubItems[2].Text;
                decimal amount = decimal.Parse(amountText.Replace("₱", "").Trim());
                subtotal += amount;

                PdfPCell amountCell = new PdfPCell(new Phrase(amountText, normalFont));
                amountCell.Border = Rectangle.NO_BORDER;
                amountCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                amountCell.PaddingBottom = 2f;
                itemsTable.AddCell(amountCell);
            }

            document.Add(itemsTable);
            document.Add(new Paragraph(" ")); // Space

            document.Add(new Paragraph("───────────────────────────", smallFont));

            // ============ TOTALS SECTION ============
            PdfPTable totalsTable = new PdfPTable(2);
            totalsTable.WidthPercentage = 100;
            totalsTable.SetWidths(new float[] { 3f, 2f });
            totalsTable.DefaultCell.Border = Rectangle.NO_BORDER;
            totalsTable.DefaultCell.PaddingBottom = 3f;

            // Subtotal
            totalsTable.AddCell(new Phrase("Subtotal:", normalFont));
            PdfPCell subtotalCell = new PdfPCell(new Phrase("₱" + subtotal.ToString("N2"), normalFont));
            subtotalCell.Border = Rectangle.NO_BORDER;
            subtotalCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            totalsTable.AddCell(subtotalCell);

            // Total Amount Due
            decimal totalAmount = subtotal;
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

            PdfPCell totalAmountCell = new PdfPCell(new Phrase("₱" + totalAmount.ToString("N2"), totalFont));
            totalAmountCell.Border = Rectangle.NO_BORDER;
            totalAmountCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            totalAmountCell.PaddingTop = 3f;
            totalAmountCell.PaddingBottom = 3f;
            totalTable.AddCell(totalAmountCell);

            document.Add(totalTable);
            document.Add(new Paragraph("═══════════════════════════", normalFont));
            document.Add(new Paragraph(" ")); // Space

            // ============ PAYMENT INFORMATION ============
            document.Add(new Paragraph("PAYMENT METHOD", boldFont));
            document.Add(new Paragraph("───────────────────────────", smallFont));
            document.Add(new Paragraph($"Payment Type: {paymentMethod}", normalFont));

            if (paymentMethod == "Cash")
            {
                document.Add(new Paragraph($"Amount Tendered: ₱{cashReceived}", normalFont));
                document.Add(new Paragraph($"Change: ₱{change.ToString("N2")}", normalFont));
            }
            else if (paymentMethod == "GCash" || paymentMethod == "PayMaya")
            {
                document.Add(new Paragraph($"Reference No: {gcashRef}", normalFont));
            }

            document.Add(new Paragraph("═══════════════════════════", normalFont));
            document.Add(new Paragraph(" ")); // Space

            // ============ FOOTER ============
            Paragraph thankYou = new Paragraph("Thank you for playing!", boldFont);
            thankYou.Alignment = Element.ALIGN_CENTER;
            document.Add(thankYou);

            Paragraph visitAgain = new Paragraph("Please visit us again!", normalFont);
            visitAgain.Alignment = Element.ALIGN_CENTER;
            document.Add(visitAgain);

            document.Close();

            // Optional: Open the PDF automatically
            System.Diagnostics.Process.Start(filePath);
        }

    }
}
