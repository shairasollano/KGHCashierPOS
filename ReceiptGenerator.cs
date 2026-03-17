using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace KGHCashierPOS
{
    public static class ReceiptGenerator
    {
        public static string GenerateReceipt(ReceiptData receipt)
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

            // Header
            AddHeader(document, receiptNo, headerFont, subHeaderFont, boldFont, normalFont, smallFont);

            // Items
            AddItems(document, receipt.Sessions, normalFont, boldFont, smallFont);

            // Totals
            AddTotals(document, receipt, normalFont, boldFont, totalFont);

            // Payment Info
            AddPaymentInfo(document, receipt, normalFont, boldFont, smallFont);

            // Footer
            AddFooter(document, boldFont, normalFont, smallFont);

            document.Close();

            return filePath;
        }

        private static void AddHeader(Document doc, string receiptNo, Font header, Font subHeader, Font bold, Font normal, Font small)
        {
            Paragraph title = new Paragraph("MATCH POINT", header);
            title.Alignment = Element.ALIGN_CENTER;
            doc.Add(title);

            Paragraph subtitle = new Paragraph("GAMING HUB", subHeader);
            subtitle.Alignment = Element.ALIGN_CENTER;
            doc.Add(subtitle);

            Paragraph address = new Paragraph("123 Gaming Street, City\nTel: (02) 1234-5678", small);
            address.Alignment = Element.ALIGN_CENTER;
            doc.Add(address);

            doc.Add(new Paragraph(" "));

            Paragraph receipt = new Paragraph("OFFICIAL RECEIPT", bold);
            receipt.Alignment = Element.ALIGN_CENTER;
            doc.Add(receipt);

            doc.Add(new Paragraph("═══════════════════════════", normal));

            doc.Add(new Paragraph($"Receipt No: {receiptNo}", normal));
            doc.Add(new Paragraph($"Date: {DateTime.Now:MM/dd/yyyy hh:mm tt}", normal));
            doc.Add(new Paragraph($"Cashier: {Environment.UserName}", normal));

            doc.Add(new Paragraph("═══════════════════════════", normal));
            doc.Add(new Paragraph(" "));
        }

        private static void AddItems(Document doc, Dictionary<string, GameSession> sessions, Font normal, Font bold, Font small)
        {
            Paragraph header = new Paragraph("TRANSACTION DETAILS", bold);
            doc.Add(header);
            doc.Add(new Paragraph("───────────────────────────", small));

            PdfPTable table = new PdfPTable(3);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 2f, 1.5f, 1.5f });
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.DefaultCell.PaddingBottom = 3f;

            // Headers
            PdfPCell h1 = new PdfPCell(new Phrase("Game", bold));
            h1.Border = Rectangle.NO_BORDER;
            table.AddCell(h1);

            PdfPCell h2 = new PdfPCell(new Phrase("Duration", bold));
            h2.Border = Rectangle.NO_BORDER;
            h2.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(h2);

            PdfPCell h3 = new PdfPCell(new Phrase("Amount", bold));
            h3.Border = Rectangle.NO_BORDER;
            h3.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(h3);

            // Items
            foreach (var session in sessions.Values)
            {
                string duration = DurationFormatter.Format(session.TotalMinutes);

                PdfPCell c1 = new PdfPCell(new Phrase(session.GameName, normal));
                c1.Border = Rectangle.NO_BORDER;
                table.AddCell(c1);

                PdfPCell c2 = new PdfPCell(new Phrase(duration, normal));
                c2.Border = Rectangle.NO_BORDER;
                c2.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(c2);

                PdfPCell c3 = new PdfPCell(new Phrase(PriceFormatter.Format(session.TotalPrice), normal));
                c3.Border = Rectangle.NO_BORDER;
                c3.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(c3);
            }

            doc.Add(table);
            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph("───────────────────────────", small));
        }

        private static void AddTotals(Document doc, ReceiptData receipt, Font normal, Font bold, Font total)
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 3f, 2f });
            table.DefaultCell.Border = Rectangle.NO_BORDER;

            table.AddCell(new Phrase("Subtotal:", normal));
            PdfPCell subtotal = new PdfPCell(new Phrase(PriceFormatter.Format(receipt.Subtotal), normal));
            subtotal.Border = Rectangle.NO_BORDER;
            subtotal.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(subtotal);

            if (receipt.DiscountAmount > 0)
            {
                table.AddCell(new Phrase($"Discount ({receipt.DiscountType}):", normal));
                PdfPCell discount = new PdfPCell(new Phrase("-" + PriceFormatter.Format(receipt.DiscountAmount), normal));
                discount.Border = Rectangle.NO_BORDER;
                discount.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(discount);
            }

            doc.Add(table);
            doc.Add(new Paragraph("═══════════════════════════", normal));

            // Total
            PdfPTable totalTable = new PdfPTable(2);
            totalTable.WidthPercentage = 100;
            totalTable.SetWidths(new float[] { 3f, 2f });
            totalTable.DefaultCell.Border = Rectangle.NO_BORDER;

            PdfPCell label = new PdfPCell(new Phrase("TOTAL AMOUNT DUE:", total));
            label.Border = Rectangle.NO_BORDER;
            totalTable.AddCell(label);

            PdfPCell amount = new PdfPCell(new Phrase(PriceFormatter.Format(receipt.FinalAmount), total));
            amount.Border = Rectangle.NO_BORDER;
            amount.HorizontalAlignment = Element.ALIGN_RIGHT;
            totalTable.AddCell(amount);

            doc.Add(totalTable);
            doc.Add(new Paragraph("═══════════════════════════", normal));
            doc.Add(new Paragraph(" "));
        }

        private static void AddPaymentInfo(Document doc, ReceiptData receipt, Font normal, Font bold, Font small)
        {
            doc.Add(new Paragraph("PAYMENT METHOD", bold));
            doc.Add(new Paragraph("───────────────────────────", small));
            doc.Add(new Paragraph($"Payment Type: {receipt.PaymentMethod}", normal));

            if (receipt.PaymentMethod == "Cash")
            {
                doc.Add(new Paragraph($"Amount Tendered: {PriceFormatter.Format(receipt.CashReceived)}", normal));
                doc.Add(new Paragraph($"Change: {PriceFormatter.Format(receipt.Change)}", normal));
            }
            else if (receipt.PaymentMethod == "GCash")
            {
                doc.Add(new Paragraph($"Reference No: {receipt.GCashReference}", normal));
            }

            doc.Add(new Paragraph("═══════════════════════════", normal));
            doc.Add(new Paragraph(" "));
        }

        private static void AddFooter(Document doc, Font bold, Font normal, Font small)
        {
            Paragraph thanks = new Paragraph("Thank you for playing!", bold);
            thanks.Alignment = Element.ALIGN_CENTER;
            doc.Add(thanks);

            Paragraph visit = new Paragraph("Please visit us again!", normal);
            visit.Alignment = Element.ALIGN_CENTER;
            doc.Add(visit);

            doc.Add(new Paragraph(" "));

            Paragraph footer = new Paragraph("This serves as your official receipt.\nPlease keep for your records.", small);
            footer.Alignment = Element.ALIGN_CENTER;
            doc.Add(footer);
        }
    }

    // ============ RECEIPT DATA CLASS ============
    public class ReceiptData
    {
        public Dictionary<string, GameSession> Sessions { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountType { get; set; }
        public decimal FinalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public decimal CashReceived { get; set; }
        public decimal Change { get; set; }
        public string GCashReference { get; set; }
    }
}