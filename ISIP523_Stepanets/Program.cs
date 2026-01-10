using System;
using System.Collections.Generic;
using System.Linq;

namespace ISIP523_Stepanets
{

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;



        bool outt = true;
        while (outt)
        {
            Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Регистрация");
            Console.WriteLine("2. Вход");
            Console.WriteLine("3. Просмотр товаров (без входа)");
            Console.WriteLine("4. Очистить базу данных");
            Console.WriteLine("0. Выход из программы");

            Console.Write("Введите выбор: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Неверный формат ввода.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    Registration();
                    break;

                case 2:
                    Login();
                    break;

                case 3:
                    WatchProducts();
                    break;

                case 4:
                    ClearDatabase();
                    break;

                case 0:
                    outt = false;
                    Console.WriteLine("До свидания!");
                    break;

                default:
                    Console.WriteLine("Неправильный пункт меню.");
                    break;
            }
        }
    }
}
          