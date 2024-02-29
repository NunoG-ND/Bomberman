using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace v003;

public class Solution
{
    static void Main(string[] args)
    {
        var bot = new Bot();
        bot.ReadGameInputs();

        while (true)
        {
            bot.ReadLoopInputs();
            Console.WriteLine(bot.GetPlay());
        }
    }
}
public class Bot
{
    public int MyId { get; private set; }
    public int SizeX { get; private set; }
    public int SizeY { get; private set; }

    public class Entity
    {
        public int EntityType { get; set; }
        public int Owner { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public int Param1 { get; set; }
        public int Param2 { get; set; }
    }
    public List<Entity> Robots { get; set; } = new List<Entity>();
    public List<Entity> Bombs { get; set; } = new List<Entity>();
    public string[] Grid { get; set; }

    private readonly Stopwatch _stopwatch = new Stopwatch();
    public void ReadGameInputs()
    {
        var inputs = new List<string> { Console.ReadLine() };
        Console.Error.WriteLine(string.Join(";", inputs));
        ProcessGameInputs(inputs);
    }
    public void LoadGameInputs(List<string> inputs) => ProcessGameInputs(inputs);
    private void ProcessGameInputs(List<string> inputs)
    {
        var parts = inputs[0].Split(' ');
        SizeX = int.Parse(parts[0]);
        SizeY = int.Parse(parts[1]);
        MyId = int.Parse(parts[2]);
    }
    public void ReadLoopInputs()
    {
        var inputs = new List<string>();
        for (var y = 0; y < SizeY; y++)
            inputs.Add(Console.ReadLine());
        var nbEntities = int.Parse(Console.ReadLine());
        inputs.Add($"{nbEntities}");
        for (var e = 0; e < nbEntities; e++)
            inputs.Add(Console.ReadLine());
        _stopwatch.Restart();
        Console.Error.WriteLine(string.Join(";", inputs));
        ProcessLoopInputs(inputs);
    }
    public void LoadLoopInputs(List<string> inputs) => ProcessLoopInputs(inputs);
    private void ProcessLoopInputs(List<string> inputs)
    {
        var sumInputs = inputs.Count;
        Console.Error.WriteLine(sumInputs);

        int numberOfEntities;
        var entitiesList = new List<string>();
        Grid = new string[SizeY];

        for (int i=0; i< inputs.Count; i++)
        {
            if (i < SizeY)
            {
                Grid[i] = inputs[i];
            }
            else if(i == SizeY)
            {
                numberOfEntities = int.Parse(inputs[i]);
            }
            else
            {
                entitiesList.Add(inputs[i]);
            }
        }

        ProcessEntities(entitiesList); 
    }
    private void ProcessEntities(List<string> entities)
    {
        Robots.Clear();
        Bombs.Clear();

        foreach(var entity in entities)
        {
            Entity entityP = new Entity();

            string[] strings = entity.Split(" ");

            entityP.EntityType = int.Parse(strings[0]);
            entityP.Owner = int.Parse(strings[1]);
            entityP.x = int.Parse(strings[2]);
            entityP.y= int.Parse(strings[3]);
            entityP.Param1 = int.Parse(strings[4]);
            entityP.Param2 = int.Parse(strings[5]);

            if(entityP.EntityType == 0)
            {
                Robots.Add(entityP);
            }
            else 
            { 
                Bombs.Add(entityP); 
            }
        }
    }
    public string GetPlay()
    {
        var robotOwner = Robots.FirstOrDefault(x => x.Owner == 0);

        var bombOwner = Bombs.FirstOrDefault(x => x.Owner == 0);

        var robotRow = Grid[robotOwner.y];

        var cells = robotRow.ToCharArray();

        return IterateRowToTheRight(cells, robotOwner, bombOwner);
    }

    private string IterateRowToTheRight(char[] cells, Entity robotOwner, Entity bombOwner)
    {
        for (var i = robotOwner.x; i < cells.Length - 1; i++)
        {
            Console.Error.WriteLine($"value of {cells[i]} at {i}");

            if (cells[i + 1] == '0')
            {
                if (robotOwner.x == i)
                    return $"BOMB {i} {robotOwner.y} Hello YLD 1.1";
                else
                    return $"MOVE {i} {robotOwner.y} Hello YLD 1.2";
            }
            if (robotOwner.x == cells.Length - 1)
            {
                return $"MOVE {0} {robotOwner.y + 1} Hello YLD 2";
            }
            //else
            //{
            //    return $"MOVE {robotOwner.x + 1} {robotOwner.y} Hello YLD 3";
            //}
        }

        return $"MOVE {0} {robotOwner.y + 1} Hello YLD 4";
    }

    public class Coordinates
    {
        public int x ; public int y; 
    }

}