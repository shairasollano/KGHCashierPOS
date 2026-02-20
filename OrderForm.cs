using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace KGHCashierPOS
{
    public partial class OrderForm : Form
    {
        // ============ VARIABLES ============
        private string currentOrderNumber = "";
        private List<OrderItem> orderItems = new List<OrderItem>();
        private string selectedGameType = "";
        private int selectedDuration = 30; // Default 30 minutes
        private decimal totalAmount = 0;

        // Game rates per hour
        private Dictionary<string, decimal> gameRates = new Dictionary<string, decimal>
        {
            { "Billiards", 150.00m },
            { "Scooter", 100.00m },
            { "Badminton", 120.00m },
            { "Table Tennis", 130.00m }
        };

        // ============ CONSTRUCTOR ============
        public OrderForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        // ============ INITIALIZATION ============
        private void InitializeForm()
        {
            GenerateOrderNumber();
            SetDefaultDuration();
            UpdateTotalDisplay();

            // Set default game button colors
            ResetGameButtonColors();

            // Highlight default duration (30 minutes)
            btn30min.BackColor = Color.Orange;
            btn1hour.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            

            // Display current date and time
            lblDate1.Text = DateTime.Now.ToString("MM/dd/yyyy");
            lblTime1.Text = DateTime.Now.ToString("hh:mm tt");

            // Start timer to update time
            Timer timeTimer = new Timer();
            timeTimer.Interval = 1000; // Update every second
            timeTimer.Tick += (s, ev) => lblTime1.Text = DateTime.Now.ToString("hh:mm tt");
            timeTimer.Start();
        }

        private void GenerateOrderNumber()
        {
            currentOrderNumber = "ORD-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            lblOrderNum.Text = "Order #: " + currentOrderNumber;
        }

        private void SetDefaultDuration()
        {
            selectedDuration = 30;
        }

        // ============ GAME SELECTION BUTTONS ============
        private void btnBilliards_Click(object sender, EventArgs e)
        {
            SelectGame("Billiards", btnBilliards);
        }

        private void btnScooter_Click(object sender, EventArgs e)
        {
            SelectGame("Scooter", btnScooter);
        }

        private void btnBadminton_Click(object sender, EventArgs e)
        {
            SelectGame("Badminton", btnBadminton);
        }

        private void btnTableTennis_Click(object sender, EventArgs e)
        {
            SelectGame("Table Tennis", btnTableTennis);
        }

        private void SelectGame(string gameName, Button clickedButton)
        {
            selectedGameType = gameName;

            // Reset all game button colors
            ResetGameButtonColors();

            // Highlight selected game
            clickedButton.BackColor = Color.Orange;

            // Auto-add game to order
            AddGameToOrder();
        }

        private void ResetGameButtonColors()
        {
            btnBilliards.BackColor = Color.FromArgb(64, 64, 64);
            btnScooter.BackColor = Color.FromArgb(64, 64, 64);
            btnBadminton.BackColor = Color.FromArgb(64, 64, 64);
            btnTableTennis.BackColor = Color.FromArgb(64, 64, 64);
        }

        // ============ DURATION SELECTION BUTTONS ============
        private void btn30min_Click(object sender, EventArgs e)
        {
            selectedDuration = 30;

            btn30min.BackColor = Color.Orange;
            btn1hour.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void btn1hour_Click(object sender, EventArgs e)
        {
            selectedDuration = 60;

            btn1hour.BackColor = Color.Orange;
            btn30min.BackColor = Color.FromArgb(64, 64, 64);
        }

        // ============ ADD GAME TO ORDER ============
        private void AddGameToOrder()
        {
            // Validate game selection
            if (string.IsNullOrEmpty(selectedGameType))
            {
                MessageBox.Show("Please select a game first!", "No Game Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Calculate price
            decimal price = CalculatePrice(selectedGameType, selectedDuration);

            // Create order item
            OrderItem item = new OrderItem
            {
                OrderNumber = currentOrderNumber,
                CustomerName = txtName.Text.Trim(),
                GameName = selectedGameType,
                Duration = selectedDuration,
                Price = price
            };

            // Add to list
            orderItems.Add(item);

            // Display in ListBox
            UpdateListBoxDisplay();

            // Update total
            UpdateTotalDisplay();

            // Show confirmation
            string durationText = selectedDuration >= 60 ? "1 hour" : "30 minutes";
            MessageBox.Show($"{selectedGameType} ({durationText}) added!\nPrice: ₱{price:N2}",
                "Game Added", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Reset game selection for next item
            ResetGameButtonColors();
            selectedGameType = "";
        }

        // ============ UPDATE LISTBOX DISPLAY ============
        private void UpdateListBoxDisplay()
        {
            lbDisplay.Items.Clear();

            // Add header
            lbDisplay.Items.Add("═══════════════════════════════════════════════");
            lbDisplay.Items.Add($"ORDER NUMBER: {currentOrderNumber}");
            lbDisplay.Items.Add($"CUSTOMER: {txtName.Text.Trim()}");
            lbDisplay.Items.Add($"AGE: {txtAge.Text.Trim()}");
            lbDisplay.Items.Add($"CONTACT: {txtContact.Text.Trim()}");
            lbDisplay.Items.Add("═══════════════════════════════════════════════");
            lbDisplay.Items.Add("");
            lbDisplay.Items.Add("GAMES ORDERED:");
            lbDisplay.Items.Add("───────────────────────────────────────────────");

            // Add items
            int itemNumber = 1;
            foreach (var item in orderItems)
            {
                string duration = item.Duration >= 60 ? $"{item.Duration / 60} hr" : $"{item.Duration} min";

                lbDisplay.Items.Add($"{itemNumber}. {item.GameName}");
                lbDisplay.Items.Add($"   Duration: {duration}");
                lbDisplay.Items.Add($"   Price: ₱{item.Price:N2}");
                lbDisplay.Items.Add("");

                itemNumber++;
            }

            lbDisplay.Items.Add("───────────────────────────────────────────────");
            lbDisplay.Items.Add($"TOTAL AMOUNT: ₱{totalAmount:N2}");
            lbDisplay.Items.Add("═══════════════════════════════════════════════");
        }

        // ============ CALCULATE PRICE ============
        private decimal CalculatePrice(string gameName, int minutes)
        {
            if (!gameRates.ContainsKey(gameName))
                return 0;

            decimal hourlyRate = gameRates[gameName];
            decimal hours = minutes / 60.0m;

            return hourlyRate * hours;
        }

        // ============ UPDATE TOTAL DISPLAY ============
        private void UpdateTotalDisplay()
        {
            totalAmount = 0;

            foreach (var item in orderItems)
            {
                totalAmount += item.Price;
            }

            lblTotalValue.Text = "₱" + totalAmount.ToString("N2");
        }

        // ============ REMOVE BUTTON ============
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbDisplay.SelectedIndex >= 0)
            {
                // Find which item number was selected
                // This is a simplified approach - remove last item
                DialogResult result = MessageBox.Show(
                    "Remove the last item from order?",
                    "Remove Item",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes && orderItems.Count > 0)
                {
                    orderItems.RemoveAt(orderItems.Count - 1);
                    UpdateListBoxDisplay();
                    UpdateTotalDisplay();

                    MessageBox.Show("Item removed from order!", "Item Removed",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (orderItems.Count > 0)
            {
                // No selection, ask to clear all
                DialogResult result = MessageBox.Show(
                    "Clear all items from order?",
                    "Clear Order",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    ClearOrder();
                }
            }
            else
            {
                MessageBox.Show("No items to remove!", "Empty Order",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ============ PAY TO CASHIER BUTTON ============
        private void btnPayCashier_Click(object sender, EventArgs e)
        {
            // Validate order
            if (orderItems.Count == 0)
            {
                MessageBox.Show("Please add at least one game to your order!", "No Games Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate customer information
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter customer name!", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContact.Text))
            {
                MessageBox.Show("Please enter contact number!", "Missing Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContact.Focus();
                return;
            }

            try
            {
                // Save order to database
                SaveOrderToDatabase();

                // Generate order slip PDF
                GenerateOrderSlipPDF();

                // Show confirmation
                MessageBox.Show(
                    $"Order submitted successfully!\n\n" +
                    $"Order Number: {currentOrderNumber}\n" +
                    $"Customer: {txtName.Text}\n" +
                    $"Total Items: {orderItems.Count}\n" +
                    $"Total Amount: ₱{totalAmount:N2}\n\n" +
                    "Order slip has been generated.\n" +
                    "Please proceed to the cashier for payment!",
                    "Order Submitted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Clear form for next customer
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting order: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============ SAVE ORDER TO DATABASE ============
        private void SaveOrderToDatabase()
        {
            using (var conn = new MySqlConnection(Database.ConnectionString))
            {
                conn.Open();

                // Save customer order
                string orderQuery = @"
                    INSERT INTO orders 
                    (order_number, customer_name, customer_age, customer_contact, 
                     total_amount, order_date, status)
                    VALUES 
                    (@orderNo, @name, @age, @contact, @total, NOW(), 'Pending')";

                using (var cmd = new MySqlCommand(orderQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@orderNo", currentOrderNumber);
                    cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@age", string.IsNullOrEmpty(txtAge.Text) ?
                        (object)DBNull.Value : txtAge.Text.Trim());
                    cmd.Parameters.AddWithValue("@contact", txtContact.Text.Trim());
                    cmd.Parameters.AddWithValue("@total", totalAmount);

                    cmd.ExecuteNonQuery();
                }

                // Save order items
                foreach (var item in orderItems)
                {
                    string itemQuery = @"
                        INSERT INTO order_items 
                        (order_number, game_name, duration_minutes, price)
                        VALUES 
                        (@orderNo, @game, @duration, @price)";

                    using (var cmd = new MySqlCommand(itemQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderNo", currentOrderNumber);
                        cmd.Parameters.AddWithValue("@game", item.GameName);
                        cmd.Parameters.AddWithValue("@duration", item.Duration);
                        cmd.Parameters.AddWithValue("@price", item.Price);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // ============ GENERATE ORDER SLIP PDF ============
        private void GenerateOrderSlipPDF()
        {
            string folderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "MatchPointOrders"
            );
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"{currentOrderNumber}.pdf");

            Document document = new Document(new iTextSharp.text.Rectangle(226.77f, 566.93f));
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.SetMargins(10f, 10f, 10f, 10f);
            document.Open();

            iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
            iTextSharp.text.Font subHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            iTextSharp.text.Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            iTextSharp.text.Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9);
            iTextSharp.text.Font smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);

            // HEADER
            iTextSharp.text.Paragraph title = new iTextSharp.text.Paragraph("MATCH POINT", headerFont);
            title.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            document.Add(title);

            iTextSharp.text.Paragraph subtitle = new iTextSharp.text.Paragraph("GAMING HUB", subHeaderFont);
            subtitle.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            document.Add(subtitle);

            document.Add(new iTextSharp.text.Paragraph(" "));

            iTextSharp.text.Paragraph orderSlipHeader = new iTextSharp.text.Paragraph("ORDER SLIP", boldFont);
            orderSlipHeader.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            document.Add(orderSlipHeader);

            document.Add(new iTextSharp.text.Paragraph("═══════════════════════════", normalFont));

            // ORDER INFO
            document.Add(new iTextSharp.text.Paragraph($"Order No: {currentOrderNumber}", normalFont));
            document.Add(new iTextSharp.text.Paragraph($"Date: {DateTime.Now:MM/dd/yyyy hh:mm tt}", normalFont));
            document.Add(new iTextSharp.text.Paragraph($"Customer: {txtName.Text.Trim()}", normalFont));

            if (!string.IsNullOrEmpty(txtAge.Text))
                document.Add(new iTextSharp.text.Paragraph($"Age: {txtAge.Text.Trim()}", normalFont));

            document.Add(new iTextSharp.text.Paragraph($"Contact: {txtContact.Text.Trim()}", normalFont));

            document.Add(new iTextSharp.text.Paragraph("═══════════════════════════", normalFont));
            document.Add(new iTextSharp.text.Paragraph(" "));

            // GAMES ORDERED
            document.Add(new iTextSharp.text.Paragraph("GAMES ORDERED:", boldFont));
            document.Add(new iTextSharp.text.Paragraph("───────────────────────────", smallFont));
            document.Add(new iTextSharp.text.Paragraph(" "));

            int itemNo = 1;
            foreach (var item in orderItems)
            {
                string duration = item.Duration >= 60 ? $"{item.Duration / 60} hr" : $"{item.Duration} min";

                document.Add(new iTextSharp.text.Paragraph($"{itemNo}. {item.GameName}", normalFont));
                document.Add(new iTextSharp.text.Paragraph($"   Duration: {duration}", normalFont));
                document.Add(new iTextSharp.text.Paragraph($"   Price: ₱{item.Price:N2}", normalFont));
                document.Add(new iTextSharp.text.Paragraph(" "));

                itemNo++;
            }

            document.Add(new iTextSharp.text.Paragraph("───────────────────────────", smallFont));

            // TOTAL
            iTextSharp.text.Paragraph totalPara = new iTextSharp.text.Paragraph(
                $"TOTAL AMOUNT: ₱{totalAmount:N2}", boldFont);
            totalPara.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
            document.Add(totalPara);

            document.Add(new iTextSharp.text.Paragraph("═══════════════════════════", normalFont));
            document.Add(new iTextSharp.text.Paragraph(" "));

            // FOOTER
            iTextSharp.text.Paragraph instruction = new iTextSharp.text.Paragraph(
                "Please present this order number\nto the cashier for payment.", normalFont);
            instruction.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            document.Add(instruction);

            document.Add(new iTextSharp.text.Paragraph(" "));

            iTextSharp.text.Paragraph thankYou = new iTextSharp.text.Paragraph(
                "Thank you!", boldFont);
            thankYou.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            document.Add(thankYou);

            document.Close();

            // Open the PDF
            System.Diagnostics.Process.Start(filePath);
        }

        // ============ CLEAR ORDER ============
        private void ClearOrder()
        {
            orderItems.Clear();
            lbDisplay.Items.Clear();
            totalAmount = 0;
            lblTotalValue.Text = "₱0.00";

            MessageBox.Show("Order cleared!", "Order Cleared",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ============ CLEAR FORM ============
        private void ClearForm()
        {
            // Clear customer info
            txtName.Clear();
            txtAge.Clear();
            txtContact.Clear();

            // Clear order
            orderItems.Clear();
            lbDisplay.Items.Clear();

            // Reset buttons
            ResetGameButtonColors();
            btn30min.BackColor = Color.Orange;
            btn1hour.BackColor = Color.FromArgb(64, 64, 64);

            // Reset variables
            selectedGameType = "";
            selectedDuration = 30;
            totalAmount = 0;

            // Update display
            lblTotalValue.Text = "₱0.00";

            // Generate new order number
            GenerateOrderNumber();

            // Focus on name field
            txtName.Focus();
        }
    }

    // ============ ORDER ITEM CLASS ============
    public class OrderItem
    {
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string GameName { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
    }
}