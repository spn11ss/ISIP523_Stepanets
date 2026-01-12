using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISIP523_Stepanets.Model
{
    namespace Model
    {
        public static class MonsterFactory
        {
            public static Enemy CreateEnemy(string enemyType)
            {
                switch (enemyType)
                {
                    case "Goblin":
                        return new Goblin();
                    case "Skeleton":
                        return new Skeleton();
                    case "Mage":
                        return new Mage();
                    case "Slime":
                        return new Slime();
                    default:
                        throw new ArgumentException($"Неизвестный тип врага: {enemyType}");
                }
            }

            public static Enemy CreateRandomEnemy(Random random)
            {
                string[] enemyTypes = { "Goblin", "Skeleton", "Mage", "Slime" };
                string randomType = enemyTypes[random.Next(enemyTypes.Length)];
                return CreateEnemy(randomType);
            }
        }
    }
}
