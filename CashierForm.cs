using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
            paymentControl.Visible = false;
            paymentControl.Dock = DockStyle.Fill;
            paymentControl.BringToFront();
            paymentControl.PaymentSuccessful += OnPaymentSuccessful;
            this.Controls.Add(paymentControl);

            InitializeButtonStyles();
            InitializeRichTextBox();  // ⭐ NEW
        }

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

        // ⭐ NEW METHOD
        private void InitializeRichTextBox()
        {
            rtbSelectedGames.ReadOnly = true;
            rtbSelectedGames.Font = new Font("Courier New", 9);
            rtbSelectedGames.BackColor = Color.White;
            rtbSelectedGames.BorderStyle = BorderStyle.FixedSingle;
        }

        // ... existing initialization methods ...

        private void CashierForm_Load(object sender, EventArgs e)
        {
            UpdateDateTime();
            timerDateTime.Start();
            RefreshDisplay();  // ⭐ Initial display
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
            ButtonStyleHelper.ResetGameButtons(btnBilliards, btnScooter, btnBadminton, btnTableTennis, btn30min, btn1hour);
        }

        // ============ DURATION SELECTION ============
        private void btn30Min_Click(object sender, EventArgs e)
        {
            AddDurationToGame(30);
            ResetGameButtonColors();
        }

        private void btn1Hour_Click(object sender, EventArgs e)
        {
            AddDurationToGame(60);
            ResetGameButtonColors();
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

                    RefreshDisplay();  // ⭐ Changed from RefreshListView
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

            RefreshDisplay();  // ⭐ Changed from RefreshListView
            ResetGameSelection();
        }

        private void ResetGameSelection()
        {
            ResetGameButtonColors();
            sessionManager.SelectedGame = "";
        }

        // ⭐ COMPLETELY NEW METHOD - Replaces RefreshListView
        private void RefreshDisplay()
        {
            rtbSelectedGames.Clear();

            if (sessionManager.ActiveSessions.Count == 0)
            {
                rtbSelectedGames.Text = "\n\n          No games selected yet.\n\n          Select a game and duration to begin.";
                lblTotal.Text = "₱0.00";
                return;
            }

            StringBuilder summary = new StringBuilder();

            summary.AppendLine("════════════════════════════════════════════════════════");
            summary.AppendLine("                    CURRENT ORDER");
            summary.AppendLine("════════════════════════════════════════════════════════");
            summary.AppendLine();

            decimal totalAmount = 0;

            foreach (var session in sessionManager.ActiveSessions.Values)
            {
                string durationText = DurationFormatter.Format(session.TotalMinutes);

                // Set times
                session.StartTime = DateTime.Now.AddMinutes(3);
                session.EndTime = session.StartTime.AddMinutes(session.TotalMinutes);
                session.IsActive = true;

                decimal displayPrice = session.TotalPrice + session.EquipmentCost;
                totalAmount += displayPrice;

                // Game header
                summary.AppendLine($"  Game:             {session.GameName}");
                summary.AppendLine($"  Duration:         {durationText}");
                summary.AppendLine($"  Start Time:       {session.StartTime:hh:mm tt}");
                summary.AppendLine($"  End Time:         {session.EndTime:hh:mm tt}");
                summary.AppendLine($"  Game Price:       {PriceFormatter.Format(session.TotalPrice)}");

                // ⭐ Equipment details
                if (session.Equipment != null && session.Equipment.Count > 0)
                {
                    summary.AppendLine("  Equipment:");

                    foreach (var eq in session.Equipment)
                    {
                        if (eq.DefaultQuantity > 0)
                        {
                            summary.AppendLine($"    • {eq.Name} x{eq.DefaultQuantity} (Included)");
                        }
                        if (eq.RentalQuantity > 0)
                        {
                            summary.AppendLine($"    • {eq.Name} x{eq.RentalQuantity} ({eq.Type}) - {PriceFormatter.Format(eq.TotalCost)}");
                        }
                    }

                    if (session.EquipmentCost > 0)
                    {
                        summary.AppendLine($"  Equipment Cost:   {PriceFormatter.Format(session.EquipmentCost)}");
                    }
                }

                summary.AppendLine("  ────────────────────────────────────────────────────");
                summary.AppendLine($"  Subtotal:         {PriceFormatter.Format(displayPrice)}");
                summary.AppendLine();
            }

            summary.AppendLine("════════════════════════════════════════════════════════");
            summary.AppendLine($"  TOTAL AMOUNT:     {PriceFormatter.Format(totalAmount)}");
            summary.AppendLine("════════════════════════════════════════════════════════");

            rtbSelectedGames.Text = summary.ToString();
            lblTotal.Text = PriceFormatter.Format(totalAmount);
        }

        // ============ REMOVE GAME ============
        private void btnRemoveGame_Click(object sender, EventArgs e)
        {
            if (sessionManager.ActiveSessions.Count == 0)
            {
                MessageBox.Show("No games to remove!", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Show selection dialog
            var gameNames = sessionManager.ActiveSessions.Keys.ToList();

            Form selectionForm = new Form();
            selectionForm.Text = "Select Game to Remove";
            selectionForm.Size = new Size(350, 250);
            selectionForm.StartPosition = FormStartPosition.CenterParent;
            selectionForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            selectionForm.MaximizeBox = false;
            selectionForm.MinimizeBox = false;

            ListBox listBox = new ListBox();
            listBox.Dock = DockStyle.Fill;
            listBox.Font = new Font("Segoe UI", 10);

            foreach (var key in gameNames)
            {
                var session = sessionManager.ActiveSessions[key];
                listBox.Items.Add($"{session.GameName} - {DurationFormatter.Format(session.TotalMinutes)}");
            }

            Button btnRemove = new Button();
            btnRemove.Text = "Remove Selected";
            btnRemove.Dock = DockStyle.Bottom;
            btnRemove.Height = 40;
            btnRemove.DialogResult = DialogResult.OK;

            selectionForm.Controls.Add(listBox);
            selectionForm.Controls.Add(btnRemove);

            if (selectionForm.ShowDialog() == DialogResult.OK && listBox.SelectedIndex >= 0)
            {
                string selectedKey = gameNames[listBox.SelectedIndex];
                var session = sessionManager.ActiveSessions[selectedKey];

                if (MessageBox.Show($"Remove {session.GameName}?", "Confirm Remove",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    sessionManager.RemoveSession(selectedKey);
                    RefreshDisplay();
                }
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

            orderNumber = new string(orderNumber.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(orderNumber))
            {
                MessageBox.Show("Invalid order number!", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOrderNumber.Clear();
                return;
            }

            if (orderNumber.Length < 6)
            {
                orderNumber = orderNumber.PadLeft(6, '0');
            }

            txtOrderNumber.Text = orderNumber;
            LoadOrderFromDatabase(orderNumber);
        }

        private void LoadOrderFromDatabase(string orderNumber)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"=== Loading order: {orderNumber} ===");

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

                // Clear current sessions
                sessionManager.ClearAll();
                rtbSelectedGames.Clear();

                StringBuilder summary = new StringBuilder();
                summary.AppendLine("════════════════════════════════════════════════════════");
                summary.AppendLine($"              ORDER #{orderNumber}");
                summary.AppendLine("════════════════════════════════════════════════════════");
                summary.AppendLine();

                decimal orderTotal = 0;

                foreach (var item in items)
                {

                    GameSession session = new GameSession
                    {
                        GameName = item.GameName,
                        TotalMinutes = item.Duration,
                        TotalPrice = item.Price,
                        EquipmentCost = item.EquipmentCost,
                        Equipment = new List<Equipment>() // or map if you have data
                    };

                    // Use unique key (important)
                    sessionManager.ActiveSessions[item.GameName + Guid.NewGuid()] = session;

                    string durationText = DurationFormatter.Format(item.Duration);
                    decimal itemTotal = item.TotalPrice;
                    orderTotal += itemTotal;

                    summary.AppendLine($"  Game:             {item.GameName}");
                    summary.AppendLine($"  Duration:         {durationText}");
                    summary.AppendLine($"  Game Price:       {PriceFormatter.Format(item.Price)}");

                    if (item.EquipmentCost > 0)
                    {
                        summary.AppendLine($"  Equipment Cost:   {PriceFormatter.Format(item.EquipmentCost)}");
                    }

                    summary.AppendLine("  ────────────────────────────────────────────────────");
                    summary.AppendLine($"  Subtotal:         {PriceFormatter.Format(itemTotal)}");
                    summary.AppendLine();
                }

                summary.AppendLine("════════════════════════════════════════════════════════");
                summary.AppendLine($"  TOTAL AMOUNT:     {PriceFormatter.Format(orderTotal)}");
                summary.AppendLine("════════════════════════════════════════════════════════");

                rtbSelectedGames.Text = summary.ToString();
                lblTotal.Text = PriceFormatter.Format(orderTotal);

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
            }
        }

        // ============ PROCEED TO PAYMENT ============
        private void btnProceedPayment_Click(object sender, EventArgs e)
        {

            string orderNumberToPass = txtOrderNumber.Text.Trim();  // ⭐ Get order number

            if (sessionManager.ActiveSessions.Count == 0)
            {
                if (string.IsNullOrWhiteSpace(rtbSelectedGames.Text) ||
                    rtbSelectedGames.Text.Contains("No games selected"))
                {
                    MessageBox.Show("Please add games to order!", "No Items",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Create sessions from current data
            Dictionary<string, GameSession> sessions = new Dictionary<string, GameSession>();
            decimal total = 0;

            if (sessionManager.ActiveSessions.Count > 0)
            {
                sessions = sessionManager.ActiveSessions;

                // ⭐ FIXED: Calculate total including equipment
                foreach (var session in sessions.Values)
                {
                    total += session.TotalPrice + session.EquipmentCost;
                }

                System.Diagnostics.Debug.WriteLine($"=== Proceed to Payment ===");
                System.Diagnostics.Debug.WriteLine($"Sessions: {sessions.Count}");
                foreach (var session in sessions.Values)
                {
                    System.Diagnostics.Debug.WriteLine($"  {session.GameName}: Game={session.TotalPrice:C}, Equipment={session.EquipmentCost:C}");
                }
                System.Diagnostics.Debug.WriteLine($"Total: {total:C}");
            }
            else
            {
                // Parse from label for loaded orders
                if (decimal.TryParse(lblTotal.Text.Replace("₱", "").Replace(",", "").Trim(), out decimal parsedTotal))
                {
                    total = parsedTotal;
                }
            }

            if (total <= 0)
            {
                MessageBox.Show("Invalid order total!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Show payment
            paymentControl.Visible = true;
            paymentControl.BringToFront();
            paymentControl.LoadPaymentData(sessions, total, orderNumberToPass);  // ⭐ Pass order number
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
            rtbSelectedGames.Clear();
            lblTotal.Text = "₱0.00";
            
            ResetGameButtonColors();
            RefreshDisplay();
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
    }
}