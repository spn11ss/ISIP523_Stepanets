using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace ISIP523_Stepanets
{
   // Перечисление для категорий товаров
    public enum ProductCategory
    {
        Electronics,
        Clothing,
        Books,
        Food,
        Sports
    }
    // Класс товара
    public class Product
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsInStock => Quantity > 0;
        public ProductCategory Category { get; set; }

        public Product(string code, string name, decimal price, int quantity, ProductCategory category)
        {
            Code = code;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Код: {Code}");
            Console.WriteLine($"Название: {Name}");
            Console.WriteLine($"Цена: {Price:C}");
            Console.WriteLine($"Количество: {Quantity}");
            Console.WriteLine($"В наличии: {(IsInStock ? "Да" : "Нет")}");
            Console.WriteLine($"Категория: {Category}");
            Console.WriteLine(new string('-', 30));
        }
    }
    public class ProductManager
    {
        private List<Product> products;
        private int lastProductId;

        public ProductManager()
        {
            products = new List<Product>();
            lastProductId = 0; // Начинаем с 1001
        }

        // Генерация уникального кода товара
        private string GenerateProductCode()
        {
            lastProductId++;
            return lastProductId.ToString();
        }

        // Валидация вводимых данных
        private bool ValidateProductData(string name, decimal price, int quantity)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Ошибка: Название товара не может быть пустым!");
                return false;
            }

            if (price <= 0)
            {
                Console.WriteLine("Ошибка: Цена должна быть положительной!");
                return false;
            }

            if (quantity < 0)
            {
                Console.WriteLine("Ошибка: Количество не может быть отрицательным!");
                return false;
            }

            return true;
        }
        // Добавление товара
        public void AddProduct(string name, decimal price, int quantity, ProductCategory category)
        {
            if (!ValidateProductData(name, price, quantity))
                return;

            string code = GenerateProductCode();
            Product newProduct = new Product(code, name, price, quantity, category);
            products.Add(newProduct);
            Console.WriteLine($"Товар '{name}' успешно добавлен с кодом {code}");
        }

        // Удаление товара
        public void RemoveProduct(string code)
        {
            Product productToRemove = products.FirstOrDefault(p => p.Code == code);

            if (productToRemove != null)
            {
                products.Remove(productToRemove);
                Console.WriteLine($"Товар с кодом {code} успешно удален");
            }
            else
            {
                Console.WriteLine($"Товар с кодом {code} не найден");
            }
        }

        // Заказ поставки товара
        public void OrderSupply(string code, int quantity)
        {
            if (quantity <= 0)
            {
                Console.WriteLine("Ошибка: Количество для поставки должно быть положительным!");
                return;
            }

            Product product = products.FirstOrDefault(p => p.Code == code);

            if (product != null)
            {
                product.Quantity += quantity;
                Console.WriteLine($"Поставка товара {product.Name}: +{quantity} единиц. Теперь в наличии: {product.Quantity}");
            }
            else
            {
                Console.WriteLine($"Товар с кодом {code} не найден");
            }
        }

        // Продажа товара
        public void SellProduct(string code, int quantity)
        {
            if (quantity <= 0)
            {
                Console.WriteLine("Ошибка: Количество для продажи должно быть положительным!");
                return;
            }

            Product product = products.FirstOrDefault(p => p.Code == code);

            if (product != null)
            {
                if (product.Quantity >= quantity)
                {
                    product.Quantity -= quantity;
                    decimal total = product.Price * quantity;
                    Console.WriteLine($"Продажа товара {product.Name}: -{quantity} единиц. Остаток: {product.Quantity}");
                    Console.WriteLine($"Общая стоимость: {total:C}");
                }
                else
                {
                    Console.WriteLine($"Недостаточно товара на складе! В наличии: {product.Quantity}, запрошено: {quantity}");
                }
            }
            else
            {
                Console.WriteLine($"Товар с кодом {code} не найден");
            }
        }
        // Поиск по коду
        public void SearchByCode(string code)
        {
            Product product = products.FirstOrDefault(p => p.Code == code);

            if (product != null)
            {
                Console.WriteLine("\nНайден товар:");
                product.PrintInfo();
            }
            else
            {
                Console.WriteLine($"Товар с кодом {code} не найден");
            }
        }

        // Поиск по названию
        public void SearchByName(string name)
        {
            var foundProducts = products.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();

            if (foundProducts.Any())
            {
                Console.WriteLine($"\nНайдено товаров: {foundProducts.Count}");
                foreach (var product in foundProducts)
                {
                    product.PrintInfo();
                }
            }
            else
            {
                Console.WriteLine($"Товары с названием '{name}' не найдены");
            }
        }

        // Поиск по категории
        public void SearchByCategory(ProductCategory category)
        {
            var foundProducts = products.Where(p => p.Category == category).ToList();

            if (foundProducts.Any())
            {
                Console.WriteLine($"\nТовары в категории '{category}': {foundProducts.Count}");
                foreach (var product in foundProducts)
                {
                    product.PrintInfo();
                }
            }
            else
            {
                Console.WriteLine($"Товары в категории '{category}' не найдены");
            }
        }

        // Показать все товары
        public void DisplayAllProducts()
        {
            if (!products.Any())
            {
                Console.WriteLine("Список товаров пуст");
                return;
            }

            Console.WriteLine($"\nВсего товаров: {products.Count}");
            foreach (var product in products)
            {
                product.PrintInfo();
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ProductManager manager = new ProductManager();
            InitializeTestData(manager);

            bool exit = false;

            while (!exit)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProductMenu(manager);
                        break;
                    case "2":
                        RemoveProductMenu(manager);
                        break;
                    case "3":
                        OrderSupplyMenu(manager);
                        break;
                    case "4":
                        SellProductMenu(manager);
                        break;
                    case "5":
                        SearchMenu(manager);
                        break;
                    case "6":
                        manager.DisplayAllProducts();
                        break;
                    case "7":
                        exit = true;
                        Console.WriteLine("До свидания!");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор! Попробуйте снова.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void InitializeTestData(ProductManager manager)
        {
            // Добавляем 5 тестовых товаров
            manager.AddProduct("Смартфон Samsung", 25000, 10, ProductCategory.Electronics);
            manager.AddProduct("Футболка хлопковая", 1500, 25, ProductCategory.Clothing);
            manager.AddProduct("Война и мир", 800, 15, ProductCategory.Books);
            manager.AddProduct("Шоколад Alpen Gold", 120, 50, ProductCategory.Food);
            manager.AddProduct("Футбольный мяч", 3000, 8, ProductCategory.Sports);

            Console.WriteLine("Тестовые данные загружены успешно!\n");
        }

        static void DisplayMenu()
        {
            Console.WriteLine("=== СИСТЕМА УЧЁТА ТОВАРОВ ===");
            Console.WriteLine("1. Добавить товар");
            Console.WriteLine("2. Удалить товар");
            Console.WriteLine("3. Заказать поставку");
            Console.WriteLine("4. Продать товар");
            Console.WriteLine("5. Поиск товаров");
            Console.WriteLine("6. Показать все товары");
            Console.WriteLine("7. Выход");
            Console.Write("Выберите действие: ");
        }
    }
