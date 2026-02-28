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
    public partial class NumericKeyboardControl : UserControl
    {

        public TextBox TargetTextBox { get; set; }
        public event EventHandler KeyboardClosed;

        public NumericKeyboardControl()
        {
            InitializeComponent();
            InitializeKeyboard();
        }

        private void InitializeKeyboard()
        {
            this.Size = new Size(320, 280);
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.BorderStyle = BorderStyle.FixedSingle;

            int btnSize = 60;
            int spacing = 10;
            int startX = 10;
            int startY = 10;

            // Number layout
            string[,] layout = new string[,]
            {
                { "7", "8", "9" },
                { "4", "5", "6" },
                { "1", "2", "3" },
                { ".", "0", "00" }
            };

            // Create number buttons
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    string num = layout[row, col];
                    Button btn = CreateButton(num,
                        startX + col * (btnSize + spacing),
                        startY + row * (btnSize + spacing),
                        btnSize, btnSize);
                    btn.Click += (s, e) => AppendToTarget(num);
                    this.Controls.Add(btn);
                }
            }

            // Action buttons (right side)
            int rightX = startX + 3 * (btnSize + spacing);

            Button btnClear = CreateButton("CLR", rightX, startY, btnSize, btnSize);
            btnClear.BackColor = Color.FromArgb(244, 67, 54);
            btnClear.ForeColor = Color.White;
            btnClear.Click += (s, e) => ClearTarget();
            this.Controls.Add(btnClear);

            Button btnBack = CreateButton("⌫", rightX, startY + btnSize + spacing, btnSize, btnSize);
            btnBack.BackColor = Color.FromArgb(255, 152, 0);
            btnBack.ForeColor = Color.White;
            btnBack.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            btnBack.Click += (s, e) => Backspace();
            this.Controls.Add(btnBack);

            Button btnDone = CreateButton("✓", rightX, startY + 2 * (btnSize + spacing),
                btnSize, 2 * btnSize + spacing);
            btnDone.BackColor = Color.FromArgb(76, 175, 80);
            btnDone.ForeColor = Color.White;
            btnDone.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            btnDone.Click += (s, e) => CloseKeyboard();
            this.Controls.Add(btnDone);
        }

        private Button CreateButton(string text, int x, int y, int width, int height)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point(x, y);
            btn.Size = new Size(width, height);
            btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btn.BackColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.LightGray;
            btn.Cursor = Cursors.Hand;
            btn.TabStop = false;

            // Hover effect
            btn.MouseEnter += (s, e) =>
            {
                if (btn.BackColor == Color.White)
                    btn.BackColor = Color.FromArgb(230, 230, 230);
            };
            btn.MouseLeave += (s, e) =>
            {
                if (btn.BackColor == Color.FromArgb(230, 230, 230))
                    btn.BackColor = Color.White;
            };

            return btn;
        }

        private void AppendToTarget(string value)
        {
            if (TargetTextBox == null) return;

            // Prevent multiple decimal points
            if (value == "." && TargetTextBox.Text.Contains("."))
                return;

            // Insert at cursor position
            int cursorPos = TargetTextBox.SelectionStart;
            TargetTextBox.Text = TargetTextBox.Text.Insert(cursorPos, value);
            TargetTextBox.SelectionStart = cursorPos + value.Length;
            TargetTextBox.Focus();

            // Trigger TextChanged event
            TargetTextBox.Invoke(new Action(() =>
            {
                typeof(TextBox).GetMethod("OnTextChanged",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic)
                    ?.Invoke(TargetTextBox, new object[] { EventArgs.Empty });
            }));
        }

        private void Backspace()
        {
            if (TargetTextBox == null || TargetTextBox.Text.Length == 0) return;

            int cursorPos = TargetTextBox.SelectionStart;
            if (cursorPos > 0)
            {
                TargetTextBox.Text = TargetTextBox.Text.Remove(cursorPos - 1, 1);
                TargetTextBox.SelectionStart = cursorPos - 1;
            }
            TargetTextBox.Focus();
        }

        private void ClearTarget()
        {
            if (TargetTextBox == null) return;
            TargetTextBox.Clear();
            TargetTextBox.Focus();
        }

        private void CloseKeyboard()
        {
            this.Visible = false;
            KeyboardClosed?.Invoke(this, EventArgs.Empty);
        }

        private void NumericKeyboardControl_Load(object sender, EventArgs e)
        {

        }
    }
}
