using MySql.Data.MySqlClient;
using System.Drawing;

namespace KGHCashierPOS
{
    using MySqlX.XDevAPI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;


    public partial class CashierForm : Form
    {
        private paymentControl1 paymentControl1;

        Dictionary<string, GameSession> activeSessions =
          new Dictionary<string, GameSession>();

        string selectedGame = "";
        decimal totalAmount = 0;

        Dictionary<string, (decimal min30, decimal hour1)> priceList =
            new Dictionary<string, (decimal, decimal)>()
            {
            { "Billiards", (80, 150) },
            { "Scooter", (100, 150) },
            { "Badminton", (50, 90) },
            { "Table Tennis", (40, 75) }
            };

        private paymentControl1 paymentControl;

        public CashierForm()
        {
            InitializeComponent();

            paymentControl = new paymentControl1();
            paymentControl.Visible = false;
            paymentControl.PaymentSuccessful += OnPaymentSuccessful;

            this.Controls.Add(paymentControl);
        }





        // GAME BUTTON CLICK EVENTS

        private void btnBilliards_Click(object sender, EventArgs e)
        {
            selectedGame = "Billiards";
            btnBilliards.BackColor = Color.FromArgb(233, 190, 95);
        }
        private void btnScooter_Click(object sender, EventArgs e)
        {
            selectedGame = "Scooter";
            btnScooter.BackColor = Color.FromArgb(233, 190, 95);
        }
        private void btnBadminton_Click(object sender, EventArgs e)
        {
            selectedGame = "Badminton";
            btnBadminton.BackColor = Color.FromArgb(233, 190, 95);
        }
        private void btnTableTennis_Click(object sender, EventArgs e)
        {
            selectedGame = "Table Tennis";
            btnTableTennis.BackColor = Color.FromArgb(233, 190, 95);
        }

        // DURATION BUTTON CLICK EVENTS

        private void btn30Min_Click(object sender, EventArgs e)
        {
            AddDurationToGame(30);
            btn30min.BackColor = Color.FromArgb(233, 190, 95);
        }
        private void btn1Hour_Click(object sender, EventArgs e)
        {
            AddDurationToGame(60);
            btn1hour.BackColor = Color.FromArgb(233, 190, 95);
        }


        // ADD TIME

        private void AddDurationToGame(int minutes)
        {
            if (selectedGame == "")
            {
                MessageBox.Show("Please select a game first.");
                return;
            }

            decimal priceToAdd =
                minutes == 30
                ? priceList[selectedGame].min30
                : priceList[selectedGame].hour1;

            // If game already exists → extend
            if (activeSessions.ContainsKey(selectedGame))
            {
                activeSessions[selectedGame].TotalMinutes += minutes;
                activeSessions[selectedGame].TotalPrice += priceToAdd;
            }
            else
            {
                activeSessions[selectedGame] = new GameSession
                {
                    GameName = selectedGame,
                    TotalMinutes = minutes,
                    TotalPrice = priceToAdd,
                    StartTime = DateTime.Now
                };
            }

            RefreshListView();
        }

        // REFRESH LISTVIEW

        private void RefreshListView()
        {
            lvSelectedGames.Items.Clear();
            totalAmount = 0;

            foreach (var session in activeSessions.Values)
            {
                // FIX: Use FormatDuration method instead of inline formatting
                string durationText = FormatDuration(session.TotalMinutes);

                // ADDED TIME 3 MINUTE INCREMENT
                session.StartTime = DateTime.Now.AddMinutes(3);
                session.EndTime = session.StartTime.AddMinutes(session.TotalMinutes);
                session.IsActive = true;

                ListViewItem item = new ListViewItem(session.GameName);
                item.SubItems.Add(durationText);
                item.SubItems.Add("₱" + session.TotalPrice.ToString("0.00"));
                item.SubItems.Add(session.StartTime.ToString("hh:mm tt"));
                item.SubItems.Add(session.EndTime.ToString("hh:mm tt"));
                item.SubItems.Add("₱" + session.TotalPrice.ToString("0.00"));

                lvSelectedGames.Items.Add(item);

                totalAmount += session.TotalPrice;
            }

            lblTotal.Text = "₱ " + totalAmount.ToString("0.00");
        }

        private string FormatDuration(int totalMinutes)
        {
            if (totalMinutes < 60)
            {
                // Less than 1 hour: "30 min"
                return $"{totalMinutes} min";
            }
            else
            {
                int hours = totalMinutes / 60;
                int minutes = totalMinutes % 60;

                if (minutes == 0)
                {
                    // Exact hours: "1 hr", "2 hr"
                    return $"{hours} hr";
                }
                else
                {
                    // Hours + minutes: "1 hr 30 min", "2 hr 30 min"
                    return $"{hours} hr {minutes} min";
                }
            }
        }


        // REMOVE SELECTED GAME
        private void btnRemoveGame_Click(object sender, EventArgs e)
        {
            if (lvSelectedGames.SelectedItems.Count == 0)
                return;

            string gameName = lvSelectedGames.SelectedItems[0].Text;

            activeSessions.Remove(gameName);
            RefreshListView();
        }

        // FUNCTIONS NG BUTTONS SA ORDER KEYPAD

        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            txtOrderNumber.Text += btn.Text;
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (txtOrderNumber.Text.Length > 0)
                txtOrderNumber.Text =
                    txtOrderNumber.Text.Substring(0, txtOrderNumber.Text.Length - 1);
        }

        // ENTER BUTTON TO LOAD ORDER FROM DATABASE UPDATED
        private void btnEnter_Click(object sender, EventArgs e)
        {
            string orderNumber = txtOrderNumber.Text.Trim();

            if (string.IsNullOrEmpty(orderNumber))
            {
                MessageBox.Show("Please enter an order number!", "No Order Number",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOrderNumber.Focus();
                return;
            }

            // Remove any non-numeric characters
            orderNumber = new string(orderNumber.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(orderNumber))
            {
                MessageBox.Show("Invalid order number format!", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOrderNumber.Clear();
                txtOrderNumber.Focus();
                return;
            }

            // Pad with leading zeros if user typed shorter number
            if (orderNumber.Length < 6)
            {
                orderNumber = orderNumber.PadLeft(6, '0');
            }

            // Update textbox to show formatted number
            txtOrderNumber.Text = orderNumber;

            // Load the order
            LoadOrderFromDatabase(orderNumber);
        }

        private void LoadOrderFromDatabase(string orderNumber)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    // Check if order exists and is pending
                    string checkQuery = @"
                SELECT COUNT(*) 
                FROM orders 
                WHERE order_number = @orderNo AND status = 'Pending'";

                    using (var cmd = new MySqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderNo", orderNumber);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 0)
                        {
                            MessageBox.Show(
                                $"Order #{orderNumber} not found or already processed!",
                                "Invalid Order",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            txtOrderNumber.Clear();
                            txtOrderNumber.Focus();
                            return;
                        }
                    }

                    // Load order items
                    string itemsQuery = @"
                SELECT game_name, duration_minutes, price 
                FROM order_items 
                WHERE order_number = @orderNo";

                    using (var cmd = new MySqlCommand(itemsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderNo", orderNumber);

                        using (var reader = cmd.ExecuteReader())
                        {
                            // Clear existing items
                            lvSelectedGames.Items.Clear();

                            int itemCount = 0;
                            while (reader.Read())
                            {
                                string gameName = reader.GetString("game_name");
                                int duration = reader.GetInt32("duration_minutes");
                                decimal price = reader.GetDecimal("price");

                                // Add to ListView
                                ListViewItem item = new ListViewItem(gameName);
                                item.SubItems.Add(duration >= 60 ? $"{duration / 60} hr" : $"{duration} min");
                                item.SubItems.Add("₱" + price.ToString("N2"));

                                lvSelectedGames.Items.Add(item);
                                itemCount++;
                            }

                            if (itemCount > 0)
                            {
                                // Calculate and update total
                                UpdateTotalAmount();

                                MessageBox.Show(
                                    $"Order #{orderNumber} loaded successfully!\n" +
                                    $"Items: {itemCount}\n" +
                                    $"Total: {lblTotal.Text}",
                                    "Order Loaded",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTotalAmount()
        {
            decimal total = 0;

            foreach (ListViewItem item in lvSelectedGames.Items)
            {
                string priceText = item.SubItems[2].Text.Replace("₱", "").Replace(",", "").Trim();
                if (decimal.TryParse(priceText, out decimal price))
                {
                    total += price;
                }
            }

            lblTotal.Text = "₱" + total.ToString("N2");
        }

        // PROCEED TO PAYMENT

       private void btnProceedPayment_Click(object sender, EventArgs e)
{
    // Validate that there are items
    if (lvSelectedGames.Items.Count == 0)
    {
        MessageBox.Show("Please add games to the order first!", "No Items",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    // Create GameSession dictionary from ListView
    Dictionary<string, GameSession> sessions = new Dictionary<string, GameSession>();
    decimal total = 0;
    int sessionCounter = 1;

            foreach(ListViewItem item in lvSelectedGames.Items)
{
                // Parse the ListView data
                string gameName = item.Text;
                string durationText = item.SubItems[1].Text;
                string priceText = item.SubItems[2].Text;

                // Parse duration - FIXED VERSION
                int totalMinutes = 0;

                if (durationText.Contains("hr"))
                {
                    // Declare hourIndex here
                    int hrIndex = durationText.IndexOf("hr");
                    string hourPart = durationText.Substring(0, hrIndex).Trim();

                    if (int.TryParse(hourPart, out int hours))
                    {
                        totalMinutes += hours * 60;
                    }

                    // Check for minutes in the SAME if block
                    if (durationText.Contains("min"))
                    {
                        string afterHr = durationText.Substring(hrIndex + 2);
                        string minPart = afterHr.Replace("min", "").Trim();

                        if (int.TryParse(minPart, out int minutes))
                        {
                            totalMinutes += minutes;
                        }
                    }
                }
                else if (durationText.Contains("min"))
                {
                    // Only minutes, no hours
                    string minPart = durationText.Replace("min", "").Trim();
                    if (int.TryParse(minPart, out int minutes))
                    {
                        totalMinutes = minutes;
                    }
                }

                // Parse price
                decimal price = 0;
        if (priceText.StartsWith("₱"))
        {
            string cleanPrice = priceText.Replace("₱", "").Replace(",", "").Trim();
            decimal.TryParse(cleanPrice, out price);
        }

        // Create GameSession
        GameSession session = new GameSession
        {
            GameName = gameName,
            //
            StartTime = DateTime.Now, // You might want to store actual start time
            EndTime = DateTime.Now.AddMinutes(totalMinutes),
            TotalMinutes = totalMinutes,
            TotalPrice = price,
            //
        };

        sessions.Add($"session_{sessionCounter}", session);
        total += price;
        sessionCounter++;
        }



        // Validate sessions were created
        if (sessions.Count == 0)
        {
            MessageBox.Show("Error creating payment sessions!", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Show payment control
        paymentControl1.Visible = true;
        paymentControl1.BringToFront();

        // Load payment data
        paymentControl1.LoadPaymentData(sessions, total);

    
        }

        public void ResetTransaction()
        {
            txtOrderNumber.Clear();
            lvSelectedGames.Items.Clear();
            lblTotal.Text = "₱0.00";
            activeSessions.Clear();
            totalAmount = 0;
            selectedGame = "";
            txtOrderNumber.Focus();
        }

        private void btnClearCashierForm_Click_1(object sender, EventArgs e)
        {
            ResetTransaction();
        }
        private void OnPaymentSuccessful()
        {
            ResetTransaction();
        }

        /* OrderForm orderForm = new OrderForm();
            orderForm.ShowDialog(); 
        
         paymentControl1 : UserControl*/

        public void ClosePayment()
        {
            paymentControl.Visible = false;
            
        }

        private void CashierForm_Load(object sender, EventArgs e)
        {
            UpdateDateTime();        
            timerDateTime.Start();   
        }


        private void timerDateTime_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            lblDate.Text = DateTime.Now.ToString("MMMM dd, yyyy"); 
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");   
        }
    }
}
