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

