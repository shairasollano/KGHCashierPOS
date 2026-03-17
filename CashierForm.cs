using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KGHCashierPOS
{
    public partial class CashierForm : Form
    {
        // ============ MANAGERS ============
        private CashierSessionManager sessionManager;
        private paymentControl1 paymentControl;

        // ============ CONSTRUCTOR ============
        public CashierForm()
        {
            InitializeComponent();

            sessionManager = new CashierSessionManager();

            paymentControl = new paymentControl1();
            paymentControl.Visible = false;  // ✅ This should already be here
            paymentControl.Dock = DockStyle.Fill;  // ⭐ ADD THIS - Makes it fill the form
            paymentControl.BringToFront();  // ⭐ ADD THIS - Ensures it's on top when visible
            paymentControl.PaymentSuccessful += OnPaymentSuccessful;

            this.Controls.Add(paymentControl);

            InitializeButtonStyles();
        }

        // ============ INITIALIZATION ============
        private void InitializeButtonStyles()
        {
            // Game buttons
            ButtonStyleHelper.ApplyGameButtonStyle(btnBilliards);
            ButtonStyleHelper.ApplyGameButtonStyle(btnScooter);
            ButtonStyleHelper.ApplyGameButtonStyle(btnBadminton);
            ButtonStyleHelper.ApplyGameButtonStyle(btnTableTennis);

            // Duration buttons
            ButtonStyleHelper.ApplyDurationButtonStyle(btn30min);
            ButtonStyleHelper.ApplyDurationButtonStyle(btn1hour);

            // Action buttons
            ButtonStyleHelper.ApplyActionButtonStyle(btnProceedPayment, Color.FromArgb(76, 175, 80)); // Green
            ButtonStyleHelper.ApplyActionButtonStyle(btnRemoveGame, Color.FromArgb(244, 67, 54)); // Red
            ButtonStyleHelper.ApplyActionButtonStyle(btnClearCashierForm, Color.FromArgb(255, 152, 0)); // Orange
        }

        private void CashierForm_Load(object sender, EventArgs e)
        {
            UpdateDateTime();
            timerDateTime.Start();
        }

        // ============ GAME SELECTION ============
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
            sessionManager.SelectedGame = gameName;
            ResetGameButtonColors();
            clickedButton.BackColor = ButtonStyleHelper.SelectedColor;
        }

        private void ResetGameButtonColors()
        {
            ButtonStyleHelper.ResetGameButtons(btnBilliards, btnScooter, btnBadminton, btnTableTennis);
        }

        // ============ DURATION SELECTION ============
        private void btn30Min_Click(object sender, EventArgs e)
        {
            AddDurationToGame(30);
        }

        private void btn1Hour_Click(object sender, EventArgs e)
        {
            AddDurationToGame(60);
        }

        private void AddDurationToGame(int minutes)
        {
            if (string.IsNullOrEmpty(sessionManager.SelectedGame))
            {
                MessageBox.Show("Please select a game first!", "No Game Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check for equipment
            if (sessionManager.HasEquipment(sessionManager.SelectedGame))
            {
                ShowEquipmentSelection(minutes);
            }
            else
            {
                AddSessionWithoutEquipment(minutes);
            }
        }

        // ============ EQUIPMENT SELECTION ============
        private void ShowEquipmentSelection(int minutes)
        {
            var equipment = sessionManager.GetEquipmentForGame(sessionManager.SelectedGame);

            using (var dialog = new EquipmentSelectionDialog(sessionManager.SelectedGame, equipment))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    sessionManager.AddOrExtendSession(
                        sessionManager.SelectedGame,
                        minutes,
                        dialog.SelectedEquipment,
                        dialog.TotalEquipmentCost
                    );

                    RefreshListView();
                    ResetGameSelection();
                }
            }
        }

        private void AddSessionWithoutEquipment(int minutes)
        {
            sessionManager.AddOrExtendSession(
                sessionManager.SelectedGame,
                minutes,
                new List<Equipment>(),
                0
            );

            RefreshListView();
            ResetGameSelection();
        }

        private void ResetGameSelection()
        {
            ResetGameButtonColors();
            sessionManager.SelectedGame = "";
        }

        // ============ REFRESH LISTVIEW ============
        private void RefreshListView()
        {
            lvSelectedGames.Items.Clear();

            foreach (var session in sessionManager.ActiveSessions.Values)
            {
                string durationText = DurationFormatter.Format(session.TotalMinutes);

                // Set times
                session.StartTime = DateTime.Now.AddMinutes(3);
                session.EndTime = session.StartTime.AddMinutes(session.TotalMinutes);
                session.IsActive = true;

                decimal displayPrice = session.TotalPrice + session.EquipmentCost;

                ListViewItem item = new ListViewItem(session.GameName);
                item.SubItems.Add(durationText);
                item.SubItems.Add(PriceFormatter.FormatSimple(displayPrice));
                item.SubItems.Add(session.StartTime.ToString("hh:mm tt"));
                item.SubItems.Add(session.EndTime.ToString("hh:mm tt"));
                item.SubItems.Add(PriceFormatter.FormatSimple(displayPrice));

                lvSelectedGames.Items.Add(item);
            }

            lblTotal.Text = "₱ " + sessionManager.TotalAmount.ToString("0.00");
        }

        // ============ REMOVE GAME ============
        private void btnRemoveGame_Click(object sender, EventArgs e)
        {
            if (lvSelectedGames.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a game to remove!", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string gameName = lvSelectedGames.SelectedItems[0].Text;

            if (MessageBox.Show($"Remove {gameName}?", "Confirm Remove",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sessionManager.RemoveSession(gameName);
                RefreshListView();
            }
        }

        // ============ ORDER NUMBER KEYPAD ============
        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            txtOrderNumber.Text += btn.Text;
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (txtOrderNumber.Text.Length > 0)
            {
                txtOrderNumber.Text = txtOrderNumber.Text.Substring(0, txtOrderNumber.Text.Length - 1);
            }
        }

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

            // Format order number
            orderNumber = new string(orderNumber.Where(char.IsDigit).ToArray());
            if (orderNumber.Length < 6)
            {
                orderNumber = orderNumber.PadLeft(6, '0');
            }
            txtOrderNumber.Text = orderNumber;

            // Load using repository
            LoadOrderUsingRepository(orderNumber);
        }

        private void LoadOrderUsingRepository(string orderNumber)
        {
            try
            {
                var items = OrderRepository.LoadOrder(orderNumber);

                if (items == null)
                {
                    MessageBox.Show(
                        $"Order #{orderNumber} not found or already processed!",
                        "Invalid Order",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    txtOrderNumber.Clear();
                    txtOrderNumber.Focus();
                    return;
                }

                // Populate ListView
                lvSelectedGames.Items.Clear();
                decimal total = 0;

                foreach (var item in items)
                {
                    string durationText = DurationFormatter.Format(item.Duration);
                    decimal itemTotal = item.TotalPrice;

                    ListViewItem lvItem = new ListViewItem(item.GameName);
                    lvItem.SubItems.Add(durationText);
                    lvItem.SubItems.Add(PriceFormatter.Format(itemTotal));

                    lvSelectedGames.Items.Add(lvItem);
                    total += itemTotal;
                }

                lblTotal.Text = PriceFormatter.Format(total);

                MessageBox.Show(
                    $"Order #{orderNumber} loaded!\n" +
                    $"Items: {items.Count}\n" +
                    $"Total: {PriceFormatter.Format(total)}",
                    "Order Loaded",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                txtOrderNumber.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadOrderFromDatabase(string orderNumber)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"=== Loading order: {orderNumber} ===");

                // Load using repository
                var items = OrderRepository.LoadOrder(orderNumber);

                if (items == null || items.Count == 0)
                {
                    MessageBox.Show(
                        $"Order #{orderNumber} not found or already processed!",
                        "Invalid Order",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    txtOrderNumber.Clear();
                    txtOrderNumber.Focus();
                    return;
                }

                // Clear ListView
                lvSelectedGames.Items.Clear();
                decimal orderTotal = 0;

                // Populate ListView
                foreach (var item in items)
                {
                    string durationText = DurationFormatter.Format(item.Duration);
                    decimal itemTotal = item.TotalPrice;

                    ListViewItem lvItem = new ListViewItem(item.GameName);
                    lvItem.SubItems.Add(durationText);
                    lvItem.SubItems.Add(PriceFormatter.Format(itemTotal));

                    lvSelectedGames.Items.Add(lvItem);
                    orderTotal += itemTotal;

                    System.Diagnostics.Debug.WriteLine($"Added: {item.GameName} - {durationText} - {PriceFormatter.Format(itemTotal)}");
                }

                // Update total
                lblTotal.Text = PriceFormatter.Format(orderTotal);

                System.Diagnostics.Debug.WriteLine($"=== Order loaded successfully: {items.Count} items, Total: {PriceFormatter.Format(orderTotal)} ===");

                MessageBox.Show(
                    $"Order #{orderNumber} loaded successfully!\n\n" +
                    $"Items: {items.Count}\n" +
                    $"Total: {PriceFormatter.Format(orderTotal)}",
                    "Order Loaded",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                txtOrderNumber.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading order:\n{ex.Message}\n\nPlease check database connection.",
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                System.Diagnostics.Debug.WriteLine($"❌ LoadOrder Exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        private void UpdateTotalAmount()
        {
            decimal total = 0;

            foreach (ListViewItem item in lvSelectedGames.Items)
            {
                total += PriceFormatter.Parse(item.SubItems[2].Text);
            }

            lblTotal.Text = PriceFormatter.Format(total);
        }

        // ============ PROCEED TO PAYMENT ============
        private void btnProceedPayment_Click(object sender, EventArgs e)
        {
            if (lvSelectedGames.Items.Count == 0)
            {
                MessageBox.Show("Please add games to order!", "No Items",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create sessions
            Dictionary<string, GameSession> sessions = new Dictionary<string, GameSession>();
            decimal total = 0;
            int counter = 1;

            foreach (ListViewItem item in lvSelectedGames.Items)
            {
                string gameName = item.Text;
                int totalMinutes = DurationFormatter.Parse(item.SubItems[1].Text);
                decimal price = PriceFormatter.Parse(item.SubItems[2].Text);

                GameSession session = new GameSession
                {
                    GameName = gameName,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now.AddMinutes(totalMinutes),
                    TotalMinutes = totalMinutes,
                    TotalPrice = price
                };

                sessions.Add($"session_{counter}", session);
                total += price;
                counter++;
            }

            // Show payment
            paymentControl.Visible = true;
            paymentControl.BringToFront();
            paymentControl.LoadPaymentData(sessions, total);
        }

        // ============ CLEAR & RESET ============
        private void btnClearCashierForm_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear all items?", "Confirm Clear",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ResetTransaction();
            }
        }

        public void ResetTransaction()
        {
            txtOrderNumber.Clear();
            lvSelectedGames.Items.Clear();
            lblTotal.Text = "₱0.00";
            sessionManager.ClearAll();
            ResetGameButtonColors();
            txtOrderNumber.Focus();
        }

        private void OnPaymentSuccessful()
        {
            ResetTransaction();
            paymentControl.Visible = false;

            MessageBox.Show("Payment completed!\nForm reset for next customer.",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ============ DATE/TIME ============
        private void timerDateTime_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            lblDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        // Add this test method
        private void TestDatabaseConnection()
        {
            try
            {
                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();
                    MessageBox.Show("Database connected successfully!", "Success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error");
            }
        }
    }
}