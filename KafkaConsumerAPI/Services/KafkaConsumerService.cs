using Confluent.Kafka;
using KafkaConsumerAPI.Models;
using KafkaConsumerAPI.Repository;

namespace KafkaConsumerAPI.Services
{
    public class KafkaConsumerService : IHostedService
    {
        private IConfiguration _config;
        private ILogger<KafkaConsumerService> _logger;
        private WalletMongoRepository _walletMongoRepository;
        private WalletMongoEntity walletMongoEnity;
        public KafkaConsumerService(IConfiguration configuration, 
            ILogger<KafkaConsumerService> logger,WalletMongoRepository walletMongoRepository)
        {
            this._config = configuration;
            this._logger = logger;  
            this._walletMongoRepository = walletMongoRepository;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = "test_group",
                BootstrapServers = _config["BootStrapServer"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            try
            {
                using (var consumerBuilder = new ConsumerBuilder
                <Ignore, string>(config).Build())
                {
                    consumerBuilder.Subscribe(_config["TopicName"]);
                    var cancelToken = new CancellationTokenSource();

                    try
                    {
                        while (true)
                        {
                            var consumer = consumerBuilder.Consume
                               (cancelToken.Token);
                            var walletRequest = consumer.Message.Value;
                            walletMongoEnity = new WalletMongoEntity();
                            walletMongoEnity.WalletBalance = Convert.ToInt64(walletRequest);
                            this._walletMongoRepository.Create(walletMongoEnity);

                            _logger.LogInformation($"Wallet Amount:{ walletRequest}");


                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumerBuilder.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }


            return Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
