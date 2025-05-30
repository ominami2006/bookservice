using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using bookservice.FB2Logic; // Required for BookDocument, BookChapter

namespace bookservice
{
    public partial class ReadingForm : Form // Was Form3 from FB2Reader
    {
        private BookDocument _book;
        private RichTextBox richTextBoxDisplay;
        private Panel readerPanel;
        private Button nextButton, prevButton, tocButton;
        private TrackBar pageTrackBar;
        private Label pageInfoLabel;
        private Size _readerPageDimensions;

        private int _currentChapterIndex = 0;
        private int _currentPageInChapterIndex = 0; // 0-based index for PagesRtf list
        private readonly string _bookFileNameForDisplay;

        private User currentUser;
        private int currentBookId;
        private ReadingSQL readingSql;

        // Timer for debouncing trackbar scroll to avoid rapid DB updates if we were to save on scroll
        // private Timer trackBarScrollTimer;

        public ReadingForm(BookDocument book, string bookFileName, User user, int bookId, int initialChapterIndex, int initialPageIndex)
        {
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 711);
            this.Name = "ReadingForm";
            this.Text = "Читалка";
            _book = book;
            _bookFileNameForDisplay = bookFileName;
            this.currentUser = user;
            this.currentBookId = bookId;
            this.readingSql = new ReadingSQL(this.currentUser);

            _readerPageDimensions = new Size(860, 580); // Default size, can be adjusted

            // Apply initial position from loaded progress
            // Ensure indices are valid before assigning
            if (_book != null && _book.Chapters != null && _book.Chapters.Any())
            {
                if (initialChapterIndex >= 0 && initialChapterIndex < _book.Chapters.Count)
                {
                    _currentChapterIndex = initialChapterIndex;
                    BookChapter chapter = _book.Chapters[_currentChapterIndex];
                    if (chapter != null && chapter.PagesRtf != null &&
                        initialPageIndex >= 0 && initialPageIndex < chapter.PagesRtf.Count)
                    {
                        _currentPageInChapterIndex = initialPageIndex;
                    }
                    else
                    {
                        _currentPageInChapterIndex = 0; // Default to first page of the chapter
                    }
                }
                else
                {
                    _currentChapterIndex = 0; // Default to first chapter
                    _currentPageInChapterIndex = 0; // Default to first page
                }
            }


            SetupUI();
            LoadInitialPage(); // This will use the _currentChapterIndex and _currentPageInChapterIndex
            this.KeyPreview = true; // For keyboard navigation
            this.FormClosing += ReadingForm_FormClosing;

            // Initialize trackbar scroll timer (if saving on scroll was desired)
            // trackBarScrollTimer = new Timer { Interval = 1000 }; // 1 second delay
            // trackBarScrollTimer.Tick += TrackBarScrollTimer_Tick;
        }

        private void SetupUI()
        {
            this.Text = $"Читалка - {_bookFileNameForDisplay}";
            this.ClientSize = new Size(900, 750); // Adjusted for better fit
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = SystemColors.ControlLightLight;
            this.Font = new Font("Verdana", 10F);

            readerPanel = new Panel
            {
                Size = _readerPageDimensions,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.WhiteSmoke // Or Color.White
            };
            this.Controls.Add(readerPanel);

            richTextBoxDisplay = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                ScrollBars = RichTextBoxScrollBars.None, // Pagination handles scrolling
                Font = new Font("Segoe UI", 14F), // Default display font, RTF can override
                WordWrap = true, // Should be true for display, pagination relies on it too
                BackColor = readerPanel.BackColor,
                Margin = new Padding(5) // Internal margin for text
            };
            readerPanel.Controls.Add(richTextBoxDisplay);

            int buttonHeight = 35;
            int buttonWidth = 80;
            int tocButtonWidth = 140;

            prevButton = new Button { Text = "< Назад", Size = new Size(buttonWidth + 20, buttonHeight), Font = new Font("Verdana", 10F, FontStyle.Bold) };
            prevButton.Click += PrevButton_Click;
            this.Controls.Add(prevButton);

            tocButton = new Button { Text = "Оглавление", Size = new Size(tocButtonWidth, buttonHeight), Font = new Font("Verdana", 10F) };
            tocButton.Click += TocButton_Click;
            this.Controls.Add(tocButton);

            nextButton = new Button { Text = "Вперёд >", Size = new Size(buttonWidth + 20, buttonHeight), Font = new Font("Verdana", 10F, FontStyle.Bold) };
            nextButton.Click += NextButton_Click;
            this.Controls.Add(nextButton);

            pageInfoLabel = new Label { Text = "1 / 1", AutoSize = false, TextAlign = ContentAlignment.MiddleRight, Size = new Size(150, 30), Font = new Font("Verdana", 10F) };
            this.Controls.Add(pageInfoLabel);

            pageTrackBar = new TrackBar
            {
                Minimum = 1,
                Maximum = Math.Max(1, _book.TotalPagesInBook), // Ensure max is at least 1
                Value = 1,
                TickFrequency = Math.Max(1, _book.TotalPagesInBook / 20), // Dynamic tick frequency
                AutoSize = false,
                Height = 45
            };
            pageTrackBar.Scroll += PageTrackBar_Scroll;
            this.Controls.Add(pageTrackBar);

            this.Resize += Form3_Resize; // Form3_Resize is the original name, kept for consistency with snippet
            Form3_Resize(this, EventArgs.Empty); // Initial layout
        }

        private void Form3_Resize(object sender, EventArgs e) // Renamed from ReadingForm_Resize for consistency
        {
            // Center readerPanel
            readerPanel.Location = new Point((this.ClientSize.Width - readerPanel.Width) / 2, 30);

            int controlsY = readerPanel.Bottom + 25;
            int availableWidthForControls = readerPanel.Width;
            int controlSpacing = 15;

            prevButton.Location = new Point(readerPanel.Left, controlsY);

            // Center TOC button between prev and next
            tocButton.Location = new Point(
                readerPanel.Left + prevButton.Width + (availableWidthForControls - prevButton.Width - nextButton.Width - tocButton.Width - 2 * controlSpacing) / 2,
                controlsY);

            nextButton.Location = new Point(readerPanel.Right - nextButton.Width, controlsY);


            pageTrackBar.Location = new Point(readerPanel.Left, controlsY + prevButton.Height + 20);
            pageTrackBar.Width = Math.Max(200, readerPanel.Width - pageInfoLabel.Width - 15); // Ensure minimum width

            pageInfoLabel.Location = new Point(pageTrackBar.Right + 10, pageTrackBar.Top + (pageTrackBar.Height - pageInfoLabel.Height) / 2);
        }

        private void LoadInitialPage()
        {
            if (_book == null || _book.TotalPagesInBook == 0)
            {
                richTextBoxDisplay.Text = "Не удалось загрузить страницы книги. Книга может быть пуста или повреждена.";
                pageInfoLabel.Text = "0 / 0";
                pageTrackBar.Enabled = false;
                prevButton.Enabled = false;
                nextButton.Enabled = false;
                tocButton.Enabled = false;
                return;
            }
            // _currentChapterIndex and _currentPageInChapterIndex should be set by constructor or navigation
            DisplayCurrentPage();
        }

        private void DisplayCurrentPage()
        {
            if (_book == null || _book.Chapters == null || !_book.Chapters.Any() || _book.TotalPagesInBook == 0)
            {
                richTextBoxDisplay.Text = "Книга не загружена или пуста.";
                UpdateNavigationState(); // Still update to disable controls etc.
                return;
            }

            // Validate current indices
            if (_currentChapterIndex < 0 || _currentChapterIndex >= _book.Chapters.Count) _currentChapterIndex = 0;

            BookChapter chapter = _book.Chapters[_currentChapterIndex];
            if (chapter == null || chapter.PagesRtf == null || !chapter.PagesRtf.Any())
            {
                richTextBoxDisplay.Text = $"Содержимое главы \"{(chapter?.Title ?? "Неизвестная глава")}\" отсутствует или не удалось загрузить.";
                UpdateNavigationState();
                return;
            }
            if (_currentPageInChapterIndex < 0 || _currentPageInChapterIndex >= chapter.PagesRtf.Count) _currentPageInChapterIndex = 0;

            try
            {
                richTextBoxDisplay.Rtf = chapter.PagesRtf[_currentPageInChapterIndex];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отображения RTF: {ex.Message}");
                richTextBoxDisplay.Text = $"Ошибка отображения страницы: {ex.Message}\n\nГлава: {chapter.Title}\nСтр. главы: {_currentPageInChapterIndex + 1}";
            }

            UpdateNavigationState();
        }

        private void UpdateNavigationState()
        {
            if (_book == null || _book.TotalPagesInBook == 0)
            {
                pageInfoLabel.Text = "0 / 0";
                if (pageTrackBar != null) pageTrackBar.Value = 1;
                if (pageTrackBar != null) pageTrackBar.Maximum = 1;
                if (pageTrackBar != null) pageTrackBar.Enabled = false;
                if (prevButton != null) prevButton.Enabled = false;
                if (nextButton != null) nextButton.Enabled = false;
                if (tocButton != null) tocButton.Enabled = false;
                this.Text = "Читалка - Ошибка";
                return;
            }

            int absolutePageNumber = _book.GetAbsolutePageNumber(_currentChapterIndex, _currentPageInChapterIndex);

            pageTrackBar.Maximum = Math.Max(1, _book.TotalPagesInBook);
            // Check if pageTrackBar value is within new max before setting
            if (absolutePageNumber <= pageTrackBar.Maximum)
            {
                pageTrackBar.Value = absolutePageNumber;
            }
            else
            {
                pageTrackBar.Value = pageTrackBar.Maximum; // or 1, if something went wrong
            }


            pageInfoLabel.Text = $"{absolutePageNumber} / {_book.TotalPagesInBook}";

            string chapterTitle = "Глава";
            int pagesInChapter = 0;
            if (_currentChapterIndex < _book.Chapters.Count && _book.Chapters[_currentChapterIndex] != null)
            {
                chapterTitle = _book.Chapters[_currentChapterIndex].Title ?? chapterTitle;
                if (_book.Chapters[_currentChapterIndex].PagesRtf != null)
                {
                    pagesInChapter = _book.Chapters[_currentChapterIndex].PagesRtf.Count;
                }
            }

            this.Text = $"Читалка - {chapterTitle} (Стр. {_currentPageInChapterIndex + 1} из {pagesInChapter}) - {_bookFileNameForDisplay}";

            prevButton.Enabled = absolutePageNumber > 1;
            nextButton.Enabled = absolutePageNumber < _book.TotalPagesInBook;
            tocButton.Enabled = _book.Chapters.Count > 0; // Enable if there are chapters
        }

        private void ReadingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (currentUser != null && currentUser.Id != 0 && _book != null && _book.TotalPagesInBook > 0)
            {
                // Save progress
                // Chapter number for DB should be 1-based
                // Page number in chapter for DB should be 1-based
                int chapterToSave = _currentChapterIndex + 1;
                int pageInChapterToSave = _currentPageInChapterIndex + 1;

                bool isFinished = (_book.GetAbsolutePageNumber(_currentChapterIndex, _currentPageInChapterIndex) == _book.TotalPagesInBook);

                readingSql.SaveReadingProgress(currentUser.Id, currentBookId, chapterToSave, pageInChapterToSave, isFinished);
            }
            if (readingSql != null) readingSql.Dispose();
            // if (trackBarScrollTimer != null) trackBarScrollTimer.Dispose();
        }

        private void PrevButton_Click(object sender, EventArgs e) => NavigatePrevious();
        private void NextButton_Click(object sender, EventArgs e) => NavigateNext();

        private void NavigatePrevious()
        {
            if (_book == null || _book.TotalPagesInBook == 0) return;
            int absolutePageNumber = _book.GetAbsolutePageNumber(_currentChapterIndex, _currentPageInChapterIndex);
            if (absolutePageNumber <= 1) return; // Already at the first page

            if (_currentPageInChapterIndex > 0)
            {
                _currentPageInChapterIndex--;
            }
            else if (_currentChapterIndex > 0)
            {
                _currentChapterIndex--;
                // Ensure the new chapter is valid and has pages
                if (_currentChapterIndex < _book.Chapters.Count &&
                    _book.Chapters[_currentChapterIndex] != null &&
                    _book.Chapters[_currentChapterIndex].PagesRtf != null &&
                    _book.Chapters[_currentChapterIndex].PagesRtf.Any())
                {
                    _currentPageInChapterIndex = _book.Chapters[_currentChapterIndex].PagesRtf.Count - 1; // Last page of previous chapter
                }
                else // Previous chapter is empty or invalid, try to find a valid one or go to start
                {
                    // This case should ideally be handled by robust GetAbsolutePageNumber/GetChapterAndPageIndices
                    // For now, attempt to recover or stay put.
                    _currentChapterIndex++; // Revert chapter change
                    // stay on current page (which is first page of current chapter)
                }
            }
            DisplayCurrentPage();
        }

        private void NavigateNext()
        {
            if (_book == null || _book.TotalPagesInBook == 0) return;
            int absolutePageNumber = _book.GetAbsolutePageNumber(_currentChapterIndex, _currentPageInChapterIndex);
            if (absolutePageNumber >= _book.TotalPagesInBook)
            {
                // Optionally, mark as finished here if not already
                // MessageBox.Show("Вы достигли конца книги.", "Конец книги", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BookChapter currentChapterObj = _book.Chapters[_currentChapterIndex];
            if (currentChapterObj != null && currentChapterObj.PagesRtf != null && _currentPageInChapterIndex < currentChapterObj.PagesRtf.Count - 1)
            {
                _currentPageInChapterIndex++;
            }
            else if (_currentChapterIndex < _book.Chapters.Count - 1)
            {
                _currentChapterIndex++;
                // Ensure the new chapter is valid and has pages
                if (_currentChapterIndex < _book.Chapters.Count &&
                    _book.Chapters[_currentChapterIndex] != null &&
                    _book.Chapters[_currentChapterIndex].PagesRtf != null &&
                    _book.Chapters[_currentChapterIndex].PagesRtf.Any())
                {
                    _currentPageInChapterIndex = 0; // First page of next chapter
                }
                else // Next chapter is empty or invalid
                {
                    _currentChapterIndex--; // Revert chapter change
                                            // Stay on current page (last page of current chapter)
                }
            }
            DisplayCurrentPage();
        }

        private void TocButton_Click(object sender, EventArgs e)
        {
            if (_book == null || _book.Chapters == null || !_book.Chapters.Any()) return;

            using (ContentForm tocForm = new ContentForm(_book.Chapters))
            {
                if (tocForm.ShowDialog(this) == DialogResult.OK)
                {
                    if (tocForm.SelectedChapterIndex >= 0 && tocForm.SelectedChapterIndex < _book.Chapters.Count)
                    {
                        _currentChapterIndex = tocForm.SelectedChapterIndex;
                        _currentPageInChapterIndex = 0; // Go to the first page of the selected chapter
                        DisplayCurrentPage();
                    }
                }
            }
        }

        private void PageTrackBar_Scroll(object sender, EventArgs e)
        {
            if (_book == null || _book.TotalPagesInBook == 0) return;

            // trackBarScrollTimer.Stop(); // Stop previous timer if any
            // trackBarScrollTimer.Start(); // Restart timer

            // For immediate update:
            ApplyTrackBarPageChange();
        }

        // private void TrackBarScrollTimer_Tick(object sender, EventArgs e)
        // {
        //    trackBarScrollTimer.Stop();
        //    ApplyTrackBarPageChange();
        // }

        private void ApplyTrackBarPageChange()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(ApplyTrackBarPageChange));
                return;
            }

            int absolutePageNumber = pageTrackBar.Value;
            var (chapIdx, pageInChapIdx) = _book.GetChapterAndPageIndices(absolutePageNumber);
            _currentChapterIndex = chapIdx;
            _currentPageInChapterIndex = pageInChapIdx;
            DisplayCurrentPage();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (_book == null || _book.TotalPagesInBook == 0) return;

            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.PageUp:
                    NavigatePrevious();
                    e.Handled = true;
                    break;
                case Keys.Right:
                case Keys.PageDown:
                case Keys.Space: // Common for paging
                    NavigateNext();
                    e.Handled = true;
                    break;
                case Keys.Home:
                    _currentChapterIndex = 0;
                    _currentPageInChapterIndex = 0;
                    DisplayCurrentPage();
                    e.Handled = true;
                    break;
                case Keys.End:
                    var (lastChapIdx, lastPageInChapIdx) = _book.GetChapterAndPageIndices(_book.TotalPagesInBook);
                    _currentChapterIndex = lastChapIdx;
                    _currentPageInChapterIndex = lastPageInChapIdx;
                    DisplayCurrentPage();
                    e.Handled = true;
                    break;
            }
        }
    }
}