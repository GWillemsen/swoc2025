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
        var objectivesByPriority = objectives
            .Where(o => o.CanBeSolved || o.HasToBeSolved)
            .Where(o => !o.IsCompleted)
            .OrderBy(o => o.Priority);

        Coord next;
        bool useAction = false;
        if (objectivesByPriority.Any())
        {
            var objective = objectivesByPriority.First();
            if (!objective.TryGetNextTarget(out next, out useAction))
            {
                Console.WriteLine("Objective cannot be solved");
                return DirectedAction.None;
            }
            target = next;
        }
        else
        {
            Console.WriteLine("No objective to solve");
            return DirectedAction.None;
        }

        return GetDirectionToTarget(next, useAction);
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
