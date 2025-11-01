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

   