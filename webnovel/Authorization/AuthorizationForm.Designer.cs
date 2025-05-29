namespace bookservice
{
    partial class AuthorizationForm
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
            this.authTabControl = new System.Windows.Forms.TabControl();
            this.authTabPage = new System.Windows.Forms.TabPage();
            this.authPanel = new System.Windows.Forms.Panel();
            this.authButton = new System.Windows.Forms.Button();
            this.authPasswordTextBox = new System.Windows.Forms.TextBox();
            this.authLoginTextBox = new System.Windows.Forms.TextBox();
            this.authPasswordLabel = new System.Windows.Forms.Label();
            this.authLoginLabel = new System.Windows.Forms.Label();
            this.registerTabPage = new System.Windows.Forms.TabPage();
            this.registerPanel = new System.Windows.Forms.Panel();
            this.registerConfirmPasswordTextBox = new System.Windows.Forms.TextBox();
            this.registerConfirmPasswordLabel = new System.Windows.Forms.Label();
            this.registerButton = new System.Windows.Forms.Button();
            this.registerPasswordTextBox = new System.Windows.Forms.TextBox();
            this.registerLoginTextBox = new System.Windows.Forms.TextBox();
            this.registerPasswordLabel = new System.Windows.Forms.Label();
            this.registerLoginLabel = new System.Windows.Forms.Label();
            this.accountTabPage = new System.Windows.Forms.TabPage();
            this.accountPanel = new System.Windows.Forms.Panel();
            this.logoutButton = new System.Windows.Forms.Button();
            this.userInfoLabel = new System.Windows.Forms.Label();
            this.authTabControl.SuspendLayout();
            this.authTabPage.SuspendLayout();
            this.authPanel.SuspendLayout();
            this.registerTabPage.SuspendLayout();
            this.registerPanel.SuspendLayout();
            this.accountTabPage.SuspendLayout();
            this.accountPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // authTabControl
            // 
            this.authTabControl.Controls.Add(this.authTabPage);
            this.authTabControl.Controls.Add(this.registerTabPage);
            this.authTabControl.Controls.Add(this.accountTabPage);
            this.authTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.authTabControl.Location = new System.Drawing.Point(0, 0);
            this.authTabControl.Margin = new System.Windows.Forms.Padding(8);
            this.authTabControl.Name = "authTabControl";
            this.authTabControl.SelectedIndex = 0;
            this.authTabControl.Size = new System.Drawing.Size(1438, 766);
            this.authTabControl.TabIndex = 0;
            // 
            // authTabPage
            // 
            this.authTabPage.BackColor = System.Drawing.Color.Transparent;
            this.authTabPage.Controls.Add(this.authPanel);
            this.authTabPage.Location = new System.Drawing.Point(8, 79);
            this.authTabPage.Margin = new System.Windows.Forms.Padding(8);
            this.authTabPage.Name = "authTabPage";
            this.authTabPage.Padding = new System.Windows.Forms.Padding(8);
            this.authTabPage.Size = new System.Drawing.Size(1422, 679);
            this.authTabPage.TabIndex = 0;
            this.authTabPage.Text = "Авторизация";
            // 
            // authPanel
            // 
            this.authPanel.Controls.Add(this.authButton);
            this.authPanel.Controls.Add(this.authPasswordTextBox);
            this.authPanel.Controls.Add(this.authLoginTextBox);
            this.authPanel.Controls.Add(this.authPasswordLabel);
            this.authPanel.Controls.Add(this.authLoginLabel);
            this.authPanel.Location = new System.Drawing.Point(438, 120);
            this.authPanel.Margin = new System.Windows.Forms.Padding(8);
            this.authPanel.Name = "authPanel";
            this.authPanel.Size = new System.Drawing.Size(645, 249);
            this.authPanel.TabIndex = 0;
            // 
            // authButton
            // 
            this.authButton.Location = new System.Drawing.Point(19, 150);
            this.authButton.Name = "authButton";
            this.authButton.Size = new System.Drawing.Size(621, 93);
            this.authButton.TabIndex = 4;
            this.authButton.Text = "Авторизоваться";
            this.authButton.UseVisualStyleBackColor = true;
            this.authButton.Click += new System.EventHandler(this.AuthButton_Click);
            // 
            // authPasswordTextBox
            // 
            this.authPasswordTextBox.Location = new System.Drawing.Point(177, 68);
            this.authPasswordTextBox.Name = "authPasswordTextBox";
            this.authPasswordTextBox.PasswordChar = '*';
            this.authPasswordTextBox.Size = new System.Drawing.Size(463, 72);
            this.authPasswordTextBox.TabIndex = 3;
            // 
            // authLoginTextBox
            // 
            this.authLoginTextBox.Location = new System.Drawing.Point(177, 11);
            this.authLoginTextBox.Name = "authLoginTextBox";
            this.authLoginTextBox.Size = new System.Drawing.Size(463, 72);
            this.authLoginTextBox.TabIndex = 2;
            // 
            // authPasswordLabel
            // 
            this.authPasswordLabel.AutoSize = true;
            this.authPasswordLabel.Location = new System.Drawing.Point(8, 66);
            this.authPasswordLabel.Name = "authPasswordLabel";
            this.authPasswordLabel.Size = new System.Drawing.Size(252, 65);
            this.authPasswordLabel.TabIndex = 1;
            this.authPasswordLabel.Text = "Пароль:";
            // 
            // authLoginLabel
            // 
            this.authLoginLabel.AutoSize = true;
            this.authLoginLabel.Location = new System.Drawing.Point(8, 16);
            this.authLoginLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.authLoginLabel.Name = "authLoginLabel";
            this.authLoginLabel.Size = new System.Drawing.Size(216, 65);
            this.authLoginLabel.TabIndex = 0;
            this.authLoginLabel.Text = "Логин:";
            // 
            // registerTabPage
            // 
            this.registerTabPage.Controls.Add(this.registerPanel);
            this.registerTabPage.Location = new System.Drawing.Point(8, 79);
            this.registerTabPage.Margin = new System.Windows.Forms.Padding(8);
            this.registerTabPage.Name = "registerTabPage";
            this.registerTabPage.Padding = new System.Windows.Forms.Padding(8);
            this.registerTabPage.Size = new System.Drawing.Size(1422, 679);
            this.registerTabPage.TabIndex = 1;
            this.registerTabPage.Text = "Регистрация";
            this.registerTabPage.UseVisualStyleBackColor = true;
            // 
            // registerPanel
            // 
            this.registerPanel.Controls.Add(this.registerConfirmPasswordTextBox);
            this.registerPanel.Controls.Add(this.registerConfirmPasswordLabel);
            this.registerPanel.Controls.Add(this.registerButton);
            this.registerPanel.Controls.Add(this.registerPasswordTextBox);
            this.registerPanel.Controls.Add(this.registerLoginTextBox);
            this.registerPanel.Controls.Add(this.registerPasswordLabel);
            this.registerPanel.Controls.Add(this.registerLoginLabel);
            this.registerPanel.Location = new System.Drawing.Point(373, 25);
            this.registerPanel.Margin = new System.Windows.Forms.Padding(8);
            this.registerPanel.Name = "registerPanel";
            this.registerPanel.Size = new System.Drawing.Size(740, 534);
            this.registerPanel.TabIndex = 1;
            // 
            // registerConfirmPasswordTextBox
            // 
            this.registerConfirmPasswordTextBox.Location = new System.Drawing.Point(57, 309);
            this.registerConfirmPasswordTextBox.Name = "registerConfirmPasswordTextBox";
            this.registerConfirmPasswordTextBox.PasswordChar = '*';
            this.registerConfirmPasswordTextBox.Size = new System.Drawing.Size(631, 72);
            this.registerConfirmPasswordTextBox.TabIndex = 5;
            // 
            // registerConfirmPasswordLabel
            // 
            this.registerConfirmPasswordLabel.AutoSize = true;
            this.registerConfirmPasswordLabel.Location = new System.Drawing.Point(46, 244);
            this.registerConfirmPasswordLabel.Name = "registerConfirmPasswordLabel";
            this.registerConfirmPasswordLabel.Size = new System.Drawing.Size(552, 65);
            this.registerConfirmPasswordLabel.TabIndex = 4;
            this.registerConfirmPasswordLabel.Text = "Повторите пароль:";
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(57, 436);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(631, 93);
            this.registerButton.TabIndex = 4;
            this.registerButton.Text = "Зарегистрироваться";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // registerPasswordTextBox
            // 
            this.registerPasswordTextBox.Location = new System.Drawing.Point(57, 180);
            this.registerPasswordTextBox.Name = "registerPasswordTextBox";
            this.registerPasswordTextBox.PasswordChar = '*';
            this.registerPasswordTextBox.Size = new System.Drawing.Size(631, 72);
            this.registerPasswordTextBox.TabIndex = 3;
            // 
            // registerLoginTextBox
            // 
            this.registerLoginTextBox.Location = new System.Drawing.Point(57, 58);
            this.registerLoginTextBox.Name = "registerLoginTextBox";
            this.registerLoginTextBox.Size = new System.Drawing.Size(631, 72);
            this.registerLoginTextBox.TabIndex = 2;
            // 
            // registerPasswordLabel
            // 
            this.registerPasswordLabel.AutoSize = true;
            this.registerPasswordLabel.Location = new System.Drawing.Point(46, 122);
            this.registerPasswordLabel.Name = "registerPasswordLabel";
            this.registerPasswordLabel.Size = new System.Drawing.Size(252, 65);
            this.registerPasswordLabel.TabIndex = 1;
            this.registerPasswordLabel.Text = "Пароль:";
            // 
            // registerLoginLabel
            // 
            this.registerLoginLabel.AutoSize = true;
            this.registerLoginLabel.Location = new System.Drawing.Point(46, 4);
            this.registerLoginLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.registerLoginLabel.Name = "registerLoginLabel";
            this.registerLoginLabel.Size = new System.Drawing.Size(216, 65);
            this.registerLoginLabel.TabIndex = 0;
            this.registerLoginLabel.Text = "Логин:";
            // 
            // accountTabPage
            // 
            this.accountTabPage.Controls.Add(this.accountPanel);
            this.accountTabPage.Location = new System.Drawing.Point(8, 79);
            this.accountTabPage.Margin = new System.Windows.Forms.Padding(8);
            this.accountTabPage.Name = "accountTabPage";
            this.accountTabPage.Padding = new System.Windows.Forms.Padding(8);
            this.accountTabPage.Size = new System.Drawing.Size(1422, 679);
            this.accountTabPage.TabIndex = 2;
            this.accountTabPage.Text = "Аккаунт";
            this.accountTabPage.UseVisualStyleBackColor = true;
            // 
            // accountPanel
            // 
            this.accountPanel.Controls.Add(this.logoutButton);
            this.accountPanel.Controls.Add(this.userInfoLabel);
            this.accountPanel.Location = new System.Drawing.Point(139, 59);
            this.accountPanel.Margin = new System.Windows.Forms.Padding(8);
            this.accountPanel.Name = "accountPanel";
            this.accountPanel.Size = new System.Drawing.Size(983, 560);
            this.accountPanel.TabIndex = 2;
            // 
            // logoutButton
            // 
            this.logoutButton.Location = new System.Drawing.Point(38, 443);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(898, 93);
            this.logoutButton.TabIndex = 5;
            this.logoutButton.Text = "Выйти из аккаунта";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.LogoutButton_Click);
            // 
            // userInfoLabel
            // 
            this.userInfoLabel.AutoSize = true;
            this.userInfoLabel.Location = new System.Drawing.Point(41, 86);
            this.userInfoLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.userInfoLabel.Name = "userInfoLabel";
            this.userInfoLabel.Size = new System.Drawing.Size(884, 65);
            this.userInfoLabel.TabIndex = 0;
            this.userInfoLabel.Text = "Информация о пользователе...";
            // 
            // AuthorizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(33F, 65F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1438, 766);
            this.Controls.Add(this.authTabControl);
            this.Font = new System.Drawing.Font("Verdana", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(8);
            this.MaximizeBox = false;
            this.Name = "AuthorizationForm";
            this.Text = "Авторизация";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AuthorizationForm_FormClosed);
            this.Load += new System.EventHandler(this.AuthorizationForm_Load);
            this.authTabControl.ResumeLayout(false);
            this.authTabPage.ResumeLayout(false);
            this.authPanel.ResumeLayout(false);
            this.authPanel.PerformLayout();
            this.registerTabPage.ResumeLayout(false);
            this.registerPanel.ResumeLayout(false);
            this.registerPanel.PerformLayout();
            this.accountTabPage.ResumeLayout(false);
            this.accountPanel.ResumeLayout(false);
            this.accountPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl authTabControl;
        private System.Windows.Forms.TabPage authTabPage;
        private System.Windows.Forms.TabPage registerTabPage;
        private System.Windows.Forms.TabPage accountTabPage;
        private System.Windows.Forms.Panel authPanel;
        private System.Windows.Forms.TextBox authLoginTextBox;
        private System.Windows.Forms.Label authPasswordLabel;
        private System.Windows.Forms.Label authLoginLabel;
        private System.Windows.Forms.Button authButton;
        private System.Windows.Forms.TextBox authPasswordTextBox;
        private System.Windows.Forms.Panel registerPanel;
        private System.Windows.Forms.TextBox registerConfirmPasswordTextBox;
        private System.Windows.Forms.Label registerConfirmPasswordLabel;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.TextBox registerPasswordTextBox;
        private System.Windows.Forms.TextBox registerLoginTextBox;
        private System.Windows.Forms.Label registerPasswordLabel;
        private System.Windows.Forms.Label registerLoginLabel;
        private System.Windows.Forms.Panel accountPanel;
        private System.Windows.Forms.Button logoutButton;
        private System.Windows.Forms.Label userInfoLabel;
    }
}