using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace bookservice {
    public partial class CatalogForm : Form {
        private CatalogSQL catalogSql;
        private User user;
        private List<string> allBookTitles;
        private List<string> allGenres;

        private const int CoverWidth = 288;
        private const int CoverHeight = 406;
        private const int LabelHeight = 45; // Increased height for potentially longer titles
        private const int HorizontalMargin = 30;
        private const int VerticalMargin = 30;
        private const int StartXBase = 90;

        private const string SearchPlaceholderText = "Поиск по названию книги...";

        private enum SortOrderState { None, Ascending, Descending }
        private SortOrderState currentSortOrder = SortOrderState.None;

        private List<string> selectedGenresFilter = new List<string>();
        private List<int> selectedAgeRatingsFilter = new List<int>();

        public CatalogForm() {
            InitializeComponent();
            // Ensure InitializeCustomComponents is called if it's not by designer.
            // It's often called in Load event.
        }

        private void CatalogForm_Load(object sender, EventArgs e) {
            novelsFlowPanel.Focus();
            user = new User(); // Start as guest
            catalogSql = new CatalogSQL(user);
            InitializeCustomComponents(); // Sets up clearSearchButton
            LoadInitialData();
            UpdateFilterControlsLayout(); // Initial layout
            DisplayBooks();
            AdminCheck(); // For createBookButton visibility
            UpdateAuthButtonText(); // Set initial text for authButton
            this.Resize += CatalogForm_Resize; // Add resize event handler for dynamic layout
        }
        private void CatalogForm_Resize(object sender, EventArgs e)
        {
            UpdateFilterControlsLayout(); // Re-apply layout on resize
        }

        private void InitializeCustomComponents() {
            searchLabel.Controls.Add(clearSearchButton);
            clearSearchButton.Location = new Point(searchLabel.ClientSize.Width - clearSearchButton.Width - 3, (searchLabel.ClientSize.Height - clearSearchButton.Height) / 2);
            clearSearchButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }

        private void LoadInitialData() {
            allBookTitles = catalogSql.GetBookTitles(); // Renamed from GetWebnovelTitles
            allGenres = catalogSql.GetGenres();

            foreach (string genre in allGenres) {
                ToolStripMenuItem genreItem = new ToolStripMenuItem(genre);
                genreItem.CheckOnClick = true;
                genreItem.CheckedChanged += GenreMenuItem_CheckedChanged;
                genresContextMenuStrip.Items.Add(genreItem);
            }
        }

        private void UpdateFilterControlsLayout() {
            int currentX = 90;
            const int controlSpacing = 10;

            searchLabel.Location = new Point(currentX, 35);
            searchLabel.Size = new Size(800, 35);
            searchTextBox.Location = new Point(currentX + 10, 39);
            searchTextBox.Size = new Size(470, 35);
            //currentX += searchLabel.Width + controlSpacing;

            genresFilterButton.Location = new Point(currentX, 80);
            genresFilterButton.AutoSize = true;
            genresFilterButton.Height = 35;
            genresFilterButton.Padding = new Padding(10, 0, 10, 0);
            genresFilterButton.MinimumSize = new Size(80, 35);
            currentX += genresFilterButton.Width + controlSpacing;

            ageRatingFilterButton.Location = new Point(currentX, 80);
            ageRatingFilterButton.AutoSize = true;
            ageRatingFilterButton.Height = 35;
            ageRatingFilterButton.Padding = new Padding(10, 0, 10, 0);
            ageRatingFilterButton.MinimumSize = new Size(80, 35);
            currentX += ageRatingFilterButton.Width + controlSpacing;

            yearSortButton.Location = new Point(currentX, 80);
            yearSortButton.AutoSize = true;
            yearSortButton.Height = 35;
            yearSortButton.Padding = new Padding(10, 0, 10, 0);
            yearSortButton.MinimumSize = new Size(120, 35);

            suggestionsListBox.Top = searchLabel.Bottom;
            suggestionsListBox.Left = searchLabel.Left;
            suggestionsListBox.Width = searchLabel.Width;

            novelsFlowPanel.Left = StartXBase; // Assuming novelsFlowPanel is for books
            novelsFlowPanel.Top = genresFilterButton.Bottom + 30;
            novelsFlowPanel.Size = new Size(this.ClientSize.Width - StartXBase, this.ClientSize.Height - 145);

            authButton.AutoSize = true;
            authButton.Height = 35;
            authButton.Padding = new Padding(10, 0, 10, 0);
            authButton.MinimumSize = new Size(100, 35);
            authButton.Location = new Point(StartXBase + CoverWidth * 4 + HorizontalMargin * 3 - authButton.Width, 35);
            
            createBookButton.AutoSize = true; 
            createBookButton.Height = 35;
            createBookButton.Padding = new Padding(10, 0, 10, 0);
            createBookButton.MinimumSize = new Size(100, 35);
            createBookButton.Location = new Point(authButton.Left - createBookButton.Width - 10, 35);

            historyButton.AutoSize = true;
            historyButton.Height = 35;
            historyButton.Padding = new Padding(10, 0, 7, 0);
            historyButton.MinimumSize = new Size(100, 35);
            historyButton.Location = new Point(authButton.Right - historyButton.Width, authButton.Bottom + 10);
        }
        private void CatalogForm_Click(object sender, EventArgs e) {
            if(suggestionsListBox.Visible) suggestionsListBox.Visible = false;
            novelsFlowPanel.Focus();
        }
        private void DisplayBooks() {
            novelsFlowPanel.Controls.Clear();
            catalogSql.SetSearchTerm(searchTextBox.Text == SearchPlaceholderText ? "" : searchTextBox.Text);
            catalogSql.SetSelectedGenres(selectedGenresFilter);
            catalogSql.SetSelectedAgeRatings(selectedAgeRatingsFilter);

            string sortDirection = "";
            if (currentSortOrder == SortOrderState.Ascending)
                sortDirection = "ASC";
            else if (currentSortOrder == SortOrderState.Descending)
                sortDirection = "DESC";
            catalogSql.SetSortByYearDirection(sortDirection);

            DataTable booksData = catalogSql.GetBooks();

            if (booksData == null || booksData.Rows.Count == 0) {
                Label noResultsLabel = new Label {
                    Text = "По вашему запросу ничего не найдено.",
                    Font = new Font("Verdana", 16, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill // Fill the panel if no results
                };
                novelsFlowPanel.Controls.Add(noResultsLabel);
                return;
            }

            foreach (DataRow row in booksData.Rows) {
                string bookId = row["id"].ToString();
                string coverPath = row["cover_path"]?.ToString();
                string title = row["title"]?.ToString();

                Panel bookPanel = new Panel {
                    Width = CoverWidth,
                    Height = CoverHeight + LabelHeight,
                    Margin = new Padding(0, 0, HorizontalMargin, VerticalMargin),
                    Cursor = Cursors.Hand
                };

                PictureBox pictureBox = new PictureBox {
                    Width = CoverWidth,
                    Height = CoverHeight,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BorderStyle = BorderStyle.None, // Cleaner look
                    Tag = bookId,
                    Dock = DockStyle.Top // Fill top part of bookPanel
                };
                pictureBox.Click += PictureBox_Click; // Event for clicking the cover

                try {
                    if (!string.IsNullOrEmpty(coverPath) && File.Exists(GetAbsolutePath(coverPath)))
                        using (FileStream stream = new FileStream(GetAbsolutePath(coverPath), FileMode.Open, FileAccess.Read)) {
                            pictureBox.Image = Image.FromStream(stream);
                        }
                    else
                        pictureBox.Image = CreatePlaceholderImage(CoverWidth, CoverHeight);
                } catch (Exception ex) {
                    Console.WriteLine($"Ошибка загрузки обложки {coverPath}: {ex.Message}");
                    pictureBox.Image = CreatePlaceholderImage(CoverWidth, CoverHeight);
                }

                Label titleLabel = new Label {
                    Text = TruncateText(title, new Font("Verdana", 11F, FontStyle.Bold), CoverWidth - 10), // Adjusted font
                    Font = new Font("Verdana", 11F, FontStyle.Bold), // Adjusted font
                    ForeColor = Color.Black,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Bottom, // Fill bottom part of bookPanel
                    Height = LabelHeight,
                    Padding = new Padding(5) // Padding for text
                };
                titleLabel.Click += (s, e) => PictureBox_Click(pictureBox, e); // Make label clickable too

                bookPanel.Controls.Add(pictureBox);
                bookPanel.Controls.Add(titleLabel);
                novelsFlowPanel.Controls.Add(bookPanel);
            }
            novelsFlowPanel.Focus();
        }
        private string GetAbsolutePath(string relativePath) {
            if (string.IsNullOrEmpty(relativePath) || Path.IsPathRooted(relativePath))
                return relativePath;
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(Path.Combine(basePath, relativePath));
        }

        private Bitmap CreatePlaceholderImage(int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(60, 60, 60));
                TextRenderer.DrawText(g, "Нет обложки",
                    new Font("Verdana", 16, FontStyle.Bold),
                    new Rectangle(0, 0, width, height),
                    Color.FromArgb(180, 180, 180),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
            return bmp;
        }



        private string TruncateText(string text, Font font, int maxWidth) {
            if (string.IsNullOrEmpty(text)) return "";
            if (TextRenderer.MeasureText(text, font).Width <= maxWidth) return text;
            string ellipsis = "...";
            int ellipsisWidth = TextRenderer.MeasureText(ellipsis, font).Width;
            string temp = "";
            for (int i = text.Length -1; i >= 0; i--) {
                temp = text.Substring(0,i);
                if(TextRenderer.MeasureText(temp,font).Width <= maxWidth - ellipsisWidth) {
                    return temp + ellipsis;
                }
            }
            return ellipsis; // Should not happen if maxWidth > ellipsisWidth
        }

        private void SearchTextBox_GotFocus(object sender, EventArgs e) {
            if (searchTextBox.Text == SearchPlaceholderText) {
                searchTextBox.Text = "";
                searchTextBox.ForeColor = Color.Black;
            }
        }

        private void SearchTextBox_LostFocus(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text)) {
                searchTextBox.Text = SearchPlaceholderText;
                searchTextBox.ForeColor = Color.Gray;
                clearSearchButton.Visible = false;
            }
             // Hide suggestions if neither search box nor listbox has focus
            if (!suggestionsListBox.Focused && !searchTextBox.Focused)
            {
                suggestionsListBox.Visible = false;
            }
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e) {
            bool hasText = !string.IsNullOrWhiteSpace(searchTextBox.Text) && searchTextBox.Text != SearchPlaceholderText;
            clearSearchButton.Visible = hasText;

            if (searchTextBox.Focused && hasText && searchTextBox.Text.Length > 0) { // Min 1 char for suggestions
                string searchText = searchTextBox.Text.ToLower();
                var filteredTitles = allBookTitles?
                    .Where(title => title.ToLower().Contains(searchText))
                    .Take(7) // More suggestions
                    .ToList() ?? new List<string>();

                if (filteredTitles.Any()) {
                    suggestionsListBox.DataSource = filteredTitles;
                    suggestionsListBox.Visible = true;
                    suggestionsListBox.Height = Math.Min(filteredTitles.Count * 23, 150); // Max height for suggestions
                } else {
                    suggestionsListBox.Visible = false;
                }
            } else {
                suggestionsListBox.Visible = false;
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                e.SuppressKeyPress = true; // Prevent beep
                suggestionsListBox.Visible = false;
                DisplayBooks();
                novelsFlowPanel.Focus(); // Move focus away from searchbox
            } else if (e.KeyCode == Keys.Down && suggestionsListBox.Visible && suggestionsListBox.Items.Count > 0) {
                e.SuppressKeyPress = true;
                suggestionsListBox.Focus();
                suggestionsListBox.SelectedIndex = 0;
            } else if (e.KeyCode == Keys.Escape) {
                suggestionsListBox.Visible = false;
                searchTextBox.SelectAll(); // Keep focus, allow easy re-typing
            }
        }

        private void ClearSearchButton_Click(object sender, EventArgs e) {
            searchTextBox.Text = SearchPlaceholderText;
            searchTextBox.ForeColor = Color.Gray;
            clearSearchButton.Visible = false;
            suggestionsListBox.Visible = false;
            DisplayBooks();
            novelsFlowPanel.Focus();
        }

        private void SuggestionsListBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab) {
                e.SuppressKeyPress = true;
                if (suggestionsListBox.SelectedItem != null) {
                    searchTextBox.Text = suggestionsListBox.SelectedItem.ToString();
                    searchTextBox.Focus(); // Return focus to search box
                    searchTextBox.Select(searchTextBox.Text.Length, 0); // Cursor at end
                    suggestionsListBox.Visible = false;
                    if (e.KeyCode == Keys.Enter) DisplayBooks(); // Search on Enter
                }
            } else if (e.KeyCode == Keys.Escape) {
                suggestionsListBox.Visible = false;
                searchTextBox.Focus();
            }
        }

        private void SuggestionsListBox_MouseClick(object sender, MouseEventArgs e) {
            if (suggestionsListBox.SelectedItem != null) {
                searchTextBox.Text = suggestionsListBox.SelectedItem.ToString();
                searchTextBox.Focus();
                searchTextBox.Select(searchTextBox.Text.Length, 0);
                suggestionsListBox.Visible = false;
                DisplayBooks();
                novelsFlowPanel.Focus();
            }
        }
        private void AuthButton_Click(object sender, EventArgs e) {
            if (Application.OpenForms.OfType<AuthorizationForm>().Count() == 0) {
                AuthorizationForm authorizationForm = new AuthorizationForm();
                authorizationForm.Owner = this;
                // this.Hide(); // Don't hide, auth form is modal-like
                authorizationForm.ShowDialog(); // Show as dialog to block interaction with catalog
                // After auth form closes, User property might have changed.
                // The Activated event handles refreshing.
            }
        }
        private void UpdateAuthButtonText()
        {
            if (user != null && user.Id != 0)
            {
                authButton.Text = $"👤 {user.Login}";
            }
            else
            {
                authButton.Text = "🔑 Войти";
            }
            UpdateFilterControlsLayout(); // Auth button width might change
        }

        private void GenresFilterButton_Click(object sender, EventArgs e) {
            genresContextMenuStrip.Show(genresFilterButton, new Point(0, genresFilterButton.Height));
        }

        private void GenreMenuItem_CheckedChanged(object sender, EventArgs e) {
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            if (clickedItem == null || clickedItem == resetGenresFilterMenuItem) return;

            string genreName = clickedItem.Text;
            if (clickedItem.Checked) {
                if (!selectedGenresFilter.Contains(genreName)) selectedGenresFilter.Add(genreName);
            } else {
                selectedGenresFilter.Remove(genreName);
            }
            UpdateGenresFilterButtonText();
            DisplayBooks();
        }

        private void ResetGenresFilterMenuItem_Click(object sender, EventArgs e) {
            selectedGenresFilter.Clear();
            foreach (ToolStripItem item in genresContextMenuStrip.Items) {
                if (item is ToolStripMenuItem genreItem && genreItem != resetGenresFilterMenuItem) {
                    genreItem.Checked = false;
                }
            }
            UpdateGenresFilterButtonText();
            DisplayBooks();
        }

        private void UpdateGenresFilterButtonText() {
            if (selectedGenresFilter.Any()) {
                genresFilterButton.Text = "📚 Жанры: " + string.Join(", ", selectedGenresFilter.Take(2)) + (selectedGenresFilter.Count > 2 ? "..." : "");
            } else {
                genresFilterButton.Text = "📚 Все жанры";
            }
            UpdateFilterControlsLayout(); // Button width might change
        }

        private void AgeRatingFilterButton_Click(object sender, EventArgs e) {
            ageRatingContextMenuStrip.Show(ageRatingFilterButton, new Point(0, ageRatingFilterButton.Height));
        }

        private void AgeRatingMenuItem_CheckedChanged(object sender, EventArgs e) {
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            if (clickedItem == null || clickedItem.Tag == null || clickedItem == resetAgeRatingFilterMenuItem) return;

            int ageRating = Convert.ToInt32(clickedItem.Tag);
            if (clickedItem.Checked) {
                if (!selectedAgeRatingsFilter.Contains(ageRating)) selectedAgeRatingsFilter.Add(ageRating);
            } else {
                selectedAgeRatingsFilter.Remove(ageRating);
            }
            UpdateAgeRatingFilterButtonText();
            DisplayBooks();
        }

        private void ResetAgeRatingFilterMenuItem_Click(object sender, EventArgs e) {
            selectedAgeRatingsFilter.Clear();
            foreach (ToolStripItem item in ageRatingContextMenuStrip.Items) {
                if (item is ToolStripMenuItem ageItem && ageItem.Tag != null && ageItem != resetAgeRatingFilterMenuItem) {
                    ageItem.Checked = false;
                }
            }
            UpdateAgeRatingFilterButtonText();
            DisplayBooks();
        }

        private void UpdateAgeRatingFilterButtonText() {
            if (selectedAgeRatingsFilter.Any()) {
                selectedAgeRatingsFilter.Sort();
                ageRatingFilterButton.Text = "🚫 Рейтинг: " + string.Join(", ", selectedAgeRatingsFilter.Select(r => r + "+").Take(2)) + (selectedAgeRatingsFilter.Count > 2 ? "..." : "");
            } else {
                ageRatingFilterButton.Text = "🚫 Все рейтинги";
            }
            UpdateFilterControlsLayout(); // Button width might change
        }

        private void YearSortButton_Click(object sender, EventArgs e) {
            switch (currentSortOrder) {
                case SortOrderState.None:
                    currentSortOrder = SortOrderState.Ascending;
                    yearSortButton.Text = "📅 Год ▲";
                    break;
                case SortOrderState.Ascending:
                    currentSortOrder = SortOrderState.Descending;
                    yearSortButton.Text = "📅 Год ▼";
                    break;
                case SortOrderState.Descending:
                    currentSortOrder = SortOrderState.None;
                    yearSortButton.Text = "📅 Год выпуска";
                    break;
            }
            DisplayBooks();
        }

        private void PictureBox_Click(object sender, EventArgs e) {
            PictureBox clickedPictureBox = sender as PictureBox;
             if (clickedPictureBox == null && sender is Label) // If label was clicked
            {
                clickedPictureBox = (sender as Label).Parent.Controls.OfType<PictureBox>().FirstOrDefault();
            }

            if (Application.OpenForms.OfType<BookForm>().Count() == 0 && clickedPictureBox != null && clickedPictureBox.Tag != null) {
                selectedBookID = clickedPictureBox.Tag.ToString();
                BookForm bookForm = new BookForm();
                bookForm.Owner = this;
                this.Hide();
                bookForm.Show();
            }
        }
        private string selectedBookID;
        public string GetSelectedBookID {
            get { return selectedBookID; }
        }
        public User User {
            get { return user; }
            set {
                user = value;
                catalogSql.updateUser(user); // Update SQL connection based on new user role
                AdminCheck();
                UpdateAuthButtonText(); // Update button text after login/logout
                DisplayBooks(); // Refresh books as permissions might change what's queryable
            }
        }
        private void AdminCheck() {
            createBookButton.Visible = user.IsAdmin; // Only admins can create
            historyButton.Visible = user.Id != 0; // Only logged-in users see history
            UpdateFilterControlsLayout(); // Visibility change affects layout
        }
        private void CreateBookButton_Click(object sender, EventArgs e) {
            if (Application.OpenForms.OfType<BookForm>().Count() == 0) {
                selectedBookID = "0"; // Indicates new book creation mode for BookForm
                BookForm bookForm = new BookForm();
                bookForm.Owner = this;
                this.Hide();
                bookForm.Show();
            }
        }

        private void historyButton_Click(object sender, EventArgs e)
        {
            if (user == null || user.Id == 0)
            {
                MessageBox.Show("Пожалуйста, войдите в систему, чтобы просмотреть историю чтения.", "Требуется авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Application.OpenForms.OfType<HistoryForm>().Count() == 0)
            {
                HistoryForm historyForm = new HistoryForm(user);
                historyForm.Owner = this;
                // this.Hide(); // Don't hide catalog, history is modal
                historyForm.ShowDialog(); // Show as dialog
            }
            else
            {
                Application.OpenForms.OfType<HistoryForm>().First().Activate(); // Bring to front if already open
            }
        }


        private void CatalogForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (catalogSql != null) {
                catalogSql.Dispose();
            }
            // Dispose images in PictureBoxes to free resources
            foreach (Control panelControl in novelsFlowPanel.Controls) {
                if (panelControl is Panel bookPanel) {
                    foreach (Control itemControl in bookPanel.Controls) {
                        if (itemControl is PictureBox pb && pb.Image != null) {
                            pb.Image.Dispose();
                            pb.Image = null;
                        }
                    }
                }
                panelControl.Dispose();
            }
            novelsFlowPanel.Controls.Clear();
        }
        private void CatalogForm_Activated(object sender, EventArgs e) {
            // This event fires when the form becomes active, e.g., after BookForm or AuthForm closes.
            // Refresh data that might have changed.
            if (catalogSql != null) // Ensure catalogSql is initialized
            {
                allBookTitles = catalogSql.GetBookTitles(); // Refresh titles for search suggestions
                DisplayBooks(); // Refresh the list of books
                AdminCheck(); // Re-check admin status for create button and history
                UpdateAuthButtonText(); // Update login status on auth button
            }
        }
    }
}
