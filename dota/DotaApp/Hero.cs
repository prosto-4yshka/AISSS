using System;

namespace DotaApp
{
    public class Hero
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Attribute { get; set; }
        public int Complexity { get; set; }

        public Hero(int id, string name, string role, string attribute, int complexity)
        {
            Id = id;
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
}