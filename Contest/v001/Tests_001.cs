using System.Linq;
using Xunit;

namespace v001;
public class Tests
{
    [Theory]
    [InlineData("BOMB 6 5 Hello YLD", "13 11 0", "....2...2....;.X.X.X.X.X.X.;100.1...1.001;0X2X0X2X0X2X0;.12.12.21.21.;.X.X1X.X1X.X.;.12.12.21.21.;0X2X0X2X0X2X0;100.1...1.001;.X.X.X.X.X.X.;....2...2....;2;0 0 0 0 1 3;0 1 12 10 1 3")]
    public void Test_Debug(string expected, string gameInputs, string loopInputs)
    {
        var bot = new Bot();
        bot.LoadGameInputs(gameInputs.Split(";").ToList());
        bot.LoadLoopInputs(loopInputs.Split(";").ToList());

        var result = bot.GetPlay();

        Assert.Equal(expected, result);
    }
    [Fact]
    public void GameInputs_Test()
    {
        // seed=6325766776776773000
        var gameInputs = "13 11 0";
        var loopInputs = "....2...2....;.X.X.X.X.X.X.;100.1...1.001;0X2X0X2X0X2X0;.12.12.21.21.;.X.X1X.X1X.X.;.12.12.21.21.;0X2X0X2X0X2X0;100.1...1.001;.X.X.X.X.X.X.;....2...2....;2;0 0 0 0 1 3;0 1 12 10 1 3";

        var bot = new Bot();
        bot.LoadGameInputs(gameInputs.Split(";").ToList());
        bot.LoadLoopInputs(loopInputs.Split(";").ToList());

        Assert.NotNull(bot);
        Assert.Equal(13, bot.SizeX);
        Assert.Equal(11, bot.SizeY);
        Assert.Equal(0, bot.MyId);
    }
}
