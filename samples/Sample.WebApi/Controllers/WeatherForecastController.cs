using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using Peppy;
using Peppy.Core;
using Peppy.RabbitMQ;
using Peppy.Redis;
using Sample.WebApi.Repositories;
using Peppy.Extensions;
using Quartz;
using Sample.WebApi.Jobs;

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

        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;
        private readonly IPersonPepository _personPepository;
        private readonly IRedisManager _redisManager;
        private readonly IRabbitMQManager _rabbitMQManager;
        private readonly ClientRegister _clientRegister;
        private readonly IMediator _mediator;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            //ICapPublisher capPublisher,
            //IRedisManager redisManager,
            ISchedulerFactory schedulerFactory,
            IPersonPepository personPepository,
            ClientRegister clientRegister,
            IRabbitMQManager rabbitMQManager,
            IMediator mediator)
        {
            _logger = logger;
            //_capBus = capPublisher;
            //_redisManager = redisManager;
            _schedulerFactory = schedulerFactory;
            _clientRegister = clientRegister;
            _rabbitMQManager = rabbitMQManager;
            _personPepository = personPepository;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<Person> Get()
        {
            //await _personPepository.BatchDeleteAsync(x => x.Id > 1);
            //await _personPepository.Query().Where(x => x.Name == "test").DeleteFromQueryAsync();
            //var persons = await _personPepository.QueryListAsync();
            //var person = persons.FirstOrDefault();
            //person.Name = "test";
            //await _personPepository.UpdateAsync(person);
            //persons = await _personPepository.QueryListAsync();
            //Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, expr2.Body), expr1.Parameters);
            //_rabbitMQManager.SendMsgAsync("test", "test", new TestModel { Date = DateTime.Now });
            //_redisManager.Add("test", "test");
            //_client.OnConsumerReceived(null, new EventArgs());
            //using (var trans = dbContext.Database.BeginTransaction(_capBus, autoCommit: false))
            //{
            //    dbContext.Persons.Add(new Person() { Name = DateTime.Now.ToString() });

            //    _capBus.Publish("sample.rabbitmq.qq", DateTime.Now);

            //    dbContext.SaveChanges();
            //    trans.Commit();
            //}
            return await _mediator.Send(new Person() { Name = "小明" }, default);
        }

        [HttpGet("CreateJob")]
        public async Task<string[]> CreateJob()
        {
            ////1、通过调度工厂获得调度器
            //_scheduler = await _schedulerFactory.GetScheduler();
            ////2、开启调度器
            //await _scheduler.Start();
            ////3、创建一个触发器
            //var trigger = TriggerBuilder.Create()
            //    .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())//每两秒执行一次
            //    .Build();
            ////4、创建任务
            //var jobDetail = JobBuilder.Create<MyJob>()
            //    .WithIdentity("job", "group")
            //    .Build();
            ////5、将触发器和任务器绑定到调度器中
            //await _scheduler.ScheduleJob(jobDetail, trigger);
            return await Task.FromResult(new string[] { "value1", "value2" });
        }

        [HttpGet("ShopJob")]
        public async Task<string[]> ShopJob()
        {
            _scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = JobKey.Create("job");
            await _scheduler.Shutdown(true);
            return await Task.FromResult(new string[] { "value1", "value2" });
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