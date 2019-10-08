using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRTestz
{
    public class MessageRepository : IMessageRepository
    { 
        private readonly IMongoCollection<Message> _messages;

        public MessageRepository(IMessageDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _messages = database.GetCollection<Message>(settings.MessageCollectionName);
        }

        public void AddMessage(Message message)
        {
            _messages.InsertOne(message);
        }

        public void DeleteMessage(string messageId)
        {
            _messages.DeleteOne(Builders<Message>.Filter.Eq("Id", messageId));
        }

        public IEnumerable<Message> GetMessages()
        {
            return _messages.Find(book => true).ToList();
        }

        public void UpdateMessage(Message message)
        {
            _messages.UpdateOne(Builders<Message>.Filter.Eq("Id", message.Id), Builders<Message>.Update.Set("Payload", message.Payload));
            _messages.UpdateOne(Builders<Message>.Filter.Eq("Id", message.Id), Builders<Message>.Update.Set("Edited", true));
        }
    }
}