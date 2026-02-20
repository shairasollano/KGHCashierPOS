using MySql.Data.MySqlClient;

namespace KGHCashierPOS
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;


    public partial class CashierForm : Form
    {

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
        }
        private void btnScooter_Click(object sender, EventArgs e)
        {
            selectedGame = "Scooter";
        }
        private void btnBadminton_Click(object sender, EventArgs e)
        {
            selectedGame = "Badminton";
        }
        private void btnTableTennis_Click(object sender, EventArgs e)
        {
            selectedGame = "Table Tennis";
        }

        // DURATION BUTTON CLICK EVENTS

        private void btn30Min_Click(object sender, EventArgs e)
        {
            AddDurationToGame(30);
        }
        private void btn1Hour_Click(object sender, EventArgs e)
        {
            AddDurationToGame(60);
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
                string durationText =
                    session.TotalMinutes >= 60
                    ? $"{session.TotalMinutes / 60} hr"
                    : $"{session.TotalMinutes} min";

                // ADDED TIME 3 MINUTE INCREMENT

                session.StartTime = DateTime.Now.AddMinutes(3);
                session.EndTime = session.StartTime.AddMinutes(session.TotalMinutes);
                session.IsActive = true;

                // If session is paused, calculate remaining time and update end time
                session.RemainingTime = session.EndTime - DateTime.Now;
                session.IsPaused = true;

                // When resuming, set new end time based on remaining time
                session.EndTime = DateTime.Now.Add(session.RemainingTime);
                session.IsPaused = false;


                ListViewItem item = new ListViewItem(session.GameName);
                item.SubItems.Add(durationText);
                item.SubItems.Add("₱" + session.TotalPrice.ToString("0.00"));
                item.SubItems.Add(session.StartTime.ToString("hh:mm tt"));
                item.SubItems.Add(session.EndTime.ToString("hh:mm tt"));
                item.SubItems.Add("₱" + session.TotalPrice.ToString("0.00"));

                lvSelectedGames.Items.Add(item);

                totalAmount += session.TotalPrice;
            }

            lblTotal.Text = "₱" + totalAmount.ToString("0.00");
        }


        // TIMER TICK EVENT TO CHECK FOR SESSION END
        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var s in activeSessions.Values)
            {
                if (!s.IsActive) continue;

                TimeSpan remaining = s.EndTime - DateTime.Now;

                if (remaining.TotalSeconds <= 0)
                {
                    s.IsActive = false;
                    MessageBox.Show($"{s.GameName} session ended!");
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




        // ORDER NUMBERS KEYPAD, CLEAR, AND ENTER BUTTONS

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
                return;
            }

            LoadOrderFromDatabase(orderNumber);
        }

        private void LoadOrderFromDatabase(string orderNumber)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    // Check if order exists
                    string checkQuery = "SELECT COUNT(*) FROM orders WHERE order_number = @orderNo AND status = 'Pending'";
                    using (var cmd = new MySqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderNo", orderNumber);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 0)
                        {
                            MessageBox.Show("Order not found or already processed!", "Invalid Order",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                            }
                        }
                    }

                    // Calculate and update total
                    UpdateTotalAmount();

                    MessageBox.Show($"Order {orderNumber} loaded successfully!", "Order Loaded",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                string priceText = item.SubItems[2].Text.Replace("₱", "").Trim();
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

            // Validate that there are items in the summary
            if (lvSelectedGames.Items.Count == 0)
            {
                MessageBox.Show("Please add games to the order first!", "No Items",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            paymentControl.LoadPaymentData(activeSessions, totalAmount);
            paymentControl.Visible = true;
            paymentControl.BringToFront();
            
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



        // EXTEND SESSION

        private void chkExtend_CheckedChanged(object sender, EventArgs e)
        {
            if (chkExtend.Checked)
                MessageBox.Show("Extension enabled.");
        }


        // DATABASE CONNECTION TEST
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


        private void ResetTransaction()
        {
            txtOrderNumber.Clear();
            lvSelectedGames.Items.Clear();
            lblTotal.Text = "₱0.00";

            activeSessions.Clear();
            totalAmount = 0;

            btnProceedPayment.Enabled = false;
        }

        private void btnClearCashierForm_Click_1(object sender, EventArgs e)
        {
            ResetTransaction();
        }
        
        
        // ========= ORDER FORM INTERACTION METHODS =========
        private void btnNewOrder_Click(object sender, EventArgs e)
        {
            OrderForm orderForm = new OrderForm();
            orderForm.ShowDialog();
        }

        




    }
}
