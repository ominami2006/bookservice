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
        private List<string> allNovelTitles;
        private List<string> allGenres;

        private const int CoverWidth = 288;
        private const int CoverHeight = 406;
        private const int LabelHeight = 45;
        private const int HorizontalMargin = 30;
        private const int VerticalMargin = 30;
        private const int StartXBase = 90;

        private const string SearchPlaceholderText = "Поиск по названию";

        private enum SortOrderState { None, Ascending, Descending }
        private SortOrderState currentSortOrder = SortOrderState.None;

        private List<string> selectedGenresFilter = new List<string>();
        private List<int> selectedAgeRatingsFilter = new List<int>();

        public CatalogForm() {
            InitializeComponent();
        }

        private void CatalogForm_Load(object sender, EventArgs e) {
            user = new User();
            catalogSql = new CatalogSQL(user);
            InitializeCustomComponents();
            LoadInitialData();
            UpdateFilterControlsLayout();
            DisplayWebnovels();
        }

        private void InitializeCustomComponents() {
            searchLabel.Controls.Add(clearSearchButton);
            clearSearchButton.Location = new Point(searchLabel.ClientSize.Width - clearSearchButton.Width - 3, (searchLabel.ClientSize.Height - clearSearchButton.Height) / 2);
            clearSearchButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }

        private void LoadInitialData() {
            allNovelTitles = catalogSql.GetWebnovelTitles();
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

            novelsFlowPanel.Left = StartXBase;
            novelsFlowPanel.Top = genresFilterButton.Bottom + 30;
            novelsFlowPanel.Size = new Size(this.ClientSize.Width - StartXBase, this.ClientSize.Height - 145);

            authButton.AutoSize = true;
            authButton.Height = 35;
            authButton.Padding = new Padding(10, 0, 10, 0);
            authButton.MinimumSize = new Size(100, 35);
            authButton.Location = new Point(StartXBase + CoverWidth * 4 + HorizontalMargin * 3 - authButton.Width, 35);

            createNovelButton.AutoSize = true;
            createNovelButton.Height = 35;
            createNovelButton.Padding = new Padding(10, 0, 10, 0);
            createNovelButton.MinimumSize = new Size(100, 35);
            createNovelButton.Location = new Point(authButton.Left - createNovelButton.Width - 10, 35);
        }
        private void CatalogForm_Click(object sender, EventArgs e) {
            novelsFlowPanel.Focus();
        }
        private void DisplayWebnovels() {
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

            DataTable novelsData = catalogSql.GetWebnovels();

            if (novelsData == null || novelsData.Rows.Count == 0) {
                Label noResultsLabel = new Label {
                    Text = "По вашему запросу ничего не найдено.",
                    Font = new Font("Verdana", 16, FontStyle.Regular),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                };
                novelsFlowPanel.Controls.Add(noResultsLabel);
                return;
            }

            foreach (DataRow row in novelsData.Rows) {
                string novelId = row["id"].ToString();
                string coverPath = row["cover_path"]?.ToString();
                string title = row["title"]?.ToString();

                Panel novelPanel = new Panel {
                    Width = CoverWidth,
                    Height = CoverHeight + LabelHeight,
                    Margin = new Padding(0, 0, HorizontalMargin, VerticalMargin)
                };

                PictureBox pictureBox = new PictureBox {
                    Width = CoverWidth,
                    Height = CoverHeight,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BorderStyle = BorderStyle.None,
                    Tag = novelId
                };
                pictureBox.Click += PictureBox_Click;

                try {
                    if (!string.IsNullOrEmpty(coverPath) && File.Exists(coverPath))
                        pictureBox.Image = Image.FromFile(coverPath);
                    else
                        pictureBox.Image = CreatePlaceholderImage(CoverWidth, CoverHeight);
                } catch (Exception ex) {
                    Console.WriteLine($"Ошибка загрузки обложки {coverPath}: {ex.Message}");
                    pictureBox.Image = CreatePlaceholderImage(CoverWidth, CoverHeight);
                }

                Label titleLabel = new Label {
                    Text = TruncateText(title, new Font("Verdana", 16, FontStyle.Bold), CoverWidth - 5),
                    Font = new Font("Verdana", 16, FontStyle.Bold),
                    ForeColor = Color.Black,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Width = CoverWidth,
                    Height = LabelHeight,
                    Location = new Point(0, CoverHeight)
                };

                novelPanel.Controls.Add(pictureBox);
                novelPanel.Controls.Add(titleLabel);
                novelsFlowPanel.Controls.Add(novelPanel);
            }
            novelsFlowPanel.Focus();
        }

        private Bitmap CreatePlaceholderImage(int width, int height) {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp)) {
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
            if (string.IsNullOrEmpty(text))
                return "";

            if (TextRenderer.MeasureText(text, font, Size.Empty, TextFormatFlags.NoPadding).Width <= maxWidth)
                return text;

            string ellipsis = "...";
            string truncatedText = "";
            int ellipsisWidth = TextRenderer.MeasureText(ellipsis, font, Size.Empty, TextFormatFlags.NoPadding).Width;

            for (int i = 0; i < text.Length; i++) {
                string testText = truncatedText + text[i];
                if (TextRenderer.MeasureText(testText + ellipsis, font, Size.Empty, TextFormatFlags.NoPadding).Width > maxWidth)
                    break;
                truncatedText += text[i];
            }
            return truncatedText.TrimEnd() + ellipsis;
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
            if (!suggestionsListBox.Focused)
                suggestionsListBox.Visible = false;
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e) {
            clearSearchButton.Visible = !string.IsNullOrWhiteSpace(searchTextBox.Text) && searchTextBox.Text != SearchPlaceholderText;

            if (searchTextBox.Focused && searchTextBox.Text != SearchPlaceholderText && searchTextBox.Text.Length > 0) {
                string searchText = searchTextBox.Text.ToLower();
                var filteredTitles = allNovelTitles
                    .Where(title => title.ToLower().Contains(searchText))
                    .Take(5)
                    .ToList();
                suggestionsListBox.Height = filteredTitles.Count() * 23;

                if (filteredTitles.Any()) {
                    suggestionsListBox.DataSource = filteredTitles;
                    suggestionsListBox.Visible = true;
                } else {
                    suggestionsListBox.Visible = false;
                }
            } else {
                suggestionsListBox.Visible = false;
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                e.SuppressKeyPress = true;
                suggestionsListBox.Visible = false;
                DisplayWebnovels();
            } else if (e.KeyCode == Keys.Down && suggestionsListBox.Visible && suggestionsListBox.Items.Count > 0) {
                e.SuppressKeyPress = true;
                suggestionsListBox.Focus();
                suggestionsListBox.SelectedIndex = 0;
            } else if (e.KeyCode == Keys.Escape) {
                suggestionsListBox.Visible = false;
            }
        }

        private void ClearSearchButton_Click(object sender, EventArgs e) {
            searchTextBox.Text = "";
            searchTextBox.ForeColor = Color.Gray;
            searchTextBox.Text = SearchPlaceholderText;
            clearSearchButton.Visible = false;
            suggestionsListBox.Visible = false;
            DisplayWebnovels();
            novelsFlowPanel.Focus();
        }

        private void SuggestionsListBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab) {
                e.SuppressKeyPress = true;
                if (suggestionsListBox.SelectedItem != null) {
                    searchTextBox.Text = suggestionsListBox.SelectedItem.ToString();
                    searchTextBox.Focus();
                    searchTextBox.Select(searchTextBox.Text.Length, 0);
                    suggestionsListBox.Visible = false;
                    if (e.KeyCode == Keys.Enter) {
                        DisplayWebnovels();
                    }
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
                DisplayWebnovels();
                novelsFlowPanel.Focus();
            }
        }
        private void AuthButton_Click(object sender, EventArgs e) {
            if (Application.OpenForms.Count == 1) {
                AuthorizationForm authorizationForm = new AuthorizationForm();
                authorizationForm.Owner = this;
                this.Hide();
                authorizationForm.Show();
            }
        }

        private void GenresFilterButton_Click(object sender, EventArgs e) {
            genresContextMenuStrip.Show(genresFilterButton, new Point(0, genresFilterButton.Height));
        }

        private void GenreMenuItem_CheckedChanged(object sender, EventArgs e) {
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            if (clickedItem == null)
                return;

            string genreName = clickedItem.Text;

            if (clickedItem.Checked) {
                if (!selectedGenresFilter.Contains(genreName)) {
                    selectedGenresFilter.Add(genreName);
                }
            } else {
                selectedGenresFilter.Remove(genreName);
            }
            UpdateGenresFilterButtonText();
            DisplayWebnovels();
            UpdateFilterControlsLayout();
        }

        private void ResetGenresFilterMenuItem_Click(object sender, EventArgs e) {
            selectedGenresFilter.Clear();
            foreach (ToolStripItem item in genresContextMenuStrip.Items) {
                if (item is ToolStripMenuItem genreItem && genreItem != resetGenresFilterMenuItem) {
                    genreItem.Checked = false;
                }
            }
            UpdateGenresFilterButtonText();
            DisplayWebnovels();
            UpdateFilterControlsLayout();
        }

        private void UpdateGenresFilterButtonText() {
            if (selectedGenresFilter.Any()) {
                genresFilterButton.Text = "📚 " + string.Join(", ", selectedGenresFilter);
            } else {
                genresFilterButton.Text = "📚 Жанры";
            }
        }

        private void AgeRatingFilterButton_Click(object sender, EventArgs e) {
            ageRatingContextMenuStrip.Show(ageRatingFilterButton, new Point(0, ageRatingFilterButton.Height));
        }

        private void AgeRatingMenuItem_CheckedChanged(object sender, EventArgs e) {
            ToolStripMenuItem clickedItem = sender as ToolStripMenuItem;
            if (clickedItem == null || clickedItem.Tag == null)
                return;

            int ageRating = Convert.ToInt32(clickedItem.Tag);

            if (clickedItem.Checked) {
                if (!selectedAgeRatingsFilter.Contains(ageRating)) {
                    selectedAgeRatingsFilter.Add(ageRating);
                }
            } else {
                selectedAgeRatingsFilter.Remove(ageRating);
            }
            UpdateAgeRatingFilterButtonText();
            DisplayWebnovels();
            UpdateFilterControlsLayout();
        }

        private void ResetAgeRatingFilterMenuItem_Click(object sender, EventArgs e) {
            selectedAgeRatingsFilter.Clear();
            foreach (ToolStripItem item in ageRatingContextMenuStrip.Items) {
                if (item is ToolStripMenuItem ageItem && ageItem.Tag != null) {
                    ageItem.Checked = false;
                }
            }
            UpdateAgeRatingFilterButtonText();
            DisplayWebnovels();
            UpdateFilterControlsLayout();
        }

        private void UpdateAgeRatingFilterButtonText() {
            if (selectedAgeRatingsFilter.Any()) {
                selectedAgeRatingsFilter.Sort();
                ageRatingFilterButton.Text = "🚫 " + string.Join(", ", selectedAgeRatingsFilter.Select(r => r + "+"));
            } else {
                ageRatingFilterButton.Text = "🚫 Возрастные ограничения";
            }
        }

        private void YearSortButton_Click(object sender, EventArgs e) {
            switch (currentSortOrder) {
                case SortOrderState.None:
                    currentSortOrder = SortOrderState.Ascending;
                    yearSortButton.Text = "📅 Год выпуска ▲";
                    break;
                case SortOrderState.Ascending:
                    currentSortOrder = SortOrderState.Descending;
                    yearSortButton.Text = "📅 Год выпуска ▼";
                    break;
                case SortOrderState.Descending:
                    currentSortOrder = SortOrderState.None;
                    yearSortButton.Text = "📅 Год выпуска";
                    break;
            }
            DisplayWebnovels();
            UpdateFilterControlsLayout();
        }

        private void PictureBox_Click(object sender, EventArgs e) {
            PictureBox clickedPictureBox = sender as PictureBox;
            if (Application.OpenForms.Count == 1 && clickedPictureBox != null && clickedPictureBox.Tag != null) {
                selectedNovelID = clickedPictureBox.Tag.ToString();
                BookForm novelForm = new BookForm();
                novelForm.Owner = this;
                this.Hide();
                novelForm.Show();
            }
        }
        private string selectedNovelID;
        public string GetSelectedNovelID {
            get { return selectedNovelID; }
        }
        public User User {
            get { return user; }
            set {
                user = value;
                catalogSql.updateUser(user);
                WriterCheck();
            }
        }
        private void WriterCheck() {
            if (user.IsWriter == true)
                createNovelButton.Visible = true;
            else
                createNovelButton.Visible = false;
        }
        private void CreateNovelButton_Click(object sender, EventArgs e) {
            if (Application.OpenForms.Count == 1) {
                selectedNovelID = "0";
                BookForm novelForm = new BookForm();
                novelForm.Owner = this;
                this.Hide();
                novelForm.Show();
            }
        }
        private void CatalogForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (catalogSql != null) {
                catalogSql.Dispose();
            }
            foreach (Control panelControl in novelsFlowPanel.Controls) {
                if (panelControl is Panel novelPanel) {
                    foreach (Control itemControl in novelPanel.Controls) {
                        if (itemControl is PictureBox pb && pb.Image != null) {
                            pb.Image.Dispose();
                        }
                    }
                }
                panelControl.Dispose();
            }
            novelsFlowPanel.Controls.Clear();
        }
        private void CatalogForm_Activated(object sender, EventArgs e) {
            if (catalogSql != null) DisplayWebnovels();
        }
    }
}