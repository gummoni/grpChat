using Grpc.Core;
using grpcChatProto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace grpcChatServer
{
    class SimpleChatService : SimpleChat.SimpleChatBase
    {
        readonly List<User> Users = new List<User>();

        void Report(string message)
        {
            var reply = new JoinReply() { Message = message };
            Users.Select(_ => _.Report(reply)).WaitAll();
            Console.WriteLine(message);
        }

        void Subscribe(User user)
        {
            Users.Add(user);
            Report($"Enter {user.Name}");
        }
        void Unsubscribe(User user)
        {
            Users.Remove(user);
            Report($"Leave {user.Name}");
        }

        public override Task Join(JoinRequest request, IServerStreamWriter<JoinReply> responseStream, ServerCallContext context)
        {
            var user = new User(context.Peer, request.Name, responseStream, Unsubscribe);
            Subscribe(user);

            while (!context.CancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(1000);
            }
            Unsubscribe(user);
            return Task.Delay(1);
        }

        public override Task<Empty> Send(SendRequest request, ServerCallContext context)
        {
            var user = Users.First(_ => _.Peer == context.Peer);
            var message = $"{user.Name}: {request.Message}";
            Report(message);
            return Task.FromResult(new Empty());
        }
    }
}
