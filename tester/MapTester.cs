namespace tester;

using Swoq.Interface;
using swoq2025;

using TileType = Swoq.Interface.Tile;

[TestClass]
public sealed class MapTester
{
    [TestMethod]
    public void EmptyMap()
    {
        Map map = new(4, 4);

        for (int y = 0; y < map.Height; ++y)
        {
            for (int x = 0; x < map.Width; ++x)
            {
                Assert.AreEqual(TileType.Unknown, map[x, y].Type);
            }
        }
    }
    [TestMethod]
    public void CreateMapFromState()
    {
        State state = new()
        {
            Level = 1,
            Tick = 1,
            Status = GameStatus.Active,
            PlayerState = new()
            {
                Position = new() { X = 0, Y = 0 },
            }
        };
        state.PlayerState.Surroundings.AddRange(
        [
            TileType.Unknown,
            TileType.Unknown,
            TileType.Wall,
            TileType.Wall,
            TileType.Player,
            TileType.Wall,
            TileType.Wall,
            TileType.Exit,
            TileType.Empty
        ]);
        Map map = Map.FromState(3, state);
        Assert.AreEqual(3, map.Width);
        Assert.AreEqual(3, map.Height);
        
        Assert.AreEqual(TileType.Unknown, map[0, 0].Type);
        Assert.AreEqual(TileType.Unknown, map[1, 0].Type);
        Assert.AreEqual(TileType.Wall, map[2, 0].Type);
        
        Assert.AreEqual(TileType.Wall, map[0, 1].Type);
        Assert.AreEqual(TileType.Player, map[1, 1].Type);
        Assert.AreEqual(TileType.Wall, map[2, 1].Type);
        
        Assert.AreEqual(TileType.Wall, map[0, 2].Type);
        Assert.AreEqual(TileType.Exit, map[1, 2].Type);
        Assert.AreEqual(TileType.Empty, map[2, 2].Type);
    }

    [TestMethod]
    public void MapReset()
    {
        Map map1 = new(3,
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

        map1.Reset();

        Assert.AreEqual(TileType.Unknown, map1[0, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[1, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[2, 0].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 1].Type);
        Assert.AreEqual(TileType.Unknown, map1[1, 1].Type);
        Assert.AreEqual(TileType.Unknown, map1[2, 1].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 2].Type);
        Assert.AreEqual(TileType.Unknown, map1[1, 2].Type);
        Assert.AreEqual(TileType.Unknown, map1[2, 2].Type);
    }

    [TestMethod]
    public void MergeSingleMap()
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

        Assert.AreEqual(TileType.Unknown, map1[0, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[1, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[2, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[3, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[4, 0].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 1].Type);
        Assert.AreEqual(TileType.Wall, map1[1, 1].Type);
        Assert.AreEqual(TileType.Wall, map1[2, 1].Type);
        Assert.AreEqual(TileType.Wall, map1[3, 1].Type);
        Assert.AreEqual(TileType.Unknown, map1[4, 1].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 2].Type);
        Assert.AreEqual(TileType.Wall, map1[1, 2].Type);
        Assert.AreEqual(TileType.Player, map1[2, 2].Type);
        Assert.AreEqual(TileType.Wall, map1[3, 2].Type);
        Assert.AreEqual(TileType.Unknown, map1[4, 2].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 3].Type);
        Assert.AreEqual(TileType.Wall, map1[1, 3].Type);
        Assert.AreEqual(TileType.Wall, map1[2, 3].Type);
        Assert.AreEqual(TileType.Wall, map1[3, 3].Type);
        Assert.AreEqual(TileType.Unknown, map1[4, 3].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 4].Type);
        Assert.AreEqual(TileType.Unknown, map1[1, 4].Type);
        Assert.AreEqual(TileType.Unknown, map1[2, 4].Type);
        Assert.AreEqual(TileType.Unknown, map1[3, 4].Type);
        Assert.AreEqual(TileType.Unknown, map1[4, 4].Type);
    }

    [TestMethod]
    public void MergeTwoMaps()
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
        Map map3 = new(3,
        [
            new() { Type = TileType.Exit },
            new() { Type = TileType.Empty },
            new() { Type = TileType.Empty },
            new() { Type = TileType.Empty },
            new() { Type = TileType.Player },
            new() { Type = TileType.Empty },
            new() { Type = TileType.Empty },
            new() { Type = TileType.Empty },
            new() { Type = TileType.Empty },
        ]);

        map1.MergeMap(map2, 2, 2);
        map1.MergeMap(map3, 3, 3);

        Assert.AreEqual(TileType.Unknown, map1[0, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[1, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[2, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[3, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[4, 0].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 1].Type);
        Assert.AreEqual(TileType.Wall, map1[1, 1].Type);
        Assert.AreEqual(TileType.Wall, map1[2, 1].Type);
        Assert.AreEqual(TileType.Wall, map1[3, 1].Type);
        Assert.AreEqual(TileType.Unknown, map1[4, 1].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 2].Type);
        Assert.AreEqual(TileType.Wall, map1[1, 2].Type);
        Assert.AreEqual(TileType.Exit, map1[2, 2].Type);
        Assert.AreEqual(TileType.Empty, map1[3, 2].Type);
        Assert.AreEqual(TileType.Empty, map1[4, 2].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 3].Type);
        Assert.AreEqual(TileType.Wall, map1[1, 3].Type);
        Assert.AreEqual(TileType.Empty, map1[2, 3].Type);
        Assert.AreEqual(TileType.Player, map1[3, 3].Type);
        Assert.AreEqual(TileType.Empty, map1[4, 3].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 4].Type);
        Assert.AreEqual(TileType.Unknown, map1[1, 4].Type);
        Assert.AreEqual(TileType.Empty, map1[2, 4].Type);
        Assert.AreEqual(TileType.Empty, map1[3, 4].Type);
        Assert.AreEqual(TileType.Empty, map1[4, 4].Type);
    }

    [TestMethod]
    public void MergeMap_ColumnOutsideOriginalMap()
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

        // Place map2 so its rightmost column is outside map1
        map1.MergeMap(map2, 4, 2);

        Assert.AreEqual(TileType.Unknown, map1[0, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[1, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[2, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[3, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[4, 0].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 1].Type);
        Assert.AreEqual(TileType.Unknown, map1[1, 1].Type);
        Assert.AreEqual(TileType.Unknown, map1[2, 1].Type);
        Assert.AreEqual(TileType.Wall, map1[3, 1].Type);
        Assert.AreEqual(TileType.Wall, map1[4, 1].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 2].Type);
        Assert.AreEqual(TileType.Unknown, map1[1, 2].Type);
        Assert.AreEqual(TileType.Unknown, map1[2, 2].Type);
        Assert.AreEqual(TileType.Wall, map1[3, 2].Type);
        Assert.AreEqual(TileType.Player, map1[4, 2].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 3].Type);
        Assert.AreEqual(TileType.Unknown, map1[1, 3].Type);
        Assert.AreEqual(TileType.Unknown, map1[2, 3].Type);
        Assert.AreEqual(TileType.Wall, map1[3, 3].Type);
        Assert.AreEqual(TileType.Wall, map1[4, 3].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 4].Type);
        Assert.AreEqual(TileType.Unknown, map1[1, 4].Type);
        Assert.AreEqual(TileType.Unknown, map1[2, 4].Type);
        Assert.AreEqual(TileType.Unknown, map1[3, 4].Type);
        Assert.AreEqual(TileType.Unknown, map1[4, 4].Type);
    }

    [TestMethod]
    public void MergeMapsAndUnknownOverIt()
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
        Map map3 = new(3,
        [
            new() { Type = TileType.Unknown },
            new() { Type = TileType.Unknown },
            new() { Type = TileType.Unknown },
            new() { Type = TileType.Unknown },
            new() { Type = TileType.Unknown },
            new() { Type = TileType.Unknown },
            new() { Type = TileType.Unknown },
            new() { Type = TileType.Unknown },
            new() { Type = TileType.Unknown },
        ]);

        map1.MergeMap(map2, 2, 2);
        map1.MergeMap(map3, 2, 2);

        Assert.AreEqual(TileType.Unknown, map1[0, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[1, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[2, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[3, 0].Type);
        Assert.AreEqual(TileType.Unknown, map1[4, 0].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 1].Type);
        Assert.AreEqual(TileType.Wall, map1[1, 1].Type);
        Assert.AreEqual(TileType.Wall, map1[2, 1].Type);
        Assert.AreEqual(TileType.Wall, map1[3, 1].Type);
        Assert.AreEqual(TileType.Unknown, map1[4, 1].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 2].Type);
        Assert.AreEqual(TileType.Wall, map1[1, 2].Type);
        Assert.AreEqual(TileType.Player, map1[2, 2].Type);
        Assert.AreEqual(TileType.Wall, map1[3, 2].Type);
        Assert.AreEqual(TileType.Unknown, map1[4, 2].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 3].Type);
        Assert.AreEqual(TileType.Wall, map1[1, 3].Type);
        Assert.AreEqual(TileType.Wall, map1[2, 3].Type);
        Assert.AreEqual(TileType.Wall, map1[3, 3].Type);
        Assert.AreEqual(TileType.Unknown, map1[4, 3].Type);

        Assert.AreEqual(TileType.Unknown, map1[0, 4].Type);
        Assert.AreEqual(TileType.Unknown, map1[1, 4].Type);
        Assert.AreEqual(TileType.Unknown, map1[2, 4].Type);
        Assert.AreEqual(TileType.Unknown, map1[3, 4].Type);
        Assert.AreEqual(TileType.Unknown, map1[4, 4].Type);
    }
}
