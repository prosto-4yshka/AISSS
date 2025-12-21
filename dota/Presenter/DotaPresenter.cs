using System;
using System.Collections.Generic;
using System.Linq;
using Shared;

namespace Presenter
{
    public class DotaPresenter
    {
        private readonly IView _view;
        private readonly IModel _model;
        private int _currentTotalPages = 1;

        public DotaPresenter(IView view, IModel model)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _model = model ?? throw new ArgumentNullException(nameof(model));

            // Подписываемся на события View
            SubscribeToViewEvents();

            // Подписываемся на события Model
            SubscribeToModelEvents();

            // Инициализация
            InitializeView();
        }

        private void SubscribeToViewEvents()
        {
            _view.OnCreateHero += View_OnCreateHero;
            _view.OnUpdateHero += View_OnUpdateHero;
            _view.OnDeleteHero += View_OnDeleteHero;
            _view.OnRefresh += View_OnRefresh;
            _view.OnFindByRole += View_OnFindByRole;
            _view.OnGroupByAttribute += View_OnGroupByAttribute;
            _view.OnPageChanged += View_OnPageChanged;
        }

        private void SubscribeToModelEvents()
        {
            _model.OnError += Model_OnError;
            _model.OnMessage += Model_OnMessage;
            _model.OnDataChanged += Model_OnDataChanged;
        }

        private void InitializeView()
        {
            _view.PageSize = 3;
            UpdateTotalPages();
            LoadHeroesPage(1);
        }

        private void UpdateTotalPages()
        {
            _currentTotalPages = _model.GetTotalPages(_view.PageSize);
            if (_currentTotalPages < 1) _currentTotalPages = 1;
        }

        private void View_OnCreateHero()
        {
            try
            {
                var hero = _model.CreateHero(
                    _view.GetHeroName(),
                    _view.GetHeroRole(),
                    _view.GetHeroAttribute(),
                    _view.GetHeroComplexity()
                );

                _view.ClearInputFields();

                // Переходим на последнюю страницу
                UpdateTotalPages();
                _view.CurrentPage = _currentTotalPages;
                LoadHeroesPage(_currentTotalPages);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void View_OnUpdateHero()
        {
            try
            {
                var heroId = _view.GetSelectedHeroId();
                if (heroId <= 0)
                {
                    _view.ShowError("Выберите героя для обновления!");
                    return;
                }

                var success = _model.UpdateHero(
                    heroId,
                    _view.GetHeroName(),
                    _view.GetHeroRole(),
                    _view.GetHeroAttribute(),
                    _view.GetHeroComplexity()
                );

                if (success)
                {
                    _view.ClearInputFields();
                    LoadHeroesPage(_view.CurrentPage);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void View_OnDeleteHero()
        {
            try
            {
                var heroId = _view.GetSelectedHeroId();
                if (heroId <= 0)
                {
                    _view.ShowError("Выберите героя для удаления!");
                    return;
                }

                var success = _model.DeleteHero(heroId);
                if (success)
                {
                    _view.ClearInputFields();
                    UpdateTotalPages();

                    // Если удалили последнего героя на текущей странице
                    if (_view.CurrentPage > _currentTotalPages)
                    {
                        _view.CurrentPage = _currentTotalPages;
                    }

                    LoadHeroesPage(_view.CurrentPage);
                }
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void View_OnRefresh()
        {
            UpdateTotalPages();
            LoadHeroesPage(_view.CurrentPage);
        }

        private void View_OnFindByRole()
        {
            try
            {
                // Получаем выбранную роль из View
                // Для этого добавим метод в IView
                string searchRole = _view.GetSearchRole();

                if (string.IsNullOrEmpty(searchRole))
                {
                    _view.ShowError("Выберите роль для поиска!");
                    return;
                }

                var heroes = _model.FindByRole(searchRole);

                if (heroes == null || heroes.Count == 0)
                {
                    _view.ShowMessage($"Героев с ролью '{searchRole}' не найдено.", "Результат поиска");
                    return;
                }

                string result = $"Найдено {heroes.Count} героев с ролью '{searchRole}':\n\n";

                foreach (var hero in heroes)
                {
                    result += $"• {hero.Name} | {hero.Attribute} | Сложность: {hero.Complexity}\n";
                }

                _view.ShowMessage(result, "Результат поиска по роли");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void View_OnGroupByAttribute()
        {
            try
            {
                var groups = _model.GroupByAttribute();
                if (groups.Count == 0)
                {
                    _view.ShowMessage("Нет данных для группировки");
                    return;
                }

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

                _view.ShowMessage(result, "Группировка по атрибуту");
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }

        private void View_OnPageChanged(int pageNumber)
        {
            // Проверяем границы
            if (pageNumber < 1) pageNumber = 1;
            if (pageNumber > _currentTotalPages) pageNumber = _currentTotalPages;

            LoadHeroesPage(pageNumber);
        }

        private void LoadHeroesPage(int pageNumber)
        {
            try
            {
                // Еще раз проверяем границы
                if (pageNumber < 1) pageNumber = 1;
                if (pageNumber > _currentTotalPages) pageNumber = _currentTotalPages;

                var heroes = _model.GetHeroesPage(pageNumber, _view.PageSize);
                _view.ShowHeroes(heroes);
                _view.CurrentPage = pageNumber;

                // Обновляем информацию о пагинации
                var totalHeroes = _model.GetTotalHeroesCount();
                _view.UpdatePaginationInfo(pageNumber, _currentTotalPages, totalHeroes, heroes.Count);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка при загрузке страницы: {ex.Message}");
            }
        }

        private void Model_OnError(string errorMessage)
        {
            _view.ShowError(errorMessage);
        }

        private void Model_OnMessage(string message)
        {
            _view.ShowMessage(message);
        }

        private void Model_OnDataChanged()
        {
            UpdateTotalPages();
            LoadHeroesPage(_view.CurrentPage);
        }
    }
}