using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace v001;

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
        // inputs list description (see statement for more details)
        //      SizeY lines => each line represent a row of the grid
        //      Integer => Nb entities on the grid
        //      NbEntities lines => each line represent an entity on the grid   
    }
    public string GetPlay()
    {
        Console.Error.WriteLine(string.Join(";", _stopwatch.ElapsedMilliseconds));
        return $"BOMB 6 5 Hello YLD";
    }
}