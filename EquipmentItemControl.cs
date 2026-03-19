using System;
using System.Drawing;
using System.Windows.Forms;

namespace KGHCashierPOS
{

    public partial class EquipmentItemControl : UserControl
    {

        private Equipment _equipment;
        private const int MAX_QUANTITY = 10;

        // Event to notify parent when quantity changes
        public event EventHandler QuantityChanged;

        public Equipment Equipment
        {
            get { return _equipment; }
            set
            {
                _equipment = value;
                UpdateDisplay();
            }
        }

        public int RentalQuantity
        {
            get { return _equipment?.RentalQuantity ?? 0; }
        }

        public EquipmentItemControl()
        {
            InitializeComponent();
            SetupButtonStyles();
        }

        private void SetupButtonStyles()
        {
            // Plus button hover
            btnPlus.MouseEnter += (s, e) => btnPlus.BackColor = Color.FromArgb(56, 142, 60);
            btnPlus.MouseLeave += (s, e) => btnPlus.BackColor = Color.FromArgb(76, 175, 80);

            // Minus button hover
            btnMinus.MouseEnter += (s, e) =>
            {
                if (btnMinus.Enabled)
                    btnMinus.BackColor = Color.FromArgb(198, 40, 40);
            };
            btnMinus.MouseLeave += (s, e) => btnMinus.BackColor = Color.FromArgb(244, 67, 54);
        }

        private void UpdateDisplay()
        {
            if (_equipment == null) return;

            lblEquipmentName.Text = _equipment.Name;
            lblPrice.Text = PriceFormatter.Format(_equipment.Price);
            lblQuantity.Text = _equipment.RentalQuantity.ToString();

            // Set type badge
            switch (_equipment.Type)
            {
                case "Rental":
                    lblType.Text = "RENTAL";
                    lblType.BackColor = Color.FromArgb(33, 150, 243); // Blue
                    break;
                case "Purchase":
                    lblType.Text = "PURCHASE";
                    lblType.BackColor = Color.FromArgb(255, 152, 0); // Orange
                    break;
                case "Included":
                    lblType.Text = "INCLUDED";
                    lblType.BackColor = Color.FromArgb(76, 175, 80); // Green
                    break;
            }

            // Show default info if applicable
            if (_equipment.DefaultQuantity > 0)
            {
                lblDefaultInfo.Text = $"✓ {_equipment.DefaultQuantity} Included";
                lblDefaultInfo.Visible = true;
            }
            else
            {
                lblDefaultInfo.Visible = false;
            }

            // Update button states
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            btnMinus.Enabled = _equipment.RentalQuantity > 0;
            btnPlus.Enabled = _equipment.RentalQuantity < MAX_QUANTITY;

            // Visual feedback
            btnMinus.BackColor = btnMinus.Enabled
                ? Color.FromArgb(244, 67, 54)
                : Color.FromArgb(189, 189, 189);

            btnPlus.BackColor = btnPlus.Enabled
                ? Color.FromArgb(76, 175, 80)
                : Color.FromArgb(189, 189, 189);
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            if (_equipment.RentalQuantity < MAX_QUANTITY)
            {
                _equipment.RentalQuantity++;
                lblQuantity.Text = _equipment.RentalQuantity.ToString();
                UpdateButtonStates();

                // Notify parent
                QuantityChanged?.Invoke(this, EventArgs.Empty);

                // Visual feedback
                AnimateButton(btnPlus);
            }
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (_equipment.RentalQuantity > 0)
            {
                _equipment.RentalQuantity--;
                lblQuantity.Text = _equipment.RentalQuantity.ToString();
                UpdateButtonStates();

                // Notify parent
                QuantityChanged?.Invoke(this, EventArgs.Empty);

                // Visual feedback
                AnimateButton(btnMinus);
            }
        }

        private void AnimateButton(Button button)
        {
            // Simple scale animation
            Timer timer = new Timer();
            timer.Interval = 50;
            int count = 0;

            timer.Tick += (s, e) =>
            {
                count++;
                if (count == 1)
                {
                    button.Size = new Size(button.Width - 4, button.Height - 4);
                    button.Location = new Point(button.Location.X + 2, button.Location.Y + 2);
                }
                else if (count == 2)
                {
                    button.Size = new Size(button.Width + 4, button.Height + 4);
                    button.Location = new Point(button.Location.X - 2, button.Location.Y - 2);
                    timer.Stop();
                    timer.Dispose();
                }
            };

            timer.Start();
        }
    }
}