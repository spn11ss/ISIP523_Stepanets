using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP523_Stepanets.Model
{
    public class Mage : Enemy
    {
        public Mage()
        {
            Name = "Маг";
            Health = 20;
            AttackPower = 12;
            Defense = 2;
        }

        public override int Attack(Player player)
        {
            if (new Random().NextDouble() < 0.25)
            {
                Console.WriteLine("Маг замораживает вас!");
                player.IsFrozen = true;
            }
            return base.Attack(player);
        }
    }
}
