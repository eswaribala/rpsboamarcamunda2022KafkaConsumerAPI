using KafkaConsumerAPI.Models;
using MongoDB.Driver;

namespace KafkaConsumerAPI.Repository
{
    public class WalletMongoRepository
    {
        private IConfiguration _configuration;
        private IMongoDatabase _db;
        public WalletMongoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            MongoClient _client= new MongoClient(_configuration["MongoUrl"]);
            _db = _client.GetDatabase(_configuration["MongoDatabase"]);
        }

        //crud operation
        //insert
        public void Create(WalletMongoEntity wallet)
        {
            _db.GetCollection<WalletMongoEntity>(_configuration["MongoCollection"])
                .InsertOne(wallet);
        }

        //findall
        public List<WalletMongoEntity> GetCustomers()
        {
            return _db.GetCollection<WalletMongoEntity>(_configuration["MongoCollection"]).Find(_ => true).ToList();
        }
    }
}
