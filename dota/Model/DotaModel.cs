using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using Shared;

namespace Model
{
    public class DotaModel : IModel
    {
        private readonly IRepository<DataAccessLayer.DomainEntity> _repository;

        // События модели
        public event Action<string> OnError;
        public event Action<string> OnMessage;
        public event Action OnDataChanged;

        public DotaModel(IRepository<DataAccessLayer.DomainEntity> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        private Hero ConvertToHero(DataAccessLayer.DomainEntity entity)
        {
            return Hero.FromDomainEntity(entity);
        }

        private List<Hero> ConvertToHeroList(IEnumerable<DataAccessLayer.DomainEntity> entities)
        {
            return entities.Select(ConvertToHero).ToList();
        }

        public IHero CreateHero(string name, string role, string attribute, int complexity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Имя героя не может быть пустым");

                var hero = new Hero(name, role, attribute, complexity);
                var entity = hero.ToDomainEntity();

                _repository.Add(entity);

                // Получаем Hero с ID
                hero.Id = entity.Id;

                OnDataChanged?.Invoke();
                OnMessage?.Invoke($"Создан герой: {hero.Name} (ID: {hero.Id})");

                return hero;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Ошибка при создании героя: {ex.Message}");
                throw;
            }
        }

        public bool UpdateHero(int id, string name, string role, string attribute, int complexity)
        {
            try
            {
                var entity = _repository.GetById(id);
                if (entity == null)
                    return false;

                entity.Name = name.Trim();
                entity.Role = role.Trim();
                entity.Attribute = attribute.Trim();
                entity.Complexity = complexity;

                _repository.Update(entity);
                OnDataChanged?.Invoke();
                OnMessage?.Invoke($"Герой {entity.Name} обновлен.");
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Ошибка при обновлении героя: {ex.Message}");
                return false;
            }
        }

        public bool DeleteHero(int id)
        {
            try
            {
                var entity = _repository.GetById(id);
                if (entity == null)
                    return false;

                _repository.Delete(id);
                OnDataChanged?.Invoke();
                OnMessage?.Invoke($"Герой {entity.Name} удален.");
                return true;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Ошибка при удалении героя: {ex.Message}");
                return false;
            }
        }

        public List<IHero> GetAllHeroes()
        {
            try
            {
                var entities = _repository.GetAll();
                return ConvertToHeroList(entities).Cast<IHero>().ToList();
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Ошибка при получении героев: {ex.Message}");
                return new List<IHero>();
            }
        }

        public IHero GetHeroById(int id)
        {
            try
            {
                var entity = _repository.GetById(id);
                return ConvertToHero(entity);
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Ошибка при получении героя: {ex.Message}");
                return null;
            }
        }

        public List<IHero> FindByRole(string role)
        {
            try
            {
                var entities = _repository.GetAll()
                    .Where(h => h.Role.Contains(role, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return ConvertToHeroList(entities).Cast<IHero>().ToList();
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Ошибка при поиске по роли: {ex.Message}");
                return new List<IHero>();
            }
        }

        public Dictionary<string, List<IHero>> GroupByAttribute()
        {
            try
            {
                var entities = _repository.GetAll();
                var groups = entities
                    .GroupBy(h => h.Attribute)
                    .ToDictionary(
                        g => g.Key,
                        g => ConvertToHeroList(g.ToList()).Cast<IHero>().ToList()
                    );

                return groups;
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Ошибка при группировке: {ex.Message}");
                return new Dictionary<string, List<IHero>>();
            }
        }

        public List<IHero> GetByComplexity(int complexity)
        {
            try
            {
                var entities = _repository.GetAll()
                    .Where(h => h.Complexity == complexity)
                    .ToList();

                return ConvertToHeroList(entities).Cast<IHero>().ToList();
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Ошибка при получении по сложности: {ex.Message}");
                return new List<IHero>();
            }
        }

        public List<IHero> GetHeroesPage(int pageNumber, int pageSize)
        {
            try
            {
                var entities = _repository.GetPage(pageNumber, pageSize);
                return ConvertToHeroList(entities).Cast<IHero>().ToList();
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Ошибка при получении страницы: {ex.Message}");
                return new List<IHero>();
            }
        }

        public int GetTotalHeroesCount()
        {
            try
            {
                return _repository.GetTotalCount();
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Ошибка при получении количества: {ex.Message}");
                return 0;
            }
        }

        public int GetTotalPages(int pageSize)
        {
            try
            {
                var total = GetTotalHeroesCount();
                return (int)Math.Ceiling((double)total / pageSize);
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Ошибка при расчете страниц: {ex.Message}");
                return 0;
            }
        }
    }
}