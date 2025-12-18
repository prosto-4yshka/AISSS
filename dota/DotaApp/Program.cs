using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DataAccessLayer;
using Ninject;

namespace DotaApp
{
    class Program
    {
        private static IKernel _ninjectKernel;
        private static DotaLogic _dotaLogic;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Dota 2 Hero Manager ===");
            Console.WriteLine("Инициализация IoC контейнера (Ninject)...");

            try
            {
                // 1. Создаём ядро Ninject с нашим модулем
                _ninjectKernel = new StandardKernel(new NinjectConfigModule());

                // 2. Получаем экземпляр DotaLogic через контейнер
                _dotaLogic = _ninjectKernel.Get<DotaLogic>();

                Console.WriteLine("IoC контейнер инициализирован!");
                Console.WriteLine("База данных: DotaDB.mdf");
                Console.WriteLine("Запуск графического интерфейса...\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при инициализации IoC: {ex.Message}");
                Console.WriteLine("Убедитесь, что установлены пакеты Ninject");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
                return;
            }

            // Проверяем и инициализируем базу данных
            try
            {
                CheckAndInitializeDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при инициализации БД: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
                return;
            }

            // Запускаем форму в отдельном потоке
            var formThread = new Thread(() =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            });

            formThread.Start();

            // Запускаем консольное меню
            ShowConsoleMenu();
        }

        // Публичный метод для получения логики (используется в MainForm)
        public static DotaLogic GetLogic()
        {
            return _dotaLogic;
        }

        static void CheckAndInitializeDatabase()
        {
            // Просто пытаемся получить данные - если БД нет, она создастся автоматически
            var count = _dotaLogic.GetTotalHeroesCount();
            Console.WriteLine($"В базе найдено героев: {count}");

            // Если база пустая - добавляем тестовых героев
            if (count == 0)
            {
                Console.WriteLine("Добавляем тестовых героев...");

                _dotaLogic.CreateHero("Axe", "Initiator", "Strength", 1);
                _dotaLogic.CreateHero("Juggernaut", "Carry", "Agility", 2);
                _dotaLogic.CreateHero("Crystal Maiden", "Support", "Intelligence", 1);
                _dotaLogic.CreateHero("Invoker", "Mid", "Intelligence", 3);
                _dotaLogic.CreateHero("Pudge", "Harder", "Strength", 2);
                _dotaLogic.CreateHero("Windranger", "Mid", "Agility", 2);
                _dotaLogic.CreateHero("Lina", "Mid", "Intelligence", 2);
                _dotaLogic.CreateHero("Sven", "Carry", "Strength", 1);
                _dotaLogic.CreateHero("Lion", "Support", "Intelligence", 2);
                _dotaLogic.CreateHero("Phantom Assassin", "Carry", "Agility", 2);

                Console.WriteLine("Добавлено 10 тестовых героев");
            }
        }

        // ВСЕ МЕТОДЫ КОНСОЛЬНОГО МЕНЮ (они используют _dotaLogic вместо создания нового)

        static void ShowConsoleMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== КОНСОЛЬНОЕ МЕНЮ ===");
                Console.WriteLine("1. Показать всех героев");
                Console.WriteLine("2. Показать героев с пагинацией");
                Console.WriteLine("3. Создать героя");
                Console.WriteLine("4. Обновить героя");
                Console.WriteLine("5. Удалить героя");
                Console.WriteLine("6. Найти героя по ID");
                Console.WriteLine("7. Найти героев по роли");
                Console.WriteLine("8. Группировать по атрибуту");
                Console.WriteLine("9. Статистика базы данных");
                Console.WriteLine("0. Выйти");
                Console.Write("Выберите опцию: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllHeroes();
                        break;
                    case "2":
                        ShowHeroesWithPagination();
                        break;
                    case "3":
                        CreateHeroConsole();
                        break;
                    case "4":
                        UpdateHeroConsole();
                        break;
                    case "5":
                        DeleteHeroConsole();
                        break;
                    case "6":
                        FindHeroById();
                        break;
                    case "7":
                        FindHeroesByRole();
                        break;
                    case "8":
                        GroupByAttributeConsole();
                        break;
                    case "9":
                        ShowDatabaseStats();
                        break;
                    case "0":
                        Console.WriteLine("Выход из программы...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Неверная опция! Попробуйте снова.");
                        break;
                }
            }
        }

        static void ShowAllHeroes()
        {
            try
            {
                var heroes = _dotaLogic.GetAllHeroes();

                if (!heroes.Any())
                {
                    Console.WriteLine("Нет героев в базе данных.");
                    return;
                }

                Console.WriteLine("\n=== ВСЕ ГЕРОИ ===");
                Console.WriteLine("ID  | Имя                  | Роль           | Атрибут       | Сложность");
                Console.WriteLine(new string('-', 80));

                foreach (var hero in heroes)
                {
                    Console.WriteLine($"{hero.Id,3} | {hero.Name,-20} | {hero.Role,-14} | {hero.Attribute,-12} | {hero.Complexity}");
                }

                Console.WriteLine($"\nВсего героев: {heroes.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void ShowHeroesWithPagination()
        {
            try
            {
                Console.Write("Сколько героев на странице (по умолчанию 5): ");
                if (!int.TryParse(Console.ReadLine(), out int pageSize) || pageSize < 1)
                    pageSize = 5;

                var totalHeroes = _dotaLogic.GetTotalHeroesCount();
                var totalPages = _dotaLogic.GetTotalPages(pageSize);

                if (totalHeroes == 0)
                {
                    Console.WriteLine("Нет героев в базе данных.");
                    return;
                }

                Console.WriteLine($"Всего героев: {totalHeroes}, Страниц: {totalPages}");

                int currentPage = 1;

                while (true)
                {
                    var heroes = _dotaLogic.GetHeroesPage(currentPage, pageSize);

                    Console.WriteLine($"\n=== СТРАНИЦА {currentPage} из {totalPages} ===");
                    Console.WriteLine("ID  | Имя                  | Роль           | Атрибут       | Сложность");
                    Console.WriteLine(new string('-', 80));

                    foreach (var hero in heroes)
                    {
                        Console.WriteLine($"{hero.Id,3} | {hero.Name,-20} | {hero.Role,-14} | {hero.Attribute,-12} | {hero.Complexity}");
                    }

                    Console.WriteLine($"\nСтраница {currentPage} из {totalPages} ({heroes.Count} героев на странице)");
                    Console.WriteLine("\nКоманды: n - следующая, p - предыдущая, f - первая, l - последняя, q - выход");
                    Console.Write("Выберите действие: ");
                    var command = Console.ReadLine()?.ToLower();

                    if (command == "n" && currentPage < totalPages)
                        currentPage++;
                    else if (command == "p" && currentPage > 1)
                        currentPage--;
                    else if (command == "f")
                        currentPage = 1;
                    else if (command == "l")
                        currentPage = totalPages;
                    else if (command == "q")
                        break;
                    else if (int.TryParse(command, out int pageNumber) && pageNumber >= 1 && pageNumber <= totalPages)
                        currentPage = pageNumber;
                    else if (command != "n" && command != "p" && command != "f" && command != "l" && command != "q")
                        Console.WriteLine("Неверная команда!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void CreateHeroConsole()
        {
            try
            {
                Console.WriteLine("\n=== СОЗДАНИЕ НОВОГО ГЕРОЯ ===");

                Console.Write("Введите имя героя: ");
                string name = Console.ReadLine();

                Console.Write("Введите роль (Carry/Support/Mid/Harder/Hard support/Lesnik): ");
                string role = Console.ReadLine();

                Console.Write("Введите атрибут (Strength/Agility/Intelligence): ");
                string attribute = Console.ReadLine();

                Console.Write("Введите сложность (1-3): ");
                if (!int.TryParse(Console.ReadLine(), out int complexity) || complexity < 1 || complexity > 3)
                {
                    Console.WriteLine("Ошибка: сложность должна быть от 1 до 3");
                    return;
                }

                var hero = _dotaLogic.CreateHero(name, role, attribute, complexity);
                Console.WriteLine($"\n✓ Создан герой: {hero.Name} (ID: {hero.Id})");
                Console.WriteLine($"Роль: {hero.Role}, Атрибут: {hero.Attribute}, Сложность: {hero.Complexity}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка валидации: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании героя: {ex.Message}");
            }
        }

        static void UpdateHeroConsole()
        {
            try
            {
                Console.Write("Введите ID героя для обновления: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
                {
                    Console.WriteLine("Ошибка: неверный формат ID");
                    return;
                }

                // Проверяем существование героя
                var existingHero = _dotaLogic.GetHeroById(id);
                if (existingHero == null)
                {
                    Console.WriteLine($"Герой с ID {id} не найден.");
                    return;
                }

                Console.WriteLine($"\nТекущие данные героя {existingHero.Name} (ID: {id}):");
                Console.WriteLine($"Имя: {existingHero.Name}");
                Console.WriteLine($"Роль: {existingHero.Role}");
                Console.WriteLine($"Атрибут: {existingHero.Attribute}");
                Console.WriteLine($"Сложность: {existingHero.Complexity}");

                Console.WriteLine("\nВведите новые данные (оставьте пустым чтобы не менять):");

                Console.Write("Новое имя: ");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                    name = existingHero.Name;

                Console.Write("Новая роль: ");
                string role = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(role))
                    role = existingHero.Role;

                Console.Write("Новый атрибут: ");
                string attribute = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(attribute))
                    attribute = existingHero.Attribute;

                Console.Write("Новая сложность (1-3): ");
                string complexityInput = Console.ReadLine();
                int complexity;
                if (string.IsNullOrWhiteSpace(complexityInput))
                    complexity = existingHero.Complexity;
                else if (!int.TryParse(complexityInput, out complexity) || complexity < 1 || complexity > 3)
                {
                    Console.WriteLine("Ошибка: сложность должна быть от 1 до 3");
                    return;
                }

                if (_dotaLogic.UpdateHero(id, name, role, attribute, complexity))
                {
                    Console.WriteLine("\n✓ Герой успешно обновлен!");
                }
                else
                {
                    Console.WriteLine("\n✗ Ошибка при обновлении героя");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка валидации: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении героя: {ex.Message}");
            }
        }

        static void DeleteHeroConsole()
        {
            try
            {
                Console.Write("Введите ID героя для удаления: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
                {
                    Console.WriteLine("Ошибка: неверный формат ID");
                    return;
                }

                var hero = _dotaLogic.GetHeroById(id);
                if (hero == null)
                {
                    Console.WriteLine($"Герой с ID {id} не найден.");
                    return;
                }

                Console.WriteLine($"\nВы уверены, что хотите удалить героя?");
                Console.WriteLine($"ID: {hero.Id}");
                Console.WriteLine($"Имя: {hero.Name}");
                Console.WriteLine($"Роль: {hero.Role}");
                Console.WriteLine($"Атрибут: {hero.Attribute}");
                Console.WriteLine($"Сложность: {hero.Complexity}");

                Console.Write("\nПодтвердите удаление (y/n): ");
                var confirm = Console.ReadLine()?.ToLower();

                if (confirm == "y" || confirm == "yes" || confirm == "да")
                {
                    if (_dotaLogic.DeleteHero(id))
                    {
                        Console.WriteLine($"\n✓ Герой '{hero.Name}' успешно удален!");
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Ошибка при удалении героя");
                    }
                }
                else
                {
                    Console.WriteLine("\nУдаление отменено");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении героя: {ex.Message}");
            }
        }

        static void FindHeroById()
        {
            try
            {
                Console.Write("Введите ID героя: ");
                if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
                {
                    Console.WriteLine("Ошибка: неверный формат ID");
                    return;
                }

                var hero = _dotaLogic.GetHeroById(id);

                if (hero == null)
                {
                    Console.WriteLine($"Герой с ID {id} не найден.");
                    return;
                }

                Console.WriteLine("\n=== НАЙДЕННЫЙ ГЕРОЙ ===");
                Console.WriteLine($"ID: {hero.Id}");
                Console.WriteLine($"Имя: {hero.Name}");
                Console.WriteLine($"Роль: {hero.Role}");
                Console.WriteLine($"Атрибут: {hero.Attribute}");
                Console.WriteLine($"Сложность: {hero.Complexity}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске героя: {ex.Message}");
            }
        }

        static void FindHeroesByRole()
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

                var heroes = _dotaLogic.FindByRole(role);

                if (!heroes.Any())
                {
                    Console.WriteLine($"Героев с ролью '{role}' не найдено.");
                    return;
                }

                Console.WriteLine($"\n=== ГЕРОИ С РОЛЬЮ '{role}' ===");
                Console.WriteLine($"Найдено: {heroes.Count} героев");
                Console.WriteLine("ID  | Имя                  | Атрибут       | Сложность");
                Console.WriteLine(new string('-', 60));

                foreach (var hero in heroes)
                {
                    Console.WriteLine($"{hero.Id,3} | {hero.Name,-20} | {hero.Attribute,-12} | {hero.Complexity}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске: {ex.Message}");
            }
        }

        static void GroupByAttributeConsole()
        {
            try
            {
                var groups = _dotaLogic.GroupByAttribute();

                if (!groups.Any())
                {
                    Console.WriteLine("Нет героев для группировки.");
                    return;
                }

                Console.WriteLine("\n=== ГРУППИРОВКА ПО АТРИБУТУ ===");

                foreach (var group in groups)
                {
                    Console.WriteLine($"\n{group.Key.ToUpper()}: {group.Value.Count} героев");
                    Console.WriteLine(new string('-', 30));

                    foreach (var hero in group.Value)
                    {
                        Console.WriteLine($"  • {hero.Name,-20} | {hero.Role,-14} | Сложность: {hero.Complexity}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при группировке: {ex.Message}");
            }
        }

        static void ShowDatabaseStats()
        {
            try
            {
                var totalHeroes = _dotaLogic.GetTotalHeroesCount();

                if (totalHeroes == 0)
                {
                    Console.WriteLine("База данных пуста.");
                    return;
                }

                var allHeroes = _dotaLogic.GetAllHeroes();

                // Статистика по сложности
                var complexityGroups = allHeroes
                    .GroupBy(h => h.Complexity)
                    .OrderBy(g => g.Key)
                    .ToDictionary(g => g.Key, g => g.Count());

                // Статистика по атрибутам
                var attributeGroups = _dotaLogic.GroupByAttribute();

                // Статистика по ролям
                var roleGroups = allHeroes
                    .GroupBy(h => h.Role)
                    .OrderByDescending(g => g.Count())
                    .ToDictionary(g => g.Key, g => g.Count());

                Console.WriteLine("\n=== СТАТИСТИКА БАЗЫ ДАННЫХ ===");
                Console.WriteLine($"Всего героев: {totalHeroes}");

                Console.WriteLine("\nПо сложности:");
                foreach (var group in complexityGroups)
                {
                    Console.WriteLine($"  Сложность {group.Key}: {group.Value} героев");
                } 

                Console.WriteLine("\nПо атрибутам:");
                foreach (var group in attributeGroups)
                {
                    double percentage = (double)group.Value.Count / totalHeroes * 100;
                    Console.WriteLine($"  {group.Key}: {group.Value.Count} героев ({percentage:F1}%)");
                }

                Console.WriteLine("\nПо ролям:");
                foreach (var group in roleGroups)
                {
                    double percentage = (double)group.Value / totalHeroes * 100;
                    Console.WriteLine($"  {group.Key}: {group.Value} героев ({percentage:F1}%)");
                }

                // Самый сложный и самый простой герой
                var hardestHero = allHeroes.OrderByDescending(h => h.Complexity).FirstOrDefault();
                var easiestHero = allHeroes.OrderBy(h => h.Complexity).FirstOrDefault();

                Console.WriteLine($"\nСамый сложный герой: {hardestHero?.Name} (Сложность: {hardestHero?.Complexity})");
                Console.WriteLine($"Самый простой герой: {easiestHero?.Name} (Сложность: {easiestHero?.Complexity})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении статистики: {ex.Message}");
            }
        }
    }
}