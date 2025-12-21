namespace View
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lstHeroes = new System.Windows.Forms.ListBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.cmbAttribute = new System.Windows.Forms.ComboBox();
            this.numComplexity = new System.Windows.Forms.NumericUpDown();
            this.cmbSearchRole = new System.Windows.Forms.ComboBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnFindByRole = new System.Windows.Forms.Button();
            this.btnGroupByAttribute = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblAttribute = new System.Windows.Forms.Label();
            this.lblComplexity = new System.Windows.Forms.Label();
            this.lblSearchRole = new System.Windows.Forms.Label();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.lblTotalPages = new System.Windows.Forms.Label();

            this.cmbSearchRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchRole.FormattingEnabled = true;
            this.cmbSearchRole.Location = new System.Drawing.Point(12, 285);
            this.cmbSearchRole.Name = "cmbSearchRole";
            this.cmbSearchRole.Size = new System.Drawing.Size(120, 21);
            this.cmbSearchRole.TabIndex = 5;

            ((System.ComponentModel.ISupportInitialize)(this.numComplexity)).BeginInit();
            this.SuspendLayout();

            // lstHeroes
            this.lstHeroes.FormattingEnabled = true;
            this.lstHeroes.Location = new System.Drawing.Point(12, 12);
            this.lstHeroes.Name = "lstHeroes";
            this.lstHeroes.Size = new System.Drawing.Size(200, 225);
            this.lstHeroes.TabIndex = 0;

            // txtName
            this.txtName.Location = new System.Drawing.Point(218, 28);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(150, 20);
            this.txtName.TabIndex = 1;

            // cmbRole
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Location = new System.Drawing.Point(218, 67);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(150, 21);
            this.cmbRole.TabIndex = 2;

            // cmbAttribute
            this.cmbAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAttribute.FormattingEnabled = true;
            this.cmbAttribute.Location = new System.Drawing.Point(218, 106);
            this.cmbAttribute.Name = "cmbAttribute";
            this.cmbAttribute.Size = new System.Drawing.Size(150, 21);
            this.cmbAttribute.TabIndex = 3;

            // numComplexity
            this.numComplexity.Location = new System.Drawing.Point(218, 145);
            this.numComplexity.Maximum = new decimal(new int[] { 3, 0, 0, 0 });
            this.numComplexity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numComplexity.Name = "numComplexity";
            this.numComplexity.Size = new System.Drawing.Size(60, 20);
            this.numComplexity.TabIndex = 4;
            this.numComplexity.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // cmbSearchRole
            this.cmbSearchRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchRole.FormattingEnabled = true;
            this.cmbSearchRole.Location = new System.Drawing.Point(12, 285);
            this.cmbSearchRole.Name = "cmbSearchRole";
            this.cmbSearchRole.Size = new System.Drawing.Size(120, 21);
            this.cmbSearchRole.TabIndex = 5;

            // btnCreate
            this.btnCreate.Location = new System.Drawing.Point(218, 171);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 6;
            this.btnCreate.Text = "Создать";
            this.btnCreate.UseVisualStyleBackColor = true;

            // btnUpdate
            this.btnUpdate.Location = new System.Drawing.Point(218, 200);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 7;
            this.btnUpdate.Text = "Обновить";
            this.btnUpdate.UseVisualStyleBackColor = true;

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(218, 229);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(12, 243);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 23);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "Обновить список";
            this.btnRefresh.UseVisualStyleBackColor = true;

            // btnFindByRole
            this.btnFindByRole.Location = new System.Drawing.Point(138, 283);
            this.btnFindByRole.Name = "btnFindByRole";
            this.btnFindByRole.Size = new System.Drawing.Size(100, 23);
            this.btnFindByRole.TabIndex = 10;
            this.btnFindByRole.Text = "Найти по роли";
            this.btnFindByRole.UseVisualStyleBackColor = true;

            // btnGroupByAttribute
            this.btnGroupByAttribute.Location = new System.Drawing.Point(12, 312);
            this.btnGroupByAttribute.Name = "btnGroupByAttribute";
            this.btnGroupByAttribute.Size = new System.Drawing.Size(180, 23);
            this.btnGroupByAttribute.TabIndex = 11;
            this.btnGroupByAttribute.Text = "Группировать по атрибуту";
            this.btnGroupByAttribute.UseVisualStyleBackColor = true;

            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(215, 12);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(32, 13);
            this.lblName.TabIndex = 12;
            this.lblName.Text = "Имя:";

            // lblRole
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(215, 51);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(35, 13);
            this.lblRole.TabIndex = 13;
            this.lblRole.Text = "Роль:";

            // lblAttribute
            this.lblAttribute.AutoSize = true;
            this.lblAttribute.Location = new System.Drawing.Point(215, 90);
            this.lblAttribute.Name = "lblAttribute";
            this.lblAttribute.Size = new System.Drawing.Size(53, 13);
            this.lblAttribute.TabIndex = 14;
            this.lblAttribute.Text = "Атрибут:";

            // lblComplexity
            this.lblComplexity.AutoSize = true;
            this.lblComplexity.Location = new System.Drawing.Point(215, 129);
            this.lblComplexity.Name = "lblComplexity";
            this.lblComplexity.Size = new System.Drawing.Size(65, 13);
            this.lblComplexity.TabIndex = 15;
            this.lblComplexity.Text = "Сложность:";

            // lblSearchRole
            this.lblSearchRole.AutoSize = true;
            this.lblSearchRole.Location = new System.Drawing.Point(12, 269);
            this.lblSearchRole.Name = "lblSearchRole";
            this.lblSearchRole.Size = new System.Drawing.Size(79, 13);
            this.lblSearchRole.TabIndex = 16;
            this.lblSearchRole.Text = "Поиск по роли:";

            // btnFirstPage
            this.btnFirstPage.Location = new System.Drawing.Point(12, 340);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(70, 23);
            this.btnFirstPage.TabIndex = 17;
            this.btnFirstPage.Text = "« Первая";
            this.btnFirstPage.UseVisualStyleBackColor = true;

            // btnPrevPage
            this.btnPrevPage.Location = new System.Drawing.Point(85, 340);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(70, 23);
            this.btnPrevPage.TabIndex = 18;
            this.btnPrevPage.Text = "< Назад";
            this.btnPrevPage.UseVisualStyleBackColor = true;

            // btnNextPage
            this.btnNextPage.Location = new System.Drawing.Point(250, 340);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(70, 23);
            this.btnNextPage.TabIndex = 19;
            this.btnNextPage.Text = "Вперед >";
            this.btnNextPage.UseVisualStyleBackColor = true;

            // btnLastPage
            this.btnLastPage.Location = new System.Drawing.Point(323, 340);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(80, 23);
            this.btnLastPage.TabIndex = 20;
            this.btnLastPage.Text = "Последняя »";
            this.btnLastPage.UseVisualStyleBackColor = true;

            // lblPageInfo
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPageInfo.Location = new System.Drawing.Point(160, 345);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(84, 13);
            this.lblPageInfo.TabIndex = 21;
            this.lblPageInfo.Text = "Страница 1/1";

            // lblTotalPages
            this.lblTotalPages.AutoSize = true;
            this.lblTotalPages.Location = new System.Drawing.Point(12, 368);
            this.lblTotalPages.Name = "lblTotalPages";
            this.lblTotalPages.Size = new System.Drawing.Size(85, 13);
            this.lblTotalPages.TabIndex = 22;
            this.lblTotalPages.Text = "Всего героев: 0";

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 390);
            this.Controls.Add(this.lblTotalPages);
            this.Controls.Add(this.lblPageInfo);
            this.Controls.Add(this.btnLastPage);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnPrevPage);
            this.Controls.Add(this.btnFirstPage);
            this.Controls.Add(this.lblSearchRole);
            this.Controls.Add(this.lblComplexity);
            this.Controls.Add(this.lblAttribute);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnGroupByAttribute);
            this.Controls.Add(this.btnFindByRole);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.cmbSearchRole);
            this.Controls.Add(this.numComplexity);
            this.Controls.Add(this.cmbAttribute);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lstHeroes);
            this.Name = "MainForm";
            this.Text = "DOTA 2 Hero Manager (MVP)";
            ((System.ComponentModel.ISupportInitialize)(this.numComplexity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox lstHeroes;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.ComboBox cmbAttribute;
        private System.Windows.Forms.NumericUpDown numComplexity;
        private System.Windows.Forms.ComboBox cmbSearchRole;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnFindByRole;
        private System.Windows.Forms.Button btnGroupByAttribute;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Label lblAttribute;
        private System.Windows.Forms.Label lblComplexity;
        private System.Windows.Forms.Label lblSearchRole;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Label lblTotalPages;
    }
}