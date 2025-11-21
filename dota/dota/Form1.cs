using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace DotaApp
{
    public partial class MainForm : Form
    {
        private DotaLogic dotaLogic;

        // Элементы формы
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

        public MainForm()
        {
            InitializeComponents();
            dotaLogic = new DotaLogic();



            RefreshHeroesList();
            LoadAttributes();
            LoadRoles();
        }

        private void InitializeComponents()
        {
            // Настройка формы
            this.Text = "DOTA 2 Hero Manager";
            this.Size = new Size(400, 380);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Создание элементов
            CreateControls();
            PositionControls();
            SetupEventHandlers();
        }

        private void CreateControls()
        {
            // ListBox для героев
            lstHeroes = new ListBox();
            lstHeroes.Size = new Size(200, 225);
            lstHeroes.Location = new Point(12, 12);

            // TextBox для имени
            txtName = new TextBox();
            txtName.Size = new Size(150, 20);
            txtName.Location = new Point(218, 28);

            // ComboBox для роли
            cmbRole = new ComboBox();
            cmbRole.Size = new Size(150, 21);
            cmbRole.Location = new Point(218, 67);
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;

            // ComboBox для атрибута
            cmbAttribute = new ComboBox();
            cmbAttribute.Size = new Size(150, 21);
            cmbAttribute.Location = new Point(218, 106);
            cmbAttribute.DropDownStyle = ComboBoxStyle.DropDownList;

            // NumericUpDown для сложности
            numComplexity = new NumericUpDown();
            numComplexity.Size = new Size(60, 20);
            numComplexity.Location = new Point(218, 145);
            numComplexity.Minimum = 1;
            numComplexity.Maximum = 3;
            numComplexity.Value = 1;

            // ComboBox для поиска по роли
            cmbSearchRole = new ComboBox();
            cmbSearchRole.Size = new Size(120, 21);
            cmbSearchRole.Location = new Point(12, 285);
            cmbSearchRole.DropDownStyle = ComboBoxStyle.DropDownList;

            // Кнопки
            btnCreate = new Button();
            btnCreate.Text = "Создать";
            btnCreate.Size = new Size(75, 23);
            btnCreate.Location = new Point(218, 171);

            btnUpdate = new Button();
            btnUpdate.Text = "Обновить";
            btnUpdate.Size = new Size(75, 23);
            btnUpdate.Location = new Point(218, 200);

            btnDelete = new Button();
            btnDelete.Text = "Удалить";
            btnDelete.Size = new Size(75, 23);
            btnDelete.Location = new Point(218, 229);

            btnRefresh = new Button();
            btnRefresh.Text = "Обновить список";
            btnRefresh.Size = new Size(120, 23);
            btnRefresh.Location = new Point(12, 243);

            btnFindByRole = new Button();
            btnFindByRole.Text = "Найти по роли";
            btnFindByRole.Size = new Size(100, 23);
            btnFindByRole.Location = new Point(138, 283);

            btnGroupByAttribute = new Button();
            btnGroupByAttribute.Text = "Группировать по атрибуту";
            btnGroupByAttribute.Size = new Size(180, 23);
            btnGroupByAttribute.Location = new Point(12, 312);

            // Метки
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
        }

        private void PositionControls()
        {
            // Добавление элементов на форму
            this.Controls.AddRange(new Control[] {
                lstHeroes, txtName, cmbRole, cmbAttribute, numComplexity, cmbSearchRole,
                btnCreate, btnUpdate, btnDelete, btnRefresh, btnFindByRole, btnGroupByAttribute,
                lblName, lblRole, lblAttribute, lblComplexity, lblSearchRole
            });
        }

        private void SetupEventHandlers()
        {
            // Назначение обработчиков событий
            btnCreate.Click += btnCreate_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnRefresh.Click += btnRefresh_Click;
            btnFindByRole.Click += btnFindByRole_Click;
            btnGroupByAttribute.Click += btnGroupByAttribute_Click;
            lstHeroes.SelectedIndexChanged += lstHeroes_SelectedIndexChanged;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                var hero = dotaLogic.CreateHero(
                    txtName.Text,
                    cmbRole.Text,
                    cmbAttribute.Text,
                    (int)numComplexity.Value
                );

                MessageBox.Show($"Создан герой: {hero.Name} (ID: {hero.Id})", "Успех");
                RefreshHeroesList();
                ClearFields();
            }
        }

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

        private void RefreshHeroesList()
        {
            var heroes = ShareData.Instance.GetHeroesSnapshot();
            lstHeroes.DataSource = null;
            lstHeroes.DataSource = heroes;
            lstHeroes.DisplayMember = "Name";
        }

        private void LoadAttributes()
        {
            var attributes = new[] { "Strength", "Agility", "Intelligence" };
            cmbAttribute.DataSource = attributes;
        }

        private void LoadRoles()
        {
            var roles = new[] { "Carry", "Support", "Initiator", "Disabler", "Nuker", "Durable" };
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
