﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using Peppy;
using Peppy.Core;
using Peppy.Domain.UnitOfWork;
using Peppy.RabbitMQ;
using Peppy.Redis;
using Sample.WebApi.Repositories;
using Peppy.Extensions;
using Peppy.Mapper;
using System.Transactions;
using Peppy.EntityFrameworkCore.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Sample.WebApi.Controllers
{
    /// <summary>
    /// WeatherForecast
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        //private readonly ICappublicer _capBus;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IBaseRepository<AppDbContext> _baseRepository;
        private readonly IMapper _mapper;
        private readonly IPersonPepository _personPepository;
        private readonly IRedisManager _redisManager;
        private readonly IRabbitMQManager _rabbitMQManager;
        private readonly ClientRegister _clientRegister;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IMediator _mediator;

        /// <summary>
        ///
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="personPepository"></param>
        /// <param name="clientRegister"></param>
        /// <param name="rabbitMQManager"></param>
        /// <param name="mediator"></param>
        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            //ICappublicer cappublicer,
            //IRedisManager redisManager,
            IMapper mapper,
            IUnitOfWorkManager unitOfWorkManager,
            IBaseRepository<AppDbContext> baseRepository,
            IPersonPepository personPepository,
            ClientRegister clientRegister,
            IRabbitMQManager rabbitMQManager,
            IMediator mediator)
        {
            _logger = logger;
            //_capBus = cappublicer;
            //_redisManager = redisManager;
            _unitOfWorkManager = unitOfWorkManager;
            _clientRegister = clientRegister;
            _rabbitMQManager = rabbitMQManager;
            _personPepository = personPepository;
            _mediator = mediator;
            _mapper = mapper;
            _baseRepository = baseRepository;
        }

        private static Stopwatch TimerStart()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            return stopwatch;
        }

        private static string TimerEnd(Stopwatch watch)
        {
            watch.Stop();
            return ((double)watch.ElapsedMilliseconds).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PersonDto> Get()
        {
            var watch = TimerStart();
            //using var uow = _unitOfWorkManager.Begin();
            //var person = await _personPepository.InsertAsync(new Person { Name = Guid.NewGuid().ToString() });
            var person = await _baseRepository.GetRepository<Person>().Query().FirstOrDefaultAsync(x => x.Id == 42);
            //await uow.CompleteAsync();
            Console.WriteLine(TimerEnd(watch));
            return _mapper.Map<Person, PersonDto>(person);


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

            //    _capBus.public("sample.rabbitmq.qq", DateTime.Now);

            //    dbContext.SaveChanges();
            //    trans.Commit();
            //}
            //var person = await _mediator.Send(new Person() { Name = "小明" }, default);
            //var person = new Person() { Name = "小明" };
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet("ShopJob")]
        public async Task<string[]> ShopJob()
        {
            return await Task.FromResult(new string[] { "value1", "value2" });
        }

        [NonAction]
        [RabbitMQReceive("test", "test")]
        private void Test(TestModel msg)
        {
            _logger.LogInformation(msg.ToJson());
        }

        //[CapSubscribe("sample.rabbitmq.mysql")]
        private void Subscriber(DateTime p)
        {
            Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {p}");
        }
    }

    public class TestModel
    {
        public DateTime Date { get; set; }
    }
}