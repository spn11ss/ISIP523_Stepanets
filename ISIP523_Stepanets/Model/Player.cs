using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP523_Stepanets.Model
{
    public class Player
    {
        public int Health { get; set; } = 100;
        public Weapon Weapon { get; set; } = new Weapon("Ржавый меч", 10);
        public Armor Armor { get; set; } = new Armor("Простая броня", 5);
        public bool IsDefending { get; set; }
        public int BlockAmount { get; set; }
        public bool IsFrozen { get; set; }
        public bool IsAlive => Health > 0;
    }
}
