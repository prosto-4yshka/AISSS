using System;
using System.Collections.Generic;

namespace Shared
{
    public interface IModel
    {
        // Базовые CRUD операции
        IHero CreateHero(string name, string role, string attribute, int complexity);
        bool UpdateHero(int id, string name, string role, string attribute, int complexity);
        bool DeleteHero(int id);
        List<IHero> GetAllHeroes();
        IHero GetHeroById(int id);

        // Специфичные операции
        List<IHero> FindByRole(string role);
        Dictionary<string, List<IHero>> GroupByAttribute();
        List<IHero> GetByComplexity(int complexity);

        // Пагинация
        List<IHero> GetHeroesPage(int pageNumber, int pageSize);
        int GetTotalHeroesCount();
        int GetTotalPages(int pageSize);

        // События модели
        event Action<string> OnError;
        event Action<string> OnMessage;
        event Action OnDataChanged;
    }
}