using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
// using bookservice.FB2Logic; // Не требуется напрямую здесь, LoadingForm этим занимается

namespace bookservice
{
    // Note: Many UI element names like 'novelTabPage', 'createNovelTabPage', 'downloadNovelLabelButton'
    // are defined in the .Designer.cs file and cannot be changed here directly in the .cs file.
    // I will adapt the logic assuming these names are updated in the Designer or new controls are added.
    // For 'downloadNovelLabelButton', I'll assume it's now 'readBookLabelButton' conceptually.
    // The ContextMenuStrip 'novelFilesContextMenuStrip' will be reused or conceptually become 'bookFilesContextMenuStrip'.
    public partial class BookForm : Form
    {
        private string selectedBookIDString;
        private int currentBookId;
        private CatalogForm catalogForm;
        private User user;
        private BookSQL bookSql;
        private BookDetails currentBookDetails;
        private List<BookFile> currentBookFiles;
        private const string CommentPlaceholder = "Написать комментарий...";
        private const int coverWidth = 432;
        private const int coverHeight = 609;

        public BookForm()
        {
            InitializeComponent(); // This would initialize all controls from Designer.cs
            this.BackColor = SystemColors.ControlLightLight;
            this.Font = new Font("Verdana", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;

            // Example: If readBookLabelButton is the new name for the download button:
            // if (this.Controls.ContainsKey("downloadNovelLabelButton"))
            // {
            //     var oldButton = this.Controls["downloadNovelLabelButton"];
            //     readBookLabelButton = new Label // Or Button, depending on original type
            //     {
            //         Name = "readBookLabelButton",
            //         Text = "Читать книгу",
            //         Location = oldButton.Location,
            //         Size = oldButton.Size,
            //         Font = oldButton.Font, // Match styling
            //         BorderStyle = BorderStyle.FixedSingle, // Example style
            //         TextAlign = ContentAlignment.MiddleCenter,
            //         Cursor = Cursors.Hand,
            //         Enabled = false // Initially disabled
            //     };
            //     readBookLabelButton.Click += new System.EventHandler(this.readBookLabelButton_Click);
            //     this.Controls.Remove(oldButton);
            //     this.Controls.Add(readBookLabelButton);
            // }
            // else if (this.Controls.ContainsKey("readBookLabelButton"))
            // {
            //    readBookLabelButton = (Label)this.Controls["readBookLabelButton"];
            //    readBookLabelButton.Text = "Читать книгу";
            // }


            // Ensure bookFilesContextMenuStrip exists (formerly novelFilesContextMenuStrip)
            // if (this.components.Components.OfType<ContextMenuStrip>().Any(c => c.Name == "novelFilesContextMenuStrip"))
            // {
            //     bookFilesContextMenuStrip = this.components.Components.OfType<ContextMenuStrip>().First(c => c.Name == "novelFilesContextMenuStrip");
            //     bookFilesContextMenuStrip.Name = "bookFilesContextMenuStrip";
            // }
            // else if (this.components.Components.OfType<ContextMenuStrip>().Any(c => c.Name == "bookFilesContextMenuStrip"))
            // {
            //     bookFilesContextMenuStrip = this.components.Components.OfType<ContextMenuStrip>().First(c => c.Name == "bookFilesContextMenuStrip");
            // }
            // else
            // {
            //     bookFilesContextMenuStrip = new ContextMenuStrip();
            // }
        }
        private void NovelForm_Load(object sender, EventArgs e)
        { // Event handler name from Designer
            catalogForm = (CatalogForm)Owner;
            if (catalogForm == null)
            {
                MessageBox.Show("Не удалось получить ссылку на родительскую форму каталога. Форма будет закрыта.", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            user = catalogForm.User;
            selectedBookIDString = catalogForm.GetSelectedBookID;
            bookSql = new BookSQL(user);

            if (string.IsNullOrEmpty(selectedBookIDString) || selectedBookIDString == "0")
            {
                currentBookId = 0;
                Text = "Создание новой книги";
                ConfigureTabsForCreateMode();
                LoadDataForCreateBookTab();
            }
            else
            {
                if (!int.TryParse(selectedBookIDString, out currentBookId))
                {
                    MessageBox.Show("Некорректный ID книги. Форма будет закрыта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                Text = $"Книга: Загрузка...";
                LoadBookData();
                ConfigureTabsForViewEditMode();
            }
            ControlsLayout(); // Call layout after data is loaded and tabs configured
        }
        private void ConfigureTabsForCreateMode()
        {
            EnsureAllTabsPresent();
            if (bookTabControl.TabPages.Contains(bookTabPage))
                bookTabControl.TabPages.Remove(bookTabPage);
            if (bookTabControl.TabPages.Contains(commentsTabPage))
                bookTabControl.TabPages.Remove(commentsTabPage);
            if (bookTabControl.TabPages.Contains(editBookTabPage))
                bookTabControl.TabPages.Remove(editBookTabPage);

            if (!bookTabControl.TabPages.Contains(createBookTabPage))
            {
                // Insert at the beginning or specific index if needed
                bookTabControl.TabPages.Insert(0, createBookTabPage);
            }
            bookTabControl.SelectedTab = createBookTabPage;
        }
        private void ConfigureTabsForViewEditMode()
        {
            EnsureAllTabsPresent();
            bookTabControl.TabPages.Clear(); // Clear all first

            bookTabControl.TabPages.Add(bookTabPage);

            if (user.Role == UserRole.READER || user.Role == UserRole.ADMIN)
            {
                if (!bookTabControl.TabPages.Contains(commentsTabPage))
                    bookTabControl.TabPages.Add(commentsTabPage);
                LoadComments();
            }

            if (user.Role == UserRole.ADMIN && currentBookDetails != null && user.Id == currentBookDetails.AdminId)
            {
                if (!bookTabControl.TabPages.Contains(editBookTabPage))
                    bookTabControl.TabPages.Add(editBookTabPage);
                // TODO: Load data for edit tab if necessary, e.g., pre-fill fields from currentBookDetails
            }

            if (bookTabControl.TabPages.Contains(createBookTabPage)) // Should not be present in view/edit mode
                bookTabControl.TabPages.Remove(createBookTabPage);

            if (bookTabControl.TabPages.Count > 0)
            {
                bookTabControl.SelectedTab = bookTabControl.TabPages[0]; // Select the first available tab (usually bookTabPage)
            }
        }
        private void EnsureAllTabsPresent()
        {
            // This is a defensive measure. These TabPage objects are usually created by the designer.
            // If they are removed from bookTabControl.TabPages, their Parent becomes null.
            // This method ensures they are re-added if they were previously removed.
            if (bookTabPage != null && bookTabPage.Parent == null) bookTabControl.TabPages.Add(bookTabPage);
            if (commentsTabPage != null && commentsTabPage.Parent == null) bookTabControl.TabPages.Add(commentsTabPage);
            if (editBookTabPage != null && editBookTabPage.Parent == null) bookTabControl.TabPages.Add(editBookTabPage);
            if (createBookTabPage != null && createBookTabPage.Parent == null) bookTabControl.TabPages.Add(createBookTabPage);
        }
        private void LoadBookData()
        {
            if (currentBookId == 0) return;
            currentBookDetails = bookSql.GetBookDetails(currentBookId);

            if (currentBookDetails == null)
            {
                MessageBox.Show("Не удалось загрузить информацию о книге.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Text = "Книга: Ошибка загрузки";
                if (readBookLabelButton != null) readBookLabelButton.Enabled = false;
                return;
            }
            this.Text = $"Книга: {currentBookDetails.Title}";
            titleLabel.Text = currentBookDetails.Title;
            authorLabel.Text = $"👤 Автор: {currentBookDetails.WriterName} (Добавил: {currentBookDetails.AdminLogin})";
            yearLabel.Text = currentBookDetails.PublicationYear.HasValue ? $"📅 Год: {currentBookDetails.PublicationYear.Value}" : "📅 Год: Н/Д";
            ageRatingLabel.Text = currentBookDetails.AgeRating.HasValue ? $"🚫 {currentBookDetails.AgeRating.Value}+" : "🚫 Н/Д";
            descriptionLabel.Text = currentBookDetails.Description ?? "Описание отсутствует.";

            if (coverPictureBox.Image != null)
            {
                coverPictureBox.Image.Dispose();
                coverPictureBox.Image = null;
            }
            if (!string.IsNullOrEmpty(currentBookDetails.CoverPath) && File.Exists(GetAbsolutePath(currentBookDetails.CoverPath)))
            {
                try
                {
                    using (FileStream stream = new FileStream(GetAbsolutePath(currentBookDetails.CoverPath), FileMode.Open, FileAccess.Read))
                    {
                        coverPictureBox.Image = Image.FromStream(stream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка загрузки обложки: " + ex.Message);
                    coverPictureBox.Image = CreatePlaceholderImage(coverWidth, coverHeight);
                }
            }
            else
            {
                coverPictureBox.Image = CreatePlaceholderImage(coverWidth, coverHeight);
            }
            LoadBookGenres();
            currentBookFiles = bookSql.GetBookFiles(currentBookId);
            // Assuming readBookLabelButton is the new name for downloadNovelLabelButton
            if (readBookLabelButton != null)
            {
                readBookLabelButton.Enabled = currentBookFiles != null && currentBookFiles.Count > 0;
                readBookLabelButton.Text = "Читать книгу"; // Ensure text is set
            }

            ControlsLayout(); // Re-apply layout after loading data
        }
        private void LoadBookGenres()
        {
            genresFlowLayoutPanel.Controls.Clear();
            if (currentBookId == 0 || currentBookDetails == null) return;
            List<string> genres = bookSql.GetBookGenres(currentBookDetails.Id);
            if (genres != null && genres.Count > 0)
            {
                foreach (string genreName in genres)
                {
                    Label genreLabel = new Label
                    {
                        Text = genreName,
                        Font = new Font("Verdana", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204))),
                        BackColor = SystemColors.Control,
                        BorderStyle = BorderStyle.FixedSingle,
                        Padding = new Padding(10, 5, 10, 5),
                        AutoSize = true,
                        Margin = new Padding(0, 0, 10, 0)
                    };
                    genresFlowLayoutPanel.Controls.Add(genreLabel);
                }
            }
        }
        private void LoadComments()
        {
            if (currentBookId == 0) return;
            commentsHostPanel.Controls.Clear(); // Assuming commentsHostPanel is a Panel or similar container
            List<BookComment> comments = bookSql.GetBookComments(currentBookId);
            if (comments != null && comments.Count > 0)
            {
                int yPos = 5;
                foreach (var commentData in comments)
                {
                    Panel commentPanel = new Panel
                    {
                        Width = commentsHostPanel.ClientSize.Width - 20, // Adjust for padding/margin
                        AutoSize = true,
                        Margin = new Padding(5, 0, 5, 10),
                        BorderStyle = BorderStyle.FixedSingle // Optional: to visually separate comments
                    };
                    Label userLoginLabel = new Label
                    {
                        Text = $"{commentData.UserLogin} ({commentData.CommentDateTime:dd.MM.yyyy HH:mm}):",
                        Font = new Font("Verdana", 12F, FontStyle.Bold), // Adjusted font size
                        AutoSize = true,
                        Location = new Point(5, 5),
                        ForeColor = Color.DarkSlateBlue
                    };
                    Label commentTextLabel = new Label
                    {
                        Text = commentData.Text,
                        Font = new Font("Verdana", 11F, FontStyle.Regular), // Adjusted font size
                        AutoSize = false, // Important for wrapping
                        Width = commentPanel.Width - 10, // Account for internal padding
                        Location = new Point(5, userLoginLabel.Bottom + 5),
                        MaximumSize = new Size(commentPanel.Width - 10, 0), // Allow unlimited height
                        AutoEllipsis = false // Disable ellipsis, let text wrap
                    };
                    // Measure text height for the label
                    using (Graphics g = CreateGraphics())
                    {
                        SizeF size = g.MeasureString(commentTextLabel.Text, commentTextLabel.Font, commentTextLabel.Width);
                        commentTextLabel.Height = (int)Math.Ceiling(size.Height);
                    }

                    commentPanel.Controls.Add(userLoginLabel);
                    commentPanel.Controls.Add(commentTextLabel);
                    commentPanel.Height = commentTextLabel.Bottom + 10; // Adjust panel height
                    commentPanel.Location = new Point(10, yPos); // Position within commentsHostPanel

                    commentsHostPanel.Controls.Add(commentPanel);
                    yPos += commentPanel.Height + 10; // Update Y position for next comment
                }
            }
            else
            {
                Label noCommentsLabel = new Label
                {
                    Text = "Комментариев пока нет.",
                    Font = new Font("Verdana", 12F, FontStyle.Italic), // Adjusted font
                    AutoSize = true,
                    Location = new Point(10, 10) // Position within commentsHostPanel
                };
                commentsHostPanel.Controls.Add(noCommentsLabel);
            }
            commentsHostPanel.PerformLayout(); // Refresh layout of the host panel
        }
        private string GetAbsolutePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return "";
            if (Path.IsPathRooted(relativePath)) return relativePath;
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
        private void ControlsLayout()
        {
            // Ensure bookTabPage is the one being laid out if it's visible
            TabPage currentDisplayTab = bookTabControl.SelectedTab;
            if (currentDisplayTab == null) currentDisplayTab = bookTabPage; // Default to bookTabPage if nothing selected (e.g. during load)
            if (currentDisplayTab == null) return; // Should not happen if tabs are configured

            // Layout for bookTabPage (main book view)
            if (currentDisplayTab == bookTabPage && bookTabPage.Parent != null)
            {
                int currentX = titleLabel.Left; // Assuming titleLabel is already positioned by designer
                int currentY = titleLabel.Bottom + 15;

                authorLabel.Location = new Point(currentX, currentY);
                authorLabel.Width = currentDisplayTab.ClientSize.Width - authorLabel.Left - currentX; // Make it take available width

                currentY = authorLabel.Bottom + 10;
                genresFlowLayoutPanel.Location = new Point(currentX, currentY);
                genresFlowLayoutPanel.MaximumSize = new Size(currentDisplayTab.ClientSize.Width - currentX - 20, 0); // Max width, auto height
                genresFlowLayoutPanel.PerformLayout(); // Recalculate layout of genres

                // Position year and age rating relative to author/genres block
                int infoBlockRightEdge = currentX; // Start with X of title/author
                int infoBlockTopY = authorLabel.Location.Y; // Align with authorLabel's top

                yearLabel.Location = new Point(authorLabel.Right + 20, infoBlockTopY); // Example positioning
                ageRatingLabel.Location = new Point(yearLabel.Right + 20, infoBlockTopY); // Example

                // Ensure description is below the dynamic genresFlowLayoutPanel
                currentY = genresFlowLayoutPanel.Bottom + 15;
                descriptionLabel.Location = new Point(currentX, currentY);
                descriptionLabel.Size = new Size(
                    Math.Max(200, currentDisplayTab.ClientSize.Width - currentX - 20), // Min width 200
                    Math.Max(50, currentDisplayTab.ClientSize.Height - descriptionLabel.Top - (readBookLabelButton != null ? readBookLabelButton.Height + 30 : 20)) // Adjust for read button
                );
                readBookLabelButton.Location = new Point(coverPictureBox.Left, coverPictureBox.Bottom + 10);
            }

            // Layout for commentsTabPage
            if (currentDisplayTab == commentsTabPage && commentsTabPage.Parent != null)
            {
                commentsTitleLabel.Location = new Point(15, 15); // Example position
                newCommentTextBox.Location = new Point(15, commentsTitleLabel.Bottom + 10);
                newCommentTextBox.Width = Math.Max(200, commentsTabPage.ClientSize.Width - 30); // Min width 200

                postCommentButton.Location = new Point(15, newCommentTextBox.Bottom + 10);

                commentsHostPanel.Location = new Point(15, postCommentButton.Bottom + 20);
                commentsHostPanel.Size = new Size(
                    Math.Max(200, commentsTabPage.ClientSize.Width - 30),
                    Math.Max(100, commentsTabPage.ClientSize.Height - commentsHostPanel.Top - 20)
                );
            }

            // Layout for createBookTabPage
            if (currentDisplayTab == createBookTabPage && createBookTabPage.Parent != null)
            {
                // This is a simplified layout, adjust based on your actual controls in createBookTabPage
                int createControlsWidth = Math.Max(300, createBookTabPage.ClientSize.Width - 40);
                int leftMargin = 20;
                int currentCreateY = 20;

                createTitlePromptLabel.Location = new Point(leftMargin, currentCreateY);
                createTitleTextBox.Location = new Point(createTitlePromptLabel.Right + 5, currentCreateY);
                createTitleTextBox.Width = createControlsWidth - createTitlePromptLabel.Width - 10;
                currentCreateY = createTitleTextBox.Bottom + 10;
                // Assuming createWriterNamePromptLabel and createWriterNameTextBox exist
                // Label createWriterNamePromptLabel = this.Controls.Find("createWriterNamePromptLabel", true).FirstOrDefault() as Label;
                if (createWriterNamePromptLabel != null && createWriterNameTextBox != null)
                {
                    createWriterNamePromptLabel.Location = new Point(leftMargin, currentCreateY);
                    createWriterNameTextBox.Location = new Point(createWriterNamePromptLabel.Right + 5, currentCreateY);
                    createWriterNameTextBox.Width = createTitleTextBox.Width;
                    currentCreateY = createWriterNameTextBox.Bottom + 10;
                }

                // ... layout for other create controls (year, age rating, description, cover, genres) ...
                // Example for description:
                // Label createDescriptionPromptLabel = ...
                // createDescriptionPromptLabel.Location = new Point(leftMargin, currentCreateY);
                // createDescriptionTextBox.Location = new Point(leftMargin, createDescriptionPromptLabel.Bottom + 5);
                // createDescriptionTextBox.Size = new Size(createControlsWidth, 100); // Example height
                // currentCreateY = createDescriptionTextBox.Bottom + 10;

                // createBookButton (center it)
                createBookButton.Location = new Point((createBookTabPage.ClientSize.Width - createBookButton.Width) / 2,
                                                    Math.Max(currentCreateY + 20, createBookTabPage.ClientSize.Height - createBookButton.Height - 20));
            }
            // Add layout for editBookTabPage if needed
            // if (currentDisplayTab == editBookTabPage && editBookTabPage.Parent != null) { ... }
        }
        private void NovelForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (bookSql != null)
                bookSql.Dispose();
            if (coverPictureBox.Image != null)
            {
                coverPictureBox.Image.Dispose();
                coverPictureBox.Image = null;
            }
            if (catalogForm != null && !catalogForm.IsDisposed)
            {
                catalogForm.Show();
                catalogForm.Activate(); // Ensure it gets focus and refreshes if needed
            }
        }

        // Event handler for the "Читать книгу" button/label
        // Assuming 'readBookLabelButton' is the new name for 'downloadNovelLabelButton'
        // And 'bookFilesContextMenuStrip' for 'novelFilesContextMenuStrip'
        private void readBookLabelButton_Click(object sender, EventArgs e)
        {
            if (currentBookFiles == null || currentBookFiles.Count == 0)
            {
                MessageBox.Show("Для этой книги нет доступных файлов для чтения.", "Файлы не найдены", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (currentBookFiles.Count == 1)
            {
                // If only one file, read it directly without showing context menu
                BookFile singleFile = currentBookFiles[0];
                InitiateReading(singleFile);
            }
            else
            {
                // Multiple files, show context menu
                bookFilesContextMenuStrip.Items.Clear();
                foreach (var bookFile in currentBookFiles)
                {
                    // Ensure we are only adding .fb2 files if other types could exist (though DB constraint helps)
                    if (Path.GetExtension(bookFile.FilePath ?? "").ToLower() == ".fb2")
                    {
                        ToolStripMenuItem menuItem = new ToolStripMenuItem(bookFile.FileName + Path.GetExtension(bookFile.FilePath)); // Display with extension
                        menuItem.Tag = bookFile;
                        menuItem.Click += BookFileMenuItem_Click; // Reusing the adapted click handler
                        bookFilesContextMenuStrip.Items.Add(menuItem);
                    }
                }

                if (bookFilesContextMenuStrip.Items.Count > 0)
                {
                    Control control = (Control)sender; // The 'readBookLabelButton'
                    bookFilesContextMenuStrip.Show(control, new Point(0, control.Height));
                }
                else
                {
                    MessageBox.Show("Не найдено файлов формата .fb2 для этой книги.", "Файлы .fb2 не найдены", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // This event is now for initiating reading the selected file
        private void BookFileMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem != null && menuItem.Tag is BookFile bookFile)
            {
                InitiateReading(bookFile);
            }
        }

        private void InitiateReading(BookFile bookFile)
        {
            if (bookFile == null || string.IsNullOrEmpty(bookFile.FilePath))
            {
                MessageBox.Show("Информация о файле некорректна.", "Ошибка файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string sourceFilePath = GetAbsolutePath(bookFile.FilePath);
            if (!File.Exists(sourceFilePath))
            {
                MessageBox.Show($"Файл '{Path.GetFileName(sourceFilePath)}' не найден по пути: {sourceFilePath}", "Ошибка файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Path.GetExtension(sourceFilePath).ToLower() != ".fb2")
            {
                MessageBox.Show($"Выбранный файл '{Path.GetFileName(sourceFilePath)}' не является файлом формата .fb2.", "Неверный формат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (user == null) // Should not happen if form logic is correct
            {
                MessageBox.Show("Ошибка: Информация о текущем пользователе отсутствует.", "Ошибка пользователя", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (currentBookDetails == null)
            {
                MessageBox.Show("Ошибка: Информация о книге отсутствует.", "Ошибка книги", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // Open LoadingForm, which will then open ReadingForm
            this.Hide(); // Hide BookForm while Loading/Reading is active
            using (LoadingForm loadingForm = new LoadingForm(user, currentBookId, sourceFilePath, currentBookDetails.Title))
            {
                //loadingForm.Owner = this; // Set owner if BookForm should reappear focused.
                // However, FormClosed event of BookForm handles showing CatalogForm.
                // Showing LoadingForm modally to BookForm's Owner (CatalogForm) might be better.
                DialogResult result = loadingForm.ShowDialog(this.Owner); // Show modally to CatalogForm or whatever Owner is.
            }
            // After LoadingForm (and ReadingForm) are closed, this BookForm might be closed by its own logic (e.g. delete)
            // or the user might expect to return to it. The FormClosed event handles showing CatalogForm.
            // If BookForm is still open and should be shown:
            if (!this.IsDisposed)
            {
                this.Show();
                this.Activate();
            }
            else
            {
                // If BookForm was closed (e.g. book deleted from reader), ensure CatalogForm is shown
                if (catalogForm != null && !catalogForm.IsDisposed)
                {
                    catalogForm.Show();
                    catalogForm.Activate();
                }
            }
        }


        private void NewCommentTextBox_Enter(object sender, EventArgs e)
        {
            if (newCommentTextBox.Text == CommentPlaceholder)
            {
                newCommentTextBox.Text = "";
                newCommentTextBox.ForeColor = SystemColors.WindowText;
            }
        }
        private void NewCommentTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(newCommentTextBox.Text))
            {
                newCommentTextBox.Text = CommentPlaceholder;
                newCommentTextBox.ForeColor = Color.Gray;
            }
        }
        private void NewCommentTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                postCommentButton_Click(postCommentButton, EventArgs.Empty);
                e.SuppressKeyPress = true;
            }
        }
        private void postCommentButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(newCommentTextBox.Text) || newCommentTextBox.Text == CommentPlaceholder)
            {
                MessageBox.Show("Комментарий не может быть пустым.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (user.Id == 0)
            {
                MessageBox.Show("Гости не могут оставлять комментарии. Пожалуйста, войдите или зарегистрируйтесь.", "Действие недоступно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            bool success = bookSql.AddComment(user.Id, currentBookId, newCommentTextBox.Text.Trim());
            if (success)
            {
                MessageBox.Show("Комментарий успешно добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                newCommentTextBox.Text = "";
                NewCommentTextBox_Leave(newCommentTextBox, EventArgs.Empty);
                LoadComments();
            }
            else
            {
                MessageBox.Show("Не удалось добавить комментарий. Попробуйте позже.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void addFileButton_Click(object sender, EventArgs e)
        {
            if (currentBookId == 0 || currentBookDetails == null)
            {
                MessageBox.Show("Сначала необходимо сохранить основную информацию о книге.", "Действие не доступно", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            openNovelFileDialog.Filter = "FB2 файлы (*.fb2)|*.fb2";
            openNovelFileDialog.Title = "Выберите файл книги (.fb2)";
            openNovelFileDialog.FileName = ""; // Clear previous filename

            if (openNovelFileDialog.ShowDialog() == DialogResult.OK)
            {
                string sourceFilePath = openNovelFileDialog.FileName;
                string fileExtension = Path.GetExtension(sourceFilePath);
                if (fileExtension.ToLower() != ".fb2")
                {
                    MessageBox.Show("Пожалуйста, выберите файл формата .fb2.", "Неверный формат файла", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string relativeDestFolder = Path.Combine("..", "..", "..", "files");
                string absoluteDestFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeDestFolder));
                if (!Directory.Exists(absoluteDestFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(absoluteDestFolder);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Не удалось создать директорию для файлов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                string destinationFileName = Path.GetFileName(sourceFilePath);
                // Ensure unique filename in destination to avoid overwriting unrelated files
                string uniqueDestinationFileName = Guid.NewGuid().ToString() + "_" + destinationFileName;
                string dbFilePath = Path.Combine("..\\..\\..\\files", uniqueDestinationFileName).Replace(Path.DirectorySeparatorChar, '\\');
                string absoluteDestinationPath = Path.Combine(absoluteDestFolder, uniqueDestinationFileName);

                DialogResult confirmResult = MessageBox.Show($"Добавить файл '{destinationFileName}' к книге '{currentBookDetails.Title}'?",
                                                           "Подтверждение добавления файла",
                                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        File.Copy(sourceFilePath, absoluteDestinationPath, true);
                        // Pass the original filename (without GUID) for display, but the unique path for storage
                        bool success = bookSql.AddBookFile(currentBookId, Path.GetFileNameWithoutExtension(destinationFileName), dbFilePath);
                        if (success)
                        {
                            MessageBox.Show("Файл успешно добавлен и запись в БД создана.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            currentBookFiles = bookSql.GetBookFiles(currentBookId); // Refresh file list
                            if (readBookLabelButton != null) readBookLabelButton.Enabled = currentBookFiles != null && currentBookFiles.Count > 0;
                        }
                        else
                        {
                            MessageBox.Show("Файл скопирован, но не удалось добавить запись в БД. Возможно, потребуется удалить файл вручную.", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (File.Exists(absoluteDestinationPath))
                                File.Delete(absoluteDestinationPath); // Attempt to clean up
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при добавлении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void deleteBookButton_Click(object sender, EventArgs e)
        {
            if (currentBookId == 0 || currentBookDetails == null) return;
            DialogResult confirmResult = MessageBox.Show($"Вы уверены, что хотите удалить книгу '{currentBookDetails.Title}'?\n" +
                                                       "Это действие необратимо и удалит все связанные файлы, комментарии и жанры.",
                                                       "Подтверждение удаления",
                                                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                // Dispose image before trying to delete file if that's part of DeleteBook
                if (coverPictureBox.Image != null)
                {
                    coverPictureBox.Image.Dispose();
                    coverPictureBox.Image = null;
                }
                // Get file paths before deleting DB record
                string coverPathToDelete = null;
                if (!string.IsNullOrEmpty(currentBookDetails.CoverPath))
                    coverPathToDelete = GetAbsolutePath(currentBookDetails.CoverPath);

                List<string> bookFilePathsToDelete = new List<string>();
                if (currentBookFiles != null)
                {
                    foreach (var bookfile in currentBookFiles)
                    {
                        if (!string.IsNullOrEmpty(bookfile.FilePath))
                            bookFilePathsToDelete.Add(GetAbsolutePath(bookfile.FilePath));
                    }
                }

                bool success = bookSql.DeleteBook(currentBookId);
                if (success)
                {
                    // Try to delete physical files after DB record is gone
                    if (coverPathToDelete != null && File.Exists(coverPathToDelete))
                    {
                        try { File.Delete(coverPathToDelete); } catch (Exception ex) { Console.WriteLine($"Error deleting cover file {coverPathToDelete}: {ex.Message}"); }
                    }
                    foreach (string bookFilePath in bookFilePathsToDelete)
                    {
                        if (File.Exists(bookFilePath))
                        {
                            try { File.Delete(bookFilePath); } catch (Exception ex) { Console.WriteLine($"Error deleting book file {bookFilePath}: {ex.Message}"); }
                        }
                    }
                    MessageBox.Show("Книга успешно удалена.", "Удаление завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    LoadBookData(); // Reload data to reflect current state (e.g. if cover was disposed but DB delete failed)
                    MessageBox.Show("Не удалось удалить книгу. Проверьте консоль на наличие ошибок.", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private string selectedCoverPathForCreation = null;
        private void LoadDataForCreateBookTab()
        {
            createGenresCheckedListBox.Items.Clear();
            List<KeyValuePair<int, string>> allGenres = bookSql.GetAllGenres();
            foreach (var genrePair in allGenres)
                createGenresCheckedListBox.Items.Add(genrePair, false);
            createGenresCheckedListBox.DisplayMember = "Value";
            createGenresCheckedListBox.ValueMember = "Key";

            createTitleTextBox.Clear();
            if (createWriterNameTextBox != null) createWriterNameTextBox.Clear();
            createYearTextBox.Clear();
            createAgeRatingListBox.ClearSelected(); // Or SelectedIndex = -1;
            createDescriptionTextBox.Clear();
            createCoverPathLabel.Text = "Файл не выбран";
            selectedCoverPathForCreation = null;
        }
        private void createSelectCoverButton_Click(object sender, EventArgs e)
        {
            openCoverFileDialog.Filter = "Изображения (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openCoverFileDialog.Title = "Выберите обложку для книги";
            openCoverFileDialog.FileName = ""; // Clear previous filename
            if (openCoverFileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult confirmResult = MessageBox.Show($"Использовать изображение '{Path.GetFileName(openCoverFileDialog.FileName)}' в качестве обложки?",
                                                           "Подтверждение выбора обложки", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    selectedCoverPathForCreation = openCoverFileDialog.FileName;
                    createCoverPathLabel.Text = Path.GetFileName(selectedCoverPathForCreation);
                }
            }
        }
        private void createBookButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(createTitleTextBox.Text))
            {
                MessageBox.Show("Название книги не может быть пустым.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                createTitleTextBox.Focus();
                return;
            }
            string writerName = "";
            if (createWriterNameTextBox != null)
            { // Check if the control exists
                writerName = createWriterNameTextBox.Text.Trim();
            }
            if (string.IsNullOrWhiteSpace(writerName))
            {
                MessageBox.Show("Имя автора не может быть пустым.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                createWriterNameTextBox?.Focus();
                return;
            }

            short? publicationYear = null;
            if (!string.IsNullOrWhiteSpace(createYearTextBox.Text))
            {
                if (short.TryParse(createYearTextBox.Text, out short yearValue) && yearValue > 0 && yearValue <= DateTime.Now.Year + 20)
                { // Allow some future years
                    publicationYear = yearValue;
                }
                else
                {
                    MessageBox.Show("Год публикации указан некорректно. Оставьте поле пустым или введите корректный год (например, 2023).", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    createYearTextBox.Focus();
                    return;
                }
            }
            short? ageRating = null;
            if (createAgeRatingListBox.SelectedItem != null)
            {
                string ratingStr = createAgeRatingListBox.SelectedItem.ToString().Replace("+", "");
                if (short.TryParse(ratingStr, out short ratingValue))
                {
                    ageRating = ratingValue;
                }
            }
            if (user.Id == 0)
            {
                MessageBox.Show("Текущий пользователь не может создавать книги (ID пользователя не определен или нет прав).", "Ошибка прав", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!user.IsAdmin)
            { // Only admins can create books
                MessageBox.Show("Только администраторы могут создавать книги.", "Ошибка прав", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string finalCoverRelativePath = null;
            if (!string.IsNullOrEmpty(selectedCoverPathForCreation))
            {
                try
                {
                    string coverFileName = Path.GetFileName(selectedCoverPathForCreation);
                    string relativeDestFolder = Path.Combine("..", "..", "..", "covers");
                    string absoluteDestFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeDestFolder));
                    if (!Directory.Exists(absoluteDestFolder))
                        Directory.CreateDirectory(absoluteDestFolder);

                    string uniqueCoverFileName = Guid.NewGuid().ToString() + Path.GetExtension(coverFileName);
                    string absoluteDestinationPath = Path.Combine(absoluteDestFolder, uniqueCoverFileName);
                    File.Copy(selectedCoverPathForCreation, absoluteDestinationPath, true);
                    finalCoverRelativePath = Path.Combine("..\\..\\..\\covers", uniqueCoverFileName).Replace(Path.DirectorySeparatorChar, '\\');
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении обложки: {ex.Message}\nКнига будет создана без обложки.", "Ошибка обложки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    finalCoverRelativePath = null;
                }
            }
            List<int> selectedGenreIds = new List<int>();
            foreach (var item in createGenresCheckedListBox.CheckedItems)
            {
                if (item is KeyValuePair<int, string> genrePair)
                    selectedGenreIds.Add(genrePair.Key);
            }
            DialogResult confirmCreate = MessageBox.Show("Создать новую книгу с указанными данными?", "Подтверждение создания", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmCreate == DialogResult.No)
            {
                if (!string.IsNullOrEmpty(finalCoverRelativePath))
                { // Cleanup copied cover if creation cancelled
                    string absPathToDelete = GetAbsolutePath(finalCoverRelativePath);
                    if (File.Exists(absPathToDelete)) try { File.Delete(absPathToDelete); } catch { }
                }
                return;
            }
            int newBookId = bookSql.CreateBook(
                createTitleTextBox.Text.Trim(),
                user.Id,
                writerName,
                publicationYear,
                ageRating,
                string.IsNullOrWhiteSpace(createDescriptionTextBox.Text) ? null : createDescriptionTextBox.Text.Trim(),
                finalCoverRelativePath,
                selectedGenreIds
            );
            if (newBookId > 0)
            {
                MessageBox.Show($"Книга '{createTitleTextBox.Text.Trim()}' успешно создана с ID: {newBookId}.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                selectedBookIDString = newBookId.ToString();
                currentBookId = newBookId;
                LoadBookData(); // Load the newly created book's data
                ConfigureTabsForViewEditMode(); // Switch to view mode
                if (bookTabControl.TabPages.Contains(bookTabPage))
                {
                    bookTabControl.SelectedTab = bookTabPage;
                }
            }
            else
            {
                MessageBox.Show("Не удалось создать книгу. Проверьте введенные данные и консоль ошибок.", "Ошибка создания", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (!string.IsNullOrEmpty(finalCoverRelativePath))
                { // Cleanup copied cover if creation failed
                    string absPathToDelete = GetAbsolutePath(finalCoverRelativePath);
                    if (File.Exists(absPathToDelete)) try { File.Delete(absPathToDelete); } catch { }
                }
            }
        }
    }
}