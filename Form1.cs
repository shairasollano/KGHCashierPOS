using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KGHCashierPOS
{

    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using GHCashierPOS;



    public partial class Form1 : Form
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

        public Form1()
        {
            InitializeComponent();
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

                DateTime endTime = session.StartTime.AddMinutes(session.TotalMinutes);

                ListViewItem item = new ListViewItem(session.GameName);
                item.SubItems.Add(durationText);
                item.SubItems.Add("₱" + session.TotalPrice.ToString("0.00"));
                item.SubItems.Add(session.StartTime.ToString("hh:mm tt"));
                item.SubItems.Add(endTime.ToString("hh:mm tt"));
                item.SubItems.Add("₱" + session.TotalPrice.ToString("0.00"));

                lvSelectedGames.Items.Add(item);

                totalAmount += session.TotalPrice;
            }

            lblTotal.Text = "₱" + totalAmount.ToString("0.00");
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

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (txtOrderNumber.Text == "")
            {
                MessageBox.Show("Enter order number.");
                return;
            }

            MessageBox.Show("Order number accepted.");
        }

        // PROCEED TO PAYMENT

        private void btnProceedPayment_Click(object sender, EventArgs e)
        {
            paymentControl1.LoadPaymentData(activeSessions, totalAmount);
            paymentControl1.Visible = true;
            paymentControl1.BringToFront();
        }

        public void ClosePayment()
        {
            paymentControl1.Visible = false;
        }


        // EXTEND SESSION

        private void chkExtend_CheckedChanged(object sender, EventArgs e)
        {
            if (chkExtend.Checked)
                MessageBox.Show("Extension enabled.");
        }


    }
}
