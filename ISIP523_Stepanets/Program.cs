using System;
using System.Collections.Generic;
using System.Linq;

namespace ISIP523_Stepanets
{
    internal class Program
    {
        static Users currentUser = null;

        static void UserMenu()
        {
            bool useroutt = true;
            while (useroutt)
            {
                Console.WriteLine("\n МЕНЮ:");
                Console.WriteLine("1. Просмотр товаров");
                Console.WriteLine("2. Просмотр корзины");
                Console.WriteLine("3. Оформить заказ");
                Console.WriteLine("4. История заказов");
                Console.WriteLine("0. Выход из аккаунта");

                Console.Write("Введите выбор: ");
                int userchoice = Convert.ToInt32(Console.ReadLine());

                switch (userchoice)
                {
                    case 1:
                        WatchProducts();
                        AddProductToBasket();
                        break;

                    case 2:
                        ShowBasket();
                        break;

                    case 3:
                        CreateOrder();
                        break;

                    case 4:
                        ShowOrderHistory();
                        break;

                    case 0:
                        useroutt = false;
                        currentUser = null;
                        Console.WriteLine("Вы вышли из аккаунта.");
                        break;

                    default:
                        Console.WriteLine("Неправильный пункт меню.");
                        break;
                }
            }
        }

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

            Console.WriteLine("\n Вы успешно зарегистрировались!");
            currentUser = dbUser;
            UserMenu();
        }

        static void Login()
        {
            Console.WriteLine("\n~~~ Войдите в аккаунт ~~~");
            Console.Write("Имя пользователя: ");
            string username = Console.ReadLine();
            Console.Write("Пароль: ");
            string password = Console.ReadLine();

            var user = Core.Context.Users.FirstOrDefault(x => x.Login == username);
            if (user == null)
            {
                Console.WriteLine("\n❌ Пользователь с таким именем не найден.");
                return;
            }
            else
            {
                //проверка пароля 
                if (user.PasswordHash == password)
                {
                    Console.WriteLine($"\n Вы успешно вошли в аккаунт, {user.Name}!");
                    currentUser = user;
                    UserMenu();
                }
                else
                {
                    Console.WriteLine("\n Неверный пароль.");
                }
            }
        }

        static void WatchProducts()
        {
            Console.WriteLine("\n НАШИ ТОВАРЫ");
            var products = Core.Context.Products.ToList();

            if (products.Count == 0)
            {
                Console.WriteLine("Товаров пока нет в базе данных.");
                return;
            }

            foreach (var p in products)
            {
                Console.WriteLine($"\n{p.ID}. {p.Name}\nОписание: {p.Description}\nЦена: {p.Price}₽\nВ наличии: {p.Quantity} шт.\nКатегория: {p.Category}");
            }
        }

        static void AddProductToBasket()
        {
            if (currentUser == null)
            {
                Console.WriteLine("Сначала войдите в систему!");
                return;
            }

            Console.Write("\nХотите добавить в корзину какой-то товар? (да/нет): ");
            string ans = Console.ReadLine()?.Trim().ToLower();

            if (ans != "да")
                return;

            Console.Write("\nВведите ID товара, который хотите добавить в корзину: ");
            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine(" Неверный формат ID.");
                return;
            }

            var product = Core.Context.Products.FirstOrDefault(p => p.ID == productId);
            if (product == null)
            {
                Console.WriteLine(" Товар с таким ID не найден.");
                return;
            }

            Console.Write("Введите количество: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine(" Некорректное количество!");
                return;
            }

            // Проверка товара на складе
            if (quantity > product.Quantity)
            {
                Console.WriteLine($" Недостаточно товара на складе. В наличии: {product.Quantity} шт.");
                return;
            }

            // Есть ли товар в корзине
            var existingCartItem = Core.Context.CartItems
                .FirstOrDefault(c => c.UserID == currentUser.ID && c.ProductID == productId);

            if (existingCartItem != null)
            {
                // Проверяем, не превысим ли общее количество с текущим запасом
                if (existingCartItem.Quantity + quantity > product.Quantity)
                {
                    Console.WriteLine($" Нельзя добавить больше {product.Quantity} шт. этого товара.");
                    return;
                }

                existingCartItem.Quantity += quantity;
                Console.WriteLine($"\n Обновлено количество {product.Name}: теперь {existingCartItem.Quantity} шт.");
            }
            else
            {
                CartItems newCartItem = new CartItems
                {
                    UserID = currentUser.ID,
                    ProductID = productId,
                    Quantity = quantity
                };
                Core.Context.CartItems.Add(newCartItem);
                Console.WriteLine($"\n {product.Name} x{quantity} добавлен в корзину!");
            }

            Core.Context.SaveChanges();
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

        public static void ClearDatabase()
        {
            Console.Write("Вы действительно хотите очистить базу данных? (да/нет): ");
            string answer = Console.ReadLine()?.Trim().ToLower();

            if (answer != "да")
            {
                Console.WriteLine("Очистка отменена.");
                return;
            }

            Console.WriteLine("Очистка базы данных...");

            // Очищаем в правильном порядке (с учетом внешних ключей)
            Core.Context.CartItems.RemoveRange(Core.Context.CartItems);
            Core.Context.OrderItems.RemoveRange(Core.Context.OrderItems);
            Core.Context.Orders.RemoveRange(Core.Context.Orders);
            Core.Context.Users.RemoveRange(Core.Context.Users);
            Core.Context.Products.RemoveRange(Core.Context.Products);
            Core.Context.PickupPoints.RemoveRange(Core.Context.PickupPoints);

            Core.Context.SaveChanges();

            Console.WriteLine("Все данные из базы успешно удалены!");
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