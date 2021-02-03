using Grpc.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace grpcChatServer
{
    class SimpleBotService : SimpleBot.SimpleBotBase
    {
        public override Task<DelaySendReply> DelaySend(DelaySendRequest request, ServerCallContext context)
        {
            var response = $"{request.Name} : Delay Send";
            Thread.Sleep(1000);
            return Task.FromResult(new DelaySendReply() { Message = response });
        }
    }
}
