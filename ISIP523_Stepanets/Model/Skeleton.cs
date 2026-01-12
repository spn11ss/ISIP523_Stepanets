using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP523_Stepanets.Model
{
    public class Skeleton : Enemy
    {
        public Skeleton()
        {
            Name = "Скелет";
            Health = 25;
            AttackPower = 10;
            Defense = 4;
        }

        public override int Attack(Player player)
        {
            Console.WriteLine("Скелет игнорирует вашу защиту!");
            return AttackPower;
        }
    }
}
