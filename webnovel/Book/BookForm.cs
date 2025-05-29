using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace bookservice {
    public partial class BookForm : Form {
        private string selectedNovelIDString;
        private int currentNovelId;
        private CatalogForm catalogForm;
        private User user;
        private BookSQL novelSql;
        private NovelDetails currentNovelDetails;
        private List<NovelFile> currentNovelFiles;
        private const string CommentPlaceholder = "Написать комментарий...";
        private const int coverWidth = 432;
        private const int coverHeight = 609;
        public BookForm() {
            InitializeComponent();
            this.BackColor = SystemColors.ControlLightLight;
            this.Font = new Font("Verdana", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;
        }
        private void NovelForm_Load(object sender, EventArgs e) {
            catalogForm = (CatalogForm)Owner;
            if (catalogForm == null) {
                MessageBox.Show("Не удалось получить ссылку на родительскую форму каталога. Форма будет закрыта.", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            user = catalogForm.User;
            selectedNovelIDString = catalogForm.GetSelectedNovelID;
            novelSql = new BookSQL(user);
            if (string.IsNullOrEmpty(selectedNovelIDString) || selectedNovelIDString == "0") {
                currentNovelId = 0;
                Text = "Создание нового веб-романа";
                ConfigureTabsForCreateMode();
                LoadDataForCreateNovelTab();
            } else {
                if (!int.TryParse(selectedNovelIDString, out currentNovelId)) {
                    MessageBox.Show("Некорректный ID веб-романа. Форма будет закрыта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                Text = $"Веб-роман: Загрузка...";
                LoadNovelData();
                ConfigureTabsForViewEditMode();
            }
            ControlsLayout();
        }
        private void ConfigureTabsForCreateMode() {
            EnsureAllTabsPresent();
            if (novelTabControl.TabPages.Contains(novelTabPage))
                novelTabControl.TabPages.Remove(novelTabPage);
            if (novelTabControl.TabPages.Contains(commentsTabPage))
                novelTabControl.TabPages.Remove(commentsTabPage);
            if (novelTabControl.TabPages.Contains(editNovelTabPage))
                novelTabControl.TabPages.Remove(editNovelTabPage);
            if (!novelTabControl.TabPages.Contains(createNovelTabPage)) {
                novelTabControl.TabPages.Add(createNovelTabPage);
            }
            novelTabControl.SelectedTab = createNovelTabPage;
        }
        private void ConfigureTabsForViewEditMode() {
            EnsureAllTabsPresent();
            novelTabControl.TabPages.Clear();
            novelTabControl.TabPages.Add(novelTabPage);
            if (user.Role == UserRole.WN_READER || user.Role == UserRole.WN_WRITER) {
                if (!novelTabControl.TabPages.Contains(commentsTabPage))
                    novelTabControl.TabPages.Add(commentsTabPage);
                LoadComments();
            }
            if (user.Role == UserRole.WN_WRITER && currentNovelDetails != null && user.Id == currentNovelDetails.WriterId) {
                if (!novelTabControl.TabPages.Contains(editNovelTabPage))
                    novelTabControl.TabPages.Add(editNovelTabPage);
            }
            if (novelTabControl.TabPages.Contains(createNovelTabPage))
                novelTabControl.TabPages.Remove(createNovelTabPage);
            if (novelTabControl.TabPages.Count > 0) {
                novelTabControl.SelectedTab = novelTabControl.TabPages[0];
            }
        }
        private void EnsureAllTabsPresent() {
            if (novelTabPage.Parent == null)
                novelTabControl.TabPages.Add(novelTabPage);
            if (commentsTabPage.Parent == null)
                novelTabControl.TabPages.Add(commentsTabPage);
            if (editNovelTabPage.Parent == null)
                novelTabControl.TabPages.Add(editNovelTabPage);
            if (createNovelTabPage.Parent == null)
                novelTabControl.TabPages.Add(createNovelTabPage);
        }
        private void LoadNovelData() {
            if (currentNovelId == 0) return;
            currentNovelDetails = novelSql.GetNovelDetails(currentNovelId);
            if (currentNovelDetails == null) {
                MessageBox.Show("Не удалось загрузить информацию о веб-романе.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Text = "Веб-роман: Ошибка загрузки";
                return;
            }
            this.Text = $"Веб-роман: {currentNovelDetails.Title}";
            titleLabel.Text = currentNovelDetails.Title;
            authorLabel.Text = $"👤 Автор: {currentNovelDetails.WriterLogin}";
            yearLabel.Text = currentNovelDetails.PublicationYear.HasValue ? $"📅 Год: {currentNovelDetails.PublicationYear.Value}" : "📅 Год: Н/Д";
            ageRatingLabel.Text = currentNovelDetails.AgeRating.HasValue ? $"🚫 {currentNovelDetails.AgeRating.Value}+" : "🚫 Н/Д";
            descriptionLabel.Text = currentNovelDetails.Description ?? "Описание отсутствует.";
            if (coverPictureBox.Image != null) {
                coverPictureBox.Image.Dispose();
                coverPictureBox.Image = null;
            }
            if (!string.IsNullOrEmpty(currentNovelDetails.CoverPath) && File.Exists(GetAbsolutePath(currentNovelDetails.CoverPath))) {
                try {
                    using (FileStream stream = new FileStream(GetAbsolutePath(currentNovelDetails.CoverPath), FileMode.Open, FileAccess.Read)) {
                        coverPictureBox.Image = Image.FromStream(stream);
                    }
                } catch (Exception ex) {
                    Console.WriteLine("Ошибка загрузки обложки: " + ex.Message);
                    coverPictureBox.Image = CreatePlaceholderImage(coverWidth, coverHeight);
                }
            } else {
                coverPictureBox.Image = CreatePlaceholderImage(coverWidth, coverHeight);
            }
            LoadNovelGenres();
            currentNovelFiles = novelSql.GetNovelFiles(currentNovelId);
            downloadNovelLabelButton.Enabled = currentNovelFiles != null && currentNovelFiles.Count > 0;
            ControlsLayout();
        }
        private void LoadNovelGenres() {
            genresFlowLayoutPanel.Controls.Clear();
            if (currentNovelId == 0 && currentNovelDetails == null) return;
            List<string> genres = novelSql.GetNovelGenres(currentNovelDetails.Id);
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
            if (currentNovelId == 0) return;
            commentsHostPanel.Controls.Clear();
            List<NovelComment> comments = novelSql.GetNovelComments(currentNovelId);
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
            if (Path.IsPathRooted(relativePath))
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
            if (novelTabPage.Parent == null) return;
            int currentX = titleLabel.Left;
            int currentY = titleLabel.Bottom + 20;
            authorLabel.Location = new Point(currentX, currentY);
            genresFlowLayoutPanel.Location = new Point(authorLabel.Left, authorLabel.Bottom + 10);
            genresFlowLayoutPanel.MaximumSize = new Size(novelTabPage.ClientSize.Width - genresFlowLayoutPanel.Left - 20, 0);
            genresFlowLayoutPanel.PerformLayout();
            yearLabel.Location = new Point(authorLabel.Right + 10, currentY);
            ageRatingLabel.Location = new Point(yearLabel.Right + 10, currentY);
            int infoBlockBottom = genresFlowLayoutPanel.Bottom;
            descriptionLabel.Location = new Point(titleLabel.Left, infoBlockBottom + 20);
            descriptionLabel.Size = new Size(novelTabPage.ClientSize.Width - titleLabel.Left - 200,
                                            novelTabPage.ClientSize.Height - descriptionLabel.Top - 100);
            if (commentsTabPage.Parent != null) {
                newCommentTextBox.Width = commentsTabPage.ClientSize.Width - 200;
                newCommentTextBox.Top = commentsTitleLabel.Bottom + 20;
                newCommentTextBox.Left = commentsTitleLabel.Left;
                postCommentButton.Location = new Point(newCommentTextBox.Left, newCommentTextBox.Bottom + 10);
                commentsHostPanel.Location = new Point(newCommentTextBox.Left, postCommentButton.Bottom + 40);
                commentsHostPanel.Size = new Size(newCommentTextBox.Width, commentsTabPage.ClientSize.Height - commentsHostPanel.Top - 60);
            }
            if (createNovelTabPage.Parent != null) {
                int createControlsWidth = createNovelTabPage.ClientSize.Width - 40;
                if (createControlsWidth < 300)
                    createControlsWidth = 300;
                createTitleTextBox.Width = createControlsWidth - createTitlePromptLabel.Width - 15;
                createYearTextBox.Width = 100;
                createAgeRatingListBox.Width = 100;
                createDescriptionTextBox.Width = createControlsWidth;
                createSelectCoverButton.Left = createCoverPromptLabel.Right + 10;
                createCoverPathLabel.Left = createSelectCoverButton.Right + 10;
                createCoverPathLabel.MaximumSize = new Size(createControlsWidth - createCoverPathLabel.Left, 0);
                createGenresCheckedListBox.Width = createControlsWidth;
                createNovelButton.Left = (createNovelTabPage.ClientSize.Width - createNovelButton.Width) / 2;
                if (createNovelButton.Bottom > createNovelTabPage.ClientSize.Height - 20) {
                    createNovelButton.Top = createNovelTabPage.ClientSize.Height - 20 - createNovelButton.Height;
                }
            }
        }
        private void NovelForm_FormClosed(object sender, FormClosedEventArgs e) {
            if (novelSql != null)
                novelSql.Dispose();
            if (coverPictureBox.Image != null) {
                coverPictureBox.Image.Dispose();
                coverPictureBox.Image = null;
            }
            if (catalogForm != null && !catalogForm.IsDisposed)
                catalogForm.Show();
        }
        private void downloadNovelLabelButton_Click(object sender, EventArgs e) {
            if (currentNovelFiles == null || currentNovelFiles.Count == 0) {
                MessageBox.Show("Для этого веб-романа нет доступных файлов.", "Файлы не найдены", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            novelFilesContextMenuStrip.Items.Clear();
            foreach (var novelFile in currentNovelFiles) {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(novelFile.FileName + ".epub");
                menuItem.Tag = novelFile;
                menuItem.Click += NovelFileMenuItem_Click;
                novelFilesContextMenuStrip.Items.Add(menuItem);
            }
            Control control = (Control)sender;
            novelFilesContextMenuStrip.Show(control, new Point(0, control.Height));
        }
        private void NovelFileMenuItem_Click(object sender, EventArgs e) {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem != null && menuItem.Tag is NovelFile novelFile) {
                string sourceFilePath = GetAbsolutePath(novelFile.FilePath);
                if (!File.Exists(sourceFilePath)) {
                    MessageBox.Show($"Файл '{novelFile.FileName}.epub' не найден по пути: {sourceFilePath}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                saveNovelFileDialog.FileName = novelFile.FileName + ".epub";
                saveNovelFileDialog.Filter = "EPUB файл (*.epub)|*.epub";
                saveNovelFileDialog.Title = "Сохранить файл веб-романа";
                if (saveNovelFileDialog.ShowDialog() == DialogResult.OK) {
                    try {
                        File.Copy(sourceFilePath, saveNovelFileDialog.FileName, true);
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
            if (user.Id == 0) {
                 MessageBox.Show("Гости не могут оставлять комментарии. Пожалуйста, войдите или зарегистрируйтесь.", "Действие запрещено", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 return;
            }
            bool success = novelSql.AddComment(user.Id, currentNovelId, newCommentTextBox.Text.Trim());
            if (success) {
                MessageBox.Show("Комментарий успешно добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                newCommentTextBox.Text = "";
                NewCommentTextBox_Leave(newCommentTextBox, EventArgs.Empty);
                LoadComments();
            } else {
                MessageBox.Show("Не удалось добавить комментарий. Попробуйте позже.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void addFileButton_Click(object sender, EventArgs e) {
            if (currentNovelId == 0) return;
            openNovelFileDialog.Filter = "EPUB файлы (*.epub)|*.epub";
            openNovelFileDialog.Title = "Выберите файл веб-романа (.epub)";
            if (openNovelFileDialog.ShowDialog() == DialogResult.OK) {
                string sourceFilePath = openNovelFileDialog.FileName;
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFilePath);
                string fileExtension = Path.GetExtension(sourceFilePath);
                if (fileExtension.ToLower() != ".epub") {
                     MessageBox.Show("Пожалуйста, выберите файл формата .epub.", "Неверный формат файла", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                }
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
                string relativeFilePathInProject = Path.Combine(relativeDestFolder, destinationFileName);
                string absoluteDestinationPath = Path.Combine(absoluteDestFolder, destinationFileName);
                DialogResult confirmResult = MessageBox.Show($"Добавить файл '{destinationFileName}' к веб-роману '{currentNovelDetails.Title}'?",
                                                           "Подтверждение добавления файла",
                                                           MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes) {
                    try {
                        File.Copy(sourceFilePath, absoluteDestinationPath, true);
                        string dbFilePath = Path.Combine("..\\..\\..\\files", destinationFileName).Replace(Path.DirectorySeparatorChar, '\\');
                        bool success = novelSql.AddNovelFile(currentNovelId, destinationFileName, dbFilePath);
                        if (success) {
                            MessageBox.Show("Файл успешно добавлен и запись в БД создана.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            currentNovelFiles = novelSql.GetNovelFiles(currentNovelId);
                            downloadNovelLabelButton.Enabled = currentNovelFiles != null && currentNovelFiles.Count > 0;
                        } else {
                            MessageBox.Show("Файл скопирован, но не удалось добавить запись в БД. Возможно, потребуется удалить файл вручную.", "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if(File.Exists(absoluteDestinationPath))
                                File.Delete(absoluteDestinationPath);
                        }
                    } catch (Exception ex) {
                        MessageBox.Show($"Ошибка при добавлении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void deleteNovelButton_Click(object sender, EventArgs e) {
            if (currentNovelId == 0 || currentNovelDetails == null) return;
            DialogResult confirmResult = MessageBox.Show($"Вы уверены, что хотите удалить веб-роман '{currentNovelDetails.Title}'?\n" +
                                                       "Это действие необратимо и удалит все связанные файлы, комментарии и жанры.",
                                                       "Подтверждение удаления",
                                                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes) {
                if (coverPictureBox.Image != null) {
                    coverPictureBox.Image.Dispose();
                    coverPictureBox.Image = null;
                }
                bool success = novelSql.DeleteNovel(currentNovelId);
                if (success) {
                    MessageBox.Show("Веб-роман успешно удален.", "Удаление завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                } else {
                    LoadNovelData();
                    MessageBox.Show("Не удалось удалить веб-роман. Проверьте консоль на наличие ошибок.", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private string selectedCoverPathForCreation = null;
        private void LoadDataForCreateNovelTab() {
            createGenresCheckedListBox.Items.Clear();
            List<KeyValuePair<int, string>> allGenres = novelSql.GetAllGenres();
            foreach (var genrePair in allGenres)
                createGenresCheckedListBox.Items.Add(genrePair, false);
            createGenresCheckedListBox.DisplayMember = "Value";
            createGenresCheckedListBox.ValueMember = "Key";
            createTitleTextBox.Clear();
            createYearTextBox.Clear();
            createAgeRatingListBox.ClearSelected();
            createDescriptionTextBox.Clear();
            createCoverPathLabel.Text = "Файл не выбран";
            selectedCoverPathForCreation = null;
        }
        private void createSelectCoverButton_Click(object sender, EventArgs e) {
            openCoverFileDialog.Filter = "Изображения (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openCoverFileDialog.Title = "Выберите обложку для веб-романа";
            if (openCoverFileDialog.ShowDialog() == DialogResult.OK) {
                DialogResult confirmResult = MessageBox.Show($"Использовать изображение '{Path.GetFileName(openCoverFileDialog.FileName)}' в качестве обложки?",
                                                           "Подтверждение выбора обложки", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes) {
                    selectedCoverPathForCreation = openCoverFileDialog.FileName;
                    createCoverPathLabel.Text = Path.GetFileName(selectedCoverPathForCreation);
                }
            }
        }
        private void createNovelButton_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(createTitleTextBox.Text)) {
                MessageBox.Show("Название книги не может быть пустым.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                createTitleTextBox.Focus();
                return;
            }
            short? publicationYear = null;
            if (!string.IsNullOrWhiteSpace(createYearTextBox.Text)) {
                if (short.TryParse(createYearTextBox.Text, out short yearValue) && yearValue > 0 && yearValue <= DateTime.Now.Year + 20) {
                    publicationYear = yearValue;
                } else {
                    MessageBox.Show("Год публикации указан некорректно. Оставьте поле пустым или введите корректный год (например, 2024).", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (user.Id == 0) {
                 MessageBox.Show("Текущий пользователь не может создавать книги (ID пользователя не определен).", "Ошибка прав", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return;
            }
            string finalCoverRelativePath = null;
            if (!string.IsNullOrEmpty(selectedCoverPathForCreation)) {
                try {
                    string coverFileName = Path.GetFileName(selectedCoverPathForCreation);
                    string relativeDestFolder = Path.Combine("..", "..", "..", "covers");
                    string absoluteDestFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeDestFolder));
                    if (!Directory.Exists(absoluteDestFolder))
                        Directory.CreateDirectory(absoluteDestFolder);
                    string uniqueCoverFileName = Guid.NewGuid().ToString() + Path.GetExtension(coverFileName);
                    string absoluteDestinationPath = Path.Combine(absoluteDestFolder, uniqueCoverFileName);
                    File.Copy(selectedCoverPathForCreation, absoluteDestinationPath, true);
                    finalCoverRelativePath = Path.Combine("..\\..\\..\\covers", uniqueCoverFileName).Replace(Path.DirectorySeparatorChar, '\\');
                } catch (Exception ex) {
                    MessageBox.Show($"Ошибка при сохранении обложки: {ex.Message}\nВеб-роман будет создан без обложки.", "Ошибка обложки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    finalCoverRelativePath = null;
                }
            }
            List<int> selectedGenreIds = new List<int>();
            foreach (var item in createGenresCheckedListBox.CheckedItems) {
                if (item is KeyValuePair<int, string> genrePair)
                    selectedGenreIds.Add(genrePair.Key);
            }
            DialogResult confirmCreate = MessageBox.Show("Создать новый веб-роман с указанными данными?", "Подтверждение создания", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmCreate == DialogResult.No) {
                if (!string.IsNullOrEmpty(finalCoverRelativePath)) {
                    string absPathToDelete = GetAbsolutePath(finalCoverRelativePath);
                    if(File.Exists(absPathToDelete))
                        File.Delete(absPathToDelete);
                }
                return;
            }
            int newNovelId = novelSql.CreateNovel(
                createTitleTextBox.Text.Trim(),
                user.Id,
                publicationYear,
                ageRating,
                string.IsNullOrWhiteSpace(createDescriptionTextBox.Text) ? null : createDescriptionTextBox.Text.Trim(),
                finalCoverRelativePath,
                selectedGenreIds
            );
            if (newNovelId > 0) {
                MessageBox.Show($"Веб-роман '{createTitleTextBox.Text.Trim()}' успешно создан с ID: {newNovelId}.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                selectedNovelIDString = newNovelId.ToString();
                currentNovelId = newNovelId;
                LoadNovelData();
                ConfigureTabsForViewEditMode();
                if (novelTabControl.TabPages.Contains(novelTabPage)) {
                    novelTabControl.SelectedTab = novelTabPage;
                    ControlsLayout();
                }
            } else {
                MessageBox.Show("Не удалось создать веб-роман. Проверьте введенные данные и консоль ошибок.", "Ошибка создания", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (!string.IsNullOrEmpty(finalCoverRelativePath)) {
                    string absPathToDelete = GetAbsolutePath(finalCoverRelativePath);
                    if(File.Exists(absPathToDelete))
                        File.Delete(absPathToDelete);
                }
            }
        }
    }
}