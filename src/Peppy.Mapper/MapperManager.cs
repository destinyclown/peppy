using System;
using System.Collections.Generic;
using System.Text;

namespace Peppy.Mapper
{
    public class MapperManager : IMapper
    {
        public TTarget Map<TSource, TTarget>(TSource source) => source.Map<TSource, TTarget>();
    }
}