﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace bookservice {
    // Note: Many UI element names like 'novelTabPage', 'createNovelTabPage', 'downloadNovelLabelButton'
    // are defined in the .Designer.cs file and cannot be changed here.
    // I will adapt the logic assuming these names remain, but their conceptual meaning changes to "Book".
    // String literals and comments will be updated.
    public partial class BookForm : Form {
        private string selectedBookIDString; // Renamed
        private int currentBookId; // Renamed
        private CatalogForm catalogForm;
        private User user;
        private BookSQL bookSql; // Renamed
        private BookDetails currentBookDetails; // Renamed
        private List<BookFile> currentBookFiles; // Renamed
        private const string CommentPlaceholder = "Написать комментарий...";
        private const int coverWidth = 432;
        private const int coverHeight = 609;

        // Assuming a new TextBox for Writer's Name in create mode
        // private TextBox createWriterNameTextBox; 

        public BookForm() {
            InitializeComponent();
            this.BackColor = SystemColors.ControlLightLight;
            this.Font = new Font("Verdana", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;

            // If createWriterNameTextBox is added via Designer, it would be initialized there.
            // If added programmatically, initialize it here or in Load.
            // Example:
            // createWriterNameTextBox = new TextBox { Name = "createWriterNameTextBox", Width = 200, Top = createTitleTextBox.Bottom + 5, Left = createTitlePromptLabel.Left };
            // createNovelTabPage.Controls.Add(new Label { Text = "Имя автора:", AutoSize = true, Top = createWriterNameTextBox.Top + 3, Left = createTitlePromptLabel.Left - 90 });
            // createNovelTabPage.Controls.Add(createWriterNameTextBox);
        }
        // Event handler name might be NovelForm_Load in Designer, can't change it here.
        private void NovelForm_Load(object sender, EventArgs e) {
            catalogForm = (CatalogForm)Owner;
            if (catalogForm == null) {
                MessageBox.Show("Не удалось получить ссылку на родительскую форму каталога. Форма будет закрыта.", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            user = catalogForm.User;
            selectedBookIDString = catalogForm.GetSelectedBookID; // Renamed
            bookSql = new BookSQL(user); // Renamed

            if (string.IsNullOrEmpty(selectedBookIDString) || selectedBookIDString == "0") {
                currentBookId = 0; // Renamed
                Text = "Создание новой книги"; // Updated text
                ConfigureTabsForCreateMode();
                LoadDataForCreateBookTab(); // Renamed
            } else {
                if (!int.TryParse(selectedBookIDString, out currentBookId)) { // Renamed
                    MessageBox.Show("Некорректный ID книги. Форма будет закрыта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Updated text
                    this.Close();
                    return;
                }
                Text = $"Книга: Загрузка..."; // Updated text
                LoadBookData(); // Renamed
                ConfigureTabsForViewEditMode();
            }
            ControlsLayout();
        }
        private void ConfigureTabsForCreateMode() {
            EnsureAllTabsPresent();
            // Assuming novelTabPage, commentsTabPage, editNovelTabPage, createNovelTabPage are existing UI names
            if (bookTabControl.TabPages.Contains(bookTabPage))
                bookTabControl.TabPages.Remove(bookTabPage);
            if (bookTabControl.TabPages.Contains(commentsTabPage))
                bookTabControl.TabPages.Remove(commentsTabPage);
            if (bookTabControl.TabPages.Contains(editBookTabPage))
                bookTabControl.TabPages.Remove(editBookTabPage);
            if (!bookTabControl.TabPages.Contains(createBookTabPage)) {
                bookTabControl.TabPages.Add(createBookTabPage);
            }
            bookTabControl.SelectedTab = createBookTabPage;
        }
        private void ConfigureTabsForViewEditMode() {
            EnsureAllTabsPresent();
            bookTabControl.TabPages.Clear();
            bookTabControl.TabPages.Add(bookTabPage); // This tab now shows book details

            if (user.Role == UserRole.READER || user.Role == UserRole.ADMIN) { // Updated roles
                if (!bookTabControl.TabPages.Contains(commentsTabPage))
                    bookTabControl.TabPages.Add(commentsTabPage);
                LoadComments();
            }
            // Edit tab visible if user is ADMIN and is the admin who added this book.
            if (user.Role == UserRole.ADMIN && currentBookDetails != null && user.Id == currentBookDetails.AdminId) { // Updated role and property
                if (!bookTabControl.TabPages.Contains(editBookTabPage)) // editNovelTabPage might be for editing book details
                    bookTabControl.TabPages.Add(editBookTabPage);
            }
            if (bookTabControl.TabPages.Contains(createBookTabPage))
                bookTabControl.TabPages.Remove(createBookTabPage);

            if (bookTabControl.TabPages.Count > 0) {
                bookTabControl.SelectedTab = bookTabControl.TabPages[0];
            }
        }
        private void EnsureAllTabsPresent() {
            // These are likely defined in the Designer.cs
            if (bookTabPage.Parent == null) bookTabControl.TabPages.Add(bookTabPage);
            if (commentsTabPage.Parent == null) bookTabControl.TabPages.Add(commentsTabPage);
            if (editBookTabPage.Parent == null) bookTabControl.TabPages.Add(editBookTabPage);
            if (createBookTabPage.Parent == null) bookTabControl.TabPages.Add(createBookTabPage);
        }
        private void LoadBookData() { // Renamed from LoadNovelData
            if (currentBookId == 0) return; // Renamed
            currentBookDetails = bookSql.GetBookDetails(currentBookId); // Renamed

            if (currentBookDetails == null) {
                MessageBox.Show("Не удалось загрузить информацию о книге.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Updated text
                this.Text = "Книга: Ошибка загрузки"; // Updated text
                return;
            }
            this.Text = $"Книга: {currentBookDetails.Title}"; // Updated text
            titleLabel.Text = currentBookDetails.Title;
            // Display actual writer's name and the admin who added it
            authorLabel.Text = $"👤 Автор: {currentBookDetails.WriterName} (Добавил: {currentBookDetails.AdminLogin})";
            yearLabel.Text = currentBookDetails.PublicationYear.HasValue ? $"📅 Год: {currentBookDetails.PublicationYear.Value}" : "📅 Год: Н/Д";
            ageRatingLabel.Text = currentBookDetails.AgeRating.HasValue ? $"🚫 {currentBookDetails.AgeRating.Value}+" : "🚫 Н/Д";
            descriptionLabel.Text = currentBookDetails.Description ?? "Описание отсутствует.";

            if (coverPictureBox.Image != null) {
                coverPictureBox.Image.Dispose();
                coverPictureBox.Image = null;
            }
            if (!string.IsNullOrEmpty(currentBookDetails.CoverPath) && File.Exists(GetAbsolutePath(currentBookDetails.CoverPath))) {
                try {
                    using (FileStream stream = new FileStream(GetAbsolutePath(currentBookDetails.CoverPath), FileMode.Open, FileAccess.Read)) {
                        coverPictureBox.Image = Image.FromStream(stream);
                    }
                } catch (Exception ex) {
                    Console.WriteLine("Ошибка загрузки обложки: " + ex.Message);
                    coverPictureBox.Image = CreatePlaceholderImage(coverWidth, coverHeight);
                }
            } else {
                coverPictureBox.Image = CreatePlaceholderImage(coverWidth, coverHeight);
            }
            LoadBookGenres(); // Renamed
            currentBookFiles = bookSql.GetBookFiles(currentBookId); // Renamed
            downloadNovelLabelButton.Enabled = currentBookFiles != null && currentBookFiles.Count > 0; // downloadNovelLabelButton is UI name
            ControlsLayout();
        }
        private void LoadBookGenres() { // Renamed from LoadNovelGenres
            genresFlowLayoutPanel.Controls.Clear();
            if (currentBookId == 0 && currentBookDetails == null) return; // Renamed
            List<string> genres = bookSql.GetBookGenres(currentBookDetails.Id); // Renamed
            if (genres != null && genres.Count > 0) {
                foreach (string genreName in genres) {
                    Label genreLabel = new Label {
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
        private void LoadComments() {
            if (currentBookId == 0) return; // Renamed
            commentsHostPanel.Controls.Clear();
            List<BookComment> comments = bookSql.GetBookComments(currentBookId); // Renamed
            if (comments != null && comments.Count > 0) {
                int yPos = 5;
                foreach (var commentData in comments) {
                    Panel commentPanel = new Panel {
                        Width = commentsHostPanel.ClientSize.Width - 100,
                        AutoSize = true,
                        Margin = new Padding(5, 0, 5, 10)
                    };
                    Label userLoginLabel = new Label {
                        Text = $"{commentData.UserLogin} ({commentData.CommentDateTime:dd.MM.yyyy HH:mm}):",
                        Font = new Font("Verdana", 14, FontStyle.Bold),
                        AutoSize = true,
                        Location = new Point(0, 0),
                        ForeColor = Color.DarkSlateBlue
                    };
                    Label commentTextLabel = new Label {
                        Text = commentData.Text,
                        Font = new Font("Verdana", 14, FontStyle.Regular),
                        AutoSize = false,
                        Width = commentPanel.Width - 10,
                        Location = new Point(0, userLoginLabel.Bottom + 5),
                        MaximumSize = new Size(commentPanel.Width - 10, 0),
                        MinimumSize = new Size(commentPanel.Width - 10, 0),
                        AutoEllipsis = false
                    };
                    using (Graphics g = CreateGraphics()) {
                         SizeF size = g.MeasureString(commentTextLabel.Text, commentTextLabel.Font, commentTextLabel.Width);
                         commentTextLabel.Height = (int)Math.Ceiling(size.Height);
                    }
                    commentPanel.Controls.Add(userLoginLabel);
                    commentPanel.Controls.Add(commentTextLabel);
                    commentPanel.Height = commentTextLabel.Bottom + 5;
                    commentPanel.Location = new Point(5, yPos);
                    commentsHostPanel.Controls.Add(commentPanel);
                    yPos += commentPanel.Height + 10;
                }
            } else {
                Label noCommentsLabel = new Label {
                    Text = "Комментариев пока нет.",
                    Font = new Font("Verdana", 14F, FontStyle.Italic),
                    AutoSize = true,
                    Location = new Point(0, 10)
                };
                commentsHostPanel.Controls.Add(noCommentsLabel);
            }
            commentsHostPanel.PerformLayout();
        }
        private string GetAbsolutePath(string relativePath) {
            if (string.IsNullOrEmpty(relativePath) || Path.IsPathRooted(relativePath)) // Added null/empty check
                return relativePath;
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(Path.Combine(basePath, relativePath));
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
        private void ControlsLayout() {
            // This method highly depends on specific UI element names from Designer.cs.
            // Assuming 'novelTabPage' means the main book display tab.
            if (bookTabPage.Parent == null) return;
            int currentX = titleLabel.Left;
            int currentY = titleLabel.Bottom + 20;
            authorLabel.Location = new Point(currentX, currentY);
            genresFlowLayoutPanel.Location = new Point(authorLabel.Left, authorLabel.Bottom + 10);
            genresFlowLayoutPanel.MaximumSize = new Size(bookTabPage.ClientSize.Width - genresFlowLayoutPanel.Left - 20, 0);
            genresFlowLayoutPanel.PerformLayout();
            yearLabel.Location = new Point(authorLabel.Right + 10, currentY); // Adjust based on new authorLabel content
            ageRatingLabel.Location = new Point(yearLabel.Right + 10, currentY);
            int infoBlockBottom = Math.Max(authorLabel.Bottom, genresFlowLayoutPanel.Bottom); // Ensure description is below both
            descriptionLabel.Location = new Point(titleLabel.Left, infoBlockBottom + 20);
            descriptionLabel.Size = new Size(bookTabPage.ClientSize.Width - titleLabel.Left - 200,
                                            bookTabPage.ClientSize.Height - descriptionLabel.Top - 100);
            if (commentsTabPage.Parent != null) {
                newCommentTextBox.Width = commentsTabPage.ClientSize.Width - 200;
                newCommentTextBox.Top = commentsTitleLabel.Bottom + 20;
                newCommentTextBox.Left = commentsTitleLabel.Left;
                postCommentButton.Location = new Point(newCommentTextBox.Left, newCommentTextBox.Bottom + 10);
                commentsHostPanel.Location = new Point(newCommentTextBox.Left, postCommentButton.Bottom + 40);
                commentsHostPanel.Size = new Size(newCommentTextBox.Width, commentsTabPage.ClientSize.Height - commentsHostPanel.Top - 60);
            }
            if (createBookTabPage.Parent != null) { // This is the "Create Book" tab
                int createControlsWidth = createBookTabPage.ClientSize.Width - 40;
                if (createControlsWidth < 300)
                    createControlsWidth = 300;
                createTitleTextBox.Width = createControlsWidth - createTitlePromptLabel.Width - 15;
                // Position createWriterNameTextBox if added
                // createWriterNameTextBox.Width = createTitleTextBox.Width;
                // createWriterNameTextBox.Top = createTitleTextBox.Bottom + 10; // Example
                // createWriterNameTextBox.Left = createTitleTextBox.Left;
                createYearTextBox.Width = 100;
                createAgeRatingListBox.Width = 100;
                createDescriptionTextBox.Width = createControlsWidth;
                createSelectCoverButton.Left = createCoverPromptLabel.Right + 10;
                createCoverPathLabel.Left = createSelectCoverButton.Right + 10;
                createCoverPathLabel.MaximumSize = new Size(createControlsWidth - createCoverPathLabel.Left, 0);
                createGenresCheckedListBox.Width = createControlsWidth;
                createBookButton.Left = (createBookTabPage.ClientSize.Width - createBookButton.Width) / 2; // createNovelButton is UI name
                if (createBookButton.Bottom > createBookTabPage.ClientSize.Height - 20) {
                    createBookButton.Top = createBookTabPage.ClientSize.Height - 20 - createBookButton.Height;
                }
            }
        }
        private void NovelForm_FormClosed(object sender, FormClosedEventArgs e) { // UI name
            if (bookSql != null) // Renamed
                bookSql.Dispose();
            if (coverPictureBox.Image != null) {
                coverPictureBox.Image.Dispose();
                coverPictureBox.Image = null;
            }
            if (catalogForm != null && !catalogForm.IsDisposed)
                catalogForm.Show();
        }
        // downloadNovelLabelButton is a UI element name
        private void downloadNovelLabelButton_Click(object sender, EventArgs e) {
            if (currentBookFiles == null || currentBookFiles.Count == 0) { // Renamed
                MessageBox.Show("Для этой книги нет доступных файлов.", "Файлы не найдены", MessageBoxButtons.OK, MessageBoxIcon.Information); // Updated text
                return;
            }
            novelFilesContextMenuStrip.Items.Clear(); // UI name
            foreach (var bookFile in currentBookFiles) { // Renamed
                ToolStripMenuItem menuItem = new ToolStripMenuItem(bookFile.FileName + ".fb2"); // Changed to .fb2
                menuItem.Tag = bookFile;
                menuItem.Click += BookFileMenuItem_Click; // Renamed
                novelFilesContextMenuStrip.Items.Add(menuItem); // UI name
            }
            Control control = (Control)sender;
            novelFilesContextMenuStrip.Show(control, new Point(0, control.Height)); // UI name
        }
        private void BookFileMenuItem_Click(object sender, EventArgs e) { // Renamed from NovelFileMenuItem_Click
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem != null && menuItem.Tag is BookFile bookFile) { // Renamed
                string sourceFilePath = GetAbsolutePath(bookFile.FilePath);
                if (!File.Exists(sourceFilePath)) {
                    MessageBox.Show($"Файл '{bookFile.FileName}.fb2' не найден по пути: {sourceFilePath}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // .fb2
                    return;
                }
                saveNovelFileDialog.FileName = bookFile.FileName + ".fb2"; // UI name, changed to .fb2
                saveNovelFileDialog.Filter = "FB2 файл (*.fb2)|*.fb2"; // UI name, changed to .fb2
                saveNovelFileDialog.Title = "Сохранить файл книги"; // UI name, updated text
                if (saveNovelFileDialog.ShowDialog() == DialogResult.OK) { // UI name
                    try {
                        File.Copy(sourceFilePath, saveNovelFileDialog.FileName, true); // UI name
                        MessageBox.Show($"Файл '{Path.GetFileName(saveNovelFileDialog.FileName)}' успешно сохранен.", "Сохранение успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } catch (Exception ex) {
                        MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void NewCommentTextBox_Enter(object sender, EventArgs e) {
            if (newCommentTextBox.Text == CommentPlaceholder) {
                newCommentTextBox.Text = "";
                newCommentTextBox.ForeColor = SystemColors.WindowText;
            }
        }
        private void NewCommentTextBox_Leave(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(newCommentTextBox.Text)) {
                newCommentTextBox.Text = CommentPlaceholder;
                newCommentTextBox.ForeColor = Color.Gray;
            }
        }
        private void NewCommentTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.Enter) {
                postCommentButton_Click(postCommentButton, EventArgs.Empty);
                e.SuppressKeyPress = true;
            }
        }
        private void postCommentButton_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(newCommentTextBox.Text) || newCommentTextBox.Text == CommentPlaceholder) {
                MessageBox.Show("Комментарий не может быть пустым.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (user.Id == 0) { // Guest user
                 MessageBox.Show("Гости не могут оставлять комментарии. Пожалуйста, войдите или зарегистрируйтесь.", "Действие запрещено", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                 return;
            }
            bool success = bookSql.AddComment(user.Id, currentBookId, newCommentTextBox.Text.Trim()); // Renamed
            if (success) {
                MessageBox.Show("Комментарий успешно добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                newCommentTextBox.Text = "";
                NewCommentTextBox_Leave(newCommentTextBox, EventArgs.Empty); // Reset placeholder
                LoadComments(); // Refresh comments
            } else {
                MessageBox.Show("Не удалось добавить комментарий. Попробуйте позже.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // addFileButton is UI name
        private void addFileButton_Click(object sender, EventArgs e) {
            if (currentBookId == 0) return; // Renamed
            openNovelFileDialog.Filter = "FB2 файлы (*.fb2)|*.fb2"; // UI name, changed to .fb2
            openNovelFileDialog.Title = "Выберите файл книги (.fb2)"; // UI name, updated text
            if (openNovelFileDialog.ShowDialog() == DialogResult.OK) { // UI name
                string sourceFilePath = openNovelFileDialog.FileName; // UI name
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFilePath);
                string fileExtension = Path.GetExtension(sourceFilePath);
                if (fileExtension.ToLower() != ".fb2") { // Changed to .fb2
                     MessageBox.Show("Пожалуйста, выберите файл формата .fb2.", "Неверный формат файла", MessageBoxButtons.OK, MessageBoxIcon.Warning); // .fb2
                     return;
                }
                // Assuming 'files' directory for book files
                string relativeDestFolder = Path.Combine("..", "..", "..", "files"); 
                string absoluteDestFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeDestFolder));
                if (!Directory.Exists(absoluteDestFolder)) {
                    try {
                        Directory.CreateDirectory(absoluteDestFolder);
                    } catch (Exception ex) {
                         MessageBox.Show($"Не удалось создать директорию для файлов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         return;
                    }
                }
                string destinationFileName = Path.GetFileName(sourceFilePath);
                // Path stored in DB should be relative for portability
                string dbFilePath = Path.Combine("..\\..\\..\\files", destinationFileName).Replace(Path.DirectorySeparatorChar, '\\'); 
                string absoluteDestinationPath = Path.Combine(absoluteDestFolder, destinationFileName);

                DialogResult confirmResult = MessageBox.Show($"Добавить файл '{destinationFileName}' к книге '{currentBookDetails.Title}'?", // Updated text
                                                           "Подтверждение добавления файла",
                                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes) {
                    try {
                        File.Copy(sourceFilePath, absoluteDestinationPath, true);
                        bool success = bookSql.AddBookFile(currentBookId, destinationFileName, dbFilePath); // Renamed method and ID
                        if (success) {
                            MessageBox.Show("Файл успешно добавлен и запись в БД создана.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            currentBookFiles = bookSql.GetBookFiles(currentBookId); // Refresh file list
                            downloadNovelLabelButton.Enabled = currentBookFiles != null && currentBookFiles.Count > 0; // UI name
                        } else {
                            MessageBox.Show("Файл скопирован, но не удалось добавить запись в БД. Возможно, потребуется удалить файл вручную.", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if(File.Exists(absoluteDestinationPath)) // Attempt to clean up
                                File.Delete(absoluteDestinationPath);
                        }
                    } catch (Exception ex) {
                        MessageBox.Show($"Ошибка при добавлении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        // deleteNovelButton is UI name
        private void deleteBookButton_Click(object sender, EventArgs e) {
            if (currentBookId == 0 || currentBookDetails == null) return; // Renamed
            DialogResult confirmResult = MessageBox.Show($"Вы уверены, что хотите удалить книгу '{currentBookDetails.Title}'?\n" + // Updated text
                                                       "Это действие необратимо и удалит все связанные файлы, комментарии и жанры.",
                                                       "Подтверждение удаления",
                                                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes) {
                if (coverPictureBox.Image != null) { // Dispose image before trying to delete file if that's part of DeleteBook
                    coverPictureBox.Image.Dispose();
                    coverPictureBox.Image = null;
                }
                // Physical file deletion (cover, book files) should be handled carefully,
                // potentially within BookSQL.DeleteBook or here based on paths from currentBookDetails/currentBookFiles.
                // For now, focusing on DB record deletion via BookSQL.
                bool success = bookSql.DeleteBook(currentBookId); // Renamed
                if (success) {
                    MessageBox.Show("Книга успешно удалена.", "Удаление завершено", MessageBoxButtons.OK, MessageBoxIcon.Information); // Updated text
                    this.Close(); // Close the form as the book is gone
                } else {
                    // If deletion failed, try to reload data to reflect current state (e.g. if cover was disposed)
                    LoadBookData(); // Renamed
                    MessageBox.Show("Не удалось удалить книгу. Проверьте консоль на наличие ошибок.", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error); // Updated text
                }
            }
        }
        private string selectedCoverPathForCreation = null;
        // LoadDataForCreateNovelTab is UI name
        private void LoadDataForCreateBookTab() { // Renamed
            createGenresCheckedListBox.Items.Clear();
            List<KeyValuePair<int, string>> allGenres = bookSql.GetAllGenres();
            foreach (var genrePair in allGenres)
                createGenresCheckedListBox.Items.Add(genrePair, false);
            createGenresCheckedListBox.DisplayMember = "Value";
            createGenresCheckedListBox.ValueMember = "Key";

            createTitleTextBox.Clear();
            // Clear the new writer name TextBox
            // if (createWriterNameTextBox != null) createWriterNameTextBox.Clear();
            createYearTextBox.Clear();
            createAgeRatingListBox.ClearSelected();
            createDescriptionTextBox.Clear();
            createCoverPathLabel.Text = "Файл не выбран";
            selectedCoverPathForCreation = null;
        }
        // createSelectCoverButton is UI name
        private void createSelectCoverButton_Click(object sender, EventArgs e) {
            openCoverFileDialog.Filter = "Изображения (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp"; // UI name
            openCoverFileDialog.Title = "Выберите обложку для книги"; // UI name, updated text
            if (openCoverFileDialog.ShowDialog() == DialogResult.OK) { // UI name
                DialogResult confirmResult = MessageBox.Show($"Использовать изображение '{Path.GetFileName(openCoverFileDialog.FileName)}' в качестве обложки?",
                                                           "Подтверждение выбора обложки", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes) {
                    selectedCoverPathForCreation = openCoverFileDialog.FileName; // UI name
                    createCoverPathLabel.Text = Path.GetFileName(selectedCoverPathForCreation);
                }
            }
        }
        // createNovelButton is UI name, should conceptually be createBookButton
        private void createBookButton_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(createTitleTextBox.Text)) {
                MessageBox.Show("Название книги не может быть пустым.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                createTitleTextBox.Focus();
                return;
            }
            string writerName = "";
            if (createWriterNameTextBox != null) {
                writerName = createWriterNameTextBox.Text.Trim();
            }

            if (string.IsNullOrWhiteSpace(writerName)) {
                 MessageBox.Show("Имя автора не может быть пустым.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 // createWriterNameTextBox?.Focus(); // Focus if it exists
                 return;
            }

            short? publicationYear = null;
            if (!string.IsNullOrWhiteSpace(createYearTextBox.Text)) {
                if (short.TryParse(createYearTextBox.Text, out short yearValue) && yearValue > 0 && yearValue <= DateTime.Now.Year + 20) {
                    publicationYear = yearValue;
                } else {
                    MessageBox.Show("Год публикации указан некорректно. Оставьте поле пустым или введите корректный год (например, 2023).", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    createYearTextBox.Focus();
                    return;
                }
            }
            short? ageRating = null;
            if (createAgeRatingListBox.SelectedItem != null) {
                string ratingStr = createAgeRatingListBox.SelectedItem.ToString().Replace("+", "");
                if (short.TryParse(ratingStr, out short ratingValue)) {
                    ageRating = ratingValue;
                }
            }
            if (user.Id == 0) { // Should be an admin to create books
                 MessageBox.Show("Текущий пользователь не может создавать книги (ID пользователя не определен или нет прав).", "Ошибка прав", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
            }
             if (!user.IsAdmin) {
                 MessageBox.Show("Только администраторы могут создавать книги.", "Ошибка прав", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
            }

            string finalCoverRelativePath = null;
            if (!string.IsNullOrEmpty(selectedCoverPathForCreation)) {
                try {
                    string coverFileName = Path.GetFileName(selectedCoverPathForCreation);
                    string relativeDestFolder = Path.Combine("..", "..", "..", "covers"); // Assuming 'covers' directory
                    string absoluteDestFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeDestFolder));
                    if (!Directory.Exists(absoluteDestFolder))
                        Directory.CreateDirectory(absoluteDestFolder);
                    string uniqueCoverFileName = Guid.NewGuid().ToString() + Path.GetExtension(coverFileName);
                    string absoluteDestinationPath = Path.Combine(absoluteDestFolder, uniqueCoverFileName);
                    File.Copy(selectedCoverPathForCreation, absoluteDestinationPath, true);
                    finalCoverRelativePath = Path.Combine("..\\..\\..\\covers", uniqueCoverFileName).Replace(Path.DirectorySeparatorChar, '\\');
                } catch (Exception ex) {
                    MessageBox.Show($"Ошибка при сохранении обложки: {ex.Message}\nКнига будет создана без обложки.", "Ошибка обложки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    finalCoverRelativePath = null;
                }
            }
            List<int> selectedGenreIds = new List<int>();
            foreach (var item in createGenresCheckedListBox.CheckedItems) {
                if (item is KeyValuePair<int, string> genrePair)
                    selectedGenreIds.Add(genrePair.Key);
            }
            DialogResult confirmCreate = MessageBox.Show("Создать новую книгу с указанными данными?", "Подтверждение создания", MessageBoxButtons.YesNo, MessageBoxIcon.Question); // Updated text
            if (confirmCreate == DialogResult.No) {
                // If creation is cancelled and a cover was copied, delete the copied cover.
                if (!string.IsNullOrEmpty(finalCoverRelativePath)) {
                    string absPathToDelete = GetAbsolutePath(finalCoverRelativePath);
                    if(File.Exists(absPathToDelete))
                        File.Delete(absPathToDelete);
                }
                return;
            }
            int newBookId = bookSql.CreateBook( // Renamed
                createTitleTextBox.Text.Trim(),
                user.Id, // This is admin_id
                writerName, // New field: actual author's name
                publicationYear,
                ageRating,
                string.IsNullOrWhiteSpace(createDescriptionTextBox.Text) ? null : createDescriptionTextBox.Text.Trim(),
                finalCoverRelativePath,
                selectedGenreIds
            );
            if (newBookId > 0) {
                MessageBox.Show($"Книга '{createTitleTextBox.Text.Trim()}' успешно создана с ID: {newBookId}.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information); // Updated text
                selectedBookIDString = newBookId.ToString(); // Renamed
                currentBookId = newBookId; // Renamed
                LoadBookData(); // Renamed
                ConfigureTabsForViewEditMode();
                if (bookTabControl.TabPages.Contains(bookTabPage)) { // novelTabPage is the view tab
                    bookTabControl.SelectedTab = bookTabPage;
                    ControlsLayout(); // Re-layout after switching tab
                }
            } else {
                MessageBox.Show("Не удалось создать книгу. Проверьте введенные данные и консоль ошибок.", "Ошибка создания", MessageBoxButtons.OK, MessageBoxIcon.Error); // Updated text
                // If creation failed and a cover was copied, delete the copied cover.
                if (!string.IsNullOrEmpty(finalCoverRelativePath)) {
                    string absPathToDelete = GetAbsolutePath(finalCoverRelativePath);
                    if(File.Exists(absPathToDelete))
                        File.Delete(absPathToDelete);
                }
            }
        }
    }
}
