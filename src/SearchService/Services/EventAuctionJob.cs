using AutoMapper;
using Confluent.Kafka;
using Contracts;
using MongoDB.Entities;
using SearchService.Models;
using System.Text.Json;

namespace SearchService.Services
{
    public class EventAuctionJob : BackgroundService
    {
        private readonly ILogger<EventAuctionJob> _logger;
        private readonly IMapper _mapper;

        public EventAuctionJob(ILogger<EventAuctionJob> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("EventAuctionJob is starting.");

            var config = new ConsumerConfig
            {
               BootstrapServers = "localhost:9092",
               GroupId = "test-auction-kafka-event",
               AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe("auction-created-topic");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    var auction = JsonSerializer.Deserialize<AuctionCreated>(consumeResult.Message.Value);

                    if (auction == null)
                    {
                        _logger.LogWarning("Consumed Kafka message but failed to deserialize AuctionCreated.");
                        continue;
                    }

                    var item = _mapper.Map<Item>(auction);

                    // Persist with the same Auction Id from event
                    item.ID = auction.Id.ToString();

                    await item.SaveAsync();

                    _logger.LogInformation("Consumed and saved auction {AuctionId} from Kafka.", auction.Id);
                }
                catch(Exception ex)
                {

                }
            }

        }
    }
}
