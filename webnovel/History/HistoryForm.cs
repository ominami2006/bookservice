using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace bookservice
{
    public partial class HistoryForm : Form
    {
        private User currentUser;
        private HistorySQL historySql;
        private DataGridView dgvHistory;
        private Button closeButton;

        public HistoryForm(User user)
        {
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 411);
            this.Name = "HistoryForm";
            this.Text = "История чтения";
            this.currentUser = user;
            this.historySql = new HistorySQL(this.currentUser);
            SetupUI();
            LoadHistory();
        }

        private void SetupUI()
        {
            this.Text = "История чтения";
            this.ClientSize = new System.Drawing.Size(700, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Font = new Font("Verdana", 10F);
            this.BackColor = SystemColors.ControlLightLight;

            dgvHistory = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.ControlLightLight,
                BorderStyle = BorderStyle.None,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Verdana", 10F, FontStyle.Bold), BackColor = SystemColors.ControlLight, ForeColor = SystemColors.WindowText },
                DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Verdana", 9F) },
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            // Define columns
            dgvHistory.Columns.Add("BookTitle", "Название книги");
            dgvHistory.Columns.Add("StatusDisplay", "Статус");
            dgvHistory.Columns.Add("LastAccessedDate", "Дата последнего доступа");

            dgvHistory.Columns["BookTitle"].FillWeight = 40; // Percentage or relative size
            dgvHistory.Columns["StatusDisplay"].FillWeight = 30;
            dgvHistory.Columns["LastAccessedDate"].FillWeight = 30;
            dgvHistory.Columns["LastAccessedDate"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";


            closeButton = new Button
            {
                Text = "Закрыть",
                DialogResult = DialogResult.Cancel, // So it can be closed by pressing Esc too
                Size = new Size(100, 35),
                Font = new Font("Verdana", 10F)
            };
            closeButton.Click += (s, e) => this.Close();

            TableLayoutPanel layoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(10)
            };
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // DataGridView
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Button panel

            Panel buttonPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 0, 0) };
            closeButton.Dock = DockStyle.Right; // Anchor to the right of the button panel
            buttonPanel.Controls.Add(closeButton);

            layoutPanel.Controls.Add(dgvHistory, 0, 0);
            layoutPanel.Controls.Add(buttonPanel, 0, 1);

            this.Controls.Add(layoutPanel);
            this.CancelButton = closeButton; // Allows Esc to close the form
        }

        private void LoadHistory()
        {
            if (currentUser == null || currentUser.Id == 0)
            {
                MessageBox.Show("Информация о пользователе отсутствует. Невозможно загрузить историю.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<UserBookHistoryEntry> historyEntries = historySql.GetUserReadingHistory(currentUser.Id);
            dgvHistory.Rows.Clear();

            if (historyEntries.Count == 0)
            {
                // Optionally, display a message in the DGV or a Label
                // For now, it will just be an empty grid.
                return;
            }

            foreach (var entry in historyEntries)
            {
                dgvHistory.Rows.Add(entry.BookTitle, entry.StatusDisplay, entry.LastAccessedDate);
            }
        }
    }
}