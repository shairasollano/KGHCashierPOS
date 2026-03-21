using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KGHCashierPOS
{
    public partial class OrderForm : Form
    {
        // ============ SINGLE MANAGER INSTANCE ============
        private OrderManager orderManager;
        private EquipmentRentalControl equipmentRentalControl;

        // ============ CONSTRUCTOR ============
        public OrderForm()
        {
            InitializeComponent();
            orderManager = new OrderManager();
            InitializeEquipmentControl();
            InitializeForm();
        }

        // ============ INITIALIZATION ============
        private void InitializeForm()
        {
            UpdateOrderNumberDisplay();
            UpdateTotalDisplay();
            InitializeButtonStyles();
        }

        private void InitializeEquipmentControl()
        {
            equipmentRentalControl = new EquipmentRentalControl();
            equipmentRentalControl.Visible = false;
            equipmentRentalControl.Location = new Point(
                (this.ClientSize.Width - equipmentRentalControl.Width) / 2,
                (this.ClientSize.Height - equipmentRentalControl.Height) / 2
            );
            equipmentRentalControl.BringToFront();

            this.Controls.Add(equipmentRentalControl);
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
            ButtonStyleHelper.ApplyActionButtonStyle(btnPayCashier, Color.FromArgb(76, 175, 80)); // Green
            ButtonStyleHelper.ApplyActionButtonStyle(btnClear, Color.FromArgb(244, 67, 54)); // Red
            ButtonStyleHelper.ApplyActionButtonStyle(btnRemove, Color.FromArgb(255, 152, 0)); // Orange
        }

        // ============ DISPLAY UPDATES ============
        private void UpdateOrderNumberDisplay()
        {
            lblOrderNum.Text = "Order #: " + orderManager.OrderNumber;
        }

        private void UpdateTotalDisplay()
        {
            lblTotalValue.Text = PriceFormatter.Format(orderManager.TotalAmount);
        }

        private void UpdateListBoxDisplay()
        {
            lbDisplay.Items.Clear();

            // Header
            lbDisplay.Items.Add("═══════════════════════════════════════════════");
            lbDisplay.Items.Add($"ORDER NUMBER: {orderManager.OrderNumber}");
            lbDisplay.Items.Add("═══════════════════════════════════════════════");
            lbDisplay.Items.Add("");
            lbDisplay.Items.Add("GAMES ORDERED:");
            lbDisplay.Items.Add("───────────────────────────────────────────────");

            // Items
            int itemNumber = 1;
            foreach (var item in orderManager.Items)
            {
                lbDisplay.Items.Add($"{itemNumber}. {item.GameName}");
                lbDisplay.Items.Add($"   Duration: {item.GetDurationText()}");
                lbDisplay.Items.Add($"   Game Price: {PriceFormatter.Format(item.GamePrice)}");

                // Equipment details
                if (item.Equipment.Count > 0)
                {
                    lbDisplay.Items.Add($"   Equipment:");
                    foreach (var eq in item.Equipment)
                    {
                        if (eq.DefaultQuantity > 0)
                        {
                            lbDisplay.Items.Add($"     • {eq.Name} x{eq.DefaultQuantity} (Included)");
                        }
                        if (eq.RentalQuantity > 0)
                        {
                            lbDisplay.Items.Add($"     • {eq.Name} x{eq.RentalQuantity} ({eq.Type}) - {PriceFormatter.Format(eq.TotalCost)}");
                        }
                    }
                    if (item.EquipmentCost > 0)
                    {
                        lbDisplay.Items.Add($"   Equipment Cost: {PriceFormatter.Format(item.EquipmentCost)}");
                    }
                }

                lbDisplay.Items.Add($"   Total: {PriceFormatter.Format(item.TotalPrice)}");
                lbDisplay.Items.Add("");
                itemNumber++;
            }

            lbDisplay.Items.Add("───────────────────────────────────────────────");
            lbDisplay.Items.Add($"TOTAL AMOUNT: {PriceFormatter.Format(orderManager.TotalAmount)}");
            lbDisplay.Items.Add("═══════════════════════════════════════════════");
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
            orderManager.SelectedGame = gameName;
            ResetGameButtonColors();
            clickedButton.BackColor = ButtonStyleHelper.SelectedColor;
        }

        private void ResetGameButtonColors()
        {
            ButtonStyleHelper.ResetGameButtons(btnBilliards, btnScooter, btnBadminton, btnTableTennis, btn30min, btn1hour);
        }

        // ============ DURATION SELECTION ============
        private void btn30min_Click(object sender, EventArgs e)
        {
            orderManager.SelectedDuration = 30;
            btn30min.BackColor = ButtonStyleHelper.SelectedColor;
            btn1hour.BackColor = ButtonStyleHelper.DefaultColor;
            TryAddGameToOrder();
        }

        private void btn1hour_Click(object sender, EventArgs e)
        {
            orderManager.SelectedDuration = 60;
            btn1hour.BackColor = ButtonStyleHelper.SelectedColor;
            btn30min.BackColor = ButtonStyleHelper.DefaultColor;
            TryAddGameToOrder();
        }

        private void ResetDurationButtonColors()
        {
            btn30min.BackColor = ButtonStyleHelper.DefaultColor;
            btn1hour.BackColor = ButtonStyleHelper.DefaultColor;
        }

        // ============ ADD TO ORDER ============
        private void TryAddGameToOrder()
        {
            if (!orderManager.IsGameSelected())
            {
                MessageBox.Show("Please select a game first!", "No Game Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!orderManager.IsDurationSelected())
            {
                MessageBox.Show("Please select duration!", "No Duration Selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ShowEquipmentSelection();
        }

        private void ShowEquipmentSelection()
        {
            if (!orderManager.HasEquipment(orderManager.SelectedGame))
            {
                AddGameToOrder(new List<Equipment>(), 0);
                return;
            }

            var equipment = orderManager.GetEquipmentForGame(orderManager.SelectedGame);

            // ⭐ Use UserControl instead of Dialog
            equipmentRentalControl.LoadEquipment(orderManager.SelectedGame, equipment);
            equipmentRentalControl.Visible = true;
            equipmentRentalControl.BringToFront();

            // ⭐ Wait for user action (using Timer to check)
            Timer checkTimer = new Timer();
            checkTimer.Interval = 100;
            checkTimer.Tick += (s, e) =>
            {
                if (!equipmentRentalControl.Visible)
                {
                    checkTimer.Stop();

                    if (equipmentRentalControl.IsConfirmed)
                    {
                        AddGameToOrder(
                            equipmentRentalControl.SelectedEquipment,
                            equipmentRentalControl.TotalEquipmentCost
                        );
                    }

                    checkTimer.Dispose();
                }
            };
            checkTimer.Start();
        }

        private void AddGameToOrder(System.Collections.Generic.List<Equipment> equipment, decimal equipmentCost)
        {
            decimal gamePrice = orderManager.CalculateGamePrice(
                orderManager.SelectedGame,
                orderManager.SelectedDuration
            );

            OrderItem item = new OrderItem
            {
                OrderNumber = orderManager.OrderNumber,
                GameName = orderManager.SelectedGame,
                Duration = orderManager.SelectedDuration,
                GamePrice = gamePrice,
                Equipment = equipment,
                EquipmentCost = equipmentCost
            };

            orderManager.AddItem(item);
            UpdateTotalDisplay();
            UpdateListBoxDisplay();

            string equipText = equipmentCost > 0 ? $"\nEquipment: {PriceFormatter.Format(equipmentCost)}" : "";
            MessageBox.Show(
                $"{item.GameName} ({item.GetDurationText()}) added!\n" +
                $"Game: {PriceFormatter.Format(gamePrice)}" + equipText +
                $"\nTotal: {PriceFormatter.Format(item.TotalPrice)}",
                "Game Added",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            ResetGameButtonColors();
            ResetDurationButtonColors();
            orderManager.ResetSelection();
        }

        // ============ BUTTON ACTIONS ============
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (!orderManager.HasItems())
            {
                MessageBox.Show("No items to remove!", "Empty Order",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Remove last item?", "Remove Item",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                orderManager.RemoveLastItem();
                UpdateTotalDisplay();
                UpdateListBoxDisplay();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (!orderManager.HasItems())
            {
                MessageBox.Show("Order is already empty!", "No Items",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Clear all items?", "Clear Order",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                orderManager.ClearOrder();
                lbDisplay.Items.Clear();
                UpdateTotalDisplay();
                ResetGameButtonColors();
                ResetDurationButtonColors();

                MessageBox.Show("Order cleared!", "Cleared",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnPayCashier_Click(object sender, EventArgs e)
        {
            if (!orderManager.HasItems())
            {
                MessageBox.Show("Please add games to order!", "No Items",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"=== Saving order: {orderManager.OrderNumber} ===");

                // Save to database using repository
                OrderRepository.SaveOrder(
                    orderManager.OrderNumber,
                    orderManager.TotalAmount,
                    orderManager.Items
                );

                // Generate PDF
                PDFGenerator.GenerateOrderSlip(
                    orderManager.OrderNumber,
                    orderManager.Items,
                    orderManager.TotalAmount
                );

                MessageBox.Show(
                    $"Order #{orderManager.OrderNumber} submitted!\n\n" +
                    $"Items: {orderManager.Items.Count}\n" +
                    $"Total: {PriceFormatter.Format(orderManager.TotalAmount)}\n\n" +
                    "Order slip generated.\nPlease proceed to cashier!",
                    "Order Submitted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error saving order:\n{ex.Message}\n\nPlease try again.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                System.Diagnostics.Debug.WriteLine($"❌ SaveOrder Exception: {ex.Message}");
            }
        }

        private void ClearForm()
        {
            orderManager.ClearAll();
            lbDisplay.Items.Clear();
            UpdateOrderNumberDisplay();
            UpdateTotalDisplay();
            ResetGameButtonColors();
            ResetDurationButtonColors();
        }

        // ============ DATE/TIME ============
        private void timerDateTime1_Tick(object sender, EventArgs e)
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