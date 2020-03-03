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

        public string Name { get; set; }

        public bool IsDrink { get; set; }

        public string Gender { get; set; }

        public bool IsHungry { get; set; }

        public bool IsTired { get; set; }

        public bool IsThirsty { get; set; }
    }
}