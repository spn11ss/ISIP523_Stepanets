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

   