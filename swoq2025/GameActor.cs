using System.Drawing;
using Swoq.Interface;

namespace swoq2025;

public class GameActor
{
    private readonly Game game;
    private readonly Router router;
    private Coord? target = null;

    private readonly MapExplorer explorer;

    public GameActor(Game game)
    {
        this.game = game;
        router = new Router(game.Map);
        explorer = new MapExplorer(game.Map, game);
    }

    public void UpdateState(State state)
    {
        int totalWidth = 1 + (game.ViewDistance * 2);
        Map newMap = Map.FromState(totalWidth, state);
        if (game.Level != state.Level)
        {
            game.Map.Reset();
        }

        Console.WriteLine("-");
        game.Player.Position = new Coord(state.PlayerState.Position.X, state.PlayerState.Position.Y);
        game.Player.Inventory = state.PlayerState.Inventory;
        game.Map.MergeMap(newMap, game.Player.Position.X, game.Player.Position.Y);
        game.Status = state.Status;
        game.Level = state.Level;
        game.Tick = state.Tick;
    }

    public DirectedAction GetAction()
    {
        Coord? exit = getExit();
        Coord next;
        if (exit != null)
        {
            target = exit;
            var path = router.FindPath(game.Player.Position, target.Value);
            next = path.First();
        }
        else
        {
            if (!explorer.TryGetNextTarget(out next))
            {
                Console.WriteLine("No more unknown tiles");
                next = game.Player.Position;
            }
        }

        return GetDirectionToTarget(next);
    }

    private DirectedAction GetDirectionToTarget(Coord target)
    {
        DirectedAction action = DirectedAction.None;
        var dx = target.X - game.Player.Position.X;
        var dy = target.Y - game.Player.Position.Y;

        if (Math.Abs(dx) > Math.Abs(dy))
        {
            action = dx > 0 ? DirectedAction.MoveEast : DirectedAction.MoveWest;
        }
        else if (dy != 0)
        {
            action = dy > 0 ? DirectedAction.MoveSouth : DirectedAction.MoveNorth;
        }
        return action;
    }

    private Coord? getExit()
    {
        for (int y = 0; y < game.Map.Height; ++y)
        {
            for (int x = 0; x < game.Map.Width; ++x)
            {
                if (game.Map[x, y].Type == Swoq.Interface.Tile.Exit)
                {
                    return new Coord(x, y);
                }
            }
        }
        return null;
    }
}
