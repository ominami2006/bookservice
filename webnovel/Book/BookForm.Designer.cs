namespace webnovel
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
            this.novelTabControl = new System.Windows.Forms.TabControl();
            this.novelTabPage = new System.Windows.Forms.TabPage();
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
            this.editNovelTabPage = new System.Windows.Forms.TabPage();
            this.deleteNovelButton = new System.Windows.Forms.Button();
            this.addFileButton = new System.Windows.Forms.Button();
            this.createNovelTabPage = new System.Windows.Forms.TabPage();
            this.createGenresCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.createNovelButton = new System.Windows.Forms.Button();
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
            this.novelTabControl.SuspendLayout();
            this.novelTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.coverPictureBox)).BeginInit();
            this.commentsTabPage.SuspendLayout();
            this.editNovelTabPage.SuspendLayout();
            this.createNovelTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // novelTabControl
            // 
            this.novelTabControl.Controls.Add(this.novelTabPage);
            this.novelTabControl.Controls.Add(this.commentsTabPage);
            this.novelTabControl.Controls.Add(this.editNovelTabPage);
            this.novelTabControl.Controls.Add(this.createNovelTabPage);
            this.novelTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.novelTabControl.Location = new System.Drawing.Point(0, 0);
            this.novelTabControl.Name = "novelTabControl";
            this.novelTabControl.SelectedIndex = 0;
            this.novelTabControl.Size = new System.Drawing.Size(1354, 856);
            this.novelTabControl.TabIndex = 0;
            // 
            // novelTabPage
            // 
            this.novelTabPage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.novelTabPage.Controls.Add(this.descriptionLabel);
            this.novelTabPage.Controls.Add(this.ageRatingLabel);
            this.novelTabPage.Controls.Add(this.yearLabel);
            this.novelTabPage.Controls.Add(this.genresFlowLayoutPanel);
            this.novelTabPage.Controls.Add(this.authorLabel);
            this.novelTabPage.Controls.Add(this.titleLabel);
            this.novelTabPage.Controls.Add(this.downloadNovelLabelButton);
            this.novelTabPage.Controls.Add(this.coverPictureBox);
            this.novelTabPage.Location = new System.Drawing.Point(4, 29);
            this.novelTabPage.Name = "novelTabPage";
            this.novelTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.novelTabPage.Size = new System.Drawing.Size(1346, 823);
            this.novelTabPage.TabIndex = 0;
            this.novelTabPage.Text = "Информация";
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
            this.ageRatingLabel.Size = new System.Drawing.Size(171, 37);
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
            this.yearLabel.Size = new System.Drawing.Size(174, 37);
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
            this.authorLabel.Size = new System.Drawing.Size(290, 37);
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
            this.titleLabel.Size = new System.Drawing.Size(449, 41);
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
            this.commentsTabPage.Location = new System.Drawing.Point(4, 29);
            this.commentsTabPage.Name = "commentsTabPage";
            this.commentsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.commentsTabPage.Size = new System.Drawing.Size(1346, 823);
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
            this.commentsTitleLabel.Size = new System.Drawing.Size(297, 41);
            this.commentsTitleLabel.TabIndex = 0;
            this.commentsTitleLabel.Text = "Комментарии:";
            // 
            // editNovelTabPage
            // 
            this.editNovelTabPage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.editNovelTabPage.Controls.Add(this.deleteNovelButton);
            this.editNovelTabPage.Controls.Add(this.addFileButton);
            this.editNovelTabPage.Location = new System.Drawing.Point(4, 29);
            this.editNovelTabPage.Name = "editNovelTabPage";
            this.editNovelTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.editNovelTabPage.Size = new System.Drawing.Size(1346, 823);
            this.editNovelTabPage.TabIndex = 2;
            this.editNovelTabPage.Text = "Редактирование";
            // 
            // deleteNovelButton
            // 
            this.deleteNovelButton.BackColor = System.Drawing.SystemColors.Control;
            this.deleteNovelButton.Font = new System.Drawing.Font("Verdana", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deleteNovelButton.ForeColor = System.Drawing.Color.Red;
            this.deleteNovelButton.Location = new System.Drawing.Point(413, 356);
            this.deleteNovelButton.Name = "deleteNovelButton";
            this.deleteNovelButton.Size = new System.Drawing.Size(542, 99);
            this.deleteNovelButton.TabIndex = 1;
            this.deleteNovelButton.Text = "Удалить книгу";
            this.deleteNovelButton.UseVisualStyleBackColor = false;
            this.deleteNovelButton.Click += new System.EventHandler(this.deleteNovelButton_Click);
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
            // createNovelTabPage
            // 
            this.createNovelTabPage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.createNovelTabPage.Controls.Add(this.createGenresCheckedListBox);
            this.createNovelTabPage.Controls.Add(this.createNovelButton);
            this.createNovelTabPage.Controls.Add(this.createSelectCoverButton);
            this.createNovelTabPage.Controls.Add(this.createDescriptionTextBox);
            this.createNovelTabPage.Controls.Add(this.createAgeRatingListBox);
            this.createNovelTabPage.Controls.Add(this.createYearTextBox);
            this.createNovelTabPage.Controls.Add(this.createTitleTextBox);
            this.createNovelTabPage.Controls.Add(this.createGenresPromptLabel);
            this.createNovelTabPage.Controls.Add(this.createCoverPathLabel);
            this.createNovelTabPage.Controls.Add(this.createCoverPromptLabel);
            this.createNovelTabPage.Controls.Add(this.createDescriptionPromptLabel);
            this.createNovelTabPage.Controls.Add(this.createAgeRatingPromptLabel);
            this.createNovelTabPage.Controls.Add(this.createYearPromptLabel);
            this.createNovelTabPage.Controls.Add(this.createTitlePromptLabel);
            this.createNovelTabPage.Font = new System.Drawing.Font("Verdana", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.createNovelTabPage.Location = new System.Drawing.Point(4, 29);
            this.createNovelTabPage.Name = "createNovelTabPage";
            this.createNovelTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.createNovelTabPage.Size = new System.Drawing.Size(1346, 823);
            this.createNovelTabPage.TabIndex = 3;
            this.createNovelTabPage.Text = "Создание веб-романа";
            // 
            // createGenresCheckedListBox
            // 
            this.createGenresCheckedListBox.BackColor = System.Drawing.SystemColors.Control;
            this.createGenresCheckedListBox.FormattingEnabled = true;
            this.createGenresCheckedListBox.Location = new System.Drawing.Point(566, 421);
            this.createGenresCheckedListBox.Name = "createGenresCheckedListBox";
            this.createGenresCheckedListBox.Size = new System.Drawing.Size(391, 124);
            this.createGenresCheckedListBox.TabIndex = 13;
            // 
            // createNovelButton
            // 
            this.createNovelButton.Font = new System.Drawing.Font("Verdana", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.createNovelButton.Location = new System.Drawing.Point(401, 560);
            this.createNovelButton.Name = "createNovelButton";
            this.createNovelButton.Size = new System.Drawing.Size(556, 80);
            this.createNovelButton.TabIndex = 12;
            this.createNovelButton.Text = "Создать веб-роман";
            this.createNovelButton.UseVisualStyleBackColor = true;
            this.createNovelButton.Click += new System.EventHandler(this.createNovelButton_Click);
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
            this.createAgeRatingListBox.ItemHeight = 28;
            this.createAgeRatingListBox.Items.AddRange(new object[] {
            "0+",
            "13+",
            "16+",
            "18+"});
            this.createAgeRatingListBox.Location = new System.Drawing.Point(566, 177);
            this.createAgeRatingListBox.Name = "createAgeRatingListBox";
            this.createAgeRatingListBox.Size = new System.Drawing.Size(391, 60);
            this.createAgeRatingListBox.TabIndex = 9;
            // 
            // createYearTextBox
            // 
            this.createYearTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.createYearTextBox.Location = new System.Drawing.Point(566, 136);
            this.createYearTextBox.MaxLength = 4;
            this.createYearTextBox.Name = "createYearTextBox";
            this.createYearTextBox.Size = new System.Drawing.Size(391, 35);
            this.createYearTextBox.TabIndex = 8;
            // 
            // createTitleTextBox
            // 
            this.createTitleTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.createTitleTextBox.Location = new System.Drawing.Point(566, 95);
            this.createTitleTextBox.MaxLength = 100;
            this.createTitleTextBox.Name = "createTitleTextBox";
            this.createTitleTextBox.Size = new System.Drawing.Size(391, 35);
            this.createTitleTextBox.TabIndex = 7;
            // 
            // createGenresPromptLabel
            // 
            this.createGenresPromptLabel.AutoSize = true;
            this.createGenresPromptLabel.Location = new System.Drawing.Point(396, 469);
            this.createGenresPromptLabel.Name = "createGenresPromptLabel";
            this.createGenresPromptLabel.Size = new System.Drawing.Size(105, 28);
            this.createGenresPromptLabel.TabIndex = 6;
            this.createGenresPromptLabel.Text = "Жанры:\r\n";
            this.createGenresPromptLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // createCoverPathLabel
            // 
            this.createCoverPathLabel.AutoSize = true;
            this.createCoverPathLabel.Location = new System.Drawing.Point(975, 372);
            this.createCoverPathLabel.Name = "createCoverPathLabel";
            this.createCoverPathLabel.Size = new System.Drawing.Size(208, 28);
            this.createCoverPathLabel.TabIndex = 5;
            this.createCoverPathLabel.Text = "Файл не выбран";
            // 
            // createCoverPromptLabel
            // 
            this.createCoverPromptLabel.AutoSize = true;
            this.createCoverPromptLabel.Location = new System.Drawing.Point(396, 372);
            this.createCoverPromptLabel.Name = "createCoverPromptLabel";
            this.createCoverPromptLabel.Size = new System.Drawing.Size(128, 28);
            this.createCoverPromptLabel.TabIndex = 4;
            this.createCoverPromptLabel.Text = "Обложка:";
            // 
            // createDescriptionPromptLabel
            // 
            this.createDescriptionPromptLabel.AutoSize = true;
            this.createDescriptionPromptLabel.Location = new System.Drawing.Point(396, 283);
            this.createDescriptionPromptLabel.Name = "createDescriptionPromptLabel";
            this.createDescriptionPromptLabel.Size = new System.Drawing.Size(140, 28);
            this.createDescriptionPromptLabel.TabIndex = 3;
            this.createDescriptionPromptLabel.Text = "Описание:";
            // 
            // createAgeRatingPromptLabel
            // 
            this.createAgeRatingPromptLabel.AutoSize = true;
            this.createAgeRatingPromptLabel.Location = new System.Drawing.Point(396, 193);
            this.createAgeRatingPromptLabel.Name = "createAgeRatingPromptLabel";
            this.createAgeRatingPromptLabel.Size = new System.Drawing.Size(115, 28);
            this.createAgeRatingPromptLabel.TabIndex = 2;
            this.createAgeRatingPromptLabel.Text = "Возраст:";
            // 
            // createYearPromptLabel
            // 
            this.createYearPromptLabel.AutoSize = true;
            this.createYearPromptLabel.Location = new System.Drawing.Point(396, 139);
            this.createYearPromptLabel.Name = "createYearPromptLabel";
            this.createYearPromptLabel.Size = new System.Drawing.Size(143, 28);
            this.createYearPromptLabel.TabIndex = 1;
            this.createYearPromptLabel.Text = "Год (ГГГГ):";
            // 
            // createTitlePromptLabel
            // 
            this.createTitlePromptLabel.AutoSize = true;
            this.createTitlePromptLabel.Location = new System.Drawing.Point(396, 98);
            this.createTitlePromptLabel.Name = "createTitlePromptLabel";
            this.createTitlePromptLabel.Size = new System.Drawing.Size(137, 28);
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
            // NovelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1354, 856);
            this.Controls.Add(this.novelTabControl);
            this.Font = new System.Drawing.Font("Verdana", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "NovelForm";
            this.Text = "Веб-роман";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NovelForm_FormClosed);
            this.Load += new System.EventHandler(this.NovelForm_Load);
            this.novelTabControl.ResumeLayout(false);
            this.novelTabPage.ResumeLayout(false);
            this.novelTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.coverPictureBox)).EndInit();
            this.commentsTabPage.ResumeLayout(false);
            this.commentsTabPage.PerformLayout();
            this.editNovelTabPage.ResumeLayout(false);
            this.createNovelTabPage.ResumeLayout(false);
            this.createNovelTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl novelTabControl;
        private System.Windows.Forms.TabPage novelTabPage;
        private System.Windows.Forms.PictureBox coverPictureBox;
        private System.Windows.Forms.TabPage commentsTabPage;
        private System.Windows.Forms.TabPage editNovelTabPage;
        private System.Windows.Forms.TabPage createNovelTabPage;
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
        private System.Windows.Forms.Button deleteNovelButton;
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
        private System.Windows.Forms.Button createNovelButton;
        private System.Windows.Forms.Button createSelectCoverButton;
        private System.Windows.Forms.TextBox createDescriptionTextBox;
        private System.Windows.Forms.OpenFileDialog openNovelFileDialog;
        private System.Windows.Forms.OpenFileDialog openCoverFileDialog;
        private System.Windows.Forms.SaveFileDialog saveNovelFileDialog;
    }
}