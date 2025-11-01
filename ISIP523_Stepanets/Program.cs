using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Game game = new Game();
        game.Start();
    }
}

public class Game
{
    private Player player;
    private Random random;
    private int turnCount;

    public Game()
    {
        random = new Random();
        player = new Player();
        turnCount = 0;
    }

    public void Start()
    {
        Console.WriteLine("Добро пожаловать в текстовый рогалик!");

        while (player.IsAlive)
        {
            turnCount++;
            Console.WriteLine($"\n--- Ход {turnCount} ---");
            Console.WriteLine($"Здоровье игрока: {player.Health}");

            if (turnCount % 10 == 0)
            {
                SpawnBoss();
            }
            else
            {
                if (random.Next(2) == 0)
                    SpawnEnemy();
                else
                    OpenChest();
            }

            if (player.IsFrozen)
            {
                Console.WriteLine("Вы заморожены и пропускаете ход!");
                player.IsFrozen = false;
            }
        }

        Console.WriteLine("\nИгра окончена! Вы погибли...");
    }

    private void SpawnBoss()
    {
        Console.WriteLine("Появляется босс!");
        Boss[] bosses = {
            new Boss("ВВГ", "Гоблин", 2.0, 1.5, 1.2, 0.1),
            new Boss("Ковальский", "Скелет", 2.5, 1.3, 1.4, 0),
            new Boss("Архимаг C++", "Маг", 1.8, 1.6, 1.1, 0.1),
            new Boss("Пестов С--", "Скелет", 1.3, 1.8, 0.6, 0.15)
        };

        Boss boss = bosses[random.Next(bosses.Length)];
        StartCombat(boss);
    }

    private void SpawnEnemy()
    {
        Enemy enemy;
        int enemyType = random.Next(3);
        switch (enemyType)
        {
            case 0:
                enemy = new Goblin();
                break;
            case 1:
                enemy = new Skeleton();
                break;
            default:
                enemy = new Mage();
                break;
        }
        Console.WriteLine($"Появляется {enemy.Name}!");
        StartCombat(enemy);
    }

    private void OpenChest()
    {
        Console.WriteLine("Вы нашли сундук!");
        int chestContent = random.Next(3);
        switch (chestContent)
        {
            case 0:
                Console.WriteLine("Внутри лечебное зелье!");
                player.Health = 100;
                Console.WriteLine("Здоровье восстановлено до 100!");
                break;
            case 1:
                Weapon newWeapon = Weapon.GenerateRandom(random);
                Console.WriteLine($"Внутри оружие: {newWeapon.Name} (Урон: {newWeapon.Damage})");
                Console.WriteLine($"Ваше текущее оружие: {player.Weapon.Name} (Урон: {player.Weapon.Damage})");
                if (GetPlayerChoice("Взять новое оружие?"))
                    player.Weapon = newWeapon;
                break;
            case 2:
                Armor newArmor = Armor.GenerateRandom(random);
                Console.WriteLine($"Внутри доспехи: {newArmor.Name} (Защита: {newArmor.Defense})");
                Console.WriteLine($"Ваши текущие доспехи: {player.Armor.Name} (Защита: {player.Armor.Defense})");
                if (GetPlayerChoice("Взять новые доспехи?"))
                    player.Armor = newArmor;
                break;
        }
    }

    private void StartCombat(Enemy enemy)
    {
        Console.WriteLine($"Бой с {enemy.Name}!");
        Console.WriteLine($"Здоровье врага: {enemy.Health}");

        while (enemy.IsAlive && player.IsAlive)
        {
            PlayerTurn(enemy);
            if (enemy.IsAlive)
                EnemyTurn(enemy);
        }

        if (enemy.IsAlive) return;

        Console.WriteLine($"Вы победили {enemy.Name}!");
        if (random.NextDouble() < 0.3)
        {
            Console.WriteLine("Противник выронил зелье здоровья!");
            player.Health = Math.Min(100, player.Health + 30);
        }
    }

    private void PlayerTurn(Enemy enemy)
    {
        Console.WriteLine("\nВаш ход:");
        Console.WriteLine("1 - Атака");
        Console.WriteLine("2 - Защита");

        string input = Console.ReadLine();
        if (input == "1")
        {
            enemy.TakeDamage(player.Weapon.Damage);
            Console.WriteLine($"Вы нанесли {player.Weapon.Damage} урона!");
        }
        else
        {
            if (random.NextDouble() < 0.4)
            {
                Console.WriteLine("Вы уклонились от атаки!");
                player.IsDefending = true;
            }
            else
            {
                double blockPercent = 0.7 + random.NextDouble() * 0.3;
                int blockedDamage = (int)(player.Armor.Defense * blockPercent);
                Console.WriteLine($"Вы подготовились к блокированию (защита: {blockedDamage})");
                player.BlockAmount = blockedDamage;
            }
        }
    }

    private void EnemyTurn(Enemy enemy)
    {
        if (player.IsDefending)
        {
            player.IsDefending = false;
            return;
        }

        int damage = enemy.Attack(player);
        if (damage > 0)
        {
            if (player.BlockAmount > 0)
            {
                damage = Math.Max(0, damage - player.BlockAmount);
                player.BlockAmount = 0;
                Console.WriteLine($"Вы заблокировали часть урона! Получено урона: {damage}");
            }
            player.Health -= damage;
            Console.WriteLine($"Враг нанес {damage} урона!");
        }
    }

    private bool GetPlayerChoice(string message)
    {
        Console.WriteLine($"{message} (y/n)");
        return Console.ReadLine().ToLower() == "y";
    }
}

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

