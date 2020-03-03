using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Mapper
{
    internal sealed class MapperManager : IMapper
    {
        public TTarget Map<TSource, TTarget>(TSource source) => source.Map<TSource, TTarget>();
    }
}