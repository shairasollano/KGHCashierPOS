namespace KGHCashierPOS
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnBilliards = new System.Windows.Forms.Button();
            this.btnTableTennis = new System.Windows.Forms.Button();
            this.btnScooter = new System.Windows.Forms.Button();
            this.btnBadminton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblSelect = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btn30min = new System.Windows.Forms.Button();
            this.btn1hour = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkExtend = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstSelectedGames = new System.Windows.Forms.ListBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnProceedPayment = new System.Windows.Forms.Button();
            this.btnRemoveGame = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAddGame = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtOrderNumber = new System.Windows.Forms.TextBox();
            this.btn1 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btn3 = new System.Windows.Forms.Button();
            this.btn4 = new System.Windows.Forms.Button();
            this.btn5 = new System.Windows.Forms.Button();
            this.btn6 = new System.Windows.Forms.Button();
            this.btn7 = new System.Windows.Forms.Button();
            this.btn8 = new System.Windows.Forms.Button();
            this.btn9 = new System.Windows.Forms.Button();
            this.btnBackspace = new System.Windows.Forms.Button();
            this.btn0 = new System.Windows.Forms.Button();
            this.btnEnter = new System.Windows.Forms.Button();
            this.totalValue = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(25)))));
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblDate);
            this.panel1.Controls.Add(this.lblTime);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1289, 71);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nirmala Text", 8F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(967, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 21);
            this.label2.TabIndex = 7;
            this.label2.Text = "Logged in as: ";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Nirmala Text", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.SystemColors.Control;
            this.lblDate.Location = new System.Drawing.Point(73, 13);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(68, 28);
            this.lblDate.TabIndex = 6;
            this.lblDate.Text = "DATE:";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Nirmala Text", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTime.Location = new System.Drawing.Point(73, 38);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(65, 28);
            this.lblTime.TabIndex = 5;
            this.lblTime.Text = "TIME:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(9, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(59, 63);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // btnBilliards
            // 
            this.btnBilliards.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(25)))));
            this.btnBilliards.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBilliards.BackgroundImage")));
            this.btnBilliards.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBilliards.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnBilliards.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnBilliards.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnBilliards.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBilliards.Location = new System.Drawing.Point(19, 47);
            this.btnBilliards.Name = "btnBilliards";
            this.btnBilliards.Size = new System.Drawing.Size(140, 100);
            this.btnBilliards.TabIndex = 1;
            this.btnBilliards.UseVisualStyleBackColor = false;
            this.btnBilliards.Click += new System.EventHandler(this.btnBilliards_Click);
            // 
            // btnTableTennis
            // 
            this.btnTableTennis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(25)))));
            this.btnTableTennis.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTableTennis.BackgroundImage")));
            this.btnTableTennis.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTableTennis.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnTableTennis.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnTableTennis.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnTableTennis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTableTennis.Location = new System.Drawing.Point(165, 148);
            this.btnTableTennis.Name = "btnTableTennis";
            this.btnTableTennis.Size = new System.Drawing.Size(140, 100);
            this.btnTableTennis.TabIndex = 2;
            this.btnTableTennis.UseVisualStyleBackColor = false;
            this.btnTableTennis.Click += new System.EventHandler(this.btnTableTennis_Click);
            // 
            // btnScooter
            // 
            this.btnScooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(25)))));
            this.btnScooter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnScooter.BackgroundImage")));
            this.btnScooter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnScooter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnScooter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnScooter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnScooter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScooter.Location = new System.Drawing.Point(165, 47);
            this.btnScooter.Name = "btnScooter";
            this.btnScooter.Size = new System.Drawing.Size(140, 100);
            this.btnScooter.TabIndex = 6;
            this.btnScooter.UseVisualStyleBackColor = false;
            this.btnScooter.Click += new System.EventHandler(this.btnScooter_Click);
            // 
            // btnBadminton
            // 
            this.btnBadminton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(25)))));
            this.btnBadminton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBadminton.BackgroundImage")));
            this.btnBadminton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBadminton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnBadminton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnBadminton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnBadminton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBadminton.Location = new System.Drawing.Point(19, 148);
            this.btnBadminton.Name = "btnBadminton";
            this.btnBadminton.Size = new System.Drawing.Size(140, 100);
            this.btnBadminton.TabIndex = 8;
            this.btnBadminton.UseVisualStyleBackColor = false;
            this.btnBadminton.Click += new System.EventHandler(this.btnBadminton_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.panel2.Controls.Add(this.lblSelect);
            this.panel2.Controls.Add(this.btnBilliards);
            this.panel2.Controls.Add(this.btnTableTennis);
            this.panel2.Controls.Add(this.btnScooter);
            this.panel2.Controls.Add(this.btnBadminton);
            this.panel2.Location = new System.Drawing.Point(50, 99);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(330, 275);
            this.panel2.TabIndex = 9;
            // 
            // lblSelect
            // 
            this.lblSelect.AutoSize = true;
            this.lblSelect.Font = new System.Drawing.Font("Nirmala Text", 9F, System.Drawing.FontStyle.Bold);
            this.lblSelect.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblSelect.Location = new System.Drawing.Point(19, 13);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(131, 25);
            this.lblSelect.TabIndex = 7;
            this.lblSelect.Text = "SELECT GAME";
            // 
            // btn30min
            // 
            this.btn30min.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(25)))));
            this.btn30min.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn30min.BackgroundImage")));
            this.btn30min.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn30min.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btn30min.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btn30min.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btn30min.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn30min.Location = new System.Drawing.Point(19, 43);
            this.btn30min.Name = "btn30min";
            this.btn30min.Size = new System.Drawing.Size(140, 100);
            this.btn30min.TabIndex = 10;
            this.btn30min.UseVisualStyleBackColor = false;
            this.btn30min.Click += new System.EventHandler(this.btn30Min_Click);
            // 
            // btn1hour
            // 
            this.btn1hour.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(25)))));
            this.btn1hour.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn1hour.BackgroundImage")));
            this.btn1hour.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn1hour.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btn1hour.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btn1hour.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btn1hour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn1hour.Location = new System.Drawing.Point(165, 43);
            this.btn1hour.Name = "btn1hour";
            this.btn1hour.Size = new System.Drawing.Size(140, 100);
            this.btn1hour.TabIndex = 11;
            this.btn1hour.UseVisualStyleBackColor = false;
            this.btn1hour.Click += new System.EventHandler(this.btn1Hour_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.panel3.Controls.Add(this.chkExtend);
            this.panel3.Controls.Add(this.btn30min);
            this.panel3.Controls.Add(this.btn1hour);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(50, 408);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(330, 191);
            this.panel3.TabIndex = 10;
            // 
            // chkExtend
            // 
            this.chkExtend.AutoSize = true;
            this.chkExtend.Font = new System.Drawing.Font("Nirmala Text", 10F, System.Drawing.FontStyle.Bold);
            this.chkExtend.Location = new System.Drawing.Point(21, 149);
            this.chkExtend.Name = "chkExtend";
            this.chkExtend.Size = new System.Drawing.Size(179, 32);
            this.chkExtend.TabIndex = 21;
            this.chkExtend.Text = "Extend Session";
            this.chkExtend.UseVisualStyleBackColor = true;
            this.chkExtend.Click += new System.EventHandler(this.chkExtend_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Nirmala Text", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(16, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "TIME DURATION";
            // 
            // lstSelectedGames
            // 
            this.lstSelectedGames.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSelectedGames.FormattingEnabled = true;
            this.lstSelectedGames.ItemHeight = 29;
            this.lstSelectedGames.Location = new System.Drawing.Point(855, 121);
            this.lstSelectedGames.Name = "lstSelectedGames";
            this.lstSelectedGames.Size = new System.Drawing.Size(359, 584);
            this.lstSelectedGames.TabIndex = 11;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.BackColor = System.Drawing.SystemColors.Window;
            this.lblTotal.Font = new System.Drawing.Font("Nirmala Text", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(967, 645);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(129, 32);
            this.lblTotal.TabIndex = 12;
            this.lblTotal.Text = "totalValue";
            // 
            // btnProceedPayment
            // 
            this.btnProceedPayment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(162)))), ((int)(((byte)(166)))));
            this.btnProceedPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProceedPayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProceedPayment.Location = new System.Drawing.Point(489, 590);
            this.btnProceedPayment.Name = "btnProceedPayment";
            this.btnProceedPayment.Size = new System.Drawing.Size(258, 54);
            this.btnProceedPayment.TabIndex = 14;
            this.btnProceedPayment.Text = "PROCEED TO PAYMENT";
            this.btnProceedPayment.UseVisualStyleBackColor = false;
            // 
            // btnRemoveGame
            // 
            this.btnRemoveGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(59)))), ((int)(((byte)(114)))));
            this.btnRemoveGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Bold);
            this.btnRemoveGame.Location = new System.Drawing.Point(622, 543);
            this.btnRemoveGame.Name = "btnRemoveGame";
            this.btnRemoveGame.Size = new System.Drawing.Size(125, 41);
            this.btnRemoveGame.TabIndex = 15;
            this.btnRemoveGame.Text = "REMOVE SELECTED GAME";
            this.btnRemoveGame.UseVisualStyleBackColor = false;
            this.btnRemoveGame.Click += new System.EventHandler(this.btnRemoveGame_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Nirmala Text", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(864, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(202, 32);
            this.label5.TabIndex = 16;
            this.label5.Text = "GAME SELECTED";
            // 
            // btnAddGame
            // 
            this.btnAddGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(177)))), ((int)(((byte)(56)))));
            this.btnAddGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Bold);
            this.btnAddGame.Location = new System.Drawing.Point(489, 543);
            this.btnAddGame.Name = "btnAddGame";
            this.btnAddGame.Size = new System.Drawing.Size(127, 41);
            this.btnAddGame.TabIndex = 17;
            this.btnAddGame.Text = "ADD SELECTED GAME";
            this.btnAddGame.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Nirmala Text", 10F);
            this.label6.Location = new System.Drawing.Point(488, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(190, 28);
            this.label6.TabIndex = 18;
            this.label6.Text = "Enter Order Number";
            // 
            // txtOrderNumber
            // 
            this.txtOrderNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOrderNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.txtOrderNumber.Location = new System.Drawing.Point(489, 163);
            this.txtOrderNumber.Name = "txtOrderNumber";
            this.txtOrderNumber.Size = new System.Drawing.Size(258, 44);
            this.txtOrderNumber.TabIndex = 19;
            // 
            // btn1
            // 
            this.btn1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn1.Location = new System.Drawing.Point(489, 219);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(82, 75);
            this.btn1.TabIndex = 21;
            this.btn1.Text = "1";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.NumberButton_Click);
            // 
            // btn2
            // 
            this.btn2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn2.Location = new System.Drawing.Point(577, 219);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(82, 75);
            this.btn2.TabIndex = 22;
            this.btn2.Text = "2";
            this.btn2.UseVisualStyleBackColor = true;
            this.btn2.Click += new System.EventHandler(this.NumberButton_Click);
            // 
            // btn3
            // 
            this.btn3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn3.Location = new System.Drawing.Point(665, 219);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(82, 75);
            this.btn3.TabIndex = 23;
            this.btn3.Text = "3";
            this.btn3.UseVisualStyleBackColor = true;
            this.btn3.Click += new System.EventHandler(this.NumberButton_Click);
            // 
            // btn4
            // 
            this.btn4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn4.Location = new System.Drawing.Point(489, 300);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(82, 75);
            this.btn4.TabIndex = 24;
            this.btn4.Text = "4";
            this.btn4.UseVisualStyleBackColor = true;
            this.btn4.Click += new System.EventHandler(this.NumberButton_Click);
            // 
            // btn5
            // 
            this.btn5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn5.Location = new System.Drawing.Point(577, 300);
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(82, 75);
            this.btn5.TabIndex = 25;
            this.btn5.Text = "5";
            this.btn5.UseVisualStyleBackColor = true;
            this.btn5.Click += new System.EventHandler(this.NumberButton_Click);
            // 
            // btn6
            // 
            this.btn6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn6.Location = new System.Drawing.Point(665, 300);
            this.btn6.Name = "btn6";
            this.btn6.Size = new System.Drawing.Size(82, 75);
            this.btn6.TabIndex = 26;
            this.btn6.Text = "6";
            this.btn6.UseVisualStyleBackColor = true;
            this.btn6.Click += new System.EventHandler(this.NumberButton_Click);
            // 
            // btn7
            // 
            this.btn7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn7.Location = new System.Drawing.Point(489, 381);
            this.btn7.Name = "btn7";
            this.btn7.Size = new System.Drawing.Size(82, 75);
            this.btn7.TabIndex = 27;
            this.btn7.Text = "7";
            this.btn7.UseVisualStyleBackColor = true;
            this.btn7.Click += new System.EventHandler(this.NumberButton_Click);
            // 
            // btn8
            // 
            this.btn8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn8.Location = new System.Drawing.Point(577, 381);
            this.btn8.Name = "btn8";
            this.btn8.Size = new System.Drawing.Size(82, 75);
            this.btn8.TabIndex = 28;
            this.btn8.Text = "8";
            this.btn8.UseVisualStyleBackColor = true;
            this.btn8.Click += new System.EventHandler(this.NumberButton_Click);
            // 
            // btn9
            // 
            this.btn9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn9.Location = new System.Drawing.Point(665, 381);
            this.btn9.Name = "btn9";
            this.btn9.Size = new System.Drawing.Size(82, 75);
            this.btn9.TabIndex = 29;
            this.btn9.Text = "9";
            this.btn9.UseVisualStyleBackColor = true;
            this.btn9.Click += new System.EventHandler(this.NumberButton_Click);
            // 
            // btnBackspace
            // 
            this.btnBackspace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(59)))), ((int)(((byte)(114)))));
            this.btnBackspace.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBackspace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackspace.Font = new System.Drawing.Font("Nirmala Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackspace.Location = new System.Drawing.Point(577, 462);
            this.btnBackspace.Name = "btnBackspace";
            this.btnBackspace.Size = new System.Drawing.Size(82, 75);
            this.btnBackspace.TabIndex = 30;
            this.btnBackspace.Text = "⌫";
            this.btnBackspace.UseVisualStyleBackColor = false;
            this.btnBackspace.Click += new System.EventHandler(this.btnBackspace_Click);
            // 
            // btn0
            // 
            this.btn0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn0.Location = new System.Drawing.Point(489, 462);
            this.btn0.Name = "btn0";
            this.btn0.Size = new System.Drawing.Size(82, 75);
            this.btn0.TabIndex = 31;
            this.btn0.Text = "0";
            this.btn0.UseVisualStyleBackColor = true;
            this.btn0.Click += new System.EventHandler(this.NumberButton_Click);
            // 
            // btnEnter
            // 
            this.btnEnter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(162)))), ((int)(((byte)(166)))));
            this.btnEnter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnter.Font = new System.Drawing.Font("Nirmala Text", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnter.Location = new System.Drawing.Point(665, 462);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(82, 75);
            this.btnEnter.TabIndex = 32;
            this.btnEnter.Text = "Enter";
            this.btnEnter.UseVisualStyleBackColor = false;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // totalValue
            // 
            this.totalValue.AutoSize = true;
            this.totalValue.BackColor = System.Drawing.SystemColors.Window;
            this.totalValue.Font = new System.Drawing.Font("Nirmala Text", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalValue.Location = new System.Drawing.Point(876, 645);
            this.totalValue.Name = "totalValue";
            this.totalValue.Size = new System.Drawing.Size(99, 32);
            this.totalValue.TabIndex = 33;
            this.totalValue.Text = "TOTAL: ";
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1252, 717);
            this.Controls.Add(this.totalValue);
            this.Controls.Add(this.btnEnter);
            this.Controls.Add(this.btn0);
            this.Controls.Add(this.btnBackspace);
            this.Controls.Add(this.btn9);
            this.Controls.Add(this.btn8);
            this.Controls.Add(this.btn7);
            this.Controls.Add(this.btn6);
            this.Controls.Add(this.btn5);
            this.Controls.Add(this.btn4);
            this.Controls.Add(this.btn3);
            this.Controls.Add(this.btn2);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.txtOrderNumber);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnAddGame);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnRemoveGame);
            this.Controls.Add(this.btnProceedPayment);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lstSelectedGames);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Name = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnBilliards;
        private System.Windows.Forms.Button btnTableTennis;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Button btnScooter;
        private System.Windows.Forms.Button btnBadminton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblSelect;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btn30min;
        private System.Windows.Forms.Button btn1hour;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstSelectedGames;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnProceedPayment;
        private System.Windows.Forms.Button btnRemoveGame;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAddGame;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtOrderNumber;
        private System.Windows.Forms.CheckBox chkExtend;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Button btn4;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Button btn6;
        private System.Windows.Forms.Button btn7;
        private System.Windows.Forms.Button btn8;
        private System.Windows.Forms.Button btn9;
        private System.Windows.Forms.Button btnBackspace;
        private System.Windows.Forms.Button btn0;
        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.Label totalValue;
    }
}

