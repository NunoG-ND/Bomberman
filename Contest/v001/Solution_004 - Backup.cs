using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace v004;

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

    public class MovedRecord
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsDone { get; set; }
    }

    public Dictionary<int,List<BoxInfo>> BoxesGrid { get; set; } = new Dictionary<int,List<BoxInfo>>();
    public class BoxInfo
    {
        public int Index { get; set; }
        public bool Destroyed { get; set; }
    }
    public MovedRecord NextMove { get; set; }
    public class Entity
    {
        public int EntityType { get; set; }
        public int Owner { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

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

            //NEW CODE
            foreach(var gridRow in Grid)
            {
                if (!BoxesGrid.ContainsKey(i))
                {
                    BoxesGrid[i] = SimpleIterateRowAndGetBoxes(gridRow.ToCharArray());
                    Console.Error.WriteLine($"BoxesGrid key {i} contains: ");
                    foreach (var box in BoxesGrid[i])
                    {
                        Console.Error.WriteLine(box.ToString());
                    }
                }
            }
            //NEW CODE

        }

        ProcessEntities(entitiesList); 
    }
    private void ProcessEntities(List<string> entities)
    {
        Robots.Clear();
        Bombs.Clear();

        foreach (var entity in entities)
        {
            Entity entityP = new Entity();

            string[] strings = entity.Split(" ");

            entityP.EntityType = int.Parse(strings[0]);
            entityP.Owner = int.Parse(strings[1]);
            entityP.X = int.Parse(strings[2]);
            entityP.Y = int.Parse(strings[3]);
            entityP.Param1 = int.Parse(strings[4]);
            entityP.Param2 = int.Parse(strings[5]);

            if (entityP.EntityType == 0)
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

        var robotRow = Grid[robotOwner.Y];

        var cells = robotRow.ToCharArray();

        //if(!BoxesGrid.ContainsKey(robotOwner.Y))
        //{
        //    BoxesGrid[robotOwner.Y] = SimpleIterateRowAndGetBoxes(cells);
        //    Console.Error.WriteLine($"BoxesGrid key {robotOwner.Y} contains: ");
        //    foreach(var box in BoxesGrid[robotOwner.Y])
        //    {
        //        Console.Error.WriteLine(box.ToString() );
        //    }
        //}

        return IterateRowAndTakeAction(cells, robotOwner, bombOwner);
    }

    private List<BoxInfo> SimpleIterateRowAndGetBoxes(char[] cells)
    {
        List<BoxInfo> boxesRow = new List<BoxInfo>();
        for (var i = 0; i < cells.Length - 1; i++)
        {
            if (cells[i] == '0')
            {
                BoxInfo boxInfo = new BoxInfo();
                boxInfo.Index = i;
                boxInfo.Destroyed = false;
                boxesRow.Add(boxInfo);
            }
        }

        return boxesRow;
    }

    private string IterateRowAndTakeAction(char[] cells, Entity robotOwner, Entity bombOwner)
    {
        for (var i = robotOwner.X; i < cells.Length - 1; i++)
        {
            if (cells[i + 1] == '0')
            {
                if (robotOwner.X == i && bombOwner == null) //The user doesnt have a record for a bomb and there is a box at the right
                {
                    AdjacentToBoxToDestroy(robotOwner);
                    return $"BOMB {i} {robotOwner.Y} YLD 1.1";
                }
                else
                {
                    if(NextMove == null) // there isnt any NextMove record, lets check the closest box in the row. 
                    {
                        foreach(var closestBoxPosition in BoxesGrid[robotOwner.Y].Where(b => !b.Destroyed)) // Only check for boxes not destroyed
                        {
                            if (closestBoxPosition.Index > robotOwner.X)
                            {
                                NextMove = new MovedRecord()
                                {
                                    X = closestBoxPosition.Index-1, //Change this for a place that doesnt have a box
                                    Y = robotOwner.Y,
                                    IsDone = false,
                                };
                                break;
                            }
                        }
                    }
                    
                    //if(!NextMove.IsDone) //TODO: bugged
                    if(!NextMove.IsDone && NextMove.X != robotOwner.X && NextMove.Y != robotOwner.Y) //TODO: bugged
                    {

                        return $"MOVE {NextMove.X} {NextMove.Y} Hello YLD 1.2";
                    }
                    else if(NextMove.IsDone) //If movement is done we want to create a new record, and find the closest record, if there is none we want to go to next row
                    {
                        foreach (var closestBoxPosition in BoxesGrid[robotOwner.Y].Where(b => !b.Destroyed))
                        {
                            if (closestBoxPosition.Index > robotOwner.X)
                            {
                                NextMove = new MovedRecord()
                                {
                                    X = closestBoxPosition.Index-1, //Change this for a place that doesnt have a box
                                    Y = robotOwner.Y,
                                    IsDone = false,
                                };
                                break;
                            }
                        }
                    }
                    else if(NextMove.X == robotOwner.X && NextMove.Y == robotOwner.Y)
                    {
                        NextMove = new MovedRecord()
                        {
                            X = i + 1,
                            Y = robotOwner.Y,
                            IsDone = true,
                        };

                        NextMove.X = i + 1;
                        NextMove.Y = robotOwner.Y;
                        NextMove.IsDone = true;

                        AdjacentToBoxToDestroy(robotOwner);

                        return $"BOMB {NextMove.X} {NextMove.Y} YLD 1.3";
                    }
                    else //Test
                    {
                        return $"MOVE {0} {robotOwner.Y + 1} Hello YLD 4";
                    }
                }
            }
            if (robotOwner.X == cells.Length - 1)
            {
                return $"MOVE {0} {robotOwner.Y + 1} Hello YLD 2";
            }
        }

        if (!NextMove.IsDone)
            return $"MOVE {NextMove.X} {NextMove.Y} Hello YLD 4";

        //It can only go to position 0 if there is no box there. Change this.
        return $"MOVE {0} {robotOwner.Y + 1} Hello YLD 5";
    }

    //private string MoveToNextLine(Entity robotOwner)
    //{
    //    if (BoxesGrid.ContainsKey(robotOwner.Y+1))
    //    {
    //        for (int i = 0; i< BoxesGrid.Count; i++) { }
    //        var box = BoxesGrid[robotOwner.Y+1].FirstOrDefault(b => b.Index == robotOwner.X);
    //        if (box != null)
    //        {
    //        }
    //    }
    //}

    private void AdjacentToBoxToDestroy(Entity robotOwner)
    {
        if (BoxesGrid.ContainsKey(robotOwner.Y))
        {
            var box = BoxesGrid[robotOwner.Y].FirstOrDefault(b => b.Index == robotOwner.X + 1);
            if (box != null)
            {
                if(NextMove != null)
                {
                    NextMove.IsDone = true; //this move is done go to next one
                }
                box.Destroyed = true;
            }
        }
    }

    public class Coordinates
    {
        public int x ; public int y; 
    }

}