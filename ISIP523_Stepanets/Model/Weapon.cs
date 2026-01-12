using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP523_Stepanets.Model
{
    public class Weapon
    {
        public string Name { get; }
        public int Damage { get; }

        public Weapon(string name, int damage)
        {
            Name = name;
            Damage = damage;
        }

        public static Weapon GenerateRandom(Random random)
        {
            string[] names = { "Меч воина", "Секира", "Кинжал", "Посох мага" };
            return new Weapon(names[random.Next(names.Length)], random.Next(10, 21));
        }
    }
}
