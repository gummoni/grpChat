using Grpc.Core;
using grpcChatProto;
using System;
using System.Linq;
using System.Reflection;

namespace grpcChatServer
{
    class Program
    {
        const int Port = 50051;

        static void Main(string[] args)
        {
            var server = new Server
            {
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            //var types = Assembly.GetExecutingAssembly().GetTypes();
            //var types2 = types
            //    .Where(_ => _.IsSubclassOf(typeof(SimpleChat.SimpleChatBase))).ToArray();
            //var value3 = types2
            //    .Select(_ => SimpleChat.BindService((SimpleChat.SimpleChatBase)Activator.CreateInstance(_))).ToArray();
            //value3
            //    .ForEach(server.Services.Add);

            server.Services.Add(SimpleChat.BindService((SimpleChat.SimpleChatBase)Activator.CreateInstance(typeof(SimpleChatService))));
            server.Services.Add(SimpleBot.BindService((SimpleBot.SimpleBotBase)Activator.CreateInstance(typeof(SimpleBotService))));

            server.Start();

            Console.WriteLine($"Server start localhost:{Port}");
            Console.ReadLine();
        }
    }
}
