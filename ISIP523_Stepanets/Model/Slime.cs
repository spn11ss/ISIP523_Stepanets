using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP523_Stepanets.Model
{
    public class Slime : Enemy
    {
        public Slime()
        {
            Name = "Слизень";
            Health = 40;
            AttackPower = 6;
            Defense = 1;
        }

        public override void TakeDamage(int damage)
        {
            int reducedDamage = Math.Max(1, damage - 2);
            Health -= reducedDamage;
            Console.WriteLine($"Слизень уменьшил урон на 2 единицы! Полученный урон: {reducedDamage}");
        }

        public override int Attack(Player player)
        {
            if (new Random().NextDouble() < 0.15)
            {
                Console.WriteLine("Слизень пытается вас обездвижить!");
                return AttackPower * 3;
            }
            return base.Attack(player);
        }
    }
}
