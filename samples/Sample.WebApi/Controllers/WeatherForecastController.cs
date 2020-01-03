using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Peppy;
using Peppy.RabbitMQ;
using Peppy.Redis;

namespace Sample.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IConsumerClient _client;
        private readonly ICapPublisher _capBus;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRedisManager _redisManager;
        private readonly IRabbitMQManager _rabbitMQManager;
        private readonly ClientRegister _clientRegister;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            ICapPublisher capPublisher,
            //IRedisManager redisManager,
            IConsumerClient client,
            ClientRegister clientRegister,
            IRabbitMQManager rabbitMQManager)
        {
            _logger = logger;
            _capBus = capPublisher;
            //_redisManager = redisManager;
            _client = client;
            _clientRegister = clientRegister;
            _rabbitMQManager = rabbitMQManager;
            RegisterMessageProcessor(_client);
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get([FromServices]AppDbContext dbContext)
        {
            _rabbitMQManager.SendMsgAsync("test", "test", $"test{DateTime.Now.ToString()}");
            //_redisManager.Add("test", "test");
            //_client.OnConsumerReceived(null, new EventArgs());
            //using (var trans = dbContext.Database.BeginTransaction(_capBus, autoCommit: false))
            //{
            //    dbContext.Persons.Add(new Person() { Name = DateTime.Now.ToString() });

            //    _capBus.Publish("sample.rabbitmq.qq", DateTime.Now);

            //    dbContext.SaveChanges();
            //    trans.Commit();
            //}
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [NonAction]
        [RabbitMQReceive("test", "test")]
        public void Test(string msg)
        {
            _logger.LogInformation(msg);
        }

        [CapSubscribe("sample.rabbitmq.mysql")]
        public void Subscriber(DateTime p)
        {
            Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {p}");
        }

        private void RegisterMessageProcessor(IConsumerClient client)
        {
            client.OnMessageReceived += (sender, transportMessage) =>
            {
                _logger.LogInformation("test");
            };
        }
    }

    public class RabbitMQConsumerClient : IConsumerClient
    {
        public event EventHandler<TransportMessage> OnMessageReceived;

        public void OnConsumerReceived(object sender, EventArgs e)
        {
            var headers = new Dictionary<string, string>();
            var message = new TransportMessage(headers, "");
            OnMessageReceived?.Invoke(sender, message);
        }
    }

    public interface IConsumerClient
    {
        event EventHandler<TransportMessage> OnMessageReceived;

        void OnConsumerReceived(object sender, EventArgs e);
    }

    public class TransportMessage
    {
        public TransportMessage(IDictionary<string, string> headers, string body)
        {
            Headers = headers ?? throw new ArgumentNullException(nameof(headers));
            Body = body;
        }

        /// <summary>
        /// Gets the headers of this message
        /// </summary>
        public IDictionary<string, string> Headers { get; }

        /// <summary>
        /// Gets the body object of this message
        /// </summary>
        public string Body { get; }
    }
}