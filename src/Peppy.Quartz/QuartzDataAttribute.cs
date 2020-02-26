using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Quartz
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QuartzDataAttribute : Attribute
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public Type ValueType { get; set; }

        public QuartzDataAttribute(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            Key = key;
            Value = value;
            ValueType = value.GetType();
        }

        public QuartzDataAttribute(string key, int value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            Key = key;
            Value = value.ToString();
            ValueType = value.GetType();
        }

        public QuartzDataAttribute(string key, long value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            Key = key;
            Value = value.ToString();
            ValueType = value.GetType();
        }

        public QuartzDataAttribute(string key, float value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            Key = key;
            Value = value.ToString();
            ValueType = value.GetType();
        }

        public QuartzDataAttribute(string key, double value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            Key = key;
            Value = value.ToString();
            ValueType = value.GetType();
        }

        public QuartzDataAttribute(string key, decimal value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            Key = key;
            Value = value.ToString();
            ValueType = value.GetType();
        }

        public QuartzDataAttribute(string key, bool value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            Key = key;
            Value = value.ToString();
            ValueType = value.GetType();
        }
    }
}