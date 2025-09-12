using Swoq.Interface;
using swoq2025;

DotEnv.Load();

string userId = Environment.GetEnvironmentVariable("SWOQ_USER_ID") ?? throw new ArgumentException("SWOQ_USER_ID has to be set.");
string userName = Environment.GetEnvironmentVariable("SWOQ_USER_NAME") ?? throw new ArgumentException("SWOQ_USER_NAME has to be set.");
string host = Environment.GetEnvironmentVariable("SWOQ_HOST") ?? throw new ArgumentException("SWOQ_HOST has to be set.");

var connection = new GameConnection(userId, userName, host);

(StartResponse startResponse, GameService.GameServiceClient client) = await connection.StartAsync();
Game game = new(
    startResponse.GameId,
    new Map(startResponse.MapWidth, startResponse.MapHeight),
    startResponse.Seed,
    startResponse.VisibilityRange
);
GameActor actor = new(game);

while (game.Status == GameStatus.Active)
{
    var action = actor.GetAction();
    Console.WriteLine($"tick: {game.Tick}, action: {action}");

    var request = new ActRequest()
    {
        GameId = game.Id,
        Action = action
    };
    var response = await client.ActAsync(request);
    actor.UpdateState(response.State);
}

