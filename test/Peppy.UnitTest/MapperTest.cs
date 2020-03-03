using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Peppy.Mapper;
using Peppy.UnitTest.Models;

namespace Peppy.UnitTest
{
    [TestClass]
    public class MapperTest
    {
        [TestMethod]
        public void Mapper()
        {
            var peoples = InitPeoples().ToList();

            var watch = Stopwatch.StartNew();
            watch.Restart();

            var result = peoples.Map<People, PeopleDto>();
            var t1 = watch.ElapsedMilliseconds;

            watch.Restart();
            var config = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<People, PeopleDto>());
            var mapper = config.CreateMapper();
            var t2 = watch.ElapsedMilliseconds;

            watch.Restart();
            var autoMapperResult = mapper.Map<List<PeopleDto>>(peoples);
            var t3 = watch.ElapsedMilliseconds;

            watch.Restart();
            var autoMapperResult2 = peoples.Select(x => mapper.Map<PeopleDto>(x)).ToList();
            var t4 = watch.ElapsedMilliseconds;

            Console.WriteLine("MapperResult:" + t1);
            Console.WriteLine("AutoMapperConfig:" + t2);
            Console.WriteLine("AutoMapperResult:" + t3);
            Console.WriteLine("AutoMapperResult2:" + t4);
        }

        private static IEnumerable<People> InitPeoples()
        {
            var peoples = new List<People>();
            for (var i = 0; i < 1_000_000; i++)
            {
                peoples.Add(new People
                {
                    Id = i,
                    Age = i,
                    Name = "陈" + i.ToString(),
                    Gender = (i & 1) == 1 ? "man" : "woman"
                });
            }
            return peoples;
        }
    }
}