using System;
using System.Collections.Generic;
using System.Linq;

namespace DotaApp
{
    public class DotaLogic
    {
        public Hero CreateHero(string name, string role, string attribute, int complexity)
        {
            int nextId = ShareData.Instance.GetNextId();
            var hero = new Hero(nextId, name, role, attribute, complexity);
            ShareData.Instance.AddHero(hero);
            return hero;
        }

        public bool DeleteHero(int id)
        {
            return ShareData.Instance.DeleteHero(id);
        }

        public bool UpdateHero(int id, string name, string role, string attribute, int complexity)
        {
            return ShareData.Instance.UpdateHero(id, name, role, attribute, complexity);
        }

        public List<Hero> GetAllHeroes()
        {
            return ShareData.Instance.GetHeroesSnapshot();
        }

        public Dictionary<string, List<Hero>> GroupByAttribute()
        {
            return GetAllHeroes().GroupBy(h => h.Attribute)
                                 .ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<Hero> FindByRole(string role)
        {
            return GetAllHeroes().Where(h => h.Role.Contains(role)).ToList();
        }
    }
}