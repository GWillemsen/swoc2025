using Swoq.Interface;
using swoq2025.Objectives;

namespace swoq2025;

public class GameActor
{
    private readonly Game game;
    private readonly Router router;
    private Coord? target = null;

    private readonly List<IObjective> objectives = [];

    public GameActor(Game game)
    {
        this.game = game;
        router = new Router(game.Map);
        objectives.Add(new MapExplorer(game.Map, game.Player));
        objectives.Add(new ExitFinder(game.Map, game.Player));
        objectives.Add(new DoorSolver(game.Map, game.Player, Swoq.Interface.Tile.DoorRed, Swoq.Interface.Tile.KeyRed));
        objectives.Add(new DoorSolver(game.Map, game.Player, Swoq.Interface.Tile.DoorBlue, Swoq.Interface.Tile.KeyBlue));
        objectives.Add(new DoorSolver(game.Map, game.Player, Swoq.Interface.Tile.DoorGreen, Swoq.Interface.Tile.KeyGreen));
    }

    public void UpdateState(State state)
    {
        int totalWidth = 1 + (game.ViewDistance * 2);
        Map newMap = Map.FromState(totalWidth, state);
        if (game.Level != state.Level)
        {
            game.Map.Reset();
            foreach (var obj in objectives)
            {
                obj.Reset();
            }
        }
        game.Player.Position = new Coord(state.PlayerState.Position.X, state.PlayerState.Position.Y);
        game.Player.Inventory = state.PlayerState.Inventory;
        game.Map.MergeMap(newMap, game.Player.Position.X, game.Player.Position.Y);
        game.Status = state.Status;
        game.Level = state.Level;
        game.Tick = state.Tick;
    }

    public DirectedAction GetAction()
    {
        var objectivesByPriority = objectives
            .Where(o => o.CanBeSolved || o.HasToBeSolved)
            .Where(o => !o.IsCompleted)
            .OrderByDescending(o => o.Priority)
            .ToList();

        Coord? next = null;
        bool useAction = false;
        if (objectivesByPriority.Count > 0)
        {
            foreach (var solver in objectivesByPriority)
            {
                if (!solver.TryGetNextTarget(out Coord newNext, out useAction))
                {
                    continue;
                }
                else
                {
                    target = newNext;
                    next = newNext;
                    break;
                }
            }
        }

        if (next.HasValue)
        {
            return GetDirectionToTarget(next.Value, useAction);
        }
        else
        {
            Console.WriteLine("No objective to solve");
            return DirectedAction.None;
        }
    }

    private DirectedAction GetDirectionToTarget(Coord target, bool use)
    {
        DirectedAction action = DirectedAction.None;
        var dx = target.X - game.Player.Position.X;
        var dy = target.Y - game.Player.Position.Y;

        if (Math.Abs(dx) > Math.Abs(dy))
        {
            if (dx > 0)
            {
                action = use ? DirectedAction.UseEast : DirectedAction.MoveEast;
            }
            else
            {
                action = use ? DirectedAction.UseWest : DirectedAction.MoveWest;
            }
        }
        else if (dy != 0)
        {
            if (dy > 0)
            {
                action = use ? DirectedAction.UseSouth : DirectedAction.MoveSouth;
            }
            else
            {
                action = use ? DirectedAction.UseNorth : DirectedAction.MoveNorth;
            }
        }
        return action;
    }
}
