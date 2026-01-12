using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP523_Stepanets.Model
{
    public abstract class Enemy
    {
        public string Name { get; protected set; }
        public int Health { get; set; }
        public int AttackPower { get; protected set; }
        public int Defense { get; protected set; }
        public bool IsAlive => Health > 0;

        public virtual int Attack(Player player)
        {
            return AttackPower;
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
        }
    }
}
