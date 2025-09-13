namespace tester;

using Swoq.Interface;
using swoq2025;

using TileType = Swoq.Interface.Tile;

[TestClass]
public sealed class MapUpdateFlag
{
    [TestMethod]
    public void UpdateBoolResetBetweenUpdates()
    {
        Map map1 = new(5, 5);
        Map map2 = new(3,
        [
            new() { Type = TileType.Wall },
            new() { Type = TileType.Wall },
            new() { Type = TileType.Wall },
            new() { Type = TileType.Wall },
            new() { Type = TileType.Player },
            new() { Type = TileType.Wall },
            new() { Type = TileType.Wall },
            new() { Type = TileType.Wall },
            new() { Type = TileType.Wall },
        ]);

        map1.MergeMap(map2, 2, 2);

        Assert.IsFalse(map1.WasUpdated(0, 0));
        Assert.IsFalse(map1.WasUpdated(1, 0));
        Assert.IsFalse(map1.WasUpdated(2, 0));
        Assert.IsFalse(map1.WasUpdated(3, 0));
        Assert.IsFalse(map1.WasUpdated(4, 0));

        Assert.IsFalse(map1.WasUpdated(0, 1));
        Assert.IsTrue(map1.WasUpdated(1, 1));
        Assert.IsTrue(map1.WasUpdated(2, 1));
        Assert.IsTrue(map1.WasUpdated(3, 1));
        Assert.IsFalse(map1.WasUpdated(4, 1));

        Assert.IsFalse(map1.WasUpdated(0, 2));
        Assert.IsTrue(map1.WasUpdated(1, 2));
        Assert.IsTrue(map1.WasUpdated(2, 2));
        Assert.IsTrue(map1.WasUpdated(3, 2));
        Assert.IsFalse(map1.WasUpdated(4, 2));

        Assert.IsFalse(map1.WasUpdated(0, 3));
        Assert.IsTrue(map1.WasUpdated(1, 3));
        Assert.IsTrue(map1.WasUpdated(2, 3));
        Assert.IsTrue(map1.WasUpdated(3, 3));
        Assert.IsFalse(map1.WasUpdated(4, 3));

        Assert.IsFalse(map1.WasUpdated(0, 4));
        Assert.IsFalse(map1.WasUpdated(1, 4));
        Assert.IsFalse(map1.WasUpdated(2, 4));
        Assert.IsFalse(map1.WasUpdated(3, 4));
        Assert.IsFalse(map1.WasUpdated(4, 4));
    }

    [TestMethod]
    public void UpdateBoolResetBetweenTwoUpdates()
    {
        Map map1 = new(5, 5);
        Map map2 = new(3,
        [
            new() { Type = TileType.Wall },
            new() { Type = TileType.Wall },
            new() { Type = TileType.Wall },
            new() { Type = TileType.Wall },
            new() { Type = TileType.Player },
            new() { Type = TileType.Wall },
            new() { Type = TileType.Wall },
            new() { Type = TileType.Wall },
            new() { Type = TileType.Wall },
        ]);
        Map map3 = new(1,
        [
            new() { Type = TileType.Wall },
        ]);

        map1.MergeMap(map2, 2, 2);
        map1.MergeMap(map3, 2, 2);

        Assert.IsFalse(map1.WasUpdated(0, 0));
        Assert.IsFalse(map1.WasUpdated(1, 0));
        Assert.IsFalse(map1.WasUpdated(2, 0));
        Assert.IsFalse(map1.WasUpdated(3, 0));
        Assert.IsFalse(map1.WasUpdated(4, 0));

        Assert.IsFalse(map1.WasUpdated(0, 1));
        Assert.IsFalse(map1.WasUpdated(1, 1));
        Assert.IsFalse(map1.WasUpdated(2, 1));
        Assert.IsFalse(map1.WasUpdated(3, 1));
        Assert.IsFalse(map1.WasUpdated(4, 1));

        Assert.IsFalse(map1.WasUpdated(0, 2));
        Assert.IsFalse(map1.WasUpdated(1, 2));
        Assert.IsTrue(map1.WasUpdated(2, 2));
        Assert.IsFalse(map1.WasUpdated(3, 2));
        Assert.IsFalse(map1.WasUpdated(4, 2));

        Assert.IsFalse(map1.WasUpdated(0, 3));
        Assert.IsFalse(map1.WasUpdated(1, 3));
        Assert.IsFalse(map1.WasUpdated(2, 3));
        Assert.IsFalse(map1.WasUpdated(3, 3));
        Assert.IsFalse(map1.WasUpdated(4, 3));

        Assert.IsFalse(map1.WasUpdated(0, 4));
        Assert.IsFalse(map1.WasUpdated(1, 4));
        Assert.IsFalse(map1.WasUpdated(2, 4));
        Assert.IsFalse(map1.WasUpdated(3, 4));
        Assert.IsFalse(map1.WasUpdated(4, 4));
    }
}
