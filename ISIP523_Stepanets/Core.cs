using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP523_Stepanets
{
    internal class Core
    {
        public static PR7_StepanetsEntities Context = new PR7_StepanetsEntities();
        public static List<PendingDelivery> PendingDeliveries = new List<PendingDelivery>();
        public static int CarsProcessed = 0;
    }
    public class PendingDelivery
    {
        public int SpareID { get; set; }
        public string SpareName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public int OrderPlacedAtCar { get; set; }
    }
    public class TempClient
    {
        public string CarModel { get; set; }
        public int BrokenPartID { get; set; }
        public string BrokenPartName { get; set; }
        public decimal RepairCost { get; set; }
    }
}
