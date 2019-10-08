using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTestz
{
    public interface ITypedHubClient
    {
        Task BroadcastMessage(string type, Message payload);
    }
}
