using System;
using System.Drawing;
using System.Windows.Forms;

namespace bookservice {
    public partial class AuthorizationForm : Form {
        private CatalogForm catalogForm;
        private User user;
        private AuthorizationSQL authorizationSql;
        public AuthorizationForm() {
            InitializeComponent();
        }
        private void AuthorizationForm_Load(object sender, EventArgs e) {
            catalogForm = (CatalogForm)Owner;
            user = catalogForm.User;
            authorizationSql = new AuthorizationSQL(user);
            UpdateTabPages();
            UpdateAccountPageInfo();
            CenterPanelOnTabPage(authPanel, authTabPage);
            CenterPanelOnTabPage(registerPanel, registerTabPage);
            CenterPanelOnTabPage(accountPanel, accountTabPage);
        }
        private void CenterPanelOnTabPage(Panel panel, TabPage tabPage) {
            if (panel != null && tabPage != null) {
                int x = (tabPage.ClientSize.Width - panel.Width) / 2;
                int y = (tabPage.ClientSize.Height - panel.Height) / 2 - 30;
                panel.Location = new Point(Math.Max(0, x), Math.Max(0, y));
            }
        }
        private void UpdateTabPages() {
            if (authTabControl.TabPages.Contains(authTabPage))
                authTabControl.TabPages.Remove(authTabPage);
            if (authTabControl.TabPages.Contains(registerTabPage))
                authTabControl.TabPages.Remove(registerTabPage);
            if (authTabControl.TabPages.Contains(accountTabPage))
                authTabControl.TabPages.Remove(accountTabPage);
            if (user != null && user.Id != 0) {
                if (accountTabPage.Parent == null)
                    authTabControl.TabPages.Add(accountTabPage);
                authTabControl.SelectedTab = accountTabPage;
            } else {
                if (authTabPage.Parent == null)
                    authTabControl.TabPages.Add(authTabPage);
                if (registerTabPage.Parent == null)
                    authTabControl.TabPages.Add(registerTabPage);
                authTabControl.SelectedTab = authTabPage;
            }
        }
        private void UpdateAccountPageInfo() {
            if (user != null && user.Id != 0) {
                userInfoLabel.Text = $"Логин: {user.Login}\nСтатус: {(user.IsWriter ? "Писатель" : "Читатель")}";
                becomeWriterButton.Visible = !user.IsWriter;
            } else {
                userInfoLabel.Text = "Вы не авторизованы.";
                becomeWriterButton.Visible = false;
            }
        }
        private void AuthButton_Click(object sender, EventArgs e) {
            string login = authLoginTextBox.Text;
            string password = authPasswordTextBox.Text;
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password)) {
                MessageBox.Show("Пожалуйста, введите логин и пароль.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            User loggedInUser = authorizationSql.CheckLogin(login, password);
            if (loggedInUser != null) {
                user = loggedInUser;
                user.Role = user.IsWriter ? UserRole.WN_WRITER : UserRole.WN_READER;
                authorizationSql.updateUser(user);
                if (catalogForm != null)
                    catalogForm.User = user;
                MessageBox.Show("Авторизация прошла успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateTabPages();
                UpdateAccountPageInfo();
                authLoginTextBox.Clear();
                authPasswordTextBox.Clear();
            } else {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                authPasswordTextBox.Clear();
            }
        }
        private void RegisterButton_Click(object sender, EventArgs e) {
            string login = registerLoginTextBox.Text;
            string password = registerPasswordTextBox.Text;
            string confirmPassword = registerConfirmPasswordTextBox.Text;
            bool isWriter = registerIsWriterCheckBox.Checked;
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword)) {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (password != confirmPassword) {
                MessageBox.Show("Пароли не совпадают.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                registerPasswordTextBox.Clear();
                registerConfirmPasswordTextBox.Clear();
                registerPasswordTextBox.Focus();
                return;
            }
            User registeredUser = authorizationSql.RegisterUser(login, password, isWriter);
            if (registeredUser != null) {
                this.user = registeredUser;
                this.user.Role = this.user.IsWriter ? UserRole.WN_WRITER : UserRole.WN_READER;
                authorizationSql.updateUser(this.user);
                if (catalogForm != null)
                    catalogForm.User = this.user;
                MessageBox.Show("Регистрация прошла успешно! Вы авторизованы.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateTabPages();
                UpdateAccountPageInfo();
                registerLoginTextBox.Clear();
                registerPasswordTextBox.Clear();
                registerConfirmPasswordTextBox.Clear();
                registerIsWriterCheckBox.Checked = false;
            } else {
                registerLoginTextBox.Focus();
            }
        }
        private void BecomeWriterButton_Click(object sender, EventArgs e) {
            if (user == null || user.Id == 0) {
                MessageBox.Show("Сначала авторизуйтесь.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = MessageBox.Show("Вы уверены, что хотите стать писателем? Это действие нельзя будет отменить через интерфейс.",
                                                 "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) {
                if (authorizationSql.BecomeWriter(user.Id)) {
                    user.IsWriter = true;
                    user.Role = UserRole.WN_WRITER;
                    authorizationSql.updateUser(this.user);
                    if (catalogForm != null)
                        catalogForm.User = this.user;
                    MessageBox.Show("Поздравляем! Теперь вы писатель.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateAccountPageInfo();
                } else {
                }
            }
        }
        private void LogoutButton_Click(object sender, EventArgs e) {
            this.user = new User();
            authorizationSql.updateUser(user);
            if (catalogForm != null)
                catalogForm.User = user;
            MessageBox.Show("Вы вышли из аккаунта.", "Выход", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateTabPages();
            UpdateAccountPageInfo();
            authLoginTextBox.Clear();
            authPasswordTextBox.Clear();
            registerLoginTextBox.Clear();
            registerPasswordTextBox.Clear();
            registerConfirmPasswordTextBox.Clear();
            registerIsWriterCheckBox.Checked = false;
        }
        private void AuthorizationForm_FormClosed(object sender, FormClosedEventArgs e) {
            catalogForm.Show();
            authorizationSql.Dispose();
        }
    }
}