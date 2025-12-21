namespace Shared
{
    public interface IHero
    {
        int Id { get; set; }
        string Name { get; set; }
        string Role { get; set; }
        string Attribute { get; set; }
        int Complexity { get; set; }
    }
}