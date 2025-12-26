using System;
using System.Collections.Generic;

namespace Shared
{
    public interface IView
    {
        // Методы для обновления UI
        void ShowHeroes(List<IHero> heroes);
        void ShowMessage(string message, string title = "Информация");
        void ShowError(string errorMessage);

        // Методы для получения данных из формы
        string GetHeroName();
        string GetHeroRole();
        string GetHeroAttribute();
        int GetHeroComplexity();
        int GetSelectedHeroId();
        string GetSearchRole();

        // Методы для очистки/сброса формы
        void ClearInputFields();
        void SelectHeroInList(int heroId);

        // Методы для обновления информации о пагинации
        void UpdatePaginationInfo(int currentPage, int totalPages, int totalHeroes, int heroesOnPage);

        // Свойства для пагинации
        int CurrentPage { get; set; }
        int PageSize { get; set; }

        // События (делегаты)
        event Action OnCreateHero;
        event Action OnUpdateHero;
        event Action OnDeleteHero;
        event Action OnRefresh;
        event Action OnFindByRole;
        event Action OnGroupByAttribute;
        event Action<int> OnPageChanged;
    }
}