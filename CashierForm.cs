using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace KGHCashierPOS
{

    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using KGHCashierPOS;


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
            paymentControl.LoadPaymentData(activeSessions, totalAmount);
            paymentControl.Visible = true;
            paymentControl.BringToFront();
        }

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

        private void CashierForm_Load(object sender, EventArgs e)
        {
            TestDatabaseConnection();
        }

        private void TestDatabaseConnection()
        {
            using (MySqlConnection conn =
                new MySqlConnection(Database.ConnectionString))
            {
                try
                {
                    conn.Open();
                    MessageBox.Show("Database connected successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error:\n" + ex.Message);
                }
            }
        }

    }
}
