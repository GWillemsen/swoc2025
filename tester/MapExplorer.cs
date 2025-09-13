namespace tester;

using Swoq.Interface;
using swoq2025;

using TileType = Swoq.Interface.Tile;

[TestClass]
public sealed class MapExplorer
{
    [TestMethod]
    public void NoUnknownTiles_ReturnsFalse()
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

        swoq2025.Objectives.MapExplorer explorer = new(map, game.Player);
        Assert.IsFalse(explorer.TryGetNextTarget(out Coord next));
        Assert.IsTrue(explorer.IsCompleted);
        Assert.IsFalse(explorer.HasToBeSolved);
        Assert.AreEqual(0, explorer.Priority);
    }

    [TestMethod]
    public void HasSingleUnknownTile_ReturnsTrue()
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
        map[2, 1].Type = TileType.Unknown;

        map[0, 2].Type = TileType.Wall;
        map[1, 2].Type = TileType.Wall;
        map[2, 2].Type = TileType.Wall;

        swoq2025.Objectives.MapExplorer explorer = new(map, game.Player);
        Assert.IsTrue(explorer.TryGetNextTarget(out Coord next));
        Assert.AreEqual(1, next.X);
        Assert.AreEqual(1, next.Y);

        Assert.IsFalse(explorer.IsCompleted);
        Assert.IsFalse(explorer.HasToBeSolved);
        Assert.AreEqual(0, explorer.Priority);
    }

    [TestMethod]
    public void HasUnknownTileInCornerBehindWall_ReturnsFalse()
    {
        Map map = new(4, 4);
        Game game = new("", map, null, 3)
        {
            Player = new(new(1, 1), Inventory.None)
        };
        map[0, 0].Type = TileType.Empty;
        map[1, 0].Type = TileType.Empty;
        map[2, 0].Type = TileType.Empty;
        map[3, 0].Type = TileType.Empty;

        map[0, 1].Type = TileType.Empty;
        map[1, 1].Type = TileType.Player;
        map[2, 1].Type = TileType.Empty;
        map[3, 1].Type = TileType.Empty;

        map[0, 2].Type = TileType.Empty;
        map[1, 2].Type = TileType.Empty;
        map[2, 2].Type = TileType.Empty;
        map[3, 2].Type = TileType.Wall;

        map[0, 3].Type = TileType.Empty;
        map[1, 3].Type = TileType.Empty;
        map[2, 3].Type = TileType.Wall;
        map[3, 3].Type = TileType.Unknown;

        swoq2025.Objectives.MapExplorer explorer = new(map, game.Player);
        Assert.IsFalse(explorer.TryGetNextTarget(out Coord next));

        Assert.IsTrue(explorer.IsCompleted);
        Assert.IsFalse(explorer.HasToBeSolved);
        Assert.AreEqual(0, explorer.Priority);
    }

    [TestMethod]
    public void HasUnknownAcrossSeveralCycles()
    {
        Map map = new(4, 4);
        Game game = new("", map, null, 3)
        {
            Player = new(new(1, 1), Inventory.None)
        };
        map[0, 0].Type = TileType.Empty;
        map[1, 0].Type = TileType.Empty;
        map[2, 0].Type = TileType.Empty;
        map[3, 0].Type = TileType.Empty;

        map[0, 1].Type = TileType.Empty;
        map[1, 1].Type = TileType.Player;
        map[2, 1].Type = TileType.Empty;
        map[3, 1].Type = TileType.Empty;

        map[0, 2].Type = TileType.Empty;
        map[1, 2].Type = TileType.Empty;
        map[2, 2].Type = TileType.Empty;
        map[3, 2].Type = TileType.Empty;

        map[0, 3].Type = TileType.Empty;
        map[1, 3].Type = TileType.Empty;
        map[2, 3].Type = TileType.Wall;
        map[3, 3].Type = TileType.Unknown;

        swoq2025.Objectives.MapExplorer explorer = new(map, game.Player);
        Assert.IsTrue(explorer.TryGetNextTarget(out Coord next));
        Assert.AreEqual(2, next.X);
        Assert.AreEqual(1, next.Y);
        game.Player.Position = next;

        Assert.IsTrue(explorer.TryGetNextTarget(out next));
        Assert.AreEqual(3, next.X);
        Assert.AreEqual(1, next.Y);
        game.Player.Position = next;

        Assert.IsTrue(explorer.TryGetNextTarget(out next));
        Assert.AreEqual(3, next.X);
        Assert.AreEqual(2, next.Y);
        game.Player.Position = next;

        Assert.IsTrue(explorer.TryGetNextTarget(out next));
        Assert.AreEqual(3, next.X);
        Assert.AreEqual(3, next.Y);
        game.Player.Position = next;

        Assert.IsFalse(explorer.TryGetNextTarget(out next));
    }

    [TestMethod]
    public void HasSeveralUnknown_SolveAcrossSeveralCycles()
    {
        Map map = new(4, 4);
        Game game = new("", map, null, 3)
        {
            Player = new(new(0, 0), Inventory.None)
        };
        map[0, 0].Type = TileType.Player;
        map[1, 0].Type = TileType.Empty;
        map[2, 0].Type = TileType.Empty;
        map[3, 0].Type = TileType.Empty;

        map[0, 1].Type = TileType.Empty;
        map[1, 1].Type = TileType.Empty;
        map[2, 1].Type = TileType.Empty;
        map[3, 1].Type = TileType.Empty;

        map[0, 2].Type = TileType.Wall;
        map[1, 2].Type = TileType.Empty;
        map[2, 2].Type = TileType.Empty;
        map[3, 2].Type = TileType.Empty;

        map[0, 3].Type = TileType.Unknown;
        map[1, 3].Type = TileType.Empty;
        map[2, 3].Type = TileType.Wall;
        map[3, 3].Type = TileType.Unknown;

        swoq2025.Objectives.MapExplorer explorer = new(map, game.Player);
        Assert.IsTrue(explorer.TryGetNextTarget(out Coord next));
        Assert.AreEqual(0, next.X);
        Assert.AreEqual(1, next.Y);
        Assert.IsFalse(explorer.IsCompleted);
        Assert.IsFalse(explorer.HasToBeSolved);
        Assert.AreEqual(0, explorer.Priority);
        game.Player.Position = next;

        Assert.IsTrue(explorer.TryGetNextTarget(out next));
        Assert.AreEqual(1, next.X);
        Assert.AreEqual(1, next.Y);
        Assert.IsFalse(explorer.IsCompleted);
        Assert.IsFalse(explorer.HasToBeSolved);
        Assert.AreEqual(0, explorer.Priority);
        game.Player.Position = next;

        Assert.IsTrue(explorer.TryGetNextTarget(out next));
        Assert.AreEqual(1, next.X);
        Assert.AreEqual(2, next.Y);
        Assert.IsFalse(explorer.IsCompleted);
        Assert.IsFalse(explorer.HasToBeSolved);
        Assert.AreEqual(0, explorer.Priority);
        game.Player.Position = next;

        Assert.IsTrue(explorer.TryGetNextTarget(out next));
        Assert.AreEqual(1, next.X);
        Assert.AreEqual(3, next.Y);
        Assert.IsFalse(explorer.IsCompleted);
        Assert.IsFalse(explorer.HasToBeSolved);
        Assert.AreEqual(0, explorer.Priority);
        game.Player.Position = next;

        Map map1 = new(1, [
            new() { Type = TileType.Empty },
        ]);
        map.MergeMap(map1, 0, 3);

        Assert.IsTrue(explorer.TryGetNextTarget(out next));
        Assert.AreEqual(1, next.X);
        Assert.AreEqual(2, next.Y);
        Assert.IsFalse(explorer.IsCompleted);
        Assert.IsFalse(explorer.HasToBeSolved);
        Assert.AreEqual(0, explorer.Priority);
        game.Player.Position = next;

        Assert.IsTrue(explorer.TryGetNextTarget(out next));
        Assert.AreEqual(2, next.X);
        Assert.AreEqual(2, next.Y);
        Assert.IsFalse(explorer.IsCompleted);
        Assert.IsFalse(explorer.HasToBeSolved);
        Assert.AreEqual(0, explorer.Priority);
        game.Player.Position = next;

        map.MergeMap(map1, 3, 3);

        Assert.IsFalse(explorer.TryGetNextTarget(out next));
        Assert.IsTrue(explorer.IsCompleted);
        Assert.IsFalse(explorer.HasToBeSolved);
        Assert.AreEqual(0, explorer.Priority);
    }

    [TestMethod]
    public void OneReachableAndOneBehindWall()
    {
        Map map = new(4, 4);
        Game game = new("", map, null, 3)
        {
            Player = new(new(1, 1), Inventory.None)
        };
        map[0, 0].Type = TileType.Empty;
        map[1, 0].Type = TileType.Empty;
        map[2, 0].Type = TileType.Empty;
        map[3, 0].Type = TileType.Empty;

        map[0, 1].Type = TileType.Empty;
        map[1, 1].Type = TileType.Player;
        map[2, 1].Type = TileType.Empty;
        map[3, 1].Type = TileType.Unknown;

        map[0, 2].Type = TileType.Empty;
        map[1, 2].Type = TileType.Empty;
        map[2, 2].Type = TileType.Empty;
        map[3, 2].Type = TileType.Wall;

        map[0, 3].Type = TileType.Empty;
        map[1, 3].Type = TileType.Empty;
        map[2, 3].Type = TileType.Wall;
        map[3, 3].Type = TileType.Unknown;

        swoq2025.Objectives.MapExplorer explorer = new(map, game.Player);
        Assert.IsTrue(explorer.TryGetNextTarget(out Coord next));
        Assert.IsFalse(explorer.IsCompleted);
        Assert.IsFalse(explorer.HasToBeSolved);
        Assert.AreEqual(0, explorer.Priority);
        game.Player.Position = next;

        Assert.IsTrue(explorer.TryGetNextTarget(out next));
        Assert.IsFalse(explorer.IsCompleted);
        Assert.IsFalse(explorer.HasToBeSolved);
        Assert.AreEqual(0, explorer.Priority);
        game.Player.Position = next;

        Assert.IsFalse(explorer.TryGetNextTarget(out next));

        Assert.IsTrue(explorer.IsCompleted);
        Assert.IsFalse(explorer.HasToBeSolved);
        Assert.AreEqual(0, explorer.Priority);
    }
}
