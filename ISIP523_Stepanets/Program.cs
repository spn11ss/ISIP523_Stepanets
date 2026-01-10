using System;
using System.Collections.Generic;
using System.Linq;

namespace ISIP523_Stepanets
{
    internal class Program
    {
       

        static void Registration()
        {
            Console.WriteLine("\n~~~ Регистрация ~~~");
            Console.Write("Введите имя пользователя: ");
            string username = Console.ReadLine();

            var exist = Core.Context.Users.FirstOrDefault(x => x.Login == username);
            if (exist != null)
            {
                Console.WriteLine("\n Пользователь с таким именем уже существует.");
                return;
            }

            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();
            Console.Write("Повторите пароль: ");
            string passwordRepeat = Console.ReadLine();

            if (password != passwordRepeat)
            {
                Console.WriteLine("\n Пароли не совпадают!");
                return;
            }

            Users dbUser = new Users
            {
                Name = username,
                Login = username,
                PasswordHash = password 
            };
            Core.Context.Users.Add(dbUser);
            Core.Context.SaveChanges();

            Console.WriteLine("\n✅ Вы успешно зарегистрировались!");
            currentUser = dbUser;
            UserMenu();
        }

        

        static void AddProductsAndPickupPoints()
        {
            if (!Core.Context.Products.Any())
            {
                // Добавляем товары
                Core.Context.Products.Add(new Products
                {
                    Name = "Кардиган шерстяной",
                    Description = "Oversize-модель, темно синий со змейкой.",
                    Price = 1792.00m,
                    Quantity = 9,
                    Category = "Одежда"
                });

                Core.Context.Products.Add(new Products
                {
                    Name = "Брелок ОСД",
                    Description = "Брелок ОСД - Оди, из керамики",
                    Price = 450.00m,
                    Quantity = 15,
                    Category = "Аксессуары"
                });

                Core.Context.Products.Add(new Products
                {
                    Name = "Бальзам для губ 'KIKO MILANO LIP VOLUME STYLO'",
                    Description = "Увлажняющий бальзам для губ с эффектом придания объема.",
                    Price = 1614.00m,
                    Quantity = 33,
                    Category = "Косметика"
                });

                Core.Context.Products.Add(new Products
                {
                    Name = "Чокер из гематита 'Higher-aura hematite choker'",
                    Description = "Чокер изготовлен из настоящего гематита размером 4 и 6 мм. Вставка в виде сердца из гипоаллергенного бижутерного сплава и родированная фурнитура обеспечивает долговечность украшению.",
                    Price = 872.00m,
                    Quantity = 13,
                    Category = "Украшения"
                });

                Core.Context.Products.Add(new Products
                {
                    Name = "Набор магнитов 'St. Petesburg'",
                    Description = "Набор из 5 магнитов, достопримечательностей Санкт-Петербурга",
                    Price = 413.80m,
                    Quantity = 15,
                    Category = "Сувениры"
                });

                Core.Context.SaveChanges();
                Console.WriteLine("Товары добавлены в базу данных.");
            }

            if (!Core.Context.PickupPoints.Any())
            {
                // Добавляем пункты выдачи
                Core.Context.PickupPoints.Add(new PickupPoints
                {
                    Address = "г. Москва, ул. Елецкая, д. 5",
                    WorkingHours = "09:00-21:00"
                });

                Core.Context.PickupPoints.Add(new PickupPoints
                {
                    Address = "г. Раменское, ул. Кирова, д. 12",
                    WorkingHours = "10:00-20:00"
                });

                Core.Context.PickupPoints.Add(new PickupPoints
                {
                    Address = "г. Тирасполь, ул. Кирова, д. 9",
                    WorkingHours = "09:00-20:00"
                });

                Core.Context.PickupPoints.Add(new PickupPoints
                {
                    Address = "г.Москва, ул. Кронштадтский б-р, д. 37Б",
                    WorkingHours = "10:00-19:00"
                });

                Core.Context.PickupPoints.Add(new PickupPoints
                {
                    Address = "г. Старый Оскол, ул. Кисловая, д. 51",
                    WorkingHours = "09:00-18:00"
                });

                Core.Context.SaveChanges();
                Console.WriteLine("Пункты выдачи добавлены в базу данных.");
            }
        }

      

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;


            AddProductsAndPickupPoints();

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
}