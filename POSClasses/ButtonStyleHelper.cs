using System.Drawing;
using System.Windows.Forms;

namespace KGHCashierPOS
{
    public static class ButtonStyleHelper
    {
        // Color scheme
        public static readonly Color DefaultColor = Color.FromArgb(64, 64, 64);
        public static readonly Color HoverColor = Color.FromArgb(90, 90, 90);
        public static readonly Color SelectedColor = Color.Orange;
        public static readonly Color PressedColor = Color.FromArgb(255, 140, 0); // Dark orange

        public static void ApplyGameButtonStyle(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = DefaultColor;
            button.ForeColor = Color.White;
            button.Cursor = Cursors.Hand;

            // Hover effect
            button.MouseEnter += (s, e) =>
            {
                if (button.BackColor == DefaultColor)
                {
                    button.BackColor = HoverColor;
                }
            };

            button.MouseLeave += (s, e) =>
            {
                if (button.BackColor == HoverColor)
                {
                    button.BackColor = DefaultColor;
                }
            };

            // Click effect
            button.MouseDown += (s, e) =>
            {
                if (button.BackColor != SelectedColor)
                {
                    button.BackColor = PressedColor;
                }
            };

            button.MouseUp += (s, e) =>
            {
                if (button.BackColor == PressedColor)
                {
                    button.BackColor = SelectedColor;
                }
            };
        }

        public static void ApplyDurationButtonStyle(Button button)
        {
            ApplyGameButtonStyle(button); // Same style as game buttons
        }

        public static void ApplyActionButtonStyle(Button button, Color actionColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = actionColor;
            button.ForeColor = Color.White;
            button.Cursor = Cursors.Hand;

            Color hoverColor = ControlPaint.Light(actionColor, 0.2f);
            Color pressedColor = ControlPaint.Dark(actionColor, 0.1f);

            button.MouseEnter += (s, e) => button.BackColor = hoverColor;
            button.MouseLeave += (s, e) => button.BackColor = actionColor;
            button.MouseDown += (s, e) => button.BackColor = pressedColor;
            button.MouseUp += (s, e) => button.BackColor = hoverColor;
        }

        public static void ResetGameButtons(params Button[] buttons)
        {
            foreach (var btn in buttons)
            {
                btn.BackColor = DefaultColor;
            }
        }
    }
}