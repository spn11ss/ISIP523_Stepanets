using System;
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
}
