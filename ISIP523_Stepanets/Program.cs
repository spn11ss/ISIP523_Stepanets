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

    }
    }
}
