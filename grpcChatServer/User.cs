using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace grpcChatServer
{
    public class User
    {
        public string Peer { get; }
        public string Name { get; }
        readonly IServerStreamWriter<JoinReply> ResponseStream;
        readonly Action<User> UnsubscribeAction;

        public User(string peer, string name, IServerStreamWriter<JoinReply> responseStream, Action<User> unsubscribeAction)
        {
            Peer = peer;
            Name = name;
            ResponseStream = responseStream;
            UnsubscribeAction = unsubscribeAction;
        }

        public Task Report(JoinReply joinReply)
        {
            try
            {
                return ResponseStream.WriteAsync(joinReply);
            }
            catch
            {
                UnsubscribeAction(this);
                return Task.Delay(1);
            }
        }
    }
}
