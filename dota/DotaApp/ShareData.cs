using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaApp
{
    public class ShareData
    {
        private static readonly Lazy<ShareData> instance = new Lazy<ShareData>(() => new ShareData());
        private readonly object lockObject = new object();

        public List<Hero> Heroes { get; private set; }
        public int Version { get; private set; }

        private ShareData()
        {
            Heroes = new List<Hero>();
            Version = 0;
        }

        public static ShareData Instance => instance.Value;

        public List<Hero> GetHeroesSnapshot()
        {
            lock (lockObject)
            {
                return new List<Hero>(Heroes);
            }
        }

        public void AddHero(Hero hero)
        {
            lock (lockObject)
            {
                Heroes.Add(hero);
                IncrementVersion();
            }
        }

        public bool UpdateHero(int id, string name, string role, string attribute, int complexity)
        {
            lock (lockObject)
            {
                var hero = Heroes.Find(h => h.Id == id);
                if (hero != null)
                {
                    hero.Name = name;
                    hero.Role = role;
                    hero.Attribute = attribute;
                    hero.Complexity = complexity;
                    IncrementVersion();
                    return true;
                }
                return false;
            }
        }

        public bool DeleteHero(int id)
        {
            lock (lockObject)
            {
                var hero = Heroes.Find(h => h.Id == id);
                if (hero != null)
                {
                    Heroes.Remove(hero);
                    IncrementVersion();
                    return true;
                }
                return false;
            }
        }

        private void IncrementVersion()
        {
            Version++;
        }

        public void LoadHeroes(IEnumerable<Hero> heroes)
        {
            lock (lockObject)
            {
                Heroes.Clear();
                Heroes.AddRange(heroes);
                IncrementVersion();
            }
        }

        public int GetNextId()
        {
            lock (lockObject)
            {
                if (Heroes.Count == 0)
                    return 1;

                return Heroes.Max(h => h.Id) + 1;
            }
        }
    }
}