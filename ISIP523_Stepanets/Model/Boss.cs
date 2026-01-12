using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP523_Stepanets.Model
{
    public class Boss : Enemy
    {
        public Boss(string name, string baseType, double healthMult, double attackMult, double defenseMult, double extraAbilityChance)
        {
            Name = name;
            Health = (int)(GetBaseHealth(baseType) * healthMult);
            AttackPower = (int)(GetBaseAttack(baseType) * attackMult);
            Defense = (int)(GetBaseDefense(baseType) * defenseMult);
        }

        private int GetBaseHealth(string type)
        {
            switch (type)
            {
                case "Гоблин": return 30;
                case "Скелет": return 25;
                case "Маг": return 20;
                case "Слизень": return 40;
                default: return 25;
            }
        }

        private int GetBaseAttack(string type)
        {
            switch (type)
            {
                case "Гоблин": return 8;
                case "Скелет": return 10;
                case "Маг": return 12;
                case "Слизень": return 6;
                default: return 10;
            }
        }

        private int GetBaseDefense(string type)
        {
            switch (type)
            {
                case "Гоблин": return 3;
                case "Скелет": return 4;
                case "Маг": return 2;
                case "Слизень": return 1;
                default: return 3;
            }
        }
    }
}
