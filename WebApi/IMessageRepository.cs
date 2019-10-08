using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTestz
{
    public interface IMessageRepository
    {
        IEnumerable<Message> GetMessages();
        void AddMessage(Message message);
        void UpdateMessage(Message message);

        void DeleteMessage(string messageId);
    }
}
