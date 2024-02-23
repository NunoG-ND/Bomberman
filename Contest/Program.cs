using System;
using System.Linq;

using v001;

var gameInputs = "13 11 0";
var loopInputs = "....2...2....;.X.X.X.X.X.X.;100.1...1.001;0X2X0X2X0X2X0;.12.12.21.21.;.X.X1X.X1X.X.;.12.12.21.21.;0X2X0X2X0X2X0;100.1...1.001;.X.X.X.X.X.X.;....2...2....;2;0 0 0 0 1 3;0 1 12 10 1 3";

var bot = new Bot();
bot.LoadGameInputs(gameInputs.Split(";").ToList());
bot.LoadLoopInputs(loopInputs.Split(";").ToList());

var result = bot.GetPlay();

Console.WriteLine();
Console.WriteLine(result);

Console.WriteLine("End");

