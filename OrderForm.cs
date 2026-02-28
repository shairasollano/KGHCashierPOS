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
        private int selectedDuration = 0;
        private decimal totalAmount = 0;

        // Game rates per hour
        private Dictionary<string, (decimal min30, decimal hour1)> priceList =
        new Dictionary<string, (decimal, decimal)>()
        {
            { "Billiards", (80, 150) },
            { "Scooter", (100, 150) },
            { "Badminton", (50, 90) },
            { "Table Tennis", (40, 75) }
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
            UpdateTotalDisplay();

            // Set default button colors
            ResetGameButtonColors();
            ResetDurationButtonColors();

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

        // Generate order num

        private void GenerateOrderNumber()
        {
            currentOrderNumber = GetNextOrderNumber();
            lblOrderNum.Text = "Order #: " + currentOrderNumber;
        }

        private string GetNextOrderNumber()
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    // Try to get the last numeric order number
                    string query = @"
                SELECT order_number 
                FROM orders 
                WHERE order_number REGEXP '^[0-9]+$'
                ORDER BY CAST(order_number AS UNSIGNED) DESC 
                LIMIT 1";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int lastNumber))
                        {
                            int nextNumber = lastNumber + 1;
                            return nextNumber.ToString("D6");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting order number: {ex.Message}");
            }

            // Start from 000001 if no orders exist
            return "000001";
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

            // Try to add game if game is selected
            TryAddGameToOrder();
        }

        private void btn1hour_Click(object sender, EventArgs e)
        {
            selectedDuration = 60;

            btn1hour.BackColor = Color.Orange;
            btn30min.BackColor = Color.FromArgb(64, 64, 64);

            // Try to add game if game is selected
            TryAddGameToOrder();
        }

        private void ResetDurationButtonColors()
        {
            btn30min.BackColor = Color.FromArgb(64, 64, 64);
            btn1hour.BackColor = Color.FromArgb(64, 64, 64);
        }

        // ============ ADD GAME TO ORDER ============
        private void TryAddGameToOrder()
        {
            // Check if both game and duration are selected
            if (string.IsNullOrEmpty(selectedGameType))
            {
                MessageBox.Show("Please select a game first!", "No Game Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedDuration == 0)
            {
                MessageBox.Show("Please select duration (30 min or 1 hour)!", "No Duration Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Both are selected, add to order
            AddGameToOrder();
        }

        // ============ UPDATE TOTAL DISPLAY ============
        private void UpdateTotalDisplay()
        {
            totalAmount = 0;

            foreach (var item in orderItems)
            {
                totalAmount += item.Price;
                System.Diagnostics.Debug.WriteLine($"Adding item: {item.GameName} - ₱{item.Price:N2}, Running total: ₱{totalAmount:N2}");
            }

            lblTotalValue.Text = "₱" + totalAmount.ToString("N2");

            System.Diagnostics.Debug.WriteLine($"=== FINAL TOTAL: ₱{totalAmount:N2} ===");
        }

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

            System.Diagnostics.Debug.WriteLine($"Calculated price for {selectedGameType} ({selectedDuration} min): ₱{price:N2}");

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

            // Update total FIRST
            UpdateTotalDisplay();

            // Display in ListBox AFTER updating total
            UpdateListBoxDisplay();

            // Show confirmation
            string durationText = selectedDuration >= 60 ? "1 hour" : "30 minutes";
            MessageBox.Show($"{selectedGameType} ({durationText}) added!\nPrice: ₱{price:N2}",
                "Game Added", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Reset selections for next item
            ResetGameButtonColors();
            ResetDurationButtonColors();
            selectedGameType = "";
            selectedDuration = 0;
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

            // Add items and calculate total
            decimal runningTotal = 0; // Calculate fresh total
            int itemNumber = 1;
            foreach (var item in orderItems)
            {
                string duration = item.Duration >= 60 ? $"{item.Duration / 60} hr" : $"{item.Duration} min";

                lbDisplay.Items.Add($"{itemNumber}. {item.GameName}");
                lbDisplay.Items.Add($"   Duration: {duration}");
                lbDisplay.Items.Add($"   Price: ₱{item.Price:N2}");
                lbDisplay.Items.Add("");

                runningTotal += item.Price; // Add to running total
                itemNumber++;
            }

            lbDisplay.Items.Add("───────────────────────────────────────────────");
            lbDisplay.Items.Add($"TOTAL AMOUNT: ₱{runningTotal:N2}"); // Use runningTotal instead of totalAmount
            lbDisplay.Items.Add("═══════════════════════════════════════════════");
        }

        // ============ CALCULATE PRICE ============
        private decimal CalculatePrice(string gameName, int minutes)
        {
            if (!priceList.ContainsKey(gameName))
            {
                System.Diagnostics.Debug.WriteLine($"WARNING: Game '{gameName}' not found in price list!");
                return 0;
            }

            var prices = priceList[gameName];
            decimal price;

            switch (minutes)
            {
                case 30:
                    price = prices.min30;
                    break;
                case 60:
                    price = prices.hour1;
                    break;
                default:
                    // Calculate proportionally for other durations
                    // Based on hourly rate
                    decimal hourlyRate = prices.hour1;
                    decimal hours = minutes / 60.0m;
                    price = hourlyRate * hours;
                    break;
            }

            System.Diagnostics.Debug.WriteLine($"Price: {gameName}, {minutes} min = ₱{price:N2}");

            return price;
        }


        // ============ REMOVE BUTTON ============
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (orderItems.Count == 0)
            {
                MessageBox.Show("No items to remove!", "Empty Order",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Remove the last item from order?",
                "Remove Item",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                orderItems.RemoveAt(orderItems.Count - 1);

                // IMPORTANT: Update total before updating display
                UpdateTotalDisplay();
                UpdateListBoxDisplay();

                MessageBox.Show("Item removed from order!", "Item Removed",
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear without confirmation (use for testing or if user prefers)
            orderItems.Clear();
            lbDisplay.Items.Clear();
            totalAmount = 0;
            lblTotalValue.Text = "₱0.00";
            ResetGameButtonColors();
            ResetDurationButtonColors();
            selectedGameType = "";
            selectedDuration = 0;

            System.Diagnostics.Debug.WriteLine("Quick clear executed");
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

            Document document = new Document(new iTextSharp.text.Rectangle(226.77f, 246.93f));
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

            // TOTAL
            iTextSharp.text.Paragraph totalPara = new iTextSharp.text.Paragraph(
                $"TOTAL AMOUNT: ₱{totalAmount:N2}", boldFont);
            totalPara.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;
            document.Add(totalPara);

            document.Add(new iTextSharp.text.Paragraph("═══════════════════════════", normalFont));
            document.Add(new iTextSharp.text.Paragraph(" "));


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