namespace bookservice
{
    partial class CatalogForm
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
            this.components = new System.ComponentModel.Container();
            this.clearSearchButton = new System.Windows.Forms.Label();
            this.genresFilterButton = new System.Windows.Forms.Label();
            this.genresContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetGenresFilterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ageRatingFilterButton = new System.Windows.Forms.Label();
            this.ageRatingContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resetAgeRatingFilterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.age0PlusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.age13PlusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.age16PlusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.age18PlusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yearSortButton = new System.Windows.Forms.Label();
            this.novelsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.searchLabel = new System.Windows.Forms.Label();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.suggestionsListBox = new System.Windows.Forms.ListBox();
            this.authButton = new System.Windows.Forms.Label();
            this.createBookButton = new System.Windows.Forms.Label();
            this.historyButton = new System.Windows.Forms.Label();
            this.genresContextMenuStrip.SuspendLayout();
            this.ageRatingContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // clearSearchButton
            // 
            this.clearSearchButton.AutoSize = true;
            this.clearSearchButton.BackColor = System.Drawing.Color.Transparent;
            this.clearSearchButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clearSearchButton.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clearSearchButton.ForeColor = System.Drawing.Color.Gray;
            this.clearSearchButton.Location = new System.Drawing.Point(542, 46);
            this.clearSearchButton.Name = "clearSearchButton";
            this.clearSearchButton.Size = new System.Drawing.Size(38, 38);
            this.clearSearchButton.TabIndex = 1;
            this.clearSearchButton.Text = "✕";
            this.clearSearchButton.Visible = false;
            this.clearSearchButton.Click += new System.EventHandler(this.ClearSearchButton_Click);
            // 
            // genresFilterButton
            // 
            this.genresFilterButton.AutoSize = true;
            this.genresFilterButton.BackColor = System.Drawing.SystemColors.Control;
            this.genresFilterButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.genresFilterButton.ContextMenuStrip = this.genresContextMenuStrip;
            this.genresFilterButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.genresFilterButton.Location = new System.Drawing.Point(90, 108);
            this.genresFilterButton.Name = "genresFilterButton";
            this.genresFilterButton.Size = new System.Drawing.Size(155, 34);
            this.genresFilterButton.TabIndex = 2;
            this.genresFilterButton.Text = "📚 Жанры";
            this.genresFilterButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.genresFilterButton.Click += new System.EventHandler(this.GenresFilterButton_Click);
            // 
            // genresContextMenuStrip
            // 
            this.genresContextMenuStrip.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.genresContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.genresContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetGenresFilterMenuItem,
            this.toolStripSeparator1});
            this.genresContextMenuStrip.Name = "genresContextMenuStrip";
            this.genresContextMenuStrip.Size = new System.Drawing.Size(329, 48);
            // 
            // resetGenresFilterMenuItem
            // 
            this.resetGenresFilterMenuItem.Name = "resetGenresFilterMenuItem";
            this.resetGenresFilterMenuItem.Size = new System.Drawing.Size(328, 38);
            this.resetGenresFilterMenuItem.Text = "Сбросить фильтр";
            this.resetGenresFilterMenuItem.Click += new System.EventHandler(this.ResetGenresFilterMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(325, 6);
            // 
            // ageRatingFilterButton
            // 
            this.ageRatingFilterButton.AutoSize = true;
            this.ageRatingFilterButton.BackColor = System.Drawing.SystemColors.Control;
            this.ageRatingFilterButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ageRatingFilterButton.ContextMenuStrip = this.ageRatingContextMenuStrip;
            this.ageRatingFilterButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ageRatingFilterButton.Location = new System.Drawing.Point(263, 108);
            this.ageRatingFilterButton.Name = "ageRatingFilterButton";
            this.ageRatingFilterButton.Size = new System.Drawing.Size(409, 34);
            this.ageRatingFilterButton.TabIndex = 3;
            this.ageRatingFilterButton.Text = "🚫 Возрастные ограничения";
            this.ageRatingFilterButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ageRatingFilterButton.Click += new System.EventHandler(this.AgeRatingFilterButton_Click);
            // 
            // ageRatingContextMenuStrip
            // 
            this.ageRatingContextMenuStrip.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ageRatingContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ageRatingContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetAgeRatingFilterMenuItem,
            this.toolStripSeparator2,
            this.age0PlusMenuItem,
            this.age13PlusMenuItem,
            this.age16PlusMenuItem,
            this.age18PlusMenuItem});
            this.ageRatingContextMenuStrip.Name = "genresContextMenuStrip";
            this.ageRatingContextMenuStrip.Size = new System.Drawing.Size(329, 200);
            // 
            // resetAgeRatingFilterMenuItem
            // 
            this.resetAgeRatingFilterMenuItem.Name = "resetAgeRatingFilterMenuItem";
            this.resetAgeRatingFilterMenuItem.Size = new System.Drawing.Size(328, 38);
            this.resetAgeRatingFilterMenuItem.Text = "Сбросить фильтр";
            this.resetAgeRatingFilterMenuItem.Click += new System.EventHandler(this.ResetAgeRatingFilterMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(325, 6);
            // 
            // age0PlusMenuItem
            // 
            this.age0PlusMenuItem.CheckOnClick = true;
            this.age0PlusMenuItem.Name = "age0PlusMenuItem";
            this.age0PlusMenuItem.Size = new System.Drawing.Size(328, 38);
            this.age0PlusMenuItem.Tag = "0";
            this.age0PlusMenuItem.Text = "0+";
            this.age0PlusMenuItem.CheckedChanged += new System.EventHandler(this.AgeRatingMenuItem_CheckedChanged);
            // 
            // age13PlusMenuItem
            // 
            this.age13PlusMenuItem.CheckOnClick = true;
            this.age13PlusMenuItem.Name = "age13PlusMenuItem";
            this.age13PlusMenuItem.Size = new System.Drawing.Size(328, 38);
            this.age13PlusMenuItem.Tag = "13";
            this.age13PlusMenuItem.Text = "13+";
            this.age13PlusMenuItem.CheckedChanged += new System.EventHandler(this.AgeRatingMenuItem_CheckedChanged);
            // 
            // age16PlusMenuItem
            // 
            this.age16PlusMenuItem.CheckOnClick = true;
            this.age16PlusMenuItem.Name = "age16PlusMenuItem";
            this.age16PlusMenuItem.Size = new System.Drawing.Size(328, 38);
            this.age16PlusMenuItem.Tag = "16";
            this.age16PlusMenuItem.Text = "16+";
            this.age16PlusMenuItem.CheckedChanged += new System.EventHandler(this.AgeRatingMenuItem_CheckedChanged);
            // 
            // age18PlusMenuItem
            // 
            this.age18PlusMenuItem.CheckOnClick = true;
            this.age18PlusMenuItem.Name = "age18PlusMenuItem";
            this.age18PlusMenuItem.Size = new System.Drawing.Size(328, 38);
            this.age18PlusMenuItem.Tag = "18";
            this.age18PlusMenuItem.Text = "18+";
            this.age18PlusMenuItem.CheckedChanged += new System.EventHandler(this.AgeRatingMenuItem_CheckedChanged);
            // 
            // yearSortButton
            // 
            this.yearSortButton.AutoSize = true;
            this.yearSortButton.BackColor = System.Drawing.SystemColors.Control;
            this.yearSortButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.yearSortButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.yearSortButton.Location = new System.Drawing.Point(694, 108);
            this.yearSortButton.Name = "yearSortButton";
            this.yearSortButton.Size = new System.Drawing.Size(232, 34);
            this.yearSortButton.TabIndex = 4;
            this.yearSortButton.Text = "📅 Год выпуска";
            this.yearSortButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.yearSortButton.Click += new System.EventHandler(this.YearSortButton_Click);
            // 
            // novelsFlowPanel
            // 
            this.novelsFlowPanel.AutoScroll = true;
            this.novelsFlowPanel.Location = new System.Drawing.Point(90, 371);
            this.novelsFlowPanel.Name = "novelsFlowPanel";
            this.novelsFlowPanel.Size = new System.Drawing.Size(1059, 579);
            this.novelsFlowPanel.TabIndex = 7;
            // 
            // searchLabel
            // 
            this.searchLabel.BackColor = System.Drawing.SystemColors.Control;
            this.searchLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchLabel.ContextMenuStrip = this.genresContextMenuStrip;
            this.searchLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.searchLabel.Location = new System.Drawing.Point(90, 35);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(500, 34);
            this.searchLabel.TabIndex = 8;
            this.searchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // searchTextBox
            // 
            this.searchTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.searchTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.searchTextBox.Font = new System.Drawing.Font("Verdana", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.searchTextBox.ForeColor = System.Drawing.Color.Gray;
            this.searchTextBox.Location = new System.Drawing.Point(90, 35);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(500, 53);
            this.searchTextBox.TabIndex = 9;
            this.searchTextBox.Text = "Поиск по названию";
            this.searchTextBox.TextChanged += new System.EventHandler(this.SearchTextBox_TextChanged);
            this.searchTextBox.Enter += new System.EventHandler(this.SearchTextBox_GotFocus);
            this.searchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchTextBox_KeyDown);
            this.searchTextBox.Leave += new System.EventHandler(this.SearchTextBox_LostFocus);
            // 
            // suggestionsListBox
            // 
            this.suggestionsListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.suggestionsListBox.Font = new System.Drawing.Font("Verdana", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.suggestionsListBox.FormattingEnabled = true;
            this.suggestionsListBox.IntegralHeight = false;
            this.suggestionsListBox.ItemHeight = 45;
            this.suggestionsListBox.Location = new System.Drawing.Point(90, 163);
            this.suggestionsListBox.Name = "suggestionsListBox";
            this.suggestionsListBox.Size = new System.Drawing.Size(500, 182);
            this.suggestionsListBox.TabIndex = 10;
            this.suggestionsListBox.Visible = false;
            this.suggestionsListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SuggestionsListBox_MouseClick);
            this.suggestionsListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SuggestionsListBox_KeyDown);
            // 
            // authButton
            // 
            this.authButton.AutoSize = true;
            this.authButton.BackColor = System.Drawing.SystemColors.Control;
            this.authButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.authButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.authButton.Location = new System.Drawing.Point(969, 51);
            this.authButton.Name = "authButton";
            this.authButton.Size = new System.Drawing.Size(170, 34);
            this.authButton.TabIndex = 11;
            this.authButton.Text = "👤 Аккаунт";
            this.authButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.authButton.Click += new System.EventHandler(this.AuthButton_Click);
            // 
            // createBookButton
            // 
            this.createBookButton.AutoSize = true;
            this.createBookButton.BackColor = System.Drawing.SystemColors.Control;
            this.createBookButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.createBookButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.createBookButton.Location = new System.Drawing.Point(629, 51);
            this.createBookButton.Name = "createBookButton";
            this.createBookButton.Size = new System.Drawing.Size(257, 34);
            this.createBookButton.TabIndex = 12;
            this.createBookButton.Text = "📖 Создать книгу";
            this.createBookButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.createBookButton.Visible = false;
            this.createBookButton.Click += new System.EventHandler(this.CreateBookButton_Click);
            // 
            // historyButton
            // 
            this.historyButton.AutoSize = true;
            this.historyButton.BackColor = System.Drawing.SystemColors.Control;
            this.historyButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.historyButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.historyButton.Location = new System.Drawing.Point(969, 108);
            this.historyButton.Name = "historyButton";
            this.historyButton.Size = new System.Drawing.Size(171, 34);
            this.historyButton.TabIndex = 13;
            this.historyButton.Text = "⌛ История";
            this.historyButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CatalogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1454, 729);
            this.Controls.Add(this.historyButton);
            this.Controls.Add(this.createBookButton);
            this.Controls.Add(this.authButton);
            this.Controls.Add(this.suggestionsListBox);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.novelsFlowPanel);
            this.Controls.Add(this.yearSortButton);
            this.Controls.Add(this.ageRatingFilterButton);
            this.Controls.Add(this.genresFilterButton);
            this.Controls.Add(this.clearSearchButton);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "CatalogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Каталог веб-романов";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.CatalogForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CatalogForm_FormClosing);
            this.Load += new System.EventHandler(this.CatalogForm_Load);
            this.Click += new System.EventHandler(this.CatalogForm_Click);
            this.genresContextMenuStrip.ResumeLayout(false);
            this.ageRatingContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label clearSearchButton;
        private System.Windows.Forms.Label genresFilterButton;
        private System.Windows.Forms.Label ageRatingFilterButton;
        private System.Windows.Forms.Label yearSortButton;
        private System.Windows.Forms.ContextMenuStrip genresContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem resetGenresFilterMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ContextMenuStrip ageRatingContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem resetAgeRatingFilterMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem age0PlusMenuItem;
        private System.Windows.Forms.ToolStripMenuItem age13PlusMenuItem;
        private System.Windows.Forms.ToolStripMenuItem age16PlusMenuItem;
        private System.Windows.Forms.ToolStripMenuItem age18PlusMenuItem;
        private System.Windows.Forms.FlowLayoutPanel novelsFlowPanel;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.ListBox suggestionsListBox;
        private System.Windows.Forms.Label authButton;
        private System.Windows.Forms.Label createBookButton;
        private System.Windows.Forms.Label historyButton;
    }
}