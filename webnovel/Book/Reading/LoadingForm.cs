using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using bookservice.FB2Logic; // Required for BookDocument

namespace bookservice
{
    public partial class LoadingForm : Form
    {
        private Label statusLabel;
        private ProgressBar progressBar;

        private User currentUser;
        private int bookId;
        private string bookFilePath;
        private string bookTitle; // To pass to ReadingForm for display
        private ReadingSQL readingSql;

        public LoadingForm(User user, int currentBookId, string filePath, string title)
        {
            InitializeComponent();
            this.currentUser = user;
            this.bookId = currentBookId;
            this.bookFilePath = filePath;
            this.bookTitle = title;
            this.readingSql = new ReadingSQL(this.currentUser);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 111);
            this.Name = "LoadingForm";
            this.Text = "Загрузка";
            SetupUI();
            this.Load += LoadingForm_Load;
        }

        private void SetupUI()
        {
            this.Text = "Загрузка книги...";
            this.Size = new System.Drawing.Size(400, 150);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ControlBox = false; // Disable closing while loading

            statusLabel = new Label
            {
                Text = "Пожалуйста, подождите, книга загружается и обрабатывается...",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(this.ClientSize.Width - 40, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Verdana", 10F),
            };
            this.Controls.Add(statusLabel);

            progressBar = new ProgressBar
            {
                Location = new System.Drawing.Point(20, statusLabel.Bottom + 10),
                Size = new System.Drawing.Size(this.ClientSize.Width - 40, 25),
                Style = ProgressBarStyle.Marquee, // Indeterminate progress
                MarqueeAnimationSpeed = 30
            };
            this.Controls.Add(progressBar);
        }

        private async void LoadingForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bookFilePath) || !File.Exists(bookFilePath))
            {
                MessageBox.Show($"Файл книги не найден по пути: {bookFilePath}", "Ошибка файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Abort;
                this.Close();
                return;
            }

            BookDocument bookDocument = new BookDocument();
            bool success = false;

            try
            {
                // Perform heavy loading in a background task
                success = await Task.Run(() => bookDocument.LoadAndProcessFile(bookFilePath, new Size(860, 580))); // Default reader panel size

                if (success && bookDocument.Chapters.Any() && bookDocument.TotalPagesInBook > 0)
                {
                    ReadingProgress progress = readingSql.GetReadingProgress(currentUser.Id, bookId);
                    int initialChapterIndex = 0;
                    int initialPageIndex = 0;

                    if (progress.Exists && !progress.IsFinished) // Only use saved progress if book is not marked finished
                    {
                        // Convert absolute page from DB to chapter/page index
                        // Note: BookDocument.GetChapterAndPageIndices expects absolute page number (1-based)
                        // progress.ChapterNumber and progress.PageNumber from DB *might* be 0/1 based or 1/1 based depending on how they are saved.
                        // Assuming they are stored as direct chapter/page indices (0-based for chapter, 0-based for page in chapter)
                        // Or, if they are 1-based absolute page, then:
                        // var indices = bookDocument.GetChapterAndPageIndices(progress.AbsolutePageNumber);
                        // For now, let's assume ReadingProgress stores 1-based chapter and 1-based page *within that chapter*.
                        // We need to convert this to 0-based indices for BookDocument.

                        // Find the 0-based chapter index
                        if (progress.ChapterNumber > 0 && progress.ChapterNumber <= bookDocument.Chapters.Count)
                        {
                            initialChapterIndex = progress.ChapterNumber - 1;
                        }
                        // Find the 0-based page index within that chapter
                        if (progress.PageNumber > 0 &&
                            initialChapterIndex < bookDocument.Chapters.Count && // ensure chapter index is valid
                            bookDocument.Chapters[initialChapterIndex].PagesRtf != null &&
                            progress.PageNumber <= bookDocument.Chapters[initialChapterIndex].PagesRtf.Count)
                        {
                            initialPageIndex = progress.PageNumber - 1;
                        }
                        else if (initialChapterIndex < bookDocument.Chapters.Count && bookDocument.Chapters[initialChapterIndex].PagesRtf != null)
                        {
                            // If page number is invalid, default to first page of the chapter
                            initialPageIndex = 0;
                        }
                        // If chapter itself was invalid and defaulted to 0, page will also be 0.

                    }


                    ReadingForm readerForm = new ReadingForm(bookDocument, Path.GetFileName(bookFilePath), currentUser, bookId, initialChapterIndex, initialPageIndex);
                    this.DialogResult = DialogResult.OK;
                    this.Hide(); // Hide this form
                    readerForm.ShowDialog(this.Owner); // Show reader form modally to the original caller (BookForm)
                    this.Close(); // Close loading form after reader is closed
                }
                else
                {
                    statusLabel.Text = "Не удалось загрузить или обработать книгу.\nВозможно, файл поврежден, пуст или не содержит текста.";
                    MessageBox.Show(statusLabel.Text, "Ошибка загрузки книги", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка при загрузке книги: {ex.ToString()}");
                statusLabel.Text = $"Ошибка при загрузке: {ex.Message}";
                MessageBox.Show($"Произошла критическая ошибка при загрузке книги: {ex.Message}", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
            finally
            {
                if (readingSql != null) readingSql.Dispose();
            }
        }
    }
}