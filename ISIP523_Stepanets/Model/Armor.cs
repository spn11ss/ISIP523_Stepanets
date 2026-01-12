using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP523_Stepanets.Model
{
    public class Armor
    {
        public string Name { get; }
        public int Defense { get; }

        public Armor(string name, int defense)
        {
            Name = name;
            Defense = defense;
        }

        public static Armor GenerateRandom(Random random)
        {
            string[] names = { "Кольчуга", "Латы", "Кожаная броня", "Мантия" };
            return new Armor(names[random.Next(names.Length)], random.Next(5, 16));
        }
    }
}
