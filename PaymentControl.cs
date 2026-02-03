using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using GHCashierPOS; 


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

        // =========================
        // CONFIRM PAYMENT
        // =========================
        private void btnConfirmPayment_Click(object sender, EventArgs e)
        {
            if (_totalAmount <= 0)
            {
                MessageBox.Show("No transaction to process.");
                return;
            }

            // CASH PAYMENT
            if (!string.IsNullOrWhiteSpace(txtCashReceived.Text))
            {
                if (!decimal.TryParse(txtCashReceived.Text, out decimal cash))
                {
                    MessageBox.Show("Invalid cash amount.");
                    return;
                }

                if (cash < _totalAmount)
                {
                    MessageBox.Show("Insufficient cash.");
                    return;
                }

                decimal change = cash - _totalAmount;
                lblChange.Text = "₱" + change.ToString("0.00");

                GenerateReceiptPDF("Cash", cash.ToString("0.00"), change, "");
            }
            // GCASH PAYMENT
            else if (!string.IsNullOrWhiteSpace(txtGCashRef.Text))
            {
                GenerateReceiptPDF("GCash", "0.00", 0.00m, txtGCashRef.Text);
            }
            else
            {
                MessageBox.Show("Please enter Cash or GCash reference.");
                return;
            }

            MessageBox.Show("Payment successful!\nReceipt saved.");

            // Clear after payment
            _sessions.Clear();
            lvSummary.Items.Clear();

            this.Visible = false;
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

            Document document = new Document(PageSize.A6);
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();

            // HEADER
            document.Add(new Paragraph("Match Point Gaming Hub")
            {
                Alignment = Element.ALIGN_CENTER
            });

            document.Add(new Paragraph("OFFICIAL RECEIPT")
            {
                Alignment = Element.ALIGN_CENTER
            });

            document.Add(new Paragraph("--------------------------------"));
            document.Add(new Paragraph($"Receipt No: {receiptNo}"));
            document.Add(new Paragraph($"Date: {DateTime.Now}"));
            document.Add(new Paragraph($"Payment Method: {paymentMethod}"));

            if (paymentMethod == "GCash")
                document.Add(new Paragraph($"GCash Ref: {gcashRef}"));

            document.Add(new Paragraph("--------------------------------"));

            // ITEMS
            foreach (ListViewItem item in lvSummary.Items)
            {
                document.Add(new Paragraph(
                    $"{item.Text} | {item.SubItems[1].Text} | {item.SubItems[2].Text}"
                ));
            }

            document.Add(new Paragraph("--------------------------------"));
            document.Add(new Paragraph("TOTAL: " + lblTotalAmount.Text));

            if (paymentMethod == "Cash")
            {
                document.Add(new Paragraph("Cash: ₱" + cashReceived));
                document.Add(new Paragraph("Change: ₱" + change.ToString("0.00")));
            }

            document.Add(new Paragraph("\nThank you for playing!")
            {
                Alignment = Element.ALIGN_CENTER
            });

            document.Close();
        }

    }
}
