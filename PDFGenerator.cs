using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace KGHCashierPOS
{
    public static class PDFGenerator
    {
        public static void GenerateOrderSlip(string orderNumber, List<OrderItem> items, decimal total)
        {
            string folderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "MatchPointOrders"
            );
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"{orderNumber}.pdf");

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                Document doc = new Document(PageSize.A5);
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

                // Header
                Paragraph title = new Paragraph("MATCH POINT GAMING HUB", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);
                doc.Add(new Paragraph("\n"));

                // Order info
                doc.Add(new Paragraph($"Order: {orderNumber}", normalFont));
                doc.Add(new Paragraph($"Date: {DateTime.Now:MMMM dd, yyyy hh:mm tt}", normalFont));
                doc.Add(new Paragraph("\n" + new string('-', 50) + "\n"));

                // Items
                int num = 1;
                foreach (var item in items)
                {
                    doc.Add(new Paragraph($"{num}. {item.GameName} - {DurationFormatter.Format(item.Duration)}", normalFont));
                    doc.Add(new Paragraph($"   Game: {PriceFormatter.Format(item.GamePrice)}", normalFont));

                    if (item.EquipmentCost > 0)
                    {
                        doc.Add(new Paragraph($"   Equipment: {PriceFormatter.Format(item.EquipmentCost)}", normalFont));
                    }

                    doc.Add(new Paragraph($"   Total: {PriceFormatter.Format(item.TotalPrice)}", normalFont));
                    doc.Add(new Paragraph("\n"));
                    num++;
                }

                doc.Add(new Paragraph(new string('-', 50)));
                doc.Add(new Paragraph($"TOTAL: {PriceFormatter.Format(total)}", boldFont));

                doc.Close();
            }

            // Open PDF
            System.Diagnostics.Process.Start(filePath);
        }

        public static void GenerateReceipt(string receiptNo, string paymentMethod,
            List<GameSession> sessions, decimal total, decimal change)
        {
            string folderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "MatchPointReceipts"
            );
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"{receiptNo}.pdf");

            // Receipt generation logic here...
        }
    }
}