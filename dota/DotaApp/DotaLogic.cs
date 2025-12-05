using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DotaApp
{
    public class DotaLogic
    {
        private readonly IRepository<Hero> repository;

        public DotaLogic()
        {
            repository = new DapperRepository<Hero>();
        }

        public DotaLogic(IRepository<Hero> customRepository)
        {
            repository = customRepository;
        }

        public Hero CreateHero(string name, string role, string attribute, int complexity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя героя не может быть пустым");

            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Роль не может быть пустой");

            if (string.IsNullOrWhiteSpace(attribute))
                throw new ArgumentException("Атрибут не может быть пустым");

            if (complexity < 1 || complexity > 3)
                throw new ArgumentException("Сложность должна быть от 1 до 3");

            var hero = new Hero
            {
                Name = name.Trim(),
                Role = role.Trim(),
                Attribute = attribute.Trim(),
                Complexity = complexity
            };

            repository.Add(hero);
            return hero;
        }

        public bool UpdateHero(int id, string name, string role, string attribute, int complexity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя героя не может быть пустым");

            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Роль не может быть пустой");

            if (string.IsNullOrWhiteSpace(attribute))
                throw new ArgumentException("Атрибут не может быть пустым");

            if (complexity < 1 || complexity > 3)
                throw new ArgumentException("Сложность должна быть от 1 до 3");

            var hero = repository.GetById(id);
            if (hero == null)
                return false;

            hero.Name = name.Trim();
            hero.Role = role.Trim();
            hero.Attribute = attribute.Trim();
            hero.Complexity = complexity;

            repository.Update(hero);
            return true;
        }

        public bool DeleteHero(int id)
        {
            var hero = repository.GetById(id);
            if (hero == null)
                return false;

            repository.Delete(id);
            return true;
        }

        public List<Hero> GetAllHeroes()
        {
            try
            {
                return repository.GetAll().ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Ошибка при получении списка героев", ex);
            }
        }

        public Hero GetHeroById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID должен быть положительным числом");

            return repository.GetById(id);
        }

        public List<Hero> FindByRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Роль для поиска не может быть пустой");

            var allHeroes = GetAllHeroes();
            return allHeroes
                .Where(h => h.Role.Contains(role, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Hero> FindByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя для поиска не может быть пустым");

            var allHeroes = GetAllHeroes();
            return allHeroes
                .Where(h => h.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Dictionary<string, List<Hero>> GroupByAttribute()
        {
            var allHeroes = GetAllHeroes();
            return allHeroes
                .GroupBy(h => h.Attribute)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<Hero> GetByComplexity(int complexity)
        {
            if (complexity < 1 || complexity > 3)
                throw new ArgumentException("Сложность должна быть от 1 до 3");

            var allHeroes = GetAllHeroes();
            return allHeroes
                .Where(h => h.Complexity == complexity)
                .ToList();
        }

        public int GetHeroesCount()
        {
            return GetAllHeroes().Count;
        }

        public bool HeroExists(int id)
        {
            return repository.GetById(id) != null;
        }

        public void ClearAllHeroes()
        {
            var allHeroes = GetAllHeroes();
            foreach (var hero in allHeroes)
            {
                repository.Delete(hero.Id);
            }
        }
    }
}
