using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace KGHCashierPOS
{
    public partial class OrderForm : Form
    {
        // ============ SINGLE MANAGER INSTANCE ============
        private OrderManager orderManager;

        // ============ CONSTRUCTOR ============
        public OrderForm()
        {
            InitializeComponent();
            orderManager = new OrderManager();
            InitializeForm();
        }

        // ============ INITIALIZATION ============
        private void InitializeForm()
        {
            UpdateOrderNumberDisplay();
            UpdateTotalDisplay();
            ResetGameButtonColors();
            ResetDurationButtonColors();
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            lblDate1.Text = DateTime.Now.ToString("MM/dd/yyyy");
            lblTime1.Text = DateTime.Now.ToString("hh:mm tt");

            Timer timeTimer = new Timer();
            timeTimer.Interval = 1000;
            timeTimer.Tick += (s, ev) => lblTime1.Text = DateTime.Now.ToString("hh:mm tt");
            timeTimer.Start();
        }

        // ============ DISPLAY UPDATES ============
        private void UpdateOrderNumberDisplay()
        {
            lblOrderNum.Text = "Order #: " + orderManager.OrderNumber;
        }

        private void UpdateTotalDisplay()
        {
            lblTotalValue.Text = "₱" + orderManager.TotalAmount.ToString("N2");
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
                lbDisplay.Items.Add($"   Game Price: ₱{item.GamePrice:N2}");

                // Equipment
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
                            lbDisplay.Items.Add($"     • {eq.Name} x{eq.RentalQuantity} ({eq.Type}) - ₱{eq.TotalCost:N2}");
                        }
                    }
                    if (item.EquipmentCost > 0)
                    {
                        lbDisplay.Items.Add($"   Equipment Cost: ₱{item.EquipmentCost:N2}");
                    }
                }

                lbDisplay.Items.Add($"   Total: ₱{item.TotalPrice:N2}");
                lbDisplay.Items.Add("");
                itemNumber++;
            }

            lbDisplay.Items.Add("───────────────────────────────────────────────");
            lbDisplay.Items.Add($"TOTAL AMOUNT: ₱{orderManager.TotalAmount:N2}");
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
            clickedButton.BackColor = Color.Orange;
        }

        private void ResetGameButtonColors()
        {
            btnBilliards.BackColor = Color.FromArgb(64, 64, 64);
            btnScooter.BackColor = Color.FromArgb(64, 64, 64);
            btnBadminton.BackColor = Color.FromArgb(64, 64, 64);
            btnTableTennis.BackColor = Color.FromArgb(64, 64, 64);
        }

        // ============ DURATION SELECTION ============
        private void btn30min_Click(object sender, EventArgs e)
        {
            orderManager.SelectedDuration = 30;
            btn30min.BackColor = Color.Orange;
            btn1hour.BackColor = Color.FromArgb(64, 64, 64);
            TryAddGameToOrder();
        }

        private void btn1hour_Click(object sender, EventArgs e)
        {
            orderManager.SelectedDuration = 60;
            btn1hour.BackColor = Color.Orange;
            btn30min.BackColor = Color.FromArgb(64, 64, 64);
            TryAddGameToOrder();
        }

        private void ResetDurationButtonColors()
        {
            btn30min.BackColor = Color.FromArgb(64, 64, 64);
            btn1hour.BackColor = Color.FromArgb(64, 64, 64);
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

            List<Equipment> equipment = orderManager.GetEquipmentForGame(orderManager.SelectedGame);

            using (var dialog = new EquipmentSelectionDialog(orderManager.SelectedGame, equipment))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    AddGameToOrder(dialog.SelectedEquipment, dialog.TotalEquipmentCost);
                }
            } 
        }

        private void AddGameToOrder(List<Equipment> equipment, decimal equipmentCost)
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

            string equipText = equipmentCost > 0 ? $"\nEquipment: ₱{equipmentCost:N2}" : "";
            MessageBox.Show(
                $"{item.GameName} ({item.GetDurationText()}) added!\n" +
                $"Game: ₱{gamePrice:N2}" + equipText +
                $"\nTotal: ₱{item.TotalPrice:N2}",
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
            orderManager.ClearOrder();
            lbDisplay.Items.Clear();
            UpdateTotalDisplay();
            ResetGameButtonColors();
            ResetDurationButtonColors();
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
                SaveOrderToDatabase();
                GenerateOrderSlipPDF();

                MessageBox.Show(
                    $"Order #{orderManager.OrderNumber} submitted!\n" +
                    $"Items: {orderManager.Items.Count}\n" +
                    $"Total: ₱{orderManager.TotalAmount:N2}\n\n" +
                    "Please proceed to cashier!",
                    "Order Submitted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // ============ DATABASE ============
        private void SaveOrderToDatabase()
        {
            using (var conn = new MySqlConnection(Database.ConnectionString))
            {
                conn.Open();

                // Save order
                string orderQuery = @"
                    INSERT INTO orders (order_number, customer_name, total_amount, order_date, status)
                    VALUES (@orderNo, @name, @total, @date, 'Pending')";

                using (var cmd = new MySqlCommand(orderQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@orderNo", orderManager.OrderNumber);
                    cmd.Parameters.AddWithValue("@name", "Walk-in Customer");
                    cmd.Parameters.AddWithValue("@total", orderManager.TotalAmount);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }

                // Save items
                foreach (var item in orderManager.Items)
                {
                    string itemQuery = @"
                        INSERT INTO order_items (order_number, game_name, duration_minutes, price, equipment_cost)
                        VALUES (@orderNo, @game, @duration, @price, @equipCost)";

                    using (var cmd = new MySqlCommand(itemQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderNo", orderManager.OrderNumber);
                        cmd.Parameters.AddWithValue("@game", item.GameName);
                        cmd.Parameters.AddWithValue("@duration", item.Duration);
                        cmd.Parameters.AddWithValue("@price", item.GamePrice);
                        cmd.Parameters.AddWithValue("@equipCost", item.EquipmentCost);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // ============ PDF GENERATION ============
        private void GenerateOrderSlipPDF()
        {
            string folderPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "MatchPointOrders"
            );
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"{orderManager.OrderNumber}.pdf");

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                Document doc = new Document(PageSize.A5);
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                iTextSharp.text.Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                iTextSharp.text.Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                iTextSharp.text.Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);

                Paragraph title = new Paragraph("MATCH POINT GAMING HUB", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);
                doc.Add(new Paragraph("\n"));

                doc.Add(new Paragraph($"Order: {orderManager.OrderNumber}", normalFont));
                doc.Add(new Paragraph($"Date: {DateTime.Now:MMMM dd, yyyy hh:mm tt}", normalFont));
                doc.Add(new Paragraph("\n" + new string('-', 50) + "\n"));

                int num = 1;
                foreach (var item in orderManager.Items)
                {
                    doc.Add(new Paragraph($"{num}. {item.GameName} - {item.GetDurationText()}", normalFont));
                    doc.Add(new Paragraph($"   ₱{item.GamePrice:N2}", normalFont));

                    if (item.EquipmentCost > 0)
                    {
                        doc.Add(new Paragraph($"   Equipment: ₱{item.EquipmentCost:N2}", normalFont));
                    }

                    doc.Add(new Paragraph("\n"));
                    num++;
                }

                doc.Add(new Paragraph(new string('-', 50)));
                doc.Add(new Paragraph($"TOTAL: ₱{orderManager.TotalAmount:N2}", boldFont));

                doc.Close();
            }
        }
    }
}