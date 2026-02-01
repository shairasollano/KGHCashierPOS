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



    public partial class Form1 : Form
    {

        class GameSession
        {
            public string GameName { get; set; }
            public int TotalMinutes { get; set; }
            public decimal TotalPrice { get; set; }
        }

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
                    TotalPrice = priceToAdd
                };
            }

            RefreshListBox();
        }

        // REFRESH LISTBOX

        private void RefreshListBox()
        {
            lstSelectedGames.Items.Clear();
            totalAmount = 0;

            foreach (var session in activeSessions.Values)
            {
                int hours = session.TotalMinutes / 60;
                int mins = session.TotalMinutes % 60;

                string timeText =
                    hours > 0 && mins > 0 ? $"{hours} hr {mins} min" :
                    hours > 0 ? $"{hours} hr" :
                    $"{mins} min";

                lstSelectedGames.Items.Add(
                    $"{session.GameName} – {timeText} – ₱{session.TotalPrice}"
                );

                totalAmount += session.TotalPrice;
            }

            lblTotal.Text = "₱" + totalAmount.ToString("0.00");
        }



        // REMOVE SELECTED GAME
        private void btnRemoveGame_Click(object sender, EventArgs e)
        {
            if (lstSelectedGames.SelectedIndex == -1)
                return;

            string selectedText = lstSelectedGames.SelectedItem.ToString();
            string gameName = selectedText.Split('–')[0].Trim();

            activeSessions.Remove(gameName);
            RefreshListBox();
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
        // CODE HERE LEFT BLANK FOR FUTURE IMPLEMENTATION

        // EXTEND SESSION

        private void chkExtend_CheckedChanged(object sender, EventArgs e)
        {
            if (chkExtend.Checked)
                MessageBox.Show("Extension enabled.");
        }


    }
}
