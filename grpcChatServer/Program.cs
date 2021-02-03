using Grpc.Core;
using System;

namespace grpcChatServer
{
    class Program
    {
        const int Port = 50051;
        static Server Server;

        static void Main(string[] args)
        {
            Server = new Server
            {
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            Server.Services.Add(SimpleChat.BindService((SimpleChat.SimpleChatBase)Activator.CreateInstance(typeof(SimpleChatService))));
            Server.Services.Add(SimpleBot.BindService((SimpleBot.SimpleBotBase)Activator.CreateInstance(typeof(SimpleBotService))));
            Server.Start();

            Console.WriteLine($"Server start localhost:{Port}");
            Console.ReadLine();
        }
    }
}
