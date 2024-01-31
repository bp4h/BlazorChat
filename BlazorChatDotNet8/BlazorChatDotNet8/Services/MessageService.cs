using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

public class MessageService
{
    private readonly IMongoCollection<ChatMessage> _messageCollection;

    public MessageService(IMongoClient mongoClient, IOptions<MongoDbSettings> mongoDbSettings)
    {
        var database = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _messageCollection = database.GetCollection<ChatMessage>("ChatMessages");
    }

    public List<ChatMessage> GetMessages()
    {
        return _messageCollection.Find(message => true).ToList();
    }

    public void SaveMessage(ChatMessage message)
    {
        _messageCollection.InsertOne(message);
    }
}
