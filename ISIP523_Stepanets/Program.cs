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
    }
}
