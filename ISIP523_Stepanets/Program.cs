using System;
using System.Collections.Generic;
using System.Linq;
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

}
