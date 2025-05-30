using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using bookservice.FB2Logic; // Required for BookChapter

namespace bookservice
{
    public partial class ContentForm : Form // Was Form2 from FB2Reader
    {
        private List<BookChapter> _chapters;
        private FlowLayoutPanel flowLayoutPanelChapters;
        public int SelectedChapterIndex { get; private set; } = -1;

        public ContentForm(List<BookChapter> chapters)
        {
            InitializeComponent();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 561);
            this.Name = "ContentForm";
            this.Text = "Оглавление";
            _chapters = chapters;
            // Designer might call InitializeComponent, ensure SetupUI is called after or during Load
            this.Load += ContentForm_Load;
        }

        private void SetupUI()
        {
            this.Text = "Оглавление книги";
            this.ClientSize = new System.Drawing.Size(500, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.BackColor = SystemColors.ControlLightLight;
            this.Font = new Font("Verdana", 10F);


            flowLayoutPanelChapters = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10),
                BackColor = SystemColors.ControlLightLight
            };
            this.Controls.Add(flowLayoutPanelChapters);

            if (_chapters == null || _chapters.Count == 0)
            {
                Label noContentLabel = new Label
                {
                    Text = "Оглавление не найдено или книга пуста.",
                    AutoSize = true,
                    Margin = new Padding(5),
                    Font = new Font("Verdana", 11F, FontStyle.Italic)
                };
                flowLayoutPanelChapters.Controls.Add(noContentLabel);
                // Center the label if it's the only control
                flowLayoutPanelChapters.Layout += (s, e) => {
                    if (flowLayoutPanelChapters.Controls.Count == 1)
                    {
                        var ctrl = flowLayoutPanelChapters.Controls[0];
                        ctrl.Location = new Point((flowLayoutPanelChapters.ClientSize.Width - ctrl.Width) / 2, (flowLayoutPanelChapters.ClientSize.Height - ctrl.Height) / 2);
                    }
                };
                return;
            }

            for (int i = 0; i < _chapters.Count; i++)
            {
                if (_chapters[i] == null) continue; // Skip null chapters

                Button chapterButton = new Button
                {
                    Text = $"{i + 1}. {(_chapters[i].Title ?? "Без названия")}",
                    Tag = i, // Store chapter index
                    AutoSize = true,
                    MinimumSize = new Size(flowLayoutPanelChapters.ClientSize.Width - 30, 35),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(10, 5, 10, 5),
                    Margin = new Padding(0, 0, 0, 8), // Increased bottom margin
                    Font = new Font("Verdana", 11F),
                    FlatStyle = FlatStyle.System // Use system style for better appearance
                };
                chapterButton.Click += ChapterButton_Click;
                flowLayoutPanelChapters.Controls.Add(chapterButton);
            }

            // Adjust button widths after all are added and flow panel has its size
            flowLayoutPanelChapters.ClientSizeChanged += (s, e) => AdjustButtonWidths();
            AdjustButtonWidths();
        }

        private void AdjustButtonWidths()
        {
            if (flowLayoutPanelChapters == null) return;
            foreach (Control ctrl in flowLayoutPanelChapters.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.Width = flowLayoutPanelChapters.ClientSize.Width - 10; // Account for padding and scrollbar
                }
            }
        }

        private void ChapterButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null && clickedButton.Tag is int chapterIndex)
            {
                SelectedChapterIndex = chapterIndex;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void ContentForm_Load(object sender, EventArgs e)
        {
            SetupUI(); // Ensure UI is set up on load
        }
    }
}