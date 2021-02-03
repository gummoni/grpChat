using Grpc.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace grpcChatClient
{
    class Program
    {
        const int Port = 50051;

        static void Main(string[] args)
        {
            Console.Write("username :");
            var userName = Console.ReadLine();

            var channel = new Channel($"127.0.0.1:{Port}", ChannelCredentials.Insecure);
            var chatClient = new SimpleChat.SimpleChatClient(channel);
            var botclient = new SimpleBot.SimpleBotClient(channel);

            var tokenSource = new CancellationTokenSource();
            Parallel.Invoke(
            async () =>
            {
                try
                {
                    using (var call = chatClient.Join(new JoinRequest() { Name = userName }, null, null, tokenSource.Token))
                    {
                        await foreach (var message in call.ResponseStream.ReadAllAsync(tokenSource.Token))
                        {
                            Console.WriteLine($">>> {message.Message}");
                        }
                    }
                }
                catch (RpcException re)
                {
                    Console.Write(re.StatusCode);
                }
                tokenSource.Cancel();
            },
            () =>
            {
                while (!tokenSource.IsCancellationRequested)
                {
                    var message = Console.ReadLine();
                    if (string.IsNullOrEmpty(message)) break;
                    if (message == "BOT")
                    {
                        var response = botclient.DelaySend(new DelaySendRequest { Name = "HELLO" });
                        Console.WriteLine($"Delay > {response.Message}");
                    }
                    else
                    {
                        chatClient.Send(new SendRequest() { Message = message });
                    }
                }
                tokenSource.Cancel();
            });
        }
    }
}
