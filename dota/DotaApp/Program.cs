
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace DotaApp
{
    class Program
    {
        // Используем один экземпляр DotaLogic для всего приложения
        static DotaLogic dotaLogic = new DotaLogic();

        static void Main(string[] args)
        {
            var initialHeroes = new[]
            {
                dotaLogic.CreateHero("Axe", "Initiator", "Strength", 1),
                dotaLogic.CreateHero("Juggernaut", "Carry", "Agility", 2),
                dotaLogic.CreateHero("Crystal Maiden", "Support", "Intelligence", 1)
            };

            // Запускаем отдельный поток для взаимодействия с формой
            var formThread = new Thread(() =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            });

            formThread.Start();

            while (true)
            {
                Console.WriteLine("\n--- Меню ---");
                Console.WriteLine("1. Показать всех героев");
                Console.WriteLine("2. Создать героя");
                Console.WriteLine("3. Обновить героя");
                Console.WriteLine("4. Удалить героя");
                Console.WriteLine("5. Группировать по атрибуту");
                Console.WriteLine("6. Найти по роли");
                Console.WriteLine("0. Выйти");
                Console.Write("Выберите опцию: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllHeroes();
                        break;
                    case "2":
                        CreateHeroConsole();
                        break;
                    case "3":
                        UpdateHeroConsole();
                        break;
                    case "4":
                        DeleteHeroConsole();
                        break;
                    case "5":
                        GroupByAttributeConsole();
                        break;
                    case "6":
                        FindByRoleConsole();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверная опция");
                        break;
                }
            }
        }

        static void ShowAllHeroes()
        {
            var heroes = ShareData.Instance.GetHeroesSnapshot();
            Console.WriteLine("\nВсе герои:");
            foreach (var hero in heroes)
            {
                Console.WriteLine($"ID: {hero.Id} | {hero.Name} | {hero.Role} | {hero.Attribute} | Сложность: {hero.Complexity}");
            }
        }

        static void CreateHeroConsole()
        {
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите роль: ");
            string role = Console.ReadLine();
            Console.Write("Введите атрибут: ");
            string attribute = Console.ReadLine();
            Console.Write("Введите сложность (1-3): ");
            int complexity = int.Parse(Console.ReadLine());

            var hero = dotaLogic.CreateHero(name, role, attribute, complexity);
            Console.WriteLine($"Создан герой: {hero.Name} (ID: {hero.Id})");
        }

        static void UpdateHeroConsole()
        {
            Console.Write("Введите ID героя для обновления: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Введите новое имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите новую роль: ");
            string role = Console.ReadLine();
            Console.Write("Введите новый атрибут: ");
            string attribute = Console.ReadLine();
            Console.Write("Введите новую сложность (1-3): ");
            int complexity = int.Parse(Console.ReadLine());

            if (dotaLogic.UpdateHero(id, name, role, attribute, complexity))
            {
                Console.WriteLine("Герой обновлен");
            }
            else
            {
                Console.WriteLine("Герой не найден");
            }
        }

        static void DeleteHeroConsole()
        {
            Console.Write("Введите ID героя для удаления: ");
            int id = int.Parse(Console.ReadLine());
            if (dotaLogic.DeleteHero(id))
            {
                Console.WriteLine("Герой удален");
            }
            else
            {
                Console.WriteLine("Герой не найден");
            }
        }

        static void GroupByAttributeConsole()
        {
            var groups = ShareData.Instance.GetHeroesSnapshot()
                .GroupBy(h => h.Attribute)
                .ToDictionary(g => g.Key, g => g.ToList());

            Console.WriteLine("Группировка по атрибуту:");
            foreach (var group in groups)
            {
                Console.WriteLine($"{group.Key}:");
                foreach (var hero in group.Value)
                {
                    Console.WriteLine($"  - {hero.Name} ({hero.Role})");
                }
            }
        }

        static void FindByRoleConsole()
        {
            Console.Write("Введите роль для поиска: ");
            string role = Console.ReadLine();
            var heroes = ShareData.Instance.GetHeroesSnapshot()
                .Where(h => h.Role.Contains(role))
                .ToList();

            Console.WriteLine($"Героев с ролью '{role}':");
            foreach (var hero in heroes)
            {
                Console.WriteLine($"ID: {hero.Id} | {hero.Name} | {hero.Attribute} | Сложность: {hero.Complexity}");
            }
        }
    }
}