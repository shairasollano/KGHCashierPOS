using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KGHCashierPOS
{
    public partial class EquipmentRentalControl : UserControl
    {
        private List<Equipment> _equipmentList;
        private string _gameName;

        public List<Equipment> SelectedEquipment
        {
            get { return _equipmentList; }
        }

        public decimal TotalEquipmentCost
        {
            get
            {
                decimal total = 0;
                foreach (var eq in _equipmentList)
                {
                    total += eq.RentalQuantity * eq.Price;
                }
                return total;
            }
        }

        public bool IsConfirmed { get; private set; }

        public EquipmentRentalControl()
        {
            InitializeComponent();
            SetupStyles();
        }

        private void SetupStyles()
        {
            // Confirm button hover
            btnConfirm.MouseEnter += (s, e) => btnConfirm.BackColor = Color.FromArgb(56, 142, 60);
            btnConfirm.MouseLeave += (s, e) => btnConfirm.BackColor = Color.FromArgb(76, 175, 80);

            // Cancel button hover
            btnCancel.MouseEnter += (s, e) => btnCancel.BackColor = Color.FromArgb(158, 158, 158);
            btnCancel.MouseLeave += (s, e) => btnCancel.BackColor = Color.FromArgb(189, 189, 189);
        }

        public void LoadEquipment(string gameName, List<Equipment> equipment)
        {
            _gameName = gameName;
            _equipmentList = equipment;

           
            lblTitle.Text = $"{gameName} - Equipment Rental";

            flowPanelEquipment.Controls.Clear();

            foreach (var eq in _equipmentList)
            {
                EquipmentItemControl itemControl = new EquipmentItemControl();
                itemControl.Equipment = eq;
                itemControl.QuantityChanged += ItemControl_QuantityChanged;

                itemControl.Width = flowPanelEquipment.ClientSize.Width - 10;

                flowPanelEquipment.Controls.Add(itemControl);
                this.Refresh();
            }

            UpdateTotal();
        }

        private void ItemControl_QuantityChanged(object sender, EventArgs e)
        {
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            lblTotalAmount.Text = PriceFormatter.Format(TotalEquipmentCost);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            IsConfirmed = true;

            // Hide this control (parent form will handle it)
            this.Visible = false;

            System.Diagnostics.Debug.WriteLine($"Equipment confirmed: {TotalEquipmentCost:C}");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsConfirmed = false;

            // Reset all quantities
            if (_equipmentList != null)
            {
                foreach (var eq in _equipmentList)
                {
                    eq.RentalQuantity = 0;
                }
            }

            this.Visible = false;

            System.Diagnostics.Debug.WriteLine("Equipment rental cancelled");
        }

        private void EquipmentRentalControl_Load(object sender, EventArgs e)
        {
            CenterInParent();
        }

        public void CenterInParent()
        {
            if (this.Parent != null)
            {
                this.Location = new Point(
                    (this.Parent.ClientSize.Width - this.Width) / 2,
                    (this.Parent.ClientSize.Height - this.Height) / 2
                );
            }
        }

    }
}