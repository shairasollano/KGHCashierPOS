using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace KGHCashierPOS
{
    public static class ReceiptGenerator
    {
        // Tax rate (12% VAT in Philippines)
        private const decimal TAX_RATE = 0.12m;

        // Fixed width for thermal receipt (80mm = 226.77 points)
        private const float RECEIPT_WIDTH = 226.77f;

        public static string GenerateReceipt(ReceiptData receipt)
        {
            string receiptNo = "MPGH-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string folderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "MatchPointReceipts"
            );

            Directory.CreateDirectory(folderPath);
            string filePath = Path.Combine(folderPath, receiptNo + ".pdf");

            // ⭐ STEP 1: Calculate required height based on content
            float requiredHeight = CalculateReceiptHeight(receipt);

            // ⭐ STEP 2: Create document with calculated height (not fixed!)
            Document document = new Document(
                new Rectangle(RECEIPT_WIDTH, requiredHeight),
                10f, 10f, 10f, 10f  // margins: left, right, top, bottom
            );

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                writer.PageEvent = null; // Ensure no headers/footers interfere
                document.SetPageSize(new Rectangle(RECEIPT_WIDTH, requiredHeight));

                // Fonts
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                Font subHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
                Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);
                Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);
                Font smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 7);
                Font tinyFont = FontFactory.GetFont(FontFactory.HELVETICA, 6);

                // ============ HEADER ============
                AddCenteredText(document, "MATCH POINT", headerFont);
                AddCenteredText(document, "GAMING HUB", subHeaderFont);
                AddCenteredText(document, "143 St. Caloocan City, Metro Manila", smallFont);
                AddCenteredText(document, "Tel: (03) 8143-6577", smallFont);

                document.Add(new Paragraph(" ", tinyFont));
                AddDashedLine(document, normalFont);
               

                // ============ RECEIPT INFO ============
                AddLeftAlignedText(document, $"Receipt No: {receiptNo}", normalFont);
                AddLeftAlignedText(document, $"Date: {DateTime.Now:MMM dd, yyyy hh:mm tt}", normalFont);
                AddLeftAlignedText(document, $"Cashier: ", normalFont); // {UserSession.Username ?? Environment.UserName}

                AddDashedLine(document, normalFont);

                // ============ ITEMS ============
                AddLeftAlignedText(document, "ITEMS:", boldFont);
                document.Add(new Paragraph(" ", tinyFont));

                foreach (var session in receipt.Sessions.Values)
                {
                    string duration = DurationFormatter.Format(session.TotalMinutes);

                    // Item name
                    AddLeftAlignedText(document, session.GameName, normalFont);

                    // Duration and game price
                    PdfPTable itemTable = new PdfPTable(2);
                    itemTable.WidthPercentage = 100;
                    itemTable.SetWidths(new float[] { 70f, 30f });
                    itemTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    itemTable.DefaultCell.PaddingBottom = 0f;
                    itemTable.DefaultCell.PaddingTop = 0f;

                    PdfPCell durationCell = new PdfPCell(new Phrase($"  {duration}", smallFont));
                    durationCell.Border = Rectangle.NO_BORDER;
                    durationCell.PaddingLeft = 5f;
                    itemTable.AddCell(durationCell);

                    PdfPCell priceCell = new PdfPCell(new Phrase($"{session.TotalPrice:N2}", normalFont));
                    priceCell.Border = Rectangle.NO_BORDER;
                    priceCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    itemTable.AddCell(priceCell);

                    document.Add(itemTable);

                    // ⭐ ADD EQUIPMENT DETAILS TO RECEIPT
                    if (session.Equipment != null && session.Equipment.Count > 0)
                    {
                        foreach (var eq in session.Equipment)
                        {
                            if (eq.RentalQuantity > 0)
                            {
                                PdfPTable equipTable = new PdfPTable(2);
                                equipTable.WidthPercentage = 100;
                                equipTable.SetWidths(new float[] { 70f, 30f });
                                equipTable.DefaultCell.Border = Rectangle.NO_BORDER;

                                PdfPCell equipNameCell = new PdfPCell(new Phrase($"    + {eq.Name} x{eq.RentalQuantity}", smallFont));
                                equipNameCell.Border = Rectangle.NO_BORDER;
                                equipNameCell.PaddingLeft = 10f;
                                equipTable.AddCell(equipNameCell);

                                PdfPCell equipPriceCell = new PdfPCell(new Phrase($"{eq.TotalCost:N2}", smallFont));
                                equipPriceCell.Border = Rectangle.NO_BORDER;
                                equipPriceCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                equipTable.AddCell(equipPriceCell);

                                document.Add(equipTable);
                            }
                        }

                        // Equipment subtotal if there's equipment cost
                        if (session.EquipmentCost > 0)
                        {
                            PdfPTable equipTotalTable = new PdfPTable(2);
                            equipTotalTable.WidthPercentage = 100;
                            equipTotalTable.SetWidths(new float[] { 70f, 30f });
                            equipTotalTable.DefaultCell.Border = Rectangle.NO_BORDER;

                            PdfPCell equipLabelCell = new PdfPCell(new Phrase("  Equipment:", smallFont));
                            equipLabelCell.Border = Rectangle.NO_BORDER;
                            equipLabelCell.PaddingLeft = 5f;
                            equipTotalTable.AddCell(equipLabelCell);

                            PdfPCell equipAmountCell = new PdfPCell(new Phrase($"{session.EquipmentCost:N2}", smallFont));
                            equipAmountCell.Border = Rectangle.NO_BORDER;
                            equipAmountCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            equipTotalTable.AddCell(equipAmountCell);

                            document.Add(equipTotalTable);
                        }
                    }
                }

                document.Add(new Paragraph(" ", tinyFont));
                AddDashedLine(document, normalFont);

                // ============ CALCULATIONS ============
                decimal subtotalBeforeTax = receipt.Subtotal / (1 + TAX_RATE);
                decimal vatAmount = receipt.Subtotal - subtotalBeforeTax;
                decimal discountedSubtotal = receipt.Subtotal - receipt.DiscountAmount;
                decimal discountedBeforeTax = discountedSubtotal / (1 + TAX_RATE);
                decimal finalVat = discountedSubtotal - discountedBeforeTax;

                // Subtotal
                AddPriceLine(document, "Subtotal:", receipt.Subtotal, normalFont, false);

                // Discount
                if (receipt.DiscountAmount > 0)
                {
                    AddPriceLine(document, $"Discount ({receipt.DiscountType}):", -receipt.DiscountAmount, normalFont, false);
                    AddPriceLine(document, "Subtotal after discount:", discountedSubtotal, normalFont, false);
                }

                document.Add(new Paragraph(" ", tinyFont));

                // Tax breakdown
                AddLeftAlignedText(document, "VAT Breakdown:", boldFont);
                AddPriceLine(document, "  VATable Sale:", discountedBeforeTax, smallFont, false);
                AddPriceLine(document, "  VAT (12%):", finalVat, smallFont, false);

                document.Add(new Paragraph(" ", tinyFont));
                AddSolidLine(document, normalFont);

                // ============ TOTAL ============
                PdfPTable totalTable = new PdfPTable(2);
                totalTable.WidthPercentage = 100;
                totalTable.SetWidths(new float[] { 60f, 40f });
                totalTable.DefaultCell.Border = Rectangle.NO_BORDER;
                totalTable.DefaultCell.PaddingTop = 2f;
                totalTable.DefaultCell.PaddingBottom = 2f;

                PdfPCell totalLabelCell = new PdfPCell(new Phrase("TOTAL AMOUNT DUE:", boldFont));
                totalLabelCell.Border = Rectangle.NO_BORDER;
                totalTable.AddCell(totalLabelCell);

                PdfPCell totalAmountCell = new PdfPCell(new Phrase($"₱ {receipt.FinalAmount:N2}", boldFont));
                totalAmountCell.Border = Rectangle.NO_BORDER;
                totalAmountCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                totalTable.AddCell(totalAmountCell);

                document.Add(totalTable);
                AddSolidLine(document, normalFont);

                // ============ PAYMENT DETAILS ============
                AddLeftAlignedText(document, "PAYMENT DETAILS:", boldFont);
                AddLeftAlignedText(document, $"Payment Method: {receipt.PaymentMethod}", normalFont);

                if (receipt.PaymentMethod == "Cash")
                {
                    AddPriceLine(document, "Cash Tendered:", receipt.CashReceived, normalFont, false);
                    AddPriceLine(document, "Change:", receipt.Change, normalFont, false);
                }
                else if (receipt.PaymentMethod == "GCash")
                {
                    AddLeftAlignedText(document, $"GCash Ref: {receipt.GCashReference}", normalFont);
                }

                AddDashedLine(document, normalFont);

                // ============ TRANSACTION TIMES ============
                if (receipt.Sessions.Count > 0)
                {
                    AddLeftAlignedText(document, "SESSION TIMES:", boldFont);

                    foreach (var session in receipt.Sessions.Values)
                    {
                        AddLeftAlignedText(document, $"{session.GameName}:", normalFont);
                        AddLeftAlignedText(document, $"  Start: {session.StartTime:hh:mm tt}", smallFont);
                        AddLeftAlignedText(document, $"  End:   {session.EndTime:hh:mm tt}", smallFont);
                    }

                    AddDashedLine(document, normalFont);
                }

                // ============ FOOTER ============
                document.Add(new Paragraph(" ", tinyFont));
                AddCenteredText(document, "Thank you for choosing", normalFont);
                AddCenteredText(document, "MATCH POINT GAMING HUB!", boldFont);
                AddCenteredText(document, "Please come again!", normalFont);

                document.Add(new Paragraph(" ", tinyFont));
                AddCenteredText(document, "This serves as your official receipt.", tinyFont);

                document.Add(new Paragraph(" ", tinyFont));
                AddDashedLine(document, normalFont);

                AddCenteredText(document, "For concerns and feedback:", tinyFont);
                AddCenteredText(document, "matchpoint@email.com", tinyFont);
                AddCenteredText(document, "www.matchpointgaming.com", tinyFont);

                document.Add(new Paragraph(" ", tinyFont));
                AddCenteredText(document, $"Printed: {DateTime.Now:MMM dd, yyyy hh:mm tt}", tinyFont);

                document.Close();

                System.Diagnostics.Debug.WriteLine($"✓ Receipt generated: {filePath} (Height: {requiredHeight}pt)");
                return filePath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Receipt generation error: {ex.Message}");
                throw;
            }
        }

        // ⭐ NEW METHOD: Calculate required height based on content
        private static float CalculateReceiptHeight(ReceiptData receipt)
        {
            // 1. Fixed Elements (Points)
            float height = 0f;
            height += 100f; // Header (Title, Address, Tel)
            height += 60f;  // Receipt Info (No, Date, Cashier)
            height += 40f;  // "ITEMS:" header
            height += 110f; // Calculations (Subtotal, VAT Breakdown)
            height += 60f;  // Total Amount Section
            height += 60f;  // Payment Details (Method, Cash, Change)
            height += 160f; // Footer (Thank you, Email, Website, Printed Date)
            height += 20f;  // Extra bottom padding

            // 2. Dynamic Session Items
            foreach (var session in receipt.Sessions.Values)
            {
                height += 15f; // Game Name line
                height += 15f; // Duration/Price line

                // 3. Dynamic Equipment Items (The part that was missing!)
                if (session.Equipment != null)
                {
                    foreach (var eq in session.Equipment)
                    {
                        if (eq.RentalQuantity > 0)
                        {
                            height += 12f; // Each equipment line
                        }
                    }
                    if (session.EquipmentCost > 0) height += 12f; // Equipment subtotal line
                }

                height += 10f; // Spacer between items
            }

            // 4. Dynamic Session Times (at the bottom)
            if (receipt.Sessions.Count > 0)
            {
                height += 30f; // "SESSION TIMES:" header
                foreach (var session in receipt.Sessions.Values)
                {
                    height += 45f; // Game Name + Start Time + End Time
                }
            }

            // 5. Discount section
            if (receipt.DiscountAmount > 0)
            {
                height += 40f;
            }

            // Add a 5% buffer for line spacing/scaling
            return height + (height * 0.05f);
        }

        // ============ HELPER METHODS ============
        private static void AddCenteredText(Document doc, string text, Font font)
        {
            Paragraph p = new Paragraph(text, font);
            p.Alignment = Element.ALIGN_CENTER;
            p.SpacingAfter = 2f;
            doc.Add(p);
        }

        private static void AddLeftAlignedText(Document doc, string text, Font font)
        {
            Paragraph p = new Paragraph(text, font);
            p.Alignment = Element.ALIGN_LEFT;
            p.SpacingAfter = 2f;
            doc.Add(p);
        }

        private static void AddPriceLine(Document doc, string label, decimal amount, Font font, bool isBold)
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 60f, 40f });
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.DefaultCell.PaddingBottom = 1f;
            table.DefaultCell.PaddingTop = 1f;
            table.SpacingAfter = 2f;

            Font useFont = isBold ? FontFactory.GetFont(FontFactory.HELVETICA_BOLD, font.Size) : font;

            PdfPCell labelCell = new PdfPCell(new Phrase(label, useFont));
            labelCell.Border = Rectangle.NO_BORDER;
            labelCell.PaddingBottom = 1f;
            table.AddCell(labelCell);

            string priceText = amount >= 0 ? $"₱ {amount:N2}" : $"-₱ {Math.Abs(amount):N2}";
            PdfPCell priceCell = new PdfPCell(new Phrase(priceText, useFont));
            priceCell.Border = Rectangle.NO_BORDER;
            priceCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            priceCell.PaddingBottom = 1f;
            table.AddCell(priceCell);

            doc.Add(table);
        }

        private static void AddDashedLine(Document doc, Font font)
        {
            Paragraph line = new Paragraph("- - - - - - - - - - - - - - - - - - - - - - - -", font);
            line.Alignment = Element.ALIGN_CENTER;
            line.SpacingAfter = 3f;
            line.SpacingBefore = 3f;
            doc.Add(line);
        }

        private static void AddSolidLine(Document doc, Font font)
        {
            Paragraph line = new Paragraph("═══════════════════════════════════", font);
            line.Alignment = Element.ALIGN_CENTER;
            line.SpacingAfter = 3f;
            line.SpacingBefore = 3f;
            doc.Add(line);
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