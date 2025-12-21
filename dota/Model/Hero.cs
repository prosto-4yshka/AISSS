using DataAccessLayer;
using Shared;

namespace Model
{
    public class Hero : IDomainObject, IHero
    {
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

        public override string ToString()
        {
            return $"{Name} ({Role})";
        }

        // Конвертер из DomainEntity (DataAccessLayer) в Hero
        public static Hero FromDomainEntity(DataAccessLayer.DomainEntity entity)
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

        // Конвертер из Hero в DomainEntity
        public DataAccessLayer.DomainEntity ToDomainEntity()
        {
            return new DataAccessLayer.DomainEntity
            {
                Id = this.Id,
                Name = this.Name,
                Role = this.Role,
                Attribute = this.Attribute,
                Complexity = this.Complexity
            };
        }
    }
}