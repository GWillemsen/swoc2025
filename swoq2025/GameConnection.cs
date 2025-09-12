using System.Threading.Tasks;
using Grpc.Net.Client;
using Swoq.Interface;

namespace swoq2025;

public class GameConnection
{
    private readonly string userId;
    private readonly string userName;

    private readonly GrpcChannel channel;
    private readonly GameService.GameServiceClient client;

    public GameConnection(string userId, string userName, string host)
    {
        this.userId = userId;
        this.userName = userName;

        channel = GrpcChannel.ForAddress($"http://{host}");
        client = new(channel);
    }

    public void Dispose()
    {
        channel.Dispose();
    }

    public async Task<(StartResponse, GameService.GameServiceClient)> StartAsync(int? level = null, int? seed = null)
    {
        var request = new StartRequest()
        {
            UserId = userId,
            UserName = userName
        };

        if (level.HasValue)
        {
            request.Level = level.Value;
        }

        if (seed.HasValue)
        {
            request.Seed = seed.Value;
        }
        
        var response = await client.StartAsync(request);
        while (response.Result == StartResult.QuestQueued)
        {
            Console.WriteLine("Quest queued, retrying ...");
            response = client.Start(request);
        }
        if (response.Result != StartResult.Ok)
        {
            throw new Exception($"Start failed (result {response.Result})");
        }

        return (response, client);
    }
}