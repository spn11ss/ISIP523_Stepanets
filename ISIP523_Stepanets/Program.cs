// Создать консольное приложение для подсчета потраченных за день средств. 
// Пользователь вводит количество операций, которые будут записаны. Можно внести от 2 до 40 операций.
// Дальше, пользователь по шаблону (Название услуги или товара; Количество денег) вводит траты. Валюта - рубли.
// Пример: (Влажные салфетки "Лента"; 235)
// После заполнения всех трат, пользователь должен увидеть следующее меню:1.Вывод данных2.Статистика(среднее,
// максимальное, минимальное, сумма)3.Сортировка по цене(пузырьковая сортировка)4.Конвертация валюты(пользователь
// вводит курс или выбирает из списка)5.Поиск по названию 0. Выход
// Выбор пунктов меню осуществляется по соответствующей цифре.
using System;
using System.Globalization;
using System.Linq;

namespace ISIP523_Stepanets
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== УЧЕТ ЕЖЕДНЕВНЫХ РАСХОДОВ ===");
            // Запрос количества операций
            int operationsCount;
            do
            {
                Console.Write("Введите количество операций (от 2 до 40): ");
            } while (!int.TryParse(Console.ReadLine(), out operationsCount) || operationsCount < 2 || operationsCount > 40);
            // Массивы для хранения данных
            string[] names = new string[operationsCount];
            decimal[] amounts = new decimal[operationsCount];

            // Ввод данных о расходах
            Console.WriteLine("\n Введите расходы в формате: Название; Сумма");
            Console.WriteLine("Пример: Влажные салфетки \"Лента\"; 235");

            for (int i = 0; i < operationsCount; i++)
            {
                bool validInput = false;
                while (!validInput)
                {
                    Console.Write($"Операция {i + 1}: ");
                    string input = Console.ReadLine();

                    // Разделение ввода по точке с запятой
                    string[] parts = input.Split(';');

                    if (parts.Length == 2)
                    {
                        string name = parts[0].Trim();
                        if (!string.IsNullOrWhiteSpace(name) &&
                            decimal.TryParse(parts[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount) &&
                            amount > 0)
                        {
                            names[i] = name;
                            amounts[i] = amount;
                            validInput = true;
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: Неверный формат суммы! Сумма должна быть положительным числом.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: Неверный формат! Используйте: Название; Сумма");
                    }

                }
            }
            // Основное меню
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
                Console.WriteLine("1. Вывод данных");
                Console.WriteLine("2. Статистика");
                Console.WriteLine("3. Сортировка по цене (пузырьковая)");
                Console.WriteLine("4. Конвертация валюты");
                Console.WriteLine("5. Поиск по названию");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        DisplayData(names, amounts);
                        break;
                    case "2":
                        ShowStatistics(amounts);
                        break;
                    case "3":
                        BubbleSort(names, amounts);
                        Console.WriteLine("Данные отсортированы по цене!");
                        break;
                    case "4":
                        ConvertCurrency(amounts);
                        break;
                    case "5":
                        SearchByName(names, amounts);
                        break;
                    case "0":
                        exit = true;
                        Console.WriteLine("До свидания!");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }
        // 1. Вывод всех данных
        static void DisplayData(string[] names, decimal[] amounts)
        {
            Console.WriteLine("=== ВСЕ РАСХОДЫ ===");
            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {names[i]} - {amounts[i]:C}");
            }
            Console.WriteLine($"Итого операций: {names.Length}");
        }

        // 2. Статистика
        static void ShowStatistics(decimal[] amounts)
        {
            if (amounts.Length == 0) return;

            decimal sum = 0;
            decimal max = amounts[0];
            decimal min = amounts[0];

            foreach (decimal amount in amounts)
            {
                sum += amount;
                if (amount > max) max = amount;
                if (amount < min) min = amount;
            }

            decimal average = sum / amounts.Length;

            Console.WriteLine("=== СТАТИСТИКА ===");
            Console.WriteLine($"Общая сумма: {sum:C}");
            Console.WriteLine($"Средний расход: {average:C}");
            Console.WriteLine($"Максимальный расход: {max:C}");
            Console.WriteLine($"Минимальный расход: {min:C}");
        }

        // 3. Пузырьковая сортировка
        static void BubbleSort(string[] names, decimal[] amounts)
        {
            int n = amounts.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (amounts[j] > amounts[j + 1])
                    {
                        // Меняем местами суммы
                        decimal tempAmount = amounts[j];
                        amounts[j] = amounts[j + 1];
                        amounts[j + 1] = tempAmount;

                        // Меняем местами названия
                        string tempName = names[j];
                        names[j] = names[j + 1];
                        names[j + 1] = tempName;
                    }
                }
            }
        }

        // 4. Конвертация валюты
        static void ConvertCurrency(decimal[] amounts)
        {
            Console.WriteLine("=== КОНВЕРТАЦИЯ ВАЛЮТЫ ===");
            Console.WriteLine("1. Доллар США (USD)");
            Console.WriteLine("2. Евро (EUR)");
            Console.WriteLine("3. Тенге (KZT)");
            Console.WriteLine("4. Другая валюта (ввести курс вручную)");
            Console.Write("Выберите валюту: ");

            string currencyChoice = Console.ReadLine();
            decimal exchangeRate;

            switch (currencyChoice)
            {
                case "1":
                    exchangeRate = 90.0m; // Примерный курс USD
                    break;
                case "2":
                    exchangeRate = 98.0m; // Примерный курс EUR
                    break;
                case "3":
                    exchangeRate = 0.2m; // Примерный курс KZT
                    break;
                case "4":
                    Console.Write("Введите курс конвертации (1 RUB = X вашей валюты): ");
                    while (!decimal.TryParse(Console.ReadLine(), out exchangeRate) || exchangeRate <= 0)
                    {
                        Console.Write("Ошибка! Введите положительное число: ");
                    }
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    return;
            }

            Console.WriteLine("\n=== РАСХОДЫ В ВЫБРАННОЙ ВАЛЮТЕ ===");
            for (int i = 0; i < amounts.Length; i++)
            {
                decimal convertedAmount = amounts[i] * exchangeRate;
                Console.WriteLine($"{i + 1}. {amounts[i]:C} RUB = {convertedAmount:F2} (по курсу {exchangeRate})");
            }
        }

        // 5. Поиск по названию
        static void SearchByName(string[] names, decimal[] amounts)
        {
            Console.Write("Введите название для поиска: ");
            string searchTerm = Console.ReadLine().ToLower();

            Console.WriteLine("=== РЕЗУЛЬТАТЫ ПОИСКА ===");
            bool found = false;

            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].ToLower().Contains(searchTerm))
                {
                    Console.WriteLine($"{names[i]} - {amounts[i]:C}");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("Расходы с таким названием не найдены.");
            }
        }
    }
}
