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

            Console.WriteLine("\n✅ Вы успешно зарегистрировались!");
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

        static void ShowBasket()
        {
            if (currentUser == null)
            {
                Console.WriteLine("Сначала войдите в систему!");
                return;
            }

            var cartItems = Core.Context.CartItems
                .Where(c => c.UserID == currentUser.ID)
                .Join(Core.Context.Products,
                      c => c.ProductID,
                      p => p.ID,
                      (c, p) => new { CartItem = c, Product = p })
                .ToList();

            if (cartItems.Count == 0)
            {
                Console.WriteLine("\n Корзина пуста!");
                return;
            }

            Console.WriteLine("\n КОРЗИНА");
            decimal totalSum = 0;

            foreach (var item in cartItems)
            {
                decimal itemSum = item.Product.Price * item.CartItem.Quantity;
                Console.WriteLine($"\n{item.Product.Name} — {item.Product.Price}₽ × {item.CartItem.Quantity} = {itemSum}₽");
                totalSum += itemSum;
            }
            Console.WriteLine($"\n Итого: {totalSum}₽");
        }

        static void CreateOrder()
        {
            if (currentUser == null)
            {
                Console.WriteLine("Сначала войдите в систему!");
                return;
            }

            Console.WriteLine("\nВыберите, что хотите заказать:");
            Console.WriteLine("1. Купить конкретный товар из корзины");
            Console.WriteLine("2. Купить всю корзину");
            Console.WriteLine("0. Отмена");

            Console.Write("Введите выбор: ");
            int orderchoice = Convert.ToInt32(Console.ReadLine());

            switch (orderchoice)
            {
                case 1:
                    BuyOneProduct();
                    break;

                case 2:
                    BuyAllBasket();
                    break;

                case 0:
                    Console.WriteLine(" Отменено.");
                    break;

                default:
                    Console.WriteLine(" Неверный пункт меню.");
                    break;
            }
        }

        static void BuyOneProduct()
        {
            var cartItems = Core.Context.CartItems
                .Where(c => c.UserID == currentUser.ID)
                .Join(Core.Context.Products,
                      c => c.ProductID,
                      p => p.ID,
                      (c, p) => new { CartItem = c, Product = p })
                .ToList();

            if (cartItems.Count == 0)
            {
                Console.WriteLine("\n Корзина пуста!");
                return;
            }

            Console.WriteLine("\n Товары в вашей корзине:");
            for (int i = 0; i < cartItems.Count; i++)
            {
                var item = cartItems[i];
                decimal itemTotal = item.Product.Price * item.CartItem.Quantity;
                Console.WriteLine($"{i + 1}. {item.Product.Name} — {item.Product.Price}₽ × {item.CartItem.Quantity} = {itemTotal}₽");
            }

            Console.Write("Введите номер товара для покупки: ");
            if (!int.TryParse(Console.ReadLine(), out int itemNumber) || itemNumber < 1 || itemNumber > cartItems.Count)
            {
                Console.WriteLine(" Некорректный номер товара!");
                return;
            }

            var selectedItem = cartItems[itemNumber - 1];
            var cartItem = selectedItem.CartItem;
            var product = selectedItem.Product;

            Console.Write($"Введите количество для покупки (макс. {cartItem.Quantity}): ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0 || qty > cartItem.Quantity)
            {
                Console.WriteLine(" Некорректное количество!");
                return;
            }

            Console.WriteLine("\n Доступные пункты выдачи:");
            var pickupPoints = Core.Context.PickupPoints.ToList();
            foreach (var p in pickupPoints)
            {
                Console.WriteLine($"{p.ID}. {p.Address} (Часы работы: {p.WorkingHours})");
            }

            Console.Write("Выберите пункт выдачи: ");
            if (!int.TryParse(Console.ReadLine(), out int pickupPointId))
            {
                Console.WriteLine(" Неверный формат ID.");
                return;
            }

            var pickupPoint = pickupPoints.FirstOrDefault(p => p.ID == pickupPointId);
            if (pickupPoint == null)
            {
                Console.WriteLine(" Пункт выдачи не найден!");
                return;
            }

            // Создаем заказ
            Orders order = new Orders
            {
                UserID = currentUser.ID,
                PickupPointID = pickupPoint.ID,
                OrderDate = DateTime.Now
            };
            Core.Context.Orders.Add(order);
            Core.Context.SaveChanges();

            // Добавляем товар в OrderItems
            OrderItems orderItem = new OrderItems
            {
                OrderID = order.ID,
                ProductID = product.ID,
                Quantity = qty,
                Price = product.Price
            };
            Core.Context.OrderItems.Add(orderItem);

            // Обновляем количество товара на складе
            product.Quantity -= qty;

            // Обновляем или удаляем товар из корзины
            if (cartItem.Quantity > qty)
            {
                cartItem.Quantity -= qty;
            }
            else
            {
                Core.Context.CartItems.Remove(cartItem);
            }

            Core.Context.SaveChanges();

            decimal totalPrice = product.Price * qty;
            Console.WriteLine($"Заказ №{order.ID} оформлен!");
            Console.WriteLine($"Товар '{product.Name}' x{qty} куплен! Стоимость: {totalPrice}₽");
            Console.WriteLine($"Забрать по адресу: {pickupPoint.Address}");
            Console.WriteLine($"Часы работы: {pickupPoint.WorkingHours}");
        }

        static void BuyAllBasket()
        {
            var cartItems = Core.Context.CartItems
                .Where(c => c.UserID == currentUser.ID)
                .Join(Core.Context.Products,
                      c => c.ProductID,
                      p => p.ID,
                      (c, p) => new { CartItem = c, Product = p })
                .ToList();

            if (cartItems.Count == 0)
            {
                Console.WriteLine("\n Корзина пуста!");
                return;
            }

            Console.WriteLine("\n Доступные пункты выдачи:");
            var pickupPoints = Core.Context.PickupPoints.ToList();
            foreach (var p in pickupPoints)
            {
                Console.WriteLine($"{p.ID}. {p.Address} (Часы работы: {p.WorkingHours})");
            }

            Console.Write("Выберите пункт выдачи: ");
            if (!int.TryParse(Console.ReadLine(), out int pickupPointId))
            {
                Console.WriteLine(" Неверный формат ID.");
                return;
            }

            var pickupPoint = pickupPoints.FirstOrDefault(p => p.ID == pickupPointId);
            if (pickupPoint == null)
            {
                Console.WriteLine(" Пункт выдачи не найден!");
                return;
            }

            // Создаем заказ
            Orders order = new Orders
            {
                UserID = currentUser.ID,
                PickupPointID = pickupPoint.ID,
                OrderDate = DateTime.Now
            };
            Core.Context.Orders.Add(order);
            Core.Context.SaveChanges();

            decimal totalSum = 0;

            foreach (var item in cartItems)
            {
                // Добавляем товар в OrderItems
                OrderItems orderItem = new OrderItems
                {
                    OrderID = order.ID,
                    ProductID = item.Product.ID,
                    Quantity = item.CartItem.Quantity,
                    Price = item.Product.Price
                };
                Core.Context.OrderItems.Add(orderItem);

                // Увеличиваем общую сумму
                totalSum += item.Product.Price * item.CartItem.Quantity;

                // Обновляем количество товара на складе
                item.Product.Quantity -= item.CartItem.Quantity;

                // Удаляем товар из корзины
                Core.Context.CartItems.Remove(item.CartItem);
            }

            Core.Context.SaveChanges();

            Console.WriteLine($"\n Заказ №{order.ID} оформлен!");
            Console.WriteLine($"Общая сумма: {totalSum}₽");
            Console.WriteLine($"Забрать по адресу: {pickupPoint.Address}");
            Console.WriteLine($"Часы работы: {pickupPoint.WorkingHours}");
            Console.WriteLine("Корзина очищена.");
        }

        static void ShowOrderHistory()
        {
            if (currentUser == null)
            {
                Console.WriteLine("Сначала войдите в систему!");
                return;
            }

            var orders = Core.Context.Orders
                .Where(o => o.UserID == currentUser.ID)
                .OrderByDescending(o => o.OrderDate)
                .Join(Core.Context.PickupPoints,
                      o => o.PickupPointID,
                      p => p.ID,
                      (o, p) => new { Order = o, PickupPoint = p })
                .ToList();

            if (orders.Count == 0)
            {
                Console.WriteLine("\nИстория заказов пуста.");
                return;
            }

            Console.WriteLine("\nИстория заказов:");

            foreach (var orderInfo in orders)
            {
                Console.WriteLine($"\nЗаказ №{orderInfo.Order.ID} от {orderInfo.Order.OrderDate:g}");
                Console.WriteLine($"Пункт выдачи: {orderInfo.PickupPoint.Address}");
                Console.WriteLine($"Часы работы: {orderInfo.PickupPoint.WorkingHours}");
                Console.WriteLine("Товары:");

                var orderItems = Core.Context.OrderItems
                    .Where(i => i.OrderID == orderInfo.Order.ID)
                    .Join(Core.Context.Products,
                          i => i.ProductID,
                          p => p.ID,
                          (i, p) => new { OrderItem = i, Product = p })
                    .ToList();

                decimal total = 0;
                foreach (var item in orderItems)
                {
                    decimal itemTotal = item.OrderItem.Price * item.OrderItem.Quantity;
                    total += itemTotal;
                    Console.WriteLine($" - {item.Product.Name} × {item.OrderItem.Quantity} шт. — {item.OrderItem.Price}₽/шт = {itemTotal}₽");
                }
                Console.WriteLine($"Итого по заказу: {total}₽");
                Console.WriteLine(new string('-', 40));
            }
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