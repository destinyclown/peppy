using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.AutoIoc
{
    public enum LifeCycle
    {
        Scoped = 0x1,
        Singleton = 0x2,
        Transient = 0x3,
    }
}