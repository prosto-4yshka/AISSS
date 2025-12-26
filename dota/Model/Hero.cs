using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using Shared;

namespace Model
{
    public class Hero : IHero, IModel, IDomainObject
    {
        private static IRepository<DomainEntity> _repository;

        public event Action<string> OnError;
        public event Action<string> OnMessage;
        public event Action OnDataChanged;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Attribute { get; set; }
        public int Complexity { get; set; }

        public Hero() { }

        public Hero(string name, string role, string attribute, int complexity)
        {
            Name = name;
            Role = role;
            Attribute = attribute;
            Complexity = complexity;
        }

        public static void InitializeRepository(IRepository<DomainEntity> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Имя героя не может быть пустым");

            if (string.IsNullOrWhiteSpace(Role))
                throw new ArgumentException("Роль не может быть пустой");

            if (string.IsNullOrWhiteSpace(Attribute))
                throw new ArgumentException("Атрибут не может быть пустым");

            if (Complexity < 1 || Complexity > 3)
                throw new ArgumentException("Сложность должна быть от 1 до 3");
        }

        public IHero CreateHero(string name, string role, string attribute, int complexity)
        {
            try
            {
                var hero = new Hero(name, role, attribute, complexity);
                hero.Validate();

                var entity = hero.ToDomainEntity();
                _repository.Add(entity);
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

                var hero = FromDomainEntity(entity);
                hero.Name = name;
                hero.Role = role;
                hero.Attribute = attribute;
                hero.Complexity = complexity;
                hero.Validate();

                var updatedEntity = hero.ToDomainEntity();
                entity.Name = updatedEntity.Name;
                entity.Role = updatedEntity.Role;
                entity.Attribute = updatedEntity.Attribute;
                entity.Complexity = updatedEntity.Complexity;

                _repository.Update(entity);
                OnDataChanged?.Invoke();
                OnMessage?.Invoke($"Герой {hero.Name} обновлен.");
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

                var hero = FromDomainEntity(entity);
                _repository.Delete(id);
                OnDataChanged?.Invoke();
                OnMessage?.Invoke($"Герой {hero.Name} удален.");
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
                return entities.Select(FromDomainEntity).Cast<IHero>().ToList();
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
                return FromDomainEntity(entity);
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

                return entities.Select(FromDomainEntity).Cast<IHero>().ToList();
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
                return entities
                    .GroupBy(h => h.Attribute)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(FromDomainEntity).Cast<IHero>().ToList()
                    );
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

                return entities.Select(FromDomainEntity).Cast<IHero>().ToList();
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
                return entities.Select(FromDomainEntity).Cast<IHero>().ToList();
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

        public static Hero FromDomainEntity(DomainEntity entity)
        {
            if (entity == null) return null;

            return new Hero
            {
                Id = entity.Id,
                Name = entity.Name,
                Role = entity.Role,
                Attribute = entity.Attribute,
                Complexity = entity.Complexity
            };
        }

        public DomainEntity ToDomainEntity()
        {
            return new DomainEntity
            {
                Id = this.Id,
                Name = this.Name,
                Role = this.Role,
                Attribute = this.Attribute,
                Complexity = this.Complexity
            };
        }

        public override string ToString()
        {
            return $"{Name} ({Role})";
        }

        public string GetDescription()
        {
            return $"{Name} - {Role} ({Attribute}), Сложность: {Complexity}/3";
        }
    }
}