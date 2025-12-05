using DataAccessLayer;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DotaApp;

namespace DotaApp
{
    class Program
    {
        // Используем DotaLogic с репозиторием
        static DotaLogic dotaLogic = new DotaLogic();

        static void Main(string[] args)
        {
            // Инициализация базы данных (добавляем тестовые данные)
            InitializeDatabase();

            Console.WriteLine("База данных инициализирована.");
            Console.WriteLine("Запуск формы...");

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
                Console.WriteLine("\n--- Консольное меню ---");
                Console.WriteLine("1. Показать всех героев");
                Console.WriteLine("2. Создать героя");
                Console.WriteLine("3. Обновить героя");
                Console.WriteLine("4. Удалить героя");
                Console.WriteLine("5. Группировать по атрибуту");
                Console.WriteLine("6. Найти по роли");
                Console.WriteLine("7. Найти героя по ID");
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
                    case "7":
                        FindByIdConsole();
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Неверная опция");
                        break;
                }
            }
        }

        // Инициализация базы данных
        static void InitializeDatabase()
        {
            try
            {
                // Проверяем, есть ли уже герои в базе
                var existingHeroes = dotaLogic.GetAllHeroes();

                if (!existingHeroes.Any())
                {
                    Console.WriteLine("Создание тестовых героев...");

                    var testHeroes = new[]
                    {
                        dotaLogic.CreateHero("Axe", "Initiator", "Strength", 1),
                        dotaLogic.CreateHero("Juggernaut", "Carry", "Agility", 2),
                        dotaLogic.CreateHero("Crystal Maiden", "Support", "Intelligence", 1)
                    };

                    Console.WriteLine($"Создано {testHeroes.Length} тестовых героев.");
                }
                else
                {
                    Console.WriteLine($"В базе уже есть {existingHeroes.Count} героев.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при инициализации базы данных: {ex.Message}");
                Console.WriteLine("Убедитесь, что база данных создана и доступна.");
            }
        }

        // 1. Показать всех героев (ЗАМЕНА: вместо ShareData.Instance.GetHeroesSnapshot())
        static void ShowAllHeroes()
        {
            var heroes = dotaLogic.GetAllHeroes(); // ЗАМЕНА

            if (!heroes.Any())
            {
                Console.WriteLine("Нет героев в базе данных.");
                return;
            }

            Console.WriteLine("\nВсе герои:");
            Console.WriteLine("=========================================");
            Console.WriteLine("ID | Имя | Роль | Атрибут | Сложность");
            Console.WriteLine("-----------------------------------------");

            foreach (var hero in heroes)
            {
                Console.WriteLine($"{hero.Id,3} | {hero.Name,-15} | {hero.Role,-12} | {hero.Attribute,-12} | {hero.Complexity}");
            }
            Console.WriteLine("=========================================");
            Console.WriteLine($"Всего: {heroes.Count} героев");
        }

        // 2. Создать героя (ОСТАЛОСЬ ТАК ЖЕ)
        static void CreateHeroConsole()
        {
            try
            {
                Console.Write("Введите имя: ");
                string name = Console.ReadLine();

                Console.Write("Введите роль: ");
                string role = Console.ReadLine();

                Console.Write("Введите атрибут (Strength/Agility/Intelligence): ");
                string attribute = Console.ReadLine();

                Console.Write("Введите сложность (1-3): ");
                if (!int.TryParse(Console.ReadLine(), out int complexity) || complexity < 1 || complexity > 3)
                {
                    Console.WriteLine("Ошибка: сложность должна быть от 1 до 3");
                    return;
                }

                var hero = dotaLogic.CreateHero(name, role, attribute, complexity);
                Console.WriteLine($"✓ Создан герой: {hero.Name} (ID: {hero.Id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании героя: {ex.Message}");
            }
        }

        // 3. Обновить героя (ОСТАЛОСЬ ТАК ЖЕ)
        static void UpdateHeroConsole()
        {
            try
            {
                Console.Write("Введите ID героя для обновления: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Ошибка: неверный формат ID");
                    return;
                }

                // Проверяем, существует ли герой
                var existingHero = dotaLogic.GetHeroById(id); // НОВЫЙ МЕТОД
                if (existingHero == null)
                {
                    Console.WriteLine($"Герой с ID {id} не найден.");
                    return;
                }

                Console.WriteLine($"Текущие данные: {existingHero.Name} | {existingHero.Role} | {existingHero.Attribute} | Сложность: {existingHero.Complexity}");

                Console.Write("Введите новое имя (или Enter чтобы оставить текущее): ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                    name = existingHero.Name;

                Console.Write("Введите новую роль (или Enter чтобы оставить текущую): ");
                string role = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(role))
                    role = existingHero.Role;

                Console.Write("Введите новый атрибут (или Enter чтобы оставить текущий): ");
                string attribute = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(attribute))
                    attribute = existingHero.Attribute;

                Console.Write("Введите новую сложность 1-3 (или Enter чтобы оставить текущую): ");
                string complexityInput = Console.ReadLine();
                int complexity;
                if (string.IsNullOrWhiteSpace(complexityInput))
                    complexity = existingHero.Complexity;
                else if (!int.TryParse(complexityInput, out complexity) || complexity < 1 || complexity > 3)
                {
                    Console.WriteLine("Ошибка: сложность должна быть от 1 до 3");
                    return;
                }

                if (dotaLogic.UpdateHero(id, name, role, attribute, complexity))
                {
                    Console.WriteLine("✓ Герой обновлен");
                }
                else
                {
                    Console.WriteLine("✗ Ошибка при обновлении героя");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении героя: {ex.Message}");
            }
        }

        // 4. Удалить героя (ОСТАЛОСЬ ТАК ЖЕ)
        static void DeleteHeroConsole()
        {
            try
            {
                Console.Write("Введите ID героя для удаления: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Ошибка: неверный формат ID");
                    return;
                }

                // Можно показать, что удаляем
                var hero = dotaLogic.GetHeroById(id);
                if (hero == null)
                {
                    Console.WriteLine($"Герой с ID {id} не найден.");
                    return;
                }

                Console.Write($"Вы уверены, что хотите удалить героя '{hero.Name}' (ID: {hero.Id})? (y/n): ");
                var confirm = Console.ReadLine();

                if (confirm?.ToLower() == "y")
                {
                    if (dotaLogic.DeleteHero(id))
                    {
                        Console.WriteLine($"✓ Герой '{hero.Name}' удален");
                    }
                    else
                    {
                        Console.WriteLine("✗ Ошибка при удалении героя");
                    }
                }
                else
                {
                    Console.WriteLine("Удаление отменено");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении героя: {ex.Message}");
            }
        }

        // 5. Группировать по атрибуту (ЗАМЕНА: вместо ShareData.Instance.GetHeroesSnapshot())
        static void GroupByAttributeConsole()
        {
            try
            {
                var groups = dotaLogic.GroupByAttribute(); // ЗАМЕНА

                if (!groups.Any())
                {
                    Console.WriteLine("Нет героев для группировки.");
                    return;
                }

                Console.WriteLine("\nГруппировка по атрибуту:");
                Console.WriteLine("==========================");

                foreach (var group in groups)
                {
                    Console.WriteLine($"\n{group.Key.ToUpper()}:");
                    Console.WriteLine(new string('-', group.Key.Length + 1));

                    foreach (var hero in group.Value)
                    {
                        Console.WriteLine($"  • {hero.Name,-20} | {hero.Role,-15} | Сложность: {hero.Complexity}");
                    }
                    Console.WriteLine($"  Всего: {group.Value.Count} героев");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при группировке: {ex.Message}");
            }
        }

        // 6. Найти по роли (ЗАМЕНА: вместо ShareData.Instance.GetHeroesSnapshot())
        static void FindByRoleConsole()
        {
            try
            {
                Console.Write("Введите роль для поиска: ");
                string role = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(role))
                {
                    Console.WriteLine("Ошибка: введите роль для поиска");
                    return;
                }

                var heroes = dotaLogic.FindByRole(role); // ЗАМЕНА

                if (!heroes.Any())
                {
                    Console.WriteLine($"Героев с ролью '{role}' не найдено.");
                    return;
                }

                Console.WriteLine($"\nНайдено {heroes.Count} героев с ролью '{role}':");
                Console.WriteLine("===================================================");

                foreach (var hero in heroes)
                {
                    Console.WriteLine($"• {hero.Name,-20} | {hero.Attribute,-12} | Сложность: {hero.Complexity}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске: {ex.Message}");
            }
        }

        // 7. Новый метод: Найти героя по ID
        static void FindByIdConsole()
        {
            try
            {
                Console.Write("Введите ID героя: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Ошибка: неверный формат ID");
                    return;
                }

                var hero = dotaLogic.GetHeroById(id);

                if (hero == null)
                {
                    Console.WriteLine($"Герой с ID {id} не найден.");
                    return;
                }

                Console.WriteLine("\nНайден герой:");
                Console.WriteLine("================");
                Console.WriteLine($"ID: {hero.Id}");
                Console.WriteLine($"Имя: {hero.Name}");
                Console.WriteLine($"Роль: {hero.Role}");
                Console.WriteLine($"Атрибут: {hero.Attribute}");
                Console.WriteLine($"Сложность: {hero.Complexity}");
                Console.WriteLine("================");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске: {ex.Message}");
            }
        }
    }
}