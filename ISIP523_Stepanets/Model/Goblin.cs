using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP523_Stepanets.Model
{
    public class Goblin : Enemy
    {
        public Goblin()
        {
            Name = "Гоблин";
            Health = 30;
            AttackPower = 8;
            Defense = 3;
        }

        public override int Attack(Player player)
        {
            if (new Random().NextDouble() < 0.2)
            {
                Console.WriteLine("Критический урон гоблина!");
                return AttackPower * 2;
            }
            return base.Attack(player);
        }
    }
}
