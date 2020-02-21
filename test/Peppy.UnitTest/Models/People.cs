using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Peppy.UnitTest.Models
{
    public class People
    {
        public void Drink()
        {
            Debug.WriteLine($"id：{Id}, Drink");
        }

        public int Id { get; set; }

        public int Age { get; set; }
    }
}