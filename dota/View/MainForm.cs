using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Model;
using Presenter;
using Shared;

namespace View
{
    public partial class MainForm : Form, IView
    {
        private DotaPresenter _presenter;

        public event Action OnCreateHero;
        public event Action OnUpdateHero;
        public event Action OnDeleteHero;
        public event Action OnRefresh;
        public event Action OnFindByRole;
        public event Action OnGroupByAttribute;
        public event Action<int> OnPageChanged;

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 3;

        public MainForm()
        {
            InitializeComponent();
            SetupForm();

            // Инициализируем репозиторий в Hero
            var repository = DataAccessLayerHelper.GetRepository();
            Hero.InitializeRepository(repository);

            // Создаем презентер (теперь модель - это Hero)
            _presenter = new DotaPresenter(this, new Hero());
        }

        private void SetupForm()
        {
            LoadAttributes();
            LoadRoles();

            btnCreate.Click += (s, e) => OnCreateHero?.Invoke();
            btnUpdate.Click += (s, e) => OnUpdateHero?.Invoke();
            btnDelete.Click += (s, e) => OnDeleteHero?.Invoke();
            btnRefresh.Click += (s, e) => OnRefresh?.Invoke();
            btnFindByRole.Click += (s, e) => OnFindByRole?.Invoke();
            btnGroupByAttribute.Click += (s, e) => OnGroupByAttribute?.Invoke();
            btnFirstPage.Click += (s, e) => OnPageChanged?.Invoke(1);
            btnPrevPage.Click += (s, e) => OnPageChanged?.Invoke(CurrentPage - 1);
            btnNextPage.Click += (s, e) => OnPageChanged?.Invoke(CurrentPage + 1);
            btnLastPage.Click += (s, e) => OnPageChanged?.Invoke(999);

            lstHeroes.SelectedIndexChanged += (s, e) =>
            {
                if (lstHeroes.SelectedItem is Hero selectedHero)
                {
                    txtName.Text = selectedHero.Name;
                    cmbRole.Text = selectedHero.Role;
                    cmbAttribute.Text = selectedHero.Attribute;
                    numComplexity.Value = selectedHero.Complexity;
                }
            };
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
            cmbSearchRole.DataSource = roles.Clone();
        }

        public void ShowHeroes(List<IHero> heroes)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ShowHeroes(heroes)));
                return;
            }

            lstHeroes.DataSource = null;
            lstHeroes.DataSource = heroes;
            lstHeroes.DisplayMember = "Name";
        }

        public void ShowMessage(string message, string title = "Информация")
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ShowMessage(message, title)));
                return;
            }

            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowError(string errorMessage)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ShowError(errorMessage)));
                return;
            }

            MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public string GetHeroName() => txtName.Text;
        public string GetHeroRole() => cmbRole.Text;
        public string GetHeroAttribute() => cmbAttribute.Text;
        public int GetHeroComplexity() => (int)numComplexity.Value;

        public string GetSearchRole()
        {
            if (cmbSearchRole.SelectedItem != null)
                return cmbSearchRole.SelectedItem.ToString();
            return string.Empty;
        }

        public int GetSelectedHeroId()
        {
            if (lstHeroes.SelectedItem is Hero hero)
                return hero.Id;
            return 0;
        }

        public void ClearInputFields()
        {
            txtName.Text = "";
            if (cmbRole.Items.Count > 0) cmbRole.SelectedIndex = 0;
            if (cmbAttribute.Items.Count > 0) cmbAttribute.SelectedIndex = 0;
            numComplexity.Value = 1;
        }

        public void SelectHeroInList(int heroId)
        {
            for (int i = 0; i < lstHeroes.Items.Count; i++)
            {
                if (lstHeroes.Items[i] is Hero hero && hero.Id == heroId)
                {
                    lstHeroes.SelectedIndex = i;
                    break;
                }
            }
        }

        public void UpdatePaginationInfo(int currentPage, int totalPages, int totalHeroes, int heroesOnPage)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdatePaginationInfo(currentPage, totalPages, totalHeroes, heroesOnPage)));
                return;
            }

            lblPageInfo.Text = $"Страница {currentPage} из {totalPages}";
            lblTotalPages.Text = $"Всего героев: {totalHeroes} (на странице: {heroesOnPage})";

            btnFirstPage.Enabled = currentPage > 1;
            btnPrevPage.Enabled = currentPage > 1;
            btnNextPage.Enabled = currentPage < totalPages;
            btnLastPage.Enabled = currentPage < totalPages;

            CurrentPage = currentPage;
        }
    }
}