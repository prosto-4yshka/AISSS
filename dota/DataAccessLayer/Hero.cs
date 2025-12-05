using DataAccessLayer;

public class Hero : IDomainObject
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public string Attribute { get; set; }
    public int Complexity { get; set; }

    // Для Entity Framework нужен пустой конструктор
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
}