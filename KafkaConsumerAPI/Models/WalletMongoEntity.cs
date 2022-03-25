using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KafkaConsumerAPI.Models
{
    public class WalletMongoEntity
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement("walletAmount")]
        public long WalletBalance { get; set; }

    }
}
