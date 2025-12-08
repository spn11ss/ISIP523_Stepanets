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

                var clientInfo = GenerateRandomClient();
                DisplayClientRequest(clientInfo);

                DisplayActionMenu();
                var choice = GetUserChoice();

                switch (choice)
                {
                    case 1: AcceptOrder(clientInfo); break;
                    case 2: DeclineOrder(clientInfo); break;
                    case 3: ShowPurchaseMenu(); break;
                    case 4: ShowWarehouseStatus(); break;
                    case 5: ShowStatistics(); break;
                    case 6: return;
                    default: Console.WriteLine("Неверный выбор!"); break;
                }
                ProcessDeliveries();
                CheckGameOver();

                Console.WriteLine("\nНажмите любую клавишу...");
                Console.ReadKey();
            }

        }
        static int GetUserChoice()
        {
            Console.Write("Ваш выбор: ");
            return int.TryParse(Console.ReadLine(), out int choice) ? choice : 0;
        }

        static void InitializeGame()
        {
            using (var context = new PR7_StepanetsEntities1())
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
                    };

                    context.Spares.AddRange(spares);
                    context.SaveChanges();
                }
            }

            Console.WriteLine("Игра 'Автосервис' запущена!");
        }
        static void DisplayGameStatus()
        {
            using (var context = new PR7_StepanetsEntities1())
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
        static TempClient GenerateRandomClient() 
        {
            var random = new Random();

            var carsCount = CarData.Cars.Count;
            var randomCar = CarData.Cars[random.Next(carsCount)];

            using (var context = new PR7_StepanetsEntities1())
            {
                var spares = context.Spares.ToList();
                var randomSpare = spares[random.Next(spares.Count)];
                var repairCost = randomSpare.PurchasePrice * randomSpare.RepairMarkup; 

                return new TempClient
                {
                    CarModel = $"{randomCar.Brand} {randomCar.Model} ({randomCar.Year})",
                    BrokenPartID = randomSpare.ID,
                    BrokenPartName = randomSpare.SpareName,
                    RepairCost = repairCost
                };
            }
        }
        static void DisplayClientRequest(TempClient client)
        {
            using (var context = new PR7_StepanetsEntities1())
            {
                var spare = context.Spares.First(s => s.ID == client.BrokenPartID);
                Console.WriteLine($"\nПриехал клиент на {client.CarModel}");
                Console.WriteLine($"Поломка: {client.BrokenPartName}");
                Console.WriteLine($"Стоимость ремонта: {client.RepairCost:C}");
                Console.WriteLine($"На складе: {spare.Quantity} шт.");
            }
        }
        static void AcceptOrder(TempClient client)
        {
            using (var context = new PR7_StepanetsEntities1())
            {
                var service = context.Service.First();
                var brokenSpare = context.Spares.First(s => s.ID == client.BrokenPartID);

                service.TotalCarsProcessed++;
                Core.CarsProcessed++;

                if (brokenSpare.Quantity > 0)
                {
                    brokenSpare.Quantity--;
                    service.Balance += client.RepairCost;
                    service.SuccessfulRepairs++;

                    var order = new Orders
                    {
                        CarModel = client.CarModel,
                        BrokenPartID = client.BrokenPartID,
                        UsedPartID = client.BrokenPartID,
                        ServiceID = 1,
                        Status = "Completed",
                        RepairCost = client.RepairCost,
                        FinalProfit = client.RepairCost - brokenSpare.PurchasePrice,
                        OrderDate = DateTime.Now
                    };
                    context.Orders.Add(order);

                    Console.WriteLine($"Ремонт выполнен успешно! Получено: {client.RepairCost:C}");
                }
                else
                {
                    Console.WriteLine("Нужной детали нет на складе! Производим замену случайной деталью...");

                    var randomSpare = GetRandomAvailableSpare(context);
                    if (randomSpare != null)
                    {
                        randomSpare.Quantity--;

                        var penalty = 150.00m;
                        service.Balance -= penalty;

                        var order = new Orders
                        {
                            CarModel = client.CarModel,
                            BrokenPartID = client.BrokenPartID,
                            UsedPartID = randomSpare.ID,
                            ServiceID = 1,
                            Status = "Failed",
                            RepairCost = 0,
                            FinalProfit = -penalty,
                            OrderDate = DateTime.Now
                        };
                        context.Orders.Add(order);

                        Console.WriteLine($"Клиент недоволен! Штраф: {penalty:C}");
                        Console.WriteLine($"Использована случайная деталь: {randomSpare.SpareName}");
                    }
                    else
                    {
                        Console.WriteLine("На складе нет вообще никаких деталей! Штраф удвоен.");
                        service.Balance -= 300.00m;
                    }
                }

                service.LastUpdated = DateTime.Now;
                context.SaveChanges();
            }
        }
        
    }
}