namespace tester;

using Swoq.Interface;
using swoq2025;

using TileType = Swoq.Interface.Tile;

[TestClass]
public sealed class DoorSolver
{
    [TestMethod]
    public void NoKeyNoDoor()
    { 
        Map map = new(3, 3);
        Game game = new("", map, null, 3)
        {
            Player = new(new(1, 1), Inventory.None)
        };
        map[0, 0].Type = TileType.Wall;
        map[1, 0].Type = TileType.Wall;
        map[2, 0].Type = TileType.Empty;

        map[0, 1].Type = TileType.Empty;
        map[1, 1].Type = TileType.Player;
        map[2, 1].Type = TileType.Empty;

        map[0, 2].Type = TileType.Empty;
        map[1, 2].Type = TileType.Empty;
        map[2, 2].Type = TileType.Empty;

        swoq2025.Objectives.DoorSolver solver = new(map, game.Player, TileType.DoorRed, TileType.KeyRed);
        Assert.IsFalse(solver.TryGetNextTarget(out Coord next, out bool use));
        Assert.IsFalse(solver.IsCompleted);
        Assert.IsFalse(solver.HasToBeSolved);
        Assert.AreEqual(3, solver.Priority);
    }

    [TestMethod]
    public void NoKeyOnlyDoor()
    { 
        Map map = new(3, 3);
        Game game = new("", map, null, 3)
        {
            Player = new(new(1, 1), Inventory.None)
        };
        map[0, 0].Type = TileType.Wall;
        map[1, 0].Type = TileType.Wall;
        map[2, 0].Type = TileType.Empty;

        map[0, 1].Type = TileType.Empty;
        map[1, 1].Type = TileType.Player;
        map[2, 1].Type = TileType.Empty;

        map[0, 2].Type = TileType.Empty;
        map[1, 2].Type = TileType.Empty;
        map[2, 2].Type = TileType.DoorRed;

        swoq2025.Objectives.DoorSolver solver = new(map, game.Player, TileType.DoorRed, TileType.KeyRed);
        Assert.IsFalse(solver.TryGetNextTarget(out Coord next, out bool use));
        Assert.IsFalse(solver.IsCompleted);
        Assert.IsTrue(solver.HasToBeSolved);
        Assert.AreEqual(3, solver.Priority);
    }

    [TestMethod]
    public void KeyAndDoor()
    { 
        Map map = new(3, 3);
        Game game = new("", map, null, 3)
        {
            Player = new(new(1, 1), Inventory.None)
        };
        map[0, 0].Type = TileType.Wall;
        map[1, 0].Type = TileType.Wall;
        map[2, 0].Type = TileType.KeyRed;

        map[0, 1].Type = TileType.Empty;
        map[1, 1].Type = TileType.Player;
        map[2, 1].Type = TileType.Empty;

        map[0, 2].Type = TileType.DoorRed;
        map[1, 2].Type = TileType.Empty;
        map[2, 2].Type = TileType.Empty;

        swoq2025.Objectives.DoorSolver solver = new(map, game.Player, TileType.DoorRed, TileType.KeyRed);
        Assert.IsTrue(solver.TryGetNextTarget(out Coord next, out bool use));
        Assert.IsFalse(use);
        Assert.AreEqual(new Coord(2, 1), next);
        game.Player.Position = next;

        Assert.IsTrue(solver.TryGetNextTarget(out next, out use));
        Assert.IsFalse(use);
        Assert.AreEqual(new Coord(2, 0), next);
        game.Player.Position = next;

        map[2, 0].Type = TileType.Empty;

        Assert.IsTrue(solver.TryGetNextTarget(out next, out use));
        Assert.IsFalse(use);
        Assert.AreEqual(new Coord(2, 1), next);
        game.Player.Position = next;

        Assert.IsTrue(solver.TryGetNextTarget(out next, out use));
        Assert.IsFalse(use);
        Assert.AreEqual(new Coord(2, 2), next);
        game.Player.Position = next;

        Assert.IsTrue(solver.TryGetNextTarget(out next, out use));
        Assert.IsFalse(use);
        Assert.AreEqual(new Coord(1, 2), next);
        game.Player.Position = next;

        Assert.IsTrue(solver.TryGetNextTarget(out next, out use));
        Assert.IsTrue(use);
        Assert.AreEqual(new Coord(0, 2), next);
        game.Player.Position = next;


        map[0, 2].Type = TileType.Empty;

        Assert.IsTrue(solver.IsCompleted);
        Assert.IsFalse(solver.HasToBeSolved);
        Assert.AreEqual(3, solver.Priority);
    }

    [TestMethod]
    public void KeyAndDoor_Partial_GoToKey()
    { 
        Map map = new(3, 3);
        Game game = new("", map, null, 3)
        {
            Player = new(new(1, 1), Inventory.None)
        };
        map[0, 0].Type = TileType.Wall;
        map[1, 0].Type = TileType.Wall;
        map[2, 0].Type = TileType.KeyRed;

        map[0, 1].Type = TileType.Empty;
        map[1, 1].Type = TileType.Player;
        map[2, 1].Type = TileType.Empty;

        map[0, 2].Type = TileType.DoorRed;
        map[1, 2].Type = TileType.Empty;
        map[2, 2].Type = TileType.Empty;

        swoq2025.Objectives.DoorSolver solver = new(map, game.Player, TileType.DoorRed, TileType.KeyRed);
        Assert.IsTrue(solver.TryGetNextTarget(out Coord next, out bool use));
        Assert.AreEqual(new Coord(2, 1), next);
        game.Player.Position = next;

        Assert.IsTrue(solver.TryGetNextTarget(out next, out use));
        Assert.AreEqual(new Coord(2, 0), next);
        game.Player.Position = next;

        map[2, 0].Type = TileType.Empty;

        Assert.IsFalse(solver.IsCompleted);
        Assert.IsTrue(solver.HasToBeSolved);
        Assert.AreEqual(3, solver.Priority);
    }
}