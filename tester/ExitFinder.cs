namespace tester;

using Swoq.Interface;
using swoq2025;

using TileType = Swoq.Interface.Tile;

[TestClass]
public sealed class ExitFinder
{
    [TestMethod]
    public void NoExit_ReturnsFalse()
    {
        Map map = new(3, 3);
        Game game = new("", map, null, 3)
        {
            Player = new(new(1, 1), Inventory.None)
        };
        map[0, 0].Type = TileType.Wall;
        map[1, 0].Type = TileType.Wall;
        map[2, 0].Type = TileType.Wall;

        map[0, 1].Type = TileType.Wall;
        map[1, 1].Type = TileType.Player;
        map[2, 1].Type = TileType.Wall;

        map[0, 2].Type = TileType.Wall;
        map[1, 2].Type = TileType.Wall;
        map[2, 2].Type = TileType.Wall;

        swoq2025.Objectives.ExitFinder finder = new(map, game.Player);
        Assert.IsFalse(finder.TryGetNextTarget(out Coord next));
        Assert.IsFalse(finder.IsCompleted);
        Assert.IsTrue(finder.HasToBeSolved);
        Assert.AreEqual(1, finder.Priority);
    }

    [TestMethod]
    public void ExitNotReachable_ReturnsFalse()
    {
        Map map = new(3, 3);
        Game game = new("", map, null, 3)
        {
            Player = new(new(0, 1), Inventory.None)
        };
        map[0, 0].Type = TileType.Wall;
        map[1, 0].Type = TileType.Wall;
        map[2, 0].Type = TileType.Wall;

        map[0, 1].Type = TileType.Player;
        map[1, 1].Type = TileType.Wall;
        map[2, 1].Type = TileType.Exit;

        map[0, 2].Type = TileType.Wall;
        map[1, 2].Type = TileType.Wall;
        map[2, 2].Type = TileType.Wall;

        swoq2025.Objectives.ExitFinder finder = new(map, game.Player);
        Assert.IsFalse(finder.TryGetNextTarget(out Coord next));
        Assert.IsFalse(finder.IsCompleted);
        Assert.IsTrue(finder.HasToBeSolved);
        Assert.AreEqual(1, finder.Priority);
    }


    [TestMethod]
    public void HasExit_ReturnsTrue()
    {
        Map map = new(3, 3);
        Game game = new("", map, null, 3)
        {
            Player = new(new(0, 1), Inventory.None)
        };
        map[0, 0].Type = TileType.Wall;
        map[1, 0].Type = TileType.Wall;
        map[2, 0].Type = TileType.Wall;

        map[0, 1].Type = TileType.Player;
        map[1, 1].Type = TileType.Empty;
        map[2, 1].Type = TileType.Exit;

        map[0, 2].Type = TileType.Wall;
        map[1, 2].Type = TileType.Wall;
        map[2, 2].Type = TileType.Wall;

        swoq2025.Objectives.ExitFinder finder = new(map, game.Player);
        Assert.IsTrue(finder.TryGetNextTarget(out Coord next));
        Assert.AreEqual(1, next.X);
        Assert.AreEqual(1, next.Y);
        game.Player.Position = next;

        Assert.IsTrue(finder.TryGetNextTarget(out next));
        Assert.AreEqual(2, next.X);
        Assert.AreEqual(1, next.Y);
        game.Player.Position = next;

        map[3, 1].Type = TileType.Player;

        Assert.IsTrue(finder.IsCompleted);
        Assert.IsTrue(finder.HasToBeSolved);
        Assert.AreEqual(1, finder.Priority);
    }
}
