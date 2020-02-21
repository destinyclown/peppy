using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Peppy.Extensions;
using Peppy.UnitTest.Models;

namespace Peppy.UnitTest
{
    [TestClass]
    public class ExpressionExtensionsTest
    {
        [TestMethod]
        public void AndAlso()
        {
            var peoples = InitPeoples();

            Expression<Func<People, bool>> lambda1 = x => x.Age > 3;
            Expression<Func<People, bool>> lambda2 = x => x.Id < 7;
            Expression<Func<People, bool>> lambda3 = x => x.Age > 3 || x.Id < 7;
            Expression<Func<People, bool>> lambda4 = x => x.Age > 3 && x.Id < 7;

            var lambda6 = lambda1.AndAlso(lambda2);
            var compile = lambda6.Compile();
            peoples.FirstOrDefault()
                .If(x => true, x => x.Drink())
                .If(x => true, x => x.Drink())
                .If(x => true, x => x.Drink());
            //peoples = peoples.Where(compile);
            Debug.WriteLine(lambda6.ToString());
            Assert.AreEqual(lambda6.ToString(), lambda4.ToString());
        }

        private static IEnumerable<People> InitPeoples()
        {
            var peoples = new List<People>();
            for (var i = 0; i < 10; i++)
            {
                peoples.Add(new People
                {
                    Id = i,
                    Age = i
                });
            }
            return peoples;
        }
    }
}