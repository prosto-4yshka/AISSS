using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataAccessLayer;

namespace DotaApp
{
    public partial class MainForm : Form
    {
        private DotaLogic dotaLogic;
        private ListBox lstHeroes;
        private TextBox txtName;
        private ComboBox cmbRole;
        private ComboBox cmbAttribute;
        private NumericUpDown numComplexity;
        private ComboBox cmbSearchRole;
        private Button btnCreate;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnRefresh;
        private Button btnFindByRole;
        private Button btnGroupByAttribute;
        private Label lblName;
        private Label lblRole;
        private Label lblAttribute;
        private Label lblComplexity;
        private Label lblSearchRole;

        // КОНТРОЛЫ ПАГИНАЦИИ (только навигация)
        private Button btnFirstPage;
        private Button btnPrevPage;
        private Button btnNextPage;
        private Button btnLastPage;
        private Label lblPageInfo;
        private Label lblTotalPages;

        // Фиксированный размер страницы
        private const int PAGE_SIZE = 3;

        public MainForm()
        {
            SetupForm();
            dotaLogic = new DotaLogic();
            LoadAttributes();
            LoadRoles();
            RefreshHeroesList();
        }

        private void SetupForm()
        {
            this.Text = "DOTA 2 Hero Manager (3 героя на странице)";
            this.Size = new Size(400, 420);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Существующие контролы
            lstHeroes = new ListBox();
            lstHeroes.Size = new Size(200, 225);
            lstHeroes.Location = new Point(12, 12);
            lstHeroes.SelectedIndexChanged += lstHeroes_SelectedIndexChanged;

            txtName = new TextBox();
            txtName.Size = new Size(150, 20);
            txtName.Location = new Point(218, 28);

            cmbRole = new ComboBox();
            cmbRole.Size = new Size(150, 21);
            cmbRole.Location = new Point(218, 67);
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbAttribute = new ComboBox();
            cmbAttribute.Size = new Size(150, 21);
            cmbAttribute.Location = new Point(218, 106);
            cmbAttribute.DropDownStyle = ComboBoxStyle.DropDownList;

            numComplexity = new NumericUpDown();
            numComplexity.Size = new Size(60, 20);
            numComplexity.Location = new Point(218, 145);
            numComplexity.Minimum = 1;
            numComplexity.Maximum = 3;
            numComplexity.Value = 1;

            cmbSearchRole = new ComboBox();
            cmbSearchRole.Size = new Size(120, 21);
            cmbSearchRole.Location = new Point(12, 285);
            cmbSearchRole.DropDownStyle = ComboBoxStyle.DropDownList;

            btnCreate = new Button();
            btnCreate.Text = "Создать";
            btnCreate.Size = new Size(75, 23);
            btnCreate.Location = new Point(218, 171);
            btnCreate.Click += btnCreate_Click;

            btnUpdate = new Button();
            btnUpdate.Text = "Обновить";
            btnUpdate.Size = new Size(75, 23);
            btnUpdate.Location = new Point(218, 200);
            btnUpdate.Click += btnUpdate_Click;

            btnDelete = new Button();
            btnDelete.Text = "Удалить";
            btnDelete.Size = new Size(75, 23);
            btnDelete.Location = new Point(218, 229);
            btnDelete.Click += btnDelete_Click;

            btnRefresh = new Button();
            btnRefresh.Text = "Обновить список";
            btnRefresh.Size = new Size(120, 23);
            btnRefresh.Location = new Point(12, 243);
            btnRefresh.Click += btnRefresh_Click;

            btnFindByRole = new Button();
            btnFindByRole.Text = "Найти по роли";
            btnFindByRole.Size = new Size(100, 23);
            btnFindByRole.Location = new Point(138, 283);
            btnFindByRole.Click += btnFindByRole_Click;

            btnGroupByAttribute = new Button();
            btnGroupByAttribute.Text = "Группировать по атрибуту";
            btnGroupByAttribute.Size = new Size(180, 23);
            btnGroupByAttribute.Location = new Point(12, 312);
            btnGroupByAttribute.Click += btnGroupByAttribute_Click;

            lblName = new Label();
            lblName.Text = "Имя:";
            lblName.Location = new Point(215, 12);
            lblName.AutoSize = true;

            lblRole = new Label();
            lblRole.Text = "Роль:";
            lblRole.Location = new Point(215, 51);
            lblRole.AutoSize = true;

            lblAttribute = new Label();
            lblAttribute.Text = "Атрибут:";
            lblAttribute.Location = new Point(215, 90);
            lblAttribute.AutoSize = true;

            lblComplexity = new Label();
            lblComplexity.Text = "Сложность:";
            lblComplexity.Location = new Point(215, 129);
            lblComplexity.AutoSize = true;

            lblSearchRole = new Label();
            lblSearchRole.Text = "Поиск по роли:";
            lblSearchRole.Location = new Point(12, 269);
            lblSearchRole.AutoSize = true;

            // КОНТРОЛЫ ПАГИНАЦИИ (только кнопки навигации)

            // Кнопки навигации
            btnFirstPage = new Button();
            btnFirstPage.Text = "« Первая";
            btnFirstPage.Size = new Size(70, 23);
            btnFirstPage.Location = new Point(12, 340);
            btnFirstPage.Click += (s, e) => GoToPage(1);

            btnPrevPage = new Button();
            btnPrevPage.Text = "< Назад";
            btnPrevPage.Size = new Size(70, 23);
            btnPrevPage.Location = new Point(85, 340);
            btnPrevPage.Click += (s, e) => GoToPage(GetCurrentPage() - 1);

            // Информация о странице
            lblPageInfo = new Label();
            lblPageInfo.Text = "Страница 1 из 1";
            lblPageInfo.Location = new Point(160, 344);
            lblPageInfo.AutoSize = true;
            lblPageInfo.Font = new Font(lblPageInfo.Font, FontStyle.Bold);

            btnNextPage = new Button();
            btnNextPage.Text = "Вперед >";
            btnNextPage.Size = new Size(70, 23);
            btnNextPage.Location = new Point(250, 340);
            btnNextPage.Click += (s, e) => GoToPage(GetCurrentPage() + 1);

            btnLastPage = new Button();
            btnLastPage.Text = "Последняя »";
            btnLastPage.Size = new Size(80, 23);
            btnLastPage.Location = new Point(323, 340);
            btnLastPage.Click += (s, e) => GoToPage(GetTotalPages());

            // Общее количество записей
            lblTotalPages = new Label();
            lblTotalPages.Text = "Всего героев: 0";
            lblTotalPages.Location = new Point(12, 368);
            lblTotalPages.AutoSize = true;

            // Добавляем все контролы на форму
            this.Controls.Add(lstHeroes);
            this.Controls.Add(txtName);
            this.Controls.Add(cmbRole);
            this.Controls.Add(cmbAttribute);
            this.Controls.Add(numComplexity);
            this.Controls.Add(cmbSearchRole);
            this.Controls.Add(btnCreate);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);
            this.Controls.Add(btnRefresh);
            this.Controls.Add(btnFindByRole);
            this.Controls.Add(btnGroupByAttribute);
            this.Controls.Add(lblName);
            this.Controls.Add(lblRole);
            this.Controls.Add(lblAttribute);
            this.Controls.Add(lblComplexity);
            this.Controls.Add(lblSearchRole);

            // Добавляем контролы пагинации
            this.Controls.Add(btnFirstPage);
            this.Controls.Add(btnPrevPage);
            this.Controls.Add(lblPageInfo);
            this.Controls.Add(btnNextPage);
            this.Controls.Add(btnLastPage);
            this.Controls.Add(lblTotalPages);
        }

        // Получить текущую страницу (для внутреннего использования)
        private int GetCurrentPage()
        {
            // Так как у нас нет NumericUpDown для номера страницы,
            // будем хранить текущую страницу в переменной
            // Или можно использовать Tag контрола
            if (lblPageInfo.Tag == null)
                return 1;
            return (int)lblPageInfo.Tag;
        }

        // Установить текущую страницу
        private void SetCurrentPage(int page)
        {
            lblPageInfo.Tag = page;
        }

        private void RefreshHeroesList()
        {
            try
            {
                var currentPage = GetCurrentPage();

                // Получаем данные для текущей страницы (фиксированно 3 героя)
                var heroes = dotaLogic.GetHeroesPage(currentPage, PAGE_SIZE);

                // Обновляем ListBox
                lstHeroes.DataSource = null;
                lstHeroes.DataSource = heroes;
                lstHeroes.DisplayMember = "Name";

                // Обновляем информацию о пагинации
                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка");
            }
        }

        private void UpdatePaginationInfo()
        {
            var currentPage = GetCurrentPage();
            var totalPages = GetTotalPages();

            // Обновляем информацию
            lblPageInfo.Text = $"Страница {currentPage} из {totalPages}";

            // Общее количество записей
            var totalHeroes = dotaLogic.GetTotalHeroesCount();
            lblTotalPages.Text = $"Всего героев: {totalHeroes}";

            // Показываем сколько героев на текущей странице
            var heroesOnPage = dotaLogic.GetHeroesPage(currentPage, PAGE_SIZE).Count;
            lblTotalPages.Text += $" (на странице: {heroesOnPage} из {PAGE_SIZE})";

            // Блокируем/разблокируем кнопки навигации
            btnFirstPage.Enabled = currentPage > 1;
            btnPrevPage.Enabled = currentPage > 1;
            btnNextPage.Enabled = currentPage < totalPages;
            btnLastPage.Enabled = currentPage < totalPages;
        }

        private void GoToPage(int pageNumber)
        {
            if (pageNumber < 1) pageNumber = 1;

            var totalPages = GetTotalPages();
            if (pageNumber > totalPages) pageNumber = totalPages;

            SetCurrentPage(pageNumber);
            RefreshHeroesList();
        }

        private int GetTotalPages()
        {
            return dotaLogic.GetTotalPages(PAGE_SIZE);
        }

        // МЕТОД СОЗДАНИЯ ГЕРОЯ С АВТОМАТИЧЕСКОЙ ПАГИНАЦИЕЙ
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                // Создаем героя
                var hero = dotaLogic.CreateHero(
                    txtName.Text,
                    cmbRole.Text,
                    cmbAttribute.Text,
                    (int)numComplexity.Value
                );

                MessageBox.Show($"Создан герой: {hero.Name} (ID: {hero.Id})", "Успех");

                // Получаем текущее количество героев
                var totalHeroes = dotaLogic.GetTotalHeroesCount();

                // Рассчитываем номер последней страницы (3 героя на странице)
                int lastPage = (int)Math.Ceiling((double)totalHeroes / PAGE_SIZE);

                // Переходим на последнюю страницу (где теперь находится новый герой)
                GoToPage(lastPage);

                ClearFields();
            }
        }

        // ОСТАЛЬНЫЕ МЕТОДЫ БЕЗ ИЗМЕНЕНИЙ

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lstHeroes.SelectedItem is Hero selectedHero)
            {
                if (ValidateInput())
                {
                    if (dotaLogic.UpdateHero(
                        selectedHero.Id,
                        txtName.Text,
                        cmbRole.Text,
                        cmbAttribute.Text,
                        (int)numComplexity.Value
                    ))
                    {
                        MessageBox.Show("Герой обновлен!", "Успех");
                        RefreshHeroesList();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите героя для обновления!", "Ошибка");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstHeroes.SelectedItem is Hero selectedHero)
            {
                if (dotaLogic.DeleteHero(selectedHero.Id))
                {
                    MessageBox.Show("Герой удален!", "Успех");
                    RefreshHeroesList();
                    ClearFields();
                }
            }
            else
            {
                MessageBox.Show("Выберите героя для удаления!", "Ошибка");
            }
        }

        private void btnGroupByAttribute_Click(object sender, EventArgs e)
        {
            var groups = dotaLogic.GroupByAttribute();
            string result = "Герои сгруппированы по атрибуту:\n\n";

            foreach (var group in groups)
            {
                result += $"{group.Key}:\n";
                foreach (var hero in group.Value)
                {
                    result += $"  • {hero.Name} ({hero.Role})\n";
                }
                result += "\n";
            }

            MessageBox.Show(result, "Группировка по атрибуту");
        }

        private void btnFindByRole_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbSearchRole.Text))
            {
                var heroes = dotaLogic.FindByRole(cmbSearchRole.Text);
                string result = $"Найдено {heroes.Count} героев с ролью '{cmbSearchRole.Text}':\n\n";

                foreach (var hero in heroes)
                {
                    result += $"• {hero.Name} | {hero.Attribute} | Сложность: {hero.Complexity}\n";
                }

                MessageBox.Show(result, "Поиск по роли");
            }
        }

        private void lstHeroes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstHeroes.SelectedItem is Hero selectedHero)
            {
                txtName.Text = selectedHero.Name;
                cmbRole.Text = selectedHero.Role;
                cmbAttribute.Text = selectedHero.Attribute;
                numComplexity.Value = selectedHero.Complexity;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshHeroesList();
        }

        private void LoadAttributes()
        {
            var attributes = new[] { "Strength", "Agility", "Intelligence" };
            cmbAttribute.DataSource = attributes;
        }

        private void LoadRoles()
        {
            var roles = new[] { "Carry", "Support", "Mid", "Harder", "Hard support", "Lesnik" };
            cmbRole.DataSource = roles;
            cmbSearchRole.DataSource = roles.ToList();
        }

        private void ClearFields()
        {
            txtName.Text = "";
            if (cmbRole.Items.Count > 0) cmbRole.SelectedIndex = 0;
            if (cmbAttribute.Items.Count > 0) cmbAttribute.SelectedIndex = 0;
            numComplexity.Value = 1;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите имя героя!", "Ошибка");
                return false;
            }
            return true;
        }
    }
}