namespace tester;

using swoq2025;

[TestClass]
public sealed class UnknownTileFilterTests
{
    [TestMethod]
    public void UnknownIsDirectNeighborOfPlayer()
    {
        Map map = new(3, 3);
        map[0, 0].Type = Swoq.Interface.Tile.Wall;
        map[1, 0].Type = Swoq.Interface.Tile.Wall;
        map[2, 0].Type = Swoq.Interface.Tile.Wall;

        map[0, 1].Type = Swoq.Interface.Tile.Wall;
        map[1, 1].Type = Swoq.Interface.Tile.Player;
        map[2, 1].Type = Swoq.Interface.Tile.Wall;

        map[0, 2].Type = Swoq.Interface.Tile.Wall;
        map[1, 2].Type = Swoq.Interface.Tile.Unknown;
        map[2, 2].Type = Swoq.Interface.Tile.Wall;

        UnknownTileFilter filter = new(map);
        var unknownTiles = filter.GetUnknownTiles(new Coord(1, 1));

        Assert.AreEqual(0, unknownTiles.Count);
    }

    [TestMethod]
    public void UnknownIsDirectNeighborOfEmpty()
    {
        Map map = new(3, 3);
        map[0, 0].Type = Swoq.Interface.Tile.Wall;
        map[1, 0].Type = Swoq.Interface.Tile.Player;
        map[2, 0].Type = Swoq.Interface.Tile.Wall;

        map[0, 1].Type = Swoq.Interface.Tile.Wall;
        map[1, 1].Type = Swoq.Interface.Tile.Empty;
        map[2, 1].Type = Swoq.Interface.Tile.Wall;

        map[0, 2].Type = Swoq.Interface.Tile.Wall;
        map[1, 2].Type = Swoq.Interface.Tile.Unknown;
        map[2, 2].Type = Swoq.Interface.Tile.Wall;

        UnknownTileFilter filter = new(map);
        var unknownTiles = filter.GetUnknownTiles(new Coord(1, 0));

        Assert.AreEqual(1, unknownTiles.Count);
        Assert.AreEqual(new Coord(1, 2), unknownTiles[0]);
    }

    [TestMethod]
    public void SomeUnknownIsBehindWall()
    {
        Map map = new(3, 3);
        map[0, 0].Type = Swoq.Interface.Tile.Unknown;
        map[1, 0].Type = Swoq.Interface.Tile.Player;
        map[2, 0].Type = Swoq.Interface.Tile.Unknown;

        map[0, 1].Type = Swoq.Interface.Tile.Wall;
        map[1, 1].Type = Swoq.Interface.Tile.Empty;
        map[2, 1].Type = Swoq.Interface.Tile.Wall;

        map[0, 2].Type = Swoq.Interface.Tile.Wall;
        map[1, 2].Type = Swoq.Interface.Tile.Unknown;
        map[2, 2].Type = Swoq.Interface.Tile.Unknown;

        UnknownTileFilter filter = new(map);
        var unknownTiles = filter.GetUnknownTiles(new Coord(1, 0));

        Assert.AreEqual(1, unknownTiles.Count);
        Assert.AreEqual(new Coord(1, 2), unknownTiles[0]);
    }
    [TestMethod]
    public void OnlyFindNextToEmpty()
    {
        Map map = new(4, 4);
        map[0, 0].Type = Swoq.Interface.Tile.Wall;
        map[1, 0].Type = Swoq.Interface.Tile.Player;
        map[2, 0].Type = Swoq.Interface.Tile.Wall;
        map[3, 0].Type = Swoq.Interface.Tile.Unknown;

        map[0, 1].Type = Swoq.Interface.Tile.Unknown;
        map[1, 1].Type = Swoq.Interface.Tile.Empty;
        map[2, 1].Type = Swoq.Interface.Tile.Unknown;
        map[3, 1].Type = Swoq.Interface.Tile.Unknown;

        map[0, 2].Type = Swoq.Interface.Tile.Unknown;
        map[1, 2].Type = Swoq.Interface.Tile.Empty;
        map[2, 2].Type = Swoq.Interface.Tile.Unknown;
        map[3, 2].Type = Swoq.Interface.Tile.Unknown;

        map[0, 3].Type = Swoq.Interface.Tile.Unknown;
        map[1, 3].Type = Swoq.Interface.Tile.Unknown;
        map[2, 3].Type = Swoq.Interface.Tile.Unknown;
        map[3, 3].Type = Swoq.Interface.Tile.Unknown;

        UnknownTileFilter filter = new(map);
        var unknownTiles = filter.GetUnknownTiles(new Coord(1, 0));

        Assert.AreEqual(5, unknownTiles.Count);
        Assert.AreEqual(new Coord(0, 1), unknownTiles[0]);
        Assert.AreEqual(new Coord(0, 2), unknownTiles[1]);
        Assert.AreEqual(new Coord(2, 1), unknownTiles[2]);
        Assert.AreEqual(new Coord(2, 2), unknownTiles[3]);
        Assert.AreEqual(new Coord(1, 3), unknownTiles[4]);
    }
}