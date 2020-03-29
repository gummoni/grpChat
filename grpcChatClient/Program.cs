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
            var client = new SimpleChat.SimpleChatClient(channel);

            var tokenSource = new CancellationTokenSource();
            Parallel.Invoke(
            async () =>
            {
                try
                {
                    using (var call = client.Join(new JoinRequest() { Name = userName }, null, null, tokenSource.Token))
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
                    client.Send(new SendRequest() { Message = message }, null, null, tokenSource.Token);
                }
                tokenSource.Cancel();
            });
        }
    }
}
