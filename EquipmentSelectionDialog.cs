using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KGHCashierPOS
{
    public class EquipmentSelectionDialog : Form
    {
        private List<Equipment> equipmentList;
        private decimal totalEquipmentCost = 0;

        public decimal TotalEquipmentCost => totalEquipmentCost;
        public List<Equipment> SelectedEquipment => equipmentList;

        public EquipmentSelectionDialog(string gameName, List<Equipment> equipment)
        {
            // Deep copy the equipment list
            equipmentList = new List<Equipment>();
            foreach (var eq in equipment)
            {
                equipmentList.Add(new Equipment
                {
                    Name = eq.Name,
                    Price = eq.Price,
                    DefaultQuantity = eq.DefaultQuantity,
                    RentalQuantity = eq.RentalQuantity,
                    Type = eq.Type
                });
            }

            InitializeDialog(gameName);
        }

        private void InitializeDialog(string gameName)
        {
            // Form settings
            this.Text = $"{gameName} - Equipment Selection";
            this.Size = new Size(500, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = $"Select Equipment for {gameName}";
            lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            // Info label
            Label lblInfo = new Label();
            lblInfo.Text = "✓ = Included with game rental";
            lblInfo.Font = new Font("Segoe UI", 9, FontStyle.Italic);
            lblInfo.ForeColor = Color.Gray;
            lblInfo.Location = new Point(20, 50);
            lblInfo.AutoSize = true;
            this.Controls.Add(lblInfo);

            // Equipment panel
            Panel equipmentPanel = new Panel();
            equipmentPanel.Location = new Point(20, 80);
            equipmentPanel.Size = new Size(440, 220);
            equipmentPanel.AutoScroll = true;
            equipmentPanel.BorderStyle = BorderStyle.FixedSingle;
            equipmentPanel.BackColor = Color.White;
            this.Controls.Add(equipmentPanel);

            int yPosition = 10;

            foreach (var eq in equipmentList)
            {
                // Equipment name
                Label lblEquipment = new Label();

                if (eq.DefaultQuantity > 0)
                {
                    lblEquipment.Text = $"✓ {eq.Name} (Included: {eq.DefaultQuantity})";
                    lblEquipment.ForeColor = Color.Green;
                    lblEquipment.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }
                else
                {
                    lblEquipment.Text = $"• {eq.Name}";
                    lblEquipment.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                }

                lblEquipment.Location = new Point(10, yPosition);
                lblEquipment.Size = new Size(400, 20);
                equipmentPanel.Controls.Add(lblEquipment);

                // Rental options
                if (eq.Price > 0)
                {
                    Label lblPrice = new Label();
                    lblPrice.Text = $"₱{eq.Price:N2} each";
                    lblPrice.Location = new Point(10, yPosition + 25);
                    lblPrice.Size = new Size(100, 20);
                    lblPrice.Font = new Font("Segoe UI", 9);
                    equipmentPanel.Controls.Add(lblPrice);

                    Label lblQty = new Label();
                    lblQty.Text = eq.Type == "Purchase" ? "Buy:" : "Rent:";
                    lblQty.Location = new Point(120, yPosition + 25);
                    lblQty.Size = new Size(50, 20);
                    lblQty.Font = new Font("Segoe UI", 9);
                    equipmentPanel.Controls.Add(lblQty);

                    NumericUpDown numQty = new NumericUpDown();
                    numQty.Location = new Point(170, yPosition + 23);
                    numQty.Size = new Size(60, 25);
                    numQty.Minimum = 0;
                    numQty.Maximum = 10;
                    numQty.Value = eq.RentalQuantity;
                    numQty.Tag = eq;
                    numQty.ValueChanged += NumQty_ValueChanged;
                    equipmentPanel.Controls.Add(numQty);

                    Label lblType = new Label();
                    lblType.Text = $"({eq.Type})";
                    lblType.Location = new Point(240, yPosition + 25);
                    lblType.Size = new Size(100, 20);
                    lblType.Font = new Font("Segoe UI", 8, FontStyle.Italic);
                    lblType.ForeColor = Color.Gray;
                    equipmentPanel.Controls.Add(lblType);

                    yPosition += 65;
                }
                else
                {
                    yPosition += 40;
                }

                Panel separator = new Panel();
                separator.Location = new Point(10, yPosition - 5);
                separator.Size = new Size(410, 1);
                separator.BackColor = Color.LightGray;
                equipmentPanel.Controls.Add(separator);
            }

            // Total cost
            Label lblTotalCost = new Label();
            lblTotalCost.Name = "lblTotalCost";
            lblTotalCost.Text = "Equipment Total: ₱0.00";
            lblTotalCost.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTotalCost.Location = new Point(20, 315);
            lblTotalCost.Size = new Size(300, 25);
            this.Controls.Add(lblTotalCost);

            // Buttons
            Button btnConfirm = new Button();
            btnConfirm.Text = "Confirm & Add";
            btnConfirm.Size = new Size(120, 45);
            btnConfirm.Location = new Point(230, 360);
            btnConfirm.BackColor = Color.FromArgb(76, 175, 80);
            btnConfirm.ForeColor = Color.White;
            btnConfirm.FlatStyle = FlatStyle.Flat;
            btnConfirm.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnConfirm.Cursor = Cursors.Hand;
            btnConfirm.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
            this.Controls.Add(btnConfirm);

            Button btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Size = new Size(120, 45);
            btnCancel.Location = new Point(360, 360);
            btnCancel.BackColor = Color.FromArgb(158, 158, 158);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            this.Controls.Add(btnCancel);

            UpdateTotalCost();
        }

        private void NumQty_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown num = sender as NumericUpDown;
            Equipment eq = num.Tag as Equipment;
            eq.RentalQuantity = (int)num.Value;
            UpdateTotalCost();
        }

        private void UpdateTotalCost()
        {
            totalEquipmentCost = 0;

            foreach (var eq in equipmentList)
            {
                totalEquipmentCost += eq.RentalQuantity * eq.Price;
            }

            Label lblCost = this.Controls.Find("lblTotalCost", false).FirstOrDefault() as Label;
            if (lblCost != null)
            {
                lblCost.Text = $"Equipment Total: ₱{totalEquipmentCost:N2}";
            }
        }
    }
}
