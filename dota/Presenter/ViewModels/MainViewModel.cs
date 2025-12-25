using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;  
using DotaApp;
using DataAccessLayer;

namespace Presenter.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly DotaLogic _logic;
        private ObservableCollection<HeroDTO> _heroes;
        private HeroDTO _selectedHero;
        private string _name;
        private string _role;
        private string _attribute;
        private int _complexity = 1;
        private string _searchRole;
        private string _statusMessage;
        private int _currentPage = 1;
        private const int PAGE_SIZE = 5;
        private int _totalPages;

        public MainViewModel()
        {
            _logic = DotaApp.Program.GetLogic();

            Heroes = new ObservableCollection<HeroDTO>();
            HeroesView = CollectionViewSource.GetDefaultView(Heroes);

            LoadCommands();
            LoadAttributes();
            LoadRoles();
            RefreshHeroes();
        }

        public RelayCommand CreateCommand { get; private set; }
        public RelayCommand UpdateCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand FindByRoleCommand { get; private set; }
        public RelayCommand GroupByAttributeCommand { get; private set; }
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand PrevPageCommand { get; private set; }

        public ObservableCollection<HeroDTO> Heroes
        {
            get => _heroes;
            set => SetField(ref _heroes, value);
        }

        public ICollectionView HeroesView { get; }

        public HeroDTO SelectedHero
        {
            get => _selectedHero;
            set
            {
                if (SetField(ref _selectedHero, value) && value != null)
                {
                    Name = value.Name;
                    Role = value.Role;
                    Attribute = value.Attribute;
                    Complexity = value.Complexity;
                }
            }
        }

        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        public string Role
        {
            get => _role;
            set => SetField(ref _role, value);
        }

        public string Attribute
        {
            get => _attribute;
            set => SetField(ref _attribute, value);
        }

        public int Complexity
        {
            get => _complexity;
            set => SetField(ref _complexity, value);
        }

        public string SearchRole
        {
            get => _searchRole;
            set => SetField(ref _searchRole, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetField(ref _statusMessage, value);
        }

        public int CurrentPage
        {
            get => _currentPage;
            set => SetField(ref _currentPage, value);
        }

        public int TotalPages
        {
            get => _totalPages;
            set => SetField(ref _totalPages, value);
        }

        public ObservableCollection<string> Roles { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Attributes { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> SearchRoles { get; } = new ObservableCollection<string>();

        private void LoadCommands()
        {
            CreateCommand = new RelayCommand(CreateHero, CanCreateHero);
            UpdateCommand = new RelayCommand(UpdateHero, CanUpdateHero);
            DeleteCommand = new RelayCommand(DeleteHero, CanDeleteHero);
            RefreshCommand = new RelayCommand(RefreshHeroes);
            FindByRoleCommand = new RelayCommand(FindByRole);
            GroupByAttributeCommand = new RelayCommand(GroupByAttribute);
            NextPageCommand = new RelayCommand(NextPage, () => CurrentPage < TotalPages);
            PrevPageCommand = new RelayCommand(PrevPage, () => CurrentPage > 1);
        }

        private void LoadAttributes()
        {
            Attributes.Clear();
            foreach (var attr in new[] { "Strength", "Agility", "Intelligence" })
                Attributes.Add(attr);

            Attribute = Attributes.FirstOrDefault();
        }

        private void LoadRoles()
        {
            var roles = new[] { "Carry", "Support", "Mid", "Harder", "Hard support", "Lesnik" };
            Roles.Clear();
            SearchRoles.Clear();

            foreach (var role in roles)
            {
                Roles.Add(role);
                SearchRoles.Add(role);
            }

            Role = Roles.FirstOrDefault();
            SearchRole = SearchRoles.FirstOrDefault();
        }

        private void RefreshHeroes()
        {
            try
            {
                var heroes = _logic.GetHeroesPage(CurrentPage, PAGE_SIZE);

                Heroes.Clear();
                foreach (var hero in heroes)
                {
                    Heroes.Add(HeroDTO.FromDomain(hero));
                }

                TotalPages = _logic.GetTotalPages(PAGE_SIZE);
                StatusMessage = $"Загружено {Heroes.Count} героев. Страница {CurrentPage} из {TotalPages}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка: {ex.Message}";
            }
        }

        private bool CanCreateHero() => !string.IsNullOrWhiteSpace(Name);
        private bool CanUpdateHero() => SelectedHero != null && !string.IsNullOrWhiteSpace(Name);
        private bool CanDeleteHero() => SelectedHero != null;

        private void CreateHero()
        {
            try
            {
                var hero = _logic.CreateHero(Name, Role, Attribute, Complexity);
                Heroes.Add(HeroDTO.FromDomain(hero));

                StatusMessage = $"Создан герой: {hero.Name} (ID: {hero.Id})";
                ClearFields();
                RefreshHeroes();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка создания: {ex.Message}";
            }
        }

        private void UpdateHero()
        {
            try
            {
                if (_logic.UpdateHero(SelectedHero.Id, Name, Role, Attribute, Complexity))
                {
                    SelectedHero.Name = Name;
                    SelectedHero.Role = Role;
                    SelectedHero.Attribute = Attribute;
                    SelectedHero.Complexity = Complexity;

                    StatusMessage = "Герой обновлен!";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка обновления: {ex.Message}";
            }
        }

        private void DeleteHero()
        {
            try
            {
                if (_logic.DeleteHero(SelectedHero.Id))
                {
                    Heroes.Remove(SelectedHero);
                    StatusMessage = "Герой удален!";
                    ClearFields();
                    RefreshHeroes();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка удаления: {ex.Message}";
            }
        }

        private void FindByRole()
        {
            try
            {
                var heroes = _logic.FindByRole(SearchRole);
                string result = $"Найдено {heroes.Count} героев с ролью '{SearchRole}':\n";

                foreach (var hero in heroes)
                {
                    result += $"• {hero.Name} | {hero.Attribute} | Сложность: {hero.Complexity}\n";
                }

                StatusMessage = result;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка поиска: {ex.Message}";
            }
        }

        private void GroupByAttribute()
        {
            try
            {
                var groups = _logic.GroupByAttribute();
                string result = "Группировка по атрибуту:\n";

                foreach (var group in groups)
                {
                    result += $"\n{group.Key}:\n";
                    foreach (var hero in group.Value)
                    {
                        result += $"  • {hero.Name} ({hero.Role})\n";
                    }
                }

                StatusMessage = result;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка группировки: {ex.Message}";
            }
        }

        private void NextPage()
        {
            CurrentPage++;
            RefreshHeroes();
        }

        private void PrevPage()
        {
            CurrentPage--;
            RefreshHeroes();
        }

        private void ClearFields()
        {
            Name = string.Empty;
            Role = Roles.FirstOrDefault();
            Attribute = Attributes.FirstOrDefault();
            Complexity = 1;
        }
    }
}