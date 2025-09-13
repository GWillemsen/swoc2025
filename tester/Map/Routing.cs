namespace tester;

using Swoq.Interface;
using swoq2025;

using TileType = Swoq.Interface.Tile;

[TestClass]
public sealed class Router
{
    private readonly Map map;
    private readonly swoq2025.Router router;
    public Router()
    {
        map = new Map(4, 4);
        router = new swoq2025.Router(map);
    }

    [TestMethod]
    public void SimpleOpenPath_ReturnsPath_NoObstacles()
    {
        var path = router.FindPath(new Coord(0, 0), new Coord(3, 3));
        Assert.AreEqual(6, path.Count);

        Assert.AreEqual(0, path[0].X);
        Assert.AreEqual(1, path[0].Y);

        Assert.AreEqual(1, path[1].X);
        Assert.AreEqual(1, path[1].Y);

        Assert.AreEqual(2, path[2].X);
        Assert.AreEqual(1, path[2].Y);

        Assert.AreEqual(3, path[3].X);
        Assert.AreEqual(1, path[3].Y);

        Assert.AreEqual(3, path[4].X);
        Assert.AreEqual(2, path[4].Y);

        Assert.AreEqual(3, path[5].X);
        Assert.AreEqual(3, path[5].Y);
    }

    [TestMethod]
    public void SimpleOpenPath_ReturnsPath_WithWall()
    {
        // 
        //   --
        // S |E
        //   --
        //

        map[2, 1].Type = TileType.Wall;
        map[1, 1].Type = TileType.Wall;

        map[1, 2].Type = TileType.Wall;

        map[1, 3].Type = TileType.Wall;
        map[2, 3].Type = TileType.Wall;

        // map[3, 1].Type = TileType.Wall;
        // map[3, 2].Type = TileType.Wall;

        var path = router.FindPath(new Coord(0, 2), new Coord(2, 2));

        Assert.AreEqual(8, path.Count);

        Assert.AreEqual(0, path[0].X);
        Assert.AreEqual(1, path[0].Y);

        Assert.AreEqual(0, path[1].X);
        Assert.AreEqual(0, path[1].Y);

        Assert.AreEqual(1, path[2].X);
        Assert.AreEqual(0, path[2].Y);

        Assert.AreEqual(2, path[3].X);
        Assert.AreEqual(0, path[3].Y);

        Assert.AreEqual(3, path[4].X);
        Assert.AreEqual(0, path[4].Y);

        Assert.AreEqual(3, path[5].X);
        Assert.AreEqual(1, path[5].Y);

        Assert.AreEqual(3, path[6].X);
        Assert.AreEqual(2, path[6].Y);

        Assert.AreEqual(2, path[7].X);
        Assert.AreEqual(2, path[7].Y);
    }

    [TestMethod]
    public void WithWall_ReturnsNoPath()
    {
        map[1, 0].Type = TileType.Wall;
        map[1, 1].Type = TileType.Wall;
        map[1, 2].Type = TileType.Wall;
        map[1, 3].Type = TileType.Wall;
        var path = router.FindPath(new Coord(0, 1), new Coord(3, 1));
        Assert.AreEqual(0, path.Count, "Path should not be found through wall");
    }

    [TestMethod]
    public void AvoidsPlayerAndExit()
    {
        map[0, 2].Type = TileType.Player;
        map[1, 3].Type = TileType.Exit;
        var path = router.FindPath(new Coord(0, 0), new Coord(0, 3));
        Assert.AreEqual(0, path.Count, "Path should not be found through player or exit");
    }

    [TestMethod]
    public void SimpleSquareWallWithExit()
    {
        var largeMap = new Map(6, 6);
        largeMap[0, 1].Type = TileType.Wall;
        largeMap[0, 2].Type = TileType.Wall;
        largeMap[0, 3].Type = TileType.Wall;
        largeMap[0, 4].Type = TileType.Wall;

        largeMap[1, 0].Type = TileType.Wall;
        largeMap[2, 0].Type = TileType.Wall;
        largeMap[3, 0].Type = TileType.Wall;
        largeMap[4, 0].Type = TileType.Wall;

        largeMap[5, 1].Type = TileType.Wall;
        largeMap[5, 2].Type = TileType.Wall;
        largeMap[5, 3].Type = TileType.Wall;
        largeMap[5, 4].Type = TileType.Exit;

        largeMap[1, 5].Type = TileType.Wall;
        largeMap[2, 5].Type = TileType.Wall;
        largeMap[3, 5].Type = TileType.Wall;
        largeMap[4, 5].Type = TileType.Wall;

        largeMap[2, 4].Type = TileType.Player;

        var router = new swoq2025.Router(largeMap);

        var path = router.FindPath(new Coord(2, 4), new Coord(4, 5));
        
        Assert.AreEqual(3, path.Count);
    }
}
