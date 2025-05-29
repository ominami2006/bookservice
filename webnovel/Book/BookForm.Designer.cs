namespace bookservice
{
    partial class BookForm
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
            this.bookTabControl = new System.Windows.Forms.TabControl();
            this.bookTabPage = new System.Windows.Forms.TabPage();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.ageRatingLabel = new System.Windows.Forms.Label();
            this.yearLabel = new System.Windows.Forms.Label();
            this.genresFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.authorLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.downloadNovelLabelButton = new System.Windows.Forms.Label();
            this.novelFilesContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.coverPictureBox = new System.Windows.Forms.PictureBox();
            this.commentsTabPage = new System.Windows.Forms.TabPage();
            this.commentsHostPanel = new System.Windows.Forms.Panel();
            this.postCommentButton = new System.Windows.Forms.Button();
            this.newCommentTextBox = new System.Windows.Forms.TextBox();
            this.commentsTitleLabel = new System.Windows.Forms.Label();
            this.editBookTabPage = new System.Windows.Forms.TabPage();
            this.deleteBookButton = new System.Windows.Forms.Button();
            this.addFileButton = new System.Windows.Forms.Button();
            this.createBookTabPage = new System.Windows.Forms.TabPage();
            this.createWriterNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.createGenresCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.createBookButton = new System.Windows.Forms.Button();
            this.createSelectCoverButton = new System.Windows.Forms.Button();
            this.createDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.createAgeRatingListBox = new System.Windows.Forms.ListBox();
            this.createYearTextBox = new System.Windows.Forms.TextBox();
            this.createTitleTextBox = new System.Windows.Forms.TextBox();
            this.createGenresPromptLabel = new System.Windows.Forms.Label();
            this.createCoverPathLabel = new System.Windows.Forms.Label();
            this.createCoverPromptLabel = new System.Windows.Forms.Label();
            this.createDescriptionPromptLabel = new System.Windows.Forms.Label();
            this.createAgeRatingPromptLabel = new System.Windows.Forms.Label();
            this.createYearPromptLabel = new System.Windows.Forms.Label();
            this.createTitlePromptLabel = new System.Windows.Forms.Label();
            this.openNovelFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openCoverFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveNovelFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.bookTabControl.SuspendLayout();
            this.bookTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.coverPictureBox)).BeginInit();
            this.commentsTabPage.SuspendLayout();
            this.editBookTabPage.SuspendLayout();
            this.createBookTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // bookTabControl
            // 
            this.bookTabControl.Controls.Add(this.bookTabPage);
            this.bookTabControl.Controls.Add(this.commentsTabPage);
            this.bookTabControl.Controls.Add(this.editBookTabPage);
            this.bookTabControl.Controls.Add(this.createBookTabPage);
            this.bookTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bookTabControl.Location = new System.Drawing.Point(0, 0);
            this.bookTabControl.Name = "bookTabControl";
            this.bookTabControl.SelectedIndex = 0;
            this.bookTabControl.Size = new System.Drawing.Size(1354, 856);
            this.bookTabControl.TabIndex = 0;
            // 
            // bookTabPage
            // 
            this.bookTabPage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.bookTabPage.Controls.Add(this.descriptionLabel);
            this.bookTabPage.Controls.Add(this.ageRatingLabel);
            this.bookTabPage.Controls.Add(this.yearLabel);
            this.bookTabPage.Controls.Add(this.genresFlowLayoutPanel);
            this.bookTabPage.Controls.Add(this.authorLabel);
            this.bookTabPage.Controls.Add(this.titleLabel);
            this.bookTabPage.Controls.Add(this.downloadNovelLabelButton);
            this.bookTabPage.Controls.Add(this.coverPictureBox);
            this.bookTabPage.Location = new System.Drawing.Point(8, 46);
            this.bookTabPage.Name = "bookTabPage";
            this.bookTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.bookTabPage.Size = new System.Drawing.Size(1338, 802);
            this.bookTabPage.TabIndex = 0;
            this.bookTabPage.Text = "Информация";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Font = new System.Drawing.Font("Verdana", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.descriptionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.descriptionLabel.Location = new System.Drawing.Point(549, 242);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(494, 274);
            this.descriptionLabel.TabIndex = 7;
            this.descriptionLabel.Text = "Описание веб-романа...";
            // 
            // ageRatingLabel
            // 
            this.ageRatingLabel.AutoSize = true;
            this.ageRatingLabel.BackColor = System.Drawing.SystemColors.Control;
            this.ageRatingLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ageRatingLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ageRatingLabel.Location = new System.Drawing.Point(1031, 123);
            this.ageRatingLabel.Name = "ageRatingLabel";
            this.ageRatingLabel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.ageRatingLabel.Size = new System.Drawing.Size(255, 50);
            this.ageRatingLabel.TabIndex = 6;
            this.ageRatingLabel.Text = "🚫 {возраст}";
            // 
            // yearLabel
            // 
            this.yearLabel.AutoSize = true;
            this.yearLabel.BackColor = System.Drawing.SystemColors.Control;
            this.yearLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.yearLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.yearLabel.Location = new System.Drawing.Point(851, 123);
            this.yearLabel.Name = "yearLabel";
            this.yearLabel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.yearLabel.Size = new System.Drawing.Size(265, 50);
            this.yearLabel.TabIndex = 5;
            this.yearLabel.Text = "📅 Год: {год}";
            // 
            // genresFlowLayoutPanel
            // 
            this.genresFlowLayoutPanel.AutoSize = true;
            this.genresFlowLayoutPanel.Location = new System.Drawing.Point(555, 179);
            this.genresFlowLayoutPanel.Name = "genresFlowLayoutPanel";
            this.genresFlowLayoutPanel.Size = new System.Drawing.Size(647, 44);
            this.genresFlowLayoutPanel.TabIndex = 4;
            // 
            // authorLabel
            // 
            this.authorLabel.AutoSize = true;
            this.authorLabel.BackColor = System.Drawing.SystemColors.Control;
            this.authorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.authorLabel.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.authorLabel.Location = new System.Drawing.Point(555, 123);
            this.authorLabel.Name = "authorLabel";
            this.authorLabel.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.authorLabel.Size = new System.Drawing.Size(450, 50);
            this.authorLabel.TabIndex = 3;
            this.authorLabel.Text = "👤 Автор: {login автора}";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Font = new System.Drawing.Font("Verdana", 19.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.titleLabel.Location = new System.Drawing.Point(574, 41);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(696, 65);
            this.titleLabel.TabIndex = 2;
            this.titleLabel.Text = "Название веб-романа";
            // 
            // downloadNovelLabelButton
            // 
            this.downloadNovelLabelButton.BackColor = System.Drawing.SystemColors.Control;
            this.downloadNovelLabelButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.downloadNovelLabelButton.ContextMenuStrip = this.novelFilesContextMenuStrip;
            this.downloadNovelLabelButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.downloadNovelLabelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.downloadNovelLabelButton.Location = new System.Drawing.Point(232, 470);
            this.downloadNovelLabelButton.Name = "downloadNovelLabelButton";
            this.downloadNovelLabelButton.Size = new System.Drawing.Size(306, 62);
            this.downloadNovelLabelButton.TabIndex = 1;
            this.downloadNovelLabelButton.Text = "Скачать веб-роман";
            this.downloadNovelLabelButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.downloadNovelLabelButton.Click += new System.EventHandler(this.downloadNovelLabelButton_Click);
            // 
            // novelFilesContextMenuStrip
            // 
            this.novelFilesContextMenuStrip.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.novelFilesContextMenuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.novelFilesContextMenuStrip.Name = "novelFilesContextMenuStrip";
            this.novelFilesContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // coverPictureBox
            // 
            this.coverPictureBox.BackColor = System.Drawing.SystemColors.ControlDark;
            this.coverPictureBox.Location = new System.Drawing.Point(232, 41);
            this.coverPictureBox.Name = "coverPictureBox";
            this.coverPictureBox.Size = new System.Drawing.Size(306, 419);
            this.coverPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.coverPictureBox.TabIndex = 0;
            this.coverPictureBox.TabStop = false;
            // 
            // commentsTabPage
            // 
            this.commentsTabPage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.commentsTabPage.Controls.Add(this.commentsHostPanel);
            this.commentsTabPage.Controls.Add(this.postCommentButton);
            this.commentsTabPage.Controls.Add(this.newCommentTextBox);
            this.commentsTabPage.Controls.Add(this.commentsTitleLabel);
            this.commentsTabPage.Location = new System.Drawing.Point(8, 46);
            this.commentsTabPage.Name = "commentsTabPage";
            this.commentsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.commentsTabPage.Size = new System.Drawing.Size(1338, 802);
            this.commentsTabPage.TabIndex = 1;
            this.commentsTabPage.Text = "Комментарии";
            // 
            // commentsHostPanel
            // 
            this.commentsHostPanel.AutoScroll = true;
            this.commentsHostPanel.Location = new System.Drawing.Point(97, 277);
            this.commentsHostPanel.Name = "commentsHostPanel";
            this.commentsHostPanel.Size = new System.Drawing.Size(1128, 280);
            this.commentsHostPanel.TabIndex = 3;
            // 
            // postCommentButton
            // 
            this.postCommentButton.BackColor = System.Drawing.SystemColors.Control;
            this.postCommentButton.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.postCommentButton.Location = new System.Drawing.Point(97, 207);
            this.postCommentButton.Name = "postCommentButton";
            this.postCommentButton.Size = new System.Drawing.Size(317, 64);
            this.postCommentButton.TabIndex = 2;
            this.postCommentButton.Text = "Отправить";
            this.postCommentButton.UseVisualStyleBackColor = false;
            this.postCommentButton.Click += new System.EventHandler(this.postCommentButton_Click);
            // 
            // newCommentTextBox
            // 
            this.newCommentTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.newCommentTextBox.Font = new System.Drawing.Font("Verdana", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.newCommentTextBox.ForeColor = System.Drawing.Color.Gray;
            this.newCommentTextBox.Location = new System.Drawing.Point(97, 101);
            this.newCommentTextBox.Multiline = true;
            this.newCommentTextBox.Name = "newCommentTextBox";
            this.newCommentTextBox.Size = new System.Drawing.Size(1128, 100);
            this.newCommentTextBox.TabIndex = 1;
            this.newCommentTextBox.Text = "Написать комментарий...";
            this.newCommentTextBox.Enter += new System.EventHandler(this.NewCommentTextBox_Enter);
            this.newCommentTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewCommentTextBox_KeyDown);
            this.newCommentTextBox.Leave += new System.EventHandler(this.NewCommentTextBox_Leave);
            // 
            // commentsTitleLabel
            // 
            this.commentsTitleLabel.AutoSize = true;
            this.commentsTitleLabel.Font = new System.Drawing.Font("Verdana", 19.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.commentsTitleLabel.Location = new System.Drawing.Point(90, 40);
            this.commentsTitleLabel.Name = "commentsTitleLabel";
            this.commentsTitleLabel.Size = new System.Drawing.Size(463, 65);
            this.commentsTitleLabel.TabIndex = 0;
            this.commentsTitleLabel.Text = "Комментарии:";
            // 
            // editBookTabPage
            // 
            this.editBookTabPage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editBookTabPage.Controls.Add(this.deleteBookButton);
            this.editBookTabPage.Controls.Add(this.addFileButton);
            this.editBookTabPage.Location = new System.Drawing.Point(8, 46);
            this.editBookTabPage.Name = "editBookTabPage";
            this.editBookTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.editBookTabPage.Size = new System.Drawing.Size(1338, 802);
            this.editBookTabPage.TabIndex = 2;
            this.editBookTabPage.Text = "Редактирование";
            // 
            // deleteBookButton
            // 
            this.deleteBookButton.BackColor = System.Drawing.SystemColors.Control;
            this.deleteBookButton.Font = new System.Drawing.Font("Verdana", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deleteBookButton.ForeColor = System.Drawing.Color.Red;
            this.deleteBookButton.Location = new System.Drawing.Point(413, 356);
            this.deleteBookButton.Name = "deleteBookButton";
            this.deleteBookButton.Size = new System.Drawing.Size(542, 99);
            this.deleteBookButton.TabIndex = 1;
            this.deleteBookButton.Text = "Удалить книгу";
            this.deleteBookButton.UseVisualStyleBackColor = false;
            this.deleteBookButton.Click += new System.EventHandler(this.deleteBookButton_Click);
            // 
            // addFileButton
            // 
            this.addFileButton.BackColor = System.Drawing.SystemColors.Control;
            this.addFileButton.Font = new System.Drawing.Font("Verdana", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.addFileButton.Location = new System.Drawing.Point(413, 226);
            this.addFileButton.Name = "addFileButton";
            this.addFileButton.Size = new System.Drawing.Size(542, 99);
            this.addFileButton.TabIndex = 0;
            this.addFileButton.Text = "Добавить файл (.epub)";
            this.addFileButton.UseVisualStyleBackColor = false;
            this.addFileButton.Click += new System.EventHandler(this.addFileButton_Click);
            // 
            // createBookTabPage
            // 
            this.createBookTabPage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.createBookTabPage.Controls.Add(this.createWriterNameTextBox);
            this.createBookTabPage.Controls.Add(this.label1);
            this.createBookTabPage.Controls.Add(this.createGenresCheckedListBox);
            this.createBookTabPage.Controls.Add(this.createBookButton);
            this.createBookTabPage.Controls.Add(this.createSelectCoverButton);
            this.createBookTabPage.Controls.Add(this.createDescriptionTextBox);
            this.createBookTabPage.Controls.Add(this.createAgeRatingListBox);
            this.createBookTabPage.Controls.Add(this.createYearTextBox);
            this.createBookTabPage.Controls.Add(this.createTitleTextBox);
            this.createBookTabPage.Controls.Add(this.createGenresPromptLabel);
            this.createBookTabPage.Controls.Add(this.createCoverPathLabel);
            this.createBookTabPage.Controls.Add(this.createCoverPromptLabel);
            this.createBookTabPage.Controls.Add(this.createDescriptionPromptLabel);
            this.createBookTabPage.Controls.Add(this.createAgeRatingPromptLabel);
            this.createBookTabPage.Controls.Add(this.createYearPromptLabel);
            this.createBookTabPage.Controls.Add(this.createTitlePromptLabel);
            this.createBookTabPage.Font = new System.Drawing.Font("Verdana", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.createBookTabPage.Location = new System.Drawing.Point(8, 46);
            this.createBookTabPage.Name = "createBookTabPage";
            this.createBookTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.createBookTabPage.Size = new System.Drawing.Size(1338, 802);
            this.createBookTabPage.TabIndex = 3;
            this.createBookTabPage.Text = "Создание веб-романа";
            // 
            // createWriterNameTextBox
            // 
            this.createWriterNameTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.createWriterNameTextBox.Location = new System.Drawing.Point(565, 91);
            this.createWriterNameTextBox.MaxLength = 100;
            this.createWriterNameTextBox.Name = "createWriterNameTextBox";
            this.createWriterNameTextBox.Size = new System.Drawing.Size(391, 52);
            this.createWriterNameTextBox.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(393, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 45);
            this.label1.TabIndex = 14;
            this.label1.Text = "Автор:";
            // 
            // createGenresCheckedListBox
            // 
            this.createGenresCheckedListBox.BackColor = System.Drawing.SystemColors.Control;
            this.createGenresCheckedListBox.FormattingEnabled = true;
            this.createGenresCheckedListBox.Location = new System.Drawing.Point(566, 421);
            this.createGenresCheckedListBox.Name = "createGenresCheckedListBox";
            this.createGenresCheckedListBox.Size = new System.Drawing.Size(391, 102);
            this.createGenresCheckedListBox.TabIndex = 13;
            // 
            // createBookButton
            // 
            this.createBookButton.Font = new System.Drawing.Font("Verdana", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.createBookButton.Location = new System.Drawing.Point(401, 560);
            this.createBookButton.Name = "createBookButton";
            this.createBookButton.Size = new System.Drawing.Size(556, 80);
            this.createBookButton.TabIndex = 12;
            this.createBookButton.Text = "Создать веб-роман";
            this.createBookButton.UseVisualStyleBackColor = true;
            this.createBookButton.Click += new System.EventHandler(this.createBookButton_Click);
            // 
            // createSelectCoverButton
            // 
            this.createSelectCoverButton.BackColor = System.Drawing.SystemColors.Control;
            this.createSelectCoverButton.Location = new System.Drawing.Point(566, 357);
            this.createSelectCoverButton.Name = "createSelectCoverButton";
            this.createSelectCoverButton.Size = new System.Drawing.Size(391, 58);
            this.createSelectCoverButton.TabIndex = 11;
            this.createSelectCoverButton.Text = "Выбрать обложку...";
            this.createSelectCoverButton.UseVisualStyleBackColor = false;
            this.createSelectCoverButton.Click += new System.EventHandler(this.createSelectCoverButton_Click);
            // 
            // createDescriptionTextBox
            // 
            this.createDescriptionTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.createDescriptionTextBox.Location = new System.Drawing.Point(566, 243);
            this.createDescriptionTextBox.Multiline = true;
            this.createDescriptionTextBox.Name = "createDescriptionTextBox";
            this.createDescriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.createDescriptionTextBox.Size = new System.Drawing.Size(391, 108);
            this.createDescriptionTextBox.TabIndex = 10;
            // 
            // createAgeRatingListBox
            // 
            this.createAgeRatingListBox.BackColor = System.Drawing.SystemColors.Control;
            this.createAgeRatingListBox.FormattingEnabled = true;
            this.createAgeRatingListBox.ItemHeight = 45;
            this.createAgeRatingListBox.Items.AddRange(new object[] {
            "0+",
            "13+",
            "16+",
            "18+"});
            this.createAgeRatingListBox.Location = new System.Drawing.Point(566, 177);
            this.createAgeRatingListBox.Name = "createAgeRatingListBox";
            this.createAgeRatingListBox.Size = new System.Drawing.Size(391, 49);
            this.createAgeRatingListBox.TabIndex = 9;
            // 
            // createYearTextBox
            // 
            this.createYearTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.createYearTextBox.Location = new System.Drawing.Point(566, 136);
            this.createYearTextBox.MaxLength = 4;
            this.createYearTextBox.Name = "createYearTextBox";
            this.createYearTextBox.Size = new System.Drawing.Size(391, 52);
            this.createYearTextBox.TabIndex = 8;
            // 
            // createTitleTextBox
            // 
            this.createTitleTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.createTitleTextBox.Location = new System.Drawing.Point(566, 40);
            this.createTitleTextBox.MaxLength = 100;
            this.createTitleTextBox.Name = "createTitleTextBox";
            this.createTitleTextBox.Size = new System.Drawing.Size(391, 52);
            this.createTitleTextBox.TabIndex = 7;
            // 
            // createGenresPromptLabel
            // 
            this.createGenresPromptLabel.AutoSize = true;
            this.createGenresPromptLabel.Location = new System.Drawing.Point(396, 469);
            this.createGenresPromptLabel.Name = "createGenresPromptLabel";
            this.createGenresPromptLabel.Size = new System.Drawing.Size(171, 45);
            this.createGenresPromptLabel.TabIndex = 6;
            this.createGenresPromptLabel.Text = "Жанры:\r\n";
            this.createGenresPromptLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // createCoverPathLabel
            // 
            this.createCoverPathLabel.AutoSize = true;
            this.createCoverPathLabel.Location = new System.Drawing.Point(975, 372);
            this.createCoverPathLabel.Name = "createCoverPathLabel";
            this.createCoverPathLabel.Size = new System.Drawing.Size(334, 45);
            this.createCoverPathLabel.TabIndex = 5;
            this.createCoverPathLabel.Text = "Файл не выбран";
            // 
            // createCoverPromptLabel
            // 
            this.createCoverPromptLabel.AutoSize = true;
            this.createCoverPromptLabel.Location = new System.Drawing.Point(396, 372);
            this.createCoverPromptLabel.Name = "createCoverPromptLabel";
            this.createCoverPromptLabel.Size = new System.Drawing.Size(208, 45);
            this.createCoverPromptLabel.TabIndex = 4;
            this.createCoverPromptLabel.Text = "Обложка:";
            // 
            // createDescriptionPromptLabel
            // 
            this.createDescriptionPromptLabel.AutoSize = true;
            this.createDescriptionPromptLabel.Location = new System.Drawing.Point(396, 283);
            this.createDescriptionPromptLabel.Name = "createDescriptionPromptLabel";
            this.createDescriptionPromptLabel.Size = new System.Drawing.Size(226, 45);
            this.createDescriptionPromptLabel.TabIndex = 3;
            this.createDescriptionPromptLabel.Text = "Описание:";
            // 
            // createAgeRatingPromptLabel
            // 
            this.createAgeRatingPromptLabel.AutoSize = true;
            this.createAgeRatingPromptLabel.Location = new System.Drawing.Point(396, 193);
            this.createAgeRatingPromptLabel.Name = "createAgeRatingPromptLabel";
            this.createAgeRatingPromptLabel.Size = new System.Drawing.Size(186, 45);
            this.createAgeRatingPromptLabel.TabIndex = 2;
            this.createAgeRatingPromptLabel.Text = "Возраст:";
            // 
            // createYearPromptLabel
            // 
            this.createYearPromptLabel.AutoSize = true;
            this.createYearPromptLabel.Location = new System.Drawing.Point(396, 139);
            this.createYearPromptLabel.Name = "createYearPromptLabel";
            this.createYearPromptLabel.Size = new System.Drawing.Size(234, 45);
            this.createYearPromptLabel.TabIndex = 1;
            this.createYearPromptLabel.Text = "Год (ГГГГ):";
            // 
            // createTitlePromptLabel
            // 
            this.createTitlePromptLabel.AutoSize = true;
            this.createTitlePromptLabel.Location = new System.Drawing.Point(396, 43);
            this.createTitlePromptLabel.Name = "createTitlePromptLabel";
            this.createTitlePromptLabel.Size = new System.Drawing.Size(220, 45);
            this.createTitlePromptLabel.TabIndex = 0;
            this.createTitlePromptLabel.Text = "Название:";
            // 
            // openNovelFileDialog
            // 
            this.openNovelFileDialog.Filter = "EPUB файлы (*.epub)|*.epub";
            this.openNovelFileDialog.Title = "Выберите файл веб-романа";
            // 
            // openCoverFileDialog
            // 
            this.openCoverFileDialog.Filter = "Изображения (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            this.openCoverFileDialog.Title = "Выберите обложку для веб-романа";
            // 
            // saveNovelFileDialog
            // 
            this.saveNovelFileDialog.Filter = "EPUB файлы (*.epub)|*.epub";
            this.saveNovelFileDialog.Title = "Сохранить файл веб-романа";
            // 
            // BookForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1354, 856);
            this.Controls.Add(this.bookTabControl);
            this.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "BookForm";
            this.Text = "Веб-роман";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NovelForm_FormClosed);
            this.Load += new System.EventHandler(this.NovelForm_Load);
            this.bookTabControl.ResumeLayout(false);
            this.bookTabPage.ResumeLayout(false);
            this.bookTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.coverPictureBox)).EndInit();
            this.commentsTabPage.ResumeLayout(false);
            this.commentsTabPage.PerformLayout();
            this.editBookTabPage.ResumeLayout(false);
            this.createBookTabPage.ResumeLayout(false);
            this.createBookTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl bookTabControl;
        private System.Windows.Forms.TabPage bookTabPage;
        private System.Windows.Forms.PictureBox coverPictureBox;
        private System.Windows.Forms.TabPage commentsTabPage;
        private System.Windows.Forms.TabPage editBookTabPage;
        private System.Windows.Forms.TabPage createBookTabPage;
        private System.Windows.Forms.Label downloadNovelLabelButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.ContextMenuStrip novelFilesContextMenuStrip;
        private System.Windows.Forms.FlowLayoutPanel genresFlowLayoutPanel;
        private System.Windows.Forms.Label authorLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label ageRatingLabel;
        private System.Windows.Forms.Label yearLabel;
        private System.Windows.Forms.Label commentsTitleLabel;
        private System.Windows.Forms.TextBox newCommentTextBox;
        private System.Windows.Forms.Panel commentsHostPanel;
        private System.Windows.Forms.Button postCommentButton;
        private System.Windows.Forms.Button deleteBookButton;
        private System.Windows.Forms.Button addFileButton;
        private System.Windows.Forms.ListBox createAgeRatingListBox;
        private System.Windows.Forms.TextBox createYearTextBox;
        private System.Windows.Forms.TextBox createTitleTextBox;
        private System.Windows.Forms.Label createGenresPromptLabel;
        private System.Windows.Forms.Label createCoverPathLabel;
        private System.Windows.Forms.Label createCoverPromptLabel;
        private System.Windows.Forms.Label createDescriptionPromptLabel;
        private System.Windows.Forms.Label createAgeRatingPromptLabel;
        private System.Windows.Forms.Label createYearPromptLabel;
        private System.Windows.Forms.Label createTitlePromptLabel;
        private System.Windows.Forms.CheckedListBox createGenresCheckedListBox;
        private System.Windows.Forms.Button createBookButton;
        private System.Windows.Forms.Button createSelectCoverButton;
        private System.Windows.Forms.TextBox createDescriptionTextBox;
        private System.Windows.Forms.OpenFileDialog openNovelFileDialog;
        private System.Windows.Forms.OpenFileDialog openCoverFileDialog;
        private System.Windows.Forms.SaveFileDialog saveNovelFileDialog;
        private System.Windows.Forms.TextBox createWriterNameTextBox;
        private System.Windows.Forms.Label label1;
    }
}