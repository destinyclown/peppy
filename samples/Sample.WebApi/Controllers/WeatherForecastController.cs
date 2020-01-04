using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Peppy;
using Peppy.Core;
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

        //private readonly ICapPublisher _capBus;
        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IRedisManager _redisManager;
        private readonly IRabbitMQManager _rabbitMQManager;
        private readonly ClientRegister _clientRegister;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            //ICapPublisher capPublisher,
            //IRedisManager redisManager,
            ClientRegister clientRegister,
            IRabbitMQManager rabbitMQManager)
        {
            _logger = logger;
            //_capBus = capPublisher;
            //_redisManager = redisManager;
            _clientRegister = clientRegister;
            _rabbitMQManager = rabbitMQManager;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get([FromServices]AppDbContext dbContext)
        {
            _rabbitMQManager.SendMsgAsync("test", "test", new TestModel { Date = DateTime.Now });
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
        public void Test(TestModel msg)
        {
            _logger.LogInformation(msg.ToJson());
        }

        //[CapSubscribe("sample.rabbitmq.mysql")]
        public void Subscriber(DateTime p)
        {
            Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {p}");
        }
    }

    public class TestModel
    {
        public DateTime Date { get; set; }
    }
}