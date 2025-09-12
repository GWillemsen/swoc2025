using System.Drawing;
using Swoq.Interface;

namespace swoq2025;

public class GameActor
{
    private readonly Game game;

    bool isMovingEast = true;

    public GameActor(Game game)
    {
        this.game = game;
    }

    public void UpdateState(State state)
    {
        Map newMap = Map.FromState(game.ViewDistance, state);
        if (game.Level != state.Level)
        {
            game.Map.Reset();
        }

        game.Map.MergeMap(newMap, game.PlayerPosition.X, game.PlayerPosition.Y);
        game.PlayerPosition = new Point(state.PlayerState.Position.X, state.PlayerState.Position.Y);
        game.Status = state.Status;
        game.Level = state.Level;
        game.Tick = state.Tick;
    }

    public DirectedAction GetAction()
    {
        var action = isMovingEast ? DirectedAction.MoveEast : DirectedAction.MoveSouth;
        isMovingEast = !isMovingEast;
        return action;
    }
}
