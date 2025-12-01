using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ISIP523_Stepanets
{
    public static class CarData
    {
        public static List<Car> Cars = new List<Car>
        {
            new Car { Id = 1, Brand = "Toyota", Model = "Camry", Year = 2018 },
            new Car { Id = 2, Brand = "Honda", Model = "Civic", Year = 2020 },
            new Car { Id = 3, Brand = "Ford", Model = "Focus", Year = 2019 },
            new Car { Id = 4, Brand = "BMW", Model = "X5", Year = 2021 },
            new Car { Id = 5, Brand = "Mercedes", Model = "C-Class", Year = 2020 },
            new Car { Id = 6, Brand = "Audi", Model = "A4", Year = 2019 },
            new Car { Id = 7, Brand = "Volkswagen", Model = "Golf", Year = 2020 },
            new Car { Id = 8, Brand = "Hyundai", Model = "Solaris", Year = 2021 },
            new Car { Id = 9, Brand = "Kia", Model = "Rio", Year = 2022 },
            new Car { Id = 10, Brand = "Lada", Model = "Vesta", Year = 2021 }
        };
    }

    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            InitializeGame();
            while (true)
            {
                Console.Clear();
                DisplayGameStatus();

                DisplayActionMenu();

            }

        }

        static void InitializeGame()
        {
            using (var context = Core.Context)
            {
                if (!context.Service.Any())
                {
                    var service = new Service
                    {
                        Balance = 1000.00m,
                        TotalCarsProcessed = 0,
                        SuccessfulRepairs = 0,
                        LastUpdated = DateTime.Now
                    };
                    context.Service.Add(service);
                    context.SaveChanges();
                }

                if (!context.Spares.Any())
                {
                    var spares = new List<Spares>
                    {
                        new Spares { SpareName = "Тормозные колодки", PurchasePrice = 80.00m, Quantity = 3, MinimumStockLevel = 2, RepairMarkup = 1.5m },
                        new Spares { SpareName = "Масляный фильтр", PurchasePrice = 15.00m, Quantity = 5, MinimumStockLevel = 3, RepairMarkup = 1.8m },
                        new Spares { SpareName = "Воздушный фильтр", PurchasePrice = 25.00m, Quantity = 4, MinimumStockLevel = 2, RepairMarkup = 1.6m },
                        new Spares { SpareName = "Свечи зажигания", PurchasePrice = 45.00m, Quantity = 6, MinimumStockLevel = 3, RepairMarkup = 1.7m },
                        new Spares { SpareName = "Аккумулятор", PurchasePrice = 120.00m, Quantity = 2, MinimumStockLevel = 1, RepairMarkup = 1.4m }
                    };

                    context.Spares.AddRange(spares);
                    context.SaveChanges();
                }
            }

            Console.WriteLine("Игра 'Автосервис' запущена!");
        }
        static void DisplayGameStatus()
        {
            using (var context = Core.Context)
            {
                var service = context.Service.First();
                Console.WriteLine("=== АВТОСЕРВИС ===");
                Console.WriteLine($"Баланс: {service.Balance} руб.");
                Console.WriteLine($"Обработано машин: {service.TotalCarsProcessed}");
                Console.WriteLine($"Успешных ремонтов: {service.SuccessfulRepairs}");
                Console.WriteLine($"Ожидающих поставок: {Core.PendingDeliveries.Count}");
                Console.WriteLine("===================");
            }
        }

        static void DisplayActionMenu()
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1 - Принять заказ");
            Console.WriteLine("2 - Отказаться от заказа");
            Console.WriteLine("3 - Купить запчасти");
            Console.WriteLine("4 - Показать склад");
            Console.WriteLine("5 - Статистика");
            Console.WriteLine("6 - Выйти из игры");
        }
    }
}
